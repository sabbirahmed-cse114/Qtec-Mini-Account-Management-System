using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;

namespace Qtec.AccountManagement.Web.Pages.Roles
{
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

            Message = success ? "Role created successfully!" : "Role name already exists.";
            return Page();
        }
    }
}
