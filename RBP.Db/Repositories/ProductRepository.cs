using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RBP.Services.Contracts;
using RBP.Services.Interfaces;
using RBP.Services.Models;
using RBP.Services.Utils;

namespace RBP.Db.Repositories
{
    public class ProductRepository : GeneralRepository<Product>, IProductRepository
    {
        private static readonly MemoryCash<IList<Product>> _memoryCash = new();

        public ProductRepository(PostgresContext context, ILogger<ProductRepository> logger, IValidator<Product> validator) : base(context, logger)
        {
            Validator = validator;
        }

        public async Task<IList<Product>> Find(string name)
        {
            name.CheckNotNull(nameof(name));

            return await DbSet
                .Where(a => EF.Functions.ILike(a.Name, $"{name}%"))
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<IList<Product>> GetAll()
        {
            IList<Product>? result = _memoryCash.Get(nameof(Product));

            if (result is not null)
            {
                return result;
            }

            result = await DbSet.OrderBy(a => a.Name).ToListAsync();
            _memoryCash.Set(nameof(Product), result);

            return result;
        }

        public override Task<Product> Create(Product entity)
        {
            _memoryCash.Remove(nameof(Product));

            return base.Create(entity);
        }

        public override Task<Product> Update(object id, object entity)
        {
            _memoryCash.Remove(nameof(Product));

            return base.Update(id, entity);
        }

        public override Task<Product> Delete(object id)
        {
            _memoryCash.Remove(nameof(Product));

            return base.Delete(id);
        }
    }
}