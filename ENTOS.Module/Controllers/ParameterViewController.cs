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
    public partial class ParameterViewController: BaseViewController<Module.BusinessObjects.Parameter>
    {      
        
        public ParameterViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Parameter);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.ParameterService parameterService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             parameterService = new Module.Services.ParameterService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 1109            Oid: 77880270-cb0a-44a7-a084-58ef418ac529
		private void CloneParameter_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(CloneParameter), "Nạp tham số");              
      
            #region CloneParameterImportCode
            int count = 0;
            var parameterList = ObjectSpace.GetObjects<Module.BusinessObjects.Parameter>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Not IsNullOrEmpty(Name) and User and PermissionPolicyUser is null"));
            var userParameterList = ObjectSpace.GetObjects<Module.BusinessObjects.Parameter>(DevExpress.Data.Filtering.CriteriaOperator.Parse("PermissionPolicyUser.Oid = ?", SecuritySystem.CurrentUserId)).Select(m => m.Name).ToList();
            var currentUser = ObjectSpace.GetObjectByKey<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser>(SecuritySystem.CurrentUserId);
            foreach (var systemParameter in parameterList)
            {
                if (!userParameterList.Contains(systemParameter.Name))
                {
                    var userParameter = systemParameter.CopyToNewParameter();
                    userParameter.PermissionPolicyUser = currentUser;                    
                    //userParameter.Session.CommitTransaction();
                    if(View is ListView)
                    {
                        ((ListView)View).CollectionSource.Add(userParameter);
                    }
                    count++;
                }
            }            
            ObjectSpace.CommitChanges();
            //Module.SystemObjects.Tools.RefreshGridView(View);
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", count + " tham số được nạp");

            #endregion CloneParameterImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}