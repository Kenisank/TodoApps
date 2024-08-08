
using Moq;
using Xunit;
using App_Core.Controllers;
using App_Core.Models;
using App_Core.Dal.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public class TodoControllerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly TodoController _controller;

    public TodoControllerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            userStore.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        _controller = new TodoController(_mockUnitOfWork.Object, _mockUserManager.Object);
    }

    private void SetUpHttpContextUser(ApplicationUser user)
    {
        var claims = new[] { new Claim(ClaimTypes.Name, user.Id) };
        var identity = new ClaimsIdentity(claims, "mock");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
    }


    [Fact]
    public void Constructor_ShouldNotThrowException_WhenDependenciesAreNotNull()
    {
        // Arrange & Act
        var exception = Record.Exception(() => new TodoController(_mockUnitOfWork.Object, _mockUserManager.Object));

        // Assert
        Assert.Null(exception);
    }

}




////        public TodoControllerTests()

////        //[Fact]
////        //public async Task Index_ReturnsViewWithTodoItems()
////        //{
////        //    // Arrange
////        //    var todoItems = new List<TodoItem>
////        //    {
////        //        new TodoItem { Title = "Test Todo 1", Description = "Description 1", IsCompleted = false },
////        //        new TodoItem { Title = "Test Todo 2", Description = "Description 2", IsCompleted = true }
////        //    };

////        //    _mockRepo.Setup(r => r.TodoLists.FindByCondition(It.IsAny<Expression<Func<TodoItem, bool>>>()))
////        //            .Returns(todoItems.AsQueryable());

////        //    // Act
////        //    var result = await _controller.Index();

////        //    // Assert
////        //    var viewResult = Assert.IsType<ViewResult>(result);
////        //    var model = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(viewResult.ViewData.Model);
////        //    Assert.Equal(2, model.Count());
////        //}

////        [Fact]
////        public async Task Create_Post_ValidTodoItem_RedirectsToIndex()
////        {
////            // Arrange
////            var newTodoItem = new TodoItem
////            {
////                Title = "New Test Todo",
////                Description = "Test Description",
////                IsCompleted = false
////            };

////            // Act
////            var result = await _controller.Create(newTodoItem);

////            // Assert
////            _mockRepo.Verify(r => r.TodoLists.AddAsync(It.Is<TodoItem>(t => t.Title == newTodoItem.Title)), Times.Once);
////            _mockRepo.Verify(r => r.Audits.AddAsync(It.IsAny<Audit>()), Times.Once);
////            _mockRepo.Verify(r => r.SaveAsync(), Times.Once);

////            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
////            Assert.Equal("Index", redirectToActionResult.ActionName);
////        }
////    }
////}










































////using System.Collections.Generic;
////using System.Threading.Tasks;
////using App_Core.Controllers;
////using App_Core.Dal.Repostories.Interfaces;
////using App_Core.Dal.UnitOfWork;
////using App_Core.Data;
////using App_Core.Models;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.EntityFrameworkCore;
////using Moq;
////using Xunit;

////namespace TodoApp.Tests
////{
////    public class TodoControllerTests
////    {
////        private readonly TodoController _controller;
////        private readonly Mock<IUnitOfWork> _mockRepo;

////        public TodoControllerTests()
////        {
////            _mockRepo = new Mock<IUnitOfWork>();
////            _controller = new TodoController(_mockRepo.Object);
////        }

////        [Fact]
////        public async Task Index_ReturnsAViewResult_WithAListOfTodoItems()
////        {
////            // Arrange
////            _mockRepo.Setup(repo => repo.TodoLists.GetAllAsync()).ReturnsAsync(GetTestTodoItems());

////            // Act
////            var result = await _controller.Index();

////            // Assert
////            var viewResult = Assert.IsType<ViewResult>(result);
////            var model = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(viewResult.Model);
////            Assert.Equal(2, ((List<TodoItem>)model).Count);
////        }

////        private IEnumerable<TodoItem> GetTestTodoItems()
////        {
////            return new List<TodoItem>
////            {
////                new TodoItem { Id = 1, Title = "Test Todo 1", Description = "Test Description 1", IsCompleted = false },
////                new TodoItem { Id = 2, Title = "Test Todo 2", Description = "Test Description 2", IsCompleted = true }
////            };
////        }
////    }
////}

























//using Moq;
//using App_Core.Controllers;// Adjust based on your namespaces
//using App_Core.Models;
//using Microsoft.AspNetCore.Identity;
//using App_Core.Dal.UnitOfWork;
//using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;

//public class TodoControllerTests
//{
//    private readonly TodoController _controller;
//    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
//    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;

//    public TodoControllerTests()
//    {
//        _mockUnitOfWork = new Mock<IUnitOfWork>();

//        //var userStore = new Mock<IUserStore<ApplicationUser>>();
//        //_mockUserManager = new Mock<UserManager<ApplicationUser>>(
//        //    userStore.Object, null, null, null, null, null, null, null, null);

//        //_controller = new TodoController(_mockUnitOfWork.Object, _mockUserManager.Object);

//        // Initialize controller
//        _controller = new TodoController(_mockUnitOfWork.Object, ControllerTestHelpers.CreateMockUserManager().Object);

//    }

//    [Fact]
//    public void Constructor_ShouldNotThrowException_WhenDependenciesAreNotNull()
//    {
//        // Arrange & Act
//        var exception = Record.Exception(() => new TodoController(_mockUnitOfWork.Object, _mockUserManager.Object));

//        // Assert
//        Assert.Null(exception);
//    }

//    [Fact]
//    public async Task Create_InvalidModel_ReturnsViewWithModel()
//    {
//        // Arrange
//        _controller.ModelState.AddModelError("Title", "Required");
//        var toDoItem = new TodoItem();

//        // Act
//        var result = await _controller.Create(toDoItem) as ViewResult;

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(toDoItem, result.Model);
//    }


//    [Fact]
//    public async Task Create_ValidModel_RedirectsToIndex()
//    {
//        // Arrange
//        var toDoItem = new TodoItem
//        {
//            Title = "Test Todo",
//            Description = "Test Description",
//            IsCompleted = false
//        };

//        var user = new ApplicationUser { Id = "user-id" };
//        _controller.MockUser(user); // Use the helper method to mock the current user

//        // Debugging to check if user is set
//        var currentUser = _controller.User.Identity.Name;
//        Debug.Assert(currentUser == user.Id, "User ID does not match");

//        // Act
//        var result = await _controller.Create(toDoItem) as RedirectToActionResult;

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal("Index", result.ActionName);
//        _mockUnitOfWork.Verify(uow => uow.TodoLists.AddAsync(It.IsAny<TodoItem>()), Times.Once);
//        _mockUnitOfWork.Verify(uow => uow.Audits.AddAsync(It.IsAny<Audit>()), Times.Once);
//        _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
//    }
//}



