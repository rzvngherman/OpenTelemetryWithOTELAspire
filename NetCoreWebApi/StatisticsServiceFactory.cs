using NetCoreWebApi.Models.Linux;
using NetCoreWebApi.Service;

namespace NetCoreWebApi;

public class StatisticsServiceFactory
{
    private readonly LinuxEnvironmentStatistics _linuxEnvironmentStatistics;

    public StatisticsServiceFactory(LinuxEnvironmentStatistics linuxEnvironmentStatistics)
    {
        _linuxEnvironmentStatistics = linuxEnvironmentStatistics;
    }

    public IStatisticsService GetRelayService(EnvironmentType relayMode)
    {
        return relayMode switch
        {
            EnvironmentType.Windows => new StatisticsWindowsService(),
            EnvironmentType.Linux => new StatisticsLinuxService(_linuxEnvironmentStatistics),
            _ => throw new NotImplementedException()
        };
    }
}
