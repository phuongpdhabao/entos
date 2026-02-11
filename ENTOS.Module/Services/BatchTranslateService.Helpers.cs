namespace ENTOS.Module.Services
{
    public partial class BatchTranslateService
    {
        #region BuildTranslateClipboardText Helpers

        private static string BuildTranslateClipboardText_BuildContentLine(string content, string symbol)
        {
            if (string.IsNullOrEmpty(content) || content.Trim().Length == 0)
            {
                return string.Empty;
            }
            if (!string.IsNullOrEmpty(symbol))
            {
                return content.Trim() + symbol + "\n";
            }
            return content.Trim() + "\n";
        }

        private static bool BuildTranslateClipboardText_HasContent(string content)
        {
            return content != null && content.Trim().Length > 0;
        }

        #endregion

        #region BuildReverseTranslatePrompt Helpers

        private static string BuildReverseTranslatePrompt_GetLanguageName(string language, string defaultName)
        {
            if (string.IsNullOrEmpty(language))
            {
                return defaultName;
            }
            return language;
        }

        private static string BuildReverseTranslatePrompt_TrimLines(string content)
        {
            var lines = content.Split('\n');
            var trimmedLines = new System.Collections.Generic.List<string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    trimmedLines.Add(line);
                }
                else
                {
                    trimmedLines.Add(line.TrimEnd());
                }
            }
            return string.Join("\n", trimmedLines);
        }

        #endregion

        #region CreateBatchTranslate Helpers

        private static bool CreateBatchTranslate_LanguageExists(System.Collections.Generic.IEnumerable<BatchTranslate> translateBatchList, Language language)
        {
            foreach (var translateBatch in translateBatchList)
            {
                if (language == translateBatch.Language)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region CalculateInferScores Helpers

        private static string CalculateInferScores_BuildString(string built, string word, bool reverse)
        {
            if (reverse)
            {
                return word + " " + built;
            }
            return built + word + " ";
        }

        private static string CalculateInferScores_RemoveFromSubtitle(string subtitleRemaining, string word, bool reverse)
        {
            if (reverse)
            {
                int removeLength = System.Math.Max(0, subtitleRemaining.Length - word.Length - 1);
                return subtitleRemaining.Remove(removeLength);
            }
            int removeCount = System.Math.Min(subtitleRemaining.Length, word.Length + 1);
            return subtitleRemaining.Remove(0, removeCount);
        }

        private static double CalculateInferScores_ComputeScore(double sim1, double sim2, double sim12, double sim22)
        {
            return (0.7 * sim1 + 0.7 * sim2 + 0.3 * sim12 + 0.3 * sim22) / 2;
        }

        private static bool EndsWithStrongPunctuation(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            char lastChar = text[text.Length - 1];
            return lastChar == '.' || lastChar == '!' || lastChar == '?';
        }

        private static bool EndsWithSoftPunctuation(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            char lastChar = text[text.Length - 1];
            return lastChar == ',' || lastChar == ';' || lastChar == ':';
        }

        #endregion

        #region FindPunctuationLine Helpers

        private static bool FindPunctuationLine_RequiresSpaceAfterPunctuation(string languageCode)
        {
            string[] languagesWithoutSpace = { "ja", "zh", "ko", "th", "my", "km", "lo" };
            foreach (var lang in languagesWithoutSpace)
            {
                if (lang == languageCode)
                {
                    return false;
                }
            }
            return true;
        }

        private static string FindPunctuationLine_GetPunctuationPattern(bool requireSpace)
        {
            if (requireSpace)
            {
                return @"[.!?]\s";
            }
            return @"[.!?]";
        }

        #endregion

        #region FindFirstDifferentLine Helpers

        private static bool FindFirstDifferentLine_IsDifferentTrend(int cPrev, int cCurr, int cNext, int tPrev, int tCurr, int tNext)
        {
            return System.Math.Sign(cCurr - cPrev) != System.Math.Sign(tCurr - tPrev) ||
                   System.Math.Sign(cNext - cCurr) != System.Math.Sign(tNext - tCurr);
        }

        #endregion

        #region FillBlankLines Helpers

        private static bool FillBlankLines_IsBlankLine(string line)
        {
            return string.IsNullOrWhiteSpace(line);
        }

        private static string FillBlankLines_GetSafeLineAtIndex(string[] lines, int index)
        {
            if (index >= 0 && index < lines.Length)
            {
                return lines[index];
            }
            return null;
        }

        #endregion
    }
}
