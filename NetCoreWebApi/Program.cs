using NetCoreWebApi;
using NetCoreWebApi.Models.Linux;
using NetCoreWebApi.Service;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace OpenTelemetry.NetCore;

public class Program
{
    private static async Task Main(string[] args)
    {
        //builder
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        //if (isWindows)
        //{
            //AddWindowsStatistics(builder.Services);
        //}
        
        AddLinuxStatistics(builder.Services);

        builder.Services.AddTransient<StatisticsServiceFactory>();
        AddStatisticsService(builder.Services);

        var app = builder.Build();
        app.MapControllers();

        app.Run();
    }

    private static void AddWindowsStatistics(IServiceCollection services)
    {
        // https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-otlp-example
        var cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        services.AddSingleton<PerformanceCounter>(cpuUsage);

        var meter = new Meter("OTel.Example", "1.0.0");
        services.AddSingleton<Meter>(meter);
    }

    private static void AddLinuxStatistics(IServiceCollection services)
    {
        var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var les = new LinuxEnvironmentStatistics(factory);
        services.AddSingleton<LinuxEnvironmentStatistics>(les);
    }

    private static void AddStatisticsService(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var statisticsServiceFactory = serviceProvider.GetRequiredService<StatisticsServiceFactory>();
        var statisticsService = statisticsServiceFactory.GetRelayService(
                                                             OperatingSystem.IsWindows() 
                                                                ? EnvironmentType.Windows 
                                                                : EnvironmentType.Linux);

        //services.AddTransient<IStatisticsService, StatisticsLinuxService>();
        //services.AddTransient<IStatisticsService, StatisticsWindowsService>();
        services.AddSingleton<IStatisticsService>(statisticsService);
    }
}
