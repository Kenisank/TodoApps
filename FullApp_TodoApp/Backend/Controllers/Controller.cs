using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Backend.Middlewares;
using Microsoft.EntityFrameworkCore;
using Backend.Resources;
using Backend.Models;
using AutoMapper;
using Serilog;
using Backend.Services;

namespace Backend.Controllers
{
    /// <summary>
    /// Contains methods for registering API endpoints, including authentication and todo management.
    /// </summary>
    public static class Controller
    {
        public static void RegisterAuthEndpoints(WebApplication app)
        {
            #region Auth Endpoints

            /// <summary>
            /// Endpoint for user registration.
            /// </summary>
            /// <param name="logger">The logger for logging operations.</param>
            /// <param name="model">The registration model containing user details.</param>
            /// <returns>A response indicating the success or failure of user registration.</returns>
            app.MapPost("/api/auth/register", async ([FromServices] UserManager<ApplicationUser> userManager,
                                                     [FromServices] ILogger<Program> logger,
                                                     [FromServices] AuditService auditService,
                                                     [FromBody] RegisterModel model) =>
            {
                logger.LogInformation("User registration attempt for {Username}", model.Username);

                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    logger.LogInformation("User {Username} registered successfully.", model.Username);
                    await auditService.LogActionAsync("User Registration", model.Username, "User registered successfully.");
                    return Results.Ok("User registered successfully.");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    logger.LogWarning("User registration failed for {Username}. Errors: {Errors}", model.Username, errors);
                    await auditService.LogActionAsync("User Registration Failed", model.Username, $"Errors: {errors}");
                    return Results.BadRequest(result.Errors.Select(e => e.Description));
                }
            });

            /// <summary>
            /// Endpoint for user login.
            /// </summary>
            /// <param name="logger">The logger for logging operations.</param>
            /// <param name="model">The login model containing user credentials.</param>
            /// <returns>A response containing the JWT token if login is successful, or an error message.</returns>
            app.MapPost("/api/auth/login", async ([FromServices] SignInManager<ApplicationUser> signInManager,
                                                  [FromServices] UserManager<ApplicationUser> userManager,
                                                  [FromServices] JwtTokenService jwtTokenService,
                                                  [FromServices] ILogger<Program> logger,
                                                  [FromServices] AuditService auditService,
                                                  [FromBody] LoginModel model) =>
            {
                logger.LogInformation("User login attempt for {Username}", model.Username);

                var user = await userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    logger.LogWarning("User login failed for {Username}: Invalid username.", model.Username);
                    await auditService.LogActionAsync("User Login Failed", model.Username, "Invalid username.");
                    return Results.BadRequest("Invalid username or password.");
                }

                var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded)
                {
                    logger.LogWarning("User login failed for {Username}: Invalid password.", model.Username);
                    await auditService.LogActionAsync("User Login Failed", model.Username, "Invalid password.");
                    return Results.BadRequest("Invalid username or password.");
                }

                var token = jwtTokenService.GenerateToken(user);
                logger.LogInformation("User {Username} logged in successfully.", model.Username);
                await auditService.LogActionAsync("User Login", model.Username, "User logged in successfully.");

                return Results.Ok(new { Token = token });
            });

            #endregion

            #region Todo Endpoints

            /// <summary>
            /// Endpoint for fetching todos for the authenticated user.
            /// </summary>
            /// <param name="logger">The logger for logging operations.</param>
            /// <returns>A response with the list of todos for the authenticated user.</returns>
            app.MapGet("/api/todos", async (ClaimsPrincipal user,
                                            [FromServices] TodoDbContext dbContext,
                                            [FromServices] IMapper mapper,
                                            [FromServices] ILogger<Program> logger,
                                            [FromServices] AuditService auditService) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.Name);
                logger.LogInformation("Fetching todos for user {UserId}", userId);

                var todos = await dbContext.TodoItems.Where(t => t.UserId == userId).ToListAsync();
                await auditService.LogActionAsync("Fetch Todos", userId, "Fetched todos for the user.");
                return Results.Ok(mapper.Map<List<TodoItemGetDto>>(todos));
            }).RequireAuthorization();

            /// <summary>
            /// Endpoint for adding a new todo item.
            /// </summary>
            /// <param name="logger">The logger for logging operations.</param>
            /// <returns>A response with the newly added todo item.</returns>
            app.MapPost("/api/todos", async (ClaimsPrincipal user,
                                             [FromBody] TodoItemPostPutDto todoDto,
                                             [FromServices] TodoDbContext dbContext,
                                             [FromServices] IMapper mapper,
                                             [FromServices] ILogger<Program> logger,
                                             [FromServices] AuditService auditService) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.Name);
                logger.LogInformation("User {UserId} adding a new todo item.", userId);

                var todo = mapper.Map<TodoItem>(todoDto);
                todo.UserId = userId;
                dbContext.TodoItems.Add(todo);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("User {UserId} successfully added a todo item.", userId);
                await auditService.LogActionAsync("Add Todo", userId, $"Added todo item: {todo.Title}");
                return Results.Ok(todo);
            }).RequireAuthorization();

            /// <summary>
            /// Endpoint for updating an existing todo item.
            /// </summary>
            /// <param name="updatedTodoDto">The DTO containing the updated details of the todo item.</param>
            /// <param name="logger">The logger for logging operations.</param>
            /// <returns>A response with the updated todo item or a not found status.</returns>
            app.MapPut("/api/todos/{id}", async (int id,
                                                 ClaimsPrincipal user,
                                                 [FromBody] TodoItemPostPutDto updatedTodoDto,
                                                 [FromServices] TodoDbContext dbContext,
                                                 [FromServices] IMapper mapper,
                                                 [FromServices] ILogger<Program> logger,
                                                 [FromServices] AuditService auditService) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.Name);
                logger.LogInformation("User {UserId} updating todo item with id {TodoId}.", userId, id);

                var updatedTodo = mapper.Map<TodoItem>(updatedTodoDto);
                var todo = await dbContext.TodoItems.FindAsync(id);
                if (todo == null || todo.UserId != userId)
                {
                    logger.LogWarning("User {UserId} failed to update todo item with id {TodoId}: Not found.", userId, id);
                    await auditService.LogActionAsync("Update Todo Failed", userId, $"Todo item not found: {id}");
                    return Results.NotFound();
                }

                todo.Title = updatedTodo.Title;
                todo.IsCompleted = updatedTodo.IsCompleted;
                await dbContext.SaveChangesAsync();

                logger.LogInformation("User {UserId} successfully updated todo item with id {TodoId}.", userId, id);
                await auditService.LogActionAsync("Update Todo", userId, $"Updated todo item: {todo.Title}");
                return Results.Ok(todo);
            }).RequireAuthorization();



            /// <summary>
            /// Endpoint for updating an existing todo item.
            /// </summary>
            /// <param name="logger">The logger for logging operations.</param>
            /// <returns>A response with the updated todo item or a not found status.</returns>
            app.MapPatch("/api/todos/{id}", async (int id,
                                                 ClaimsPrincipal user,
                                                 [FromServices] TodoDbContext dbContext,
                                                 [FromServices] IMapper mapper,
                                                 [FromServices] ILogger<Program> logger,
                                                 [FromServices] AuditService auditService) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.Name);
                logger.LogInformation("User {UserId} updating isComplete Status item with id {TodoId}.", userId, id);

                
                var todo = await dbContext.TodoItems.FindAsync(id);
                if (todo == null || todo.UserId != userId)
                {
                    logger.LogWarning("User {UserId} failed to update todo item with id {TodoId}: Not found.", userId, id);
                    await auditService.LogActionAsync("Update Todo Failed", userId, $"Todo item not found: {id}");
                    return Results.NotFound();
                }

                
                todo.IsCompleted = (!todo.IsCompleted);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("User {UserId} successfully updated isComplete status of the todo item with id {TodoId}.", userId, id);
                await auditService.LogActionAsync("Update Todo", userId, $"Updated todo item: {todo.Title}");
                return Results.Ok(todo);
            }).RequireAuthorization();


            /// <summary>
            /// Endpoint for deleting a todo item.
            /// </summary>
            /// <param name="logger">The logger for logging operations.</param>
            /// <returns>A response indicating the success or failure of the delete operation.</returns>
            app.MapDelete("/api/todos/{id}", async (int id,
                                                    ClaimsPrincipal user,
                                                    [FromServices] TodoDbContext dbContext,
                                                    [FromServices] ILogger<Program> logger,
                                                    [FromServices] AuditService auditService) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.Name);
                logger.LogInformation("User {UserId} deleting todo item with id {TodoId}.", userId, id);

                var todo = await dbContext.TodoItems.FindAsync(id);
                if (todo == null || todo.UserId != userId)
                {
                    logger.LogWarning("User {UserId} failed to delete todo item with id {TodoId}: Not found.", userId, id);
                    await auditService.LogActionAsync("Delete Todo Failed", userId, $"Todo item not found: {id}");
                    return Results.NotFound();
                }

                dbContext.TodoItems.Remove(todo);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("User {UserId} successfully deleted todo item with id {TodoId}.", userId, id);
                await auditService.LogActionAsync("Delete Todo", userId, $"Deleted todo item: {todo.Title}");
                return Results.Ok("Todo deleted successfully.");
            }).RequireAuthorization();

            #endregion
        }
    }
}
