using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ENTOS.Module.Services
{
    public partial class BatchTranslateService
    {
        private static string BuildContentText(IList<string> contentLines, string symbol)
        {
            var sb = new StringBuilder();
            foreach (var line in contentLines)
            {
                sb.Append(line);
                if (symbol != null)
                    sb.Append(symbol);
                sb.Append("\n");
            }
            return sb.ToString();
        }

        private static string UpdatePromptLanguage(string prompt, string originLanguage, string targetLanguage)
        {
            if (string.IsNullOrWhiteSpace(originLanguage) || string.IsNullOrWhiteSpace(targetLanguage))
                return prompt;

            var regex = new Regex(@"tiếng\s+((?:[A-ZÀ-Ỵ][\p{L}\-]*\s*){1,3})", RegexOptions.Multiline);
            var matches = regex.Matches(prompt);
            if (matches.Count == 0)
                return prompt;

            var result = new StringBuilder();
            int lastIndex = 0;

            foreach (Match match in matches)
            {
                result.Append(prompt.Substring(lastIndex, match.Index - lastIndex));
                var foundLang = match.Value.Trim();
                if (foundLang.Equals($"tiếng {originLanguage}", StringComparison.OrdinalIgnoreCase))
                {
                    result.Append(match.Value);
                }
                else
                {
                    result.Append($"tiếng {targetLanguage} ");
                }
                lastIndex = match.Index + match.Length;
            }

            result.Append(prompt.Substring(lastIndex));
            return result.ToString();
        }

        private static string NormalizePromptLines(string content)
        {
            var lines = content.Split('\n');
            var sb = new StringBuilder();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (!string.IsNullOrWhiteSpace(line))
                    line = line.TrimEnd();

                sb.Append(line);
                if (i < lines.Length - 1)
                    sb.Append('\n');
            }

            return sb.ToString();
        }

        private static double ComputeBaseInferScore(double sim1, double sim2, double sim12, double sim22)
        {
            return (0.7 * sim1 + 0.7 * sim2 + 0.3 * sim12 + 0.3 * sim22) / 2;
        }

        private static double ApplyPunctuationScore(double baseScore, bool strongEnd, bool softEnd)
        {
            double score = baseScore;
            if (strongEnd)
            {
                score += 0.1;
            }
            if (softEnd)
            {
                score += 0.05;
            }

            return score;
        }

        private static bool IsPunctuationInMiddle(string line, Regex regex)
        {
            if (string.IsNullOrWhiteSpace(line) || line.Length < 3)
                return false;

            var matches = regex.Matches(line);
            foreach (Match match in matches)
            {
                int index = match.Index;

                // Dấu ngắt phải nằm không ở cuối dòng
                if (index > 0 && index < line.Length - 2)
                    return true;
            }

            return false;
        }
    }
}
