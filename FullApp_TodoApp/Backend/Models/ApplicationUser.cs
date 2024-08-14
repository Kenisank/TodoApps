using Backend.Models;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser: IdentityUser
{
    
    public ICollection<TodoItem> Todos { get; set; } = new List<TodoItem>();
}
