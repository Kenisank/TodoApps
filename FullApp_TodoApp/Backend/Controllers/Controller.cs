using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Backend.Middlewares;
using Microsoft.EntityFrameworkCore;
using Backend.Resources;
using Backend.Models;
using AutoMapper;

namespace Backend.Controllers
{
    public static class Controller
    {
        /// <summary>
        /// Registers all API endpoints, including authentication and todo management.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        public static void RegisterAuthEndpoints(WebApplication app)
        {
            #region Auth Endpoints

            // Endpoint for user registration
            app.MapPost("/api/auth/register", async ([FromServices] UserManager<ApplicationUser> userManager, [FromBody] RegisterModel model) =>
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Results.Ok("User registered successfully.");
                }
                else
                {
                    return Results.BadRequest(result.Errors.Select(e => e.Description));
                }
            });

            // Endpoint for user login
            app.MapPost("/api/auth/login", async ([FromServices] SignInManager<ApplicationUser> signInManager,
                                                  [FromServices] UserManager<ApplicationUser> userManager,
                                                  [FromServices] JwtTokenService jwtTokenService,
                                                  [FromBody] LoginModel model) =>
            {
                var user = await userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    return Results.BadRequest("Invalid username or password.");
                }

                var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded)
                {
                    return Results.BadRequest("Invalid username or password.");
                }

                var token = jwtTokenService.GenerateToken(user);

                return Results.Ok(new { Token = token });
            });

            #endregion

            #region Todo Endpoints

            // Endpoint for fetching todos for the authenticated user
            app.MapGet("/api/todos", async (ClaimsPrincipal user, [FromServices] TodoDbContext dbContext, [FromServices] IMapper mapper) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var todos = await dbContext.TodoItems.Where(t => t.UserId == userId).ToListAsync();
                return Results.Ok(mapper.Map<List<TodoItemGetDto>>(todos));
            }).RequireAuthorization();

            // Endpoint for adding a new todo item
            app.MapPost("/api/todos", async (ClaimsPrincipal user, [FromBody] TodoItemPostPutDto todoDto, [FromServices] TodoDbContext dbContext, [FromServices] IMapper mapper) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var todo = mapper.Map<TodoItem>(todoDto);
                todo.UserId = userId;
                dbContext.TodoItems.Add(todo);
                await dbContext.SaveChangesAsync();
                return Results.Ok(todo);
            }).RequireAuthorization();

            // Endpoint for updating an existing todo item
            app.MapPut("/api/todos/{id}", async (int id, ClaimsPrincipal user, [FromBody] TodoItemPostPutDto updatedTodoDto, [FromServices] TodoDbContext dbContext, [FromServices] IMapper mapper) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var updatedTodo = mapper.Map<TodoItem>(updatedTodoDto);
                var todo = await dbContext.TodoItems.FindAsync(id);
                if (todo == null || todo.UserId != userId) return Results.NotFound();

                todo.Title = updatedTodo.Title;
                todo.IsCompleted = updatedTodo.IsCompleted;
                await dbContext.SaveChangesAsync();

                return Results.Ok(todo);
            }).RequireAuthorization();

            // Endpoint for deleting a todo item
            app.MapDelete("/api/todos/{id}", async (int id, ClaimsPrincipal user, [FromServices] TodoDbContext dbContext) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var todo = await dbContext.TodoItems.FindAsync(id);
                if (todo == null || todo.UserId != userId) return Results.NotFound();

                dbContext.TodoItems.Remove(todo);
                await dbContext.SaveChangesAsync();
                return Results.Ok("Todo deleted successfully.");
            }).RequireAuthorization();

            #endregion
        }
    }
}
