using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models;
using TodoList.StaticData;
using TodoList.ViewModels;
using TodoList.Enums;

namespace TodoList.Controllers;

[Authorize]
public class TaskController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public TaskController (UserManager<ApplicationUser> UserManager, ApplicationDbContext Context)
    {
        _userManager = UserManager;
        _context = Context;
    }

    private async Task<ApplicationUser> GetCurrentUser() 
        { 
            return await _userManager.GetUserAsync(User); 
        }

    private async Task<TaskItem?> GetTask (int itemId, string userId)
    {
        return await _context.TaskItem.FirstOrDefaultAsync(X => X.Id == itemId && X.UserId == userId);;
    }

    public async Task<IActionResult> Index (
    string? searchTerm,
    int? categoryId,
    MyTaskStatus? status)
    {
        var user = await GetCurrentUser();

        var query = _context.TaskItem
                    .Include(x => x.Category)
                    .Where(x => x.UserId == user.Id)
                    .OrderByDescending(x => x.CreatedAt)
                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm)) query = query.Where(x => x.Title.Contains(searchTerm));
        
        if (categoryId.HasValue) query = query.Where(x => x.CategoryId == categoryId);

        if (status.HasValue) query = query.Where(x => x.Status == status);

        var tasks = await query.OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

        var categories = await _context.Category
        .Where(x => x.UserId == user.Id)
        .ToListAsync();


        var model = new TaskIndexViewModel
        {
            SearchTerm = searchTerm,
            CategoryId = categoryId,
            Status = status,

            Categories = categories,


            Tasks = tasks.Select(task => new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                CategoryId = task.CategoryId,

                CategoryName =
                    task.Category?.Name ?? "بدون دسته‌بندی",

                CategoryColor =
                    task.Category == null
                        ? "#64748B"
                        : TodoList.StaticData.CategoryColors.Colors
                            .FirstOrDefault(x =>
                                x.Id == task.Category.ColorIndex)
                            ?.HexColor ?? "#64748B"

            }).ToList()
        };


        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create (CreateTaskViewModel model)
    {
        var user = await GetCurrentUser();

        if (!ModelState.IsValid) return NotFound();

        var task = new TaskItem
        {
            Title = model.Title,
            Description = model.Description,

            CategoryId = model.CategoryId,
            DueDate = model.DueDate,

            Status = MyTaskStatus.InProgress,

            IsCompleted = false,

            CreatedAt = DateTime.Now,
            UserId = user.Id
        };

        _context.TaskItem.Add(task);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }

    [HttpPost]
    public async Task<IActionResult> Edit(TaskViewModel model)
    {
        var user = await GetCurrentUser();
        var task = await GetTask(model.Id, user.Id);

        if (task == null) return NotFound();

         task.Title = model.Title;
        task.Description = model.Description;
        task.CategoryId = model.CategoryId;
        task.DueDate = model.DueDate;
        task.Status = model.Status;

        await _context.SaveChangesAsync();

        TempData["Toast"] = "تسک با موفقیت تغییر کرد";

        return RedirectToAction(nameof(Index));

    }

     [HttpPost]
    public async Task<IActionResult> delete(TaskViewModel model)
    {
        var user = await GetCurrentUser();
        var task = await GetTask(model.Id, user.Id);

        if (task == null) return NotFound();

        _context.TaskItem.Remove(task);
        await _context.SaveChangesAsync();

        TempData["Toast"] = "تسک با موفقیت حذف شد";

        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Complete (int id)
    {
        var user = await GetCurrentUser();
        var task = await GetTask(id, user.Id);

        task.Status = MyTaskStatus.Completed;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

}