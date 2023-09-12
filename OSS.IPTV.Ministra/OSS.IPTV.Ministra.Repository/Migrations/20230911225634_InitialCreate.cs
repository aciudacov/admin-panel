using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OSS.IPTV.Ministra.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TvChannels",
                columns: table => new
                {
                    ChannelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinistraId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsHd = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvChannels", x => x.ChannelId);
                });

            migrationBuilder.CreateTable(
                name: "TvLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeAction = table.Column<int>(type: "int", nullable: false),
                    ChangeType = table.Column<int>(type: "int", nullable: false),
                    UpdatedItemId = table.Column<int>(type: "int", nullable: false),
                    UpdatedItemType = table.Column<int>(type: "int", nullable: false),
                    AffectedItemIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedFields = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvLogs", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "TvPackages",
                columns: table => new
                {
                    PackageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    PromoLimit = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsBillingActive = table.Column<bool>(type: "bit", nullable: true),
                    IsCustomizable = table.Column<bool>(type: "bit", nullable: false),
                    UserType = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvPackages", x => x.PackageId);
                });

            migrationBuilder.CreateTable(
                name: "TvPackageTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TvChannelTvPackage",
                columns: table => new
                {
                    ChannelsChannelId = table.Column<int>(type: "int", nullable: false),
                    PackagesPackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvChannelTvPackage", x => new { x.ChannelsChannelId, x.PackagesPackageId });
                    table.ForeignKey(
                        name: "FK_TvChannelTvPackage_TvChannels_ChannelsChannelId",
                        column: x => x.ChannelsChannelId,
                        principalTable: "TvChannels",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TvChannelTvPackage_TvPackages_PackagesPackageId",
                        column: x => x.PackagesPackageId,
                        principalTable: "TvPackages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TvPackageChannels",
                columns: table => new
                {
                    PackageChannelId = table.Column<int>(type: "int", nullable: false),
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_TvPackageChannels_TvChannels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "TvChannels",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TvPackageChannels_TvPackages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "TvPackages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TvChannelTvPackage_PackagesPackageId",
                table: "TvChannelTvPackage",
                column: "PackagesPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_TvPackageChannels_ChannelId",
                table: "TvPackageChannels",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_TvPackageChannels_PackageId",
                table: "TvPackageChannels",
                column: "PackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TvChannelTvPackage");

            migrationBuilder.DropTable(
                name: "TvLogs");

            migrationBuilder.DropTable(
                name: "TvPackageChannels");

            migrationBuilder.DropTable(
                name: "TvPackageTypes");

            migrationBuilder.DropTable(
                name: "TvChannels");

            migrationBuilder.DropTable(
                name: "TvPackages");
        }
    }
}
