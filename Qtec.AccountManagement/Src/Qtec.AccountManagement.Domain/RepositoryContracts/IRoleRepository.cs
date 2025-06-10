using Qtec.AccountManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qtec.AccountManagement.Domain.RepositoryContracts
{
    public interface IRoleRepository
    {
        Task<bool> IsRoleNameTakenAsync(string Name);
        Task CreateRoleAsync(Role role);
    }
}
