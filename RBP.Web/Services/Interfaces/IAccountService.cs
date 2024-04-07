using RBP.Web.Dto;
using RBP.Web.Models;

namespace RBP.Web.Services.Interfaces
{
    public interface IAccountService : IApiService
    {
        Task<AccountData?> Get(Guid id);
        Task<IList<AccountData>> Find(string? name, string role);
        Task<IList<AccountData>> GetAll(string role);
        Task<AccountData> CreateEmployee(EmployeeCreateDto data);
        Task<AccountData> UpdateEmployee(EmployeeUpdateDto data);
        Task<AccountData> CreateAdmin(AdminCreateDto data);
        Task<AccountData> UpdateAdmin(AdminUpdateDto data);
        Task<bool> ResetPassword(Guid userId, string newPassword);
        Task<bool> UpdatePassword(Guid userId, string oldPassword, string newPassword);
    }
}