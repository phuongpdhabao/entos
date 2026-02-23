namespace ENTOS.Module.Services
{
    public partial class ParagraphStyleService
    {
        private static string BuildStyleName(int index)
        {
            string styleName = "S";
            if (index < 9)
                styleName += "0";
            styleName += index + 1;
            return styleName;
        }

        private static string AppendReplacementMessage(string currentMessage, string sourceName, string targetName)
        {
            if (!string.IsNullOrEmpty(currentMessage))
                currentMessage += System.Environment.NewLine;

            currentMessage += string.Format("Kiểu cách {0} được thay thế bằng {1}", sourceName, targetName);
            return currentMessage;
        }
    }
}
