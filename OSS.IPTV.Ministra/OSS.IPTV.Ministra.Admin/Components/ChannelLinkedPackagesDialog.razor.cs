using LinqToDB.Tools;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OSS.IPTV.Ministra.Admin.Interfaces;
using OSS.IPTV.Ministra.Admin.Models.Channel;
using OSS.IPTV.Ministra.Admin.Models.Package;
using OSS.IPTV.Ministra.Admin.Services.AppUser;
using OSS.IPTV.Ministra.Repository.Entities;

namespace OSS.IPTV.Ministra.Admin.Components
{
    public partial class ChannelLinkedPackagesDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] private AppUserProvider UserProvider { get; set; }
        [Inject] private IChannel ChannelService { get; set; }
        [Inject] private IPackage PackageService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Parameter] public int ChannelId { get; set; }
        [Parameter] public string ChannelName { get; set; } = string.Empty;
        private ChannelLinkedPackages Response { get; set; } = new();
        private string SearchAvailablePackages { get; set; } = string.Empty;
        private string SearchExistingPackages { get; set; } = string.Empty;
        private List<TvPackageType> PackageTypes { get; set; } = new();

        private HashSet<PackageMin> SelectedPackagesWithoutChannel { get; set; } = new(); //selected items from left
        private HashSet<PackageMin> SelectedPackagesWithChannel { get; set; } = new(); //selected items from right
        private List<PackageMin> AddPending { get; set; } = new(); //changelist from left
        private List<PackageMin> RemovePending { get; set; } = new(); //changelist from right

        private void OnCancelClick() => MudDialog.Cancel();

        protected override async Task OnInitializedAsync()
        {
            Response = await ChannelService.GetLinkedPackages(ChannelId);
            PackageTypes = await PackageService.GetPackageTypes();
        }
        private bool FilterAvailable(PackageMin channel) => FilterFunc(channel, SearchAvailablePackages);
        private bool FilterExisting(PackageMin channel) => FilterFunc(channel, SearchExistingPackages);
        private static bool FilterFunc(PackageMin package, string searchString)
        {
            return string.IsNullOrWhiteSpace(searchString) || package.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase);
        }

        private void OnPackageAddClick()
        {
            AddPending.AddRange(SelectedPackagesWithoutChannel); //add packages to changelist
            RemovePending.RemoveAll(p => p.Id.In(AddPending.Select(a => a.Id))); //remove added packages from "remove" list
            Response.CurrentPackages.AddRange(SelectedPackagesWithoutChannel); //update grid
            Response.CurrentPackages = Response.CurrentPackages.OrderBy(p => p.Id).ToList(); //order added packages
            Response.AvailablePackages.RemoveAll(p => p.In(SelectedPackagesWithoutChannel)); //remove moved packages
            SelectedPackagesWithoutChannel.Clear(); //clear selected data
        }

        private void OnPackageRemoveClick()
        {
            RemovePending.AddRange(SelectedPackagesWithChannel); //add packages to changelist
            AddPending.RemoveAll(p => p.Id.In(RemovePending.Select(a => a.Id)));
            Response.AvailablePackages.AddRange(SelectedPackagesWithChannel);
            Response.AvailablePackages = Response.AvailablePackages.OrderBy(p => p.Id).ToList();
            Response.CurrentPackages.RemoveAll(p => p.In(SelectedPackagesWithChannel));
            SelectedPackagesWithChannel.Clear();
        }

        private async Task OnApplyClick()
        {
            var request = new ChannelEditPackagesRequest()
            {
                Username = UserProvider.User.Login,
                ChannelId = ChannelId,
                AddToPackages = AddPending,
                RemoveFromPackages = RemovePending
            };

            await ChannelService.UpdateChannelPackages(request);
            Snackbar.Add("Packages updated");
            MudDialog.Close(DialogResult.Ok(true));
        }
    }
}
