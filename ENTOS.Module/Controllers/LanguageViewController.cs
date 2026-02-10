using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using Microsoft.Extensions.DependencyInjection;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;
using ENTOS.Module.Services;
using ListView = DevExpress.ExpressApp.ListView;


namespace ENTOS.Module.Controllers 
{
    public partial class LanguageViewController: BaseViewController<Module.BusinessObjects.Language>
    {      
        
        public LanguageViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Language);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.LanguageService languageService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             languageService = new Module.Services.LanguageService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 3312            Oid: 27d14998-ec12-4ba7-9fd8-b7a1c1907b69
		private void TranslateAllElement_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(TranslateAllElement), "Dịch toàn bộ");              
      
            #region TranslateAllElementImportCode
            Module.BusinessObjects.Video video = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            if (video is null)
            {
                return;
            }
            var audioList = video.AudioList.ToList();
            List<Module.BusinessObjects.Language> languageList = new List<Language>();

            foreach (Module.BusinessObjects.Language language in View.SelectedObjects)
            {
                languageList.Add(language);  
            }

            var param = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "SelectionQuantity", "3000", SecuritySystem.CurrentUserId);
            int maxLength = Convert.ToInt32(param?.Value ?? "3000");

            var dataService = languageService.GetDataService(this);

            Module.Services.ElementTranslateService.TranslateElementToMutiLanguage(languageList, audioList, video, dataService, maxLength);
        

            #endregion TranslateAllElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}