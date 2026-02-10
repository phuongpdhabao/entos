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
    public partial class FilterFilteringMemberViewController : BaseFilteringViewController<Module.BusinessObjects.IMember>
    {
        
        public FilterFilteringMemberViewController()
        {
            filteringCriterionAction = new SingleChoiceAction(
                    this, "FilteringFilteringMemberCriterion", PredefinedCategory.Filters);
            filteringCriterionAction.Caption = "Lọc Thành viên";
            filteringCriterionAction.ImageName = "Member";
            filteringCriterionAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FilteringCriterionAction_Execute);
            filteringCriterionAction.ItemType = SingleChoiceActionItemType.ItemIsMode;
            //this.Actions.Add(filteringCriterionAction);
            //TargetObjectType = typeof(Module.BusinessObjects.IMember);
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
                    CreateDefaultFilter(CriteriaOperator.Parse("IsActive"));
        } 
        private void CreateDefaultFilter(CriteriaOperator criteria)
        {
            //var listMember = ObjectSpace.GetObjects<Module.BusinessObjects.Member>(criteria);
            var sortProperties = new DevExpress.Xpo.SortProperty[] { new DevExpress.Xpo.SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending) };
            var listMember = new DevExpress.Xpo.XPCollection<Module.BusinessObjects.Member>(((DevExpress.ExpressApp.Xpo.XPObjectSpace)ObjectSpace).Session, criteria, sortProperties);
            if(listMember.Count == 0)
            {
                filteringCriterionAction.Items.Clear();
                return;
            }
            if (filteringCriterionAction.Items.FindItemByID("AllMember") == null)
                filteringCriterionAction.Items.Add(new ChoiceActionItem("AllMember", "Tất cả Thành viên", null));
            
            var oidList = listMember.Select(f => f.Oid.ToString()).ToHashSet();
            RemoveOldChoice(oidList);    
            string data = "[Member.Oid] = ?";
            LoadDataFilter(ref data);
            foreach (var member in listMember.OrderBy(m => m.Name))
            {
                if (filteringCriterionAction.Items.FindItemByID(member.Oid.ToString()) == null)
                {
                    var criteriaParse = CriteriaOperator.Parse(data, member.Oid, member.Oid, member.Oid,
                        member.Oid, member.Oid, member.Oid, member.Oid, member.Oid, member.Oid,
                        member.Oid);
                    var choiceAction = new ChoiceActionItem(member.Oid.ToString(), member.Name, criteriaParse.LegacyToString());
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