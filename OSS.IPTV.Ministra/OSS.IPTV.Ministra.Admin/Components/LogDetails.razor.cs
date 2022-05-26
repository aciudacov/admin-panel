using Microsoft.AspNetCore.Components;
using MudBlazor;
using OSS.IPTV.Ministra.Admin.Interfaces;
using OSS.IPTV.Ministra.Admin.Models.Channel;
using OSS.IPTV.Ministra.Admin.Models.Package;
using static OSS.IPTV.Ministra.Admin.Enums.LogEnums;
using static OSS.IPTV.Ministra.Admin.Pages.Logs;

namespace OSS.IPTV.Ministra.Admin.Components
{
    public partial class LogDetails
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] private IPackage PackageService { get; set; }
        [Inject] private IChannel ChannelService { get; set; }
        [Parameter] public TvLogsNamed Log { get; set; }
        [Parameter] public string ItemName { get; set; } = string.Empty;
        public List<ChannelMin> Channels { get; set; } = new();
        public List<PackageMin> Packages { get; set; } = new();

        void OnCloseClick() => MudDialog.Close();

        protected override async Task OnInitializedAsync()
        {
            if (Log.AffectedItemIds != null)
            {
                var ids = Log.AffectedItemIds.Split(',');

                if ((Log.ChangeType == (int)ChangeType.AddChannelsToPackage || Log.ChangeType == (int)ChangeType.RemoveChannelsFromPackage))
                {
                    Channels = await GetAffectedChannels(ids);
                }
                else if ((Log.ChangeType == (int)ChangeType.AddChannelToPackages || Log.ChangeType == (int)ChangeType.RemoveChannelsFromPackage))
                {
                    Packages = await GetAffectedPackages(ids);
                }
            }
        }

        public async Task<List<ChannelMin>> GetAffectedChannels(string[] channelIds)
        {
            var ids = channelIds.Select(i => int.Parse(i));
            return await ChannelService.GetChannelsById(ids);
        }

        public async Task<List<PackageMin>> GetAffectedPackages(string[] packageIds)
        {
            var ids = packageIds.Select(i => int.Parse(i));
            return await PackageService.GetPackagesById(ids);
        }
    }
}
