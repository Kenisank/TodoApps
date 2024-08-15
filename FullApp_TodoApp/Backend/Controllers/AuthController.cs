using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Models;
using TodoApp.Api.Services;

namespace TodoApp.Api.Controllers
{
    public class AuthController
    {
        public static void RegisterAuthEndpoints(WebApplication app)
        {
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
        }
    }
}
