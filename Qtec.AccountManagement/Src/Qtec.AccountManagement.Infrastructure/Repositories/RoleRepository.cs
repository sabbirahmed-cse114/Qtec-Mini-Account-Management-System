﻿using Qtec.AccountManagement.Domain.Entities;
using Qtec.AccountManagement.Domain.RepositoryContracts;
using System.Data;
using System.Data.SqlClient;

namespace Qtec.AccountManagement.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;

        public RoleRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task<bool> IsRoleNameTakenAsync(string name)
        {
            var cmd = new SqlCommand("sp_CheckRoleExistsOrNot", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Name", name);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result) == 1;
        }

        public async Task CreateRoleAsync(Role role)
        {
            var cmd = new SqlCommand("sp_CreateNewRole", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", role.Id);
            cmd.Parameters.AddWithValue("@Name", role.Name);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            var roles = new List<Role>();

            using var cmd = new SqlCommand("sp_GetAllRoles", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                roles.Add(new Role
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1)
                });
            }
            return roles;
        }

        public async Task UpdateRoleAsync(Role role)
        {
            var cmd = new SqlCommand("sp_UpdateRole", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", role.Id);
            cmd.Parameters.AddWithValue("@Name", role.Name);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteRoleAsync(Guid Id)
        {
            var cmd = new SqlCommand("sp_DeleteRoleById", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Guid> GetRoleIdByNameAsync(string roleName)
        {
            var cmd = new SqlCommand("sp_GetRoleIdByName", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Name", roleName);

            var result = await cmd.ExecuteScalarAsync();
            if (result != null && Guid.TryParse(result.ToString(), out Guid roleId))
            {
                return roleId;
            }
            return Guid.Empty;
        }
    }
}