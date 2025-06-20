using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;

namespace Qtec.AccountManagement.Web.Pages.Roles
{
    [Authorize(Roles = "Admin")]
    public class CreateRoleModel : PageModel
    {
        private readonly RoleManagementService _roleManagementService;

        public CreateRoleModel(RoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        [BindProperty]
        public string Name { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var success = await _roleManagementService.CreateRoleAsync(Name);

            if (success)
            {
                TempData["SuccessMessage"] = "Role created successfully!";
                return RedirectToPage("/Roles/RoleList");
            }

            Message = "Role name already exists.";
            return Page();
        }
    }
}