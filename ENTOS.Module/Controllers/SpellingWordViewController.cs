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
    public partial class SpellingWordViewController: BaseViewController<Module.BusinessObjects.SpellingWord>
    {      
        
        public SpellingWordViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.SpellingWord);    
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


        
        //Code: 3326            Oid: 4d7eeb7a-d11c-4235-b165-136b1ba2ae9a
		private void DisplaySpelling_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(DisplaySpelling), "Hiển thị phiên âm");              
      
            #region DisplaySpellingImportCode
        foreach (SpellingWord spellingWord in View.SelectedObjects)
        {
            spellingWord.SetDefaultSpelling();
        }

        // Lưu và refresh view
        ObjectSpace.CommitChanges();
        View.Refresh();
            #endregion DisplaySpellingImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}