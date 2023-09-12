using LinqToDB;
using LinqToDB.Tools;
using OSS.IPTV.Ministra.Admin.Data.Models.Channel;
using OSS.IPTV.Ministra.Admin.Extensions;
using OSS.IPTV.Ministra.Admin.Interfaces;
using OSS.IPTV.Ministra.Admin.Models.Channel;
using OSS.IPTV.Ministra.Admin.Models.Log;
using OSS.IPTV.Ministra.Admin.Models.Package;
using OSS.IPTV.Ministra.Admin.Models.Shared;
using OSS.IPTV.Ministra.Repository;
using OSS.IPTV.Ministra.Repository.Entities;
using static OSS.IPTV.Ministra.Admin.Enums.LogEnums;
using ILogger = OSS.IPTV.Ministra.Admin.Interfaces.ILogger;
using OrderFields = OSS.IPTV.Ministra.Admin.Data.Models.Channel.OrderFields;

namespace OSS.IPTV.Ministra.Admin.Services;

public class TvChannelService : IChannel
{
    private readonly IptvContext _context;
    private readonly ILogger _logger;
    public TvChannelService(IptvContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> AddTvChannel(ChannelAddRequest request)
    {
        var newChannel = new TvChannel()
        {
            Name = request.Name,
            IsActive = request.IsActive,
            IsHd = request.IsHd,
            MinistraId = request.MinistraId
        };

        var identity = await _context.AddAsync(newChannel);

        var logRequest = new LogRequest
        {
            ChangeAction = ChangeAction.Insert,
            ChangeType = ChangeType.CreateChannel,
            UpdatedItemId = identity.Entity.ChannelId,
            UpdatedItemType = ItemType.Channel,
            Username = request.Username
        };

        await _logger.LogChanges(logRequest);

        return identity.Entity.ChannelId;
    }

    public Task DeleteTvChannel()
    {
        throw new NotImplementedException();
    }

    public async Task EditTvChannel(ChannelEditRequest request)
    {
        await _context.TvChannels
            .Where(t => t.ChannelId == request.ChannelId)
            .Set(t => t.IsActive, request.IsActive)
            .Set(p => p.IsHd, request.IsHd)
            .Set(p => p.MinistraId, request.MinistraId)
            .Set(p => p.Name, request.ChannelName)
            .UpdateAsync();

        var logRequest = new LogRequest
        {
            ChangeAction = ChangeAction.Update,
            ChangeType = ChangeType.UpdateChannel,
            UpdatedItemId = request.ChannelId,
            UpdatedItemType = ItemType.Channel,
            UpdatedFields = request.UpdatedFields,
            Username = request.Username
        };

        await _logger.LogChanges(logRequest);
    }

    public async Task<List<ChannelMin>> GetChannelsById(IEnumerable<int> ids)
    {
        var query = _context.TvChannels.Where(c => c.ChannelId.In(ids)).Select(c => new ChannelMin { Id = c.ChannelId, MinistraId = c.MinistraId, Name = c.Name });
        return await query.ToListAsync();
    }

    public async Task<ChannelFindResponse> ListTvChannels(ChannelFindRequest request)
    {
        var query = _context.TvChannels.AsQueryable();

        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(c => c.Name.Contains(request.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == request.IsActive);
        }

        if (request.MinistraId.HasValue)
        {
            query = query.Where(c => c.MinistraId == request.MinistraId);
        }

        if (request.OrderAscending)
        {
            switch (request.OrderBy)
            {
                case OrderFields.Id: query = query.OrderBy(c => c.ChannelId); break;
                case OrderFields.Name: query = query.OrderBy(c => c.Name); break;
                case OrderFields.Ministra: query = query.OrderBy(c => c.MinistraId); break;
            }
        }
        else
        {
            switch (request.OrderBy)
            {
                case OrderFields.Id: query = query.OrderByDescending(c => c.ChannelId); break;
                case OrderFields.Name: query = query.OrderByDescending(c => c.Name); break;
                case OrderFields.Ministra: query = query.OrderByDescending(c => c.MinistraId); break;
            }
        }

        var paged = query
            .Paginate(new PagingSettings
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            });

        var result = new ChannelFindResponse
        {
            TotalRows = await query.CountAsync(),
            PageRows = await paged.ToArrayAsync()
        };

        return result;
    }

    public async Task<ChannelLinkedPackages> GetLinkedPackages(int channelId)
    {
        var currentPackages =
            from channel in _context.TvPackageChannels.Where(c => c.ChannelId == channelId)
            from package in _context.TvPackages.Where(p => p.PackageId == channel.PackageId)
            select package;

        var availablePackages =
            from package in _context.TvPackages
            where package.IsActive
            where !(from channel in _context.TvPackageChannels
                    where channel.ChannelId == channelId
                    select channel.PackageId)
                .Contains(package.PackageId)
            select package;

        var result = new ChannelLinkedPackages
        {
            ChannelId = channelId,
            CurrentPackages = await currentPackages.Select(p => new PackageMin
            {
                Id = p.PackageId,
                Name = p.Name,
                Type = p.TypeId,
                Active = p.IsActive
            }).ToListAsync(),
            AvailablePackages = await availablePackages.Select(p => new PackageMin
            {
                Id = p.PackageId,
                Name = p.Name,
                Type = p.TypeId,
                Active = p.IsActive
            }).ToListAsync()
        };
        return result;
    }

    public bool IsChannelNameUnique(string channelName)
    {
        var channelPresent =
            _context.TvChannels.Any(c => c.Name.Equals(channelName));
        return !channelPresent;
    }

    public bool IsMinistraIdUnique(int? ministraId)
    {
        var idUnique = _context.TvChannels.Any(c => c.MinistraId == ministraId);
        return !idUnique;
    }

    public async Task UpdateChannelPackages(ChannelEditPackagesRequest request)
    {
        if (request.RemoveFromPackages.Any())
        {
            await _context.TvPackageChannels.DeleteAsync(p => p.ChannelId == request.ChannelId &&
                                                         p.PackageId.In(request.RemoveFromPackages.Select(r => r.Id)));

            var logDelete = new LogRequest
            {
                ChangeAction = ChangeAction.Delete,
                ChangeType = ChangeType.RemoveChannelFromPackages,
                UpdatedItemId = request.ChannelId,
                UpdatedItemType = ItemType.PackageChannels,
                AffectedItemIds = request.RemoveFromPackages.Select(s => s.Id).ToArray(),
                Username = request.Username
            };

            await _logger.LogChanges(logDelete);
        }
        if (request.AddToPackages.Any())
        {
            var insertList = request.AddToPackages.Select(p => new TvPackageChannels
            {
                ChannelId = request.ChannelId,
                PackageId = p.Id
            }).ToList();

            foreach (var item in insertList)
            {
                await _context.AddAsync(item);
            }

            var logAdd = new LogRequest
            {
                ChangeAction = ChangeAction.Update,
                ChangeType = ChangeType.AddChannelToPackages,
                UpdatedItemId = request.ChannelId,
                UpdatedItemType = ItemType.PackageChannels,
                AffectedItemIds = request.AddToPackages.Select(s => s.Id).ToArray(),
                Username = request.Username
            };

            await _logger.LogChanges(logAdd);
        }
    }
}