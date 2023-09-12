using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OSS.IPTV.Ministra.Repository.Entities
{
    [Keyless]
    public class TvPackageChannels
    {
        public int PackageChannelId { get; set; }
        public int ChannelId { get; set; } // int
        public int PackageId { get; set; } // int

        public virtual TvChannel Channel { get; set; }
        public virtual TvPackage Package { get; set; }
    }
}
