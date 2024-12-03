using Microsoft.AspNetCore.Mvc;
using NetCoreWebApi.Models;
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
        //TODO - call '_statisticsService'
        // https://softwareengineering.stackexchange.com/questions/345672/c-when-one-should-go-for-factory-method-pattern-instead-of-factory-pattern
        // use 'ShipperFactory.CreateInstance(...)'
        var  res = await _statisticsService.GetCpuUsageData();
        return res;
    }
}
