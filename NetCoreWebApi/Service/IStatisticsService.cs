using NetCoreWebApi.Models.Response;

namespace NetCoreWebApi.Service;

public interface IStatisticsService
{
    public EnvironmentType Environment { get; }

    Task<StatisticsResponseModel> GetCpuUsageData();
}

public enum EnvironmentType
{
    Undefined,
    Windows,
    Linux
}