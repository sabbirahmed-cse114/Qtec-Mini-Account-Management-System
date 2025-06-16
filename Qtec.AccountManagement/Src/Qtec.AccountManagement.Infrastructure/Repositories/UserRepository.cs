using Qtec.AccountManagement.Domain.Dtos;
using Qtec.AccountManagement.Domain.Entities;
using Qtec.AccountManagement.Domain.RepositoryContracts;
using System.Data;
using System.Data.SqlClient;

namespace Qtec.AccountManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;

        public UserRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }
        public async Task<bool> IsEmailTakenAsync(string email)
        {
            var cmd = new SqlCommand("sp_CheckEmailExists", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Email", email);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result) == 1;
        }
        public async Task CreateNewUserAsync(User user)
        {
            var cmd = new SqlCommand("sp_CreateNewUser", _connection, _transaction)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@RoleId", user.RoleId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<UserDto>> GetAllUserAsync()
        {
            var users = new List<UserDto>();

            using var cmd = new SqlCommand("sp_GetUserList", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new UserDto
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    RoleName = reader.GetString(3)
                });
            }
            return users;
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var cmd = new SqlCommand("sp_ValidateUserLogin", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Password = reader.GetString(3)
                };
            }
            return null;
        }

        public async Task<bool> ChangeUserRoleAsync(Guid userId, Guid roleId)
        {
            var cmd = new SqlCommand("sp_ChangeUserRole", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@RoleId", roleId);

            var result = await cmd.ExecuteNonQueryAsync();
            return Convert.ToInt32(result) == 1;
        }

        public async Task DeleteUserByIdAsync(Guid Id)
        {
            var cmd = new SqlCommand("sp_DeleteUserById", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", Id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}