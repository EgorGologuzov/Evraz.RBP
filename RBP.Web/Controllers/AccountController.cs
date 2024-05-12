using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using RBP.Services.Dto;
using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Services.Utils;
using RBP.Web.Models;
using RBP.Web.Services;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;

namespace RBP.Web.Controllers
{
    public class AccountController : GeneralController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
            _accountService.Client = this;
        }

        [NonAction]
        protected async Task Login(string phone, string password)
        {
            ApiSecrets? apiData = await _accountService.Login(phone, password);

            if (apiData is null)
            {
                throw new AuthenticationException();
            }

            SetApiData(apiData);
            await SetClientData(apiData.Account);
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string phone, string password, int attemptNumber)
        {
            try
            {
                await Login(phone, password);
                _logger.LogInformation("Выполнен вход в аккаунт {phone}, {auth}", phone, HttpContext.User.Identity?.IsAuthenticated);

                return RedirectToAction("Index", "Home");
            }
            catch (AuthenticationException)
            {
                attemptNumber++;
                LoginViewModel model = new()
                {
                    Phone = phone,
                    Password = password,
                    AttemptNumber = attemptNumber
                };

                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            if (await IsAuthorized() == false)
            {
                return RedirectUnauthorizedAction();
            }

            SetApiData(null);
            await SetClientData(null);

            return RedirectToLogin();
        }

        public async Task<IActionResult> ResetPassword(Guid id)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            ResetPasswordViewModel model = new("Сброс пароля", GetClientData())
            {
                Id = id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(Guid id, string newPassword, string passwordRepeat)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            ResetPasswordViewModel model = new("Сброс пароля", GetClientData())
            {
                Id = id,
                NewPassword = newPassword,
                PasswordRepeat = passwordRepeat
            };

            if (newPassword != passwordRepeat)
            {
                model.ErrorMessage = "Пароли не совпадают";

                return View(model);
            }

            try
            {
                await _accountService.ResetPassword(id, newPassword);
                model.OkMessage = "Пароль обновлен";

                return View(model);
            }
            catch (NotOkResponseException ex)
            {
                model.ErrorMessage = ex.Message;

                return View(model);
            }
        }

        public async Task<IActionResult> UpdatePassword()
        {
            if (await IsAuthorized() == false)
            {
                return RedirectUnauthorizedAction();
            }

            UpdatePasswordViewModel model = new("Обновление пароля", GetClientData());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(string oldPassword, string newPassword, string passwordRepeat)
        {
            if (await IsAuthorized() == false)
            {
                return RedirectUnauthorizedAction();
            }

            UpdatePasswordViewModel model = new("Обновление пароля", GetClientData())
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
                PasswordRepeat = passwordRepeat
            };

            if (newPassword != passwordRepeat)
            {
                model.ErrorMessage = "Пароли не совпадают";

                return View(model);
            }

            try
            {
                await _accountService.UpdatePassword(GetClientData().Id, oldPassword, newPassword);
                model.OkMessage = "Пароль обновлен";

                return View(model);
            }
            catch (NotOkResponseException ex)
            {
                model.ErrorMessage = ex.Message;

                return View(model);
            }
        }
    }
}