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
    public partial class EntryFolderViewController: BaseViewController<Module.BusinessObjects.EntryFolder>
    {      
        
        public EntryFolderViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.EntryFolder);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.EntryFolderService entryFolderService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             entryFolderService = new Module.Services.EntryFolderService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 3361            Oid: 7b272929-3772-4d1d-be38-4e227f982110
		private void AccountBalance_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(AccountBalance), "Số dư tài khoản");              
      
            #region AccountBalanceImportCode
            //Chức năng Số dư tài khoản
            foreach (Module.BusinessObjects.EntryFolder folder in View.SelectedObjects)
            {
                Module.Services.EntryFolderService.CalculateTotalPropertyValue(folder, e.SelectedChoiceActionItem.Id);
            }
            #endregion AccountBalanceImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}