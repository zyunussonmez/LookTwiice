using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LookTwiice.Models.Constants;

namespace LookTwiice.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        [Area("Admin")]
        [Authorize(Roles = RoleNames.Admin)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
