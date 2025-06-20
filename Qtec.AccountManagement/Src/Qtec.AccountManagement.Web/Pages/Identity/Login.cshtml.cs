using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;
using System.Security.Claims;

namespace Qtec.AccountManagement.Web.Pages.Identity
{
    public class LoginModel : PageModel
    {
        private readonly UserManagementService _userManagementService;

        public LoginModel(UserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManagementService.LoginAsync(Email, Password);

                if (user == null)
                {
                    TempData["ErrorMessage"] = "Invalid email or password...";
                    return Page();
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.RoleName)
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("MyCookieAuth", principal);
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}