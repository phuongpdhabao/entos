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

    public partial class ElementTranslateService : BaseService
    {

        public ElementTranslateService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public ElementTranslateService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3311ImportCode
                public static void TranslateElementToMutiLanguage(
    List<Language> languageList,
    List<Audio> audioList,
    Video video,
    DataService dataService,
    int maxLength)
        {
            if (languageList == null || video == null || dataService == null || audioList == null)
                return;

            foreach (var language in languageList)
            {
                string block = "";
                var blockList = new List<Audio>();

                foreach (var audio in audioList)
                {
                    string content = audio.Content?.Trim() ?? "";
                    if (string.IsNullOrWhiteSpace(content))
                        continue;

                    string marked = content;

                    if ((block + "\n").Length + marked.Length >= maxLength)
                    {
                        Module.Services.ElementTranslateService.ProcessBlockTranslate(
                            dataService, blockList, block, video.LanguageOrigin.Code, language.Code);

                        block = marked;
                        blockList = new List<Audio> { audio };
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(block))
                            block += "\n";
                        block += marked;
                        blockList.Add(audio);
                    }
                }

                if (blockList.Count > 0)
                {
                    Module.Services.ElementTranslateService.ProcessBlockTranslate(
                        dataService, blockList, block, video.LanguageOrigin.Code, language.Code);
                }
            }
        }

        #endregion SourceCode3311ImportCode

        #region SourceCode3310ImportCode
                public static void ProcessBlockTranslate(
 DataService dataService,
 List<Audio> audioBlock,
 string blockContent,
 string langOrigin,
 string langTranslate)
        {
            string fullTranslated = Module.Services.ElementTranslateService
                .GetElementTranslateContent(dataService, blockContent, langOrigin, langTranslate);

            if (string.IsNullOrWhiteSpace(fullTranslated))
                return;

            var results = fullTranslated
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToArray();

            if (results.Length != audioBlock.Count)
                return; // fallback nếu không khớp

            for (int i = 0; i < audioBlock.Count; i++)
            {
                var audio = audioBlock[i];

                var translated = results[i];

                var elementTranslate = audio.ElementTranslateList?.FirstOrDefault(e => e.Language?.Code == langTranslate);
                if (elementTranslate != null)
                {
                    if (string.IsNullOrWhiteSpace(elementTranslate.Content))
                        elementTranslate.Content = translated;
                }
                else
                {
                    var newElementTranslate = new ElementTranslate(audio.Session);
                    newElementTranslate.Audio = audio;
                    newElementTranslate.Language = audio.Video?.LanguageList?.FirstOrDefault(e => e.Code == langTranslate);
                    newElementTranslate.Content = translated;
                }
            }
        }
        #endregion SourceCode3310ImportCode

        #region SourceCode3308ImportCode
                public static string GetElementTranslateContent(DataService dataService, string input, string languageOriginCode, string languageTranslateCode)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(languageOriginCode) || string.IsNullOrEmpty(languageTranslateCode))
            {
                return string.Empty;
            }
            var dataServiceService = new Module.Services.DataServiceService();

            if (dataService.Address.Contains("google"))
            {
                var translateContent = System.Threading.Tasks.Task
                .Run(() => dataServiceService.TranslateUsingGoogleAsync(dataService, input, languageOriginCode, languageTranslateCode))
                .Result;

                return translateContent;
            }
            return string.Empty;
        }
        #endregion SourceCode3308ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.ElementTranslate elementtranslate)
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
		//public object StartToolTipControllerText(View view, Module.BusinessObjects.ElementTranslate elementtranslate)
        //{
        //    if (Start != null) 
		//			return Start;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object EndToolTipControllerText(View view, Module.BusinessObjects.ElementTranslate elementtranslate)
        //{
        //    if (End != null) 
		//			return End;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VoiceToolTipControllerText(View view, Module.BusinessObjects.ElementTranslate elementtranslate)
        //{
        //    if (Voice != null) 
		//			return Voice;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ContentToolTipControllerText(View view, Module.BusinessObjects.ElementTranslate elementtranslate)
        //{
        //    if (Content != null) 
		//			return Content;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VoiceSpeedToolTipControllerText(View view, Module.BusinessObjects.ElementTranslate elementtranslate)
        //{
        //    if (VoiceSpeed != null) 
		//			return VoiceSpeed;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AudioLinkToolTipControllerText(View view, Module.BusinessObjects.ElementTranslate elementtranslate)
        //{
        //    if (AudioLink != null) 
		//			return AudioLink;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AudioDurationToolTipControllerText(View view, Module.BusinessObjects.ElementTranslate elementtranslate)
        //{
        //    if (AudioDuration != null) 
		//			return AudioDuration;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SpellingToolTipControllerText(View view, Module.BusinessObjects.ElementTranslate elementtranslate)
        //{
        //    if (Spelling != null) 
		//			return Spelling;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AudioToolTipControllerText(View view, Module.BusinessObjects.ElementTranslate elementtranslate)
        //{
        //    if (Audio != null) 
		//			return Audio;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AudioRateToolTipControllerText(View view, Module.BusinessObjects.ElementTranslate elementtranslate)
        //{
        //    if (AudioRate != null) 
		//			return AudioRate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LanguageToolTipControllerText(View view, Module.BusinessObjects.ElementTranslate elementtranslate)
        //{
        //    if (Language != null) 
		//			return Language;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.ElementTranslate elementtranslate)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

	    #endregion
  

    }
}
