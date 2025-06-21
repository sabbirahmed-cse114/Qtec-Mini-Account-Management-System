
using Qtec.AccountManagement.Domain.Dtos;

namespace Qtec.AccountManagement.Application.Services
{
    public interface IAccountManagementService
    {
        Task<bool> CreateAccountAsync(string name, string type, Guid? parentId);
        Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
        Task<List<AccountDto>> GetTreeAsync();
        Task<AccountDto?> GetAccountByIdAsync(Guid id);
        Task<bool> UpdateAccountAsync(Guid id, string name, string type, string? parentName);
        Task<bool> DeleteAccountAsync(Guid Id);
    }
}
