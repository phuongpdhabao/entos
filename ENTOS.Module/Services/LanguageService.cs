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

    public partial class LanguageService : BaseService
    {

        public LanguageService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public LanguageService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode4520ImportCode
         internal DataService GetDataService(ViewController viewController)
 {
     DataService _dataService = null;
     if (_dataService is null)
     {
         using (DevExpress.ExpressApp.SystemModule.DialogController dc =
                     viewController.Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
         {
             dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
             {
                 _dataService = (DataService)args?.AcceptActionArgs?.CurrentObject;
             };
             var criteria = GetTranslateDataServiceCriteria();
             Module.Helpers.XafXpoHelper.PopupDialogControllerListView(viewController, dc, typeof(DataService), viewController.View.ObjectSpace, "BookmarkImport", criteria, false, null, false, true);
         }
     }
     return _dataService;
}
        #endregion SourceCode4520ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.Language language)
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
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.Language language)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object EnglishNameToolTipControllerText(View view, Module.BusinessObjects.Language language)
        //{
        //    if (EnglishName != null) 
		//			return EnglishName;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OriginNameToolTipControllerText(View view, Module.BusinessObjects.Language language)
        //{
        //    if (OriginName != null) 
		//			return OriginName;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CodeToolTipControllerText(View view, Module.BusinessObjects.Language language)
        //{
        //    if (Code != null) 
		//			return Code;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LocaleCodeToolTipControllerText(View view, Module.BusinessObjects.Language language)
        //{
        //    if (LocaleCode != null) 
		//			return LocaleCode;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SpeakerToolTipControllerText(View view, Module.BusinessObjects.Language language)
        //{
        //    if (Speaker != null) 
		//			return Speaker;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CountryToolTipControllerText(View view, Module.BusinessObjects.Language language)
        //{
        //    if (Country != null) 
		//			return Country;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CharacterToolTipControllerText(View view, Module.BusinessObjects.Language language)
        //{
        //    if (Character != null) 
		//			return Character;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VowelToolTipControllerText(View view, Module.BusinessObjects.Language language)
        //{
        //    if (Vowel != null) 
		//			return Vowel;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object RepeatCharacterToolTipControllerText(View view, Module.BusinessObjects.Language language)
        //{
        //    if (RepeatCharacter != null) 
		//			return RepeatCharacter;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NotUpCaseToolTipControllerText(View view, Module.BusinessObjects.Language language)
        //{
        //    if (NotUpCase != null) 
		//			return NotUpCase;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VideoListToolTipControllerText(View view, Module.BusinessObjects.Language language)
        //{
        //    if (VideoList != null) 
		//			return VideoList;
        //    return null;
        //}
    

	    #endregion
  

    }
}
