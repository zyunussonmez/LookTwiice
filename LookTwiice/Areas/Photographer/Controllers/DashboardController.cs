using LookTwiice.Models.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LookTwiice.Areas.Photographer.Controllers
{
    [Area("Photographer")]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Photographer}")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}