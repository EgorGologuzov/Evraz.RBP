using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBP.API.Utils;
using RBP.Services.Contracts;
using RBP.Services.Dto;
using RBP.Services.Exceptions;
using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Services.Utils;

namespace RBP.API.Controllers
{
    [ApiController]
    [Route("Account")]
    [Authorize]
    public class AccountController : GeneralController
    {
        public IAccountRepository Repository { get; }

        public AccountController(ILogger<AccountController> logger, IMapper mapper, IAccountRepository repository) : base(logger, mapper)
        {
            Repository = repository;
        }

        [HttpGet("Get/{id}")]
        public Task<IActionResult> Get(Guid id)
        {
            return TryResult(async () =>
            {
                Account entity = await Repository.Get(id);
                entity.ThrowEntityNotExistsIfNull(id);
                AccountReturnDto dto = Mapper.Map<AccountReturnDto>(entity);

                return Ok(dto);
            });
        }

        [HttpPost("Create")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> Create([FromBody] AccountCreateDto data)
        {
            return TryResult(async () =>
            {
                Account entity = Mapper.Map<Account>(data);
                string password = ApiUtils.GeneratePassword();
                entity.PasswordHash = password.ToSha256Hash();
                entity.CreationTime = DateTime.Now;
                entity.IsActive = true;

                entity = await Repository.Create(entity);

                AccountSecrets secrets = new()
                {
                    Id = entity.Id,
                    Phone = entity.Phone,
                    Password = password
                };

                return Ok(secrets);
            });
        }

        [HttpPut("Update")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> Update([FromBody] AccountUpdateDto data)
        {
            return TryResult(async () =>
            {
                Account entity = await Repository.Update(data.Id, data);
                AccountReturnDto dto = Mapper.Map<AccountReturnDto>(entity);

                return Ok(dto);
            });
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> Delete(Guid id)
        {
            return TryResult(async () => Ok(await Repository.Delete(id)));
        }

        [HttpGet("Find/{role}/{name}")]
        public Task<IActionResult> Find(string role, string name)
        {
            return TryResult(async () => Ok(await Repository.Find(name, role)));
        }

        [HttpGet("GetAll/{role}")]
        public Task<IActionResult> GetAll(string role)
        {
            return TryResult(async () => Ok(await Repository.GetAll(role)));
        }

        [HttpPut("ResetPassword")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> ResetPassword([FromBody] PasswordResetDto data)
        {
            return TryResult(async () =>
            {
                await Repository.ResetPassword(data.AccountId, data.NewPassword);

                return Ok();
            });
        }

        [HttpPut("UpdatePassword")]
        public Task<IActionResult> UpdatePassword([FromBody] PasswordResetDto data)
        {
            return TryResult(async () =>
            {
                Account account = await Repository.Get(data.AccountId);
                account.ThrowEntityNotExistsIfNull(data.AccountId);
                account.PasswordHash.CheckIsEqual(data.OldPassword?.ToSha256Hash(), nameof(data.OldPassword));
                await Repository.ResetPassword(data.AccountId, data.NewPassword);

                return Ok();
            });
        }
    }
}