
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;
using static System.Net.Mime.MediaTypeNames;

namespace OpenTelemetry.NetCore;

internal class Program
{
    private static float _hatsSold4;

    private static void Main(string[] args)
    {
        // https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-otlp-example

        var _cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        var _meter = new Meter("OTel.Example", "1.0.0");
        //var _countGreetings = _meter.CreateCounter<int>("greetings.count", description: "Counts the number of greetings");
        //var _meter = new Meter("HatCo.Store");
        //Histogram<float> _hatsSold = _meter.CreateHistogram<float>("cpu.usage");
        _meter.CreateObservableGauge<float>("cpu.usage(%)", () => _hatsSold4);

        // Custom ActivitySource for the application
        var _greeterActivitySource = new ActivitySource("OTel.Example");

        //builder
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // 5. Configure OpenTelemetry with the correct providers
        ConfigureOpenTelemetry(builder);

        //Minimal API
        var app = builder.Build();

        ////app.MapGet("/", SendGreeting);
        ////app.MapGet("/", CpuUsageActivity);
        //app.MapGet("/", GetCpuUsage);

        var timer = new System.Timers.Timer(1000);//1 second
        timer.AutoReset = true;
        timer.Elapsed += Timer_Elapsed;
        timer.Start();

        app.Run();

        //methods:
        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Create a new Activity scoped to the method
            float w = _cpuUsage.NextValue();
            _hatsSold4 = w;

            StringBuilder msg = new StringBuilder();
            msg.AppendLine("CPU Usage: " + w + "%");
            msg.AppendLine(DateTime.Now.ToString());

            Console.WriteLine(msg.ToString());
        }

        void ConfigureOpenTelemetry(WebApplicationBuilder builder)
        {
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
                tracing.AddSource(_greeterActivitySource.Name);
            });

            // Export OpenTelemetry data via OTLP, using env vars for the configuration
            var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
            if (otlpEndpoint != null)
            {
                otel.UseOtlpExporter();
            }
        }

        //async Task<string> RunGetCpuUsage(ILogger<Program> logger, HttpContext context)
        //{

        //    // Create a new Activity scoped to the method
        //    float w = _cpuUsage.NextValue();
        //    _hatsSold4 = w;

        //    StringBuilder msg = new StringBuilder();
        //    msg.AppendLine("CPU Usage: " + w + "%");
        //    msg.AppendLine(DateTime.Now.ToString());

        //    return msg.ToString();
        //}

        //async Task<string> GetCpuUsage(ILogger<Program> logger, HttpContext context)
        //{
        //    // Create a new Activity scoped to the method
        //    float w = _cpuUsage.NextValue();
        //    _hatsSold4 = w;

        //    StringBuilder msg = new StringBuilder();
        //    msg.AppendLine("CPU Usage: " + w + "%");
        //    msg.AppendLine(DateTime.Now.ToString());

        //    return msg.ToString();
        //}

        //async Task<string> CpuUsageActivity(ILogger<Program> logger, HttpContext context)
        //{
        //    // Create a new Activity scoped to the method
        //    using var activity = _greeterActivitySource.StartActivity("GetCpuActivity");

        //    StringBuilder msg = new StringBuilder();

        //    float w = _cpuUsage.NextValue();
        //    DoSomeWork(logger, context);

        //    //_hatsSold.Record(w);
        //    _hatsSold4 = w;

        //    msg.AppendLine("CPU Usage: " + w + "%");

        //    // Increment the custom counter



        //    // Add a tag to the Activity
        //    activity?.SetTag("CPU-Usage", msg.ToString());

        //    // Log a message
        //    logger.LogInformation(msg.ToString());

        //    msg.AppendLine(DateTime.Now.ToString());

        //    return msg.ToString();
        //}

        //async Task<string> SendGreeting(ILogger<Program> logger)
        //{
        //    // Step 1:
        //    // The endpoint definition does not use anything specific to OpenTelemetry. It uses the .NET APIs for observability.

        //    // Create a new Activity scoped to the method
        //    using var activity = _greeterActivitySource.StartActivity("GreeterActivity");

        //    // Log a message
        //    logger.LogInformation("Sending greeting");

        //    // Increment the custom counter
        //    _countGreetings.Add(1);

        //    // Add a tag to the Activity
        //    activity?.SetTag("greeting", "Hello World!");

        //    return "Hello World!";
        //}

        //async Task DoSomeWork(ILogger<Program> logger, HttpContext context)
        //{


        //    //
        //    await Task.Delay(500);
        //}

        //async Task DoSomeWork2(ILogger<Program> logger, HttpContext context)
        //{
        //    //TODO - call external service ?
        //    int[] numbers = new int[1000000]; // Example array 

        //    // Initialize the array with some values 
        //    for (int i = 0; i < numbers.Length; i++)
        //    {
        //        numbers[i] = i;
        //    }

        //    // Use Parallel.For to process the array in parallel 
        //    Parallel.For(0, numbers.Length, i =>
        //    {
        //        // Perform some computation 
        //        numbers[i] = numbers[i] * 2;
        //    });

        //    Console.WriteLine("Processing completed.");
        //}
    }
}