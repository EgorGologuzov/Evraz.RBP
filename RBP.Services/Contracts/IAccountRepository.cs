using RBP.Services.Dto;
using RBP.Services.Models;

namespace RBP.Services.Contracts
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account?> Get(string phone);
        Task<IList<Account>> Find(string? name, string role);
        Task<IList<Account>> GetAll(string role);
        Task ResetPassword(Guid userId, string newPassword);
    }
}