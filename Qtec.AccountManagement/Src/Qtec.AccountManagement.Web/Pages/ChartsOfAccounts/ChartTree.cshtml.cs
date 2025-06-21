using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;
using Qtec.AccountManagement.Domain.Dtos;

namespace Qtec.AccountManagement.Web.Pages.ChartsOfAccounts
{
    [Authorize]
    public class ChartTreeModel : PageModel
    {
        private readonly AccountManagementService _accountManagementService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<ChartTreeModel> _logger;

        public ChartTreeModel(AccountManagementService accountManagementService, 
            IAuthorizationService authorizationService,
            ILogger<ChartTreeModel> logger )
        {
            _accountManagementService = accountManagementService;
            _authorizationService = authorizationService;
            _logger = logger;
        }
        public List<AccountDto> TreeView { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                TreeView = await _accountManagementService.GetTreeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to get all account using tree view...");
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            try
            {
                var result = await _authorizationService.AuthorizeAsync(User, "Admin");

                if (!result.Succeeded)
                {
                    return RedirectToPage("/AccessDenied");
                }
                var success = await _accountManagementService.DeleteAccountAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Account deleted successfully.";
                    return RedirectToPage("/ChartsOfAccounts/ChartTree");
                }
                TempData["ErrorMessage"] = "Failed to delete account.";
                return RedirectToPage("/ChartsOfAccounts/ChartTree");
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex,"Failed to delete account...");
                return RedirectToPage("/Error");
            }
        }
    }
}