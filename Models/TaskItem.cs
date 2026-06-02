using TodoList.Enums;
namespace TodoList.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime DueDate { get; set; }

        public MyTaskStatus Status { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; } 

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
