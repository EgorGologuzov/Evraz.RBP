using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RBP.Services.Contracts;
using RBP.Services.Interfaces;
using RBP.Services.Models;

namespace RBP.Db.Repositories
{
    public class StatementRepository : GeneralRepository<Statement>, IStatementRepository
    {
        public StatementRepository(PostgresContext context, ILogger<StatementRepository> logger, IValidator<Statement> validator) : base(context, logger)
        {
            Validator = validator;
        }

        public override Task<Statement?> Get(object id)
        {
            var guidId = Guid.Parse(id.ToString());

            return DbSet
                .Where(s => s.Id == guidId)
                .Include(s => s.Defects)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<Statement>> Find(DateTime date, Guid? employeeId, int? segmentId)
        {
            DateTime start = date.Date;
            DateTime end = start.AddDays(1);

            return await DbSet
                .Where(s => (employeeId == null || s.ResponsibleId == employeeId)
                    && (segmentId == null || s.SegmentId == segmentId)
                    && s.Date >= start
                    && s.Date < end)
                .OrderByDescending(s => s.Date)
                .Include(s => s.Defects)
                .ToListAsync();
        }

        public override Task<Statement> Update(object id, object newData)
        {
            throw new InvalidOperationException("ОБновлять данные ведомостей запрещено");
        }
    }
}