namespace ENTOS.Module.Services
{
    public partial class ArticleService
    {
        private static string GetDisplayValue(string value, string fallback)
        {
            return string.IsNullOrEmpty(value) ? fallback : value;
        }

        private static string BuildShareEntry(string name, string link)
        {
            return $"{name}\n{link}\n";
        }
    }
}
