namespace OSS.IPTV.Ministra.Admin.Models.Channel
{
    public class ChannelEditRequest
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; } = string.Empty;
        public int? MinistraId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsHd { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<string> UpdatedFields { get; set; } = new();
    }
}
