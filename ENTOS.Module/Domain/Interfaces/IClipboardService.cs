using DevExpress.ExpressApp.DC;
using ENTOS.Module.BusinessObjects;

namespace ENTOS.Module.BusinessObjects
{
	public interface IClipboardService
    {

		        bool GetDataPresent(string format);
        object? GetData(string format);

        bool ContainsText();
        string? GetText();

        void SetData(string format, object data);
        void SetText(string text);

        string? GetXPathFromClipboard();

        void Clear();
    }

}