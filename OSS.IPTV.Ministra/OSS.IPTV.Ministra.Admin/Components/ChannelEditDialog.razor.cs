using Microsoft.AspNetCore.Components;
using MudBlazor;
using OSS.IPTV.Ministra.Admin.Extensions;
using OSS.IPTV.Ministra.Admin.Interfaces;
using OSS.IPTV.Ministra.Admin.Models.Channel;
using OSS.IPTV.Ministra.Admin.Services.AppUser;
using OSS.IPTV.Ministra.Repository.Entities;

namespace OSS.IPTV.Ministra.Admin.Components
{
    public partial class ChannelEditDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] private AppUserProvider UserProvider { get; set; }
        [Inject] private IChannel ChannelService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Parameter] public TvChannel Channel { get; set; } = new();

        private bool _formValid;

        private TvChannel _channelCopy = new();

        protected override void OnInitialized()
        {
            _channelCopy = Channel.JsonClone();
        }

        async Task Submit()
        {
            if (_channelCopy.GetModifiedProperties(Channel).Any())
            {
                var request = new ChannelEditRequest()
                {
                    Username = UserProvider.User.Login,
                    ChannelId = _channelCopy.ChannelId,
                    ChannelName = _channelCopy.Name,
                    MinistraId = _channelCopy.MinistraId,
                    IsActive = _channelCopy.IsActive ?? false,
                    IsHd = _channelCopy.IsHd ?? false,
                    UpdatedFields = _channelCopy.GetModifiedProperties(Channel)
                };
                await ChannelService.EditTvChannel(request);
                Snackbar.Add("Channel updated");
                MudDialog.Close(DialogResult.Ok(true));
            }
            MudDialog.Cancel();
        }

        void Cancel() => MudDialog.Cancel();

        private string VerifyName(string channelName)
        {
            var isUnique = ChannelService.IsChannelNameUnique(channelName);
            if (_channelCopy.Name != Channel.Name && !isUnique)
                return "Channel already exists";
            return null;
        }

        private string VerifyId(int? ministraId)
        {
            var isUnique = ChannelService.IsMinistraIdUnique(ministraId);
            if (_channelCopy.MinistraId != Channel.MinistraId && !isUnique)
                return "ID already exists";
            return null;
        }
    }
}
