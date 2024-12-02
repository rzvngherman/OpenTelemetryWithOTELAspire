using System.Diagnostics.Metrics;

namespace OpenTelemetry.NetCore.Linux.Diagnostics.Metrics;

public static class Instruments
{
    public static readonly Meter Meter = new("Microsoft.Orleans");
}
