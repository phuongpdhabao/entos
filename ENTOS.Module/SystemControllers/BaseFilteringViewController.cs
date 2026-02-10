using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using System.ComponentModel;
using System.Reflection;

namespace ENTOS.Module.FilterControllers
{
    [ToolboxItem(false)]
    public abstract class BaseFilteringViewController<T> : ObjectViewController<ListView, T>
    {
    
        protected SingleChoiceAction filteringCriterionAction;
        private IModelChoiceActionItem _savedChoiceActionItem;
        protected IModelChoiceActionItem SavedChoiceActionItem
        {
            get
            {
                if (_savedChoiceActionItem == null)
                {
                    var actionNode = Application.Model.ActionDesign.Actions[filteringCriterionAction.Id];
                    if (actionNode != null)
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
                }
                return _savedChoiceActionItem;
            }
        }
        protected override void OnDeactivated()
        {
            filteringCriterionAction.Items.Clear();
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }



        protected abstract void CreateDefaultFilter();

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            var listview = View as ListView;
            if (listview != null)
            {
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
                    CreateDefaultFilter();
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

        protected void FilteringCriterionAction_Execute(
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

        protected void SavedChoiceDefaultFilter()
        {
            if (SavedChoiceActionItem != null && filteringCriterionAction.SelectedItem == null && !string.IsNullOrEmpty(SavedChoiceActionItem?.ToolTip))
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


        protected void RemoveOldChoice(HashSet<string> validIds)
        {                
            
            var itemsToRemove = filteringCriterionAction.Items
                .Where(item => item.Data != null && !validIds.Contains(item.Id))
                .ToList();
            foreach (var item in itemsToRemove)
                filteringCriterionAction.Items.Remove(item);                      
        }
            
        protected void LoadDataFilter(ref string data)
        {
            foreach (Module.SystemObjects.CustomFilter customAttribute in View.ObjectTypeInfo.Type.GetCustomAttributes<Module.SystemObjects.CustomFilter>())
            {
                if (customAttribute.Name.Equals(nameof(T)))
                {
                    data = customAttribute.Criteria;
                    break;
                }
            }           
        }

    }
}
