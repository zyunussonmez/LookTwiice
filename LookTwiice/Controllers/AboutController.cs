using Microsoft.AspNetCore.Mvc;

namespace LookTwiice.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
