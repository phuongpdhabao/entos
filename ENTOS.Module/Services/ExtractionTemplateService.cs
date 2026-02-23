using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.Xpo;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ENTOS.Module;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;
using ENTOS.Module.Helpers;
using ENTOS.Module.Extensions;
using ENTOS.Module.SystemServices;
using ENTOS.Module.Services;


 
namespace ENTOS.Module.Services 
{

    public partial class ExtractionTemplateService : BaseService
    {

        public ExtractionTemplateService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public ExtractionTemplateService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3926ImportCode
                public static string ExtractionKeyJson(ExtractionTemplate template)
        {
            // Lấy tên các key theo layout
            var headerKeys = GetLayoutKeys(template, "Header", "Footer", "Body");
            var tableKeys = GetLayoutKeys(template, "Table");
            var table2Keys = GetLayoutKeys(template, "Table2");


            // Tạo object cho phần DataType (giá trị của template.SystemType.Name)
            var dataTypeObject = new Dictionary<string, object>();

            // Đưa headerKeys vào làm các property với giá trị rỗng
            foreach (var hk in headerKeys)
            {
                // nếu trùng key thì không ghi đè
                if (!dataTypeObject.ContainsKey(hk))
                    dataTypeObject[hk] = "";
            }

            // Tạo DataTypeMember: mảng có 1 object chứa các tableKeys
            var dataTypeMemberObj = BuildKeyDictionary(tableKeys);
            dataTypeObject["DataTypeMember"] = new List<Dictionary<string, string>> { dataTypeMemberObj };

            // Tạo nested DataType: mảng có 1 object chứa các table2Keys (ví dụ Interface,...)
            var nestedDataTypeObj = BuildKeyDictionary(table2Keys);
            dataTypeObject["DataType"] = new List<Dictionary<string, string>> { nestedDataTypeObj };

            // Tạo root: mảng chứa 1 object { SystemType.Name : dataTypeObject }
            var rootEntry = new Dictionary<string, object>
            {
                [template.SystemType.Name] = dataTypeObject
            };

            var rootArray = new List<Dictionary<string, object>> { rootEntry };

            // Serialize với options (indent + unicode full)
            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
            };

            return System.Text.Json.JsonSerializer.Serialize(rootArray, options);
        }




        #endregion SourceCode3926ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.ExtractionTemplate extractiontemplate)
        //{
        //    var result = "";
            //if (this.Name != null)
            //{
            //    if (!string.IsNullOrEmpty(result))
            //        result += "\r\n";
            //    result += "Tiêu đề:" + Name;
            //}            
        //    return result;
        //}
		
		//Tooltip for Object
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.ExtractionTemplate extractiontemplate)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CodeToolTipControllerText(View view, Module.BusinessObjects.ExtractionTemplate extractiontemplate)
        //{
        //    if (Code != null) 
		//			return Code;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberToolTipControllerText(View view, Module.BusinessObjects.ExtractionTemplate extractiontemplate)
        //{
        //    if (Member != null) 
		//			return Member;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SystemTypeToolTipControllerText(View view, Module.BusinessObjects.ExtractionTemplate extractiontemplate)
        //{
        //    if (SystemType != null) 
		//			return SystemType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TableSystemTypeToolTipControllerText(View view, Module.BusinessObjects.ExtractionTemplate extractiontemplate)
        //{
        //    if (TableSystemType != null) 
		//			return TableSystemType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Table2SystemTypeToolTipControllerText(View view, Module.BusinessObjects.ExtractionTemplate extractiontemplate)
        //{
        //    if (Table2SystemType != null) 
		//			return Table2SystemType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ExtractionKeyListToolTipControllerText(View view, Module.BusinessObjects.ExtractionTemplate extractiontemplate)
        //{
        //    if (ExtractionKeyList != null) 
		//			return ExtractionKeyList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ExtractionJsonToolTipControllerText(View view, Module.BusinessObjects.ExtractionTemplate extractiontemplate)
        //{
        //    if (ExtractionJson != null) 
		//			return ExtractionJson;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.ExtractionTemplate extractiontemplate)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdaterToolTipControllerText(View view, Module.BusinessObjects.ExtractionTemplate extractiontemplate)
        //{
        //    if (Updater != null) 
		//			return Updater;
        //    return null;
        //}
    

	    #endregion
  

    }
}
