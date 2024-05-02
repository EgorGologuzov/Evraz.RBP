using RBP.Services.Dto;
using RBP.Services.Models;

namespace RBP.Web.Services.Interfaces
{
    public interface IHandbookService : IApiService
    {
        Task<HandbookEntityReturnDto?> Get(int id, string handbook);
        Task<IList<Handbook>> GetAll();
        Task<IList<HandbookEntityReturnDto>> GetAll(string handbookName);
        Task<Handbook> GetSegmentsHandbook();
        Task<IList<HandbookEntityReturnDto>> GetAllSegments();
        Task<IList<HandbookEntityReturnDto>> GetAllSteels();
        Task<IList<HandbookEntityReturnDto>> GetAllProfiles();
        Task<IList<HandbookEntityReturnDto>> GetAllDefects();
        Task<HandbookEntityReturnDto> Create(HandbookEntityCreateDto data);
        Task<HandbookEntityReturnDto> Update(HandbookEntityUpdateDto data);
        Task<HandbookEntityReturnDto> Delete(int id, string handbook);
    }
}