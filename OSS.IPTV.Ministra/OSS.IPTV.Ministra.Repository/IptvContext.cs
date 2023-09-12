using Microsoft.EntityFrameworkCore;
using OSS.IPTV.Ministra.Repository.Entities;

namespace OSS.IPTV.Ministra.Repository;

public class IptvContext : DbContext
{
    public DbSet<TvChannel> TvChannels { get; set; }
    public DbSet<TvPackageChannels> TvPackageChannels { get; set; }
    public DbSet<TvPackage> TvPackages { get; set; }
    public DbSet<TvPackageType> TvPackageTypes { get; set; }
    public DbSet<TvLogs> TvLogs { get; set; }
    public DbSet<Translations> Translations { get; set; }

    public IptvContext(DbContextOptions<IptvContext> options) : base(options) { }
}