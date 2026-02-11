namespace ENTOS.Module.Services
{
    public partial class AudioService
    {
        #region ElementFlagUpperCase Helpers

        private static bool ElementFlagUpperCase_IsAllUpperCase(string audioContent)
        {
            return audioContent.Equals(audioContent.ToUpper());
        }

        private static bool ElementFlagUpperCase_IsSingleWord(int rowsLength, int childContentsLength, int wordsLength)
        {
            return rowsLength == 1 && childContentsLength == 1 && wordsLength == 1;
        }

        private static bool ElementFlagUpperCase_IsAbbreviation(string word)
        {
            return word.Length > 1 && word.ToUpper().Equals(word);
        }

        private static bool ElementFlagUpperCase_HasLowerCaseChar(string word)
        {
            foreach (var w in word)
            {
                if (char.IsLower(w))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool ElementFlagUpperCase_IsAcceptedLowerWord(string word, int wordIndex, string[] upperCaseAcceptWordsArray)
        {
            if (wordIndex > 0 && upperCaseAcceptWordsArray != null)
            {
                foreach (var acceptWord in upperCaseAcceptWordsArray)
                {
                    if (acceptWord.Equals(word, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool ElementFlagUpperCase_ComputeResult(bool upperCaseMany, int upperCount, int lowerCount, bool upper, bool lower)
        {
            if (upperCaseMany)
            {
                return upperCount > lowerCount;
            }
            else if (upper && !lower)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region ShiftSubtitle Helpers

        private static bool ShiftSubtitle_ValidateShiftBounds(bool up, int startIdx, int maxIdx, int lineCount)
        {
            if (up && startIdx - lineCount < 0)
            {
                return false;
            }
            if (!up && startIdx + lineCount > maxIdx)
            {
                return false;
            }
            return true;
        }

        private static bool ShiftSubtitle_HasExistingData(string subtitle, string spelling)
        {
            return !string.IsNullOrEmpty(subtitle) || !string.IsNullOrEmpty(spelling);
        }

        #endregion

        #region SemanticMatchLine Helpers

        private static void SemanticMatchLine_InitializeResult(int count, System.Collections.Generic.List<(System.Collections.Generic.List<int?>, bool)> matchResult)
        {
            for (int i = 0; i < count; i++)
            {
                matchResult.Add((new System.Collections.Generic.List<int?>(), false));
            }
        }

        #endregion

        #region RearrangeElement Helpers

        private static void RearrangeElement_ClearAudioData(System.Collections.Generic.List<Audio> list)
        {
            foreach (var audio in list)
            {
                audio.Order = null;
                audio.Subtitle = null;
                audio.Spelling = null;
                audio.Flag = false;
            }
        }

        private static string RearrangeElement_JoinStrings(System.Collections.Generic.List<string> strings)
        {
            if (strings.Count == 0)
            {
                return null;
            }
            return string.Join(" ", strings);
        }

        #endregion

        #region EndContentIsBreakLine Helpers

        private static bool EndContentIsBreakLine_CheckContent(string content, string[] newLineTexts)
        {
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }
            content = content.TrimEnd();
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }
            if (content.EndsWith('.'))
            {
                return true;
            }
            foreach (var endText in newLineTexts)
            {
                if (content.EndsWith(endText))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region GetUrlFrontContent Helpers

        private static string GetUrlFrontContent_ParseResult(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            var resultArray = result.Split(',');
            if (resultArray.Length >= 2 &&
                resultArray[1] == "\"error\":0" &&
                resultArray[0].IndexOf(':') > 0 &&
                !resultArray[0].EndsWith(':'))
            {
                return resultArray[0].Substring(resultArray[0].IndexOf(':') + 1).Replace("\"", "");
            }
            return null;
        }

        #endregion

        #region BestMatchSemantic Helpers

        private static int BestMatchSemantic_ComputeSearchRange(int currentIndex, int m, int listCount)
        {
            int start = System.Math.Max(0, currentIndex - m);
            int end = System.Math.Min(listCount - 1, currentIndex);
            return end - start + 1;
        }

        #endregion
    }
}
