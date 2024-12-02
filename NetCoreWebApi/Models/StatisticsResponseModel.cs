namespace NetCoreWebApi.Models
{
    public class StatisticsResponseModel
    {
        public string CpuUsage { get; set; }
        public TimeSpan? MonitorPeriod { get; set; }
    }
}
