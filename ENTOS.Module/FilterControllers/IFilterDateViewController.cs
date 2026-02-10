using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Module.FilterControllers
{    
    public partial class FilterDateViewController : ViewController
    {
        private SingleChoiceAction filteringCriterionAction;
        private IModelChoiceActionItem _savedChoiceActionItem;
        private IModelChoiceActionItem SavedChoiceActionItem
        {
            get
            {
                if (_savedChoiceActionItem == null)
                {
                    if (Application.Model.ActionDesign.Actions[filteringCriterionAction.Id]
                            .ChoiceActionItems.GetNode(this.View.Id) == null)
                    {
                        _savedChoiceActionItem = Application.Model.ActionDesign.Actions[filteringCriterionAction.Id]
                            .ChoiceActionItems.AddNode<IModelChoiceActionItem>(this.View.Id);
                    }
                    else
                    {
                        _savedChoiceActionItem = Application.Model.ActionDesign.Actions[filteringCriterionAction.Id]
                            .ChoiceActionItems[this.View.Id];
                    }
                }
                return _savedChoiceActionItem;
            }
        }
        public FilterDateViewController()
        {
            filteringCriterionAction = new SingleChoiceAction(
                    this, "FilteringDateCriterion", PredefinedCategory.Filters);
            filteringCriterionAction.Caption = "Lọc thời gian";
            filteringCriterionAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FilteringCriterionAction_Execute);
            
            TargetObjectType = typeof(IFilterDate);
            TargetViewNesting = Nesting.Root;
            TargetViewType = ViewType.ListView;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            
            // Perform various tasks depending on the target View.
        }
        protected override void OnDeactivated()
        {
			filteringCriterionAction.Items.Clear();
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        private string criteriaData = null;
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            var listview = View as ListView;
            if (listview != null)
            {
                foreach (CustomFilter customAttribute in View.ObjectTypeInfo.Type.GetCustomAttributes<CustomFilter>())
                {
                    if (customAttribute.Name.Equals(nameof(IFilterDate)))
                    {
                        criteriaData = customAttribute.Criteria;
                        break;
                    }
                }
                if (filteringCriterionAction.Items.Count > 0)
                {
                    if (filteringCriterionAction.SelectedItem != null)
                    {
                        filteringCriterionAction.DoExecute(filteringCriterionAction.SelectedItem);
                    }
                }
                else
                if (filteringCriterionAction.SelectedItem == null)
                {
                    CreateDefaultFilter(null);
                    if (filteringCriterionAction.Items.Count > 0)
                    {
                        if (filteringCriterionAction.SelectedItem == null)
                        {
                            filteringCriterionAction.SelectedIndex = 0;
                        }
                        else if (filteringCriterionAction.SelectedIndex != 0)
                        {
                            filteringCriterionAction.DoExecute(filteringCriterionAction.SelectedItem);
                        }
                    }
                }

            }
          
        }
        private void CreateDefaultFilter(CriteriaOperator criteria)
        {
            if (string.IsNullOrEmpty(criteriaData))
            {
                if(View.ObjectTypeInfo.FindMember("Update") != null)
                    criteriaData = "Update";
                else if(View.ObjectTypeInfo.FindMember("Date") != null)
                    criteriaData = "Date";
                else if (View.ObjectTypeInfo.FindMember("CreateDate") != null)
                    criteriaData = "CreateDate";
            }
            if (!string.IsNullOrEmpty(criteriaData))
            {
                var choiceActionThisMonth = new ChoiceActionItem("ThisMonth", "Tháng này", string.Format("IsThisMonth({0})", criteriaData));               
                filteringCriterionAction.Items.Add(choiceActionThisMonth);
                var today = (DateTime)View.ObjectSpace.Evaluate(typeof(XPObjectType),
                        (CriteriaOperator)new FunctionOperator(FunctionOperatorType.Now, new CriteriaOperator[0]), (CriteriaOperator)null);
                if (today.Month == 1 || today.Month == 2 || today.Month == 3)
                {
                    var choiceActionThisQuarter = new ChoiceActionItem("ThisQuarter", "Quý này", string.Format("(GetMonth({0}) = 1 or GetMonth({0}) = 2 or GetMonth({0}) = 3) and IsThisYear({0})", criteriaData));
                    filteringCriterionAction.Items.Add(choiceActionThisQuarter);
                }
                else if (today.Month == 4 || today.Month == 5 || today.Month == 6)
                {
                    var choiceActionThisQuarter = new ChoiceActionItem("ThisQuarter", "Quý này", string.Format("(GetMonth({0}) = 4 or GetMonth({0}) = 5 or GetMonth({0}) = 6) and IsThisYear({0})", criteriaData));
                    filteringCriterionAction.Items.Add(choiceActionThisQuarter);
                }else if (today.Month == 7 || today.Month == 8 || today.Month == 9)
                {
                    var choiceActionThisQuarter = new ChoiceActionItem("ThisQuarter", "Quý này", string.Format("(GetMonth({0}) = 7 or GetMonth({0}) = 8 or GetMonth({0}) = 9) and IsThisYear({0})", criteriaData));
                    filteringCriterionAction.Items.Add(choiceActionThisQuarter);
                }
                else if (today.Month == 10 || today.Month == 11 || today.Month == 12)
                {
                    var choiceActionThisQuarter = new ChoiceActionItem("ThisQuarter", "Quý này", string.Format("(GetMonth({0}) = 10 or GetMonth({0}) = 11 or GetMonth({0}) = 12) and IsThisYear({0})", criteriaData));
                    filteringCriterionAction.Items.Add(choiceActionThisQuarter);
                }
                var choiceActionThisYear = new ChoiceActionItem("ThisYear", "Năm nay", string.Format("IsThisYear({0})", criteriaData));
                filteringCriterionAction.Items.Add(choiceActionThisYear);

                if (filteringCriterionAction.Items.FindItemByID("AllDate") == null)
                    filteringCriterionAction.Items.Add(new ChoiceActionItem("AllDate", "Tất cả", null));
                 
                

                if (filteringCriterionAction.SelectedItem == null && !string.IsNullOrEmpty(SavedChoiceActionItem.ToolTip))
                {
                    foreach (var choiceItem in filteringCriterionAction.Items)
                    {
                        if (choiceItem.Id.Equals(SavedChoiceActionItem.ToolTip))
                        {
                            filteringCriterionAction.SelectedItem = choiceItem;
                            break;
                        }
                    }
                }
                if (filteringCriterionAction.SelectedItem == null)
                {
                    filteringCriterionAction.SelectedIndex = 0;
                }
            }
            
        }
       
        
        private void FilteringCriterionAction_Execute(
            object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var filterKey = this.GetType().Name;
            ((DevExpress.ExpressApp.ListView)View).CollectionSource.BeginUpdateCriteria();
            if (((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.ContainsKey(filterKey))
            {
                ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.Remove(filterKey);
            }
            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria[filterKey] =
                CriteriaEditorHelper.GetCriteriaOperator(
                    e.SelectedChoiceActionItem.Data as string, View.ObjectTypeInfo.Type, ObjectSpace);
            ((DevExpress.ExpressApp.ListView)View).CollectionSource.EndUpdateCriteria();
            SavedChoiceActionItem.ToolTip = e.SelectedChoiceActionItem.Id;
        }
 
    }
}
