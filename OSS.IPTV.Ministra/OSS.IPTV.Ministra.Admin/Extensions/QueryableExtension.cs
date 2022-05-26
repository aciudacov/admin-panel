using OSS.IPTV.Ministra.Admin.Models.Shared;

namespace OSS.IPTV.Ministra.Admin.Extensions
{
    public static class QueryableExtension
    {
        public static IQueryable<TSource> Paginate<TSource>(this IQueryable<TSource> source, PagingSettings settings)
        {
            var take = settings.PageSize < 1 ? 10 : settings.PageSize;
            var skip = ((settings.PageNumber < 1 ? 1 : settings.PageNumber) - 1) * settings.PageSize;

            return source.Skip(skip).Take(take);
        }
    }
}
