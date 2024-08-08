using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using App_Core.Models;
using Moq;

public static class ControllerTestHelpers
{
    // Helper method to mock User in controller
    public static void MockUser(this Controller controller, ApplicationUser user)
    {
        var claims = new[] { new Claim(ClaimTypes.Name, user.Id) };
        var identity = new ClaimsIdentity(claims, "mock");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
    }

    // Optional: Create a mock UserManager<ApplicationUser> if needed
    public static Mock<UserManager<ApplicationUser>> CreateMockUserManager()
    {
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        return new Mock<UserManager<ApplicationUser>>(
            userStore.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);
    }
}
