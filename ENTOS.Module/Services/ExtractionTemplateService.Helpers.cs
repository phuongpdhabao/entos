using System.Collections.Generic;
using System.Linq;

namespace ENTOS.Module.Services
{
    public partial class ExtractionTemplateService
    {
        private static List<string> GetLayoutKeys(ExtractionTemplate template, params string[] layouts)
        {
            return template.ExtractionKeyList
                .Where(k => layouts.Contains(k.DataLayout.GetName()))
                .Select(k => k.Name)
                .OrderBy(x => x)
                .ToList();
        }

        private static Dictionary<string, string> BuildKeyDictionary(IEnumerable<string> keys)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                if (!dictionary.ContainsKey(key))
                    dictionary[key] = string.Empty;
            }
            return dictionary;
        }
    }
}
