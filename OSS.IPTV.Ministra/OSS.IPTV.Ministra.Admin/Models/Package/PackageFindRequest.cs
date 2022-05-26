using OSS.IPTV.Ministra.Admin.Models.Shared;

namespace OSS.IPTV.Ministra.Admin.Models.Package
{
    public class PackageFindRequest : PagingSettings
    {
        public string Name { get; set; } = string.Empty;
        public int? Id { get; set; }
        public int? Type { get; set; }
        public decimal? StartPrice { get; set; }
        public decimal? EndPrice { get; set; }
        public bool? IsActive { get; set; }
        public OrderFields OrderBy { get; set; } = OrderFields.Id;
        public bool OrderAscending { get; set; } = true;
    }

    public enum OrderFields
    {
        Id = 1,
        Name = 2
    }
}
