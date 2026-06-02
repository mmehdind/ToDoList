using TodoList.Models;
using TodoList.Enums;

namespace TodoList.ViewModels;

public class TaskIndexViewModel
{
    public string? SearchTerm { get; set; }

    public int? CategoryId { get; set; }

    public MyTaskStatus? Status { get; set; }

    public List<TaskViewModel> Tasks { get; set; } = [];

    public List<Category> Categories { get; set; } = [];
}