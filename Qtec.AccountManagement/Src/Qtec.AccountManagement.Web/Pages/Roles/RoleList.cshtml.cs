using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;
using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Web.Pages.Roles
{
    public class RoleListModel : PageModel
    {
        private readonly RoleManagementService _roleManagementService;
        public RoleListModel(RoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
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
            var success = await _roleManagementService.UpdateRoleAsync(Id, Name);
            if (!success)
            {
                ModelState.AddModelError("", "Update failed. Role name may already exist.");
            }

            return RedirectToPage();
        }
    }
}
