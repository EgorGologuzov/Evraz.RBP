using RBP.Services.Utils;
using RBP.Web.Dto;
using RBP.Web.Models;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;

namespace RBP.Web.Services
{
    public class HandbookService : ApiServiceBase, IHandbookService
    {
        public static readonly Dictionary<string, List<HandbookEntityData>> Handbooks = new()
        {
            {
                "RailProfile",
                new()
                {
                    new()
                    {
                        Id = 1,
                        Name = "Т58",
                        Comment = "Рельс трамвайный"
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Р65",
                        Comment = "Рельс железнодорожный"
                    },
                }
            },
            {
                "Defect",
                new()
                {
                    new()
                    {
                        Id = 1,
                        Name = "Скол",
                        Comment = "Нарушение геометричесокй формы профиля с острыми краями диаметром более 2 мм."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Трещина",
                        Comment = "Области с полностью нарушенными межатомными связями и частично нарушенными межатомными связями."
                    },
                }
            },
            {
                "SteelGrade",
                new()
                {
                    new()
                    {
                        Id = 1,
                        Name = "ГОСТ 1050-2013",
                        Comment = ""
                    },
                    new()
                    {
                        Id = 2,
                        Name = "ГОСТ 1050-88"
                    },
                }
            },
            {
                "WorkshopSegment",
                new()
                {
                    new()
                    {
                        Id = 1,
                        Name = "Разгрузка поставок - Приемка"
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Разгрузка поставок - Склад"
                    },
                    new()
                    {
                        Id = 3,
                        Name = "Печь - Приемка"
                    },
                }
            },
        };
        
        private readonly ILogger<HandbookService> _logger;

        public HandbookService(ILogger<HandbookService> logger)
        {
            _logger = logger;
        }

        public async Task<HandbookEntityData?> Get(int id, string handbook) => Handbooks[handbook].Find(e => e.Id == id);

        public async Task<IList<HandbookData>> GetAll() => Properties.Handbooks.Config;

        public async Task<HandbookData> GetSegmentsHandbook() => Properties.Handbooks.Config.Find(h => h.Name == "WorkshopSegment");

        public async Task<IList<HandbookEntityData>> GetAll(string handbookName) => Handbooks[handbookName];

        public async Task<IList<HandbookEntityData>> GetAllSegments() => Handbooks["WorkshopSegment"];

        public async Task<IList<HandbookEntityData>> GetAllSteels() => Handbooks["SteelGrade"];

        public async Task<IList<HandbookEntityData>> GetAllProfiles() => Handbooks["RailProfile"];

        public async Task<IList<HandbookEntityData>> GetAllDefects() => Handbooks["Defect"];

        public async Task<HandbookEntityData> Create(HandbookEntityCreateDto data)
        {
            if (Handbooks.ContainsKey(data.HandbookName) == false || Handbooks[data.HandbookName].Find(e => e.Name == data.Name) is not null)
            {
                throw new NotOkResponseException("Уже существует сущность с такими именем");
            }

            _logger.LogInformation("Создан новый элемент справочника {data}", data.ToJson());

            return Handbooks[data.HandbookName][0];
        }

        public async Task<HandbookEntityData> Update(HandbookEntityUpdateDto data)
        {
            if (Handbooks.ContainsKey(data.HandbookName) == false || Handbooks[data.HandbookName].Find(e => e.Id == data.Id) is null)
            {
                throw new NotOkResponseException("Не найдена сущность с такими параметрами");
            }

            _logger.LogInformation("Обновлен справочник {data}", data.ToJson());

            return Handbooks[data.HandbookName].Find(e => e.Id == data.Id);
        }

        public async Task<HandbookEntityData> Delete(int id, string handbook)
        {
            HandbookEntityData? entity = await Get(id, handbook);

            if (entity is null)
            {
                throw new NotOkResponseException("Элемента нет в справочнике");
            }

            if (handbook == "WorkshopSegment" &&
                AccountService.Accounts.Find(a => a.RoleDataJson is not null && a.RoleDataJson.FromJson<EmployeeRoleData>().SegmentId == id) is not null)
            {
                throw new NotOkResponseException("Элемент справочника уже используется, его нельзя удалить");
            }

            _logger.LogInformation("Удален элемент справочника {id}, {handbook}", id, handbook);

            return entity;
        }
    }
}