
using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Application.Services
{
    public interface IRoleManagementService
    {
        Task<bool> CreateRoleAsync(string name);
        Task<IEnumerable<Role>> GetRoleAsync();
        Task<bool> UpdateRoleAsync(Guid id, string name);
        Task<bool> DeleteRoleAsync(Guid Id);
    }
}
