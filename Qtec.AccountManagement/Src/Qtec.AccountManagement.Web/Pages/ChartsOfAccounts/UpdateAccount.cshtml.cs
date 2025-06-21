using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;
using Qtec.AccountManagement.Domain.Dtos;

namespace Qtec.AccountManagement.Web.Pages.ChartsOfAccounts
{
    [Authorize(Roles = "Admin,Manager,Accountant")]
    public class UpdateAccountModel : PageModel
    {
        private readonly AccountManagementService _accountManagementService;
        private readonly ILogger<UpdateAccountModel> _logger;
        
        public UpdateAccountModel(AccountManagementService accountManagementService,
            ILogger<UpdateAccountModel> logger)
        {
            _accountManagementService = accountManagementService;
            _logger = logger;
        }

        [BindProperty]
        public Guid Id { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Type { get; set; }
        [BindProperty]
        public string ParentName { get; set; }

        public List<AccountDto> AllAccounts { get; set; } = new();
        public List<Guid> ExcludedChildAccountIds { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                var account = await _accountManagementService.GetAccountByIdAsync(id);
                if (account == null)
                {
                    return NotFound();
                }

                Id = account.Id;
                Name = account.Name;
                Type = account.Type;
                ParentName = account.ParentName;
                AllAccounts = (await _accountManagementService.GetAllAccountsAsync()).ToList();
                ExcludedChildAccountIds.Add(Id);

                void FindChildAccount(Guid parentId)
                {
                    foreach (var acc in AllAccounts)
                    {
                        if (acc.ParentId == parentId)
                        {
                            ExcludedChildAccountIds.Add(acc.Id);
                            FindChildAccount(acc.Id);
                        }
                    }
                }
                FindChildAccount(Id);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to get account by Id...");
                return RedirectToPage("/Error");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Type))
                {
                    TempData["ErrorMessage"] = "Invalid account name or type.";
                    return Page();
                }

                var success = await _accountManagementService.UpdateAccountAsync(Id, Name, Type, ParentName);
                if (success)
                {
                    TempData["SuccessMessage"] = "Account updated successfully.";
                    return RedirectToPage("/ChartsOfAccounts/ChartTree");
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to update account...");
                return RedirectToPage("/Error");
            }
        }
    }
}