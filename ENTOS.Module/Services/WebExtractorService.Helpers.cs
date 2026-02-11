namespace ENTOS.Module.Services
{
    public partial class WebExtractorService
    {
        #region UrlPasteWebExtractor Helpers

        private static bool UrlPasteWebExtractor_IsSearchChoice(string choice)
        {
            return choice.Contains("Search");
        }

        private static bool UrlPasteWebExtractor_IsUrlOrImageChoice(string choice)
        {
            return choice.Equals("Url") || choice.Equals("Image");
        }

        private static bool UrlPasteWebExtractor_IsSearchPageChoice(string choice)
        {
            return choice.Equals("SearchPage");
        }

        private static bool UrlPasteWebExtractor_IsSearchImageChoice(string choice)
        {
            return choice.Equals("SearchImage");
        }

        private static bool UrlPasteWebExtractor_IsHttpLink(string text)
        {
            return text.StartsWith("http") || text.StartsWith("www");
        }

        private static bool UrlPasteWebExtractor_IsValidHtml(string htmlText)
        {
            if (string.IsNullOrEmpty(htmlText))
            {
                return false;
            }
            if (!htmlText.StartsWith("<") && !htmlText.StartsWith(">") && 
                !htmlText.Contains("<html>") && !htmlText.Contains("</html>"))
            {
                return false;
            }
            return true;
        }

        private static string UrlPasteWebExtractor_GetNodeName(string choice)
        {
            return choice.Equals("Url") ? "a" : "img";
        }

        private static string UrlPasteWebExtractor_GetNodeAttribute(string choice)
        {
            return choice.Equals("Url") ? "href" : "src";
        }

        private static string UrlPasteWebExtractor_PrependNewLine(string currentLink, string existingAddresses)
        {
            if (!string.IsNullOrEmpty(existingAddresses) && !existingAddresses.EndsWith("\r\n"))
            {
                return "\r\n" + currentLink;
            }
            return currentLink;
        }

        #endregion

        #region URL Helpers

        private static string UnescapeUrl(string url)
        {
            return System.Uri.UnescapeDataString(url);
        }

        private static bool IsValidHttpUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            return url.StartsWith("http://") || url.StartsWith("https://");
        }

        #endregion

        #region Keyword Helpers

        private static string[] SplitKeywords(string text)
        {
            return text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        }

        private static string BuildSearchKeyword(string siteName, string keyword)
        {
            return (siteName + " " + keyword).Trim();
        }

        #endregion

        #region JSON Helpers

        private static string ExtractFirstLinkFromSearchResult(string jsonResult)
        {
            if (string.IsNullOrEmpty(jsonResult))
            {
                return null;
            }
            using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(jsonResult))
            {
                var items = doc.RootElement.GetProperty("items");
                if (items.GetArrayLength() > 0)
                {
                    return items[0].GetProperty("link").GetString();
                }
            }
            return null;
        }

        #endregion
    }
}
