using RBP.Services.Utils;
using RBP.Services.Models;
using RBP.Web.Services.Interfaces;
using RBP.Services.Dto;
using RBP.Services.Static;
using AutoMapper;
using System.Net;
using System.Runtime.InteropServices;
using RBP.Services.Exceptions;

namespace RBP.Web.Services
{
    public class StatementService : ApiServiceBase, IStatementService
    {
        private readonly IAccountService _accountService;
        private readonly IHandbookService _handbookService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public StatementService(
            HttpClient client,
            ILogger<StatementService> logger,
            IAccountService accountService,
            IHandbookService handbookService,
            IProductService productService,
            IMapper mapper) : base(client, logger)
        {
            _accountService = accountService;
            _handbookService = handbookService;
            _productService = productService;
            _mapper = mapper;
        }

        private async Task<WebStatementReturnDto> Convert(StatementReturnDto data)
        {
            WebStatementReturnDto result = _mapper.Map<WebStatementReturnDto>(data);
            result.Product = await _productService.Get(data.ProductId);
            result.Responsible = await _accountService.Get(data.ResponsibleId);
            result.Segment = await _handbookService.Get(data.SegmentId, nameof(WorkshopSegment));
            IList<HandbookEntityReturnDto> allDefects = await _handbookService.GetAll(nameof(Defect));
            result.Defects = data.Defects.Select(d => new WebStatementDefectReturnDto
            {
                DefectName = allDefects.First(he => he.Id == d.DefectId).Name,
                Size = d.Size
            }).ToList();

            return result;
        }

        private async Task<IList<WebStatementReturnDto>> Convert(IList<StatementReturnDto> list)
        {
            IList<ProductReturnDto> products = await _productService.GetAll();
            IList<AccountReturnDto> employees = await _accountService.GetAll(ClientRoles.Employee);
            IList<HandbookEntityReturnDto> segments = await _handbookService.GetAll(nameof(WorkshopSegment));
            IList<HandbookEntityReturnDto> defects = await _handbookService.GetAll(nameof(Defect));
            List<WebStatementReturnDto> result = new();

            foreach (var statement in list)
            {
                WebStatementReturnDto webStatement  = _mapper.Map<WebStatementReturnDto>(statement);
                webStatement.Product = products.First(p => p.Id == statement.ProductId);
                webStatement.Responsible = employees.First(e => e.Id == statement.ResponsibleId);
                webStatement.Segment = segments.First(segment => segment.Id == statement.SegmentId);
                webStatement.Defects = statement.Defects.Select(d => new WebStatementDefectReturnDto
                {
                    DefectName = defects.First(he => he.Id == d.DefectId).Name,
                    Size = d.Size
                }).ToList();
                result.Add(webStatement);
            }

            return result;
        }

        public async Task<WebStatementReturnDto?> Get(Guid id)
        {
            HttpResponseMessage response = await Http.GetAsync($"Statement/Get/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            StatementReturnDto data = await response.FromContent<StatementReturnDto>();

            return await Convert(data);
        }

        public Task<IList<WebStatementReturnDto>> GetAll(Guid employeeId, DateTime date)
        {
            return GetAll(null, employeeId, date);
        }

        public Task<IList<WebStatementReturnDto>> GetAll(int segmentId, DateTime date)
        {
            return GetAll(segmentId, null, date);
        }

        public Task<IList<WebStatementReturnDto>> GetAll(int segmentId, DateTime date, Guid employeeId)
        {
            return GetAll(segmentId, employeeId, date);
        }

        public async Task<IList<WebStatementReturnDto>> GetAll(int? segmentId, Guid? employeeId, DateTime date)
        {
            StatementGetDto data = new() { EmployeeId = employeeId, Date = date, SegmentId = segmentId };
            HttpResponseMessage response = await Http.PostAsync("Statement/FindForEmployee", data.ToJsonContent());

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                response = await Http.PostAsync("Statement/FindForAdmin", data.ToJsonContent());
            }

            if (!response.IsSuccessStatusCode)
            {
                return new List<WebStatementReturnDto>();
            }

            IList<StatementReturnDto> result = await response.FromContent<IList<StatementReturnDto>>();

            return await Convert(result);
        }

        public async Task<WebStatementReturnDto> Create(WebStatementCreateDto data)
        {
            StatementCreateDto createDto = _mapper.Map<StatementCreateDto>(data);
            IList<WebStatementDefectReturnDto> defects = data.DefectsJson.FromJson<IList<WebStatementDefectReturnDto>>();
            IList<HandbookEntityReturnDto> allDefects = await _handbookService.GetAll(nameof(Defect));
            createDto.Defects = defects.Select(d => new StatementDefectReturnDto
            {
                DefectId = allDefects.First(he => he.Name == d.DefectName).Id,
                Size = d.Size
            }).ToList();

            return await TryResult(
                action: async () =>
            {
                HttpResponseMessage response = await Http.PostAsync("Statement/Create", createDto.ToJsonContent());
                response.ThrowIfUnsuccess();
                StatementReturnDto result = await response.FromContent<StatementReturnDto>();

                return await Convert(result);
            },
            unseccessHandler: (data) => data.Message);
        }

        public Task<WebStatementReturnDto> Delete(Guid id)
        {
            return TryResult(
            action: async () =>
            {
                HttpResponseMessage response = await Http.DeleteAsync($"Statement/Delete/{id}");
                response.ThrowIfUnsuccess();
                StatementReturnDto result = await response.FromContent<StatementReturnDto>();

                return await Convert(result);
            },
            unseccessHandler: (data) => data.Message);
        }
    }
}