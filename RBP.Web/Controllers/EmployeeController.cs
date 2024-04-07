using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RBP.Services;
using RBP.Services.Utils;
using RBP.Web.Dto;
using RBP.Web.Models;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;

namespace RBP.Web.Controllers
{
    public class EmployeeController : GeneralController
    {
        private readonly IHandbookService _handbookService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public EmployeeController(
            IHandbookService handbookService,
            IAccountService accountService,
            IMapper mapper)
        {
            _handbookService = handbookService;
            _handbookService.Client = this;
            _accountService = accountService;
            _accountService.Client = this;
            _mapper = mapper;
        }

        [NonAction]
        private async Task<EmployeeListViewModel> BuildViewModel(string title, string searchRequest)
        {
            IList<HandbookEntityData> segments = await _handbookService.GetAllSegments();

            return new EmployeeListViewModel(title, GetClientData())
            {
                SearchRequest = searchRequest,
                Employees = (await _accountService.Find(searchRequest, ClientRoles.Employee))
                    .Select(e => new EmployeeViewModel(null, null, e, segments)).ToList()
            };
        }

        [NonAction]
        private async Task<EmployeeViewModel> BuildViewModel(string title, object data)
        {
            EmployeeRoleData roleData = _mapper.Map<EmployeeRoleData>(data);
            AccountData accountData = new() { Role = ClientRoles.Employee, RoleDataJson = roleData.ToJson() };
            _mapper.Map(data, accountData);

            return new EmployeeViewModel(title, GetClientData(), accountData, await _handbookService.GetAllSegments());
        }

        [NonAction]
        private async Task<EmployeeViewModel> BuildViewModel(string title, AccountData data)
        {
            return new EmployeeViewModel(title, GetClientData(), data, await _handbookService.GetAllSegments());
        }

        public async Task<IActionResult> Index(string? searchRequest)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            EmployeeListViewModel model = await BuildViewModel("Сотрудники", searchRequest);

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            AccountData data = new()
            {
                Role = ClientRoles.Employee,
                RoleDataJson = new EmployeeRoleData().ToJson()
            };

            EmployeeViewModel model = await BuildViewModel("Создание", data);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateDto data)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            try
            {
                AccountData result = await _accountService.CreateEmployee(data);

                return Redirect("/Employee/Index");
            }
            catch (NotOkResponseException ex)
            {
                EmployeeViewModel model = await BuildViewModel("Создание", data);
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

            AccountData? data = await _accountService.Get(id);

            if (data is null)
            {
                return NotFoundPage();
            }

            EmployeeViewModel model = await BuildViewModel("Редактирование", data);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeUpdateDto data)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            try
            {
                AccountData result = await _accountService.UpdateEmployee(data);

                return RedirectToAction(nameof(Index), new { SearchRequest = string.Empty });
            }
            catch (NotOkResponseException ex)
            {
                EmployeeViewModel model = await BuildViewModel("Создание", data);
                model.ErrorMessage = ex.Message;

                return View(model);
            }
        }
    }
}