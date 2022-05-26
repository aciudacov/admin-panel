using OSS.IPTV.Ministra.Admin.Models.Channel;

namespace OSS.IPTV.Ministra.Admin.Models.Package;

public class PackageLinkedChannels
{
    public int PackageId { get; set; }
    public List<ChannelMin> ChannelsInPackage { get; set; } = new();
    public List<ChannelMin> ChannelsNotInPackage { get; set; } = new();
}