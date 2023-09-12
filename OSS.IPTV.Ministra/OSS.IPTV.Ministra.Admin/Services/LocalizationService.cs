using Microsoft.EntityFrameworkCore;
using OSS.IPTV.Ministra.Repository;

namespace OSS.IPTV.Ministra.Admin.Services
{
    public class LocalizationService
    {
        private readonly IptvContext _context;
        private readonly Dictionary<string, Dictionary<string, string>> _localizationDictionary = new();
        public LocalizationService(IptvContext context)
        {
            _context = context;
        }

        public async Task CachePageStrings(string pageName)
        {
            if (!_localizationDictionary.ContainsKey(pageName))
            {
                var pageTranslations = await _context.Translations
                    .Where(s => s.PageName.Equals(pageName))
                    .ToDictionaryAsync(s => s.Key, s => s.Value);

                if (pageTranslations.Any())
                {
                    _localizationDictionary.Add(pageName, pageTranslations);
                }
            }
        }

        public string GetString(string pageName, string key)
        {
            if (!_localizationDictionary.ContainsKey(pageName))
            {
                var result = Resources.Fallback.ResourceManager.GetString(key);
                return result;
            }
            else
            {
                var pageDictionary = _localizationDictionary[pageName];
                var result = pageDictionary[key];
                return result;
            }
        }
    }
}
