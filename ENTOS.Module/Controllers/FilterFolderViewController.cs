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
    public partial class FilterFolderViewController : ViewController
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
        public FilterFolderViewController()
        {
            filteringCriterionAction = new SingleChoiceAction(
                    this, "FilteringFolderCriterion", PredefinedCategory.Filters);
            filteringCriterionAction.Caption = "Lọc Thư mục";
            filteringCriterionAction.ImageName = "Folder";
            filteringCriterionAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            filteringCriterionAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FilteringCriterionAction_Execute);
            filteringCriterionAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            TargetObjectType = typeof(IFolder);
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
                    if (filteringCriterionAction.Items.FindItemByID("AllFolder") == null)
                        filteringCriterionAction.Items.Add(new ChoiceActionItem("AllFolder", "Tất cả Thư mục", null));
                    //Hỗ trợ lazy load
                    //filteringCriterionAction.ShowItemsOnClick = true;
                    filteringCriterionAction.ProcessCreatedView += delegate (object sender, ActionBaseEventArgs e)
                    {
                        CreateDefaultFilter(CriteriaOperator.Parse("UpperFolder is null"));                        
                    };                
                    //CreateDefaultFilter(CriteriaOperator.Parse("UpperFolder is null"));
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
          
        }
        private IList<Module.BusinessObjects.Folder> allFolders = null;
        private void CreateDefaultFilter(CriteriaOperator criteria)
        {
            if(allFolders == null)
                allFolders = View.ObjectSpace.GetObjects<Module.BusinessObjects.Folder>(null, new List<SortProperty> { new SortProperty { PropertyName = "Order" } }, false);
            var listFolder = allFolders.Where(x => x.UpperFolder is null);
            existed = new List<Guid>();
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

            foreach (var folder in listFolder)
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
            if (filteringCriterionAction.SelectedItem == null && !string.IsNullOrEmpty(SavedChoiceActionItem.ToolTip))
            {
                var choiceItem = FindItemByID(filteringCriterionAction.Items, SavedChoiceActionItem.ToolTip);
                if (choiceItem != null)
                    filteringCriterionAction.SelectedItem = choiceItem;
                //foreach (var choiceItem in filteringCriterionAction.Items)
                //{
                //    if (choiceItem.Id.Equals(SavedChoiceActionItem.ToolTip))
                //    {
                //        filteringCriterionAction.SelectedItem = choiceItem;
                //        break;
                //    }
                //}
            }
            if (filteringCriterionAction.SelectedItem == null)
            {
                filteringCriterionAction.SelectedIndex = 0;
            }
        }
        private ChoiceActionItem FindItemByID(ChoiceActionItemCollection items, string id)
        {
            var result = items.FindItemByID(id);
            if (result != null)
                return result;
            foreach(var childItem in items)
            {
                result = FindItemByID(childItem.Items, id);
                if (result != null)
                    return result;
            }
            return null;
        }
        private IList<Guid> existedCriteria = null;
        private CriteriaOperator AddAllChildCriteriaOperator(System.Guid currentItem, CriteriaOperator currentCriteriaOperator, string data)
        {            
            if(!existedCriteria.Contains(currentItem))
            {
                existedCriteria.Add(currentItem);
                var lowerFolder = allFolders?.Where(x => x.UpperFolder != null && x.UpperFolder?.Oid == currentItem);
                if (lowerFolder != null && lowerFolder.Count() > 0)
                {
                    foreach (var childGroup in lowerFolder)
                    {
                        var childGroupParse = CriteriaOperator.Parse(data, childGroup.Oid, childGroup.Oid,
                            childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid,
                            childGroup.Oid, childGroup.Oid);
                        currentCriteriaOperator = CriteriaOperator.Or(currentCriteriaOperator, childGroupParse);
                        currentCriteriaOperator = AddAllChildCriteriaOperator(currentItem, currentCriteriaOperator, data);
                    }
                }
            }
            else
            {
                //Bị trùng
            }
            return currentCriteriaOperator;
        }

        //Kiểm tra xem có bị add trùng không
        private IList<Guid> existed = null;
        private void CreateTreeSource(ChoiceActionItem parentItem, Module.BusinessObjects.Folder currentItem, string data, string prefix)
        {
            if (currentItem != null && !string.IsNullOrEmpty(data))
            {
                var foundItem = filteringCriterionAction.Items.FindItemByID(currentItem.Oid.ToString()); 
                if (foundItem == null && parentItem != null)
                {
                    foundItem = parentItem.Items.FindItemByID(currentItem.Oid.ToString());
                }
                if (foundItem == null)
                {
                    if(currentItem.Oid == System.Guid.Parse("d5b71606-ffb4-434c-a975-52a5741bfd24"))
                    {

                    }
                    if(existed.Contains(currentItem.Oid))
                        return;
                    existed.Add(currentItem.Oid);
                    //var criteriaParse = CriteriaOperator.Parse(data, currentItem.Oid, currentItem.Oid,
                    //    currentItem.Oid,
                    //    currentItem.Oid, currentItem.Oid, currentItem.Oid, currentItem.Oid,
                    //    currentItem.Oid,
                    //    currentItem.Oid,
                    //    currentItem.Oid);
                    //criteriaParse = AddAllChildCriteriaOperator(currentItem, criteriaParse, data);

                    //string parser = criteriaParse.LegacyToString();
                    //var choiceAction = new ChoiceActionItem(currentItem.Oid.ToString(), currentItem.Name, parser);
                    var choiceAction = new ChoiceActionItem(currentItem.Oid.ToString(), currentItem.Name, data);
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
                            prefix += "    ";
                            choiceAction.Caption = prefix + choiceAction.Caption;
                            filteringCriterionAction.Items.Add(choiceAction);
                        }    
                        else
                            parentItem.Items.Add(choiceAction);
                        
                    }
                    var lowerFolder = allFolders?.Where( x => x.UpperFolder != null && x.UpperFolder?.Oid == currentItem.Oid);
                    //foreach (var child in currentItem.LowerFolder.OrderBy(m => m.Order))
                    //Dùng lazy load
                    foreach (var child in lowerFolder)
                    {
                        CreateTreeSource(choiceAction, child, data, prefix);
                    }
                }else if(parentItem != null)
                {
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
                    return AddAllChildCriteriaOperator(currentItemOid, criteriaParse, data);
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
            string data = e.SelectedChoiceActionItem.Data as string;
            if (!string.IsNullOrEmpty(data))
            {
                if (existedCriteria == null)
                    existedCriteria = new List<Guid>();
                else
                    existedCriteria.Clear();
                var currentId = Guid.Parse(e.SelectedChoiceActionItem.Id);
                var criteriaParse = CriteriaOperator.Parse(data, currentId, currentId, currentId);
                criteriaParse = AddAllChildCriteriaOperator(currentId, criteriaParse, data);
                data = criteriaParse.LegacyToString();
            }
            
            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria[filterKey] =
                CriteriaEditorHelper.GetCriteriaOperator(
                    data, View.ObjectTypeInfo.Type, ObjectSpace);
            ((DevExpress.ExpressApp.ListView)View).CollectionSource.EndUpdateCriteria();
            if (e.SelectedChoiceActionItem.Data is null)
            {
                filteringCriterionAction.Caption = "Lọc thư mục";
            }
            else
            {
                filteringCriterionAction.Caption = "Lọc " + e.SelectedChoiceActionItem.Caption;
            }
            SavedChoiceActionItem.ToolTip = e.SelectedChoiceActionItem.Id;
        }
 
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using DevExpress.Data.Filtering;
//using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.Actions;
//using DevExpress.ExpressApp.Editors;
//using DevExpress.ExpressApp.Model;
//using DevExpress.Persistent.Base;
//using DevExpress.Xpo;
//using DevExpress.Xpo.Helpers;
//using ENTOS.Module.BusinessObjects;
//using ENTOS.Module.SystemObjects;

//namespace ENTOS.Module.FilterControllers
//{
//    public partial class FilterFolderViewController : ViewController
//    {
//        private SingleChoiceAction filteringCriterionAction;
//        private IModelChoiceActionItem _savedChoiceActionItem;
//        private IModelChoiceActionItem SavedChoiceActionItem
//        {
//            get
//            {
//                if (_savedChoiceActionItem == null)
//                {
//                    if (Application.Model.ActionDesign.Actions[filteringCriterionAction.Id]
//                            .ChoiceActionItems.GetNode(this.View.Id) == null)
//                    {
//                        _savedChoiceActionItem = Application.Model.ActionDesign.Actions[filteringCriterionAction.Id]
//                            .ChoiceActionItems.AddNode<IModelChoiceActionItem>(this.View.Id);
//                    }
//                    else
//                    {
//                        _savedChoiceActionItem = Application.Model.ActionDesign.Actions[filteringCriterionAction.Id]
//                            .ChoiceActionItems[this.View.Id];
//                    }
//                }
//                return _savedChoiceActionItem;
//            }
//        }
//        public FilterFolderViewController()
//        {
//            filteringCriterionAction = new SingleChoiceAction(
//                    this, "FilteringFolderCriterion", PredefinedCategory.Filters);
//            filteringCriterionAction.Caption = "Lọc Thư mục";
//            filteringCriterionAction.ImageName = "Folder";
//            filteringCriterionAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
//            filteringCriterionAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FilteringCriterionAction_Execute);
//            filteringCriterionAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
//            TargetObjectType = typeof(IFolder);
//            TargetViewNesting = Nesting.Root;
//            TargetViewType = ViewType.ListView;
//            // Target required Views (via the TargetXXX properties) and create their Actions.
//        }
//        protected override void OnActivated()
//        {
//            base.OnActivated();

//            // Perform various tasks depending on the target View.
//        }
//        protected override void OnDeactivated()
//        {
//            filteringCriterionAction.Items.Clear();
//            // Unsubscribe from previously subscribed events and release other references and resources.
//            base.OnDeactivated();
//        }

//        protected override void OnViewControlsCreated()
//        {
//            base.OnViewControlsCreated();
//            var listview = View as ListView;
//            if (listview != null)
//            {
//                if (filteringCriterionAction.Items.Count > 0)
//                {
//                    if (filteringCriterionAction.SelectedItem != null)
//                    {
//                        filteringCriterionAction.DoExecute(filteringCriterionAction.SelectedItem);
//                    }
//                }
//                else
//                if (filteringCriterionAction.SelectedItem == null)
//                {
//                    CreateDefaultFilter(CriteriaOperator.Parse("UpperFolder is null"));
//                    if (filteringCriterionAction.Items.Count > 0)
//                    {
//                        if (filteringCriterionAction.SelectedItem == null)
//                        {
//                            filteringCriterionAction.SelectedIndex = 0;
//                        }
//                        else if (filteringCriterionAction.SelectedIndex != 0)
//                        {
//                            filteringCriterionAction.DoExecute(filteringCriterionAction.SelectedItem);
//                        }
//                    }
//                }
//            }

//        }
//        private void CreateDefaultFilter(CriteriaOperator criteria)
//        {
//            if (filteringCriterionAction.Items.FindItemByID("AllFolder") == null)
//                filteringCriterionAction.Items.Add(new ChoiceActionItem("AllFolder", "Tất cả Thư mục", null));
//            var listFolder = ObjectSpace.GetObjects<Module.BusinessObjects.Folder>(criteria, new System.Collections.Generic.List<SortProperty> { new SortProperty { PropertyName = "Order" } }, true);
//            IList<ChoiceActionItem> listRemove = new List<ChoiceActionItem>();
//            foreach (var item in filteringCriterionAction.Items)
//            {
//                if (item.Data != null)
//                {
//                    bool remove = true;
//                    foreach (var folder in listFolder)
//                    {
//                        if (item.Id.Equals(folder.Oid.ToString()))
//                        {
//                            remove = false;
//                            break;
//                        }
//                    }

//                    if (remove)
//                    {
//                        listRemove.Add(item);
//                    }
//                }
//            }

//            foreach (var item in listRemove)
//            {
//                filteringCriterionAction.Items.Remove(item);
//            }

//            foreach (var folder in listFolder)
//            {
//                if (filteringCriterionAction.Items.FindItemByID(folder.Oid.ToString()) == null)
//                {
//                    string data = "[Folder.Oid] = ?";
//                    foreach (CustomFilter customAttribute in View.ObjectTypeInfo.Type.GetCustomAttributes<CustomFilter>())
//                    {
//                        if (customAttribute.Name.Equals("IFolder"))
//                        {
//                            data = customAttribute.Criteria;
//                            break;
//                        }
//                    }
//                    CreateTreeSource(null, folder, data, "");
//                }

//            }
//            if (filteringCriterionAction.SelectedItem == null && !string.IsNullOrEmpty(SavedChoiceActionItem.ToolTip))
//            {
//                var choiceItem = FindItemByID(filteringCriterionAction.Items, SavedChoiceActionItem.ToolTip);
//                if (choiceItem != null)
//                    filteringCriterionAction.SelectedItem = choiceItem;
//                //foreach (var choiceItem in filteringCriterionAction.Items)
//                //{
//                //    if (choiceItem.Id.Equals(SavedChoiceActionItem.ToolTip))
//                //    {
//                //        filteringCriterionAction.SelectedItem = choiceItem;
//                //        break;
//                //    }
//                //}
//            }
//            if (filteringCriterionAction.SelectedItem == null)
//            {
//                filteringCriterionAction.SelectedIndex = 0;
//            }
//        }
//        private ChoiceActionItem FindItemByID(ChoiceActionItemCollection items, string id)
//        {
//            var result = items.FindItemByID(id);
//            if (result != null)
//                return result;
//            foreach (var childItem in items)
//            {
//                result = FindItemByID(childItem.Items, id);
//                if (result != null)
//                    return result;
//            }
//            return null;
//        }

//        private CriteriaOperator AddAllChildCriteriaOperator(Module.BusinessObjects.Folder currentItem, CriteriaOperator currentCriteriaOperator, string data)
//        {
//            if (currentItem.LowerFolder != null && currentItem.LowerFolder.Count > 0)
//            {
//                foreach (var childGroup in currentItem.LowerFolder)
//                {
//                    var childGroupParse = CriteriaOperator.Parse(data, childGroup.Oid, childGroup.Oid,
//                        childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid,
//                        childGroup.Oid, childGroup.Oid);
//                    currentCriteriaOperator = CriteriaOperator.Or(currentCriteriaOperator, childGroupParse);
//                    currentCriteriaOperator = AddAllChildCriteriaOperator(childGroup, currentCriteriaOperator, data);
//                }
//            }
//            return currentCriteriaOperator;
//        }
//        private void CreateTreeSource(ChoiceActionItem parentItem, Module.BusinessObjects.Folder currentItem, string data, string prefix)
//        {
//            if (currentItem != null && !string.IsNullOrEmpty(data))
//            {
//                var foundItem = filteringCriterionAction.Items.FindItemByID(currentItem.Oid.ToString());
//                if (foundItem == null)
//                {
//                    var criteriaParse = CriteriaOperator.Parse(data, currentItem.Oid, currentItem.Oid,
//                        currentItem.Oid,
//                        currentItem.Oid, currentItem.Oid, currentItem.Oid, currentItem.Oid,
//                        currentItem.Oid,
//                        currentItem.Oid,
//                        currentItem.Oid);
//                    criteriaParse = AddAllChildCriteriaOperator(currentItem, criteriaParse, data);

//                    string parser = criteriaParse.LegacyToString();
//                    var choiceAction = new ChoiceActionItem(currentItem.Oid.ToString(), currentItem.Name, parser);
//                    if (parentItem == null)
//                    {
//                        //Thêm vào gốc
//                        filteringCriterionAction.Items.Add(choiceAction);
//                    }
//                    else
//                    {
//                        //Thêm vào cành
//                        //parentItem.Items.Add(choiceAction); 
//                        if (filteringCriterionAction.ItemType == SingleChoiceActionItemType.ItemIsMode)
//                        {
//                            prefix += "    ";
//                            choiceAction.Caption = prefix + choiceAction.Caption;
//                            filteringCriterionAction.Items.Add(choiceAction);
//                        }
//                        else
//                            parentItem.Items.Add(choiceAction);

//                    }
//                    foreach (var child in currentItem.LowerFolder.OrderBy(m => m.Order))
//                    {
//                        CreateTreeSource(choiceAction, child, data, prefix);
//                    }
//                }
//                else if (parentItem != null)
//                {
//                    filteringCriterionAction.Items.Remove(foundItem);
//                    if (filteringCriterionAction.ItemType == SingleChoiceActionItemType.ItemIsMode)
//                    {
//                        foundItem.Caption = prefix + foundItem.Caption;
//                        filteringCriterionAction.Items.Add(foundItem);
//                    }
//                    else
//                        parentItem.Items.Add(foundItem);

//                }
//            }
//        }
//        public CriteriaOperator AddAllChildCriteriaOperator(CriteriaOperator currentCriteriaOperator, string data)
//        {
//            if (filteringCriterionAction != null && filteringCriterionAction.SelectedItem != null)
//            {
//                var currentItemOid = Guid.Parse(filteringCriterionAction.SelectedItem.Id);
//                var criteriaParse = CriteriaOperator.Parse(data, currentItemOid, currentItemOid,
//                    currentItemOid,
//                    currentItemOid, currentItemOid, currentItemOid, currentItemOid,
//                    currentItemOid,
//                    currentItemOid,
//                    currentItemOid);
//                var currentItem = ObjectSpace.GetObjectByKey<Module.BusinessObjects.Folder>(currentItemOid);
//                if (currentItem != null)
//                    return AddAllChildCriteriaOperator(currentItem, criteriaParse, data);
//            }
//            return currentCriteriaOperator;
//        }
//        public bool IsTree = true;
//        private void FilteringCriterionAction_Execute(
//            object sender, SingleChoiceActionExecuteEventArgs e)
//        {
//            var filterKey = this.GetType().Name;
//            ((DevExpress.ExpressApp.ListView)View).CollectionSource.BeginUpdateCriteria();
//            if (((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.ContainsKey(filterKey))
//            {
//                ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.Remove(filterKey);
//            }
//            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria[filterKey] =
//                CriteriaEditorHelper.GetCriteriaOperator(
//                    e.SelectedChoiceActionItem.Data as string, View.ObjectTypeInfo.Type, ObjectSpace);
//            ((DevExpress.ExpressApp.ListView)View).CollectionSource.EndUpdateCriteria();
//            if (e.SelectedChoiceActionItem.Data is null)
//            {
//                filteringCriterionAction.Caption = "Lọc thư mục";
//            }
//            else
//            {
//                filteringCriterionAction.Caption = "Lọc " + e.SelectedChoiceActionItem.Caption;
//            }
//            SavedChoiceActionItem.ToolTip = e.SelectedChoiceActionItem.Id;
//        }

//    }
//}
