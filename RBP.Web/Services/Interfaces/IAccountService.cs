using RBP.Services.Dto;
using RBP.Services.Models;

namespace RBP.Web.Services.Interfaces
{
    public interface IAccountService : IApiService
    {
        Task<AccountReturnDto?> Get(Guid id);
        Task<ApiSecrets?> Login(string phone, string password);
        Task<IList<AccountReturnDto>> Find(string? name, string role);
        Task<IList<AccountReturnDto>> GetAll(string role);
        Task<AccountSecrets> CreateEmployee(EmployeeCreateDto data);
        Task<AccountReturnDto> UpdateEmployee(EmployeeUpdateDto data);
        Task<AccountSecrets> CreateAdmin(AdminCreateDto data);
        Task<AccountReturnDto> UpdateAdmin(AdminUpdateDto data);
        Task ResetPassword(Guid userId, string newPassword);
        Task UpdatePassword(Guid userId, string oldPassword, string newPassword);
    }
}