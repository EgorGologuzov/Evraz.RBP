using RBP.Services.Dto;
using RBP.Services.Models;

namespace RBP.Services.Contracts
{
    public interface IStatementRepository : IRepository<Statement>
    {
        Task<IList<Statement>> Find(DateTime date, Guid? employeeId, int? segmentId);
    }
}