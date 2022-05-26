using OSS.IPTV.Ministra.Admin.Models.Log;

namespace OSS.IPTV.Ministra.Admin.Interfaces
{
    public interface ILogger
    {
        Task LogChanges(LogRequest request);
        Task<LogFindResponse> FindLogs(LogFindRequest request);
    }
}
