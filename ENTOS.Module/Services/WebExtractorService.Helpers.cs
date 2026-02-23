using System.Text;

namespace ENTOS.Module.Services
{
    public partial class WebExtractorService
    {
        private static bool IsHtmlContent(string htmlText)
        {
            return htmlText.StartsWith("<") || htmlText.StartsWith(">") || htmlText.Contains("<html>") || htmlText.Contains("</html>");
        }

        private static string PrependNewLineIfNeeded(string existingAddresses, string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (!string.IsNullOrEmpty(existingAddresses) && !existingAddresses.EndsWith("\r\n"))
            {
                return "\r\n" + value;
            }

            return value;
        }

        private static string NormalizeDirectLink(string htmlText, string existingAddresses)
        {
            var currentLink = System.Uri.UnescapeDataString(htmlText);
            return PrependNewLineIfNeeded(existingAddresses, currentLink);
        }

        private static string ExtractLinkLines(string htmlText, string nodeName, string nodeAttribute)
        {
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(htmlText);
            var allLink = htmlDocument.DocumentNode.Descendants(nodeName);
            var result = new StringBuilder();
            var links = new System.Collections.Generic.HashSet<string>();

            foreach (var linkNode in allLink)
            {
                string href = linkNode.GetAttributeValue(nodeAttribute, "default");
                if (string.IsNullOrEmpty(href))
                    continue;
                href = System.Uri.UnescapeDataString(href);
                if (string.IsNullOrEmpty(href) || links.Contains(href))
                    continue;

                if (result.Length > 0)
                    result.Append(System.Environment.NewLine);
                result.Append(href);
                links.Add(href);
            }

            return result.ToString();
        }
    }
}
