using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RBP.Services.Dto;
using RBP.Services.Static;
using RBP.Web.Models;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;

namespace RBP.Web.Controllers
{
    public class ProductController : GeneralController
    {
        private readonly IProductService _productService;
        private readonly IHandbookService _handbookService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IHandbookService handbookService, IMapper mapper)
        {
            _productService = productService;
            _productService.Client = this;
            _handbookService = handbookService;
            _handbookService.Client = this;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string? searchRequest)
        {
            if (await IsAuthorized() == false)
            {
                return RedirectUnauthorizedAction();
            }

            ProductListViewModel model = await BuildViewModel("Продукция", searchRequest);

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            ProductReturnDto data = new()
            {
                PropertiesJson = "[]"
            };

            ProductViewModel model = await BuildViewModel("Создание", data);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto data)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            try
            {
                ProductReturnDto result = await _productService.Create(data);

                return RedirectToAction(nameof(Index));
            }
            catch (NotOkResponseException ex)
            {
                ProductViewModel model = await BuildViewModel("Создание", data);
                model.ErrorMessage = ex.Message;

                return View(model);
            }
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            ProductReturnDto? data = await _productService.Get(id);

            if (data is null)
            {
                return NotFoundPage();
            }

            ProductViewModel model = await BuildViewModel("Редактирование", data);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateDto data)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            try
            {
                ProductReturnDto result = await _productService.Update(data);

                return RedirectToAction(nameof(Index));
            }
            catch (NotOkResponseException ex)
            {
                ProductViewModel model = await BuildViewModel("Создание", data);
                model.ErrorMessage = ex.Message;

                return View(model);
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            ProductReturnDto? entity = await _productService.Get(id);

            if (entity is null)
            {
                ProductViewModel model = await BuildViewModel("Удаление", new ProductReturnDto());
                model.ErrorMessage = "Продукт не существует";

                return View(model);
            }

            return View(await BuildViewModel("Удаление", entity));
        }

        public async Task<IActionResult> DeleteFinnaly(Guid id)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            try
            {
                ProductReturnDto result = await _productService.Delete(id);
                ProductViewModel model = await BuildViewModel("Удаление", result);
                model.OkMessage = "Продукт удален";

                return View(nameof(Delete), model);
            }
            catch (NotOkResponseException ex)
            {
                ProductViewModel model = await BuildViewModel("Удаление", new ProductReturnDto());
                model.ErrorMessage = ex.Message;

                return View(nameof(Delete), model);
            }
        }

        [NonAction]
        private async Task<ProductListViewModel> BuildViewModel(string title, string searchRequest)
        {
            IList<HandbookEntityReturnDto> profiles = await _handbookService.GetAllProfiles();
            IList<HandbookEntityReturnDto> steels = await _handbookService.GetAllSteels();

            return new ProductListViewModel(
                pageTitle: title,
                client: GetClientData(),
                products: (await _productService.Find(searchRequest))
                    .Select(p => new ProductViewModel(null, null, p, profiles, steels)).ToList(),
                searchRequest: searchRequest
            );
        }

        [NonAction]
        private async Task<ProductViewModel> BuildViewModel(string title, ProductReturnDto data)
        {
            IList<HandbookEntityReturnDto> profiles = await _handbookService.GetAllProfiles();
            IList<HandbookEntityReturnDto> steels = await _handbookService.GetAllSteels();

            return new ProductViewModel(
                pageTitle: title,
                client: GetClientData(),
                product: data,
                allProfiles: profiles,
                allSteels: steels
            );
        }

        [NonAction]
        private async Task<ProductViewModel> BuildViewModel(string title, ProductUpdateDto data)
        {
            IList<HandbookEntityReturnDto> profiles = await _handbookService.GetAllProfiles();
            IList<HandbookEntityReturnDto> steels = await _handbookService.GetAllSteels();

            return new ProductViewModel(
                pageTitle: title,
                client: GetClientData(),
                product: _mapper.Map<ProductReturnDto>(data),
                allProfiles: profiles,
                allSteels: steels
            );
        }

        [NonAction]
        private async Task<ProductViewModel> BuildViewModel(string title, ProductCreateDto data)
        {
            IList<HandbookEntityReturnDto> profiles = await _handbookService.GetAllProfiles();
            IList<HandbookEntityReturnDto> steels = await _handbookService.GetAllSteels();

            return new ProductViewModel(
                pageTitle: title,
                client: GetClientData(),
                product: _mapper.Map<ProductReturnDto>(data),
                allProfiles: profiles,
                allSteels: steels
            );
        }
    }
}