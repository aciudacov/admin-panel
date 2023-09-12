using Microsoft.AspNetCore.Components;
using MudBlazor;
using OSS.IPTV.Ministra.Admin.Components;
using OSS.IPTV.Ministra.Admin.Data.Models.Channel;
using OSS.IPTV.Ministra.Admin.Interfaces;
using OSS.IPTV.Ministra.Admin.Models.Channel;
using OSS.IPTV.Ministra.Repository.Entities;

namespace OSS.IPTV.Ministra.Admin.Pages
{
    public partial class Index
    {
        [Inject] private IChannel ChannelService { get; set; }
        [Inject] private IDialogService Dialog { get; set; }
        private ChannelFindResponse Response { get; set; } = new();
        private ChannelFilterModel Filter { get; set; } = new();
        private MudTable<TvChannel> Table { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await Localization.CachePageStrings("index");
        }

        protected override void OnAfterRender(bool firstRender)
        {
            Table.SetRowsPerPage(15);
        }

        private async Task<TableData<TvChannel>> LoadItems(TableState state)
        {
            var request = new ChannelFindRequest()
            {
                Name = Filter.Name,
                IsActive = Filter.State switch
                {
                    2 => true,
                    3 => false,
                    _ => null
                },
                MinistraId = Filter.StalkerId,
                PageSize = state.PageSize,
                PageNumber = state.Page + 1,
                OrderBy = state.SortLabel switch
                {
                    "channel_name" => OrderFields.Name,
                    "channel_ministra" => OrderFields.Ministra,
                    _ => OrderFields.Id
                },
                OrderAscending = state.SortDirection != SortDirection.Descending
            };
            Response = await ChannelService.ListTvChannels(request);
            return new TableData<TvChannel> { Items = Response.PageRows, TotalItems = Response.TotalRows };
        }

        private async Task OnApplyClick()
        {
            await Table.ReloadServerData();
        }

        private async Task OnAddClick()
        {
            var options = new DialogOptions()
            {
                DisableBackdropClick = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                CloseButton = true
            };
            var dialog = Dialog.Show<ChannelAddDialog>("Adding new channel", options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                Filter.Name = result.Data.ToString();
                await Table.ReloadServerData();
            }
        }

        private async Task OnResetClick()
        {
            Filter = new();
            await Table.ReloadServerData();
        }

        private async Task OnEditClick(TvChannel channel)
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
                { "Channel", channel }
            };
            var dialog = Dialog.Show<ChannelEditDialog>("", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Table.ReloadServerData();
            }
        }

        private void OnPackagesClick(int channelId, string channelName)
        {
            var options = new DialogOptions()
            {
                DisableBackdropClick = true,
                NoHeader = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = true,
                CloseButton = true
            };
            var parameters = new DialogParameters
            {
                { "ChannelId", channelId },
                { "ChannelName", channelName}
            };
            Dialog.Show<ChannelLinkedPackagesDialog>("", parameters, options);
        }

        private sealed class ChannelFilterModel
        {
            public string Name { get; set; } = string.Empty;
            public int? StalkerId { get; set; }
            public int State { get; set; } = 1;
        }
    }
}
