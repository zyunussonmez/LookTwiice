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

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .Include(c => c.Galleries)
                .ThenInclude(g => g.Photos)
            .Where(c => c.Galleries.Any(g => g.Photos.Any()))
            .OrderBy(c => c.Name)
            .ToListAsync();

        return View(categories);
    }

    public async Task<IActionResult> Gallery(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return NotFound();
        }

        var gallery = await _context.Galleries
            .Include(g => g.Category)
            .Include(g => g.Photos)
            .FirstOrDefaultAsync(g => g.Slug == slug);

        if (gallery == null)
        {
            return NotFound();
        }

        gallery.Photos = gallery.Photos
            .OrderBy(p => p.DisplayOrder)
            .ToList();

        return View(gallery);
    }

    public async Task<IActionResult> WeddingGallery()
    {
        var weddingCategory = await _context.Categories
            .Include(c => c.Galleries)
                .ThenInclude(g => g.Photos)
            .FirstOrDefaultAsync(c =>
                c.Name.ToLower() == "wedding");

        if (weddingCategory == null)
            return NotFound();

        var galleries = weddingCategory.Galleries
            .Where(g => g.Photos.Any())
            .OrderByDescending(g => g.IsFeatured)
            .ThenBy(g => g.Title)
            .ToList();

        return View(galleries);
    }
}