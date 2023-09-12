using Microsoft.EntityFrameworkCore;

namespace OSS.IPTV.Ministra.Repository.Entities
{
    [Keyless]
    public class TvPackageType
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
    }
}
