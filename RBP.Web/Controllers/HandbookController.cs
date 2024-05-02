using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RBP.Services.Dto;
using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Web.Models;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;

namespace RBP.Web.Controllers
{
    public class HandbookController : GeneralController
    {
        private readonly IHandbookService _handbookService;
        private readonly IMapper _mapper;

        public HandbookController(IHandbookService handbookService, IMapper mapper)
        {
            _handbookService = handbookService;
            _handbookService.Client = this;
            _mapper = mapper;
        }

        [NonAction]
        private async Task<HandbookEntityViewModel> BuildViewModel(string title, string handbookName, HandbookEntityReturnDto entity)
        {
            return new HandbookEntityViewModel(
                pageTitle: title,
                client: GetClientData(),
                handbook: (await _handbookService.GetAll()).First(h => h.Name == handbookName),
                entity: entity
           );
        }

        [NonAction]
        private async Task<HandbookEntityListViewModel> BuildViewModel(string handbookName, IList<HandbookEntityReturnDto> entities)
        {
            Handbook handbook = (await _handbookService.GetAll()).First(h => h.Name == handbookName);

            return new HandbookEntityListViewModel(
                pageTitle: handbook.Title,
                client: GetClientData(),
                handbook: handbook,
                entities: entities
           );
        }

        [NonAction]
        private async Task<HandbookListViewModel> BuildViewModel(string title, IList<Handbook> handbooks)
        {
            return new HandbookListViewModel(title, GetClientData(), handbooks);
        }

        public async Task<IActionResult> Index()
        {
            if (await IsAuthorized() == false)
            {
                return RedirectUnauthorizedAction();
            }

            HandbookListViewModel model = await BuildViewModel("Справочники", await _handbookService.GetAll());

            return View(model);
        }

        public async Task<IActionResult> List(string handbookName)
        {
            if (await IsAuthorized() == false)
            {
                return RedirectUnauthorizedAction();
            }

            HandbookEntityListViewModel model = await BuildViewModel(handbookName, await _handbookService.GetAll(handbookName));

            return View(model);
        }

        public async Task<IActionResult> Create(string handbookName)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            HandbookEntityViewModel model = await BuildViewModel("Создание", handbookName, new HandbookEntityReturnDto());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HandbookEntityCreateDto data)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            try
            {
                HandbookEntityReturnDto result = await _handbookService.Create(data);

                return RedirectToAction(nameof(List), new { data.HandbookName });
            }
            catch (NotOkResponseException ex)
            {
                HandbookEntityReturnDto result = _mapper.Map<HandbookEntityReturnDto>(data);
                HandbookEntityViewModel model = await BuildViewModel("Создание", data.HandbookName, result);
                model.ErrorMessage = ex.Message;

                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id, string handbookName)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            HandbookEntityReturnDto? entity = await _handbookService.Get(id, handbookName);

            if (entity is null)
            {
                return NotFoundPage();
            }

            return View(await BuildViewModel("Редактирование", handbookName, entity));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(HandbookEntityUpdateDto data)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            try
            {
                HandbookEntityReturnDto result = await _handbookService.Update(data);

                return RedirectToAction(nameof(List), new { data.HandbookName });
            }
            catch (NotOkResponseException ex)
            {
                HandbookEntityReturnDto result = _mapper.Map<HandbookEntityReturnDto>(data);
                HandbookEntityViewModel model = await BuildViewModel("Редактирование", data.HandbookName, result);
                model.ErrorMessage = ex.Message;

                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id, string handbookName)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            HandbookEntityReturnDto? entity = await _handbookService.Get(id, handbookName);

            if (entity is null)
            {
                HandbookEntityViewModel model = await BuildViewModel("Удаление", handbookName, new HandbookEntityReturnDto());
                model.ErrorMessage = "Элемент не существует";

                return View(model);
            }

            return View(await BuildViewModel("Удаление", handbookName, entity));
        }

        public async Task<IActionResult> DeleteFinnaly(int id, string handbookName)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            try
            {
                HandbookEntityReturnDto result = await _handbookService.Delete(id, handbookName);
                HandbookEntityViewModel model = await BuildViewModel("Удаление", handbookName, result);
                model.OkMessage = "Элемент удален из справочника";

                return View(nameof(Delete), model);
            }
            catch (NotOkResponseException ex)
            {
                HandbookEntityViewModel model = await BuildViewModel("Удаление", handbookName, new HandbookEntityReturnDto());
                model.ErrorMessage = ex.Message;

                return View(nameof(Delete), model);
            }
        }
    }
}