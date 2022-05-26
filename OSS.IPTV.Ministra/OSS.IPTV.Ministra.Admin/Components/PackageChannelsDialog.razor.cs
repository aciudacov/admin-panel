using LinqToDB.Tools;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OSS.IPTV.Ministra.Admin.Interfaces;
using OSS.IPTV.Ministra.Admin.Models.Channel;
using OSS.IPTV.Ministra.Admin.Models.Package;
using OSS.IPTV.Ministra.Admin.Services.AppUser;

namespace OSS.IPTV.Ministra.Admin.Components
{
    public partial class PackageChannelsDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] private AppUserProvider UserProvider { get; set; }
        [Inject] private IPackage PackageService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Parameter] public int PackageId { get; set; }
        [Parameter] public string PackageName { get; set; } = string.Empty;
        private PackageLinkedChannels TableData { get; set; } = new();
        private string SearchAvailableChannels { get; set; } = string.Empty;
        private string SearchPackageChannels { get; set; } = string.Empty;

        private HashSet<ChannelMin> SelectedToAdd { get; set; } = new();
        private HashSet<ChannelMin> SelectedToRemove { get; set; } = new();
        private List<ChannelMin> AddPending { get; set; } = new();
        private List<ChannelMin> RemovePending { get; set; } = new();

        void OnCancelClick() => MudDialog.Cancel();

        protected override async Task OnInitializedAsync()
        {
            await LoadTableData();
        }

        private async Task LoadTableData()
        {
            TableData = await PackageService.GetLinkedChannels(PackageId);
            SelectedToAdd.Clear();
            SelectedToRemove.Clear();
        }

        private bool FilterAvailable(ChannelMin channel) => FilterFunc(channel, SearchAvailableChannels);
        private bool FilterPackage(ChannelMin channel) => FilterFunc(channel, SearchPackageChannels);
        private static bool FilterFunc(ChannelMin channel, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (channel.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        private void OnChannelAddClick()
        {
            foreach (var channel in SelectedToAdd)
            {
                if (!channel.In(RemovePending))
                {
                    AddPending.Add(channel);
                }
                else
                {
                    RemovePending.Remove(channel);
                }
            }
            TableData.ChannelsInPackage.AddRange(SelectedToAdd); //move channels to right
            TableData.ChannelsInPackage = TableData.ChannelsInPackage.OrderBy(p => p.Id).ToList(); //order added channels
            TableData.ChannelsNotInPackage.RemoveAll(p => p.In(SelectedToAdd)); //remove channels from left
            RemovePending.RemoveAll(p => p.In(AddPending)); //remove added channels from pending list
            SelectedToAdd.Clear(); //clear selected data
        }

        private void OnChannelRemoveClick()
        {
            foreach (var channel in SelectedToRemove)
            {
                if (!channel.In(AddPending))
                {
                    RemovePending.Add(channel);
                }
                else
                {
                    AddPending.Remove(channel);
                }
            }
            TableData.ChannelsNotInPackage.AddRange(SelectedToRemove);
            TableData.ChannelsNotInPackage = TableData.ChannelsNotInPackage.OrderBy(p => p.Id).ToList();
            TableData.ChannelsInPackage.RemoveAll(p => p.In(SelectedToRemove));
            AddPending.RemoveAll(p => p.In(RemovePending));
            SelectedToRemove.Clear();
        }

        private async Task OnApplyClick()
        {
            var request = new PackageEditChannelsRequest
            {
                Username = UserProvider.User.Login,
                PackageId = PackageId,
                ChannelsToAdd = AddPending,
                ChannelsToRemove = RemovePending
            };
            await PackageService.EditPackageChannels(request);
            Snackbar.Add("Package channels updated");
            MudDialog.Close(DialogResult.Ok(true));
        }
    }
}
