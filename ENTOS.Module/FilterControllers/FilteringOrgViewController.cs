using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.Helpers;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Module.FilterControllers
{    
    public partial class FilterFilteringOrgViewController : BaseFilteringViewController<Module.BusinessObjects.IOrg>
    {
        
        public FilterFilteringOrgViewController()
        {
            filteringCriterionAction = new SingleChoiceAction(
                    this, "FilteringFilteringOrgCriterion", PredefinedCategory.Filters);
            filteringCriterionAction.Caption = "Lọc Tổ chức";
            filteringCriterionAction.ImageName = "Org";
            filteringCriterionAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FilteringCriterionAction_Execute);
            filteringCriterionAction.ItemType = SingleChoiceActionItemType.ItemIsMode;
            //this.Actions.Add(filteringCriterionAction);
            //TargetObjectType = typeof(Module.BusinessObjects.IOrg);
            TargetViewNesting = Nesting.Root;
            //TargetViewType = ViewType.ListView;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            
            // Perform various tasks depending on the target View.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }     

        protected string FieldSet => "";
        protected override void CreateDefaultFilter()
        {
                    CreateDefaultFilter(null);
        } 
        private void CreateDefaultFilter(CriteriaOperator criteria)
        {
            //var listOrg = ObjectSpace.GetObjects<Module.BusinessObjects.Org>(criteria);
            var sortProperties = new DevExpress.Xpo.SortProperty[] { new DevExpress.Xpo.SortProperty("Code", DevExpress.Xpo.DB.SortingDirection.Ascending) };
            var listOrg = new DevExpress.Xpo.XPCollection<Module.BusinessObjects.Org>(((DevExpress.ExpressApp.Xpo.XPObjectSpace)ObjectSpace).Session, criteria, sortProperties);
            if(listOrg.Count == 0)
            {
                filteringCriterionAction.Items.Clear();
                return;
            }
            if (filteringCriterionAction.Items.FindItemByID("AllOrg") == null)
                filteringCriterionAction.Items.Add(new ChoiceActionItem("AllOrg", "Tất cả Tổ chức", null));
            
            var oidList = listOrg.Select(f => f.Oid.ToString()).ToHashSet();
            RemoveOldChoice(oidList);    
            string data = "[Org.Oid] = ?";
            LoadDataFilter(ref data);
            foreach (var org in listOrg)
            {
                if (filteringCriterionAction.Items.FindItemByID(org.Oid.ToString()) == null)
                {
                    var criteriaParse = CriteriaOperator.Parse(data, org.Oid, org.Oid, org.Oid,
                        org.Oid, org.Oid, org.Oid, org.Oid, org.Oid, org.Oid,
                        org.Oid);
                    var choiceAction = new ChoiceActionItem(org.Oid.ToString(), org.Oid.ToString(), criteriaParse.LegacyToString());
                    filteringCriterionAction.Items.Add(choiceAction);
                }

            }
            SavedChoiceDefaultFilter();
        }
        public bool IsTree = false;
        public CriteriaOperator AddAllChildCriteriaOperator(CriteriaOperator currentCriteriaOperator, string data)
        {
            if (filteringCriterionAction != null && filteringCriterionAction.SelectedItem != null)
            {
                return CriteriaOperator.And(currentCriteriaOperator, CriteriaOperator.Parse(data, Guid.Parse(filteringCriterionAction.SelectedItem.Id)));
            }
            return currentCriteriaOperator;
        }

 
    }
}