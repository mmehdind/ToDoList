using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TodoList.Data;
using TodoList.Models;
using TodoList.StaticData;
using TodoList.ViewModels;

namespace TodoList.Controllers;

[Authorize]
public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CategoryController (ApplicationDbContext Context, UserManager<ApplicationUser> UserManager)
    {
        _context = Context;
        _userManager = UserManager;
    }

    private async Task<ApplicationUser> GetCurrentUser() 
        { 
            return await _userManager.GetUserAsync(User); 
        }

    public async Task<IActionResult> Index()
    {
        var user = await GetCurrentUser();

        var categories = await _context.Category
                        .Where(x => x.UserId == user.Id)
                        .ToListAsync();

        // if (categories.Count == 0) return NotFound();

        var model = categories.Select(category => new CategoryViewModel
        {
            Id = category.Id,
            Name = category.Name,
            ColorIndex = category.ColorIndex,

            ColorHex = CategoryColors.Colors
                        .First(x => x.Id == category.ColorIndex)
                        .HexColor
        }).ToList();

        return View(model);

    }

    public async Task<IActionResult> Create(CreateCategoryViewModel model)
    {
        if (!ModelState.IsValid) return NotFound();

        var user = await GetCurrentUser();

        var category = new Category
        {
            Name = model.Name,
            ColorIndex = model.ColorIndex,

            UserId = user.Id
        };

        _context.Category.Add(category);
        await _context.SaveChangesAsync();

        TempData["Toast"] = "با موفقیت ایجاد شد";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(
        Category model)
    {
        var user = await GetCurrentUser();

        var category = await _context.Category
            .FirstOrDefaultAsync(X => X.Id == model.Id
            && X.UserId == user.Id);

        if (category == null) return NotFound();

        category.Name = model.Name;
        category.ColorIndex = model.ColorIndex;

        _context.Category.Update(category);
        await _context.SaveChangesAsync();

        TempData["Toast"] = "با موفقیت تغییر کرد";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {

        var user = await GetCurrentUser();

        var category = await _context.Category
                        .FirstOrDefaultAsync(X => X.Id == id 
                        && X.UserId == user.Id);

        if (category == null) return NotFound();

        var hasTask = await _context.TaskItem
                        .AnyAsync(x => x.CategoryId == id);
        if (hasTask)
        {
        TempData["Error"] =
            "این دسته‌بندی دارای تسک است و قابل حذف نیست.";

        return RedirectToAction(nameof(Index));
        }

        _context.Category.Remove(category);
        await _context.SaveChangesAsync();

        TempData["Toast"] = "با موفقیت حذف شد";

        return RedirectToAction(nameof(Index));
        
    }


}