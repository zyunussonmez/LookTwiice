using Microsoft.AspNetCore.Mvc;

namespace LookTwiice.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}