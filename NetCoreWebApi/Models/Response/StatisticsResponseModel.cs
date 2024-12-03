namespace NetCoreWebApi.Models.Response
{
    public class StatisticsResponseModel
    {
        public string CpuUsage { get; set; }
        public TimeSpan? MonitorPeriod { get; set; }
    }
}
