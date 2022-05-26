using LinqToDB;
using OSS.IPTV.Ministra.Admin.Extensions;
using OSS.IPTV.Ministra.Admin.Models.Log;
using OSS.IPTV.Ministra.Admin.Models.Shared;
using OSS.IPTV.Ministra.Repository;
using OSS.IPTV.Ministra.Repository.Entities;
using ILogger = OSS.IPTV.Ministra.Admin.Interfaces.ILogger;

namespace OSS.IPTV.Ministra.Admin.Services
{
    public class LoggerService : ILogger
    {
        private readonly IptvContext _context;

        public LoggerService(IptvContext context)
        {
            _context = context;
        }

        public async Task LogChanges(LogRequest request)
        {
            var log = new TvLogs
            {
                ChangeAction = (int)request.ChangeAction,
                ChangeType = (int)request.ChangeType,
                UpdatedItemId = request.UpdatedItemId,
                UpdatedItemType = (int)request.UpdatedItemType,
                AffectedItemIds = request.AffectedItemIds is null ? null : string.Join(',', request.AffectedItemIds),
                UpdatedFields = request.UpdatedFields is null ? null : string.Join(",", request.UpdatedFields),
                LogTime = DateTime.Now,
                Username = request.Username
            };

            await _context.InsertAsync(log);
        }

        public async Task<LogFindResponse> FindLogs(LogFindRequest request)
        {
            var query = _context.TvLogs.AsQueryable();

            if (request.ItemId.HasValue)
            {
                query = query.Where(c => c.UpdatedItemId == request.ItemId);
            }

            if (request.ItemType.HasValue)
            {
                query = query.Where(c => c.UpdatedItemType == (int?)request.ItemType);
            }

            if (request.ActionType.HasValue)
            {
                query = query.Where(c => c.ChangeAction == (int?)request.ActionType);
            }

            if (request.ChangeType.HasValue)
            {
                query = query.Where(c => c.ChangeType == (int?)request.ChangeType);
            }

            if (request.LogFrom.HasValue)
            {
                query = query.Where(c => c.LogTime >= request.LogFrom);
            }

            if (request.LogTo.HasValue)
            {
                query = query.Where(c => c.LogTime <= request.LogTo);
            }

            query = query.OrderBy(c => c.LogTime);

            var paged = query
                .Paginate(new PagingSettings
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                });

            var result = new LogFindResponse
            {
                TotalRows = await query.CountAsync(),
                PageRows = await paged.ToArrayAsync()
            };

            return result;
        }
    }
}
