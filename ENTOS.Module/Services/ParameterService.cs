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

    public partial class ParameterService : BaseService
    {

        public ParameterService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public ParameterService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3561ImportCode
                public Parameter CopyToNewParameter(Parameter source)
        {
            return Module.Helpers.XafXpoHelper.CopyObject<Parameter>(source, source.Session);
            //        var newParameter = new Parameter(source.Session); // lấy session từ source
            //newParameter.Name = source.Name;
            //newParameter.Value = source.Value;
            //newParameter.SoftwareBusiness = source.SoftwareBusiness;
            //newParameter.Note = source.Note;
            //newParameter.Order = source.Order;
            //newParameter.ParameterFormat = source.ParameterFormat;
            //newParameter.User = false;
            //return newParameter;
        }
        #endregion SourceCode3561ImportCode

        #region SourceCode3559ImportCode
        public double GetDoubleValue(Parameter parameter)
{
    return ParseDoubleValue(parameter.Value);
}
        #endregion SourceCode3559ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.Parameter parameter)
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
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.Parameter parameter)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CategoryToolTipControllerText(View view, Module.BusinessObjects.Parameter parameter)
        //{
        //    if (Category != null) 
		//			return Category;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ParameterFormatToolTipControllerText(View view, Module.BusinessObjects.Parameter parameter)
        //{
        //    if (ParameterFormat != null) 
		//			return ParameterFormat;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NoteToolTipControllerText(View view, Module.BusinessObjects.Parameter parameter)
        //{
        //    if (Note != null) 
		//			return Note;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UserToolTipControllerText(View view, Module.BusinessObjects.Parameter parameter)
        //{
        //    if (User != null) 
		//			return User;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ValueToolTipControllerText(View view, Module.BusinessObjects.Parameter parameter)
        //{
        //    if (Value != null) 
		//			return Value;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.Parameter parameter)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdaterToolTipControllerText(View view, Module.BusinessObjects.Parameter parameter)
        //{
        //    if (Updater != null) 
		//			return Updater;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OrderToolTipControllerText(View view, Module.BusinessObjects.Parameter parameter)
        //{
        //    if (Order != null) 
		//			return Order;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PermissionPolicyUserToolTipControllerText(View view, Module.BusinessObjects.Parameter parameter)
        //{
        //    if (PermissionPolicyUser != null) 
		//			return PermissionPolicyUser;
        //    return null;
        //}
    

	    #endregion
  

    }
}
