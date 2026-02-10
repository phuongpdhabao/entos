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
    public partial class ProgressViewController: BaseViewController<Module.BusinessObjects.Progress>
    {      
        
        public ProgressViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Progress);    
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


        
        //Code: 0256            Oid: 47ed610c-44ca-4436-8316-10c6f2f973fd
		private void OpenReferenceProgress_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(OpenReferenceProgress), "Mở đối tượng");              
      
            #region OpenReferenceProgressImportCode
if (View.CurrentObject is Module.BusinessObjects.Progress &&
    ((Module.BusinessObjects.Progress) View.CurrentObject).SystemType != null)
{
    using (DevExpress.ExpressApp.SystemModule.DialogController dc =
        Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
    {
        IObjectSpace os = Application.CreateObjectSpace();
        var progress = (Module.BusinessObjects.Progress) View.CurrentObject;
        var referenceObject = os.GetObjectByKey(progress.SystemType, progress.ObjectID);
        if (referenceObject != null)
        {
            ShowViewParameters showViewParameters = new ShowViewParameters()
            {
                TargetWindow = TargetWindow.NewWindow,
                CreateAllControllers = true,
                Context = TemplateContext.View,
            };
            showViewParameters.CreatedView = Application.CreateDetailView(os, referenceObject, true);
            Application.ShowViewStrategy.ShowView(showViewParameters,
                new ShowViewSource(Frame, dc.AcceptAction));
        }
        else
        {
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Đối tượng không tồn tại",
                InformationType.Error);
        }
    }
}
            #endregion OpenReferenceProgressImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}