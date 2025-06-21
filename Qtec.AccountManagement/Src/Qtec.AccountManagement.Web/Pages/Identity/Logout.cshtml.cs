using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Qtec.AccountManagement.Web.Pages.Identity
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;
        public LogoutModel(ILogger<LogoutModel> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                await HttpContext.SignOutAsync("MyCookieAuth");
                TempData["SuccessMessage"] = "Logout successful!";
                return RedirectToPage("/Identity/Login");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to LogOut....");
                return RedirectToPage("/Error");
            }
        }
    }
}