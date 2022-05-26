namespace OSS.IPTV.Ministra.Admin.Models.Package
{
    public class PackageMin
    {
        public string Name { get; set; } = string.Empty;
        public int Id { get; set; }
        public int? Type { get; set; }
        public bool Active { get; set; }
    }
}
