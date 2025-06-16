using Qtec.AccountManagement.Domain;
using Qtec.AccountManagement.Domain.Dtos;
using Qtec.AccountManagement.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Qtec.AccountManagement.Application.Services
{
    public class UserManagementService : IUserManagementService
    {
       private readonly IUnitOfWork _unitOfWork;

        public UserManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> RegistrationAsync(string name, string email, string password, Guid rollId)
        {
            if (await _unitOfWork.Users.IsEmailTakenAsync(email))
                return false;

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    Password = HashPassword(password),
                    RoleId = rollId
                };

                await _unitOfWork.Users.CreateNewUserAsync(user);
                await _unitOfWork.CommitAsync();
                return true;
        }

        public Task<IEnumerable<UserDto>> GetUserAsync()
        {
            return _unitOfWork.Users.GetAllUserAsync();
        }

        public async Task<bool> ChangeUserRoleAsync(Guid userId, string roleName)
        {
            var roleId = await _unitOfWork.Roles.GetRoleIdByNameAsync(roleName);
            if (roleId == Guid.Empty) return false;

            await _unitOfWork.Users.ChangeUserRoleAsync(userId, roleId);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid Id)
        {
            if (Id == Guid.Empty)
                return false;
            await _unitOfWork.Users.DeleteUserByIdAsync(Id);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var hashedPassword = HashPassword(password);
            var user = await _unitOfWork.Users.ValidateUserAsync(email, hashedPassword);
            return user;
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }
    }
}
