namespace NetCoreWebApi.Models
{
    public class StatisticsLinuxResponseModel
    {
        public string CpuUsage { get; set; }
        public TimeSpan? MonitorPeriod { get; set; }
    }
}
