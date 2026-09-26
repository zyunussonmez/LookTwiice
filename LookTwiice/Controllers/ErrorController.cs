using Microsoft.AspNetCore.Mvc;

namespace LookTwiice.Controllers;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult Index(int statusCode)
    {
        return statusCode switch
        {
            404 => View("NotFound"),
            403 => View("Forbidden"),
            _ => View("Error")
        };
    }
}

