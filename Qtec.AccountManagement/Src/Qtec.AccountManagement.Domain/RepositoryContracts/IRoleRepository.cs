using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Domain.RepositoryContracts
{
    public interface IRoleRepository
    {
        Task<bool> IsRoleNameTakenAsync(string Name);
        Task CreateRoleAsync(Role role);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(Guid roleId);
        Task<Guid> GetRoleIdByNameAsync(string roleName);
    }
}