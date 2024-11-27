
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;
using System.Timers;

namespace OpenTelemetry.NetCore;

public class Program
{
    private PerformanceCounter _cpuUsage;
    private Meter _meter;
    private System.Timers.Timer _timer;
    private static float _cpuUsagePercent;

    public Program()
    {
        // https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-otlp-example

        _cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        _meter = new Meter("OTel.Example", "1.0.0");
        _meter.CreateObservableGauge<float>("cpu.usage", () => _cpuUsagePercent);

        //add timer
        _timer = new System.Timers.Timer(1000);//1 second
        _timer.AutoReset = true;
        _timer.Elapsed += Timer_Elapsed;
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
        // Create a new Activity scoped to the method
        float w = _cpuUsage.NextValue();
        _cpuUsagePercent = w;

        StringBuilder msg = new StringBuilder();
        msg.AppendLine("CPU Usage: " + w + "%");
        msg.AppendLine(DateTime.Now.ToString());

        Console.WriteLine(msg.ToString());
    }

    private void DoMain(string[] args)
    {
        //builder
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        ConfigureOpenTelemetry(builder);

        var app = builder.Build();

       //start timer
        _timer.Start();

        app.Run();

        //methods:
        void ConfigureOpenTelemetry(WebApplicationBuilder builder)
        {
            // 5. Configure OpenTelemetry with the correct providers

            // Setup logging to be exported via OpenTelemetry
            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            });

            var otel = builder.Services.AddOpenTelemetry();

            // Add Metrics for ASP.NET Core and our custom metrics and export via OTLP
            otel.WithMetrics(metrics =>
            {
                // Metrics provider from OpenTelemetry
                metrics.AddAspNetCoreInstrumentation();

                //Our custom metrics
                //metrics.AddMeter(_greeterMeter.Name);
                metrics.AddMeter(_meter.Name);

                // Metrics provides by ASP.NET Core in .NET 8
                metrics.AddMeter("Microsoft.AspNetCore.Hosting");
                metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
            });

            // Add Tracing for ASP.NET Core and our custom ActivitySource and export via OTLP
            otel.WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();
            });

            // Export OpenTelemetry data via OTLP, using env vars for the configuration
            var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
            if (otlpEndpoint != null)
            {
                otel.UseOtlpExporter();
            }
        }
    }

    private static void Main(string[] args)
    {
        new Program().DoMain(args);
    }
}