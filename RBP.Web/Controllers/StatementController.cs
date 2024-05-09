using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RBP.Services.Utils;
using RBP.Services.Dto;
using RBP.Web.Models;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;
using RBP.Services.Static;

namespace RBP.Web.Controllers
{
    public class StatementController : GeneralController
    {
        private readonly IHandbookService _handbookService;
        private readonly IStatementService _statementService;
        private readonly ILogger<StatementController> _logger;
        private readonly IProductService _productService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public StatementController(
            IHandbookService handbookService,
            IStatementService statementService,
            ILogger<StatementController> logger,
            IProductService productService,
            IAccountService accountService,
            IMapper mapper)
        {
            _handbookService = handbookService;
            _handbookService.Client = this;
            _statementService = statementService;
            _statementService.Client = this;
            _logger = logger;
            _productService = productService;
            _productService.Client = this;
            _accountService = accountService;
            _accountService.Client = this;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            if (await IsAuthorized(ClientRoles.Admin, ClientRoles.Employee) == false)
            {
                return RedirectUnauthorizedAction();
            }

            HandbookEntityListViewModel model = await BuildViewModel("Ведомости");

            return View(model);
        }

        public async Task<IActionResult> List(int segmentId, DateTime? date)
        {
            if (await IsAuthorized(ClientRoles.Admin, ClientRoles.Employee) == false)
            {
                return RedirectUnauthorizedAction();
            }

            date ??= DateTime.Now;

            try
            {
                IList<WebStatementReturnDto> data = ClientInRole(ClientRoles.Employee)
                    ? await _statementService.GetAll(segmentId, date.Value, GetClientData().Id)
                    : await _statementService.GetAll(segmentId, date.Value);
                StatementListViewModel model = await BuildViewModel(data, segmentId, date.Value);
                _logger.LogInformation("Запрошен список ведомостей: {id}, {date}", segmentId, date);

                return View(model);
            }
            catch (NotOkResponseException ex)
            {
                StatementListViewModel model = await BuildViewModel(null, segmentId, date.Value);
                model.ErrorMessage = ex.Message;

                return View(model);
            }
        }

        public async Task<IActionResult> EmployeeList(Guid employeeId, DateTime? date)
        {
            if (await IsAuthorized(ClientRoles.Admin, ClientRoles.Employee) == false)
            {
                return RedirectUnauthorizedAction();
            }

            date ??= DateTime.Now;
            AccountReturnDto? employee = await _accountService.Get(employeeId);

            if (employee is null)
            {
                return NotFoundPage();
            }

            try
            {
                IList<WebStatementReturnDto> data = await _statementService.GetAll(employeeId, date.Value);
                StatementListViewModel model = await BuildViewModel(data, employee, date.Value);
                _logger.LogInformation("Запрошен список ведомостей сотрдуника: {id}, {date}", employee.Id, date);

                return View(model);
            }
            catch (NotOkResponseException ex)
            {
                StatementListViewModel model = await BuildViewModel(null, employee, date.Value);
                model.ErrorMessage = ex.Message;

                return View(model);
            }
        }

        public async Task<IActionResult> Create(int? segmentId)
        {
            if (await IsAuthorized(ClientRoles.Employee) == false)
            {
                return RedirectUnauthorizedAction();
            }

            StatementViewModel model = await BuildViewModel(
                "Создание",
                new WebStatementReturnDto
                {
                    Segment = segmentId is null ? null : new HandbookEntityReturnDto { Id = segmentId.Value }
                }
            );

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WebStatementCreateDto data)
        {
            if (await IsAuthorized(ClientRoles.Employee) == false)
            {
                return RedirectUnauthorizedAction();
            }

            try
            {
                WebStatementReturnDto result = await _statementService.Create(data);

                return RedirectToAction(nameof(List), new { data.SegmentId, Date = DateTime.Now });
            }
            catch (NotOkResponseException ex)
            {
                StatementViewModel model = await BuildViewModel("Создание", data);
                model.ErrorMessage = ex.Message;

                return View(model);
            }
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (await IsAuthorized(ClientRoles.Admin, ClientRoles.Employee) == false)
            {
                return RedirectUnauthorizedAction();
            }

            WebStatementReturnDto? data = await _statementService.Get(id);

            if (data is null)
            {
                return NotFoundPage();
            }

            StatementViewModel model = await BuildViewModel("Просмотр", data);

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            WebStatementReturnDto? entity = await _statementService.Get(id);

            if (entity is null)
            {
                return NotFound();
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
                WebStatementReturnDto result = await _statementService.Delete(id);
                StatementViewModel model = await BuildViewModel("Удаление", result);
                model.OkMessage = "Ведомость удалена";

                return View(nameof(Delete), model);
            }
            catch (NotOkResponseException ex)
            {
                StatementViewModel model = await BuildViewModel("Удаление", new WebStatementReturnDto());
                model.ErrorMessage = ex.Message;

                return View(nameof(Delete), model);
            }
        }

        [NonAction]
        private async Task<HandbookEntityListViewModel> BuildViewModel(string title)
        {
            return new HandbookEntityListViewModel(
                pageTitle: title,
                client: GetClientData(),
                handbook: await _handbookService.GetSegmentsHandbook(),
                entities: await _handbookService.GetAllSegments()
           );
        }

        [NonAction]
        private async Task<StatementListViewModel> BuildViewModel(IList<WebStatementReturnDto> data, int segmentId, DateTime date)
        {
            HandbookEntityReturnDto segment = (await _handbookService.GetAllSegments()).First(s => s.Id == segmentId);

            return new StatementListViewModel(
                pageTitle: segment.Name,
                client: GetClientData(),
                segment: segment,
                date: date,
                statements: data.Select(s => new StatementViewModel(null, null, s, null, null, null, null)).ToList(),
                employee: null);
        }

        [NonAction]
        private async Task<StatementListViewModel> BuildViewModel(IList<WebStatementReturnDto> data, AccountReturnDto employee, DateTime date)
        {
            return new StatementListViewModel(
                pageTitle: employee.Name,
                client: GetClientData(),
                segment: null,
                date: date,
                statements: data.Select(s => new StatementViewModel(null, null, s, null, null, null, null)).ToList(),
                employee: employee);
        }

        [NonAction]
        private async Task<StatementViewModel> BuildViewModel(string title, WebStatementReturnDto data)
        {
            Task<IList<ProductReturnDto>> allProducts = _productService.GetAll();
            Task<IList<AccountReturnDto>> allEmployees = _accountService.GetAll(ClientRoles.Employee);
            Task<IList<HandbookEntityReturnDto>> allDefects = _handbookService.GetAllDefects();
            Task<IList<HandbookEntityReturnDto>> allSegments = _handbookService.GetAllSegments();

            await Task.WhenAll(allProducts, allEmployees, allDefects, allSegments);

            return new StatementViewModel(
                pageTitle: title,
                client: GetClientData(),
                statement: data,
                allProducts: allProducts.Result,
                allEmployees: allEmployees.Result,
                allDefects: allDefects.Result,
                allSegments: allSegments.Result);
        }

        [NonAction]
        private async Task<StatementViewModel> BuildViewModel(string title, WebStatementCreateDto data)
        {
            Task<IList<ProductReturnDto>> allProducts = _productService.GetAll();
            Task<IList<AccountReturnDto>> allEmployees = _accountService.GetAll(ClientRoles.Employee);
            Task<IList<HandbookEntityReturnDto>> allDefects = _handbookService.GetAllDefects();
            Task<IList<HandbookEntityReturnDto>> allSegments = _handbookService.GetAllSegments();

            await Task.WhenAll(allProducts, allEmployees, allDefects, allSegments);

            return new StatementViewModel(
                pageTitle: title,
                client: GetClientData(),
                statement: _mapper.Map<WebStatementReturnDto>(data),
                allProducts: allProducts.Result,
                allEmployees: allEmployees.Result,
                allDefects: allDefects.Result,
                allSegments: allSegments.Result);
        }
    }
}