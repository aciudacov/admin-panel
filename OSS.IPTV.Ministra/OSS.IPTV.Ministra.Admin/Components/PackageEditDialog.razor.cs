using Microsoft.AspNetCore.Components;
using MudBlazor;
using OSS.IPTV.Ministra.Admin.Extensions;
using OSS.IPTV.Ministra.Admin.Interfaces;
using OSS.IPTV.Ministra.Admin.Models.Package;
using OSS.IPTV.Ministra.Admin.Services.AppUser;
using OSS.IPTV.Ministra.Repository.Entities;

namespace OSS.IPTV.Ministra.Admin.Components
{
    public partial class PackageEditDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] private AppUserProvider UserProvider { get; set; }
        [Inject] private IPackage PackageService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Parameter] public TvPackage Package { get; set; } = new();

        private bool _formValid;
        private int _packageUserType;
        private TvPackage _packageCopy = new();

        protected override void OnInitialized()
        {
            _packageCopy = Package.JsonClone();
            _packageUserType = Package.UserType switch
            {
                true => 1,
                false => 2,
                null => 3
            };
        }

        async Task Submit()
        {
            if (_packageCopy.GetModifiedProperties(Package).Any())
            {
                var request = new PackageEditRequest()
                {
                    Username = UserProvider.User.Login,
                    Id = _packageCopy.PackageId,
                    Name = _packageCopy.Name,
                    Type = _packageCopy.TypeId,
                    Active = _packageCopy.IsActive,
                    Duration = _packageCopy.Duration,
                    PromoLimit = _packageCopy.PromoLimit,
                    BillingActive = _packageCopy.IsBillingActive ?? false,
                    Price = _packageCopy.Price,
                    UpdatedFields = _packageCopy.GetModifiedProperties(Package),
                    UserType = _packageUserType switch
                    {
                        1 => true,
                        2 => false,
                        _ => null
                    }
                };
                if (request.UserType != Package.UserType)
                {
                    request.UpdatedFields.Add("Usertype");
                }

                await PackageService.EditTvPackage(request);
                Snackbar.Add("Package updated");
                MudDialog.Close(DialogResult.Ok(true));
            }
            MudDialog.Cancel();
        }

        void Cancel() => MudDialog.Cancel();
    }
}
