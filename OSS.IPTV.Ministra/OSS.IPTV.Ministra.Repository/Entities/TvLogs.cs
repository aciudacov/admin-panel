using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OSS.IPTV.Ministra.Repository.Entities
{
    public partial class TvLogs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }
        public int ChangeAction { get; set; }
        public int ChangeType { get; set; }
        public int UpdatedItemId { get; set; }
        public int UpdatedItemType { get; set; }
        public string AffectedItemIds { get; set; }
        public string UpdatedFields { get; set; }
        public DateTime LogTime { get; set; }
        public string Username { get; set; }
    }
}
