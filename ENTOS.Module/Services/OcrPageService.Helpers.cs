using System.Text;
using DevExpress.ExpressApp.DC;
using ENTOS.Module.BusinessObjects;

namespace ENTOS.Module.Services
{
    public partial class OcrPageService
    {
        private static void SetMemberValue(IMemberInfo memberInfo, object target, OcrValue value)
        {
            string dataTypeName = value.ExtractionKey.DataType?.Name ?? "String";
            object castValue = OcrValueService.CastValue(value.Value, dataTypeName);
            memberInfo.SetValue(target, castValue);
        }

        private static void AppendPageMarkdown(StringBuilder builder, OcrPage page, bool addSeparator)
        {
            if (addSeparator)
            {
                builder.AppendLine();
                builder.AppendLine("---");
                builder.AppendLine();
            }

            builder.AppendLine(page.OcrMarkdown);
            builder.AppendLine();
        }
    }
}
