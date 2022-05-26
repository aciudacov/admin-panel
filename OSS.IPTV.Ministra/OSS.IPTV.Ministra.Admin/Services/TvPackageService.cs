using LinqToDB;
using LinqToDB.Tools;
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

namespace OSS.IPTV.Ministra.Admin.Services;

public class TvPackageService : IPackage
{
    private readonly IptvContext _context;
    private readonly ILogger _logger;

    public TvPackageService(IptvContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> AddTvPackage(PackageAddRequest request)
    {
        var newPackage = new TvPackage
        {
            TypeId = request.Type,
            Name = request.Name,
            Price = request.Price,
            IsActive = request.Active,
            Duration = request.Duration,
            PromoLimit = request.PromoLimit,
            IsBillingActive = request.BillingActive,
            UserType = request.UserType
        };

        var identity = await _context.InsertWithInt32IdentityAsync(newPackage);

        var logRequest = new LogRequest
        {
            ChangeAction = ChangeAction.Insert,
            ChangeType = ChangeType.CreatePackage,
            UpdatedItemId = identity,
            UpdatedItemType = ItemType.Package,
            Username = request.Username
        };

        await _logger.LogChanges(logRequest);

        return identity;
    }

    public Task DeleteTvPackage()
    {
        throw new NotImplementedException();
    }

    public async Task EditPackageChannels(PackageEditChannelsRequest request)
    {
        if (request.ChannelsToAdd.Any())
        {
            var insertList = request.ChannelsToAdd.Select(c => new TvPackageChannels
            {
                ChannelId = c.Id,
                PackageId = request.PackageId
            }).ToList();

            foreach (var item in insertList)
            {
                await _context.InsertAsync(item);
            }

            var logAdd = new LogRequest
            {
                ChangeAction = ChangeAction.Insert,
                ChangeType = ChangeType.AddChannelsToPackage,
                UpdatedItemId = request.PackageId,
                UpdatedItemType = ItemType.PackageChannels,
                AffectedItemIds = insertList.Select(s => s.ChannelId).ToArray(),
                Username = request.Username
            };

            await _logger.LogChanges(logAdd);
        }

        if (request.ChannelsToRemove.Any())
        {
            await _context.TvPackageChannels.DeleteAsync(c =>
                c.PackageId == request.PackageId && c.ChannelId.In(request.ChannelsToRemove.Select(r => r.Id)));

            var logDelete = new LogRequest
            {
                ChangeAction = ChangeAction.Delete,
                ChangeType = ChangeType.RemoveChannelsFromPackage,
                UpdatedItemId = request.PackageId,
                UpdatedItemType = ItemType.PackageChannels,
                AffectedItemIds = request.ChannelsToRemove.Select(s => s.Id).ToArray(),
                Username = request.Username
            };

            await _logger.LogChanges(logDelete);
        }
    }

    public async Task EditTvPackage(PackageEditRequest request)
    {
        await _context.TvPackages
            .Where(t => t.PackageId == request.Id)
            .Set(t => t.Name, request.Name)
            .Set(t => t.Price, request.Price)
            .Set(t => t.IsActive, request.Active)
            .Set(t => t.Duration, request.Duration)
            .Set(t => t.PromoLimit, request.PromoLimit)
            .Set(t => t.IsBillingActive, request.BillingActive)
            .Set(t => t.UserType, request.UserType)
            .UpdateAsync();

        var logRequest = new LogRequest
        {
            ChangeAction = ChangeAction.Update,
            ChangeType = ChangeType.UpdatePackage,
            UpdatedItemId = request.Id,
            UpdatedItemType = ItemType.Package,
            Username = request.Username,
            UpdatedFields = request.UpdatedFields
        };

        await _logger.LogChanges(logRequest);
    }

    public async Task<PackageFindResponse> ListTvPackages(PackageFindRequest request)
    {
        var query = _context.TvPackages.AsQueryable();

        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(c => c.Name.Contains(request.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == request.IsActive);
        }

        if (request.Id.HasValue)
        {
            query = query.Where(c => c.PackageId == request.Id);
        }

        if (request.Type.HasValue)
        {
            query = query.Where(c => c.TypeId == request.Type);
        }

        if (request.StartPrice.HasValue)
        {
            query = query.Where(c => c.Price >= request.StartPrice);
        }

        if (request.EndPrice.HasValue)
        {
            query = query.Where(c => c.Price <= request.EndPrice);
        }

        if (request.OrderAscending)
        {
            switch (request.OrderBy)
            {
                case OrderFields.Id: query = query.OrderBy(c => c.PackageId); break;
                case OrderFields.Name: query = query.OrderBy(c => c.Name); break;
            }
        }
        else
        {
            switch (request.OrderBy)
            {
                case OrderFields.Id: query = query.OrderByDescending(c => c.PackageId); break;
                case OrderFields.Name: query = query.OrderByDescending(c => c.Name); break;
            }
        }

        var paged = await query
            .Paginate(new PagingSettings()
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            }).ToArrayAsync();

        var result = new PackageFindResponse
        {
            TotalRows = query.Count(),
            PageRows = paged
        };
        return result;
    }

    public async Task<List<PackageMin>> GetPackagesById(IEnumerable<int> ids)
    {
        var query = _context.TvPackages
            .Where(c => c.PackageId.In(ids))
            .Select(c => new PackageMin
            {
                Id = c.PackageId,
                Name = c.Name,
                Active = c.IsActive,
                Type = c.TypeId
            });
        return await query.ToListAsync();
    }

    public async Task<PackageLinkedChannels> GetLinkedChannels(int packageId)
    {
        var packageChannelsQuery =
            from package in _context.TvPackageChannels.Where(p => p.PackageId == packageId)
            from channel in _context.TvChannels.Where(c => c.ChannelId == package.ChannelId)
            select channel;

        var availableChannelsQuery =
            from channel in _context.TvChannels
            where channel.IsActive == true
            where !(from package in _context.TvPackageChannels
                    where package.PackageId == packageId
                    select package.ChannelId)
                .Contains(channel.ChannelId)
            select channel;

        var result = new PackageLinkedChannels
        {
            PackageId = packageId,
            ChannelsInPackage = await packageChannelsQuery.Select(p => new ChannelMin { Id = p.ChannelId, MinistraId = p.MinistraId, Name = p.Name }).ToListAsync(),
            ChannelsNotInPackage = await availableChannelsQuery.Select(p => new ChannelMin { Id = p.ChannelId, MinistraId = p.MinistraId, Name = p.Name }).ToListAsync()
        };
        return result;
    }

    public async Task<List<TvPackageType>> GetPackageTypes()
    {
        var result =
            from types in _context.TvPackageTypes
            select types;

        return await result.ToListAsync();
    }
}