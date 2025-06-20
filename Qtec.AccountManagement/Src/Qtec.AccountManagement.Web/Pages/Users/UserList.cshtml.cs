using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;
using Qtec.AccountManagement.Domain.Dtos;
using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Web.Pages.Users
{
    [Authorize(Roles = "Admin,Accountant,Member")]
    public class UserListModel : PageModel
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManagementService _userManagementService;
        private readonly RoleManagementService _roleManagementService;

        public UserListModel(UserManagementService userManagementService,
            RoleManagementService roleManagementService,
            IAuthorizationService authorizationService)
        {
            _userManagementService = userManagementService;
            _roleManagementService = roleManagementService;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public Guid UserId { get; set; }

        [BindProperty]
        public string NewRole { get; set; }

        public List<UserDto> Users { get; set; }
        public List<Role> RoleNames { get; set; }

        public async Task OnGetAsync()
        {
            var users = await _userManagementService.GetUserAsync();
            Users = users != null ? new List<UserDto>(users) : new List<UserDto>();
            RoleNames = (await _roleManagementService.GetRoleAsync()).ToList();
        }

        public async Task<IActionResult> OnPostChangeRoleAsync()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "Admin");

            if (!result.Succeeded)
            {
                return RedirectToPage("/AccessDenied");
            }

            if (UserId == Guid.Empty || string.IsNullOrWhiteSpace(NewRole))
            {
                TempData["ErrorMessage"] = "Invalid user or role.";
                return RedirectToPage();
            }

            var updated = await _userManagementService.ChangeUserRoleAsync(UserId, NewRole);
            TempData["SuccessMessage"] = "Role changed successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(Guid Id)
        {
            var result = await _authorizationService.AuthorizeAsync(User, "Admin");

            if (!result.Succeeded)
            {
                return RedirectToPage("/AccessDenied");
            }
            await _userManagementService.DeleteUserAsync(Id);
            TempData["SuccessMessage"] = "User deleted successfully.";
            return RedirectToPage();
        }
    }
}