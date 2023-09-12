﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OSS.IPTV.Ministra.Repository;

#nullable disable

namespace OSS.IPTV.Ministra.Repository.Migrations
{
    [DbContext(typeof(IptvContext))]
    [Migration("20230912001445_Translations")]
    partial class Translations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OSS.IPTV.Ministra.Repository.Entities.Translations", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Translations");
                });

            modelBuilder.Entity("OSS.IPTV.Ministra.Repository.Entities.TvChannel", b =>
                {
                    b.Property<int>("ChannelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChannelId"));

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsHd")
                        .HasColumnType("bit");

                    b.Property<int?>("MinistraId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ChannelId");

                    b.ToTable("TvChannels");
                });

            modelBuilder.Entity("OSS.IPTV.Ministra.Repository.Entities.TvLogs", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LogId"));

                    b.Property<string>("AffectedItemIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ChangeAction")
                        .HasColumnType("int");

                    b.Property<int>("ChangeType")
                        .HasColumnType("int");

                    b.Property<DateTime>("LogTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedFields")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UpdatedItemId")
                        .HasColumnType("int");

                    b.Property<int>("UpdatedItemType")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LogId");

                    b.ToTable("TvLogs");
                });

            modelBuilder.Entity("OSS.IPTV.Ministra.Repository.Entities.TvPackage", b =>
                {
                    b.Property<int>("PackageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PackageId"));

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsBillingActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCustomizable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("PromoLimit")
                        .HasColumnType("int");

                    b.Property<int?>("TypeId")
                        .HasColumnType("int");

                    b.Property<bool?>("UserType")
                        .HasColumnType("bit");

                    b.HasKey("PackageId");

                    b.ToTable("TvPackages");
                });

            modelBuilder.Entity("OSS.IPTV.Ministra.Repository.Entities.TvPackageChannels", b =>
                {
                    b.Property<int>("ChannelId")
                        .HasColumnType("int");

                    b.Property<int>("PackageChannelId")
                        .HasColumnType("int");

                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.HasIndex("ChannelId");

                    b.HasIndex("PackageId");

                    b.ToTable("TvPackageChannels");
                });

            modelBuilder.Entity("OSS.IPTV.Ministra.Repository.Entities.TvPackageType", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.ToTable("TvPackageTypes");
                });

            modelBuilder.Entity("TvChannelTvPackage", b =>
                {
                    b.Property<int>("ChannelsChannelId")
                        .HasColumnType("int");

                    b.Property<int>("PackagesPackageId")
                        .HasColumnType("int");

                    b.HasKey("ChannelsChannelId", "PackagesPackageId");

                    b.HasIndex("PackagesPackageId");

                    b.ToTable("TvChannelTvPackage");
                });

            modelBuilder.Entity("OSS.IPTV.Ministra.Repository.Entities.TvPackageChannels", b =>
                {
                    b.HasOne("OSS.IPTV.Ministra.Repository.Entities.TvChannel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OSS.IPTV.Ministra.Repository.Entities.TvPackage", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");

                    b.Navigation("Package");
                });

            modelBuilder.Entity("TvChannelTvPackage", b =>
                {
                    b.HasOne("OSS.IPTV.Ministra.Repository.Entities.TvChannel", null)
                        .WithMany()
                        .HasForeignKey("ChannelsChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OSS.IPTV.Ministra.Repository.Entities.TvPackage", null)
                        .WithMany()
                        .HasForeignKey("PackagesPackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
