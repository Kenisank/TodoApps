namespace Backend.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}