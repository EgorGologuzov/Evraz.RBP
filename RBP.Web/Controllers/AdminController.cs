﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RBP.Services.Utils;
using RBP.Services.Dto;
using RBP.Web.Models;
using RBP.Services.Models;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;
using RBP.Services.Static;

namespace RBP.Web.Controllers
{
    public class AdminController : GeneralController
    {
        private readonly IHandbookService _handbookService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AdminController(
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
        private async Task<AdminListViewModel> BuildViewModel(string title, string searchRequest)
        {
            return new AdminListViewModel(
                pageTitle: title,
                client: GetClientData(),
                searchRequest: searchRequest,
                admins: (await _accountService.Find(searchRequest, ClientRoles.Admin))
                    .Select(a => new AdminViewModel(null, null, a)).ToList()
            );
        }

        [NonAction]
        private async Task<AdminViewModel> BuildViewModel(string title, object data)
        {
            AdminRoleData roleData = _mapper.Map<AdminRoleData>(data);
            AccountReturnDto accountData = new() { Role = ClientRoles.Admin, RoleDataJson = roleData.ToJson() };
            _mapper.Map(data, accountData);

            return new AdminViewModel(title, GetClientData(), accountData);
        }

        [NonAction]
        private async Task<AdminViewModel> BuildViewModel(string title, AccountReturnDto data)
        {
            return new AdminViewModel(title, GetClientData(), data);
        }

        [NonAction]
        private async Task<AccountSecretsViewModel> BuildViewModel(string title, AccountSecrets secrets)
        {
            return new AccountSecretsViewModel(title, GetClientData(), secrets);
        }

        public async Task<IActionResult> Index(string? searchRequest)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            AdminListViewModel model = await BuildViewModel("Администраторы", searchRequest);

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            AccountReturnDto data = new()
            {
                Role = ClientRoles.Admin,
                RoleDataJson = new AdminRoleData().ToJson()
            };

            AdminViewModel model = await BuildViewModel("Создание", data);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCreateDto data)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            try
            {
                AccountSecrets result = await _accountService.CreateAdmin(data);
                AccountSecretsViewModel model = await BuildViewModel("Создан аккаунт", result);

                return View("~/Views/Account/Secrets.cshtml", model);
            }
            catch (NotOkResponseException ex)
            {
                AdminViewModel model = await BuildViewModel("Создание", data);
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

            AccountReturnDto? data = await _accountService.Get(id);

            if (data is null)
            {
                return NotFoundPage();
            }

            AdminViewModel model = await BuildViewModel("Редактирование", data);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminUpdateDto data)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            try
            {
                AccountReturnDto result = await _accountService.UpdateAdmin(data);

                return RedirectToAction(nameof(Index), new { SearchRequest = string.Empty });
            }
            catch (NotOkResponseException ex)
            {
                AdminViewModel model = await BuildViewModel("Создание", data);
                model.ErrorMessage = ex.Message;

                return View(model);
            }
        }
    }
}