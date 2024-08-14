
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.Resources
{
    public class TodoItemGetDto
    {
        public int Id { get; set; }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public string CreatedDate { get; set; } 

        public string ModifiedDate { get; set; } 
     
       
    }
}
