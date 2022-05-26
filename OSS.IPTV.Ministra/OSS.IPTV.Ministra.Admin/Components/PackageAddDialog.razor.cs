using Microsoft.AspNetCore.Components;
using MudBlazor;
using OSS.IPTV.Ministra.Admin.Interfaces;
using OSS.IPTV.Ministra.Admin.Models.Package;
using OSS.IPTV.Ministra.Admin.Services.AppUser;
using OSS.IPTV.Ministra.Repository.Entities;

namespace OSS.IPTV.Ministra.Admin.Components
{
    public partial class PackageAddDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] private AppUserProvider UserProvider { get; set; }
        [Inject] private IPackage PackageService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Parameter] public PackageAddRequest Request { get; set; } = new();
        [Parameter] public List<TvPackageType> PackageTypes { get; set; } = new();

        private bool _formValid;
        private int _packageUserType;

        protected override void OnInitialized()
        {
            _packageUserType = Request.UserType switch
            {
                true => 1,
                false => 2,
                null => 3
            };
        }

        private void ModifyComponentValues(int? value)
        {
            Request.Type = value;

            if (Request.Type != 2)
            {
                Request.BillingActive = false;
            }

            if (Request.Type == 1)
            {
                Request.Price = 0;
                Request.Duration = 0;
                Request.PromoLimit = 0;
            }
        }

        async Task Submit()
        {
            Request.Username = UserProvider.User.Login;
            Request.UserType = _packageUserType switch
            {
                1 => true,
                2 => false,
                _ => null
            };
            await PackageService.AddTvPackage(Request);
            Snackbar.Add("Package created");
            MudDialog.Close(DialogResult.Ok(Request.Name));
        }

        void Cancel() => MudDialog.Cancel();
    }
}
