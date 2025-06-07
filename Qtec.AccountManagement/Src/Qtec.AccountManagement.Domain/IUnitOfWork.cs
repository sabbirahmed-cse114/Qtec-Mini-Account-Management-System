using Qtec.AccountManagement.Domain.RepositoryContracts;

namespace Qtec.AccountManagement.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        Task CommitAsync();
    }
}
