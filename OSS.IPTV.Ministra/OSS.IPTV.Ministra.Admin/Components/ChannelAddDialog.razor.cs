using Microsoft.AspNetCore.Components;
using MudBlazor;
using OSS.IPTV.Ministra.Admin.Interfaces;
using OSS.IPTV.Ministra.Admin.Models.Channel;
using OSS.IPTV.Ministra.Admin.Services.AppUser;

namespace OSS.IPTV.Ministra.Admin.Components
{
    public partial class ChannelAddDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] private AppUserProvider UserProvider { get; set; }
        [Inject] private IChannel ChannelService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        private ChannelAddRequest Request { get; } = new();
        private bool _formValid;

        async Task Submit()
        {
            Request.Username = UserProvider.User.Login;
            await ChannelService.AddTvChannel(Request);
            Snackbar.Add("Channel created");
            MudDialog.Close(DialogResult.Ok(Request.Name));
        }

        void Cancel() => MudDialog.Cancel();

        private string VerifyName(string channelName)
        {
            var isUnique = ChannelService.IsChannelNameUnique(channelName);
            return isUnique ? null : "Channel already exists";
        }

        private string VerifyId(int? ministraId)
        {
            if (ministraId is null) return null;

            var isUnique = ChannelService.IsMinistraIdUnique(ministraId);
            return isUnique ? null : "ID already exists";
        }
    }
}
