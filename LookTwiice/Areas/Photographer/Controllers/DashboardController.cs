using LookTwiice.Data;
using LookTwiice.Models;
using LookTwiice.Models.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LookTwiice.Areas.Photographer.Controllers;

[Area("Photographer")]
[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Photographer}")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var inquiries = await _context.ContactInquiries
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        ViewBag.PendingCount = inquiries.Count(x =>
            x.Status == InquiryStatus.New);

        ViewBag.InDiscussionCount = inquiries.Count(x =>
            x.Status == InquiryStatus.Contacted ||
            x.Status == InquiryStatus.InDiscussion);

        ViewBag.CompletedCount = inquiries.Count(x =>
            x.Status == InquiryStatus.Booked);

        ViewBag.ArchivedCount = inquiries.Count(x =>
            x.Status == InquiryStatus.Rejected ||
            x.Status == InquiryStatus.Archived);

        ViewBag.Pending = inquiries
            .Where(x => x.Status == InquiryStatus.New)
            .ToList();

        ViewBag.InDiscussion = inquiries
            .Where(x =>
                x.Status == InquiryStatus.Contacted ||
                x.Status == InquiryStatus.InDiscussion)
            .ToList();

        ViewBag.Completed = inquiries
            .Where(x => x.Status == InquiryStatus.Booked)
            .ToList();

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> InquiryDetails(int id)
    {
        var inquiry = await _context.ContactInquiries
            .FirstOrDefaultAsync(x => x.Id == id);

        if (inquiry == null)
            return NotFound();

        return PartialView("_InquiryDetails", inquiry);
    }

}