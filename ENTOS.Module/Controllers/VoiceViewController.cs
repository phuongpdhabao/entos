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
    public partial class VoiceViewController: BaseViewController<Module.BusinessObjects.Voice>
    {      
        
        public VoiceViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Voice);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        
		private void CalculatorSpeelingOfMinutes_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(CalculatorSpeelingOfMinutes), "Tính Âm phút");              
      
            #region CalculatorSpeelingOfMinutesImportCode

            #endregion CalculatorSpeelingOfMinutesImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        
		private void DemoVoice_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(DemoVoice), "Nghe thử");              
      
            #region DemoVoiceImportCode

            #endregion DemoVoiceImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}