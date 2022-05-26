using OSS.IPTV.Ministra.Admin.Models.Channel;

namespace OSS.IPTV.Ministra.Admin.Models.Package
{
    public class PackageEditChannelsRequest
    {
        public int PackageId { get; set; }
        public List<ChannelMin> ChannelsToAdd { get; set; } = new();
        public List<ChannelMin> ChannelsToRemove { get; set; } = new();
        public string Username { get; set; } = string.Empty;
    }
}
