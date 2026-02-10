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
    public partial class IFilterOwnerViewController: ViewController
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
        public IFilterOwnerViewController()
        {
            filteringCriterionAction = new SingleChoiceAction(
                    this, "FilteringOwnerCriterion", PredefinedCategory.Filters);
            filteringCriterionAction.Caption = "Lọc của tôi";
            filteringCriterionAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FilteringCriterionAction_Execute);
            
            TargetObjectType = typeof(IFilterOwner);
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
                    if (customAttribute.Name.Equals(nameof(IFilterOwner)))
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
                        else// if (filteringCriterionAction.SelectedIndex != 0)
                        {
                            filteringCriterionAction.DoExecute(filteringCriterionAction.SelectedItem);
                        }
                    }
                }

            }
          
        }
        private void CreateDefaultFilter(CriteriaOperator criteria)
        {
            if (!string.IsNullOrEmpty(criteriaData))
            {
                var choiceActionOwner = new ChoiceActionItem("Owner", "Của tôi", criteriaData);               
                filteringCriterionAction.Items.Add(choiceActionOwner);

                if (filteringCriterionAction.Items.FindItemByID("All") == null)
                    filteringCriterionAction.Items.Add(new ChoiceActionItem("All", "Tất cả", null));                                 

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
