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
    public partial class FilterFilteringFolderInContactViewController : BaseFilteringViewController<Module.BusinessObjects.IFilteringFolderInContact>
    {
        
        public FilterFilteringFolderInContactViewController()
        {
            filteringCriterionAction = new SingleChoiceAction(
                    this, "FilteringFilteringFolderInContactCriterion", PredefinedCategory.Filters);
            filteringCriterionAction.Caption = "Lọc Thư mục";
            filteringCriterionAction.ImageName = "Folder";
            filteringCriterionAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FilteringCriterionAction_Execute);
            filteringCriterionAction.ItemType = SingleChoiceActionItemType.ItemIsMode;
            //this.Actions.Add(filteringCriterionAction);
            //TargetObjectType = typeof(Module.BusinessObjects.IFilteringFolderInContact);
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
                    CreateDefaultFilter(CriteriaOperator.Parse("FolderType = 'Contact' and UpperFolder.FolderType <> 'Contact'"));
        } 
        private void CreateDefaultFilter(CriteriaOperator criteria)
        {
            //var listFolder = ObjectSpace.GetObjects<Module.BusinessObjects.Folder>(criteria);
            var sortProperties = new DevExpress.Xpo.SortProperty[] { new DevExpress.Xpo.SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending) };
            var listFolder = new DevExpress.Xpo.XPCollection<Module.BusinessObjects.Folder>(((DevExpress.ExpressApp.Xpo.XPObjectSpace)ObjectSpace).Session, criteria, sortProperties);
            if(listFolder.Count == 0)
            {
                filteringCriterionAction.Items.Clear();
                return;
            }
            if (filteringCriterionAction.Items.FindItemByID("AllFolder") == null)
                filteringCriterionAction.Items.Add(new ChoiceActionItem("AllFolder", "Tất cả Thư mục", null));
            
            var oidList = listFolder.Select(f => f.Oid.ToString()).ToHashSet();
            RemoveOldChoice(oidList);    
            string data = "[Folder.Oid] = ?";
            LoadDataFilter(ref data);
            foreach (var folder in listFolder.OrderBy(m => m.Name))
            {
                if (filteringCriterionAction.Items.FindItemByID(folder.Oid.ToString()) == null)
                {
                    CreateTreeSource(null, folder, data, "");
                }

            }
            SavedChoiceDefaultFilter();
        }
        //private CriteriaOperator AddAllChildCriteriaOperator(Module.BusinessObjects.Folder currentItem, CriteriaOperator currentCriteriaOperator, string data)
        //{
        //    if (currentItem.LowerFolder != null && currentItem.LowerFolder.Count > 0)
        //    {
        //        foreach (var childGroup in currentItem.LowerFolder)
        //        {
        //            var childGroupParse = CriteriaOperator.Parse(data, childGroup.Oid, childGroup.Oid,
       //                 childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid,
        //                childGroup.Oid, childGroup.Oid);
        //            currentCriteriaOperator = CriteriaOperator.Or(currentCriteriaOperator, childGroupParse);
        //            currentCriteriaOperator = AddAllChildCriteriaOperator(childGroup, currentCriteriaOperator, data);
       //         }
       //     }
       //     return currentCriteriaOperator;
       // }

        private CriteriaOperator AddAllChildCriteriaOperator(Module.BusinessObjects.Folder currentItem,CriteriaOperator currentCriteriaOperator, string data, int maxDepth = 100) // Không giới hạn độ sâu mặc định
        {
            var stack = new Stack<(Module.BusinessObjects.Folder Folder, int Depth)>();
            stack.Push((currentItem, 1));

            while (stack.Count > 0)
            {
                var (folder, depth) = stack.Pop();

                if (folder.LowerFolder != null && folder.LowerFolder.Count > 0 && depth < maxDepth)
                {
                    foreach (var childGroup in folder.LowerFolder)
                    {
                        var childGroupParse = CriteriaOperator.Parse(data, childGroup.Oid, childGroup.Oid,
                            childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid,
                            childGroup.Oid, childGroup.Oid);
                        currentCriteriaOperator = CriteriaOperator.Or(currentCriteriaOperator, childGroupParse);

                        stack.Push((childGroup, depth + 1));
                    }
                }
            }
            return currentCriteriaOperator;
        }


        //private void CreateTreeSource(ChoiceActionItem parentItem, Module.BusinessObjects.Folder currentItem, string data, string prefix)
        //{
        //    if (currentItem != null && !string.IsNullOrEmpty(data))
        //    {
        //        //var foundItem = filteringCriterionAction.Items.FindItemByID(currentItem.Oid.ToString());
        //        //if (foundItem == null)
        //        //{
        //        //    var criteriaParse = CriteriaOperator.Parse(data, currentItem.Oid, currentItem.Oid,
        //        //        //currentItem.Oid,
        //        //        //currentItem.Oid, currentItem.Oid, currentItem.Oid, currentItem.Oid,
        //        //        //currentItem.Oid,
        //        //        //currentItem.Oid,
        //        //        //currentItem.Oid);
        //        //    criteriaParse = AddAllChildCriteriaOperator(currentItem, criteriaParse, data);

        //        //    string parser = criteriaParse.LegacyToString();
        //        //    var choiceAction = new ChoiceActionItem(currentItem.Oid.ToString(), currentItem.Name, parser);
        //        //    if (parentItem == null)
        //        //    {
        //        //        ////Thêm vào gốc
        //        //        //filteringCriterionAction.Items.Add(choiceAction);
        //        //    }
        //        //    else
        //        //    {
        //        //        ////Thêm vào cành
        //        //        ////parentItem.Items.Add(choiceAction); 
        //        //        //prefix += "    ";
        //        //        //choiceAction.Caption = prefix + choiceAction.Caption;
        //        //        //filteringCriterionAction.Items.Add(choiceAction);
        //        //    }
        //        //    foreach (var child in currentItem.LowerFolder.OrderBy(m => m.Name))
        //        //    {
        //        //        //CreateTreeSource(choiceAction, child, data, prefix);
        //        //    }
        //        //}else if(parentItem != null)
        //        //{
        //        //    filteringCriterionAction.Items.Remove(foundItem);
        //        //    foundItem.Caption = prefix + foundItem.Caption;
        //        //    filteringCriterionAction.Items.Add(foundItem);
        //        //}
        //    }
        //}

        private void CreateTreeSource(ChoiceActionItem parentItem,Module.BusinessObjects.Folder currentItem,string data,string prefix,int maxDepth = 100)
        {
            var stack = new Stack<(ChoiceActionItem Parent, Module.BusinessObjects.Folder Folder, string Prefix, int Depth)>();
            stack.Push((parentItem, currentItem, prefix, 1));

            while (stack.Count > 0)
            {
                var (curParent, curFolder, curPrefix, depth) = stack.Pop();

                if (curFolder != null && !string.IsNullOrEmpty(data))
                {
                    var foundItem = filteringCriterionAction.Items.FindItemByID(curFolder.Oid.ToString());
                    if (foundItem == null)
                    {
                        var criteriaParse = CriteriaOperator.Parse(data, curFolder.Oid, curFolder.Oid,
                            curFolder.Oid, curFolder.Oid, curFolder.Oid, curFolder.Oid, curFolder.Oid,
                            curFolder.Oid, curFolder.Oid, curFolder.Oid);
                        criteriaParse = AddAllChildCriteriaOperator(curFolder, criteriaParse, data, maxDepth);

                        string parser = criteriaParse.LegacyToString();
                        var choiceAction = new ChoiceActionItem(curFolder.Oid.ToString(), curFolder.Name, parser);
                        if (curParent == null)
                        {
                            filteringCriterionAction.Items.Add(choiceAction);
                        }
                        else
                        {
                            var newPrefix = curPrefix + "    ";
                            choiceAction.Caption = newPrefix + choiceAction.Caption;
                            filteringCriterionAction.Items.Add(choiceAction);
                        }

                        // Đẩy các child vào stack nếu chưa vượt quá maxDepth
                        if (curFolder.LowerFolder != null && depth < maxDepth)
                        {
                            foreach (var child in curFolder.LowerFolder.OrderBy(m => m.Name).Reverse())
                            {
                                stack.Push((choiceAction, child, curPrefix + "    ", depth + 1));
                            }
                        }
                    }
                    else if (curParent != null)
                    {
                        filteringCriterionAction.Items.Remove(foundItem);
                        foundItem.Caption = curPrefix + foundItem.Caption;
                        filteringCriterionAction.Items.Add(foundItem);
                    }
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

 
    }
}