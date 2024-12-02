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
    public async Task<StatisticsLinuxResponseModel> GetCpuUsageForLinuxEnvironment()
    {
        await _linuxEnvironmentStatistics.OnStart(CancellationToken.None);
        var res = new StatisticsLinuxResponseModel
        {
            CpuUsage = _linuxEnvironmentStatistics.CpuUsage!.Value + " %",
            MonitorPeriod = _linuxEnvironmentStatistics.MONITOR_PERIOD
        };
        await _linuxEnvironmentStatistics.OnStop(CancellationToken.None);

        // https://localhost:32770/StatisticsLinux
        return res;
    }
}
