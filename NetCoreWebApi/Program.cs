using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyModel;
using OpenTelemetry.NetCore.Linux;
using OpenTelemetry.NetCore.Windows;
using Orleans.Statistics;
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

        if (OperatingSystem.IsWindows())
        {
            // Add services to the container.
            // https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-otlp-example
            await AddWindowsStatistics(builder.Services);

            //use timer:
            //var app = new ProgramWindows(cpuUsage).DoMain(builder, args);
        }
        else
        {
            // Add services to the container.
            // https://github.com/dotnet/orleans/blob/639be7f3e83262e70327b58892d6cf54c801b32d/src/Orleans.Core/Statistics/LinuxEnvironmentStatistics.cs
            await AddLinuxStatistics(builder.Services);
        }

        var app = builder.Build();
        app.MapControllers();

        app.Run();
    }

    private static async Task AddWindowsStatistics(IServiceCollection services)
    {
        var cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        services.AddSingleton<PerformanceCounter>(cpuUsage);

        var meter = new Meter("OTel.Example", "1.0.0");
        services.AddSingleton<Meter>(meter);
    }

    private static async Task AddLinuxStatistics(IServiceCollection services)
    {
        ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        //ILogger logger = factory.CreateLogger("Program");
        var x = new LinuxEnvironmentStatistics(factory);
        services.AddSingleton<LinuxEnvironmentStatistics>(x);

        //use this line on endpoint
        //await x.OnStart(CancellationToken.None);
    }
}
