using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OSS.IPTV.Ministra.Admin.Services.AppUser;

namespace OSS.IPTV.Ministra.Admin
{
    public partial class App
    {
        [Parameter] public AppState AppState { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private AppUserProvider AppUserProvider { get; set; }

        protected override Task OnInitializedAsync()
        {
            AppUserProvider.User = AppState.AppUser;

            return base.OnInitializedAsync();
        }

        private void OnLoginClick(MouseEventArgs arg)
        {
            var url = Navigation.Uri;
            var returnUrl = Navigation.ToBaseRelativePath(url);
            var redirectUrl = "account/login";

            if (returnUrl is { Length: > 0 })
            {
                redirectUrl += $"?returnUrl={returnUrl}";
            }

            Navigation.NavigateTo(redirectUrl, true);
        }
    }
}
