using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OSS.IPTV.Ministra.Repository.Entities
{
    public class TvPackage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PackageId { get; set; } // int
        public string Name { get; set; } // varchar(150)
        public int? TypeId { get; set; } // int
        public decimal? Price { get; set; } // money
        public int Duration { get; set; } // int
        public int PromoLimit { get; set; } // int
        public bool IsActive { get; set; } // bit
        public bool? IsBillingActive { get; set; } // bit
        public bool IsCustomizable { get; set; } // bit
        public bool? UserType { get; set; } // bit
        public virtual List<TvChannel> Channels { get; } = new();
    }
}
