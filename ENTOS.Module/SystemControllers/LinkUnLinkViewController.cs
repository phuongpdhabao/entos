using System;
using System.Collections;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.Xpo;
using DevExpress.XtraGrid;
using ENTOS.Module.SystemObjects;
using ListView = DevExpress.ExpressApp.ListView;


namespace ENTOS.Module.SystemControllers
{
    public partial class LinkUnLinkViewController : ViewController
    {


        public LinkUnLinkViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(ILinkUnLink);
            TargetViewNesting = Nesting.Nested;
            TargetViewType = ViewType.ListView;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (View is ListView)
            {
                //var parent = View.ObjectSpace.Owner as DetailView;
                //View.CurrentObjectChanged +=ViewOnCurrentObjectChanged;
                View.SelectionChanged += ViewOnSelectionChanged;
            }

        }

        private void ViewOnSelectionChanged(object sender, EventArgs e)
        {
            if (View != null && View.SelectedObjects.Count > 0)
            {
                bool enable = true;
                foreach (var selectedObject in View.SelectedObjects)
                {
                    //if(View.ObjectSpace.IsObjectFitForCriteria())
                    var appearanceDisableDelete = selectedObject.GetPropertyValue("AppearanceDisableDelete");
                    if (appearanceDisableDelete != null && appearanceDisableDelete is bool)
                    {
                        if ((bool)appearanceDisableDelete == true)
                        {
                            enable = false;
                        }
                    }
                }
                ActionUnLinkObject.Enabled["AppearanceDisableDelete"] = enable;
            }
        }

        private void ViewOnCurrentObjectChanged(object sender, EventArgs e)
        {

        }


        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View is ListView)
            {
                var customLinkUnLinkAttributes = this.View.ObjectTypeInfo.FindAttributes<CustomLinkUnLinkAttribute>();
                foreach (var customLinkUnLinkAttribute in customLinkUnLinkAttributes)
                {
                    if (!string.IsNullOrEmpty(customLinkUnLinkAttribute.ViewId) && !View.Id.Equals(customLinkUnLinkAttribute.ViewId))
                        continue;
                    if (customLinkUnLinkAttribute.Type != null)
                    {
                        ActionLinkObject.Items.Add(new ChoiceActionItem(customLinkUnLinkAttribute.Type.FullName,
                            customLinkUnLinkAttribute.Name, customLinkUnLinkAttribute));
                    }

                }

                if (ActionLinkObject.Items.Count == 0)
                {
                    ActionUnLinkObject.Enabled["AppearanceDisableDelete"] = false;
                    ActionUnLinkObject.Active["AppearanceDisableDelete"] = false;
                }
            }
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private object MasterObject
        {
            get
            {
                if (View is ListView && ((ListView)View).CollectionSource is PropertyCollectionSource)
                {
                    return ((PropertyCollectionSource)((ListView)View).CollectionSource).MasterObject;
                }
                return null;
            }
        }
        private void ActionLinkObject_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            PopupActionLinkContact(e, true);
        }
        private void ActionUnLinkObject_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                View.ObjectSpace.Delete(View.SelectedObjects);
            }
        }
        private void PopupActionLinkContact(SingleChoiceActionExecuteEventArgs e, bool add = true)
        {
            if (View is ListView && ((ListView)View).CollectionSource is PropertyCollectionSource && e.SelectedChoiceActionItem != null)
            {
                if (MasterObject is null)
                    return;
                var customLinkUnLinkAttribute = ((CustomLinkUnLinkAttribute)e.SelectedChoiceActionItem.Data);
                string caption = e.SelectedChoiceActionItem.Caption.ToLower();
                using (DevExpress.ExpressApp.SystemModule.DialogController dc =
                        Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
                {
                    ShowViewParameters showViewParameters = new ShowViewParameters()
                    {
                        TargetWindow = TargetWindow.NewModalWindow,
                        CreateAllControllers = true,
                        Context = TemplateContext.LookupWindow,
                    };

                    showViewParameters.Controllers.Add(dc);
                    string viewId = Application.FindLookupListViewId(customLinkUnLinkAttribute.Type);
                    //string viewId = Application.FindListViewId(typeof(IpcLineItem));
                    if (!string.IsNullOrEmpty(viewId))
                    {
                        CollectionSourceBase collectionSource = Application.CreateCollectionSource(View.ObjectSpace,
                            customLinkUnLinkAttribute.Type, viewId, true, CollectionSourceMode.Normal);
                        CriteriaOperator parser = null;
                        if (customLinkUnLinkAttribute.Existed)
                        {
                            foreach (var obj in ((PropertyCollectionSource)((ListView)View).CollectionSource).List)
                            {
                                var currentObject = obj.GetPropertyValue(customLinkUnLinkAttribute.Field);
                                if (currentObject != null)
                                {
                                    var id = currentObject.GetPropertyValue("Oid");
                                    if (id != null)
                                    {
                                        parser = CriteriaOperator.And(parser,
                                            CriteriaOperator.Parse("Oid <> ?", id));
                                    }
                                }
                            }
                        }
                        if (customLinkUnLinkAttribute.Criteria != null)
                        {
                            var masterObjectId = MasterObject.GetPropertyValue("Oid");
                            parser = CriteriaOperator.And(parser,
                                CriteriaOperator.Parse(customLinkUnLinkAttribute.Criteria, masterObjectId,
                                    masterObjectId, masterObjectId, masterObjectId, masterObjectId));
                        }

                        if (!(parser is null))
                        {
                            collectionSource.BeginUpdateCriteria();
                            collectionSource.Criteria["FilterActionLink"] = parser;
                            collectionSource.EndUpdateCriteria();
                        }
                        if (collectionSource.GetCount() > 0)
                        {
                            var listview = Application.CreateListView(viewId, collectionSource, false);
                            if (add)
                            {
                                dc.AcceptAction.Caption = $"{Resources.CommonMessages.Import} {caption}";
                                dc.Accepting += AcceptActionOnExecute;
                            }
                            else
                            {
                                dc.AcceptAction.Caption = $"{Resources.CommonMessages.Choice} {caption}";
                                dc.Accepting += AcceptActionRemoveOnExecute;
                            }
                            dc.WindowTemplateChanged += delegate (object o, EventArgs args)
                            {
                                if (o is DevExpress.ExpressApp.Controller && ((DevExpress.ExpressApp.Controller)o).Frame != null &&
                                    ((DevExpress.ExpressApp.Controller)o).Frame.Template is ILookupPopupFrameTemplate)
                                {
                                    ((ILookupPopupFrameTemplate)((Controller)o).Frame.Template).IsSearchEnabled = true;
                                }
                            };
                            dc.Tag = customLinkUnLinkAttribute;
                            dc.SaveOnAccept = false;
                            //dc.Actions
                            dc.CancelAction.Active.SetItemValue("", false);
                            showViewParameters.CreatedView = listview;
                            Application.ShowViewStrategy.ShowView(showViewParameters,
                                new ShowViewSource(Frame, dc.AcceptAction));
                        }
                        else
                        {
                            var options = new MessageOptions()
                            {
                                Duration = 5000,
                                Message = string.Format("Không có {1} nào {0}", add ? "cần nạp" : "để chọn",
                                caption),
                                Type = InformationType.Info,

                            };
                            options.Web.Position = InformationPosition.Right;
                            options.Win.Caption = Module.Resources.CommonMessages.Message;
                            options.Win.Type = WinMessageType.Alert;


                            Application.ShowViewStrategy.ShowMessage(options);
                        }
                    }

                }

            }
        }

        private void AcceptActionOnExecute(object sender, DialogControllerAcceptingEventArgs e)
        {
            if (e.AcceptActionArgs.SelectedObjects.Count > 0 && View is ListView && ((ListView)View).CollectionSource is PropertyCollectionSource && sender is DevExpress.ExpressApp.SystemModule.DialogController)
            {
                var customLinkUnLinkAttribute = ((DevExpress.ExpressApp.SystemModule.DialogController)sender).Tag as CustomLinkUnLinkAttribute;
                if (customLinkUnLinkAttribute == null)
                    return;
                var member = View.ObjectTypeInfo.FindMember(customLinkUnLinkAttribute.Field);
                if (member == null)
                    return;
                if (customLinkUnLinkAttribute.InvertSelect)
                {
                    //Đảo ngược select để chọn những ưu tiên đầu
                    for (int i = e.AcceptActionArgs.SelectedObjects.Count - 1; i >= 0; i--)
                    {
                        var refObject = e.AcceptActionArgs.SelectedObjects[i];
                        if (!customLinkUnLinkAttribute.Existed || FindReferenceObject(refObject, member) == null)
                        {
                            var obj = View.ObjectSpace.CreateObject(View.ObjectTypeInfo.Type);
                            member.SetValue(obj, refObject);
                            ((PropertyCollectionSource)((ListView)View).CollectionSource).Add(obj);
                        }

                    }
                }
                else
                {
                    foreach (var refObject in e.AcceptActionArgs.SelectedObjects)
                    {
                        if (!customLinkUnLinkAttribute.Existed || FindReferenceObject(refObject, member) == null)
                        {
                            var obj = View.ObjectSpace.CreateObject(View.ObjectTypeInfo.Type);
                            member.SetValue(obj, refObject);
                            ((PropertyCollectionSource)((ListView)View).CollectionSource).Add(obj);
                        }

                    }
                }

            }
        }

        private object FindReferenceObject(object refObject, IMemberInfo memberInfo)
        {
            if (refObject == null)
                return null;
            var refObjectId = refObject.GetPropertyValue("Oid");
            if (refObjectId == null)
                return null;
            foreach (var obj in ((PropertyCollectionSource)((ListView)View).CollectionSource).List)
            {
                var currentObject = memberInfo.GetValue(obj);
                if (currentObject != null && refObjectId.Equals(currentObject.GetPropertyValue("Oid")))
                {
                    return obj;
                }
            }
            return null;
        }

        private void AcceptActionRemoveOnExecute(object sender, DialogControllerAcceptingEventArgs e)
        {
            if (e.AcceptActionArgs.SelectedObjects.Count > 0 && View is ListView && ((ListView)View).CollectionSource is PropertyCollectionSource)
            {

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
            this.ActionLinkObject = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ActionUnLinkObject = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ActionLinkObject
            // 
            this.ActionLinkObject.Caption = "Liên kết";
            this.ActionLinkObject.Category = "Edit";
            this.ActionLinkObject.ConfirmationMessage = null;
            this.ActionLinkObject.Id = "ActionLinkObject";
            this.ActionLinkObject.ImageName = "Action_LinkUnlink_Link";
            this.ActionLinkObject.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ActionLinkObject.TargetObjectsCriteria = "";
            this.ActionLinkObject.TargetViewId = "";
            this.ActionLinkObject.TargetViewNesting = DevExpress.ExpressApp.Nesting.Nested;
            this.ActionLinkObject.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ActionLinkObject.ToolTip = null;
            this.ActionLinkObject.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ActionLinkObject.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ActionLinkObject_Execute);
            // 
            // ActionUnLinkObject
            // 
            this.ActionUnLinkObject.Caption = "Hủy liên kết";
            this.ActionUnLinkObject.Category = "Edit";
            this.ActionUnLinkObject.ConfirmationMessage = null;
            this.ActionUnLinkObject.Id = "ActionUnLinkObject";
            this.ActionUnLinkObject.ImageName = "Action_LinkUnlink_UnLink";
            this.ActionUnLinkObject.TargetObjectsCriteria = "";
            this.ActionUnLinkObject.TargetViewId = "";
            this.ActionUnLinkObject.TargetViewNesting = DevExpress.ExpressApp.Nesting.Nested;
            this.ActionUnLinkObject.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ActionUnLinkObject.ToolTip = null;
            this.ActionUnLinkObject.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ActionUnLinkObject.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ActionUnLinkObject_Execute);
            // 
            // LinkUnLinkViewController
            // 
            this.Actions.Add(this.ActionLinkObject);
            this.Actions.Add(this.ActionUnLinkObject);

        }

        #endregion
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ActionLinkObject;
        private DevExpress.ExpressApp.Actions.SimpleAction ActionUnLinkObject;
    }
}