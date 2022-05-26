using Microsoft.AspNetCore.Components;
using MudBlazor;
using OSS.IPTV.Ministra.Admin.Components;
using OSS.IPTV.Ministra.Admin.Interfaces;
using OSS.IPTV.Ministra.Admin.Models.Channel;
using OSS.IPTV.Ministra.Admin.Models.Log;
using OSS.IPTV.Ministra.Admin.Models.Package;
using OSS.IPTV.Ministra.Repository.Entities;
using static OSS.IPTV.Ministra.Admin.Enums.LogEnums;
using ILogger = OSS.IPTV.Ministra.Admin.Interfaces.ILogger;

namespace OSS.IPTV.Ministra.Admin.Pages
{
    public partial class Logs
    {
        [Inject] private ILogger LoggerService { get; set; }
        [Inject] private IDialogService Dialog { get; set; }
        [Inject] private IPackage PackageService { get; set; }
        [Inject] private IChannel ChannelService { get; set; }
        public LogFindRequest Filter { get; set; } = new();
        public MudTable<TvLogsNamed> Table { get; set; } = new();
        [Parameter] public int? Type { get; set; }
        [Parameter] public int? Id { get; set; }
        public List<ChannelMin> Channels { get; set; } = new();
        public List<PackageMin> Packages { get; set; } = new();

        protected override void OnAfterRender(bool firstRender)
        {
            Table.SetRowsPerPage(25);
            if (Id != null && Type != null)
            {
                Filter.ItemId = Id;
                Filter.ItemType = (ItemType)Type;
            }
        }

        private async Task<TableData<TvLogsNamed>> LoadItems(TableState state)
        {
            var request = new LogFindRequest()
            {
                ItemId = Filter.ItemId,
                ItemType = Filter.ItemType,
                ChangeType = Filter.ChangeType,
                ActionType = Filter.ActionType,
                LogFrom = Filter.LogFrom,
                LogTo = Filter.LogTo,
                PageSize = state.PageSize,
                PageNumber = state.Page + 1
            };
            var response = await LoggerService.FindLogs(request);

            var channelIds = response.PageRows
                .Where(r => r.UpdatedItemType == (int)ItemType.Channel ||
                r.ChangeType == (int)ChangeType.AddChannelToPackages ||
                r.ChangeType == (int)ChangeType.RemoveChannelFromPackages)
                .Select(c => c.UpdatedItemId);

            var packageIds = response.PageRows
                .Where(r => r.UpdatedItemType == (int)ItemType.Package ||
                r.ChangeType == (int)ChangeType.AddChannelsToPackage ||
                r.ChangeType == (int)ChangeType.RemoveChannelsFromPackage)
                .Select(c => c.UpdatedItemId);

            Channels = await ChannelService.GetChannelsById(channelIds);
            Packages = await PackageService.GetPackagesById(packageIds);

            var compositeLogs = response.PageRows.Select(r => new TvLogsNamed(r, GetItemName(r)));

            return new TableData<TvLogsNamed> { Items = compositeLogs, TotalItems = response.TotalRows };
        }

        private string GetItemName(TvLogs item)
        {
            if ((ItemType)item.UpdatedItemType == ItemType.Channel ||
                                item.ChangeType == (int)ChangeType.AddChannelToPackages ||
                                item.ChangeType == (int)ChangeType.RemoveChannelFromPackages)
            {
                return Channels.Single(c => c.Id == item.UpdatedItemId).Name;
            }
            else if ((ItemType)item.UpdatedItemType == ItemType.Package ||
                                item.ChangeType == (int)ChangeType.AddChannelsToPackage ||
                                item.ChangeType == (int)ChangeType.RemoveChannelsFromPackage)
            {
                return Packages.Single(c => c.Id == item.UpdatedItemId).Name;
            }
            return "";
        }

        private async Task OnApplyClick()
        {
            await Table.ReloadServerData();
        }

        private async Task OnResetClick()
        {
            Filter = new();
            await Table.ReloadServerData();
        }

        private void OnDetailsClick(TvLogsNamed log)
        {
            var options = new DialogOptions()
            {
                DisableBackdropClick = true,
                NoHeader = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                CloseButton = true
            };
            var parameters = new DialogParameters
            {
                { "Log", log }
            };
            Dialog.Show<LogDetails>($"Log details", parameters, options);
        }

        public class TvLogsNamed : TvLogs
        {
            public string ItemName { get; set; }
            public TvLogsNamed(TvLogs log, string name)
            {
                ChangeAction = log.ChangeAction;
                ChangeType = log.ChangeType;
                UpdatedItemId = log.UpdatedItemId;
                UpdatedItemType = log.UpdatedItemType;
                AffectedItemIds = log.AffectedItemIds;
                UpdatedFields = log.UpdatedFields;
                LogTime = log.LogTime;
                Username = log.Username;
                ItemName = name;
            }
        }
    }
}
