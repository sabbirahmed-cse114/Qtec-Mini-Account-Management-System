using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Qtec.AccountManagement.Web.Pages.Identity
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPost()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            TempData["SuccessMessage"] = "Logout successful!";
            return RedirectToPage("/Identity/Login");
        }
    }
}