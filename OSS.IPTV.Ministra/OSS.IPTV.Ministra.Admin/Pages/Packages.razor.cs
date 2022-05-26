using Microsoft.AspNetCore.Components;
using MudBlazor;
using OSS.IPTV.Ministra.Admin.Components;
using OSS.IPTV.Ministra.Admin.Interfaces;
using OSS.IPTV.Ministra.Admin.Models.Package;
using OSS.IPTV.Ministra.Repository.Entities;

namespace OSS.IPTV.Ministra.Admin.Pages
{
    public partial class Packages
    {
        [Inject] private IPackage PackageService { get; set; }
        [Inject] private IDialogService Dialog { get; set; }
        private PackageFindResponse Response { get; set; } = new();
        private PackageFilterModel Filter { get; set; } = new();
        private MudTable<TvPackage> Table { get; set; } = new();
        private List<TvPackageType> PackageTypes { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            PackageTypes = await PackageService.GetPackageTypes();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            Table.SetRowsPerPage(15);
        }

        private async Task<TableData<TvPackage>> LoadItems(TableState state)
        {
            var request = new PackageFindRequest()
            {
                Name = Filter.Name,
                IsActive = Filter.State switch
                {
                    2 => true,
                    3 => false,
                    _ => null
                },
                Type = Filter.Type,
                Id = Filter.Id,
                PageSize = state.PageSize,
                PageNumber = state.Page + 1,
                OrderBy = state.SortLabel switch
                {
                    "package_name" => OrderFields.Name,
                    _ => OrderFields.Id
                },
                OrderAscending = state.SortDirection != SortDirection.Descending
            };
            Response = await PackageService.ListTvPackages(request);
            return new TableData<TvPackage> { Items = Response.PageRows, TotalItems = Response.TotalRows };
        }

        private async Task OnApplyClick()
        {
            await Table.ReloadServerData();
        }

        private async Task OnAddClick()
        {
            var parameters = new DialogParameters
            {
                { "PackageTypes", PackageTypes }
            };
            var options = new DialogOptions()
            {
                DisableBackdropClick = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                CloseButton = true
            };
            var dialog = Dialog.Show<PackageAddDialog>("Adding new package", parameters, options);
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

        private async Task OnEditClick(TvPackage package)
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
                { "Package", package }
            };
            var dialog = Dialog.Show<PackageEditDialog>($"Editing package \"{package.Name}\"", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Table.ReloadServerData();
            }
        }

        private void OnChannelsClick(int packageId, string packageName)
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
                { "PackageId", packageId },
                { "PackageName", packageName}
            };
            Dialog.Show<PackageChannelsDialog>($"Channels for package \"{packageName}\"", parameters, options);
        }

        private sealed class PackageFilterModel
        {
            public string Name { get; set; } = string.Empty;
            public int? Id { get; set; }
            public int State { get; set; } = 1;
            public int? Type { get; set; }
        }
    }
}
