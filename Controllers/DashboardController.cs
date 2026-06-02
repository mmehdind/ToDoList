using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TodoList.Models;
using TodoList.ViewModels;
using TodoList.Data;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.EntityFrameworkCore;
using TodoList.Enums;
using TodoList.StaticData;

namespace TodoList.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;

        public DashboardController (ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<ApplicationUser> GetCurrentUser() 
        { 
            return await _userManager.GetUserAsync(User); 
        }
        

        public async Task<IActionResult> Index()
{
            var user = await GetCurrentUser();

            var tasks = await _context.TaskItem
                .Include(x => x.Category)
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            var model = new DashboardViewModel
            {
                FullName = string.IsNullOrWhiteSpace(user.FirstName)
                    ? user.UserName
                    : $"{user.FirstName} {user.LastName}",

                TotalTasks = tasks.Count,
                CompletedTasks = tasks.Count(x => x.Status == MyTaskStatus.Completed),
                InProgressTasks = tasks.Count(x => x.Status == MyTaskStatus.InProgress),
                TodayTasks = tasks.Count(x => x.DueDate.Date == DateTime.Today),

                Tasks = tasks.Select(task => new TaskViewModel
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.DueDate,

                    Status = task.Status,

                    CategoryName = task.Category?.Name ?? "بدون دسته‌بندی",

                    CategoryColor =
                        task.Category == null
                            ? "#64748B"
                            : TodoList.StaticData.CategoryColors.Colors
                                .First(x => x.Id == task.Category.ColorIndex)
                                .HexColor
                }).ToList(),

                Categories = new List<CategoryViewModel>() // ❗ مهم: Sidebar دیگر استفاده نمی‌کند
            };

            return View(model);
}

        

        [HttpPost]
        public async Task<IActionResult> CreateTask (CreateTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await GetCurrentUser();

            var task = new TaskItem
            {
                Title = model.Title,
                Description = model.Description,

                IsCompleted = false,
                CreatedAt = DateTime.Now,
                Status = MyTaskStatus.InProgress,

                DueDate = model.DueDate,
                CategoryId = model.CategoryId,

                UserId = user.Id
            };

            _context.TaskItem.Add(task);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> CompleteTask(int id)
        {
            
            var user = await GetCurrentUser();

            var task = await _context.TaskItem.FindAsync(id);

            if (task == null || task.UserId != user.Id)
            {

                return RedirectToAction(nameof(Index));
            }

            task.IsCompleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> DeleteTask(int id)
        {

            Console.WriteLine($"Task Id = {id}");

            var user = await GetCurrentUser();

            var task = await _context.TaskItem.FindAsync(id);

            if (task == null || task.UserId != user.Id)
            {
                return RedirectToAction(nameof(Index));
            }

            _context.TaskItem.Remove(task);

            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));

        }

    }
}

