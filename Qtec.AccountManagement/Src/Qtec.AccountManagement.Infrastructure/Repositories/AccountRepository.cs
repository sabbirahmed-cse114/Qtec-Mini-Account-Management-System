using Qtec.AccountManagement.Domain.Dtos;
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
    }
}
