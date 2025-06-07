using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;

namespace Qtec.AccountManagement.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManagementService _userManagementService;

        public LoginModel(UserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }      

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManagementService.LoginAsync(Email, Password);

            if (user == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}
