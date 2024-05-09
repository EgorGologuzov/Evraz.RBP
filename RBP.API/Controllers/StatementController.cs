using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RBP.Services.Contracts;
using RBP.Services.Dto;
using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Services.Utils;
using System.Runtime.InteropServices;

namespace RBP.API.Controllers
{
    [ApiController]
    [Route("Statement")]
    [Authorize]
    public class StatementController : GeneralController
    {
        public readonly IStatementRepository Repository;

        public StatementController(
            ILogger<StatementController> logger,
            IMapper mapper,
            IStatementRepository repository) : base(logger, mapper)
        {
            Repository = repository;
        }

        [HttpGet("Get/{id}")]
        public Task<IActionResult> Get(Guid id)
        {
            return TryResult(async () =>
            {
                Statement entity = await Repository.Get(id);
                entity.ThrowEntityNotExistsIfNull(id);

                return Ok(entity);
            });
        }

        [HttpPost("Create")]
        [Authorize(Roles = ClientRoles.Employee)]
        public Task<IActionResult> Create([FromBody] StatementCreateDto data)
        {
            return TryResult(async () =>
            {
                Statement entity = Mapper.Map<Statement>(data);
                entity.Date = DateTime.Now;
                entity.ResponsibleId = ClientId;

                return Ok(await Repository.Create(entity));
            });
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> Delete(Guid id)
        {
            return TryResult(async () => Ok(await Repository.Delete(id)));
        }

        [HttpPost("FindForAdmin")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> FindForAdmin(StatementGetDto request)
        {
            return TryResult(async () => Ok(await Repository.Find(request.Date, request.EmployeeId, request.SegmentId)));
        }

        [HttpPost("FindForEmployee")]
        [Authorize(Roles = ClientRoles.Employee)]
        public Task<IActionResult> FindForEmployee(StatementGetDto request)
        {
            return TryResult(async () => Ok(await Repository.Find(request.Date, ClientId, request.SegmentId)));
        }
    }
}