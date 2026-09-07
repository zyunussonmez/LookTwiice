using LookTwiice.Data;
using LookTwiice.Models;
using LookTwiice.Models.Constants;
using LookTwiice.Services.Interfaces;
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
        private readonly IImageService _imageService;

        private readonly IFileService _fileService;

        public PortfolioController(ApplicationDbContext context,IImageService imageService, IFileService fileService)
        {
            _context = context;
            _imageService = imageService;
            _fileService = fileService;
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

        public async Task<IActionResult> Photos(int galleryId)
        {
            var gallery = await _context.Galleries
                .Include(g => g.Photos)
                .FirstOrDefaultAsync(g => g.Id == galleryId);

            if (gallery == null)
            {
                return NotFound();
            }

            var photos = gallery.Photos
                .OrderBy(p => p.DisplayOrder)
                .ToList();

            ViewBag.Gallery = gallery;

            return View(photos);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePhoto(int galleryId)
        {
            var gallery = await _context.Galleries.FindAsync(galleryId);

            if (gallery == null)
            {
                return NotFound();
            }

            ViewBag.Gallery = gallery;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePhoto(
            int galleryId,
            Photo photo,
            IFormFile? file)
        {
            var gallery = await _context.Galleries.FindAsync(galleryId);

            if (gallery == null)
            {
                return NotFound();
            }

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(nameof(file), "Please select an image file.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Gallery = gallery;
                return View(photo);
            }

            try
            {
                var processedImage = await _imageService.ProcessAsync(
                    file!,
                    "uploads/photos");

                photo.GalleryId = gallery.Id;
                photo.OriginalFileName = processedImage.OriginalFileName;
                photo.OriginalUrl = processedImage.OriginalUrl;
                photo.WebUrl = processedImage.WebUrl;
                photo.ThumbnailUrl = processedImage.ThumbnailUrl;
                photo.MimeType = processedImage.MimeType;
                photo.FileSize = processedImage.FileSize;
                photo.Width = processedImage.Width;
                photo.Height = processedImage.Height;
                photo.DisplayOrder = await _context.Photos
                    .Where(p => p.GalleryId == gallery.Id)
                    .Select(p => p.DisplayOrder)
                    .DefaultIfEmpty()
                    .MaxAsync() + 1;

                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Photo uploaded successfully.";

                return RedirectToAction(nameof(Photos), new { galleryId = gallery.Id });
            }
            catch (ArgumentException exception)
            {
                ModelState.AddModelError(nameof(file), exception.Message);
                ViewBag.Gallery = gallery;
                return View(photo);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photo = await _context.Photos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (photo == null)
            {
                return NotFound();
            }

            var galleryId = photo.GalleryId;

            _fileService.DeleteFiles(
                photo.OriginalUrl,
                photo.WebUrl,
                photo.ThumbnailUrl
            );

            _context.Photos.Remove(photo);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Photo deleted successfully.";

            return RedirectToAction(
                nameof(Photos),
                new { galleryId });
        }

        [HttpGet]
        public async Task<IActionResult> EditPhoto(int id)
        {
            var photo = await _context.Photos
                .Include(p => p.Gallery)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPhoto(int id, Photo photo)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(photo);
            }

            var existingPhoto = await _context.Photos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPhoto == null)
            {
                return NotFound();
            }

            existingPhoto.Title = photo.Title;
            existingPhoto.IsFeatured = photo.IsFeatured;
            existingPhoto.DisplayOrder = photo.DisplayOrder;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Photo updated successfully.";

            return RedirectToAction(
                nameof(Photos),
                new { galleryId = existingPhoto.GalleryId });
        }

    }
}
