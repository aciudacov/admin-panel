using Microsoft.EntityFrameworkCore;

namespace OSS.IPTV.Ministra.Repository.Entities
{
    [Keyless]
    public class Translations
    {
        public string PageName { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }
}
