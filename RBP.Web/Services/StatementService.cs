using RBP.Services.Utils;
using RBP.Services.Models;
using RBP.Web.Services.Interfaces;
using RBP.Services.Dto;
using RBP.Services.Static;

namespace RBP.Web.Services
{
    public class StatementService : ApiServiceBase, IStatementService
    {
        public static readonly List<WebStatementReturnDto> Statements = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Weight = 1340,
                Product = new ProductReturnDto
                {
                    Id = Guid.NewGuid(),
                    Name = "КР70, 9 м.",
                    ProfileId = 1,
                    SteelId = 1,
                    PropertiesJson = "[{\"Key\":\"Стандарт\",\"Value\":\"ГОСТ 4121-76\"},{\"Key\":\"Длина\",\"Value\":\"12 м.\"}]",
                    Comment = "Крановый рельс. Применяется при прокладке подкрановых путей, необходимых для работы подъёмных кранов."
                },
                Responsible = new AccountReturnDto
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
                Segment = new HandbookEntityReturnDto
                {
                    Id = 1,
                    Name = "Разгрузка поставок - Приемка"
                },
                Defects = new List<WebStatementDefectReturnDto>
                {
                    new()
                    {
                        DefectName = "Скол",
                        Size = 23.5674m
                    },
                    new()
                    {
                        DefectName = "Трещина",
                        Size = 3.5674m
                    }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Weight = 300,
                Product = new ProductReturnDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Т62, 12.5 м.",
                    ProfileId = 1,
                    SteelId = 2,
                    PropertiesJson = "[{\"Key\":\"Стандарт\",\"Value\":\"ГОСТ 21174-75\"},{\"Key\":\"Длина\",\"Value\":\"12.5 м.\"}]",
                    Comment = "Рельс трамвайный."
                },
                Responsible = new AccountReturnDto
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
                },
                Segment = new HandbookEntityReturnDto
                {
                    Id = 2,
                    Name = "Разгрузка поставок - Склад"
                },
            },
        };

        private readonly ILogger<StatementService> _logger;

        public StatementService(ILogger<StatementService> logger)
        {
            _logger = logger;
        }

        public async Task<WebStatementReturnDto?> Get(Guid id)
        {
            return Statements.Find(s => s.Id == id);
        }

        public async Task<IList<WebStatementReturnDto>> GetAll(Guid employeeId, DateTime date)
        {
            return new List<WebStatementReturnDto> { Statements[1] };
        }

        public async Task<IList<WebStatementReturnDto>> GetAll(int segmentId, DateTime date)
        {
            return Statements;
        }

        public async Task<IList<WebStatementReturnDto>> GetAll(int segmentId, DateTime date, Guid employeeId)
        {
            return new List<WebStatementReturnDto> { Statements[0] };
        }

        public async Task<WebStatementReturnDto> Create(WebStatementCreateDto data)
        {
            _logger.LogInformation("Создана ведомость: {data}", data.ToJson());

            return Statements[0];
        }

        public async Task<WebStatementReturnDto> Delete(Guid id)
        {
            return Statements.Find(s => s.Id == id);
        }
    }
}