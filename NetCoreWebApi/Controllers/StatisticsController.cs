using Microsoft.AspNetCore.Mvc;
using NetCoreWebApi.Models;
using NetCoreWebApi.Service;
using OpenTelemetry.NetCore.Linux;

namespace NetCoreWebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly ILogger<StatisticsLinuxController> _logger;
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(ILogger<StatisticsLinuxController> logger, IStatisticsService statisticsService)
    {
        _logger = logger;
        _statisticsService = statisticsService;
    }

    [HttpGet]
    public StatisticsResponseModel GetCpuUsageForLinuxEnvironment()
    {
        //TODO - call '_statisticsService'
        throw new NotImplementedException();
    }
}
