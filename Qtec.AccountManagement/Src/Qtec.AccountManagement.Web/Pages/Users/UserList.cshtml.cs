using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;
using Qtec.AccountManagement.Domain.Dtos;
using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Web.Pages.Users
{
    [Authorize(Roles = "Admin,Manager,Accountant,Member")]
    public class UserListModel : PageModel
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManagementService _userManagementService;
        private readonly RoleManagementService _roleManagementService;
        private readonly ILogger<UserListModel> _logger;

        public UserListModel(UserManagementService userManagementService,
            RoleManagementService roleManagementService,
            IAuthorizationService authorizationService,
            ILogger<UserListModel> logger)
        {
            _userManagementService = userManagementService;
            _roleManagementService = roleManagementService;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [BindProperty]
        public Guid UserId { get; set; }

        [BindProperty]
        public string NewRole { get; set; }

        public List<UserDto> Users { get; set; }
        public List<Role> RoleNames { get; set; }
        public async Task OnGetAsync()
        {
            try
            {
                var users = await _userManagementService.GetUserAsync();
                Users = users != null ? new List<UserDto>(users) : new List<UserDto>();
                RoleNames = (await _roleManagementService.GetRoleAsync()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to get User List...");
            }
        }

        public async Task<IActionResult> OnPostChangeRoleAsync()
        {
            try
            {
                var result = await _authorizationService.AuthorizeAsync(User, "Admin");

                if (!result.Succeeded)
                {
                    return RedirectToPage("/AccessDenied");
                }

                if (UserId == Guid.Empty || string.IsNullOrWhiteSpace(NewRole))
                {
                    TempData["ErrorMessage"] = "Invalid user or role.";
                    return RedirectToPage("/Users/UserList");
                }

                var updated = await _userManagementService.ChangeUserRoleAsync(UserId, NewRole);
                TempData["SuccessMessage"] = "Role changed successfully!";
                return RedirectToPage("/Users/UserList");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to Change User Role...");
                return RedirectToPage("/Error");

            }
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(Guid Id)
        {
            try
            {
                var result = await _authorizationService.AuthorizeAsync(User, "Admin");

                if (!result.Succeeded)
                {
                    return RedirectToPage("/AccessDenied");
                }
                var success = await _userManagementService.DeleteUserAsync(Id);
                if (success)
                {
                    TempData["SuccessMessage"] = "User deleted successfully.";
                    return RedirectToPage("/Users/UserList");
                }
                TempData["ErrorMessage"] = "Failed to delete user.";
                return RedirectToPage("/Users/UserList");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to delete user...");
                return RedirectToPage("/Error");
            }
        }
    }
}