using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Qtec.AccountManagement.Application.Services;

namespace Qtec.AccountManagement.Web.Pages.Users
{
    public class RegisterModel : PageModel
    {
        private readonly UserManagementService _userManagementService;
        private readonly RoleManagementService _roleManagementService;
        private readonly ILogger<RegisterModel> _logger;
        public RegisterModel(UserManagementService userManagementService, 
            RoleManagementService roleManagementService,
            ILogger<RegisterModel> logger)
        {
            _userManagementService = userManagementService;
            _roleManagementService = roleManagementService;
            _logger = logger;
        }

        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public Guid RoleId { get; set; }
        public List<SelectListItem> RoleList { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
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
                        return RedirectToPage("/Identity/Login");
                    }
                    TempData["ErrorMessage"] = "Email already exists...";
                    return Page();
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to user registration...");
                return RedirectToPage("/Error");
            }
        }
    }
}