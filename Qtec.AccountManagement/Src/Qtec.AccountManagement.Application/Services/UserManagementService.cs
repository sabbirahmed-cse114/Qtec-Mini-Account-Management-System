using Qtec.AccountManagement.Domain;
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
        public async Task<bool> RegistrationAsync(string name, string email, string password)
        {
            if (await _unitOfWork.Users.IsEmailTakenAsync(email))
                return false;

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    Password = HashPassword(password)
                };

                await _unitOfWork.Users.RegisterAsync(user);
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
