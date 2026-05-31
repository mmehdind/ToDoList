using TodoList.Models;

namespace TodoList.ViewModels
{
    public class DashboardViewModel
    {
        public string FullName { get; set; }

        public int TotalTasks { get; set; }

        public int CompletedTasks { get; set; }

        public int PendingTasks { get; set; }

        public List<TaskItem> RecentTasks { get; set; }
    }
}

