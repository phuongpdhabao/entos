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

    public partial class SoftwareServiceTypeService : BaseService
    {

        public SoftwareServiceTypeService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public SoftwareServiceTypeService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3749ImportCode
                public class KieResult
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public decimal? Confidence { get; set; }
            public decimal Height { get; set; }
            public decimal Width { get; set; }
            public decimal X { get; set; }
            public decimal Y { get; set; }
            public int? PageIndex { get; set; }  // nếu cần phân biệt kết quả từ nhiều trang

        }

        public async static Task<string> GetKIEResult(Application.DTOs.DataServiceDto dataServiceDto, string fileJson, string[] headerKeyList, string[] footerKeyList, string[] tableKeyList, string[] bodyKeyList)
        {
            string headerKeys = JoinKeys(headerKeyList);
            string footerKeys = JoinKeys(footerKeyList);
            string tableKeys = JoinKeys(tableKeyList);
            string bodyKeys = JoinKeys(bodyKeyList);
            object[] inputs = new object[] { fileJson, headerKeys, footerKeys, tableKeys, bodyKeys };
            var DataServiceService = new Module.Services.DataServiceService();
            using var client = Module.Helpers.HttpHelper.CreateHttpClient(1800, true);
            var result = await DataServiceService.GetResultAsync(client, dataServiceDto, inputs);
            if (result is HttpContent httpContent)
            {
                return await httpContent.ReadAsStringAsync();
            }
            throw new InvalidCastException("GetResultAsync không trả về HttpContent");
        }
        public static List<KieResult> KieService(string jsonString)
        {
            var kieResults = new List<KieResult>();
            using var doc = System.Text.Json.JsonDocument.Parse(jsonString);
            foreach (var element in doc.RootElement.EnumerateArray())
            {
                string name = element.GetProperty("Name").GetString();
                decimal? confidence = element.TryGetProperty("Confidence", out var c) ? c.GetDecimal() : null;
                decimal height = element.TryGetProperty("Height", out var h) ? h.GetDecimal() : 0;
                decimal width = element.TryGetProperty("Width", out var w) ? w.GetDecimal() : 0;
                decimal x = element.TryGetProperty("X", out var xx) ? xx.GetDecimal() : 0;
                decimal y = element.TryGetProperty("Y", out var yy) ? yy.GetDecimal() : 0;
                int? pageIndex = element.TryGetProperty("PageIndex", out var p) ? p.GetInt32() : null;
                if (pageIndex == 0)
                    pageIndex = null;

                var valueProp = element.GetProperty("Value");

                if (valueProp.ValueKind == System.Text.Json.JsonValueKind.String)
                {
                    // Trường phẳng
                    kieResults.Add(new KieResult
                    {
                        Name = name,
                        Value = valueProp.GetString(),
                        Confidence = confidence,
                        Height = height,
                        Width = width,
                        X = x,
                        Y = y,
                        PageIndex = pageIndex
                    });
                }
                else if (valueProp.ValueKind == System.Text.Json.JsonValueKind.Object && valueProp.TryGetProperty("data", out var rows))
                {
                    // Trường bảng → flatten từng cell
                    foreach (var row in rows.EnumerateArray())
                    {
                        foreach (var cell in row.EnumerateObject())
                        {
                            string cellValue = cell.Value.GetProperty("Value").ToString();
                            var bbox = cell.Value.GetProperty("BBox");

                            kieResults.Add(new KieResult
                            {
                                Name = $"{name}.{cell.Name}",   // ví dụ: OrderDetails.Name
                                Value = cellValue,
                                Confidence = cell.Value.GetProperty("Confidence").GetDecimal(),
                                Height = Math.Round(bbox.GetProperty("Height").GetDecimal(), 0),
                                Width = Math.Round(bbox.GetProperty("Width").GetDecimal(), 0),
                                X = Math.Round(bbox.GetProperty("X").GetDecimal(), 0),
                                Y = Math.Round(bbox.GetProperty("Y").GetDecimal(), 0),
                                PageIndex = pageIndex

                            });
                        }
                    }
                }
            }
            return kieResults;
        }


        #endregion SourceCode3749ImportCode

        #region SourceCode3750ImportCode
                public class OcrResult
        {
            public string Json { get; set; }
            public string Markdown { get; set; }
        }

        public static async Task<OcrResult> StructureOcrService(Application.DTOs.DataServiceDto dataServiceDto, byte[] fileData)
        {
            if (dataServiceDto == null)
                throw new ArgumentNullException(nameof(dataServiceDto));
            if (fileData == null || fileData.Length == 0)
                throw new ArgumentException("File data is empty", nameof(fileData));


            object[] inputs = new object[] { fileData };
            string[] outputs = new string[] { "json", "markdown" };
            var DataServiceService = new Module.Services.DataServiceService();   // tạo instance
            using var client = Module.Helpers.HttpHelper.CreateHttpClient(1800, true);

            var result = await DataServiceService.GetResultAsync(client, dataServiceDto, inputs);

            using (var httpContent = result as HttpContent)
            {
                if (httpContent == null)
                    throw new InvalidOperationException("GetResultAsync không trả về HttpContent.");

                string responseString = await httpContent.ReadAsStringAsync();

                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(responseString);
                    var root = doc.RootElement;

                    return new OcrResult
                    {
                        Json = root.TryGetProperty("json", out var jsonProp) ? jsonProp.ToString() : null,
                        Markdown = root.TryGetProperty("markdown", out var mdProp) ? mdProp.ToString() : null
                    };
                }
                catch
                {
                    // fallback: gán toàn bộ response vào Json
                    return new OcrResult
                    {
                        Json = responseString,
                        Markdown = null
                    };
                }
            }

        }


        #endregion SourceCode3750ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.SoftwareServiceType softwareservicetype)
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
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.SoftwareServiceType softwareservicetype)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CodeToolTipControllerText(View view, Module.BusinessObjects.SoftwareServiceType softwareservicetype)
        //{
        //    if (Code != null) 
		//			return Code;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DataServiceToolTipControllerText(View view, Module.BusinessObjects.SoftwareServiceType softwareservicetype)
        //{
        //    if (DataService != null) 
		//			return DataService;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberToolTipControllerText(View view, Module.BusinessObjects.SoftwareServiceType softwareservicetype)
        //{
        //    if (Member != null) 
		//			return Member;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DataServiceListToolTipControllerText(View view, Module.BusinessObjects.SoftwareServiceType softwareservicetype)
        //{
        //    if (DataServiceList != null) 
		//			return DataServiceList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ServiceInputListToolTipControllerText(View view, Module.BusinessObjects.SoftwareServiceType softwareservicetype)
        //{
        //    if (ServiceInputList != null) 
		//			return ServiceInputList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ServiceOutputListToolTipControllerText(View view, Module.BusinessObjects.SoftwareServiceType softwareservicetype)
        //{
        //    if (ServiceOutputList != null) 
		//			return ServiceOutputList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.SoftwareServiceType softwareservicetype)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdaterToolTipControllerText(View view, Module.BusinessObjects.SoftwareServiceType softwareservicetype)
        //{
        //    if (Updater != null) 
		//			return Updater;
        //    return null;
        //}
    

	    #endregion
  

    }
}
