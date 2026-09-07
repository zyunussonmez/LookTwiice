using LookTwiice.Data;
using LookTwiice.Models;
using LookTwiice.Models.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LookTwiice.Areas.Photographer.Controllers
{
    [Area("Photographer")]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Photographer}")]
    public class PortfolioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PortfolioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            category.Slug = category.Slug.Trim().ToLowerInvariant();

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Category created successfully.";

            return RedirectToAction(nameof(Categories));
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            category.Slug = category.Slug.Trim().ToLowerInvariant();

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Category updated successfully.";

            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Category deleted successfully.";

            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> Galleries()
        {
            var galleries = await _context.Galleries
                .Include(g => g.Category)
                .OrderBy(g => g.Title)
                .ToListAsync();

            return View(galleries);
        }

        [HttpGet]
        public async Task<IActionResult> CreateGallery()
        {
            ViewBag.Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGallery(Gallery gallery)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                return View(gallery);
            }

            gallery.Slug = gallery.Slug.Trim().ToLowerInvariant();

            _context.Galleries.Add(gallery);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Gallery created successfully.";

            return RedirectToAction(nameof(Galleries));
        }

        [HttpGet]
        public async Task<IActionResult> EditGallery(int id)
        {
            var gallery = await _context.Galleries.FindAsync(id);

            if (gallery == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(gallery);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGallery(int id, Gallery gallery)
        {
            if (id != gallery.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                return View(gallery);
            }

            gallery.Slug = gallery.Slug.Trim().ToLowerInvariant();

            _context.Galleries.Update(gallery);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Gallery updated successfully.";

            return RedirectToAction(nameof(Galleries));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGallery(int id)
        {
            var gallery = await _context.Galleries.FindAsync(id);

            if (gallery == null)
            {
                return NotFound();
            }

            _context.Galleries.Remove(gallery);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Gallery deleted successfully.";

            return RedirectToAction(nameof(Galleries));
        }
    }
}