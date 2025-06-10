using Qtec.AccountManagement.Domain;
using Qtec.AccountManagement.Domain.Entities;


namespace Qtec.AccountManagement.Application.Services
{
    public class RoleManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateRoleAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name cannot be empty");

            if (await _unitOfWork.Roles.IsRoleNameTakenAsync(name))
                return false;

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = name
            };
            await _unitOfWork.Roles.CreateRoleAsync(role);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
