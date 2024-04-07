using RBP.Web.Dto;
using RBP.Web.Models;

namespace RBP.Web.Services.Interfaces
{
    public interface IHandbookService : IApiService
    {
        Task<HandbookEntityData?> Get(int id, string handbook);
        Task<IList<HandbookData>> GetAll();
        Task<IList<HandbookEntityData>> GetAll(string handbookName);
        Task<HandbookData> GetSegmentsHandbook();
        Task<IList<HandbookEntityData>> GetAllSegments();
        Task<IList<HandbookEntityData>> GetAllSteels();
        Task<IList<HandbookEntityData>> GetAllProfiles();
        Task<IList<HandbookEntityData>> GetAllDefects();
        Task<HandbookEntityData> Create(HandbookEntityCreateDto data);
        Task<HandbookEntityData> Update(HandbookEntityUpdateDto data);
        Task<HandbookEntityData> Delete(int id, string handbook);
    }
}