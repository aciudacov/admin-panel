using OSS.IPTV.Ministra.Admin.Models.Shared;
using static OSS.IPTV.Ministra.Admin.Enums.LogEnums;

namespace OSS.IPTV.Ministra.Admin.Models.Log
{
    public class LogFindRequest : PagingSettings
    {
        public int? ItemId { get; set; }
        public ItemType? ItemType { get; set; }
        public ChangeType? ChangeType { get; set; }
        public ChangeAction? ActionType { get; set; }
        public DateTime? LogFrom { get; set; }
        public DateTime? LogTo { get; set; }
    }
}
