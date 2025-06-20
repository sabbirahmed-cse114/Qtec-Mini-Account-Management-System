using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;
using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Web.Pages.Roles
{
    public class RoleListModel : PageModel
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly RoleManagementService _roleManagementService;
        private readonly ILogger<RoleListModel> _logger;
        public RoleListModel(RoleManagementService roleManagementService,
            ILogger<RoleListModel> logger, IAuthorizationService authorizationService)
        {
            _roleManagementService = roleManagementService;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [BindProperty]
        public Guid Id { get; set; }

        [BindProperty]
        public string Name { get; set; } = default!;

        public IEnumerable<Role> Roles { get; set; } = new List<Role>();

        public async Task OnGetAsync()
        {
            Roles = await _roleManagementService.GetRoleAsync();
        }

        public async Task<IActionResult> OnPostUpdateRoleAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _authorizationService.AuthorizeAsync(User, "Admin");

                if (!result.Succeeded)
                {
                    return RedirectToPage("/AccessDenied");
                }
                var success = await _roleManagementService.UpdateRoleAsync(Id, Name);

                if (success)
                {
                    TempData["SuccessMessage"] = "Role updated successfully.";
                    return RedirectToPage("/Roles/RoleList");
                }
                TempData["ErrorMessage"] = "Failed to delete role.";
                return RedirectToPage("/Roles/RoleList");
            }
            return RedirectToPage("/Roles/RoleList");
        }


        public async Task<IActionResult> OnPostDeleteRoleAsync(Guid Id)
        {
            var result = await _authorizationService.AuthorizeAsync(User, "Admin");

            if (!result.Succeeded)
            {
                return RedirectToPage("/AccessDenied");
            }
            var success = await _roleManagementService.DeleteRoleAsync(Id);
            if (success)
            {
                TempData["SuccessMessage"] = "Role deleted successfully.";
                return RedirectToPage("/Roles/RoleList");
            }
            TempData["ErrorMessage"] = "Failed to delete role.";
            return RedirectToPage("/Roles/RoleList");
        }
    }
}