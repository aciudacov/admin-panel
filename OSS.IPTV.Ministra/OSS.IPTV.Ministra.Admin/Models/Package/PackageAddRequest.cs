namespace OSS.IPTV.Ministra.Admin.Models.Package
{
    public class PackageAddRequest
    {
        public int? Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public bool BillingActive { get; set; }
        public bool? UserType { get; set; }
        public int Duration { get; set; }
        public int PromoLimit { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
