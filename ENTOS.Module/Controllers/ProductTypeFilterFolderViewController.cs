using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Module.Controllers
{    
    public partial class ProductTypeFilterFolderViewController : ViewController
    {
        private SingleChoiceAction filteringCriterionAction;
        //private IModelChoiceActionItem _savedChoiceActionItem;
        //private IModelChoiceActionItem SavedChoiceActionItem
        //{
        //    get
        //    {
        //        if (_savedChoiceActionItem == null)
        //        {
        //            if (Application.Model.ActionDesign.Actions[filteringCriterionAction.Id]
        //                    .ChoiceActionItems.GetNode(this.View.Id) == null)
        //            {
        //                _savedChoiceActionItem = Application.Model.ActionDesign.Actions[filteringCriterionAction.Id]
        //                    .ChoiceActionItems.AddNode<IModelChoiceActionItem>(this.View.Id);
        //            }
        //            else
        //            {
        //                _savedChoiceActionItem = Application.Model.ActionDesign.Actions[filteringCriterionAction.Id]
        //                    .ChoiceActionItems[this.View.Id];
        //            }
        //        }
        //        return _savedChoiceActionItem;
        //    }
        //}
        public ProductTypeFilterFolderViewController()
        {
            filteringCriterionAction = new SingleChoiceAction(
                    this, "ProductTypeFilterFolderCriterion", PredefinedCategory.PopupActions);
            filteringCriterionAction.Caption = "Lọc thư mục";
            filteringCriterionAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FilteringCriterionAction_Execute);
            filteringCriterionAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            TargetObjectType = typeof(Module.BusinessObjects.ProductType);
            TargetViewNesting = Nesting.Nested;
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
                    //var masterObject = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Folder;

                    CreateDefaultFilter(CriteriaOperator.Parse("UpperFolder is null and FolderType = 'ProductType'"));
                    //CreateDefaultFilter(null);
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
            //criteria = CriteriaOperator.And(criteria, CriteriaOperator.Parse("UpperFolder is null"));
            if (filteringCriterionAction.Items.FindItemByID("AllFolder") == null)
                filteringCriterionAction.Items.Add(new ChoiceActionItem("AllFolder", "Tất cả Thư mục", null));
            var listFolder = ObjectSpace.GetObjects<Module.BusinessObjects.Folder>(criteria);
            IList<ChoiceActionItem> listRemove = new List<ChoiceActionItem>();
            foreach (var item in filteringCriterionAction.Items)
            {
                if (item.Data != null)
                {
                    bool remove = true;
                    foreach (var folder in listFolder)
                    {
                        if (item.Id.Equals(folder.Oid.ToString()))
                        {
                            remove = false;
                            break;
                        }
                    }

                    if (remove)
                    {
                        listRemove.Add(item);
                    }
                }
            }

            foreach (var item in listRemove)
            {
                filteringCriterionAction.Items.Remove(item);
            }

            foreach (var folder in listFolder.OrderBy(m => m.Name))
            {
                if (filteringCriterionAction.Items.FindItemByID(folder.Oid.ToString()) == null)
                {
                    string data = "[Folder.Oid] = ?";
                    foreach (CustomFilter customAttribute in View.ObjectTypeInfo.Type.GetCustomAttributes<CustomFilter>())
                    {
                        if (customAttribute.Name.Equals("IFolder"))
                        {
                            data = customAttribute.Criteria;
                            break;
                        }
                    }
                    CreateTreeSource(null, folder, data, "");
                }

            }
            //if (filteringCriterionAction.SelectedItem == null && !string.IsNullOrEmpty(SavedChoiceActionItem.ToolTip))
            //{
            //    foreach (var choiceItem in filteringCriterionAction.Items)
            //    {
            //        if (choiceItem.Id.Equals(SavedChoiceActionItem.ToolTip))
            //        {
            //            filteringCriterionAction.SelectedItem = choiceItem;
            //            break;
            //        }
            //    }
            //}
            if (filteringCriterionAction.SelectedItem == null)
            {
                filteringCriterionAction.SelectedIndex = 0;
            }
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
        private void CreateTreeSource(ChoiceActionItem parentItem, Module.BusinessObjects.Folder currentItem, string data, string prefix)
        {
            if (currentItem != null && !string.IsNullOrEmpty(data))
            {
                var foundItem = filteringCriterionAction.Items.FindItemByID(currentItem.Oid.ToString());
                if (foundItem == null)
                {
                    var criteriaParse = CriteriaOperator.Parse(data, currentItem.Oid, currentItem.Oid,
                        currentItem.Oid,
                        currentItem.Oid, currentItem.Oid, currentItem.Oid, currentItem.Oid,
                        currentItem.Oid,
                        currentItem.Oid,
                        currentItem.Oid);
                    criteriaParse = AddAllChildCriteriaOperator(currentItem, criteriaParse, data);
                    
                    string parser = criteriaParse.LegacyToString();
                    var choiceAction = new ChoiceActionItem(currentItem.Oid.ToString(), currentItem.Name, parser);
                    if (parentItem == null)
                    {
                        //Thêm vào gốc
                        filteringCriterionAction.Items.Add(choiceAction);
                    }
                    else
                    {
                        //Thêm vào cành
                        //parentItem.Items.Add(choiceAction); 
                        if (filteringCriterionAction.ItemType == SingleChoiceActionItemType.ItemIsMode)
                        {
                            prefix += "   ";
                            choiceAction.Caption = prefix + choiceAction.Caption;
                            filteringCriterionAction.Items.Add(choiceAction);
                        }
                        else
                            parentItem.Items.Add(choiceAction);
                    }
                    foreach (var child in currentItem.LowerFolder.Where(m => m.Order != null).OrderBy(m => m.Order))
                    {
                        CreateTreeSource(choiceAction, child, data, prefix);
                    }
                }else if(parentItem != null)
                {
                    filteringCriterionAction.Items.Remove(foundItem);
                    filteringCriterionAction.Items.Remove(foundItem);
                    if (filteringCriterionAction.ItemType == SingleChoiceActionItemType.ItemIsMode)
                    {
                        foundItem.Caption = prefix + foundItem.Caption;
                        filteringCriterionAction.Items.Add(foundItem);
                    }
                    else
                        parentItem.Items.Add(foundItem);
                }
            }
        }
        public CriteriaOperator AddAllChildCriteriaOperator(CriteriaOperator currentCriteriaOperator, string data)
        {
            if (filteringCriterionAction != null && filteringCriterionAction.SelectedItem != null)
            {
                var currentItemOid = Guid.Parse(filteringCriterionAction.SelectedItem.Id);
                var criteriaParse = CriteriaOperator.Parse(data, currentItemOid, currentItemOid,
                    currentItemOid,
                    currentItemOid, currentItemOid, currentItemOid, currentItemOid,
                    currentItemOid,
                    currentItemOid,
                    currentItemOid);
                var currentItem = ObjectSpace.GetObjectByKey<Module.BusinessObjects.Folder>(currentItemOid);
                if (currentItem != null)
                    return AddAllChildCriteriaOperator(currentItem, criteriaParse, data);
            }
            return currentCriteriaOperator;
        }
        public bool IsTree = true;
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
            //SavedChoiceActionItem.ToolTip = e.SelectedChoiceActionItem.Id;
        }
 
    }
}