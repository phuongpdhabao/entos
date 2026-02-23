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

    public partial class WebExtractorService : BaseService
    {

        public WebExtractorService() : base()
        {
        }
        #region DependencyInjection
        private IClipboardService clipboardService;
        protected IClipboardService _clipboardService => clipboardService ??= Application.ServiceProvider.GetRequiredService<IClipboardService>();        
  
  
        #endregion DependencyInjection

        public WebExtractorService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode4534ImportCode
                internal void UrlPasteWebExtractor(string choice,WebExtractor currentWebExtractor)
{
            if (currentWebExtractor != null)
            {             
                if (_clipboardService.GetDataPresent("Html"))
                {
                    var htmlText = _clipboardService.GetData((choice.Equals("Url") || choice.Equals("Image")) ? "Html" : "UnicodeText") as string;
                    if (string.IsNullOrEmpty(htmlText) && (choice.Equals("Url") || choice.Equals("Image")))
                    {
                        htmlText = _clipboardService.GetData("UnicodeText") as string;
                        if (string.IsNullOrEmpty(htmlText))
                            return;

                    }
                    if (string.IsNullOrEmpty(htmlText))
                        return;
                    if (choice.Contains("Search"))
                    {
                        var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Contains(Address,'www.googleapis.com/customsearch')");
                        if (choice.Equals("SearchPage"))
                        {
                            criteria = DevExpress.Data.Filtering.CriteriaOperator.And(criteria,
                                DevExpress.Data.Filtering.CriteriaOperator.Parse("Oid = '0b358a1e-05bb-4efd-8064-899852b61511'"));
                        }
                        else if (choice.Equals("SearchImage"))
                        {
                            criteria = DevExpress.Data.Filtering.CriteriaOperator.And(criteria,
                                DevExpress.Data.Filtering.CriteriaOperator.Parse("Oid = 'e05d18e5-34f9-4b2c-877a-058a856d397c'"));
                        }
                        var dataService = View.ObjectSpace.FindObject<DataService>(criteria);
                        var dataServiceDto = _mapper.Map<Application.DTOs.DataServiceDto>(dataService);

                        if (dataServiceDto is null)
                        {
                            _notificationService.Notify("Thông báo", "Vui lòng cấu hình dịch vụ tìm kiếm \r\nwww.googleapis.com/customsearch", InformationType.Warning);
                            return;
                        }
                        string addresses = String.Empty;
                        var keyWords = htmlText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        //if(htmlText.StartsWith)
                        Module.Services.DataServiceService dataServiceService1 = new Module.Services.DataServiceService();
                        foreach (var keyWord in keyWords)
                        {
                            var keyWordWithSite = ((WebExtractor)currentWebExtractor).Name + " " + keyWord;
                            keyWordWithSite = keyWordWithSite.Trim();
                            var searchResult = Task.Run(() => dataServiceService1.GetResultAsync(dataServiceDto, new object[] { keyWordWithSite })).GetAwaiter().GetResult();

                            if (searchResult != null)
                            {
                                var resultText = searchResult.ToString();
                                if (!string.IsNullOrEmpty(resultText))
                                {
                                    using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(resultText))
                                    {
                                        var items = doc.RootElement.GetProperty("items");
                                        if (items.GetArrayLength() > 0)
                                        {
                                            string firstResultLink = items[0].GetProperty("link").GetString();
                                            addresses += firstResultLink + "\r\n";
                                        }

                                    }
                                }
                            }

                        }
                        if (!string.IsNullOrEmpty(addresses))
                        {
                            addresses = PrependNewLineIfNeeded(currentWebExtractor.Addresses, addresses);
                            currentWebExtractor.Addresses += addresses;
                        }
                        return;
                    }
                    if (htmlText.StartsWith("http") || htmlText.StartsWith("www"))
                    {
                        //Hỗ trợ paste link trực tiếp
                        var currentLink = NormalizeDirectLink(htmlText, currentWebExtractor.Addresses);
                        currentWebExtractor.Addresses += currentLink;
                        return;
                    }
                    //Không phải là cấu trúc html
                    if (!IsHtmlContent(htmlText))
                        return;
                    string nodeName = choice.Equals("Url") ? "a" : "img";
                    string nodeAttribute = choice.Equals("Url") ? "href" : "src";
                    string result = ExtractLinkLines(htmlText, nodeName, nodeAttribute);
                    if (!string.IsNullOrEmpty(result))
                    {
                        result = PrependNewLineIfNeeded(currentWebExtractor.Addresses, result);
                        currentWebExtractor.Addresses += result;
                    }
                }
            }

}


        #endregion SourceCode4534ImportCode

        #region SourceCode4535ImportCode
                internal void WebExtractorResult(string choice,WebExtractor currentWebExtractor)
{
            Module.Services.DataServiceService dataServiceService1 = new Module.Services.DataServiceService();
            if (View != null && currentWebExtractor != null)
            {               
                if (_clipboardService.GetDataPresent("Html"))
                {
                    var htmlText = _clipboardService.GetData(choice.Equals("SearchPage") ? "UnicodeText" : "Html") as string;
                    if (string.IsNullOrEmpty(htmlText))
                    {
                        htmlText = _clipboardService.GetData("UnicodeText") as string;
                        if (string.IsNullOrEmpty(htmlText))
                            return;

                    }
                    if (string.IsNullOrEmpty(htmlText))
                        return;
                    if (choice.Equals("SearchPage"))
                    {
                        var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Contains(Address,'www.googleapis.com/customsearch')");
                        var dataService = View.ObjectSpace.FindObject<DataService>(criteria);
                        var dataServiceDto = _mapper.Map<Application.DTOs.DataServiceDto>(dataService);

                        if (dataServiceDto is null)
                        {
                            _notificationService.Notify("Thông báo", "Vui lòng cấu hình dịch vụ tìm kiếm \r\nwww.googleapis.com/customsearch", InformationType.Warning);
                            return;
                        }
                        string addresses = String.Empty;
                        var keyWords = htmlText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        //if(htmlText.StartsWith)
                        foreach (var keyWord in keyWords)
                        {
                            var keyWordWithSite = ((WebExtractor)currentWebExtractor).URL + " " + keyWord;
                            keyWordWithSite = keyWordWithSite.Trim();
                            var searchResult = Task.Run(() => dataServiceService1.GetResultAsync(dataServiceDto, new object[] { keyWordWithSite })).GetAwaiter().GetResult();

                            if (searchResult != null)
                            {
                                var resultText = searchResult.ToString();
                                if (!string.IsNullOrEmpty(resultText))
                                {
                                    using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(resultText))
                                    {
                                        var items = doc.RootElement.GetProperty("items");
                                        if (items.GetArrayLength() > 0)
                                        {
                                            string firstResultLink = items[0].GetProperty("link").GetString();
                                            addresses += firstResultLink + "\r\n";
                                        }

                                    }
                                }
                            }

                        }
                        if (!string.IsNullOrEmpty(addresses))
                        {
                            addresses = PrependNewLineIfNeeded(currentWebExtractor.Addresses, addresses);
                            currentWebExtractor.Addresses += addresses;
                        }
                        return;
                    }
                    if (htmlText.StartsWith("http") || htmlText.StartsWith("www"))
                    {
                        //Hỗ trợ paste link trực tiếp
                        var currentLink = NormalizeDirectLink(htmlText, currentWebExtractor.Addresses);
                        currentWebExtractor.Addresses += currentLink;
                        return;
                    }
                    //Không phải là cấu trúc html
                    if (!IsHtmlContent(htmlText))
                        return;
                    string nodeName = choice.Equals("Url") ? "a" : "img";
                    string nodeAttribute = choice.Equals("Url") ? "href" : "src";
                    string result = ExtractLinkLines(htmlText, nodeName, nodeAttribute);
                    if (!string.IsNullOrEmpty(result))
                    {
                        result = PrependNewLineIfNeeded(currentWebExtractor.Addresses, result);
                        currentWebExtractor.Addresses += result;
                    }
                }
            }



}


        #endregion SourceCode4535ImportCode


        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.WebExtractor webextractor)
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
		//public object URLToolTipControllerText(View view, Module.BusinessObjects.WebExtractor webextractor)
        //{
        //    if (URL != null) 
		//			return URL;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.WebExtractor webextractor)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SystemTypeToolTipControllerText(View view, Module.BusinessObjects.WebExtractor webextractor)
        //{
        //    if (SystemType != null) 
		//			return SystemType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ConnectTimeOutToolTipControllerText(View view, Module.BusinessObjects.WebExtractor webextractor)
        //{
        //    if (ConnectTimeOut != null) 
		//			return ConnectTimeOut;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AutomaticToolTipControllerText(View view, Module.BusinessObjects.WebExtractor webextractor)
        //{
        //    if (Automatic != null) 
		//			return Automatic;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RepeatToolTipControllerText(View view, Module.BusinessObjects.WebExtractor webextractor)
        //{
        //    if (Repeat != null) 
		//			return Repeat;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ExtractorItemListToolTipControllerText(View view, Module.BusinessObjects.WebExtractor webextractor)
        //{
        //    if (ExtractorItemList != null) 
		//			return ExtractorItemList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ExtractorDataConfigurationListToolTipControllerText(View view, Module.BusinessObjects.WebExtractor webextractor)
        //{
        //    if (ExtractorDataConfigurationList != null) 
		//			return ExtractorDataConfigurationList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AddressesToolTipControllerText(View view, Module.BusinessObjects.WebExtractor webextractor)
        //{
        //    if (Addresses != null) 
		//			return Addresses;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.WebExtractor webextractor)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object StartToolTipControllerText(View view, Module.BusinessObjects.WebExtractor webextractor)
        //{
        //    if (Start != null) 
		//			return Start;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object EndToolTipControllerText(View view, Module.BusinessObjects.WebExtractor webextractor)
        //{
        //    if (End != null) 
		//			return End;
        //    return null;
        //}
    

	    #endregion
  

    }
}
