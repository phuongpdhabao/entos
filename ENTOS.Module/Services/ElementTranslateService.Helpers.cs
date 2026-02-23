using System;
using System.Linq;

namespace ENTOS.Module.Services
{
    public partial class ElementTranslateService
    {
        private static bool IsBlockOverLimit(string block, string marked, int maxLength)
        {
            return (block + "\n").Length + marked.Length >= maxLength;
        }

        private static string[] SplitTranslatedLines(string fullTranslated)
        {
            return fullTranslated
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToArray();
        }
    }
}
