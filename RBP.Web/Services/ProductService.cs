using RBP.Services.Models;
using RBP.Services.Utils;
using RBP.Services.Dto;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;

namespace RBP.Web.Services
{
    public class ProductService : ApiServiceBase, IProductService
    {
        public static readonly List<ProductReturnDto> Products = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "КР70, 9 м.",
                ProfileId = 1,
                SteelId = 1,
                PropertiesJson = "[{\"Key\":\"Стандарт\",\"Value\":\"ГОСТ 4121-76\"},{\"Key\":\"Длина\",\"Value\":\"12 м.\"}]",
                Comment = "Крановый рельс. Применяется при прокладке подкрановых путей, необходимых для работы подъёмных кранов."
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Т62, 12.5 м.",
                ProfileId = 1,
                SteelId = 2,
                PropertiesJson = "[{\"Key\":\"Стандарт\",\"Value\":\"ГОСТ 21174-75\"},{\"Key\":\"Длина\",\"Value\":\"12.5 м.\"}]",
                Comment = "Рельс трамвайный."
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Р-18",
                ProfileId = 2,
                SteelId = 1,
                PropertiesJson = "[{\"Key\":\"Стандарт\",\"Value\":\"ГОСТ 6368-82\"},{\"Key\":\"Длина\",\"Value\":\"9 м.\"}]",
                Comment = "Рельсы железнодорожные Р-18 предназначены для укладки на железных дорогах узкой колеи и подземных путях шахт."
            }
        };

        private readonly ILogger<ProductService> _logger;

        public ProductService(ILogger<ProductService> logger)
        {
            _logger = logger;
        }

        public async Task<ProductReturnDto?> Get(Guid id)
        {
            return Products.Find(p => p.Id == id);
        }

        public async Task<IList<ProductReturnDto>> Find(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await GetAll();
            }

            return Products.Where(p => p.Name.Contains(name)).ToList();
        }

        public async Task<IList<ProductReturnDto>> GetAll()
        {
            return Products;
        }

        public async Task<ProductReturnDto> Create(ProductCreateDto data)
        {
            ProductReturnDto? product = Products.Find(p => p.Name == data.Name);

            if (product is not null)
            {
                throw new NotOkResponseException("Продукт с таким именм уже существует");
            }

            _logger.LogInformation("Создан продукт: {data}", data.ToJson());

            return Products[0];
        }

        public async Task<ProductReturnDto> Update(ProductUpdateDto data)
        {
            ProductReturnDto? product = Products.Find(p => p.Id == data.Id);

            if (product is null)
            {
                throw new NotOkResponseException("Продукта не существует");
            }

            _logger.LogInformation("Обновлены данные продукта: {data}", data.ToJson());

            return product;
        }

        public async Task<ProductReturnDto> Delete(Guid id)
        {
            ProductReturnDto? product = Products.Find(p => p.Id == id);

            if (product is null)
            {
                throw new NotOkResponseException("Продукта не существует");
            }

            _logger.LogInformation("Удален продукт: {id}", id);

            return product;
        }
    }
}