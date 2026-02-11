namespace ENTOS.Module.Services
{
    public partial class TermService
    {
        #region ImportTermsFromDictionaries Helpers

        private static string[] ImportTermsFromDictionaries_GetWordForms(string baseWord)
        {
            if (string.IsNullOrWhiteSpace(baseWord))
            {
                return System.Array.Empty<string>();
            }
            return new[] { baseWord, baseWord + "s", baseWord + "es" };
        }

        private static bool ImportTermsFromDictionaries_ShouldShowProgress(int total)
        {
            return total > 5;
        }

        private static string ImportTermsFromDictionaries_FormatProgress(decimal countNumber, int total)
        {
            return (countNumber / total).ToString("p0");
        }

        #endregion

        #region GetSingularForm Helpers

        private static bool GetSingularForm_EndsWithIes(string word)
        {
            return word.EndsWith("ies", System.StringComparison.OrdinalIgnoreCase) && word.Length > 3;
        }

        private static bool GetSingularForm_EndsWithEs(string word)
        {
            return word.EndsWith("es", System.StringComparison.OrdinalIgnoreCase) && word.Length > 2;
        }

        private static bool GetSingularForm_EndsWithS(string word)
        {
            return word.EndsWith("s", System.StringComparison.OrdinalIgnoreCase) && word.Length > 1;
        }

        private static string GetSingularForm_ConvertIesToY(string word)
        {
            return word.Substring(0, word.Length - 3) + "y";
        }

        private static string GetSingularForm_RemoveEs(string word)
        {
            return word.Substring(0, word.Length - 2);
        }

        private static string GetSingularForm_RemoveS(string word)
        {
            return word.Substring(0, word.Length - 1);
        }

        #endregion

        #region GetTextInTranslate Helpers

        private static int GetTextInTranslate_FindOptionIndex(string content, string option, int startFrom)
        {
            return content.IndexOf(option, startFrom, System.StringComparison.OrdinalIgnoreCase);
        }

        private static bool GetTextInTranslate_IsValidIndexRange(int startIndex, int endIndex)
        {
            return startIndex < endIndex && (startIndex >= 0 || endIndex > 0);
        }

        private static string GetTextInTranslate_ExtractBetweenIndices(string content, int startIndex, int endIndex)
        {
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            if (endIndex < 0)
            {
                endIndex = content.Length;
            }
            return content.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

        private static string GetTextInTranslate_TrimFirstWord(string text)
        {
            var spaceIndex = text.IndexOf(' ');
            if (spaceIndex > 0)
            {
                return text.Substring(spaceIndex + 1).Trim();
            }
            return string.Empty;
        }

        private static string GetTextInTranslate_TrimLastWord(string text)
        {
            var spaceIndex = text.LastIndexOf(' ');
            if (spaceIndex > 0)
            {
                return text.Substring(0, spaceIndex).Trim();
            }
            return string.Empty;
        }

        #endregion

        #region IsOverlap Helpers

        private static bool IsOverlap_CheckPositionOverlap(int currentLocation, int termLocationValue, int termNameLength, int maxText)
        {
            int termEndPosition = termLocationValue + termNameLength - 1;
            int currentEndPosition = currentLocation + maxText - 1;
            bool startOverlap = currentLocation >= termLocationValue && currentLocation <= termEndPosition;
            bool endOverlap = currentEndPosition >= termLocationValue && currentEndPosition <= termEndPosition;
            bool containsOverlap = currentLocation <= termLocationValue && currentEndPosition >= termEndPosition;
            return startOverlap || endOverlap || containsOverlap;
        }

        #endregion

        #region SortTermLocations Helpers

        private static int SortTermLocations_CompareByPosition(int? sentenceA, int? locationA, int? sentenceB, int? locationB)
        {
            int sentenceCompare = (sentenceA ?? 0).CompareTo(sentenceB ?? 0);
            if (sentenceCompare != 0)
            {
                return sentenceCompare;
            }
            return (locationA ?? 0).CompareTo(locationB ?? 0);
        }

        #endregion

        #region AddWordToTerm Helpers

        private static int AddWordToTerm_ComputeNewPosition(int sentencePosition, int rowPosition, int add)
        {
            return sentencePosition + rowPosition + add;
        }

        #endregion
    }
}
