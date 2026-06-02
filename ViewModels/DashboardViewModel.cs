namespace TodoList.ViewModels
{
    public class DashboardViewModel
    {
        public string FullName { get; set; }

        public int TotalTasks { get; set; }

        public int CompletedTasks { get; set; }

        public int InProgressTasks { get; set; }

        public int TodayTasks { get; set; }

        public string? SearchTerm { get; set; }

        public int SelectedCategoryId { get; set; }

        public List<TaskViewModel> Tasks { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}

