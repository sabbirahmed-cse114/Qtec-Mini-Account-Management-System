using Qtec.AccountManagement.Domain;
using Qtec.AccountManagement.Domain.RepositoryContracts;
using Qtec.AccountManagement.Infrastructure.Repositories;
using System.Data.SqlClient;

namespace Qtec.AccountManagement.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;

        public IUserRepository Users { get; }
        public IRoleRepository Roles { get; }
        public UnitOfWork(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            Users = new UserRepository(_connection, _transaction);
            Roles = new RoleRepository(_connection, _transaction);
        }

        public async Task CommitAsync()
        {
            _transaction?.Commit();
            await Task.CompletedTask;
        }
        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Close();
        }
    }
}
