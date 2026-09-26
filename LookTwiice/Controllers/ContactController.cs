using LookTwiice.Data;
using LookTwiice.Models;
using LookTwiice.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LookTwiice.Controllers;

public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;

    public ContactController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new ContactInquiryViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactInquiryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var inquiry = new ContactInquiry
        {
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            ServiceType = model.ServiceType,
            EventDate = model.EventDate,
            Budget = model.Budget,
            Message = model.Message,
            Country = model.Country,
            City = model.City,
            Status = InquiryStatus.New
        };

        _context.ContactInquiries.Add(inquiry);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Success));
    }

    [HttpGet]
    public IActionResult Success()
    {
        return View();
    }
}