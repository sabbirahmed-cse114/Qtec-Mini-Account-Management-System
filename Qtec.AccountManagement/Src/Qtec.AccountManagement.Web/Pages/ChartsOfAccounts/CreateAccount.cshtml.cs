using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qtec.AccountManagement.Application.Services;
using Qtec.AccountManagement.Domain.Dtos;

namespace Qtec.AccountManagement.Web.Pages.ChartsOfAccounts
{
    public class CreateAccountModel : PageModel
    {
        private readonly AccountManagementService _accountManagementService;

        public CreateAccountModel(AccountManagementService accountManagementService)
        {
            _accountManagementService = accountManagementService;
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
            AllAccounts = (await _accountManagementService.GetAllAccountsAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
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
    }
}