using Qtec.AccountManagement.Domain.Dtos;
using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Domain.RepositoryContracts
{
    public interface IAccountRepository
    {
        Task CreateAccountAsync(Account account);
        Task<bool> IsAccountTakenAsync(string name, Guid? paretnId);
        Task<IEnumerable<AccountDto>> GetAllAccountAsync();
        Task<AccountDto?> GetAccountByIdAsync(Guid id);
        Task<Guid> GetParentIdByParentNameAsync(string parentName);
        Task<bool> UpdateAccountAsync(Account account);
        Task DeleteAccountAsync(Guid id);
    }
}