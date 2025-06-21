using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;
using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Web.Pages.Roles
{
    [Authorize(Roles = "Admin,Manager,Accountant,Member")]
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
            try
            {
                Roles = await _roleManagementService.GetRoleAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to load  Role List...");
            }
        }

        public async Task<IActionResult> OnPostUpdateRoleAsync()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to update role...");
                return RedirectToPage("/Error");
            }
        }
        public async Task<IActionResult> OnPostDeleteRoleAsync(Guid Id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to delete role...");
                return RedirectToPage("/Error");
            }
        }
    }
}