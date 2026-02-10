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
using System.Text.RegularExpressions;

 
namespace ENTOS.Module.Services 
{

    public partial class SourceCodeService : BaseService
    {

        public SourceCodeService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public SourceCodeService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3330ImportCode
        public static string ContentClean(string content)
{
            if (string.IsNullOrEmpty(content))
                return content;

            var withoutComments = Regex.Replace(content, @"^\s*//.*(?:\r?\n|$)", string.Empty, RegexOptions.Multiline);
            var cleanedContent = Regex.Replace(withoutComments, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).TrimEnd();

            return cleanedContent;
}
        #endregion SourceCode3330ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
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
		//public object CodeToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (Code != null) 
		//			return Code;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (Member != null) 
		//			return Member;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ProgrammingLanguageToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (ProgrammingLanguage != null) 
		//			return ProgrammingLanguage;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ContentToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (Content != null) 
		//			return Content;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ObjectRelationListToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (ObjectRelationList != null) 
		//			return ObjectRelationList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object BookMarkListToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (BookMarkList != null) 
		//			return BookMarkList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdaterToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (Updater != null) 
		//			return Updater;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SystemTypeToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (SystemType != null) 
		//			return SystemType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SoftwareObjectTypeToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (SoftwareObjectType != null) 
		//			return SoftwareObjectType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ObjectIDToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (ObjectID != null) 
		//			return ObjectID;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NoteToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (Note != null) 
		//			return Note;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FlagToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (Flag != null) 
		//			return Flag;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LineQuantityToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (LineQuantity != null) 
		//			return LineQuantity;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RelationQuantityToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (RelationQuantity != null) 
		//			return RelationQuantity;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DesignDataTypeToolTipControllerText(View view, Module.BusinessObjects.SourceCode sourcecode)
        //{
        //    if (DesignDataType != null) 
		//			return DesignDataType;
        //    return null;
        //}
    

	    #endregion
  

    }
}
