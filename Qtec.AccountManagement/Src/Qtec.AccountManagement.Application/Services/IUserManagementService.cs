using Qtec.AccountManagement.Domain.Dtos;

namespace Qtec.AccountManagement.Application.Services
{
    public interface IUserManagementService
    {
        Task<bool> RegistrationAsync(string name, string email, string password, Guid RoleId);
        Task<UserDto?> LoginAsync(string email, string password);
    }
}