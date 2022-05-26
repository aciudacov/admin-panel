namespace OSS.IPTV.Ministra.Admin.Models.Shared
{
    public class PagingSettings
    {
        /// <summary>
        /// Requested page number
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Number of rows to display
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}
