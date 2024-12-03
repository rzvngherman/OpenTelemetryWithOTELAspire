using System.Diagnostics.Metrics;

namespace NetCoreWebApi.Models.Linux.Diagnostics.Metrics;

public static class Instruments
{
    public static readonly Meter Meter = new("Microsoft.Orleans");
}
