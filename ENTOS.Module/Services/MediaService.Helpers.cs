namespace ENTOS.Module.Services
{
    public partial class MediaService
    {
        #region CalculateTextWord Helpers

        private static int CalculateTextWord_CountWords(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }
            return text.Split(new[] { ' ', '\t', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries).Length;
        }

        private static bool CalculateTextWord_HasText(string text)
        {
            return !string.IsNullOrEmpty(text);
        }

        #endregion

        #region CalculateSameGroup Helpers

        private static bool CalculateSameGroup_HasUpperMedia(Media media)
        {
            return media.UpperMedia != null;
        }

        #endregion

        #region CalculateChildElement Helpers

        private static bool CalculateChildElement_IsGroupType(MediaType mediaType)
        {
            return mediaType == MediaType.Group;
        }

        #endregion

        #region CalculateChildTextbox Helpers

        private static int CalculateChildTextbox_CountTextBoxes(System.Collections.Generic.List<Media> mediaList)
        {
            int count = 0;
            foreach (var media in mediaList)
            {
                if (media.MediaType == MediaType.TextBox)
                {
                    count++;
                }
            }
            return count;
        }

        private static bool CalculateChildTextbox_IsTextBox(MediaType mediaType)
        {
            return mediaType == MediaType.TextBox;
        }

        #endregion

        #region ProcessQuantity Helpers

        private static bool ProcessQuantity_IsTextWordAction(string actionId)
        {
            return actionId == "TextWord";
        }

        private static bool ProcessQuantity_IsSameGroupAction(string actionId)
        {
            return actionId == "SameGroup";
        }

        private static bool ProcessQuantity_IsChildElementAction(string actionId)
        {
            return actionId == "ChildElement";
        }

        private static bool ProcessQuantity_IsChildTextboxAction(string actionId)
        {
            return actionId == "ChildTextbox";
        }

        #endregion
    }
}
