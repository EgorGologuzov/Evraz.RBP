using RBP.Services.Utils;
using RBP.Services.Dto;
using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;
using RBP.Web.Properties;
using RBP.Services.Exceptions;

namespace RBP.Web.Services
{
    public class HandbookService : ApiServiceBase, IHandbookService
    {
        public HandbookService(HttpClient client, ILogger<HandbookService> logger) : base(client, logger)
        {
        }

        public async Task<IList<Handbook>> GetAll() => Handbooks.Config;
        public async Task<Handbook> GetSegmentsHandbook() => Handbooks.Config.Find(h => h.Name == nameof(WorkshopSegment));

        public async Task<HandbookEntityReturnDto?> Get(int id, string handbook)
        {
            HttpResponseMessage response = await Http.GetAsync($"Handbook/Get/{handbook}/{id}");

            return response.IsSuccessStatusCode ? await response.FromContent<HandbookEntityReturnDto>() : null;
        }

        public async Task<IList<HandbookEntityReturnDto>> GetAll(string handbookName)
        {
            HttpResponseMessage response = await Http.GetAsync($"Handbook/GetAll/{handbookName}");

            return response.IsSuccessStatusCode ? await response.FromContent<IList<HandbookEntityReturnDto>>() : new List<HandbookEntityReturnDto>();
        }

        public Task<IList<HandbookEntityReturnDto>> GetAllSegments() => GetAll(nameof(WorkshopSegment));

        public Task<IList<HandbookEntityReturnDto>> GetAllSteels() => GetAll(nameof(SteelGrade));

        public Task<IList<HandbookEntityReturnDto>> GetAllProfiles() => GetAll(nameof(RailProfile));

        public Task<IList<HandbookEntityReturnDto>> GetAllDefects() => GetAll(nameof(Defect));

        public Task<HandbookEntityReturnDto> Create(HandbookEntityCreateDto data)
        {
            return TryResult(
            action: async () =>
            {
                HttpResponseMessage response = await Http.PostAsync("Handbook/Create", data.ToJsonContent());
                response.ThrowIfUnsuccess();

                return await response.FromContent<HandbookEntityReturnDto>();
            },
            unseccessHandler: (data) => data.Exception == nameof(UniquenessViolationException) ? "Сущность с таким именем уже существует" : null);
        }

        public Task<HandbookEntityReturnDto> Update(HandbookEntityUpdateDto data)
        {
            return TryResult(
            action: async () =>
            {
                HttpResponseMessage response = await Http.PutAsync("Handbook/Update", data.ToJsonContent());
                response.ThrowIfUnsuccess();

                return await response.FromContent<HandbookEntityReturnDto>();
            },
            unseccessHandler: (data) => data.Exception == nameof(UniquenessViolationException) ? "Сущность с таким именем уже существует" : null);
        }

        public Task<HandbookEntityReturnDto> Delete(int id, string handbook)
        {
            return TryResult(
            action: async () =>
            {
                HttpResponseMessage response = await Http.DeleteAsync($"Handbook/Delete/{handbook}/{id}");
                response.ThrowIfUnsuccess();

                return await response.FromContent<HandbookEntityReturnDto>();
            },
            unseccessHandler: (data) => data.Exception == nameof(UniquenessViolationException) ? "Сущность нельзя удалить пока существуют связанные сущности" : null);
        }
    }
}