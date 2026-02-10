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
    public partial class WebExtractorViewController: BaseViewController<Module.BusinessObjects.WebExtractor>
    {      
        
        public WebExtractorViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.WebExtractor);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.WebExtractorService webExtractorService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             webExtractorService = new Module.Services.WebExtractorService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 0520            Oid: 33b39ea7-8541-4722-8014-c3e22c07791f
		private void UrlPasteWebExtractor_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(UrlPasteWebExtractor), "Dán URL");              
      
            #region UrlPasteWebExtractorImportCode
webExtractorService.UrlPasteWebExtractor(e.SelectedChoiceActionItem.Id,GetCurrentObject());
            #endregion UrlPasteWebExtractorImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1581            Oid: 93529f57-8374-425f-8d85-b5dcdaae658b
		private void WebExtractorResult_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(WebExtractorResult), "Dữ liệu");              
      
            #region WebExtractorResultImportCode
webExtractorService.WebExtractorResult(e.SelectedChoiceActionItem.Id,GetCurrentObject());
            #endregion WebExtractorResultImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}