using RBP.Services.Utils;
using RBP.Services.Dto;
using RBP.Services.Models;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;
using RBP.Services.Static;

namespace RBP.Web.Services
{
    public class AccountService : ApiServiceBase, IAccountService
    {
        public static List<AccountReturnDto> Accounts = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Phone = "1111111111",
                Name = "Иванов Иван Иванович",
                Role = ClientRoles.Employee,
                CreationTime = DateTime.Now - TimeSpan.FromHours(10),
                Comment = "Comment text",
                IsActive = true,
                RoleDataJson = new EmployeeRoleData
                {
                    SegmentId = 1,
                    Gender = "М",
                    BirthDate = DateTime.Now - TimeSpan.FromDays(365 * 30),
                    EmploymentDate = DateTime.Now - TimeSpan.FromDays(365 * 10)
                }.ToJson()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Phone = "2222222222",
                Name = "Федотов Федот Федотович",
                Role = ClientRoles.Admin,
                CreationTime = DateTime.Now - TimeSpan.FromHours(10),
                IsActive = true,
                RoleDataJson = new AdminRoleData
                {
                    JobTitle = "Начальник цеха"
                }.ToJson(),
                Comment = "Самый главный в цехе"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Phone = "3333333333",
                Name = "Ахматова Галина Николаевна",
                Role = ClientRoles.Employee,
                CreationTime = DateTime.Now - TimeSpan.FromHours(10),
                IsActive = false,
                RoleDataJson = new EmployeeRoleData
                {
                    SegmentId = 3,
                    Gender = "Ж",
                    BirthDate = DateTime.Now - TimeSpan.FromDays(365 * 35),
                    EmploymentDate = DateTime.Now - TimeSpan.FromDays(365 * 4)
                }.ToJson()
            }
        };

        private readonly ILogger<AccountService> _logger;

        public AccountService(ILogger<AccountService> logger)
        {
            _logger = logger;
        }

        public async Task<AccountReturnDto?> Get(Guid id)
        {
            return Accounts.Find(a => a.Id == id);
        }

        public async Task<IList<AccountReturnDto>> Find(string? name, string role)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await GetAll(role);
            }

            return Accounts.Where(a => a.Name.Contains(name) && a.Role == role).ToList();
        }

        public async Task<IList<AccountReturnDto>> GetAll(string role)
        {
            return Accounts.Where(a => a.Role == role).ToList();
        }

        public async Task<AccountReturnDto> CreateEmployee(EmployeeCreateDto data)
        {
            AccountReturnDto? account = Accounts.Find(a => a.Phone == data.Phone);

            if (account is not null)
            {
                throw new NotOkResponseException("Аккаунт с таким номером уже существует");
            }

            _logger.LogInformation("Создан сотрудник: {data}", data.ToJson());

            return Accounts[0];
        }

        public async Task<AccountReturnDto> UpdateEmployee(EmployeeUpdateDto data)
        {
            AccountReturnDto? account = Accounts.Find(a => a.Phone == data.Phone);

            if (account is null)
            {
                throw new NotOkResponseException("Аккаунт с таким номером не существует");
            }

            _logger.LogInformation("Обновлены данные сотрудника: {data}", data.ToJson());

            return Accounts.Find(a => a.Id == data.Id);
        }

        public async Task<AccountReturnDto> CreateAdmin(AdminCreateDto data)
        {
            AccountReturnDto? account = Accounts.Find(a => a.Phone == data.Phone);

            if (account is not null)
            {
                throw new NotOkResponseException("Аккаунт с таким номером уже существует");
            }

            _logger.LogInformation("Создан администратор: {data}", data.ToJson());

            return Accounts[0];
        }

        public async Task<AccountReturnDto> UpdateAdmin(AdminUpdateDto data)
        {
            AccountReturnDto? account = Accounts.Find(a => a.Phone == data.Phone);

            if (account is null)
            {
                throw new NotOkResponseException("Аккаунт с таким номером не существует");
            }

            _logger.LogInformation("Обновлены данные администратора: {data}", data.ToJson());

            return Accounts.Find(a => a.Id == data.Id);
        }

        public async Task<bool> ResetPassword(Guid userId, string newPassword)
        {
            _logger.LogInformation("Сброшен пароль аккаунта: {id}", userId);

            return true;
        }

        public async Task<bool> UpdatePassword(Guid userId, string oldPassword, string newPassword)
        {
            _logger.LogInformation("Обновлен пароль аккаунта: {id}", userId);

            return true;
        }
    }
}