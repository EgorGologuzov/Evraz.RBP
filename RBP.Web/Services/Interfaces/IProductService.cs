using RBP.Services.Dto;

namespace RBP.Web.Services.Interfaces
{
    public interface IProductService : IApiService
    {
        Task<ProductReturnDto?> Get(Guid id);
        Task<IList<ProductReturnDto>> GetAll();
        Task<IList<ProductReturnDto>> Find(string name);
        Task<ProductReturnDto> Create(ProductCreateDto data);
        Task<ProductReturnDto> Update(ProductUpdateDto data);
        Task<ProductReturnDto> Delete(Guid id);
    }
}