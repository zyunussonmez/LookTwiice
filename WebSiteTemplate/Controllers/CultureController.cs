using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;

namespace WebSiteTemplate.Controllers
{
    public class CultureController : Controller
    {
        [HttpGet]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            if (string.IsNullOrEmpty(returnUrl))
            {
                return Redirect("/");
            }

            return LocalRedirect(returnUrl);
        }
    }
}