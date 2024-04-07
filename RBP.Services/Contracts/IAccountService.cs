using RBP.Services.Models;

namespace RBP.Services.Contracts
{
    public interface IAccountService
    {
        Task<Account> Get(string id);
        Task<Account> Create(Account account);
        Task<Account> Update(Account account);
        Task<Account> Delete(Account account);
    }
}