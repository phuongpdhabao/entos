using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Xpo;
using ListView = DevExpress.ExpressApp.ListView;
using Controller = DevExpress.ExpressApp.Controller;
using ENTOS.Module.SystemObjects;
using ENTOS.Module.BusinessObjects;

namespace ENTOS.Module.Controllers
{
    public partial class PopupControlEditMultiViewController : ViewController
    {

        public PopupControlEditMultiViewController()
        {
            InitializeComponent();
            //TargetViewType = ViewType.ListView;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.         
            base.OnDeactivated();
        }

        private const string setNullAndSetDefaultText = " (Xóa trắng + Nạp giá trị mặc định)";
        private const string setNullText = " (Xóa trắng)";
        private const string defaultText = " (Nạp giá trị mặc định)";
        private IDictionary<string, string> conditionsDictionary = null;
        private IObjectSpace _dataInputObjectSpace = null;
        private IObjectSpace GetDataInputObjectSpace()
        {
            if (_dataInputObjectSpace is null)
                _dataInputObjectSpace = Application.CreateObjectSpace(typeof(Module.SystemObjects.DataInput));
            return _dataInputObjectSpace;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (ActionPopupControlMultiEdit != null && ActionPopupControlMultiEdit.Items.Count == 0 && View.ObjectTypeInfo != null && View.ObjectTypeInfo.Type.IsSubclassOf(typeof(PersistentBase)))
            {
                var dataInputs = GetDataInputObjectSpace().GetObjects<Module.SystemObjects.DataInput>(CriteriaOperator.Parse(
                    "Active = True and Field is not null and ObjectType = ? and (IsNullOrEmpty(ViewId) or ViewId = ?) ",
                    View.ObjectTypeInfo.Type, View.Id));
                if (dataInputs.Count > 0)
                {
                    var items = dataInputs.OrderBy(m => m.Field.Name);
                    ActionPopupControlMultiEdit.Items.Clear();
                    if (conditionsDictionary == null)
                    {
                        conditionsDictionary = new Dictionary<string, string>();
                    }

                    foreach (var dataInput in items)
                    {
                        if (string.IsNullOrEmpty(dataInput.ViewId) && View is DetailView)
                            continue; //Trong DetailView không cho phép nhập liệu trống View
                        if ((View.AllowEdit || View.Id.Equals(dataInput.ViewId) || string.IsNullOrEmpty(dataInput.ViewId) || dataInput.AutoSave) && dataInput.Field != null &&
                            dataInput.Field.Value is string &&
                            !string.IsNullOrEmpty((string)dataInput.Field.Value))
                        {
                            var member = View.ObjectTypeInfo.FindMember((string)dataInput.Field.Value);
                            if (member != null)
                            {
                                if (string.IsNullOrEmpty(dataInput.Name))
                                {
                                    //Trường hợp chưa có tên thì tạo tên
                                    string displayName = string.IsNullOrEmpty(member.DisplayName)
                                    ? Module.Helpers.XafXpoHelper.GetCaptionFromField(View.ObjectTypeInfo.Type, member.Name)
                                    : member.DisplayName;
                                    if (dataInput.CallDefaultMethod || dataInput.SetNull)
                                    {
                                        if (dataInput.CallDefaultMethod && dataInput.SetNull)
                                        {
                                            displayName += setNullAndSetDefaultText;
                                        }
                                        else if (dataInput.CallDefaultMethod)
                                        {
                                            displayName += defaultText;
                                        }
                                        else if (dataInput.SetNull)
                                        {
                                            displayName += setNullText;
                                        }
                                        ActionPopupControlMultiEdit.Items.Add(
                                            new ChoiceActionItem(dataInput.Oid.ToString(), displayName,
                                                member));
                                    }
                                    else
                                    {
                                        ActionPopupControlMultiEdit.Items.Add(new ChoiceActionItem(dataInput.Oid.ToString(),
                                            displayName,
                                            member));
                                        if (!string.IsNullOrEmpty(dataInput.Condition))
                                        {
                                            conditionsDictionary.Add(dataInput.Oid.ToString(), dataInput.Condition);
                                        }
                                    }
                                }
                                else
                                {
                                    ActionPopupControlMultiEdit.Items.Add(new ChoiceActionItem(dataInput.Oid.ToString(),
                                            dataInput.Name,
                                            member));
                                    if (!string.IsNullOrEmpty(dataInput.Condition))
                                    {
                                        conditionsDictionary.Add(dataInput.Oid.ToString(), dataInput.Condition);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ActionPopupControlMultiEdit_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if (!(e.SelectedChoiceActionItem.Data is IMemberInfo) || View is null)
                return;
            //if (View.ObjectTypeInfo != null && (e.SelectedChoiceActionItem.Caption.EndsWith(defaultText) || e.SelectedChoiceActionItem.Caption.EndsWith(setNullText) || e.SelectedChoiceActionItem.Caption.EndsWith(setNullAndSetDefaultText)))
            var dataInput = GetDataInputObjectSpace().FindObject<Module.SystemObjects.DataInput>(CriteriaOperator.Parse("Oid =?", Guid.Parse(e.SelectedChoiceActionItem.Id)));
            if (dataInput is null)
                return;
            if (View.ObjectTypeInfo != null && (dataInput.SetNull || dataInput.CallDefaultMethod || !string.IsNullOrEmpty(dataInput.SourceCondition)))
            {
                //Trong trường hợp xóa trắng và gọi hàm mặc định                
                //if (e.SelectedChoiceActionItem.Caption.EndsWith(setNullText) ||
                //    e.SelectedChoiceActionItem.Caption.EndsWith(setNullAndSetDefaultText))
                if (dataInput.SetNull)
                {
                    if (ActionPopupControlMultiEdit.SelectedItem.Data is IMemberInfo)
                    {
                        var memberInfo = (IMemberInfo)ActionPopupControlMultiEdit.SelectedItem.Data;
                        foreach (var selectedObject in View.SelectedObjects)
                        {
                            memberInfo.SetValue(selectedObject, null);
                        }
                        CallAutoSave();
                    }
                }
                //if (e.SelectedChoiceActionItem.Caption.EndsWith(defaultText) ||
                //    e.SelectedChoiceActionItem.Caption.EndsWith(setNullAndSetDefaultText))
                if (dataInput.CallDefaultMethod)
                {
                    System.Reflection.MethodInfo theMethod = View.ObjectTypeInfo.Type.GetMethod("SetDefault" + ((IMemberInfo)e.SelectedChoiceActionItem.Data).Name);
                    if (theMethod != null)
                    {
                        foreach (var selectedObject in View.SelectedObjects)
                        {
                            if (theMethod.GetParameters().Length > 0)
                                theMethod.Invoke(selectedObject, new object[] { View });
                            else
                                theMethod.Invoke(selectedObject, null);
                        }
                        CallAutoSave();
                    }
                }
                else if (!string.IsNullOrEmpty(dataInput.SourceCondition) && View.CurrentObject is XPBaseObject)
                {
                    foreach (XPBaseObject selectedObject in View.SelectedObjects)
                    {
                        var result = selectedObject.Evaluate(dataInput.SourceCondition);
                        if (result != null)
                        {
                            //selectedObject.SetMemberValue(((IMemberInfo)e.SelectedChoiceActionItem.Data).Name, result);
                            Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, dataInput.Field.Value as string, result);
                        }
                    }
                    CallAutoSave();
                }
                return;
            }
            var dc = Application.CreateController<DialogController>();
            dc.SaveOnAccept = true;
            var showViewParameters = new ShowViewParameters
            {
                TargetWindow = TargetWindow.NewModalWindow,
                CreateAllControllers = true,
                NewWindowTarget = NewWindowTarget.Separate
            };
            dc.Accepting += DialogControllerOnAccepting;
            dc.WindowTemplateChanged += delegate (object o, EventArgs args)
            {
                if (o is Controller && ((Controller)o).Frame != null &&
                    ((Controller)o).Frame.Template is ILookupPopupFrameTemplate)
                {
                    ((ILookupPopupFrameTemplate)((Controller)o).Frame.Template).IsSearchEnabled = true;
                }
            };
            showViewParameters.Controllers.Add(dc);
            if (((IMemberInfo)e.SelectedChoiceActionItem.Data).MemberType.IsSubclassOf(typeof(DevExpress.Xpo.PersistentBase)))
            {
                string viewId = Application.FindLookupListViewId(((IMemberInfo)e.SelectedChoiceActionItem.Data).MemberType);
                if (!string.IsNullOrEmpty(viewId))
                {
                    var modelListView = Application.FindModelView(viewId) as DevExpress.ExpressApp.Model.IModelListView;
                    if (modelListView != null)
                    {
                        CollectionSourceBase collectionSource = Application.CreateCollectionSource(View.ObjectSpace,
                            ((IMemberInfo)e.SelectedChoiceActionItem.Data).MemberType, viewId,
                            modelListView.DataAccessMode, CollectionSourceMode.Normal);
                        if (conditionsDictionary != null && conditionsDictionary.ContainsKey(e.SelectedChoiceActionItem.Id))
                        {
                            collectionSource.BeginUpdateCriteria();
                            if (conditionsDictionary[e.SelectedChoiceActionItem.Id].Contains("?"))
                            {
                                var masterObject = Module.SystemObjects.Tools.GetMasterObjectFromView(View);
                                if (masterObject != null)
                                {
                                    var masterObjectId = masterObject.GetPropertyValue("Oid");
                                    if (masterObjectId != null)
                                    {
                                        collectionSource.Criteria["PopupControlEditMulti"] =
                                            CriteriaOperator.Parse(conditionsDictionary[e.SelectedChoiceActionItem.Id],
                                                masterObjectId, masterObjectId, masterObjectId, masterObjectId,
                                                masterObjectId);
                                    }
                                }
                            }
                            else
                            {
                                collectionSource.Criteria["PopupControlEditMulti"] = CriteriaOperator.Parse(conditionsDictionary[e.SelectedChoiceActionItem.Id]);
                            }

                            collectionSource.EndUpdateCriteria();

                        }
                        var listView = Application.CreateListView(viewId, collectionSource, false);
                        showViewParameters.CreatedView = listView;
                        showViewParameters.Context = TemplateContext.LookupWindow;
                        dc.SaveOnAccept = false;
                    }
                }
            }
            else
            {
                Module.SystemObjects.PopupControlText popupControl = new Module.SystemObjects.PopupControlText(((IMemberInfo)e.SelectedChoiceActionItem.Data).MemberType);
                showViewParameters.CreatedView = Application.CreateDetailView(Application.CreateObjectSpace(), popupControl, true);
                showViewParameters.Context = TemplateContext.PopupWindow;
            }

            Application.ShowViewStrategy.ShowView(showViewParameters,
                new ShowViewSource(Frame, dc.AcceptAction));
        }
        private void CallAutoSave()
        {
            try
            {
                if (ActionPopupControlMultiEdit.SelectedItem != null && ActionPopupControlMultiEdit.SelectedItem.Id != null)
                {
                    var dataInput = GetDataInputObjectSpace().GetObjectByKey<Module.SystemObjects.DataInput>(Guid.Parse(ActionPopupControlMultiEdit.SelectedItem.Id));
                    if (dataInput != null && dataInput.AutoSave)
                    {
                        ObjectSpace.CommitChanges();
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        private char[] splitChars = new char[] { ' ', ' ' };

        private void DialogControllerOnAccepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            if (!(ActionPopupControlMultiEdit.SelectedItem.Data is IMemberInfo))
                return;
            var memberInfo = (IMemberInfo)ActionPopupControlMultiEdit.SelectedItem.Data;
            if (e.AcceptActionArgs.CurrentObject is Module.SystemObjects.PopupControlText)
            {
                var popupControl = (Module.SystemObjects.PopupControlText)e.AcceptActionArgs.CurrentObject;
                if (View is ListView && View.SelectedObjects.Count > 0)
                {
                    foreach (var selectedObject in View.SelectedObjects)
                    {
                        if (!string.IsNullOrEmpty(popupControl.ReplaceText) || !string.IsNullOrEmpty(popupControl.OriginText))
                        {
                            //Thay thế nội dung
                            var newContent = popupControl.ReplaceText;
                            if (!string.IsNullOrEmpty(popupControl.OriginText))
                            {
                                newContent = memberInfo.GetValue(selectedObject) as string;
                                if (!string.IsNullOrEmpty(newContent))
                                {
                                    newContent = newContent.Replace(popupControl.OriginText, popupControl.ReplaceText);
                                }
                            }
                            Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, newContent);
                        }
                        if (!string.IsNullOrEmpty(popupControl.RemovePrefix))
                        {
                            //Xóa nội dung phía trước
                            var currentValue = selectedObject.GetPropertyValue(memberInfo.Name) as string;
                            if (!string.IsNullOrEmpty(currentValue) && currentValue.IndexOf(popupControl.RemovePrefix) >= 0)
                            {
                                var newValue = currentValue.Substring(currentValue.IndexOf(popupControl.RemovePrefix));
                                Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, newValue);
                            }
                        }
                        if (!string.IsNullOrEmpty(popupControl.RemoveSuffix))
                        {
                            //Xóa nội dung phía sau
                            var currentValue = selectedObject.GetPropertyValue(memberInfo.Name) as string;
                            if (!string.IsNullOrEmpty(currentValue) && currentValue.IndexOf(popupControl.RemoveSuffix) >= 0)
                            {
                                var newValue = currentValue.Substring(0, currentValue.IndexOf(popupControl.RemoveSuffix) + popupControl.RemoveSuffix.Length);
                                Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, newValue);
                            }
                        }
                        if (popupControl.UpperLowerText != UpperLowerText.None)
                        {
                            if (popupControl.UpperLowerText == UpperLowerText.Upper)
                            {
                                //Thêm đầu hoa                            
                                var currentValue = selectedObject.GetPropertyValue(memberInfo.Name) as string;
                                if (!string.IsNullOrEmpty(currentValue))
                                {
                                    //106 : - Đầu hoa: từ thường > Đầu hoa, từ toàn hoa giữ nguyên

                                    var nameArray = currentValue.Split(splitChars, System.StringSplitOptions.RemoveEmptyEntries);
                                    var result = "";
                                    foreach (var word in nameArray)
                                    {
                                        if (!string.IsNullOrEmpty(result)) result += " ";
                                        if (word.Equals(word.ToUpper())) result += word;
                                        else result += char.ToUpper(word[0]) + word.ToLower().Substring(1);
                                    }
                                    if (!result.Equals(currentValue))
                                        Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, result);
                                }
                            }
                            if (popupControl.UpperLowerText == UpperLowerText.UpperAll)
                            {
                                //Thêm đầu hoa                            
                                var currentValue = selectedObject.GetPropertyValue(memberInfo.Name) as string;
                                if (!string.IsNullOrEmpty(currentValue))
                                {
                                    //106 : - Toàn hoa: chuyển hoa toàn bộ
                                    var result = currentValue.ToUpper();
                                    if (!result.Equals(currentValue))
                                        Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, result);
                                }
                            }
                            if (popupControl.UpperLowerText == UpperLowerText.Lower)
                            {
                                //Thêm đầu hoa                            
                                var currentValue = selectedObject.GetPropertyValue(memberInfo.Name) as string;
                                if (!string.IsNullOrEmpty(currentValue))
                                {
                                    //106 : - Bỏ hoa: chuyền toàn bộ thành chữ thường trừ chữ cái đầu câu thành hoa
                                    var result = currentValue.Trim();
                                    if (!string.IsNullOrEmpty(result))
                                    {
                                        result = result[0] + result.ToLower().Substring(1);
                                    }
                                    if (!result.Equals(currentValue))

                                        Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, result);
                                }
                            }
                        }

                        if (popupControl.ConvertString != ConvertString.None)
                        {
                            var currentValue = selectedObject.GetPropertyValue(memberInfo.Name) as string;
                            string result = currentValue;
                            if (!string.IsNullOrEmpty(currentValue))
                            {
                                if (popupControl.ConvertString == ConvertString.Escape)
                                    result = System.Uri.EscapeDataString(currentValue);
                                else if (popupControl.ConvertString == ConvertString.Unescape)
                                    result = System.Uri.UnescapeDataString(currentValue);
                                else if (popupControl.ConvertString == ConvertString.RemoveUnicode)
                                    result = Module.Helpers.TextHelper.RemoveUnicode(currentValue);
                                else if (popupControl.ConvertString == ConvertString.RemoveSpecialCharacters)
                                    result = Module.Helpers.TextHelper.ReplaceSpecialCharacters(currentValue, null, "");
                                else if (popupControl.ConvertString == ConvertString.KeepNumber)
                                    result = string.Join("", result.ToCharArray().Where(c => c == '.' || c == ',' || Char.IsDigit(c)));

                            }
                            if (!result.Equals(currentValue))
                                Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, result);

                        }

                        if (!string.IsNullOrEmpty(popupControl.Prefix) || !string.IsNullOrEmpty(popupControl.Suffix))
                        {
                            //Thêm nội dung vào phía sau
                            var currentValue = selectedObject.GetPropertyValue(memberInfo.Name);
                            var newValue = string.Format("{0}{1}{2}", popupControl.Prefix, currentValue,
                                popupControl.Suffix);
                            Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, newValue);
                        }
                        else if (popupControl.Date != null)
                        {
                            //Thay thế ngày
                            Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, popupControl.Date);
                        }
                        else if (popupControl.DaysAdd != null)
                        {
                            //Thêm ngày
                            var currentValue = selectedObject.GetPropertyValue(memberInfo.Name);
                            if (currentValue is DateTime)
                            {
                                var newValue = Convert.ToDateTime(currentValue).AddDays(popupControl.DaysAdd.Value);
                                Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, newValue);
                            }
                        }
                        else if (popupControl.Number != null)
                        {
                            //Thay thế số
                            Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, popupControl.Number);
                        }
                        else if (popupControl.AddNumber != null)
                        {
                            //Thêm thế số
                            var currentValue = selectedObject.GetPropertyValue(memberInfo.Name);
                            if (currentValue != null)
                            {
                                var newValue = Convert.ToDecimal(currentValue) + popupControl.AddNumber;
                                Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, newValue);
                            }
                        }
                        else if (popupControl.Logic != null)
                        {
                            //Thay thế giá trị logic: true, false
                            Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, popupControl.Logic);
                        }
                        else if (popupControl.EnumObject != null)
                        {
                            //Thay thế giá trị enum
                            Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, popupControl.EnumObject.Value);
                        }
                        else if (popupControl.ObjectType != null || popupControl.TimeSpan != null || popupControl.AppearanceType == 5)
                        {
                            //Thay thế giá trị enum
                            memberInfo.SetValue(selectedObject, popupControl.ObjectType);
                            //Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, popupControl.ObjectType);
                        }
                        else if (popupControl.TimeSpan != null || popupControl.AppearanceType == 6)
                        {
                            //Thay thế giá trị enum
                            memberInfo.SetValue(selectedObject, popupControl.TimeSpan);
                            //Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, popupControl.ObjectType);
                        }
                        else if (popupControl.AppearanceType == 7)
                        {
                            //màu sắc
                            memberInfo.SetValue(selectedObject, popupControl.Color);
                        }
                    }
                    CallAutoSave();
                }
            }
            else if (e.AcceptActionArgs.CurrentObject != null && e.AcceptActionArgs.CurrentObject.GetType()
                         .IsSubclassOf(typeof(DevExpress.Xpo.PersistentBase)))
            {
                if (View is ListView && View.SelectedObjects.Count > 0)
                {
                    foreach (var selectedObject in View.SelectedObjects)
                    {
                        Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, memberInfo.Name, e.AcceptActionArgs.CurrentObject);
                    }
                    CallAutoSave();
                }
            }

        }
        /// <summary>
        /// Required designer variable.
        /// </summary>
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
            this.ActionPopupControlMultiEdit = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // ActionPopupControlMultiEdit
            // 
            this.ActionPopupControlMultiEdit.Caption = "Nhập liệu";
            this.ActionPopupControlMultiEdit.Category = "Edit";
            this.ActionPopupControlMultiEdit.ConfirmationMessage = null;
            this.ActionPopupControlMultiEdit.Id = "ActionPopupControlMultiEdit";
            this.ActionPopupControlMultiEdit.ImageName = "DataInput";
            this.ActionPopupControlMultiEdit.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ActionPopupControlMultiEdit.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.ActionPopupControlMultiEdit.TargetObjectsCriteria = "";
            this.ActionPopupControlMultiEdit.TargetViewId = "";
            //this.ActionPopupControlMultiEdit.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ActionPopupControlMultiEdit.ToolTip = null;
            this.ActionPopupControlMultiEdit.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ActionPopupControlMultiEdit.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ActionPopupControlMultiEdit_Execute);
            // 
            // PopupControlEditMultiViewController
            // 
            this.Actions.Add(this.ActionPopupControlMultiEdit);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction ActionPopupControlMultiEdit;
    }
}