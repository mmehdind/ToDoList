using TodoList.Enums;
namespace TodoList.ViewModels;

public class TaskViewModel
{
    public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public int CategoryId { get; set; }

        public MyTaskStatus Status { get; set; }

        public string CategoryName { get; set; }

        public string CategoryColor { get; set; }
}
