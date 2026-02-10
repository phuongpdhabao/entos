using System;
using System.Collections;
using System.Collections.Generic;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.Xpo;
using ENTOS.Module.SystemObjects;
using ListView = DevExpress.ExpressApp.ListView;


namespace ENTOS.Module.SystemControllers
{
    public partial class EditCollectionInListViewController : ViewController
    {

        public EditCollectionInListViewController()
        {

            InitializeComponent();
            TargetViewType = ViewType.ListView;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (View is ListView && ((ListView)View).CollectionSource != null &&
                ((ListView)View).CollectionSource.IsLoaded)
            {
                ((ListView)View).CollectionSource.CollectionChanged += new EventHandler(this.CollectionSource_CollectionChanged);
                this.UpdateActionState();
            }
        }

        protected override void OnDeactivated()
        {
            if (View is ListView && ((ListView)View).CollectionSource != null)
            {
                ((ListView)View).CollectionSource.CollectionChanged -= new EventHandler(this.CollectionSource_CollectionChanged);
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        private void CollectionSource_CollectionChanged(object sender, EventArgs e)
        {
            this.UpdateActionState();
        }


        private PropertyCollectionSource GetPropertyCollectionSource()
        {
            if (this.View == null || !(this.View is ListView))
                return (PropertyCollectionSource)null;
            if (this.View == null)
                return (PropertyCollectionSource)null;
            return ((ListView)this.View).CollectionSource as PropertyCollectionSource;
        }

        protected virtual void UpdateActionState()
        {
            if (this.View == null || !enableAction)
                return;
            //this.ActionPasteFromClipboard.Active.SetItemValue("IsRootDetailView", !(this.View is DetailView) || this.View.IsRoot);
            //this.ActionPopupEditInCollection.Active.SetItemValue("AllowEdit", (bool)this.View.AllowEdit);
            string diagnosticInfo = "";
            //this.ActionPopupEditInCollection.Active.SetItemValue("AllowEdit", DataManipulationRight.IsAddToCollectionAllowed(this.View, this.View.ObjectTypeInfo.Type, out diagnosticInfo));
            bool flag = true;
            PropertyCollectionSource collectionSource = this.GetPropertyCollectionSource();
            if (collectionSource != null)
            {
                if (!DataManipulationRight.CanEdit(this.View.ObjectTypeInfo.Type, "", (object)null, (CollectionSourceBase)collectionSource, this.View.ObjectSpace))
                {
                    flag = false;
                    diagnosticInfo = diagnosticInfo + Environment.NewLine + string.Format("No access to edit {0}", (object)this.View.ObjectTypeInfo);
                }
                if (flag && !DataManipulationRight.CanEdit(collectionSource.MemberInfo.Owner.Type, collectionSource.MemberInfo.Name, collectionSource.MasterObject, (CollectionSourceBase)null, collectionSource.ObjectSpace))
                {
                    flag = false;
                    diagnosticInfo = diagnosticInfo + Environment.NewLine + string.Format("No access to modify the {0} member of the {1}", (object)collectionSource.MemberInfo.Name, (object)collectionSource.MemberInfo.Owner.Type);
                }
                if (flag)
                {
                    IMemberInfo associatedMemberInfo = collectionSource.MemberInfo.AssociatedMemberInfo;
                    if (associatedMemberInfo != null)
                    {
                        if (this.View.CurrentObject != null && this.ObjectSpace.TypesInfo.FindTypeInfo(this.View.CurrentObject.GetType()).FindMember(associatedMemberInfo.Name) != null && !DataManipulationRight.CanEdit(associatedMemberInfo.Owner.Type, associatedMemberInfo.Name, this.View.CurrentObject, (CollectionSourceBase)collectionSource, this.View.ObjectSpace))
                        {
                            flag = false;
                            diagnosticInfo = diagnosticInfo + Environment.NewLine + string.Format("No access to modify the {0} member of the {1}", (object)associatedMemberInfo.Name, (object)associatedMemberInfo.Owner.Type);
                        }
                    }
                }
                this.ActionPopupEditInCollection.Active.SetItemValue("SecurityAllowEditByPermissions", flag);
            }
            this.ActionPopupEditInCollection.DiagnosticInfo = diagnosticInfo.ToString();
        }

        //private ChangeCollectionInListViewAttribute changeCollectionInListViewAttribute = null;
        private bool enableAction = false;


        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View != null && View.ObjectTypeInfo.Type.IsSubclassOf(typeof(PersistentBase)))
            {
                var objectSpace = Application.CreateObjectSpace(typeof(EditCollectionInList));
                var editCollectionInLists = objectSpace.GetObjects<EditCollectionInList>(CriteriaOperator.Parse(
                    "Active = True and ObjectType = ? and (IsNullOrEmpty(ViewId) or ViewId = ?) ",
                    View.ObjectTypeInfo.Type, View.Id));
                if (editCollectionInLists != null && editCollectionInLists.Count > 0)
                {
                    enableAction = true;
                    var linkChoice = new ChoiceActionItem("Link", Module.Resources.CommonMessages.MsgLink, "Link")
                    { ImageName = "Action_LinkUnlink_Link" };
                    var unlinkChoice = new ChoiceActionItem("UnLink", Module.Resources.CommonMessages.MsgUnlink, "UnLink")
                    { ImageName = "Action_LinkUnlink_UnLink" };
                    ActionPopupEditInCollection.Items.Add(linkChoice);
                    ActionPopupEditInCollection.Items.Add(unlinkChoice);

                    if (editCollectionInLists.Count == 1)
                    {
                        var memeInfo = editCollectionInLists[0].GetFieldInfo();
                        if (memeInfo != null)
                        {
                            ActionPopupEditInCollection.Caption = CaptionHelper.GetMemberCaption(memeInfo);
                            if (memeInfo.ListElementType != null)
                            {
                                var image = Tools.GetTypeImage(memeInfo.ListElementType);
                                if (!string.IsNullOrEmpty(image))
                                    ActionPopupEditInCollection.ImageName = image;
                            }
                            else if (memeInfo.MemberType != null)
                            {
                                var image = Tools.GetTypeImage(memeInfo.MemberType);
                                if (!string.IsNullOrEmpty(image))
                                    ActionPopupEditInCollection.ImageName = image;
                                unlinkChoice.Active["AllowUnlink"] = false;
                            }
                            linkChoice.Data = editCollectionInLists[0];
                            unlinkChoice.Data = editCollectionInLists[0];
                        }
                    }
                    else
                    {
                        foreach (var editCollectionInList in editCollectionInLists)
                        {
                            var memeInfo = editCollectionInList.GetFieldInfo();
                            if (memeInfo != null)
                            {
                                var caption = CaptionHelper.GetMemberCaption(memeInfo);
                                var childLinkChoice = new ChoiceActionItem("Link" + editCollectionInList.Oid, caption,
                                    editCollectionInList);
                                var childUnLinkChoice = new ChoiceActionItem("UnLink" + editCollectionInList.Oid, caption,
                                    editCollectionInList);
                                if (memeInfo.ListElementType != null)
                                {
                                    var image = Tools.GetTypeImage(memeInfo.ListElementType);
                                    if (!string.IsNullOrEmpty(image))
                                    {
                                        childLinkChoice.ImageName = image;
                                        childUnLinkChoice.ImageName = image;
                                    }
                                }

                                linkChoice.Items.Add(childLinkChoice);
                                unlinkChoice.Items.Add(childUnLinkChoice);
                            }


                        }
                    }
                }
            }
            //if (View is ListView)
            //{

            //    if (View.AllowEdit)
            //    {
            //        changeCollectionInListViewAttribute = this.View.ObjectTypeInfo.FindAttribute<ChangeCollectionInListViewAttribute>();          
            //        if (changeCollectionInListViewAttribute != null)
            //        {
            //            if (!string.IsNullOrEmpty(changeCollectionInListViewAttribute.Context))
            //            {
            //                var views = changeCollectionInListViewAttribute.Context.Split(',');
            //                foreach (var viewid in views)
            //                {
            //                    if (View.Id.Equals(viewid))
            //                    {
            //                        enableAction = true;
            //                        break;
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                enableAction = true;
            //            }
            //            if (enableAction)
            //            {
            //                if (!string.IsNullOrEmpty(changeCollectionInListViewAttribute.Image))
            //                    ActionPopupEditInCollection.ImageName = changeCollectionInListViewAttribute.Image;
            //                if (!string.IsNullOrEmpty(changeCollectionInListViewAttribute.Caption))
            //                    ActionPopupEditInCollection.Caption = changeCollectionInListViewAttribute.Caption;
            //                ActionPopupEditInCollection.Items.Add(new ChoiceActionItem("Link", "Liên kết", "Link")
            //                    { ImageName = "Action_LinkUnlink_Link" });
            //                ActionPopupEditInCollection.Items.Add(new ChoiceActionItem("UnLink", "Hủy liên kết", "UnLink")
            //                    { ImageName = "Action_LinkUnlink_UnLink"});
            //            }
            //        }                    
            //    }                               
            //}
        }


        private void ActionPopupEditInCollection_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if (View == null || !(e.SelectedChoiceActionItem.Data is EditCollectionInList))
                return;
            var currentChoice = (EditCollectionInList)e.SelectedChoiceActionItem.Data;
            var fieldInfo = currentChoice.GetFieldInfo();
            if (fieldInfo is null)
                return;

            using (DevExpress.ExpressApp.SystemModule.DialogController dc =
                Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
            {
                bool isLink = e.SelectedChoiceActionItem.Id.StartsWith("Link");
                var type = currentChoice.FieldType;
                if (type is null)
                    return;
                if (!isLink && !fieldInfo.IsList)
                {
                    //Nếu là Unlink cấp 1 thì tương đương xóa
                    View.ObjectSpace.Delete(View.SelectedObjects);
                    return;
                }
                string viewId = Application.FindLookupListViewId(type);
                var objectSpace = Application.CreateObjectSpace();
                if (!string.IsNullOrEmpty(viewId))
                {
                    CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace,
                        type, viewId, CollectionSourceMode.Normal);
                    CriteriaOperator criteria = null;
                    if (isLink && !string.IsNullOrEmpty(currentChoice.Condition))
                    {
                        try
                        {
                            if (currentChoice.Condition.Contains("?"))
                            {
                                //Nạp tham số nếu có
                                var masterObject = Tools.GetMasterObjectFromView(View);
                                if (masterObject != null)
                                {
                                    var masterObjectOid = masterObject.GetPropertyValue("Oid");
                                    if (masterObjectOid != null)
                                    {
                                        criteria = CriteriaOperator.Parse(currentChoice.Condition,
                                            masterObjectOid, masterObjectOid, masterObjectOid, masterObjectOid,
                                            masterObjectOid);
                                    }
                                }
                            }
                            else if (fieldInfo.IsList && currentChoice.Condition.EndsWith("()"))
                            {
                                //Kiểm tra nếu Criteria là phương thức
                                var criteriaProperty = Tools.CallObjectMethod(View.CurrentObject,
                                    currentChoice.Condition.Replace("()", ""),
                                    new object[1] { View });
                                if (criteriaProperty is CriteriaOperator)
                                {
                                    criteria = (CriteriaOperator)criteriaProperty;
                                }
                                else if (criteriaProperty is string)
                                {
                                    criteria = CriteriaOperator.Parse((string)criteriaProperty);
                                }
                            }
                            else
                            {
                                criteria = CriteriaOperator.Parse(currentChoice.Condition);
                            }

                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                        }
                    }

                    if (fieldInfo.IsList)
                    {
                        var collectionObject = fieldInfo.GetValue(View.CurrentObject) as IEnumerable;
                        if (collectionObject != null)
                        {
                            foreach (var childObject in collectionObject)
                            {
                                ////Trong trường hợp link trực tiếp
                                //if (string.IsNullOrEmpty(changeCollectionInListViewAttribute.Property))
                                //{
                                var oid = childObject.GetPropertyValue("Oid");
                                if (oid != null)
                                {
                                    if (isLink)
                                        criteria = CriteriaOperator.And(criteria,
                                            CriteriaOperator.Parse("Oid <> ?", oid));
                                    else
                                        criteria = CriteriaOperator.Or(criteria,
                                            CriteriaOperator.Parse("Oid = ?", oid));
                                }
                                //}
                                //else
                                //{
                                //    //Trong trường hợp link là bản trung gian
                                //    var propertyValue =
                                //        childObject.GetPropertyValue(changeCollectionInListViewAttribute.Property) as
                                //            PersistentBase;
                                //    if (propertyValue != null)
                                //    {
                                //        var oid = propertyValue.GetPropertyValue("Oid");
                                //        if (oid != null)
                                //        {
                                //            if (isLink)
                                //                criteria = CriteriaOperator.And(criteria,
                                //                    CriteriaOperator.Parse("Oid <> ?", oid));
                                //            else
                                //                criteria = CriteriaOperator.Or(criteria,
                                //                    CriteriaOperator.Parse("Oid = ?", oid));
                                //        }
                                //    }
                                //}                            
                            }
                        }
                    }
                    if (!(criteria is null))
                    {
                        collectionSource.BeginUpdateCriteria();
                        collectionSource.Criteria["PopupEditCollection"] = criteria;
                        collectionSource.EndUpdateCriteria();
                    }

                    var listview = Application.CreateListView(viewId, collectionSource, true);
                    if (isLink)
                        dc.Accepting += LinkInCollectionDialogControllerOnAccepting;
                    else
                        dc.Accepting += UnlinkInCollectionDialogControllerOnAccepting;
                    dc.SaveOnAccept = false;
                    //dc.Actions
                    dc.CancelAction.Active.SetItemValue("", false);
                    dc.WindowTemplateChanged += delegate (object o, EventArgs args)
                    {
                        if (o is Controller && ((Controller)o).Frame != null &&
                            ((Controller)o).Frame.Template is ILookupPopupFrameTemplate)
                        {
                            ((ILookupPopupFrameTemplate)((Controller)o).Frame.Template).IsSearchEnabled = isLink;
                        }
                    };
                    ShowViewParameters showViewParameters = new ShowViewParameters()
                    {
                        TargetWindow = TargetWindow.NewModalWindow,
                        CreateAllControllers = true,
                        Context = TemplateContext.LookupWindow,
                        CreatedView = listview,
                    };
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters,
                        new ShowViewSource(Frame, dc.AcceptAction));

                }

            }
        }
        private void LinkInCollectionDialogControllerOnAccepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            if (View == null || !(ActionPopupEditInCollection.SelectedItem.Data is EditCollectionInList))
                return;
            var currentChoice = (EditCollectionInList)ActionPopupEditInCollection.SelectedItem.Data;
            var fieldInfo = currentChoice.GetFieldInfo();
            if (fieldInfo is null)
                return;
            if (fieldInfo.IsList)
            {
                //Nạp trong form tập con
                if (View.CurrentObject is PersistentBase && e.AcceptActionArgs.SelectedObjects.Count > 0 && e.AcceptActionArgs.SelectedObjects[0] is PersistentBase)
                {
                    var linkObjectType = e.AcceptActionArgs.SelectedObjects[0].GetType();
                    //DevExpress.Xpo.Metadata.XPClassInfo collectionObjectInfo = null;
                    //DevExpress.Xpo.Metadata.XPMemberInfo collectionMemberObjectInfo = null;
                    foreach (PersistentBase modifyObject in View.SelectedObjects)
                    {
                        var collectionObject = fieldInfo.GetValue(modifyObject) as XPBaseCollection;
                        if (collectionObject != null)
                        {
                            //if (collectionObjectInfo is null && !string.IsNullOrEmpty(changeCollectionInListViewAttribute.Property))
                            //{
                            //    collectionObjectInfo = collectionObject.GetObjectClassInfo();
                            //    collectionMemberObjectInfo = collectionObjectInfo.GetMember(changeCollectionInListViewAttribute.Property);
                            //}
                            foreach (PersistentBase selectedObject in e.AcceptActionArgs.SelectedObjects)
                            {
                                ////Trong trường hợp link trực tiếp
                                //if (string.IsNullOrEmpty(changeCollectionInListViewAttribute.Property))
                                //{
                                var oid = selectedObject.GetPropertyValue("Oid");
                                if (oid != null)
                                {
                                    var linkObject = ObjectSpace.GetObjectByKey(linkObjectType, oid);
                                    if (linkObject != null)
                                    {
                                        collectionObject.BaseAdd(linkObject);
                                        this.ObjectSpace.SetModified(modifyObject);
                                    }
                                }
                                //}
                                //else if (collectionObjectInfo != null && collectionMemberObjectInfo != null)
                                //{
                                //    //Trong trường hợp link là bản trung gian
                                //    //Đơi xử lý
                                //    var newObject =
                                //        ObjectSpace.CreateObject(collectionObjectInfo.ClassType) as PersistentBase;
                                //    if (newObject != null)
                                //    {
                                //        collectionObject.BaseAdd(newObject);
                                //        var oid = selectedObject.GetPropertyValue("Oid");
                                //        if (oid != null)
                                //        {
                                //            var linkObject = ObjectSpace.GetObjectByKey(linkObjectType, oid);
                                //            if (linkObject != null)
                                //            {
                                //                collectionMemberObjectInfo.SetValue(newObject, linkObject);
                                //                this.ObjectSpace.SetModified(modifyObject);
                                //            }
                                //        }

                                //    }
                                //}

                            }
                        }
                    }
                    if (currentChoice.AutoSave)
                    {
                        ObjectSpace.CommitChanges();
                    }
                    Tools.RefreshGridView(View);
                }
            }
            else
            {
                //Nạp trong form hiện tại
                if (e.AcceptActionArgs.SelectedObjects.Count > 0 &&
                    e.AcceptActionArgs.SelectedObjects[0] is PersistentBase)
                {
                    var linkObjectType = e.AcceptActionArgs.SelectedObjects[0].GetType();
                    foreach (PersistentBase selectedObject in e.AcceptActionArgs.SelectedObjects)
                    {
                        var oid = selectedObject.GetPropertyValue("Oid");
                        if (oid is null)
                        {
                            //Tìm Key của đối tượng
                            var objTypeInfo = XafTypesInfo.Instance.FindTypeInfo(selectedObject.GetType());
                            if (objTypeInfo != null && objTypeInfo.KeyMember != null)
                                oid = objTypeInfo.KeyMember.GetValue(selectedObject);
                        }
                        if (oid != null)
                        {
                            var linkObject = ObjectSpace.GetObjectByKey(linkObjectType, oid);
                            if (linkObject != null)
                            {
                                if (((string)currentChoice.Field?.Value).Contains("."))
                                {
                                    var fieldValueArray = ((string)currentChoice.Field.Value).Split('.', StringSplitOptions.RemoveEmptyEntries);
                                    var currentMember = XafTypesInfo.Instance.FindTypeInfo(currentChoice.ObjectType).FindMember(fieldValueArray[0]);
                                    if (currentMember != null && fieldValueArray.Length >= 2)
                                    {
                                        var childMember = currentMember.IsList ? currentMember.ListElementTypeInfo.FindMember(fieldValueArray[1]) :
                                                            currentMember.MemberTypeInfo.FindMember(fieldValueArray[1]);
                                        if (childMember != null)
                                        {

                                            if (fieldValueArray.Length >= 3)
                                            {
                                                //    return childMember.IsList ? childMember.ListElementTypeInfo.FindMember(fieldValueArray[2]) :
                                                //        childMember.MemberTypeInfo.FindMember(fieldValueArray[2]);
                                                Module.SystemObjects.Tools.ShowMessage(Application, "Lỗi", "Tính năng này chỉ hỗ trợ 2 cấp độ", InformationType.Error);
                                            }
                                            else
                                            {
                                                var objectRow = ObjectSpace.CreateObject(currentMember.ListElementType);
                                                foreach (PersistentBase modifyObject in View.SelectedObjects)
                                                {
                                                    var collectionObject = currentMember.GetValue(modifyObject) as XPBaseCollection;
                                                    if (collectionObject != null)
                                                    {
                                                        collectionObject.BaseAdd(objectRow);
                                                        fieldInfo.SetValue(objectRow, linkObject);
                                                        this.ObjectSpace.SetModified(modifyObject);
                                                    }
                                                }

                                                //((ListView)View).CollectionSource.Add(objectRow);                                                
                                                //fieldInfo.SetValue(objectRow, linkObject);

                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    //Đơn cấp
                                    var objectRow = ObjectSpace.CreateObject(View.ObjectTypeInfo.Type);
                                    ((ListView)View).CollectionSource.Add(objectRow);
                                    fieldInfo.SetValue(objectRow, linkObject);
                                }
                            }


                        }

                    }
                    if (currentChoice.AutoSave)
                    {
                        ObjectSpace.CommitChanges();
                    }
                    Tools.RefreshGridView(View);
                }
            }

        }

        private void UnlinkInCollectionDialogControllerOnAccepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            if (View == null || !(ActionPopupEditInCollection.SelectedItem.Data is EditCollectionInList))
                return;
            var currentChoice = (EditCollectionInList)ActionPopupEditInCollection.SelectedItem.Data;
            var fieldInfo = currentChoice.GetFieldInfo();
            if (View.CurrentObject == null || fieldInfo is null)
                return;
            if (View.CurrentObject is PersistentBase && e.AcceptActionArgs.SelectedObjects.Count > 0 &&
                e.AcceptActionArgs.SelectedObjects[0] is PersistentBase)
            {
                var linkObjectType = e.AcceptActionArgs.SelectedObjects[0].GetType();
                var deleteObject = new System.Collections.Generic.List<object>();
                foreach (PersistentBase modifyObject in View.SelectedObjects)
                {
                    var collectionObject = fieldInfo.GetValue(modifyObject) as XPBaseCollection;
                    if (collectionObject != null)
                    {
                        foreach (PersistentBase selectedObject in e.AcceptActionArgs.SelectedObjects)
                        {
                            var selectedObjectOid = selectedObject.GetPropertyValue("Oid");
                            if (selectedObjectOid != null)
                            {
                                //if (string.IsNullOrEmpty(changeCollectionInListViewAttribute.Property))
                                //{
                                //    //Trong trường hợp unlink trực tiếp
                                var linkObject = ObjectSpace.GetObjectByKey(linkObjectType, selectedObjectOid);
                                if (linkObject != null)
                                {
                                    collectionObject.BaseRemove(linkObject);
                                    this.ObjectSpace.SetModified(modifyObject);
                                }
                                //}
                                //else
                                //{
                                //    //Trong trường hợp unlink là bản trung gian cần xóa                                                                          
                                //    foreach (var refObject in collectionObject)
                                //    {
                                //        var propertyObject =
                                //            refObject.GetPropertyValue(changeCollectionInListViewAttribute.Property) as
                                //                PersistentBase;
                                //        if (propertyObject != null)
                                //        {
                                //            var propertyObjectOid = propertyObject.GetPropertyValue("Oid");
                                //            if (propertyObjectOid != null && propertyObjectOid.Equals(selectedObjectOid))
                                //            {
                                //                deleteObject.Add(refObject);
                                //                this.ObjectSpace.SetModified(modifyObject);
                                //            }
                                //        }
                                //    }

                                //}
                            }

                        }
                    }
                }
                if (deleteObject.Count > 0)
                    ObjectSpace.Delete(deleteObject);
                if (currentChoice.AutoSave)
                {
                    ObjectSpace.CommitChanges();
                }
                Tools.RefreshGridView(View);
            }
        }

        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ActionPopupEditInCollection = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // ActionPopupEditInCollection
            // 
            this.ActionPopupEditInCollection.Caption = "Popup";
            this.ActionPopupEditInCollection.Category = "Edit";
            this.ActionPopupEditInCollection.ConfirmationMessage = null;
            this.ActionPopupEditInCollection.Id = "ActionPopupEditInCollection";
            this.ActionPopupEditInCollection.ImageName = "Action_Copy";
            this.ActionPopupEditInCollection.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ActionPopupEditInCollection.TargetObjectsCriteria = "";
            this.ActionPopupEditInCollection.TargetViewId = "";
            this.ActionPopupEditInCollection.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ActionPopupEditInCollection.ToolTip = null;
            this.ActionPopupEditInCollection.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ActionPopupEditInCollection.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ActionPopupEditInCollection_Execute);
            // 
            // EditCollectionInListViewController
            // 
            this.Actions.Add(this.ActionPopupEditInCollection);

        }

        #endregion
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ActionPopupEditInCollection;
    }
}