using Microsoft.AspNetCore.Mvc;
using NetCoreWebApi.Models;
using OpenTelemetry.NetCore.Linux;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace NetCoreWebApi.Controllers;


[Route("[controller]")]
[ApiController]
public class StatisticsWindowsController : ControllerBase
{
    private readonly ILogger<StatisticsWindowsController> _logger;
    private readonly PerformanceCounter _performanceCounter;
    private readonly Meter _meter;
    private static float _cpuUsagePercent;

    public StatisticsWindowsController(
        ILogger<StatisticsWindowsController> logger
        , PerformanceCounter performanceCounter
        , Meter meter)
    {
        _logger = logger;
        _performanceCounter = performanceCounter;
        _meter = meter;
        _meter.CreateObservableGauge("cpu.usage", () => _cpuUsagePercent);
    }

    [HttpGet]
    public StatisticsWindowsResponseModel GetCpuUsageForWindowsEnvironment()
    {
        float w = _performanceCounter.NextValue();
        _cpuUsagePercent = w;

        // https://localhost:7282/StatisticsWindows	
        return new StatisticsWindowsResponseModel
        {
            CpuUsage = w + " %",
        };
    }
}
