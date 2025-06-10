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

        public IEnumerable<Role> Roles { get; set; } = new List<Role>();

        public async Task OnGetAsync()
        {
            Roles = await _roleManagementService.GetRoleAsync();
        }
    }
}
