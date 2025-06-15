using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Qtec.AccountManagement.Application.Services;
using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Web.Pages.Users
{
    public class RegisterModel : PageModel
    {
        private readonly UserManagementService _userManagementService;
        private readonly RoleManagementService _roleManagementService;
        public RegisterModel(UserManagementService userManagementService, RoleManagementService roleManagementService)
        {
            _userManagementService = userManagementService;
            _roleManagementService = roleManagementService;
        }

        [BindProperty]
        public string Name { get; set; } = string.Empty;
        [BindProperty]
        public string Email { get; set; } = string.Empty;
        [BindProperty]
        public string Password { get; set; } = string.Empty;
        [BindProperty]
        public Guid RoleId { get; set; }
        public List<SelectListItem> RoleList { get; set; }

        public string Message { get; set; } = "";

        public async Task<IActionResult> OnPostAsync()
        {
            var RoleNames = await _roleManagementService.GetRoleAsync();
            foreach (var role in RoleNames)
            {
                if (role.Name == "Viewer")
                {
                    RoleId = role.Id;
                    break;
                }
            }

            var success = await _userManagementService.RegistrationAsync(Name, Email, Password, RoleId);

            if (success)
            {
                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToPage("/Users/UserList");
            }
            Message = "Email already exists...";
            return Page();
        }
    }
}
