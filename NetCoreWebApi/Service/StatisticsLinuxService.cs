using NetCoreWebApi.Models.Linux;
using NetCoreWebApi.Models.Response;

namespace NetCoreWebApi.Service;

public class StatisticsLinuxService : IStatisticsService
{
    private readonly LinuxEnvironmentStatistics _linuxEnvironmentStatistics;
    public EnvironmentType Environment => EnvironmentType.Linux;

    public StatisticsLinuxService(LinuxEnvironmentStatistics linuxEnvironmentStatistics)
    {
        _linuxEnvironmentStatistics = linuxEnvironmentStatistics;
    }

    public async Task<StatisticsResponseModel> GetCpuUsageData()
    {
        await _linuxEnvironmentStatistics.OnStart(CancellationToken.None);

        var cpu = _linuxEnvironmentStatistics.CpuUsage;
        if(cpu is null)
        {
            return new StatisticsResponseModel
            {
                CpuUsage = "0 %",
                MonitorPeriod = _linuxEnvironmentStatistics.MONITOR_PERIOD
            };
        }
        var res = new StatisticsResponseModel
        {
            CpuUsage = cpu!.Value + " %",
            MonitorPeriod = _linuxEnvironmentStatistics.MONITOR_PERIOD
        };
        await _linuxEnvironmentStatistics.OnStop(CancellationToken.None);

        return res;
    }
}
