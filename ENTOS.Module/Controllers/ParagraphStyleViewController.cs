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
    public partial class ParagraphStyleViewController: BaseViewController<Module.BusinessObjects.ParagraphStyle>
    {      
        
        public ParagraphStyleViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.ParagraphStyle);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.ParagraphStyleService paragraphStyleService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             paragraphStyleService = new Module.Services.ParagraphStyleService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 0900            Oid: e54bd82a-918f-4ee6-b78f-b23e09831fe3
		private void AssignFont_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(AssignFont), "Gán phông");              
      
            #region AssignFontImportCode
            paragraphStyleService.AssignFont(GetSelectedObjects(), GetMasterObject<Video>());

            #endregion AssignFontImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 0910            Oid: 8f6c6e44-baa7-431d-83fe-53299d0d1e20
		private void AdjustName_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(AdjustName), "Chỉnh tên");              
      
            #region AdjustNameImportCode
            paragraphStyleService.AdjustName(GetDisplayObjects());

            #endregion AdjustNameImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}