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
    public partial class OrderDetailViewController: BaseViewController<Module.BusinessObjects.OrderDetail>
    {      
        
        public OrderDetailViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.OrderDetail);    
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


        
        
		private void ActionSplitQuantity_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ActionSplitQuantity), "Tách số lượng");              
      
            #region ActionSplitQuantityImportCode

            #endregion ActionSplitQuantityImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        
		private void FillIpcLineItem_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(FillIpcLineItem), "Nạp hàng hóa");              
      
            #region FillIpcLineItemImportCode

            #endregion FillIpcLineItemImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}