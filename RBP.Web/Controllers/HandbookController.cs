using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RBP.Services;
using RBP.Web.Dto;
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
        private async Task<HandbookEntityViewModel> BuildViewModel(string title, string handbookName, HandbookEntityData entity)
        {
            return new HandbookEntityViewModel(
                pageTitle: title,
                client: GetClientData(),
                handbook: (await _handbookService.GetAll()).First(h => h.Name == handbookName),
                entity: entity
           );
        }

        [NonAction]
        private async Task<HandbookEntityListViewModel> BuildViewModel(string handbookName, IList<HandbookEntityData> entities)
        {
            HandbookData handbook = (await _handbookService.GetAll()).First(h => h.Name == handbookName);

            return new HandbookEntityListViewModel(
                pageTitle: handbook.Title,
                client: GetClientData(),
                handbook: handbook,
                entities: entities
           );
        }

        [NonAction]
        private async Task<HandbookListViewModel> BuildViewModel(string title, IList<HandbookData> handbooks)
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

            HandbookEntityViewModel model = await BuildViewModel("Создание", handbookName, new HandbookEntityData());

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
                HandbookEntityData result = await _handbookService.Create(data);

                return RedirectToAction(nameof(List), new { data.HandbookName });
            }
            catch (NotOkResponseException ex)
            {
                HandbookEntityData result = _mapper.Map<HandbookEntityData>(data);
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

            HandbookEntityData? entity = await _handbookService.Get(id, handbookName);

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
                HandbookEntityData result = await _handbookService.Update(data);

                return RedirectToAction(nameof(List), new { data.HandbookName });
            }
            catch (NotOkResponseException ex)
            {
                HandbookEntityData result = _mapper.Map<HandbookEntityData>(data);
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

            HandbookEntityData? entity = await _handbookService.Get(id, handbookName);

            if (entity is null)
            {
                HandbookEntityViewModel model = await BuildViewModel("Удаление", handbookName, new HandbookEntityData());
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
                HandbookEntityData result = await _handbookService.Delete(id, handbookName);
                HandbookEntityViewModel model = await BuildViewModel("Удаление", handbookName, result);
                model.OkMessage = "Элемент удален из справочника";

                return View(nameof(Delete), model);
            }
            catch (NotOkResponseException ex)
            {
                HandbookEntityViewModel model = await BuildViewModel("Удаление", handbookName, new HandbookEntityData());
                model.ErrorMessage = ex.Message;

                return View(nameof(Delete), model);
            }
        }
    }
}