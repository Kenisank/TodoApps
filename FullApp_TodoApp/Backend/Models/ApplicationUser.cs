using Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
    public class ApplicationUser : IdentityUser
    {

        public ICollection<TodoItem> Todos { get; set; } = new List<TodoItem>();
    }
}