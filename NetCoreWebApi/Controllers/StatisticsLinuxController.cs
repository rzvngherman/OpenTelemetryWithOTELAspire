using Microsoft.AspNetCore.Mvc;
using NetCoreWebApi.Models;
using OpenTelemetry.NetCore.Linux;

namespace NetCoreWebApi.Controllers;


[Route("[controller]")]
[ApiController]
public class StatisticsLinuxController  : ControllerBase
{
    private readonly ILogger<StatisticsLinuxController> _logger;
    private readonly LinuxEnvironmentStatistics _linuxEnvironmentStatistics;

    public StatisticsLinuxController(
        ILogger<StatisticsLinuxController> logger
        , LinuxEnvironmentStatistics linuxEnvironmentStatistics)
    {
        _logger = logger;
        _linuxEnvironmentStatistics = linuxEnvironmentStatistics;
    }

    [HttpGet]
    public async Task<StatisticsResponseModel> GetCpuUsageForLinuxEnvironment()
    {
        // https://localhost:32770/StatisticsLinux
        return new StatisticsResponseModel
        {
            CpuUsage = _linuxEnvironmentStatistics.CpuUsage!.Value + " %",
            MonitorPeriod = _linuxEnvironmentStatistics.MONITOR_PERIOD
        };
    }
}
