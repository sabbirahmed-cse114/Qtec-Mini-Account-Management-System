using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;

namespace Qtec.AccountManagement.Web.Pages.Roles
{
    [Authorize(Roles = "Admin,Manager")]
    public class CreateRoleModel : PageModel
    {
        private readonly RoleManagementService _roleManagementService;
        private readonly ILogger<CreateRoleModel> _logger;

        public CreateRoleModel(RoleManagementService roleManagementService, 
            ILogger<CreateRoleModel> logger)
        {
            _roleManagementService = roleManagementService;
            _logger = logger;
        }

        [BindProperty]
        public string Name { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var success = await _roleManagementService.CreateRoleAsync(Name);

                    if (success)
                    {
                        TempData["SuccessMessage"] = "Role created successfully!";
                        return RedirectToPage("/Roles/RoleList");
                    }
                    TempData["ErrorMessage"] = "Role name already exists.";
                    return Page();
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to create Roll....");
                return RedirectToPage("/Error");
            }
        }
    }
}