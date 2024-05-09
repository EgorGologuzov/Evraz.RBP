using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using RBP.Services.Contracts;
using RBP.Services.Exceptions;
using RBP.Services.Interfaces;
using RBP.Services.Utils;

namespace RBP.Db.Repositories
{
    public class GeneralRepository<T> : IRepository<T> where T : class
    {
        protected readonly PostgresContext Context;
        protected readonly string TypeName;
        protected readonly DbSet<T> DbSet;
        protected readonly ILogger Logger;
        protected IValidator<T> Validator;

        public GeneralRepository(PostgresContext context, ILogger logger)
        {
            Context = context;
            Logger = logger;
            TypeName = typeof(T).Name;
            DbSet = Context.Set<T>();
        }

        public virtual async Task<T> Create(T entity)
        {
            Validator?.Validate(entity);

            T result = (await DbSet.AddAsync(entity)).Entity;
            await Context.SaveChangesAsync();

            Logger.LogInformation("Created {type} creation data {data}", TypeName, result.ToJson());

            return result;
        }

        public virtual async Task<T> Delete(object id)
        {
            T entity = await Get(id);
            entity.ThrowIfNull(new EntityNotExistsException(id));
            T result = DbSet.Remove(entity).Entity;
            await Context.SaveChangesAsync();

            Logger.LogInformation("Deleted {type} with data {data}", TypeName, result.ToJson());

            return result;
        }

        public virtual async Task<T?> Get(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<bool> IsExists(object id)
        {
            return await Get(id) is not null;
        }

        public virtual async Task<T> Update(object id, object newData)
        {
            T entity = await Get(id);
            entity.ThrowIfNull(new EntityNotExistsException(id));
            string original = entity.ToJson();
            EntityEntry<T> entry = Context.Entry(entity);
            entry.CurrentValues.SetValues(newData);
            T result = entry.Entity;
            Validator?.Validate(result);

            await Context.SaveChangesAsync();

            Logger.LogInformation("Updated {type} old data {oldData}", TypeName, original);

            return result;
        }
    }
}