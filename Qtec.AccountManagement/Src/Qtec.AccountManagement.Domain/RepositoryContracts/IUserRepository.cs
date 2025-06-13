using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Domain.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<User?> ValidateUserAsync(string email, string password);
        Task<bool> IsEmailTakenAsync(string email);
        Task RegisterAsync(User user);
    }
}
