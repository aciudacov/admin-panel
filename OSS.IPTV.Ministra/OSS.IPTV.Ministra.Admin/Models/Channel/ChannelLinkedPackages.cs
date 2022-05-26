using OSS.IPTV.Ministra.Admin.Models.Package;

namespace OSS.IPTV.Ministra.Admin.Models.Channel
{
    public class ChannelLinkedPackages
    {
        public int ChannelId { get; set; }
        public List<PackageMin> CurrentPackages { get; set; }
        public List<PackageMin> AvailablePackages { get; set; }
    }
}
