using LookTwiice.Data;
using LookTwiice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
namespace LookTwiice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var featuredGalleries = await _context.Galleries
                .Include(g => g.Category)
                .Include(g => g.Photos)
                .Where(g => g.IsFeatured && g.Photos.Any())
                .OrderBy(g => g.Title)
                .ToListAsync();

            var categories = await _context.Categories
                .Include(c => c.Galleries)
                    .ThenInclude(g => g.Photos)
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.Categories = categories;

            return View(featuredGalleries);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode ?? 500
            };

            return View(model);
        }

        
    }
}
