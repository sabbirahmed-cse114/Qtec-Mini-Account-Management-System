using Qtec.AccountManagement.Domain;
using Qtec.AccountManagement.Domain.Dtos;
using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Application.Services
{
    public class AccountManagementService : IAccountManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAccountAsync(string name, string type, Guid? parentId)
        {
            if (await _unitOfWork.Accounts.IsAccountTakenAsync(name, parentId))
                return false;

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = name,
                Type = type,
                ParentId = parentId
            };

            await _unitOfWork.Accounts.CreateAccountAsync(account);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            return await _unitOfWork.Accounts.GetAllAccountAsync();
        }

        public async Task<List<AccountDto>> GetTreeAsync()
        {
            var accounts = (await _unitOfWork.Accounts.GetAllAccountAsync()).ToList();

            var dict = new Dictionary<Guid, AccountDto>();
            var roots = new List<AccountDto>();

            foreach (var acc in accounts)
            {
                dict[acc.Id] = acc;
            }

            foreach (var acc in accounts)
            {
                if (acc.ParentId.HasValue && dict.ContainsKey(acc.ParentId.Value))
                {
                    var parent = dict[acc.ParentId.Value];
                    acc.Level = parent.Level + 1;
                    parent.Children.Add(acc);
                }
                else
                {
                    roots.Add(acc);
                }
            }
            return roots;
        }

        public async Task<AccountDto?> GetAccountByIdAsync(Guid id)
        {
            return await _unitOfWork.Accounts.GetAccountByIdAsync(id);
        }

        public async Task<bool> UpdateAccountAsync(Guid id, string name, string type, string? parentName)
        {
            var result = await _unitOfWork.Accounts.GetAccountByIdAsync(id);
            if (result == null)
                return false;
            var parentId = await _unitOfWork.Accounts.GetParentIdByParentNameAsync(parentName);

            var account = new Account
            {
                Id = id,
                Name = name,
                Type = type,
                ParentId = parentId,
            };
            await _unitOfWork.Accounts.UpdateAccountAsync(account);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> DeleteAccountAsync(Guid Id)
        {
            if (Id == Guid.Empty)
                return false;
            await _unitOfWork.Accounts.DeleteAccountAsync(Id);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}