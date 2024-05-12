using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RBP.Services.Contracts;
using RBP.Services.Interfaces;
using RBP.Services.Models;
using RBP.Services.Utils;

namespace RBP.Db.Repositories
{
    public class AccountRepository : GeneralRepository<Account>, IAccountRepository
    {
        public AccountRepository(PostgresContext context, ILogger<AccountRepository> logger, IValidator<Account> validator) : base(context, logger)
        {
            Validator = validator;
        }

        public async Task<IList<Account>> Find(string? name, string role)
        {
            return await DbSet
                .Where(a => EF.Functions.ILike(a.Name, $"{name}%") && a.Role == role)
                .OrderByDescending(a => a.CreationTime)
                .ToListAsync();
        }

        public Task<Account?> Get(string phone)
        {
            return DbSet.Where(a => a.Phone == phone).FirstOrDefaultAsync();
        }

        public async Task<IList<Account>> GetAll(string role)
        {
            return await DbSet
                .Where(a => a.Role == role)
                .OrderByDescending(a => a.CreationTime)
                .ToListAsync();
        }

        public async Task ResetPassword(Guid userId, string newPassword)
        {
            Account account = await Get(userId);
            newPassword.CheckPassword(nameof(newPassword));
            account.PasswordHash = newPassword.ToSha256Hash();
            await Context.SaveChangesAsync();

            Logger.LogInformation("Password updated {userId}", userId);
        }
    }
}