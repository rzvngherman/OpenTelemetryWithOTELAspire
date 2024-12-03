using Microsoft.AspNetCore.Mvc;
using NetCoreWebApi.Models.Response;
using NetCoreWebApi.Service;

namespace NetCoreWebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly ILogger<StatisticsController> _logger;
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(ILogger<StatisticsController> logger, IStatisticsService statisticsService)
    {
        _logger = logger;
        _statisticsService = statisticsService;
    }

    [HttpGet]
    public async Task<StatisticsResponseModel> GetCpuUsageForEnvironment()
    {
        var cpuUsageData = await _statisticsService.GetCpuUsageData();
        return cpuUsageData;
    }
}
