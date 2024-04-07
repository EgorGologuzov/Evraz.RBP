using RBP.Web.Dto;
using RBP.Web.Models;

namespace RBP.Web.Services.Interfaces
{
    public interface IProductService : IApiService
    {
        Task<ProductData?> Get(Guid id);
        Task<IList<ProductData>> GetAll();
        Task<IList<ProductData>> Find(string name);
        Task<ProductData> Create(ProductCreateDto data);
        Task<ProductData> Update(ProductUpdateDto data);
        Task<ProductData> Delete(Guid id);
    }
}