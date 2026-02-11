namespace ENTOS.Module.Services
{
    public partial class OcrPageService
    {
        #region CreateOcrPageObject Helpers

        private static bool CreateOcrPageObject_HasValidTemplate(OcrPage ocrPage)
        {
            return ocrPage?.ExtractionTemplate != null;
        }

        private static bool CreateOcrPageObject_HasSystemType(System.Type systemType)
        {
            return systemType != null;
        }

        private static bool CreateOcrPageObject_IsTableLayout(DataLayout dataLayout)
        {
            return dataLayout == DataLayout.Table;
        }

        private static bool CreateOcrPageObject_IsNotTableLayout(DataLayout dataLayout)
        {
            return dataLayout != DataLayout.Table;
        }

        private static string CreateOcrPageObject_GetDataTypeName(DataType dataType)
        {
            return dataType?.Name ?? "String";
        }

        private static int CreateOcrPageObject_CalculateRowGroup(decimal? yValue)
        {
            if (!yValue.HasValue)
            {
                return 0;
            }
            return (int)(yValue.Value / 5);
        }

        #endregion

        #region MarkdownMerging Helpers

        private static string MarkdownMerging_CombineMarkdown(System.Collections.Generic.List<string> markdowns)
        {
            if (markdowns == null || markdowns.Count == 0)
            {
                return string.Empty;
            }
            return string.Join("\n\n---\n\n", markdowns);
        }

        #endregion

        #region Member Info Helpers

        private static bool MemberInfo_CanSetValue(DevExpress.ExpressApp.DC.IMemberInfo memberInfo)
        {
            return memberInfo != null && !memberInfo.IsReadOnly;
        }

        private static bool MemberInfo_IsListMember(DevExpress.ExpressApp.DC.IMemberInfo memberInfo)
        {
            return memberInfo.IsList;
        }

        #endregion
    }
}
