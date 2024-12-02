using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;
using System.Timers;

namespace OpenTelemetry.NetCore.Windows;

public class ProgramWindows
{
    private PerformanceCounter _cpuUsage;
    private Meter _meter;
    private System.Timers.Timer _timer;
    private static float _cpuUsagePercent;

    public ProgramWindows(PerformanceCounter cpuUsage)
    {
        _cpuUsage = cpuUsage;
        //_cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        //_meter = new Meter("OTel.Example", "1.0.0");
        _meter.CreateObservableGauge("cpu.usage", () => _cpuUsagePercent);

        //add timer
        double ms = 1000;
        Console.WriteLine("Timer time: " + ms + "(ms)");
        _timer = new System.Timers.Timer(ms);//1 second
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

    internal WebApplication DoMain(WebApplicationBuilder builder, string[] args)
    {
        ConfigureOpenTelemetry(builder);

        var app = builder.Build();

        //start timer
        _timer.Start();

        
        return app;

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
}