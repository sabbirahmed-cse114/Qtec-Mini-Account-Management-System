﻿using Qtec.AccountManagement.Domain.Dtos;
using Qtec.AccountManagement.Domain.Entities;
using Qtec.AccountManagement.Domain.RepositoryContracts;
using System.Data;
using System.Data.SqlClient;

namespace Qtec.AccountManagement.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;

        public AccountRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task CreateAccountAsync(Account account)
        {
            var cmd = new SqlCommand("sp_ManageChartOfAccounts", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Action", "Create");
            cmd.Parameters.AddWithValue("@Id", account.Id);
            cmd.Parameters.AddWithValue("@Name", account.Name);
            cmd.Parameters.AddWithValue("@Type", account.Type);
            cmd.Parameters.AddWithValue("@ParentId", (object?)account.ParentId ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> IsAccountTakenAsync(string name, Guid? parentId)
        {
            var cmd = new SqlCommand("sp_ManageChartOfAccounts", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Action", "Exists");
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("ParentId", parentId);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result) == 1;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountAsync()
        {
            var cmd = new SqlCommand("sp_ManageChartOfAccounts", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Action", "GetAll");

            var accounts = new List<AccountDto>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                accounts.Add(new AccountDto
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Type = reader.GetString(2),
                    ParentId = reader.IsDBNull(3) ? null : reader.GetGuid(3),
                    ParentName = reader.IsDBNull(4) ? null : reader.GetString(4)
                });
            }
            return accounts;
        }

        public async Task<AccountDto?> GetAccountByIdAsync(Guid id)
        {
            var cmd = new SqlCommand("sp_ManageChartOfAccounts", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Action", "GetById");
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new AccountDto
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Type = reader.GetString(2),
                    ParentId = reader.IsDBNull(3) ? null : reader.GetGuid(3),
                    ParentName = reader.IsDBNull(4) ? null : reader.GetString(4)
                };
            }
            return null;
        }
        public async Task<Guid> GetParentIdByParentNameAsync(string name)
        {
            var cmd = new SqlCommand("sp_ManageChartOfAccounts", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Action", "GetParentId");
            cmd.Parameters.AddWithValue("@Name", name);

            var result = await cmd.ExecuteScalarAsync();
            if (result != null && Guid.TryParse(result.ToString(), out Guid parentId))
            {
                return parentId;
            }
            return Guid.Empty;
        }


        public async Task<bool> UpdateAccountAsync(Account account)
        {
            var cmd = new SqlCommand("sp_ManageChartOfAccounts", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Action", "Update");
            cmd.Parameters.AddWithValue("@Id", account.Id);
            cmd.Parameters.AddWithValue("@Name", account.Name);
            cmd.Parameters.AddWithValue("@Type", account.Type);
            cmd.Parameters.AddWithValue("@ParentId", (object?)account.ParentId ?? DBNull.Value);

            var result = await cmd.ExecuteNonQueryAsync();
            return Convert.ToInt32(result) == 1;
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            var cmd = new SqlCommand("sp_ManageChartOfAccounts", _connection, _transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Action", "Delete");
            cmd.Parameters.AddWithValue("@Id", id);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}