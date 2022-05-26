namespace OSS.IPTV.Ministra.Admin.Models.Shared
{
    /// <summary>
    /// Base type for paged response
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class PagedResponse<TItem>
    {
        /// <summary>
        /// Number of rows filtered by request  
        /// </summary>
        public int TotalRows { get; set; }

        /// <summary>
        /// The filtered data
        /// </summary>
        public TItem[] PageRows { get; set; } = Array.Empty<TItem>();
    }
}
