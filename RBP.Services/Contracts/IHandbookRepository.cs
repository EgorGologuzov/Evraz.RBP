using RBP.Services.Dto;

namespace RBP.Services.Contracts
{
    public interface IHandbookRepository
    {
        Task<HandbookEntityReturnDto> Get(string handbook, object id);
        Task<HandbookEntityReturnDto> Create(HandbookEntityCreateDto data);
        Task<HandbookEntityReturnDto> Update(HandbookEntityUpdateDto data);
        Task<HandbookEntityReturnDto> Delete(string handbook, object id);
        Task<IList<HandbookEntityReturnDto>> GetAll(string handbook);
    }
}