using RBP.Services.Utils;
using RBP.Services;
using RBP.Web.Models;
using RBP.Web.Services.Interfaces;
using RBP.Web.Dto;

namespace RBP.Web.Services
{
    public class StatementService : ApiServiceBase, IStatementService
    {
        public static readonly List<StatementData> Statements = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Weight = 1340,
                Product = new ProductData
                {
                    Id = Guid.NewGuid(),
                    Name = "КР70, 9 м.",
                    ProfileId = 1,
                    SteelId = 1,
                    PropertiesJson = "[{\"Key\":\"Стандарт\",\"Value\":\"ГОСТ 4121-76\"},{\"Key\":\"Длина\",\"Value\":\"12 м.\"}]",
                    Comment = "Крановый рельс. Применяется при прокладке подкрановых путей, необходимых для работы подъёмных кранов."
                },
                Responsible = new AccountData
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
                Segment = new HandbookEntityData
                {
                    Id = 1,
                    Name = "Разгрузка поставок - Приемка"
                },
                Defects = new List<StatementDefectData>
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
                Product = new ProductData
                {
                    Id = Guid.NewGuid(),
                    Name = "Т62, 12.5 м.",
                    ProfileId = 1,
                    SteelId = 2,
                    PropertiesJson = "[{\"Key\":\"Стандарт\",\"Value\":\"ГОСТ 21174-75\"},{\"Key\":\"Длина\",\"Value\":\"12.5 м.\"}]",
                    Comment = "Рельс трамвайный."
                },
                Responsible = new AccountData
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
                Segment = new HandbookEntityData
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

        public async Task<StatementData?> Get(Guid id)
        {
            return Statements.Find(s => s.Id == id);
        }

        public async Task<IList<StatementData>> GetAll(Guid employeeId, DateTime date)
        {
            return new List<StatementData> { Statements[1] };
        }

        public async Task<IList<StatementData>> GetAll(int segmentId, DateTime date)
        {
            return Statements;
        }

        public async Task<IList<StatementData>> GetAll(int segmentId, DateTime date, Guid employeeId)
        {
            return new List<StatementData> { Statements[0] };
        }

        public async Task<StatementData> Create(StatementCreateDto data)
        {
            _logger.LogInformation("Создана ведомость: {data}", data.ToJson());

            return Statements[0];
        }

        public async Task<StatementData> Delete(Guid id)
        {
            return Statements.Find(s => s.Id == id);
        }
    }
}