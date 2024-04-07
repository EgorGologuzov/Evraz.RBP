using RBP.Web.Dto;
using RBP.Web.Models;

namespace RBP.Web.Services.Interfaces
{
    public interface IStatementService : IApiService
    {
        Task<StatementData?> Get(Guid id);
        Task<IList<StatementData>> GetAll(Guid employeeId, DateTime date);
        Task<IList<StatementData>> GetAll(int segmentId, DateTime date);
        Task<IList<StatementData>> GetAll(int segmentId, DateTime date, Guid employeeId);
        Task<StatementData> Create(StatementCreateDto data);
        Task<StatementData> Delete(Guid id);
    }
}