using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevExpress.Charts.Model;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.XtraEditors;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Module.FilterControllers
{    
    public partial class FilterFilteringFolderInProductViewController : ViewController
    {
        private SimpleAction filteringCriterionAction;
        
        public FilterFilteringFolderInProductViewController()
        {
            filteringCriterionAction = new SimpleAction(
                    this, "FilteringFilteringFolderInProductCriterion", PredefinedCategory.Filters);
            filteringCriterionAction.Caption = "Lọc Thư mục";
            filteringCriterionAction.ImageName = "Folder";
            filteringCriterionAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;            
            filteringCriterionAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FilteringCriterionAction_Execute);           
            //filteringCriterionAction.CustomizeTemplate += new CustomizeTemplateEventHandler(this.FilteringCriterionAction_CustomizeTemplate);
            filteringCriterionAction.
            TargetObjectType = typeof(IFilteringFolderInProduct);
            TargetViewNesting = Nesting.Root;
            TargetViewType = ViewType.ListView;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        private void FilteringCriterionAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            using (DevExpress.ExpressApp.SystemModule.DialogController dc =
            Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
            {
                dc.Accepting += new EventHandler<DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs>(DialogController_Accepting);
                var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("FolderType = 'Product' and UpperFolder.FolderType <> 'Product'");
                Module.Helpers.XafXpoHelper.PopupDialogControllerListView(this, dc, typeof(Module.BusinessObjects.Folder), Application.CreateObjectSpace(), "FolderType", criteria, false, null, true, true);
            }
        }

             

        private void DialogController_Accepting(object sender, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs e)
        {
            if (View is null)
                return;
            #region PopupNewFieldSetFieldImportCode
            if (e.AcceptActionArgs.CurrentObject is Module.BusinessObjects.Folder currentItem)
            {
                string data = "[Folder.Oid] = ?";
                foreach (CustomFilter customAttribute in View.ObjectTypeInfo.Type.GetCustomAttributes<CustomFilter>())
                {
                    if (customAttribute.Name.Equals("IFilteringFolderInProduct"))
                    {
                        data = customAttribute.Criteria;
                        break;
                    }
                }
                var criteriaParse = CriteriaOperator.Parse(data, currentItem.Oid, currentItem.Oid,
                       currentItem.Oid,
                       currentItem.Oid, currentItem.Oid, currentItem.Oid, currentItem.Oid,
                       currentItem.Oid,
                       currentItem.Oid,
                       currentItem.Oid);
                criteriaParse = AddAllChildCriteriaOperator(currentItem, criteriaParse, data);
                var filterKey = this.GetType().Name;
                ((DevExpress.ExpressApp.ListView)View).CollectionSource.BeginUpdateCriteria();
                if (((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.ContainsKey(filterKey))
                {
                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.Remove(filterKey);
                }
                   ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria[filterKey] =
                       CriteriaEditorHelper.GetCriteriaOperator(criteriaParse.LegacyToString(), View.ObjectTypeInfo.Type, ObjectSpace);
                ((DevExpress.ExpressApp.ListView)View).CollectionSource.EndUpdateCriteria();
            }
            #endregion PopupNewFieldSetFieldImportCode
        }

        private CriteriaOperator AddAllChildCriteriaOperator(Module.BusinessObjects.Folder currentItem, CriteriaOperator currentCriteriaOperator, string data)
        {
            if (currentItem.LowerFolder != null && currentItem.LowerFolder.Count > 0)
            {
                foreach (var childGroup in currentItem.LowerFolder)
                {
                    var childGroupParse = CriteriaOperator.Parse(data, childGroup.Oid, childGroup.Oid,
                        childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid,
                        childGroup.Oid, childGroup.Oid);
                    currentCriteriaOperator = CriteriaOperator.Or(currentCriteriaOperator, childGroupParse);
                    currentCriteriaOperator = AddAllChildCriteriaOperator(childGroup, currentCriteriaOperator, data);
                }
            }
            return currentCriteriaOperator;
        }       
    }
}