using LookTwiice.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LookTwiice.Controllers;

public class PortfolioController : Controller
{
    private readonly ApplicationDbContext _context;

    public PortfolioController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoryId)
    {
        var categories = await _context.Categories
            .Include(c => c.Galleries)
            .ThenInclude(g => g.Photos)
            .OrderBy(c => c.Name)
            .ToListAsync();

        var galleriesQuery = _context.Galleries
            .Include(g => g.Category)
            .Include(g => g.Photos)
            .Where(g => g.Photos.Any());

        if (categoryId.HasValue)
        {
            galleriesQuery = galleriesQuery
                .Where(g => g.CategoryId == categoryId.Value);
        }

        var galleries = await galleriesQuery
            .OrderByDescending(g => g.IsFeatured)
            .ThenBy(g => g.Title)
            .ToListAsync();

        ViewBag.Categories = categories;
        ViewBag.SelectedCategoryId = categoryId;

        return View(galleries);
    }
}