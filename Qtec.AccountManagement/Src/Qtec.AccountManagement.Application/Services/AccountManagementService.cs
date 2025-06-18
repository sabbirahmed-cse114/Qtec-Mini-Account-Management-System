using Qtec.AccountManagement.Domain;
using Qtec.AccountManagement.Domain.Dtos;
using Qtec.AccountManagement.Domain.Entities;

namespace Qtec.AccountManagement.Application.Services
{
    public class AccountManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public AccountManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAccountAsync(string name, string type, Guid? parentId)
        {
            if ((string.IsNullOrWhiteSpace(name)) || (string.IsNullOrWhiteSpace(type)))
                return false;
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


        public Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            return _unitOfWork.Accounts.GetAllAccountAsync();
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
    }
}
