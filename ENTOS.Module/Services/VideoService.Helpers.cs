namespace ENTOS.Module.Services
{
    public partial class VideoService
    {
        #region NodeContentIsValidate Helpers

        private static bool NodeContentIsValidate_HasLetterChar(string content)
        {
            foreach (var c in content)
            {
                if (char.IsLetter(c))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool NodeContentIsValidate_HasNumberChar(string content)
        {
            foreach (var c in content)
            {
                if (char.IsNumber(c))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool NodeContentIsValidate_IsTooShort(string content, bool numberOption)
        {
            if (!numberOption && content.Trim().Length < 2)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region InGroup Helpers

        private static bool InGroup_HasImageData(System.Xml.XmlNode groupNode)
        {
            foreach (System.Xml.XmlNode childNode in groupNode.ChildNodes)
            {
                if (childNode.Name == "v:shape")
                {
                    foreach (System.Xml.XmlNode imagedataNode in childNode.ChildNodes)
                    {
                        if (childNode.Name == "v:imagedata")
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static bool InGroup_HasPicNode(System.Xml.XmlNode groupNode)
        {
            foreach (System.Xml.XmlNode childNode in groupNode.ChildNodes)
            {
                if (childNode.Name == ":pic")
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region CheckTextIsXpath Helpers

        private static bool CheckTextIsXpath_IsXpathFormat(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            if (text.StartsWith("/html") || text.StartsWith("//*["))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region LogToNote Helpers

        private static string LogToNote_FormatEntry(System.DateTime startTime, string function, int select, int resultCount, double totalMinutes)
        {
            return string.Format("\r\n{0} : {1} : {2} : {3} : {4}",
                startTime.ToString("dd/MM/yyyy h:mm"),
                function,
                select,
                resultCount,
                System.Math.Round(totalMinutes, 0));
        }

        #endregion

        #region ImportAudiosFromPyanoteString Helpers

        private static bool ImportAudiosFromPyanoteString_ParseLine(string line, out string[] textArray)
        {
            textArray = line.Split(' ', 4);
            return textArray.Length == 4;
        }

        private static System.TimeSpan ImportAudiosFromPyanoteString_AdjustTime(System.TimeSpan time, int? bookMarkOrder)
        {
            if (bookMarkOrder.HasValue)
            {
                return time.Add(System.TimeSpan.FromDays(System.Convert.ToInt32(bookMarkOrder)));
            }
            return time;
        }

        #endregion

        #region GetAttributeInNode Helpers

        private static string GetAttributeInNode_ExtractValue(System.Xml.XmlNode node, string attributeName)
        {
            if (node == null || node.Attributes == null)
            {
                return null;
            }
            foreach (System.Xml.XmlAttribute attr in node.Attributes)
            {
                if (attr.Name == attributeName)
                {
                    return attr.Value;
                }
            }
            return null;
        }

        #endregion

        #region CheckAndUpdateLocationInTermIsValidate Helpers

        private static bool CheckAndUpdateLocationInTermIsValidate_IsLowerCase(string termName)
        {
            if (string.IsNullOrEmpty(termName))
            {
                return false;
            }
            return char.IsLower(termName[0]);
        }

        private static int CheckAndUpdateLocationInTermIsValidate_ComputeRelationByName(string[] termNameArray, string[] relationTermNameArray)
        {
            if (relationTermNameArray == null || relationTermNameArray.Length == 0)
            {
                return 4;
            }
            int intersectCount = 0;
            foreach (var term in termNameArray)
            {
                foreach (var relationTerm in relationTermNameArray)
                {
                    if (term.Equals(relationTerm, System.StringComparison.OrdinalIgnoreCase))
                    {
                        intersectCount++;
                        break;
                    }
                }
            }
            if (intersectCount == 0)
            {
                return 4;
            }
            else if (intersectCount < termNameArray.Length && intersectCount < relationTermNameArray.Length)
            {
                return 1;
            }
            else if (intersectCount == termNameArray.Length && intersectCount == relationTermNameArray.Length)
            {
                return 0;
            }
            else if (intersectCount == termNameArray.Length)
            {
                return 3;
            }
            else if (intersectCount == relationTermNameArray.Length)
            {
                return 2;
            }
            return 0;
        }

        #endregion

        #region ExportTranslateDocument Helpers

        private static string ExportTranslateDocument_FormatSubtitleLine(int index, System.TimeSpan start, System.TimeSpan end, string content)
        {
            string result = index.ToString("D");
            result += System.Environment.NewLine;
            result += string.Format("{0} --> {1}", start.ToString(@"hh\:mm\:ss\,fff"), end.ToString(@"hh\:mm\:ss\,fff"));
            result += System.Environment.NewLine;
            result += content;
            result += System.Environment.NewLine;
            result += System.Environment.NewLine;
            return result;
        }

        private static bool ExportTranslateDocument_IsDocFile(string extension)
        {
            string ext = extension.ToLower();
            return ext == ".doc" || ext == ".docx";
        }

        #endregion

        #region GetVideoTempFolder Helpers

        private static string GetVideoTempFolder_BuildPath(string baseDirectory, System.Guid videoOid, bool currentDirectory)
        {
            if (currentDirectory)
            {
                return System.IO.Path.Combine(baseDirectory, "Temp", videoOid.ToString());
            }
            return System.IO.Path.Combine(baseDirectory, videoOid.ToString());
        }

        #endregion
    }
}
