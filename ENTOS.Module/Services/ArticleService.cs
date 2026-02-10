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

    public partial class ArticleService : BaseService
    {

        public ArticleService() : base()
        {
        }
        #region DependencyInjection
        private IClipboardService clipboardService;
        protected IClipboardService _clipboardService => clipboardService ??= Application.ServiceProvider.GetRequiredService<IClipboardService>();        
  
  
        #endregion DependencyInjection

        public ArticleService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode4531ImportCode
        internal void KnowledgeShare(IEnumerable<Article> selectedObjects)
{
        
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    foreach (Article obj in selectedObjects)
    {
        var name = string.IsNullOrEmpty(obj.Name) ? "Không có tên" : obj.Name;
        var link = string.IsNullOrEmpty(obj.Link) ? "Không có link" : obj.Link;
        sb.AppendLine($"{name}\n{link}\n");
    }
    // copy vào clipboard
    if (sb.Length > 0)
    {
        _clipboardService.SetText(sb.ToString() + "\n");
    }
}
        #endregion SourceCode4531ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.Article article)
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
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.Article article)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LinkToolTipControllerText(View view, Module.BusinessObjects.Article article)
        //{
        //    if (Link != null) 
		//			return Link;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ContentToolTipControllerText(View view, Module.BusinessObjects.Article article)
        //{
        //    if (Content != null) 
		//			return Content;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.Article article)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdaterToolTipControllerText(View view, Module.BusinessObjects.Article article)
        //{
        //    if (Updater != null) 
		//			return Updater;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OrderToolTipControllerText(View view, Module.BusinessObjects.Article article)
        //{
        //    if (Order != null) 
		//			return Order;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CreatedDateToolTipControllerText(View view, Module.BusinessObjects.Article article)
        //{
        //    if (CreatedDate != null) 
		//			return CreatedDate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object KnowledgeToolTipControllerText(View view, Module.BusinessObjects.Article article)
        //{
        //    if (Knowledge != null) 
		//			return Knowledge;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberToolTipControllerText(View view, Module.BusinessObjects.Article article)
        //{
        //    if (Member != null) 
		//			return Member;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CategoryToolTipControllerText(View view, Module.BusinessObjects.Article article)
        //{
        //    if (Category != null) 
		//			return Category;
        //    return null;
        //}
    

	    #endregion
  

    }
}
