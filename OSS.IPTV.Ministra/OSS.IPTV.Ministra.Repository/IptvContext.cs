using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;
using Microsoft.Extensions.Options;
using OSS.IPTV.Ministra.Repository.Entities;

namespace OSS.IPTV.Ministra.Repository;

public class IptvContext : DataConnection
{
    public ITable<TvChannel> TvChannels => GetTable<TvChannel>();
    public ITable<TvPackageChannels> TvPackageChannels => GetTable<TvPackageChannels>();
    public ITable<TvPackage> TvPackages => GetTable<TvPackage>();
    public ITable<TvPackageType> TvPackageTypes => GetTable<TvPackageType>();
    public ITable<TvLogs> TvLogs => GetTable<TvLogs>();

    public IptvContext(IOptions<IptvContextOptions> options) : base(ProviderName.SqlServer2017, options.Value.ConnectionString, Schema.Value)
    {

    }

    private static readonly Lazy<MappingSchema> Schema = new(SchemaFactory);
    private static MappingSchema SchemaFactory()
    {
        var builder = MappingSchema.Default.GetFluentMappingBuilder();

        builder.Entity<TvChannel>()
               .HasTableName("tv_channels")
               .HasPrimaryKey(t => t.ChannelId)
               .HasIdentity(t => t.ChannelId)
               .Property(t => t.ChannelId).HasColumnName("idchannel")
               .Property(t => t.Name).HasColumnName("name")
               .Property(t => t.MinistraId).HasColumnName("id_stalker")
               .Property(t => t.IsActive).HasColumnName("f_active")
               .Property(t => t.IsHd).HasColumnName("f_hd");

        builder.Entity<TvPackage>()
            .HasTableName("tv_packages")
            .HasPrimaryKey(t => t.PackageId)
            .HasIdentity(t => t.PackageId)
            .Property(t => t.PackageId).HasColumnName("idpackage")
            .Property(t => t.Name).HasColumnName("name")
            .Property(t => t.TypeId).HasColumnName("idtype")
            .Property(t => t.Price).HasColumnName("price")
            .Property(t => t.Duration).HasColumnName("duration")
            .Property(t => t.PromoLimit).HasColumnName("promo_limit")
            .Property(t => t.IsActive).HasColumnName("f_active")
            .Property(t => t.IsBillingActive).HasColumnName("f_billing_active")
            .Property(t => t.IsCustomizable).HasColumnName("f_customizable")
            .Property(t => t.UserType).HasColumnName("usertype");

        builder.Entity<TvPackageChannels>()
            .HasTableName("tv_channels_packages")
            .Property(t => t.ChannelId).HasColumnName("idchannel")
            .Property(t => t.PackageId).HasColumnName("idpackage");

        builder.Entity<TvPackageType>()
            .HasTableName("tv_package_types")
            .Property(t => t.TypeId).HasColumnName("idtype")
            .Property(t => t.Name).HasColumnName("name");

        builder.Entity<TvLogs>()
               .HasTableName("tv_logs");

        return builder.MappingSchema;
    }
}