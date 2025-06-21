using Qtec.AccountManagement.Domain.Dtos;

namespace Qtec.AccountManagement.Application.Services
{
    public interface IUserManagementService
    {
        Task<bool> RegistrationAsync(string name, string email, string password, Guid RoleId);
        Task<IEnumerable<UserDto>> GetUserAsync();
        Task<bool> ChangeUserRoleAsync(Guid userId, string roleName);
        Task<bool> DeleteUserAsync(Guid Id);
        Task<UserDto?> LoginAsync(string email, string password);
    }
}