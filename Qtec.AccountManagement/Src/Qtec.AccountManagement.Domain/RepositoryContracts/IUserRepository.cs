using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Domain.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<bool> IsEmailTakenAsync(string email);
        Task RegisterAsync(User user);
    }
}
