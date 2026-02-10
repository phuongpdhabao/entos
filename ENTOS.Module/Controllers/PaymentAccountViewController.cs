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
    public partial class PaymentAccountViewController: BaseViewController<Module.BusinessObjects.PaymentAccount>
    {      
        
        public PaymentAccountViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.PaymentAccount);    
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


        
        
		private void TransactionSynchronization_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(TransactionSynchronization), "Đồng bộ");              
      
            #region TransactionSynchronizationImportCode

            #endregion TransactionSynchronizationImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}