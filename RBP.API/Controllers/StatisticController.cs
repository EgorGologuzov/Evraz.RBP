using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBP.Services.Contracts;
using RBP.Services.Dto;
using RBP.Services.Exceptions;
using RBP.Services.Models;
using RBP.Services.Static;
using RBP.Services.Utils;

namespace RBP.API.Controllers
{
    [ApiController]
    [Route("Statistic")]
    [Authorize]
    public class StatisticController : GeneralController
    {
        public IStatisticRepository Repository { get; }

        public StatisticController(
            ILogger<StatisticController> logger,
            IMapper mapper,
            IStatisticRepository repository) : base(logger, mapper)
        {
            Repository = repository;
        }

        [HttpPost("ForSegment")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> ForSegment(StatisticGetDto data)
        {
            return TryResult(async () =>
            {
                StatisticData entity = await Repository.GetSegmentStatistic(data.SegmentId, data.Start, data.End);

                return Ok(entity);
            });
        }

        [HttpPost("ForWorkshop")]
        [Authorize(Roles = ClientRoles.Admin)]
        public Task<IActionResult> ForWorkshop(StatisticGetDto data)
        {
            return TryResult(async () =>
            {
                StatisticData entity = await Repository.GetAllWorkshopStatistic(data.Start, data.End);

                return Ok(entity);
            });
        }
    }
}