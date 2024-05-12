using RBP.Services.Utils;
using RBP.Services.Dto;
using RBP.Services.Models;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;
using RBP.Services.Static;
using System.Xml.Linq;
using AutoMapper;
using RBP.Services.Exceptions;
using RBP.Web.Properties;

namespace RBP.Web.Services
{
    public class AccountService : ApiServiceBase, IAccountService
    {
        private readonly IMapper _mapper;

        public AccountService(HttpClient client, ILogger<AccountService> logger, IMapper mapper) : base(client, logger)
        {
            _mapper = mapper;
        }

        public async Task<AccountReturnDto?> Get(Guid id)
        {
            HttpResponseMessage response = await Http.GetAsync($"Account/Get/{id}");

            return response.IsSuccessStatusCode ? await response.FromContent<AccountReturnDto>() : null;
        }

        public async Task<ApiSecrets?> Login(string phone, string password)
        {
            AccountSecrets data = new()
            {
                Phone = phone,
                Password = password
            };

            HttpResponseMessage response = await Http.PostAsync("Auth/Login", data.ToJsonContent());

            return response.IsSuccessStatusCode ? await response.FromContent<ApiSecrets>() : null;
        }

        public async Task<IList<AccountReturnDto>> Find(string? name, string role)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await GetAll(role);
            }

            HttpResponseMessage response = await Http.GetAsync($"Account/Find/{role}/{name}");

            return response.IsSuccessStatusCode ? await response.FromContent<IList<AccountReturnDto>>() : new List<AccountReturnDto>();
        }

        public async Task<IList<AccountReturnDto>> GetAll(string role)
        {
            HttpResponseMessage response = await Http.GetAsync($"Account/GetAll/{role}");

            return response.IsSuccessStatusCode ? await response.FromContent<IList<AccountReturnDto>>() : new List<AccountReturnDto>();
        }

        public Task<AccountSecrets> CreateEmployee(EmployeeCreateDto data)
        {
            EmployeeRoleData roleData = _mapper.Map<EmployeeRoleData>(data);
            AccountCreateDto accountData = _mapper.Map<AccountCreateDto>(data);
            accountData.Role = ClientRoles.Employee;
            accountData.RoleDataJson = roleData.ToJson();

            return TryResult(
            action: async () =>
            {
                HttpResponseMessage response = await Http.PostAsync("Account/Create", accountData.ToJsonContent());
                response.ThrowIfUnsuccess();

                return await response.FromContent<AccountSecrets>();
            },
            unseccessHandler: (data) => data.Exception == nameof(UniquenessViolationException) ? "Аккаунт с таким телефоном уже существует" : data.Message);
        }

        public Task<AccountReturnDto> UpdateEmployee(EmployeeUpdateDto data)
        {
            EmployeeRoleData roleData = _mapper.Map<EmployeeRoleData>(data);
            AccountUpdateDto accountData = _mapper.Map<AccountUpdateDto>(data);
            accountData.RoleDataJson = roleData.ToJson();

            return TryResult(
            action: async () =>
            {
                HttpResponseMessage response = await Http.PutAsync("Account/Update", accountData.ToJsonContent());
                response.ThrowIfUnsuccess();

                return await response.FromContent<AccountReturnDto>();
            },
            unseccessHandler: (data) => data.Exception == nameof(UniquenessViolationException) ? "Аккаунт с таким телефоном уже существует" : data.Message);
        }

        public Task<AccountSecrets> CreateAdmin(AdminCreateDto data)
        {
            AdminRoleData roleData = _mapper.Map<AdminRoleData>(data);
            AccountCreateDto accountData = _mapper.Map<AccountCreateDto>(data);
            accountData.Role = ClientRoles.Admin;
            accountData.RoleDataJson = roleData.ToJson();

            return TryResult(
            action: async () =>
            {
                HttpResponseMessage response = await Http.PostAsync("Account/Create", accountData.ToJsonContent());
                response.ThrowIfUnsuccess();

                return await response.FromContent<AccountSecrets>();
            },
            unseccessHandler: (data) => data.Exception == nameof(UniquenessViolationException) ? "Аккаунт с таким телефоном уже существует" : data.Message);
        }

        public Task<AccountReturnDto> UpdateAdmin(AdminUpdateDto data)
        {
            AdminRoleData roleData = _mapper.Map<AdminRoleData>(data);
            AccountUpdateDto accountData = _mapper.Map<AccountUpdateDto>(data);
            accountData.RoleDataJson = roleData.ToJson();

            return TryResult(
            action: async () =>
            {
                HttpResponseMessage response = await Http.PutAsync("Account/Update", accountData.ToJsonContent());
                response.ThrowIfUnsuccess();

                return await response.FromContent<AccountReturnDto>();
            },
            unseccessHandler: (data) => data.Exception == nameof(UniquenessViolationException) ? "Аккаунт с таким телефоном уже существует" : data.Message);
        }

        public async Task ResetPassword(Guid userId, string newPassword)
        {
            PasswordResetDto data = new()
            {
                AccountId = userId,
                NewPassword = newPassword
            };

            await TryResult<object>(
            action: async () =>
            {
                HttpResponseMessage response = await Http.PutAsync("Account/ResetPassword", data.ToJsonContent());
                response.ThrowIfUnsuccess();

                return null;
            },
            unseccessHandler: (data) => data.Message);
        }

        public async Task UpdatePassword(Guid userId, string oldPassword, string newPassword)
        {
            PasswordResetDto data = new()
            {
                AccountId = userId,
                NewPassword = newPassword,
                OldPassword = oldPassword
            };

            await TryResult<object>(
            action: async () =>
            {
                HttpResponseMessage response = await Http.PutAsync("Account/UpdatePassword", data.ToJsonContent());
                response.ThrowIfUnsuccess();

                return null;
            },
            unseccessHandler: (data) => data.Message);
        }
    }
}