using App_Core.Dal.UnitOfWork;
using App_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App_Core.Controllers
{

    [Authorize]
    public class TodoController : Controller
    {

        //private readonly IMapper _mapper;

        //private readonly UserManager<ApplicationUser> _userManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;



        public TodoController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {

            //  _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;


        }



        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var items = await _unitOfWork.TodoLists.FindByCondition(t => t.UserId == user.Id).ToListAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,IsCompleted")] TodoItem toDoItem)
        {
            var user = await GetCurrentUserAsync();
            if (ModelState.IsValid)
            {
                toDoItem.UserId = user.Id;
                await _unitOfWork.TodoLists.AddAsync(toDoItem);


                var audit = new Audit
                {
                    Action = "Create",
                    Timestamp = DateTime.Now,
                    User = User.Identity.Name,
                    Details = $"Created ToDoItem: {toDoItem.Title}"
                };
                await _unitOfWork.Audits.AddAsync(audit);

                _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDoItem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await GetCurrentUserAsync();
            var toDoItem = await _unitOfWork.TodoLists.
                FindByCondition(t => t.Id == id && t.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (toDoItem == null)
            {
                return NotFound();
            }
            return View(toDoItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted,CreatedDate,ModifiedDate,UserId")] TodoItem toDoItem)
        {
            if (id != toDoItem.Id)
            {
                return NotFound();
            }

            var user = await GetCurrentUserAsync();
            if (user.Id != toDoItem.UserId)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    toDoItem.ModifiedDate = DateTime.Now;
                    await _unitOfWork.TodoLists.UpdateAsync(toDoItem);

                    var audit = new Audit
                    {
                        Action = "Edit",
                        Timestamp = DateTime.Now,
                        User = User.Identity.Name,
                        Details = $"Edited ToDoItem: {toDoItem.Title}"
                    };
                    await _unitOfWork.Audits.AddAsync(audit);

                    _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoItemExists(toDoItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(toDoItem);
        }

        private bool TodoItemExists(int id)
        {
            return (_unitOfWork.TodoLists.FindByCondition(e => e.Id == id).Count() > 0);
        }
        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

    }
}
