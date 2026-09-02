using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSiteTemplate.Models.Constants;

namespace WebSiteTemplate.Areas.Admin.Controllers
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
