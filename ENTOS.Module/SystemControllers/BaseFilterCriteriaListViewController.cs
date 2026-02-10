using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Templates.ActionControls;
using DevExpress.ExpressApp.Templates.ActionControls.Binding;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Base.ReportsV2;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata.Helpers;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Module.SystemControllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class BaseFilterCriteriaListViewController : ViewController<ListView>
    {
        private IList<ActionBinding> actionBindings;
        private IDictionary<string, IModelChoiceActionItem> savedChoiceActionItems;
        public BaseFilterCriteriaListViewController()
        {
            // Target required Views (via the TargetXXX properties) and create their Actions.

        }
        protected override void OnActivated()
        {
            base.OnActivated();
            Frame.ViewChanged += Frame_ViewChanged;
            // Perform various tasks depending on the target View.
        }
        private void Frame_ViewChanged(object sender, ViewChangedEventArgs e)
        {
            if (View != null && View.ObjectTypeInfo != null && View.ObjectTypeInfo.Type.IsSubclassOf(typeof(PersistentBase)) && ObjectSpace is XPObjectSpace)
            {
                var criteria = CriteriaOperator.Parse("ObjectType = ?", View.ObjectTypeInfo.Type);
                if (View.ObjectTypeInfo.Type.BaseType != null && View.ObjectTypeInfo.Type.BaseType.IsSubclassOf(typeof(PersistentBase)))
                {
                    criteria = CriteriaOperator.Or(criteria,
                        CriteriaOperator.Parse("ObjectType = ?", View.ObjectTypeInfo.Type.BaseType));
                }

                criteria = CriteriaOperator.And(criteria, CriteriaOperator.Parse(
                    "Active and IsListView and TypeCondition is not null and (IsNullOrEmpty(ViewId) or ViewId = ?)",
                    View.Id));
                criteria = CriteriaOperator.And(criteria, CriteriaOperator.Parse(
                    "TargetViewNesting = ? or TargetViewNesting = ?", Nesting.Any,
                    View.IsRoot ? Nesting.Root : Nesting.Nested));
                var sortList = new List<SortProperty>()
                    {new SortProperty(nameof(FilterCriteria.DisplayOrder), SortingDirection.Descending)};
                var filtersObjects = GetDataInputObjectSpace().GetObjects<FilterCriteria>(criteria, sortList, false);
                if (filtersObjects != null && filtersObjects.Count > 0 && Frame.Template is IActionControlsSite)
                {
                    IActionControlContainer container = GetTargetActionContainer((IActionControlsSite)Frame.Template);
                    if (container != null)
                    {
                        foreach (var filtersObject in filtersObjects)
                        {
                            var id = string.Format("Filter_{0}_{1}_Action", filtersObject.TypeCondition.Name, View.Id);
                            if (container.FindActionControl(id) == null)
                            {
                                var filteringCriterionAction = new SingleChoiceAction(
                                    this, id, PredefinedCategory.Filters);
                                filteringCriterionAction.Execute += FilteringCriterionAction_Execute;
                                CreateDefaultFilter(filteringCriterionAction, filtersObject);
                                var singleChoiceActionControl =
                                    container.AddSingleChoiceActionControl(filteringCriterionAction.Id, false,
                                        SingleChoiceActionItemType.ItemIsMode);
                                singleChoiceActionControl.NativeControlDisposed += SingleChoiceControlOnNativeControlDisposed;
                                var filteringCriterionActionBinding = ActionBindingFactory.Instance.Create(filteringCriterionAction, singleChoiceActionControl);
                                if (actionBindings == null)
                                    actionBindings = new List<ActionBinding>();
                                actionBindings.Add(filteringCriterionActionBinding);
                            }
                        }
                    }
                }
            }
        }

        private IObjectSpace _typeObjectSpace = null;
        private IObjectSpace GetDataInputObjectSpace()
        {
            if (_typeObjectSpace is null)
                _typeObjectSpace = Application.CreateObjectSpace(typeof(FilterCriteria));
            return _typeObjectSpace;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View != null && View.ObjectTypeInfo != null && ObjectSpace is XPObjectSpace && ObjectSpace.CanInstantiate(typeof(FilterCriteria)))
            {
                try
                {
                    //var objectSpace = Application.CreateObjectSpace(typeof(FilterCriteria));
                    CriteriaOperator criteria = Module.Helpers.XafXpoHelper.GetCriteriaOperator(View.ObjectTypeInfo.Type, null, View.Id,
                        ((XPObjectSpace)GetDataInputObjectSpace()).Session);
                    if (!(criteria is null))
                    {
                        ((ListView)View).CollectionSource.BeginUpdateCriteria();
                        ((ListView)View).CollectionSource.Criteria["FilterCriteria"] = criteria;
                        //View.ObjectSpace.ParseCriteria(criteria.LegacyToString());
                        ((ListView)View).CollectionSource.EndUpdateCriteria();
                    }
                }
                catch (System.Exception) { }
                //filteringCriterionAction.Execute += FilteringCriterionAction_Execute;
            }

            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            Frame.ViewChanged -= Frame_ViewChanged;
            base.OnDeactivated();
        }

        private void CreateDefaultFilter(SingleChoiceAction filteringCriterionAction, FilterCriteria filterCriteria)
        {
            filteringCriterionAction.Items.Clear();
            string caption = CaptionHelper.GetClassCaption(filterCriteria.TypeCondition.FullName);
            filteringCriterionAction.Caption = caption;
            if (filteringCriterionAction.Items.FindItemByID("AllCategory") == null)
                filteringCriterionAction.Items.Add(new ChoiceActionItem("AllCategory", "Chọn " + caption.ToLower(), null));

            var objectTypeInfo = XafTypesInfo.Instance.FindTypeInfo(filterCriteria.TypeCondition);
            var oidMemberInfo = objectTypeInfo.FindMember("Oid");
            if (oidMemberInfo == null)
                return;
            IMemberInfo displayInfo = null;
            var defaultPropertyAttribute = objectTypeInfo.FindAttribute<DefaultPropertyAttribute>();
            if (defaultPropertyAttribute != null && !string.IsNullOrEmpty(defaultPropertyAttribute.Name))
            {
                displayInfo = objectTypeInfo.FindMember(defaultPropertyAttribute.Name);
            }
            if (displayInfo is null)
            {
                var lookupDefault = GetDataInputObjectSpace().FindObject<DefaultLookupField>(
                    CriteriaOperator.Parse(nameof(DefaultLookupField.ObjectType) + " = ?", filterCriteria.TypeCondition));
                if (lookupDefault != null && lookupDefault.Field != null)
                {
                    displayInfo = objectTypeInfo.FindMember(lookupDefault.Field.Value as string);
                }
            }
            if (displayInfo is null)
            {
                Tools.ShowMessage(Application, "Lỗi", "Không tìm thấy thuộc tính mặc định của " + caption,
                    InformationType.Error);
                return;
            }

            var listCategory = ObjectSpace.GetObjects(filterCriteria.TypeCondition, null,
                new System.Collections.Generic.List<SortProperty>()
                    {new SortProperty(displayInfo.Name, SortingDirection.Ascending)}, false);
            bool isTree = false;
            if (typeof(ITreeNode).IsAssignableFrom(filterCriteria.TypeCondition))
            {
                foreach (ITreeNode category in listCategory)
                {
                    if (category.Parent is null)
                    {
                        CreateTreeSource(filteringCriterionAction, category, SavedChoiceActionItem(filteringCriterionAction.Id).ToolTip, filterCriteria.Condition, "",
                            oidMemberInfo, displayInfo);
                        isTree = true;
                    }
                }

            }
            if (!isTree)
            {
                foreach (var category in listCategory)
                {
                    var oid = oidMemberInfo.GetValue(category);
                    if (filteringCriterionAction.Items.FindItemByID(oid.ToString()) == null)
                    {
                        var criteriaParse = CriteriaOperator.Parse(filterCriteria.Condition, oid, oid, oid,
                            oid, oid, oid, oid, oid, oid, oid);
                        var choiceAction = new ChoiceActionItem(oid.ToString(), string.Format("{0}", displayInfo.GetValue(category)),
                            criteriaParse.LegacyToString());
                        filteringCriterionAction.Items.Add(choiceAction);
                    }
                }
            }

            if (filteringCriterionAction.SelectedItem == null) filteringCriterionAction.SelectedIndex = 0;
        }

        private void CreateTreeSource(SingleChoiceAction filteringCriterionAction, ITreeNode treeNode, string selectedObjectOid, string data, string prefix, IMemberInfo oidMemberInfo, IMemberInfo captionMemberInfo)
        {
            if (treeNode != null && !string.IsNullOrEmpty(data) && oidMemberInfo != null)
            {
                var oid = oidMemberInfo.GetValue(treeNode);
                if (oid is null)
                    return;
                if (filteringCriterionAction.Items.FindItemByID(oid.ToString()) != null)
                    return;
                var criteriaParse = GetTreeCriteria(treeNode, oidMemberInfo, data, null);
                string parser = criteriaParse.LegacyToString();
                var choiceAction = new ChoiceActionItem(oid.ToString(), string.Format("{0}", captionMemberInfo.GetValue(treeNode)), parser);
                choiceAction.Caption = prefix + choiceAction.Caption;
                filteringCriterionAction.Items.Add(choiceAction);
                prefix += "     ";
                if (!string.IsNullOrEmpty(selectedObjectOid) && choiceAction.Id.Equals(selectedObjectOid))
                {
                    filteringCriterionAction.SelectedItem = choiceAction;
                }
                if (treeNode.Children != null && treeNode.Children.Count > 0)
                {
                    foreach (ITreeNode childGroup in treeNode.Children)
                    {
                        CreateTreeSource(filteringCriterionAction, childGroup, selectedObjectOid, data,
                            prefix, oidMemberInfo,
                            captionMemberInfo);
                    }

                }
            }
        }

        private CriteriaOperator GetTreeCriteria(ITreeNode currentNode, IMemberInfo oidMemberInfo, string data, CriteriaOperator criteria)
        {
            if (currentNode != null)
            {
                var oid = oidMemberInfo.GetValue(currentNode);
                if (oid != null)
                {
                    criteria = CriteriaOperator.Or(criteria,
                        CriteriaOperator.Parse(data, oid, oid, oid, oid, oid, oid, oid, oid, oid, oid, oid));
                    if (currentNode.Children != null)
                        foreach (ITreeNode childNode in currentNode.Children)
                            criteria = GetTreeCriteria(childNode, oidMemberInfo, data, criteria);

                }
            }

            return criteria;

        }

        private IActionControlContainer GetTargetActionContainer(IActionControlsSite site)
        {
            if (site == null) return null;
            foreach (IActionControlContainer container in site.ActionContainers)
            {
                if (container.ActionCategory == PredefinedCategory.Filters.ToString())
                {
                    return container;
                }
            }
            return null;
        }
        private void SingleChoiceControlOnNativeControlDisposed(object sender, System.EventArgs e)
        {
            IActionControl actionControl = (IActionControl)sender;
            actionControl.NativeControlDisposed -= SingleChoiceControlOnNativeControlDisposed;
            if (actionBindings != null && actionBindings.Count > 0)
            {
                for (int i = actionBindings.Count - 1; i >= 0; i--)
                {
                    if (actionBindings[i].ActionControl == actionControl)
                        actionBindings[i].Dispose();
                }
            }
        }
        private void FilteringCriterionAction_Execute(
            object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var filterKey = e.Action.Id;
            ((ListView)View).CollectionSource.BeginUpdateCriteria();
            if (((ListView)View).CollectionSource.Criteria.ContainsKey(filterKey))
                ((ListView)View).CollectionSource.Criteria.Remove(filterKey);
            try
            {
                ((ListView)View).CollectionSource.Criteria[filterKey] =
                    CriteriaEditorHelper.GetCriteriaOperator(
                        e.SelectedChoiceActionItem.Data as string, View.ObjectTypeInfo.Type, ObjectSpace);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                //throw;
            }

            ((ListView)View).CollectionSource.EndUpdateCriteria();
            SavedChoiceActionItem(filterKey).ToolTip = e.SelectedChoiceActionItem.Id;
        }

        private IModelChoiceActionItem SavedChoiceActionItem(string key)
        {
            if (savedChoiceActionItems == null)
            {
                savedChoiceActionItems = new ConcurrentDictionary<string, IModelChoiceActionItem>();
            }
            //Mượn nhờ ActionSetFilter để lưu giá trị
            string actionSaveId = "SetFilter";
            var result = Application.Model.ActionDesign.Actions[actionSaveId]
                .ChoiceActionItems.GetNode(key) as IModelChoiceActionItem;
            if (result != null)
                return result;
            else
                return Application.Model.ActionDesign.Actions[actionSaveId]
                    .ChoiceActionItems.AddNode<IModelChoiceActionItem>(key);
        }

        private CriteriaOperator GetExcludeLinkedObjectsCriteria()
        {

            CriteriaOperator criteriaOperator = (CriteriaOperator)null;
            IMemberInfo memberInfo = ((PropertyCollectionSource)this.View.CollectionSource).MemberInfo;
            var test = ((XPObjectSpace)ObjectSpace).GetAssociatedCollectionCriteria(Tools.GetMasterObjectFromView(View), memberInfo);
            CriteriaOperator collectionCriteria = this.View.ObjectSpace.GetAssociatedCollectionCriteria(Tools.GetMasterObjectFromView(View), memberInfo);
            if ((object)collectionCriteria != null)
            {
                if (!memberInfo.IsManyToMany)
                    criteriaOperator = (CriteriaOperator)new GroupOperator(GroupOperatorType.Or, new CriteriaOperator[2]
                    {
                        (CriteriaOperator) new NullOperator(memberInfo.AssociatedMemberInfo.Name),
                        (CriteriaOperator) new NotOperator(collectionCriteria)
                    });
                else
                    criteriaOperator = (CriteriaOperator)new NotOperator(collectionCriteria);
            }
            return criteriaOperator;
        }

        protected virtual CriteriaOperator GetAssociatedCollectionCriteriaCore(
            object obj,
            IMemberInfo memberInfo)
        {
            if (!memberInfo.IsList || !(memberInfo.ListElementType != (Type)null) || memberInfo.AssociatedMemberInfo == null)
                return (CriteriaOperator)null;
            if (memberInfo.IsManyToMany)
                return (CriteriaOperator)new ContainsOperator(memberInfo.AssociatedMemberInfo.Name, (CriteriaOperator)new BinaryOperator(memberInfo.Owner.KeyMember.Name, ObjectSpace.GetKeyValue(obj)));
            return (CriteriaOperator)new BinaryOperator(memberInfo.AssociatedMemberInfo.Name + "." + memberInfo.Owner.KeyMember.Name, ObjectSpace.GetKeyValue(obj));
        }

    }

}
