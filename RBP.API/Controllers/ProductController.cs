using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBP.Services.Contracts;
using RBP.Services.Dto;
using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Services.Utils;

namespace RBP.API.Controllers
{
    [ApiController]
    [Route("Product")]
    [Authorize]
    public class ProductController : GeneralController
    {
        public IProductRepository Repository { get; }

        public ProductController(ILogger<ProductController> logger, IMapper mapper, IProductRepository repository) : base(logger, mapper)
        {
            Repository = repository;
        }

        [HttpGet("Get/{id}")]
        public Task<IActionResult> Get(Guid id)
        {
            return TryResult(async () =>
            {
                Product entity = await Repository.Get(id);
                entity.ThrowEntityNotExistsIfNull(id);

                return Ok(entity);
            });
        }

        [HttpPost("Create")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> Create([FromBody] ProductCreateDto data)
        {
            return TryResult(async () =>
            {
                Product entity = Mapper.Map<Product>(data);

                return Ok(await Repository.Create(entity));
            });
        }

        [HttpPut("Update")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> Update([FromBody] ProductUpdateDto data)
        {
            return TryResult(async () => Ok(await Repository.Update(data.Id, data)));
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> Delete(Guid id)
        {
            return TryResult(async () => Ok(await Repository.Delete(id)));
        }

        [HttpGet("Find/{name}")]
        public Task<IActionResult> Find(string name)
        {
            return TryResult(async () => Ok(await Repository.Find(name)));
        }

        [HttpGet("GetAll")]
        public Task<IActionResult> GetAll()
        {
            return TryResult(async () => Ok(await Repository.GetAll()));
        }
    }
}