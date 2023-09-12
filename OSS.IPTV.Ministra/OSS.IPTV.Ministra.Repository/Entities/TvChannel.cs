using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OSS.IPTV.Ministra.Repository.Entities
{
    public class TvChannel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChannelId { get; set; } // int
        public string Name { get; set; } // nvarchar(250)
        public int? MinistraId { get; set; } // int
        public bool? IsActive { get; set; } // bit
        public bool? IsHd { get; set; } // bit
        public virtual List<TvPackage> Packages { get; } = new();
    }
}
