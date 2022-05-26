using System.Reflection;
using System.Text.Json;

namespace OSS.IPTV.Ministra.Admin.Extensions
{
    public static class ClassExtensions
    {
        public static List<string> GetModifiedProperties<T>(this T currentEntity, T originalEntity)
        {
            var modifiedFields = new List<string>();
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                var prop1 = prop.GetValue(currentEntity, null);
                var prop2 = prop.GetValue(originalEntity, null);

                if (!Equals(prop1, prop2))
                {
                    modifiedFields.Add(prop.Name);
                }
            }

            return modifiedFields;
        }

        public static T JsonClone<T>(this T obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
