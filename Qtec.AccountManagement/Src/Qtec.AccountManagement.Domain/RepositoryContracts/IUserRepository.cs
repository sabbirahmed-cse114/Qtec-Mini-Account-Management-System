using Qtec.AccountManagement.Domain.Dtos;
using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Domain.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<UserDto?> ValidateUserAsync(string email, string password);
        Task<bool> IsEmailTakenAsync(string email);
        Task CreateNewUserAsync(User user);
        Task<IEnumerable<UserDto>> GetAllUserAsync();
        Task<bool> ChangeUserRoleAsync(Guid userId, Guid roleId);
        Task DeleteUserByIdAsync(Guid Id);
    }
}
