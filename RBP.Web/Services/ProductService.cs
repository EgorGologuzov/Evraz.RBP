using RBP.Services.Models;
using RBP.Services.Utils;
using RBP.Services.Dto;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;
using RBP.Web.Properties;
using System.Xml.Linq;
using RBP.Services.Exceptions;

namespace RBP.Web.Services
{
    public class ProductService : ApiServiceBase, IProductService
    {
        public ProductService(HttpClient client, ILogger<ProductService> logger) : base(client, logger)
        {
        }

        public async Task<ProductReturnDto?> Get(Guid id)
        {
            HttpResponseMessage response = await Http.GetAsync($"Product/Get/{id}");

            return response.IsSuccessStatusCode ? await response.FromContent<ProductReturnDto>() : null;
        }

        public async Task<IList<ProductReturnDto>> Find(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await GetAll();
            }

            HttpResponseMessage response = await Http.GetAsync($"Product/Find/{name}");

            return response.IsSuccessStatusCode ? await response.FromContent<IList<ProductReturnDto>>() : new List<ProductReturnDto>();
        }

        public async Task<IList<ProductReturnDto>> GetAll()
        {
            HttpResponseMessage response = await Http.GetAsync("Product/GetAll");

            return response.IsSuccessStatusCode ? await response.FromContent<IList<ProductReturnDto>>() : new List<ProductReturnDto>();
        }

        public Task<ProductReturnDto> Create(ProductCreateDto data)
        {
            return TryResult(
            action: async () =>
            {
                HttpResponseMessage response = await Http.PostAsync("Product/Create", data.ToJsonContent());
                response.ThrowIfUnsuccess();

                return await response.FromContent<ProductReturnDto>();
            },
            unseccessHandler: (data) => data.Exception == nameof(UniquenessViolationException) ? "Продукт с таким именем уже существует" : null);
        }

        public Task<ProductReturnDto> Update(ProductUpdateDto data)
        {
            return TryResult(
            action: async () =>
            {
                HttpResponseMessage response = await Http.PutAsync("Product/Update", data.ToJsonContent());
                response.ThrowIfUnsuccess();

                return await response.FromContent<ProductReturnDto>();
            },
            unseccessHandler: (data) => data.Exception == nameof(UniquenessViolationException) ? "Продукт с таким именем уже существует" : null);
        }

        public Task<ProductReturnDto> Delete(Guid id)
        {
            return TryResult(
            action: async () =>
            {
                HttpResponseMessage response = await Http.DeleteAsync($"Product/Delete/{id}");
                response.ThrowIfUnsuccess();

                return await response.FromContent<ProductReturnDto>();
            },
            unseccessHandler: (data) => data.Exception == nameof(UniquenessViolationException) ? "Продукт нельзя удалить пока существуют связанные сущности" : null);
        }
    }
}