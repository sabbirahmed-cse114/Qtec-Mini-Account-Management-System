using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;
using Qtec.AccountManagement.Domain.Dtos;

namespace Qtec.AccountManagement.Web.Pages.ChartsOfAccounts
{
    public class ChartTreeModel : PageModel
    {
        private readonly AccountManagementService _accountManagementService;

        public ChartTreeModel(AccountManagementService accountManagementService)
        {
            _accountManagementService = accountManagementService;
        }

        public List<AccountDto> TreeView { get; set; } = new();

        public async Task OnGetAsync()
        {
            TreeView = await _accountManagementService.GetTreeAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var success = await _accountManagementService.DeleteAccountAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Account deleted successfully.";
                return RedirectToPage("/ChartsOfAccounts/ChartTree");
            }
            TempData["ErrorMessage"] = "Failed to delete account.";
            return RedirectToPage("/ChartsOfAccounts/ChartTree");
        }
    }
}