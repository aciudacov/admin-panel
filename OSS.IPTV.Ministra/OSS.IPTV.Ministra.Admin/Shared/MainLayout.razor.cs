using Microsoft.AspNetCore.Components;
using OSS.IPTV.Ministra.Admin.Services;

namespace OSS.IPTV.Ministra.Admin.Shared
{
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        [Inject] private LayoutService LayoutService { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }

        protected override void OnInitialized()
        {
            LayoutService.MajorUpdateOccured += LayoutServiceOnMajorUpdateOccurred;
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                await ApplyUserPreferences();
                StateHasChanged();
            }
        }

        private async Task ApplyUserPreferences()
        {
            await LayoutService.ApplyUserPreferences();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            LayoutService.MajorUpdateOccured -= LayoutServiceOnMajorUpdateOccurred;
        }

        private void LayoutServiceOnMajorUpdateOccurred(object sender, EventArgs e) => StateHasChanged();
    }
}
