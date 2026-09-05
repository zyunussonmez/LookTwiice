using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LookTwiice.Areas.Photographer.Controllers
{
    [Area("Photographer")]
    [Authorize(Roles = "Photographer")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}