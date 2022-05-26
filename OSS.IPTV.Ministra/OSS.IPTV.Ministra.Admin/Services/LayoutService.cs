using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace OSS.IPTV.Ministra.Admin.Services
{
    public class LayoutService
    {
        private readonly ProtectedLocalStorage _storage;
        public bool IsDarkMode { get; private set; }

        public LayoutService(ProtectedLocalStorage storage)
        {
            _storage = storage;
        }

        public void SetDarkMode(bool value)
        {
            IsDarkMode = value;
        }

        public async Task ApplyUserPreferences()
        {
            try
            {
                var value = await _storage.GetAsync<bool>("DarkMode");
                if (value.Success)
                {
                    IsDarkMode = value.Value;
                }
            }
            catch
            {
                await _storage.SetAsync("DarkMode", false);
            }
        }

        public event EventHandler MajorUpdateOccured;

        private void OnMajorUpdateOccured() => MajorUpdateOccured?.Invoke(this, EventArgs.Empty);

        public async Task ToggleDarkMode()
        {
            IsDarkMode = !IsDarkMode;
            await _storage.SetAsync("DarkMode", IsDarkMode);
            OnMajorUpdateOccured();
        }
    }
}
