using System.Diagnostics;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RBP.Services;
using RBP.Services.Utils;
using RBP.Services.Dto;
using RBP.Services.Models;
using RBP.Web.Models;
using RBP.Web.Services;
using RBP.Web.Services.Interfaces;

namespace RBP.Web.Controllers
{
    public class GeneralController : Controller, IApiClient
    {
        private const string _apiAuthenticationDataSessionKey = "ApiAuthenticationData";
        private const string _accountAuthenticationDataSessionKey = "AccountAuthenticationData";
        private const string _loginPagePath = "/Account/Login";

        private AccountReturnDto? _accountData;
        private ApiSecrets? _apiData;
        private Func<IActionResult> _unauthorizedAction;

        public ApiSecrets? ApiData
        {
            get => GetApiData();
            set => SetApiData(value);
        }

        [NonAction]
        protected AccountReturnDto? GetClientData()
        {
            if (_accountData is not null)
            {
                return _accountData;
            }

            Claim? nameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType);

            if (nameClaim is null)
            {
                return null;
            }

            string? accountDataJson = HttpContext.Session.GetString(_accountAuthenticationDataSessionKey);

            if (accountDataJson is null)
            {
                return null;
            }

            AccountReturnDto accountData = accountDataJson.FromJson<AccountReturnDto>();

            if (nameClaim.Value != accountData.Phone)
            {
                return null;
            }

            _accountData = accountData;

            return _accountData;
        }

        [NonAction]
        protected async Task SetClientData(AccountReturnDto? value)
        {
            if (_accountData is not null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            if (value is null)
            {
                return;
            }

            HttpContext.Session.SetString(_accountAuthenticationDataSessionKey, value.ToJson());

            List<Claim> claims = new()
            {
                new(ClaimsIdentity.DefaultNameClaimType, value.Phone)
            };

            ClaimsIdentity identity = new(claims, "ApplicationCookie");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }

        [NonAction]
        protected ApiSecrets? GetApiData()
        {
            if (_apiData is not null)
            {
                return _apiData;
            }

            string? data = HttpContext.Session.GetString(_apiAuthenticationDataSessionKey);

            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            _apiData = data.FromJson<ApiSecrets>();

            return _apiData;
        }

        [NonAction]
        protected void SetApiData(ApiSecrets? value)
        {
            _apiData = value;

            if (value is not null)
            {
                HttpContext.Session.SetString(_apiAuthenticationDataSessionKey, value.ToJson());
            }
            else
            {
                HttpContext.Session.Remove(_apiAuthenticationDataSessionKey);
            }
        }

        [NonAction]
        protected async Task<bool> IsAuthorized(params string[] roles)
        {
            if (GetClientData() is null || GetApiData() is null)
            {
                _unauthorizedAction = RedirectToLogin;
                return false;
            }

            if (roles is not null && roles.Length != 0 && roles.Contains(GetClientData().Role) == false)
            {
                _unauthorizedAction = RedirectToForbiden;
                return false;
            }

            if (GetApiData().TokenExpirationTime <= DateTime.UtcNow.AddMinutes(-1))
            {
                _unauthorizedAction = RedirectToLogin;
                return false;
            }

            return true;
        }

        [NonAction]
        protected IActionResult RedirectUnauthorizedAction() => _unauthorizedAction.Invoke();

        [NonAction]
        protected IActionResult RedirectToLogin() => Redirect(_loginPagePath);

        [NonAction]
        protected IActionResult RedirectToForbiden() => NotFoundPage("Вам не доступно это действие.");

        [NonAction]
        protected IActionResult NotFoundPage(string? message = "Страница не найдена.")
        {
            return View("Error", new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [NonAction]
        protected bool ClientInRole(params string[] roles)
        {
            return roles.Contains(GetClientData().Role);
        }
    }
}