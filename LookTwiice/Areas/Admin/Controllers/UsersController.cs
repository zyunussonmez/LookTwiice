using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LookTwiice.Models;
using LookTwiice.Models.Constants;

namespace LookTwiice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new Dictionary<string, IList<string>>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles;
            }

            ViewBag.UserRoles = userRoles;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            // Gelen rol adı gerçekten tanımlı rollerden biri mi kontrol et
            if (newRole != RoleNames.Admin && newRole != RoleNames.User && newRole != RoleNames.Photographer)
            {
                return BadRequest("Geçersiz rol.");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            if (userId == currentUserId && newRole != RoleNames.Admin)
            {
                TempData["ErrorMessage"] = "Kendi Admin yetkinizi değiştiremezsiniz.";
                return RedirectToAction(nameof(Index));
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var addResult = await _userManager.AddToRoleAsync(user, newRole);

            if (!removeResult.Succeeded || !addResult.Succeeded)
            {
                TempData["ErrorMessage"] = "Rol güncellenirken bir hata oluştu.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // Kullanıcı kendi kendini silemesin
            var currentUserId = _userManager.GetUserId(User);
            if (userId == currentUserId)
            {
                TempData["ErrorMessage"] = "Kendi hesabınızı silemezsiniz.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] = "Kullanıcı silinirken bir hata oluştu.";
            }
            else
            {
                TempData["SuccessMessage"] = "Kullanıcı silindi.";
            }

            return RedirectToAction(nameof(Index));
        }   
    }
}
