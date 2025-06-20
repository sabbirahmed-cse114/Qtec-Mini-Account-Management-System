using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;
using Qtec.AccountManagement.Domain.Dtos;

namespace Qtec.AccountManagement.Web.Pages.ChartsOfAccounts
{
    public class CreateAccountModel : PageModel
    {
        private readonly AccountManagementService _accountManagementService;
        private readonly ILogger<CreateAccountModel> _logger;

        public CreateAccountModel(AccountManagementService accountManagementService,
            ILogger<CreateAccountModel> logger)
        {
            _accountManagementService = accountManagementService;
            _logger = logger;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Type { get; set; }

        [BindProperty]
        public Guid? ParentId { get; set; }

        public List<AccountDto> AllAccounts { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                AllAccounts = (await _accountManagementService.GetAllAccountsAsync()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to create account...");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var success = await _accountManagementService.CreateAccountAsync(Name, Type, ParentId);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Account created successfully!";
                        return RedirectToPage("/ChartsOfAccounts/ChartTree");
                    }
                    TempData["ErrorMessage"] = "Failed to create account.";
                    return Page();
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to create account...");
                return RedirectToAction("/Error");
            }
        }
    }
}