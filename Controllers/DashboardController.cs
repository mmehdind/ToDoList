using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TodoList.Models;
using TodoList.ViewModels;
using TodoList.Data;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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
        

        // public IActionResult Index()
        // {
        //     return View();
        // }

        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUser();


            var fullName = string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName) ? user.UserName : $"{user.FirstName} {user.LastName}";

            // var tasks = new List<TaskItem> 
            // {
            //      new TaskItem 
            //     {
            //          Title = "تکمیل پروژه MVC",
            //           Description = "اتصال کامل داشبورد",
            //            IsCompleted = false 
            //     }, 

            //     new TaskItem 
            //     { 
            //         Title = "ساخت فرم افزودن تسک",
            //          Description = "طراحی صفحه Create",
            //           IsCompleted = true 
            //     } 
            // }; 

            var tasks = await _context.TaskItem
            .Where(x => x.UserId == user.Id)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

            var model = new DashboardViewModel
            { 
                FullName = fullName,
                TotalTasks = tasks.Count,
                CompletedTasks = tasks.Count(x => x.IsCompleted),
                PendingTasks = tasks.Count(x => !x.IsCompleted),
                RecentTasks = tasks 
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

