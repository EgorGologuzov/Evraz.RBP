using RBP.Services.Dto;

namespace RBP.Web.Services.Interfaces
{
    public interface IStatementService : IApiService
    {
        Task<WebStatementReturnDto?> Get(Guid id);
        Task<IList<WebStatementReturnDto>> GetAll(Guid employeeId, DateTime date);
        Task<IList<WebStatementReturnDto>> GetAll(int segmentId, DateTime date);
        Task<IList<WebStatementReturnDto>> GetAll(int segmentId, DateTime date, Guid employeeId);
        Task<WebStatementReturnDto> Create(WebStatementCreateDto data);
        Task<WebStatementReturnDto> Delete(Guid id);
    }
}