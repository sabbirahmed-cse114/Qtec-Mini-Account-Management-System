using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;

namespace Qtec.AccountManagement.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManagementService _userManagementService;
        public RegisterModel(UserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [BindProperty]
        public string Name { get; set; } = string.Empty;
        [BindProperty]
        public string Email { get; set; } = string.Empty;
        [BindProperty] 
        public string Password { get; set; } = string.Empty;        

        public string ErrorMessage { get; set; } = "";

        public async Task<IActionResult> OnPostAsync()
        {
            var success = await _userManagementService.ExecuteAsync(Name, Email, Password);

            if (!success)
            {
                ErrorMessage = "Email already exists!";
                return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}
