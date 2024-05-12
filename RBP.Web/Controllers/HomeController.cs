using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RBP.Services.Static;
using RBP.Web.Models;
using RBP.Web.Services;
using RBP.Web.Services.Interfaces;

namespace RBP.Web.Controllers
{
    public class HomeController : GeneralController
    {
        private readonly IStatisticService _statisticService;
        private readonly IHandbookService _handbookService;

        public HomeController(IStatisticService statisticService, IHandbookService handbookService)
        {
            _statisticService = statisticService;
            _statisticService.Client = this;
            _handbookService = handbookService;
            _handbookService.Client = this;
        }

        public async Task<IActionResult> Index()
        {
            if (await IsAuthorized() == false)
            {
                return RedirectUnauthorizedAction();
            }

            if (ClientInRole(ClientRoles.Employee))
            {
                return Redirect($"/Statement/EmployeeList?EmployeeId={GetClientData().Id}&Date={DateTime.Now.ToString("yyyy-MM-dd")}");
            }
            else if (ClientInRole(ClientRoles.Admin))
            {
                return Redirect("/Statistic/Index");
            }

            return NotFoundPage("Для этой роли не задана главная страница");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}