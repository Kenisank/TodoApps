using App_Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace App_Core.Dtos
{
    public class TodoItemPostPutDto
    {
       

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }
       
       
        
    }
}
