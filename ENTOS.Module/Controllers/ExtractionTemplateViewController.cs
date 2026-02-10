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
    public partial class ExtractionTemplateViewController: BaseViewController<Module.BusinessObjects.ExtractionTemplate>
    {      
        
        public ExtractionTemplateViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.ExtractionTemplate);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.ExtractionTemplateService extractionTemplateService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             extractionTemplateService = new Module.Services.ExtractionTemplateService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 3929            Oid: 09afc071-1b1a-4160-82c9-3c25d0a98c7b
		private void ExtractionJson_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ExtractionJson), "Tạo Json");              
      
            #region ExtractionJsonImportCode
            var extractionTemplate = View.CurrentObject as Module.BusinessObjects.ExtractionTemplate;
            if (extractionTemplate is null)
                return;
            extractionTemplate.ExtractionJson = Module.Services.ExtractionTemplateService.ExtractionKeyJson(extractionTemplate);

            #endregion ExtractionJsonImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}