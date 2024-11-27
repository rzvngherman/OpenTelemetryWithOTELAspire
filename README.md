# OpenTelemetryWithOTELAspire
OpenTelemetry with OTEL and Aspire Dashboard

-> OpenTelemetry (OTel) is a cross-platform, open standard for collecting and emitting telemetry data.
When you run an application, you want to know how well the app is performing and to detect potential problems before they become larger. You can do this by emitting telemetry data such as logs or metrics from your app, then monitoring and analyzing that data.

-> Aspire Dashboard shows metrics on a per resource basis (a resource being the OTel way of talking about sources of telemetry such as a process). the Aspire Dashboard is intended as a developer visualization tool, and not for production monitoring

-> added a Timer that runs every 1000 ms (1 second)

-> Timer displays CPU usage percentage on Console.Write

-> see file 'powershell aspire-dashboard.txt' for Aspire Dashboard

-> 'PerformanceCounter' and 'PerformanceCounter.NextValue()' is only supported on: 'windows'. (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1416)


# Documentation
https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-with-otel

https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-otlp-example

https://learn.microsoft.com/en-us/dotnet/core/diagnostics/metrics-instrumentation



# Example of Aspire Dashboard for cpu.usage



![image](https://github.com/user-attachments/assets/fb590b92-d264-4034-9bf4-bf942f04fb7f)

