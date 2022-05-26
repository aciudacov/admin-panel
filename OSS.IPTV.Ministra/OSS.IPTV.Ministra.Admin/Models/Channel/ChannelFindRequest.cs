using OSS.IPTV.Ministra.Admin.Models.Shared;

namespace OSS.IPTV.Ministra.Admin.Data.Models.Channel
{
    public class ChannelFindRequest : PagingSettings
    {
        public string Name { get; set; }
        public int? MinistraId { get; set; }
        public bool? IsActive { get; set; }
        public OrderFields OrderBy { get; set; } = OrderFields.Id;
        public bool OrderAscending { get; set; } = true;
    }

    public enum OrderFields
    {
        Id = 1,
        Name = 2,
        Ministra = 3
    }
}
