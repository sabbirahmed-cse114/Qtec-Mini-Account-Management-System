using Qtec.AccountManagement.Domain.Entities;
using System;

namespace Qtec.AccountManagement.Application.Services
{
    public interface IUserManagementService
    {
        Task<bool> RegistrationAsync(string name, string email, string password, Guid RoleId);
        Task<User?> LoginAsync(string email, string password);

    }
}
