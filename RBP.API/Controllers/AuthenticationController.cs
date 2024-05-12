using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RBP.API.Utils;
using RBP.Services.Contracts;
using RBP.Services.Dto;
using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Services.Utils;

namespace RBP.API.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthenticationController : GeneralController
    {
        private readonly TimeSpan _tokenExpirationTimeSpan = TimeSpan.FromHours(1);
        private readonly TimeSpan _recommendedRefreshTokenTimeSpan = TimeSpan.FromMinutes(50);
        private readonly JWTSettings _options;

        protected readonly IAccountRepository Repository;

        public AuthenticationController(ILogger<AuthenticationController> logger, IMapper mapper, IAccountRepository repository, IOptions<JWTSettings> options) : base(logger, mapper)
        {
            Repository = repository;
            _options = options.Value;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> GetToken([FromBody] AccountSecrets secrets)
        {
            Account? account = await Repository.Get(secrets.Phone ?? string.Empty);

            if (account is null || account.PasswordHash != secrets.Password?.ToSha256Hash() || !account.IsActive)
            {
                return Forbid();
            }

            List<Claim> claims = new()
            {
                new(ClaimTypes.Role, account.Role),
                new(ClaimTypes.PrimarySid, account.Id.ToString())
            };

            ApiSecrets apiSecrets = new()
            {
                Token = GenerateToken(claims),
                TokenExpirationTime = DateTime.UtcNow.Add(_tokenExpirationTimeSpan),
                RecommendedRefreshTokenTime = DateTime.UtcNow.Add(_recommendedRefreshTokenTimeSpan),
                Account = Mapper.Map<AccountReturnDto>(account)
            };

            return Ok(apiSecrets);
        }

        [HttpPost("Refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            Account? account = await Repository.Get(Guid.Parse(GetClaimValue(ClaimTypes.PrimarySid)));

            List<Claim> claims = new()
            {
                new(ClaimTypes.Role, account.Role),
                new(ClaimTypes.PrimarySid, account.Id.ToString())
            };

            ApiSecrets apiSecrets = new()
            {
                Token = GenerateToken(claims),
                TokenExpirationTime = DateTime.UtcNow.Add(_tokenExpirationTimeSpan),
                RecommendedRefreshTokenTime = DateTime.UtcNow.Add(_recommendedRefreshTokenTimeSpan),
                Account = Mapper.Map<AccountReturnDto>(account)
            };

            return Ok(apiSecrets);
        }

        [NonAction]
        public string GenerateToken(IList<Claim> claims)
        {
            SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(_options.SecretKey));

            JwtSecurityToken token =
                new(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.Add(_tokenExpirationTimeSpan),
                    signingCredentials: new(signingKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}