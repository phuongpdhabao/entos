namespace ENTOS.Module.Services
{
    public partial class ParagraphStyleService
    {
        #region AdjustName Helpers

        private static string AdjustName_BuildStyleName(int index)
        {
            string styleName = "S";
            if (index < 9)
            {
                styleName += "0";
            }
            styleName += (index + 1).ToString();
            return styleName;
        }

        #endregion

        #region AssignFont Helpers

        private static bool AssignFont_StylesMatch(ParagraphStyle style1, ParagraphStyle style2)
        {
            return style1.Size == style2.Size &&
                   style1.Color == style2.Color &&
                   style1.Bold == style2.Bold &&
                   style1.Italic == style2.Italic &&
                   style1.Underline == style2.Underline;
        }

        private static string AssignFont_BuildReplacementMessage(string originalName, string replacementName)
        {
            return string.Format("Kiểu cách {0} được thay thế bằng {1}", originalName, replacementName);
        }

        private static string AssignFont_AppendMessage(string existingMessage, string newMessage)
        {
            if (!string.IsNullOrEmpty(existingMessage))
            {
                return existingMessage + System.Environment.NewLine + newMessage;
            }
            return newMessage;
        }

        private static bool AssignFont_HasValidFont(ParagraphStyle style)
        {
            return style.Font != "inherit" && style.Size != null;
        }

        private static bool AssignFont_IsSameStyle(System.Guid oid1, System.Guid oid2)
        {
            return oid1.Equals(oid2);
        }

        #endregion

        #region Style Comparison Helpers

        private static bool CompareStyleSize(decimal? size1, decimal? size2)
        {
            if (!size1.HasValue || !size2.HasValue)
            {
                return false;
            }
            return size1.Value == size2.Value;
        }

        private static bool CompareFontColor(string color1, string color2)
        {
            return color1 == color2;
        }

        private static bool CompareFontStyle(bool? style1, bool? style2)
        {
            return style1 == style2;
        }

        #endregion
    }
}
