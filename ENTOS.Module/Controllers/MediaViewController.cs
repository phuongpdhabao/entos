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
    public partial class MediaViewController: BaseViewController<Module.BusinessObjects.Media>
    {      
        
        public MediaViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Media);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.MediaService mediaService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             mediaService = new Module.Services.MediaService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 2611            Oid: 3e7627be-3c54-46e1-a213-4407078c6cfc
		private void QuantityMedia_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(QuantityMedia), "Số lượng");              
      
            #region QuantityMediaImportCode
            mediaService.QuantityMedia(
                View.SelectedObjects.Cast<Module.BusinessObjects.Media>().ToList(),
                e.SelectedChoiceActionItem.Id,
                parent =>
                {
                    var criteria = new DevExpress.Data.Filtering.BinaryOperator("UpperMedia.Oid", parent.Oid);
                    return View.ObjectSpace.GetObjects<Media>(criteria).ToList();
                });



            #endregion QuantityMediaImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}