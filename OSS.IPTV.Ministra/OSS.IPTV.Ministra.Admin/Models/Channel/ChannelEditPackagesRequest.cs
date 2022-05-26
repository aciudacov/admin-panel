using OSS.IPTV.Ministra.Admin.Models.Package;

namespace OSS.IPTV.Ministra.Admin.Models.Channel
{
    public class ChannelEditPackagesRequest
    {
        public int ChannelId { get; set; }
        public List<PackageMin> RemoveFromPackages { get; set; } = new();
        public List<PackageMin> AddToPackages { get; set; } = new();
        public string Username { get; set; } = string.Empty;
    }
}
