using Qtec.AccountManagement.Domain.Entities;
using System;

namespace Qtec.AccountManagement.Application.Services
{
    public interface IUserManagementService
    {
        Task<bool> ExecuteAsync(string name, string email, string password);
        Task<User?> LoginAsync(string email, string password);

    }
}
