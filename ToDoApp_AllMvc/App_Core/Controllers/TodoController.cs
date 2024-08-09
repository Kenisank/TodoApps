using App_Core.Dal.UnitOfWork;
using App_Core.Dtos;
using App_Core.Models;
using App_Core.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App_Core.Controllers
{

    [Authorize]
    public class TodoController : Controller
    {

        private readonly IMapper _mapper;

        //private readonly UserManager<ApplicationUser> _userManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;



        public TodoController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {

            _mapper = mapper;
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
        public async Task<IActionResult> Create([Bind("Title,Description,IsCompleted")] TodoItemPostPutDto toDoItemDto)
        {
            var user = await GetCurrentUserAsync();
            if (ModelState.IsValid)
            {

                var toDoItem = _mapper.Map<TodoItem>(toDoItemDto);


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
            return View(toDoItemDto);
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
        public async Task<IActionResult> Edit(TodoItem todoItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   await _unitOfWork.TodoLists.UpdateAsync(todoItem);
                     _unitOfWork.SaveAsync();

                    if (Request.IsAjaxRequest()) // Use the extension method here
                    {
                        return Json(new { success = true });
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if ((await _unitOfWork.TodoLists.GetByIdAsync(todoItem.Id)) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if (Request.IsAjaxRequest())
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                );
                return Json(new { success = false, errors });
            }

            return View(todoItem);
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
