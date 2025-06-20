using Qtec.AccountManagement.Domain.RepositoryContracts;

namespace Qtec.AccountManagement.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IAccountRepository Accounts { get; }
        Task CommitAsync();
    }
}