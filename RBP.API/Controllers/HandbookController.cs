using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBP.Services.Contracts;
using RBP.Services.Dto;
using RBP.Services.Static;
using RBP.Services.Utils;

namespace RBP.API.Controllers
{
    [ApiController]
    [Route("Handbook")]
    [Authorize]
    public class HandbookController : GeneralController
    {
        public IHandbookRepository Repository { get; }

        public HandbookController(ILogger<HandbookController> logger, IMapper mapper, IHandbookRepository repository) : base(logger, mapper)
        {
            Repository = repository;
        }

        [HttpGet("Get/{handbook}/{id}")]
        public Task<IActionResult> Get(string handbook, int id)
        {
            return TryResult(async () =>
            {
                HandbookEntityReturnDto entity = await Repository.Get(handbook, id);
                entity.ThrowEntityNotExistsIfNull(id);

                return Ok(entity);
            });
        }

        [HttpPost("Create")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> Create([FromBody] HandbookEntityCreateDto data)
        {
            return TryResult(async () => Ok(await Repository.Create(data)));
        }

        [HttpPut("Update")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> Update([FromBody] HandbookEntityUpdateDto data)
        {
            return TryResult(async () => Ok(await Repository.Update(data)));
        }

        [HttpDelete("Delete/{handbook}/{id}")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> Delete(string handbook, int id)
        {
            return TryResult(async () => Ok(await Repository.Delete(handbook, id)));
        }

        [HttpGet("GetAll/{handbook}")]
        public Task<IActionResult> GetAll(string handbook)
        {
            return TryResult(async () => Ok(await Repository.GetAll(handbook)));
        }
    }
}