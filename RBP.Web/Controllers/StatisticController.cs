using Microsoft.AspNetCore.Mvc;
using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Web.Models;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;

namespace RBP.Web.Controllers
{
    public class StatisticController : GeneralController
    {
        private readonly ILogger<StatisticController> _logger;
        private readonly IStatisticService _statisticService;
        private readonly IHandbookService _handbookService;

        public StatisticController(ILogger<StatisticController> logger, IStatisticService statisticService, IHandbookService handbookService)
        {
            _logger = logger;
            _statisticService = statisticService;
            _statisticService.Client = this;
            _handbookService = handbookService;
            _handbookService.Client = this;
        }

        [NonAction]
        private async Task<SegmentStatisticViewModel> BuildViewModel(string title, StatisticData data, DateTime start, DateTime end)
        {
            return new SegmentStatisticViewModel(
                pageTitle: title,
                client: GetClientData(),
                statistic: data,
                allSegments: await _handbookService.GetAllSegments(),
                start: start,
                end: end);
        }

        public async Task<IActionResult> Index()
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            DateTime start = DateTime.Now - TimeSpan.FromDays(7);
            DateTime end = DateTime.Now;

            StatisticData data = await _statisticService.GetAllWorkshopStatistic(start, end);
            SegmentStatisticViewModel model = await BuildViewModel("Статистика цеха", data, start, end);

            return View(model);
        }

        public async Task<IActionResult> Segment(int segmentId, DateTime? start, DateTime? end)
        {
            if (await IsAuthorized(ClientRoles.Admin) == false)
            {
                return RedirectUnauthorizedAction();
            }

            start ??= DateTime.Now - TimeSpan.FromDays(7);
            end ??= DateTime.Now;

            try
            {
                StatisticData data = await _statisticService.GetSegmentStatistic(segmentId, start.Value, end.Value);
                SegmentStatisticViewModel model = await BuildViewModel("Статистика", data, start.Value, end.Value);
                _logger.LogInformation("Запрошена статистика сегмента: {id}, {start} - {end}", segmentId, start, end);

                return View(model);
            }
            catch(NotOkResponseException ex)
            {
                SegmentStatisticViewModel model = await BuildViewModel("Статистика", new StatisticData { SegmentId = segmentId }, start.Value, end.Value);
                model.ErrorMessage = ex.Message;

                return View(model);
            }
        }
    }
}