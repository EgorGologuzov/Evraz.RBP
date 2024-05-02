using RBP.Services.Dto;

namespace RBP.Web.Services.Interfaces
{
    public interface IAccountService : IApiService
    {
        Task<AccountReturnDto?> Get(Guid id);
        Task<IList<AccountReturnDto>> Find(string? name, string role);
        Task<IList<AccountReturnDto>> GetAll(string role);
        Task<AccountReturnDto> CreateEmployee(EmployeeCreateDto data);
        Task<AccountReturnDto> UpdateEmployee(EmployeeUpdateDto data);
        Task<AccountReturnDto> CreateAdmin(AdminCreateDto data);
        Task<AccountReturnDto> UpdateAdmin(AdminUpdateDto data);
        Task<bool> ResetPassword(Guid userId, string newPassword);
        Task<bool> UpdatePassword(Guid userId, string oldPassword, string newPassword);
    }
}