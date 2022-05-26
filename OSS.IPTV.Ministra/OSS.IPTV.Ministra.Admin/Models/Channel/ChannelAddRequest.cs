namespace OSS.IPTV.Ministra.Admin.Models.Channel
{
    public class ChannelAddRequest
    {
        public string Name { get; set; }
        public int? MinistraId { get; set; }
        public bool IsActive { get; set; }
        public bool IsHd { get; set; }
        public string Username { get; set; }
    }
}
