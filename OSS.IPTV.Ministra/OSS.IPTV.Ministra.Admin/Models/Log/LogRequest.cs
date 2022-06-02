using static OSS.IPTV.Ministra.Admin.Enums.LogEnums;

namespace OSS.IPTV.Ministra.Admin.Models.Log
{
    public class LogRequest
    {
        public ChangeAction ChangeAction { get; set; }
        public ChangeType ChangeType { get; set; }
        public int UpdatedItemId { get; set; }
        public ItemType UpdatedItemType { get; set; }
        public int[] AffectedItemIds { get; set; }
        public List<string> UpdatedFields { get; set; } = new();
        public string Username { get; set; } = string.Empty;
    }
}
