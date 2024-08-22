
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.Resources
{
    public class TodoItemPostPutDto
    {
       

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
       
       
        
    }
}
