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
    public partial class FolderViewController: BaseViewController<Module.BusinessObjects.Folder>
    {      
        
        public FolderViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Folder);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.FolderService folderService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             folderService = new Module.Services.FolderService(this);
             
            #region MemberFolderOnViewControlsCreatedCode
		    var listview = View as ListView;
if (listview != null)
{
    if (MemberFolder.Items.Count > 0)
    {
        if (MemberFolder.SelectedItem != null)
        {
            MemberFolder.DoExecute(MemberFolder.SelectedItem);
        }
    }
    else
    if (MemberFolder.SelectedItem == null)
    {
        //Hỗ trợ lazy loadz
        //filteringCriterionAction.ShowItemsOnClick = true;

        folderService.CreateDefaultFilter(DevExpress.Data.Filtering.CriteriaOperator.Parse("[FolderType] = 'Accounting'"), MemberFolder, this);
        //if (filteringCriterionAction.Items.Count > 0)
        //{
        //    if (filteringCriterionAction.SelectedItem == null)
        //    {
        //        filteringCriterionAction.SelectedIndex = 0;
        //    }
        //    else if (filteringCriterionAction.SelectedIndex != 0)
        //    {
        //        filteringCriterionAction.DoExecute(filteringCriterionAction.SelectedItem);
        //    }
        //}
    }
}
		    #endregion MemberFolderOnViewControlsCreatedCode
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 1545            Oid: 664a5d5a-d3d4-4db4-8acd-193ce4bcb945
		private void MemberFolder_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(MemberFolder), "Chọn tập thể");              
      
            #region MemberFolderImportCode
            var filterKey = this.GetType().Name;
            ((DevExpress.ExpressApp.ListView)View).CollectionSource.BeginUpdateCriteria();
            if (((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.ContainsKey(filterKey))
            {
                ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.Remove(filterKey);
            }
            string data = e.SelectedChoiceActionItem.Data as string;
            if (!string.IsNullOrEmpty(data))
            {
                if (folderService.existedCriteria == null)
                    folderService.existedCriteria = new System.Collections.Generic.List<System.Guid>();
                else
                    folderService.existedCriteria.Clear();
                var currentId = System.Guid.Parse(e.SelectedChoiceActionItem.Id);
                var criteriaParse = DevExpress.Data.Filtering.CriteriaOperator.Parse(data, currentId, currentId, currentId);
                criteriaParse = folderService.AddAllChildCriteriaOperator(currentId, criteriaParse, data);
                data = criteriaParse.LegacyToString();
            }
            foreach (Module.BusinessObjects.Folder folder in View.SelectedObjects)
            {
                folderService.MemberFolderLoad(data, folder);
            }       



            #endregion MemberFolderImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1094            Oid: f9a74513-e8bf-487c-91ef-ad3e5943ab30
		private void ExportComputer_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ExportComputer), "Xuất máy tinh");              
      
            #region ExportComputerImportCode
            //105; Computer: Tạo các thư mục được chọn (kể cả con cháu) vào trong đường dẫn được chọn trên máy tính(không có quyền thì báo lỗi)
            int result = 0;
            int total = 0;
            foreach (Module.BusinessObjects.Folder folder in View.SelectedObjects)
            {
                if (e.SelectedChoiceActionItem.Id.Equals("Computer"))
                {
                    if (!folderService.ExportFolderComputer(folder, ref result, ref total))
                    {
                        return;
                    }
                }
                else
                {
                    //WordPress
                }

            }
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", result + "/" + total + " được tạo");




            #endregion ExportComputerImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}