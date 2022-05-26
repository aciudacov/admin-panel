using OSS.IPTV.Ministra.Admin.Data.Models.Channel;
using OSS.IPTV.Ministra.Admin.Models.Channel;

namespace OSS.IPTV.Ministra.Admin.Interfaces
{
    public interface IChannel
    {
        Task<ChannelFindResponse> ListTvChannels(ChannelFindRequest request);
        Task<List<ChannelMin>> GetChannelsById(IEnumerable<int> ids);
        Task<int> AddTvChannel(ChannelAddRequest request);
        Task EditTvChannel(ChannelEditRequest request);
        Task DeleteTvChannel();
        Task<ChannelLinkedPackages> GetLinkedPackages(int channelId);
        bool IsChannelNameUnique(string channelName);
        bool IsMinistraIdUnique(int? ministraId);
        Task UpdateChannelPackages(ChannelEditPackagesRequest request);
    }
}
