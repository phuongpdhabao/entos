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
    public partial class FilterFilteringOrgDivisionViewController : BaseFilteringViewController<Module.BusinessObjects.IOrgDivision>
    {
        
        public FilterFilteringOrgDivisionViewController()
        {
            filteringCriterionAction = new SingleChoiceAction(
                    this, "FilteringFilteringOrgDivisionCriterion", PredefinedCategory.Filters);
            filteringCriterionAction.Caption = "Lọc Bộ phận";
            filteringCriterionAction.ImageName = "OrgDivision";
            filteringCriterionAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FilteringCriterionAction_Execute);
            filteringCriterionAction.ItemType = SingleChoiceActionItemType.ItemIsMode;
            //this.Actions.Add(filteringCriterionAction);
            //TargetObjectType = typeof(Module.BusinessObjects.IOrgDivision);
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
            //var listOrgDivision = ObjectSpace.GetObjects<Module.BusinessObjects.OrgDivision>(criteria);
            var sortProperties = new DevExpress.Xpo.SortProperty[] { new DevExpress.Xpo.SortProperty("Code", DevExpress.Xpo.DB.SortingDirection.Ascending) };
            var listOrgDivision = new DevExpress.Xpo.XPCollection<Module.BusinessObjects.OrgDivision>(((DevExpress.ExpressApp.Xpo.XPObjectSpace)ObjectSpace).Session, criteria, sortProperties);
            if(listOrgDivision.Count == 0)
            {
                filteringCriterionAction.Items.Clear();
                return;
            }
            if (filteringCriterionAction.Items.FindItemByID("AllOrgDivision") == null)
                filteringCriterionAction.Items.Add(new ChoiceActionItem("AllOrgDivision", "Tất cả Bộ phận", null));
            
            var oidList = listOrgDivision.Select(f => f.Oid.ToString()).ToHashSet();
            RemoveOldChoice(oidList);    
            string data = "[OrgDivision.Oid] = ?";
            LoadDataFilter(ref data);
            foreach (var orgdivision in listOrgDivision.OrderBy(m => m.Code))
            {
                if (filteringCriterionAction.Items.FindItemByID(orgdivision.Oid.ToString()) == null)
                {
                    var criteriaParse = CriteriaOperator.Parse(data, orgdivision.Oid, orgdivision.Oid, orgdivision.Oid,
                        orgdivision.Oid, orgdivision.Oid, orgdivision.Oid, orgdivision.Oid, orgdivision.Oid, orgdivision.Oid,
                        orgdivision.Oid);
                    var choiceAction = new ChoiceActionItem(orgdivision.Oid.ToString(), orgdivision.Code, criteriaParse.LegacyToString());
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