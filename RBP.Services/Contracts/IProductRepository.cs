using RBP.Services.Models;

namespace RBP.Services.Contracts
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IList<Product>> Find(string name);
        Task<IList<Product>> GetAll();
    }
}