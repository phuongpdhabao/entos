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

    public partial class OcrPageService : BaseService
    {

        public OcrPageService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public OcrPageService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3905ImportCode
                        public object CreateOcrPageObject(OcrPage ocrPage)
        {
            if (ocrPage?.ExtractionTemplate == null)
                return null;

            var docType = ocrPage.ExtractionTemplate;
            var systemType = docType.SystemType;
            var tableSystemType = docType.TableSystemType;

            if (systemType == null) return null;

            var mainObject = CreateObject(systemType);

            // --- Map field không thuộc Table ---
            var typeInfo = FindTypeInfo(systemType);
            foreach (var val in ocrPage.OcrValue
                                       .Where(v => v.ExtractionKey != null && v.ExtractionKey.DataLayout != DataLayout.Table))
            {
                var memberInfo = typeInfo.FindMember(val.ExtractionKey.Code);
                if (memberInfo != null && !memberInfo.IsReadOnly)
                {
                    SetMemberValue(memberInfo, mainObject, val);
                }
            }

            // --- Xử lý Table ---
            if (tableSystemType != null)
            {
                var mainTypeInfo = FindTypeInfo(systemType);
                var tableMember = mainTypeInfo.Members
                    .FirstOrDefault(m =>
                        m.MemberType == tableSystemType ||
                        m.ListElementType == tableSystemType);

                if (tableMember != null)
                {
                    var tableValues = ocrPage.OcrValue
                        .Where(v => v.ExtractionKey != null && v.ExtractionKey.DataLayout == DataLayout.Table)
                        .ToList();

                    var rowGroups = tableValues
                        .Where(v => v.Y.HasValue)
                        .GroupBy(v => (int)(v.Y.Value / 5))
                        .OrderBy(g => g.Key);

                    foreach (var row in rowGroups)
                    {
                        var tableRowObj = CreateObject(tableSystemType);
                        var tableTypeInfo = FindTypeInfo(tableSystemType);

                        foreach (var val in row)
                        {
                            var memberInfo = tableTypeInfo.FindMember(val.ExtractionKey.Code);
                            if (memberInfo != null && !memberInfo.IsReadOnly)
                            {
                                SetMemberValue(memberInfo, tableRowObj, val);
                            }
                        }

                        if (tableMember.IsList)
                        {
                            var list = (System.Collections.IList)tableMember.GetValue(mainObject);
                            if (list == null)
                            {
                                var listType = typeof(List<>).MakeGenericType(tableSystemType);
                                list = (System.Collections.IList)Activator.CreateInstance(listType);
                                tableMember.SetValue(mainObject, list);
                            }
                            list.Add(tableRowObj);
                        }
                        else
                        {
                            tableMember.SetValue(mainObject, tableRowObj);
                        }
                    }
                }
            }

            return mainObject;
        }



        #endregion SourceCode3905ImportCode

        #region SourceCode3942ImportCode
                        public void AddPage(OcrDocument doc, string imagePath)
        {
            var page = CreateObject<OcrPage>();
            page.PageLink = imagePath;
            page.Member = doc.Member;
            page.MemberFolder = doc.MemberFolder;
            page.ExtractionTemplate = doc.ExtractionTemplate;        
            doc.OcrPageList.Add(page);

		}

        #endregion SourceCode3942ImportCode

        #region SourceCode3853ImportCode
                        public string MarkdownMerging(List<OcrPage> pages)
        {
            var orderedPages = pages.OrderBy(p => p.Order);

            var sb = new StringBuilder();
            bool first = true;

            foreach (var page in orderedPages)
            {
                AppendPageMarkdown(sb, page, !first);
                first = false;
            }

            return sb.ToString();
        }



        #endregion SourceCode3853ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
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
		//public object PageLinkToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (PageLink != null) 
		//			return PageLink;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ExtractionTemplateToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (ExtractionTemplate != null) 
		//			return ExtractionTemplate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CodeToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (Code != null) 
		//			return Code;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (Member != null) 
		//			return Member;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberFolderToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (MemberFolder != null) 
		//			return MemberFolder;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MultiObjectToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (MultiObject != null) 
		//			return MultiObject;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OcrJsonToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (OcrJson != null) 
		//			return OcrJson;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MarkdownToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (Markdown != null) 
		//			return Markdown;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OcrValueToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (OcrValue != null) 
		//			return OcrValue;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdaterToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (Updater != null) 
		//			return Updater;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CreatedDateToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (CreatedDate != null) 
		//			return CreatedDate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OcrDocumentToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (OcrDocument != null) 
		//			return OcrDocument;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OrderToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (Order != null) 
		//			return Order;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SystemTypeToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (SystemType != null) 
		//			return SystemType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ObjectIDToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (ObjectID != null) 
		//			return ObjectID;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OcrMarkdownToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (OcrMarkdown != null) 
		//			return OcrMarkdown;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ValueMarkdownToolTipControllerText(View view, Module.BusinessObjects.OcrPage ocrpage)
        //{
        //    if (ValueMarkdown != null) 
		//			return ValueMarkdown;
        //    return null;
        //}
    

	    #endregion
  

    }
}
