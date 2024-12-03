using NetCoreWebApi.Models.Response;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace NetCoreWebApi.Service;

public class StatisticsWindowsService : IStatisticsService
{
    private readonly PerformanceCounter _performanceCounter;
    private readonly Meter _meter;
    private static float _cpuUsagePercent;

    public EnvironmentType Environment => EnvironmentType.Windows;

    public StatisticsWindowsService()
    {
        //added here and not used DI because class 'PerformanceCounter' is only available on windows platforms
        // https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-otlp-example

        _performanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        var meter = new Meter("OTel.Example", "1.0.0");
        meter.CreateObservableGauge("cpu.usage", () => _cpuUsagePercent);
    }

    public async Task<StatisticsResponseModel> GetCpuUsageData()
    {
        float w = _performanceCounter.NextValue();
        _cpuUsagePercent = w;

        return new StatisticsResponseModel
        {
            CpuUsage = w + " %",
        };
    }
}
