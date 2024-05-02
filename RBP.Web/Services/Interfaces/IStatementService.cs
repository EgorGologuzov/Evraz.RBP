using RBP.Services.Dto;

namespace RBP.Web.Services.Interfaces
{
    public interface IStatementService : IApiService
    {
        Task<StatementReturnDto?> Get(Guid id);
        Task<IList<StatementReturnDto>> GetAll(Guid employeeId, DateTime date);
        Task<IList<StatementReturnDto>> GetAll(int segmentId, DateTime date);
        Task<IList<StatementReturnDto>> GetAll(int segmentId, DateTime date, Guid employeeId);
        Task<StatementReturnDto> Create(StatementCreateDto data);
        Task<StatementReturnDto> Delete(Guid id);
    }
}