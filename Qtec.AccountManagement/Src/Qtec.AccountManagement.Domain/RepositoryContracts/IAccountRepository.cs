using Qtec.AccountManagement.Domain.Dtos;
using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Domain.RepositoryContracts
{
    public interface IAccountRepository
    {
        Task CreateAccountAsync(Account account);
        Task<IEnumerable<AccountDto>> GetAllAccountAsync();
    }
}
