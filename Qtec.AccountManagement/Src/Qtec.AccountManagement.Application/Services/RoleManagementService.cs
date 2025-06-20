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

        public Task<IEnumerable<Role>> GetRoleAsync()
        {
            return _unitOfWork.Roles.GetAllRolesAsync();
        }

        public async Task<bool> UpdateRoleAsync(Guid id, string name)
        {
            var isTaken = await _unitOfWork.Roles.IsRoleNameTakenAsync(name);
            if (isTaken)
                return false;

            var role = new Role
            {
                Id = id,
                Name = name
            };

            await _unitOfWork.Roles.UpdateRoleAsync(role);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> DeleteRoleAsync(Guid Id)
        {
            if (Id == Guid.Empty)
                return false;
            await _unitOfWork.Roles.DeleteRoleAsync(Id);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}