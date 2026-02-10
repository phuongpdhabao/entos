using System;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace ENTOS.Module.SystemControllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class BaseObjectViewController : ViewController
    {
        public BaseObjectViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(BaseObject);
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void ActionDisplayDeleted_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            DialogController dc = Application.CreateController<DialogController>();
            dc.AcceptAction.Caption = "Phục hồi";
            dc.Accepting += delegate (object o, DialogControllerAcceptingEventArgs args)
            {
                if (args.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    foreach (BaseObject baseObject in args.AcceptActionArgs.SelectedObjects)
                    {
                        baseObject.SetMemberValue("GCRecord", null);
                    }
                    var action = o as SimpleAction;
                    if (action != null && action.Controller != null && action.Controller.Frame != null && action.Controller.Frame.View != null && action.Controller.Frame.View.ObjectSpace != null)
                    {
                        action.Controller.Frame.View.ObjectSpace.CommitChanges();
                    }
                    //objectSpace.CommitChanges();
                    View.RefreshDataSource();
                }
            };
            //dc.Actions
            dc.CancelAction.Active.SetItemValue("", false);
            //dc.CancelAction.Caption = "Xóa vĩnh viễn";
            //dc.CancelAction.ConfirmationMessage =
            //    "Hành động này sẽ không thể phục hổi. Bạn có chắc chắn muốn xóa đối tượng này từ cơ sở dữ liệu";
            //dc.CancelAction.Execute += delegate(object o, SimpleActionExecuteEventArgs args)
            //{

            //    if (args.SelectedObjects.Count > 0)
            //    {
            //        var sql = string.Format("delete {0} where Oid = '{1}'", View.ObjectTypeInfo.Type.Name,
            //            ((BaseObject) args.SelectedObjects[0]).Oid);

            //        ((DevExpress.ExpressApp.Xpo.XPObjectSpace)View.ObjectSpace).Session.ExecuteQuery(sql);
            //        XPClassInfo classInfo = ((DevExpress.ExpressApp.Xpo.XPObjectSpace)objectSpace).Session.GetClassInfo(typeof(XPCustomObject));
            //       if (classInfo != null)
            //        {

            //            var attribute = (DeferredDeletionAttribute)classInfo.GetAttributeInfo(typeof(DeferredDeletionAttribute));
            //            attribute.Enabled = false;                        
            //            foreach (var deleteObject in args.SelectedObjects)
            //            {
            //                ((BaseObject)deleteObject).Delete();
            //                ((BaseObject)deleteObject).Session.CommitTransaction();
            //            }
            //            View.ObjectSpace.Delete(args.SelectedObjects);
            //            View.ObjectSpace.CommitChanges();
            //            //var action = o as SimpleAction;
            //            //if (action != null && action.Controller != null && action.Controller.Frame != null && action.Controller.Frame.View != null && action.Controller.Frame.View.ObjectSpace != null)
            //            //{
            //            //    action.Controller.Frame.View.ObjectSpace.CommitChanges();
            //            //}
            //            attribute.Enabled = true;
            //        }
            //        //foreach (BaseObject baseObject in args.SelectedObjects)
            //        //{
            //        //    baseObject.SetMemberValue("GCRecord", null);
            //        //}
            //        //var action = o as SimpleAction;
            //        //if (action != null && action.Controller != null && action.Controller.Frame != null && action.Controller.Frame.View != null && action.Controller.Frame.View.ObjectSpace != null)
            //        //{
            //        //    action.Controller.Frame.View.ObjectSpace.CommitChanges();
            //        //}
            //        ////objectSpace.CommitChanges();
            //        //View.RefreshDataSource();
            //    }
            //};

            ShowViewParameters showViewParameters = new ShowViewParameters()
            {
                TargetWindow = TargetWindow.NewModalWindow,
                CreateAllControllers = true,
                Context = TemplateContext.LookupWindow
            };
            dc.WindowTemplateChanged += delegate (object o, EventArgs args)
            {
                if (o is Controller && ((Controller)o).Frame != null &&
                    ((Controller)o).Frame.Template is ILookupPopupFrameTemplate)
                {
                    ((ILookupPopupFrameTemplate)((Controller)o).Frame.Template).IsSearchEnabled = true;
                }
            };
            showViewParameters.Controllers.Add(dc);
            DeletedObjectsCollectionSource collectionSource = new DeletedObjectsCollectionSource(objectSpace, View.ObjectTypeInfo.Type, false);
            showViewParameters.CreatedView = Application.CreateListView(View.Id, collectionSource, true);


            Application.ShowViewStrategy.ShowView(showViewParameters,
                new ShowViewSource(Frame, dc.AcceptAction));
        }

        private void BtnRestoreObject_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.CurrentObject is BaseObject)
            {
                ((BaseObject)View.CurrentObject).SetMemberValue("GCRecord", null);
                View.ObjectSpace.CommitChanges();
                View.Refresh();
                MessageOptions options = new MessageOptions();
                options.Duration = 2000;
                options.Message = "Phục hồi thành công";
                options.Type = InformationType.Success;
                options.Web.Position = InformationPosition.Right;
                options.Win.Caption = "Success";
                options.Win.Type = WinMessageType.Flyout;
                Application.ShowViewStrategy.ShowMessage(options);
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
            this.BtnDisplayDeleted = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.BtnRestoreObject = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // BtnDisplayDeleted
            // 
            this.BtnDisplayDeleted.Caption = "Thùng rác";
            this.BtnDisplayDeleted.Category = "Tools";
            this.BtnDisplayDeleted.ConfirmationMessage = null;
            this.BtnDisplayDeleted.Id = "BtnDisplayDeleted";
            this.BtnDisplayDeleted.ImageName = "RecycleBin";
            this.BtnDisplayDeleted.TargetObjectsCriteria = "IsCurrentUserInRole(\'Administrators\')";
            this.BtnDisplayDeleted.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.BtnDisplayDeleted.ToolTip = null;
            this.BtnDisplayDeleted.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.BtnDisplayDeleted.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ActionDisplayDeleted_Execute);
            // 
            // BtnRestoreObject
            // 
            this.BtnRestoreObject.Caption = "Phục hồi";
            this.BtnRestoreObject.Category = "Tools";
            this.BtnRestoreObject.ConfirmationMessage = "Bạn có chắc chắn muốn phục hồi đối tượng này";
            this.BtnRestoreObject.Id = "BtnRestoreObject";
            this.BtnRestoreObject.ImageName = "Action_Reload";
            this.BtnRestoreObject.TargetObjectsCriteria = "GCRecord is not null";
            this.BtnRestoreObject.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.BtnRestoreObject.ToolTip = null;
            this.BtnRestoreObject.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.BtnRestoreObject.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BtnRestoreObject_Execute);
            // 
            // BaseObjectViewController
            // 
            this.Actions.Add(this.BtnDisplayDeleted);
            this.Actions.Add(this.BtnRestoreObject);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction BtnDisplayDeleted;
        private DevExpress.ExpressApp.Actions.SimpleAction BtnRestoreObject;
    }

    public class DeletedObjectsCollectionSource : CollectionSource
    {
        public DeletedObjectsCollectionSource(IObjectSpace objectSpace, Type objectType, bool showUndeletedObjects) : base(objectSpace, objectType)
        {
            if (!showUndeletedObjects)
            {
                Criteria["HideUndeleted"] = new UnaryOperator(UnaryOperatorType.Not, new UnaryOperator(UnaryOperatorType.IsNull, XPBaseObject.Fields.GCRecord));
            }
        }
        protected override object CreateCollection()
        {
            Session session = ((XPObjectSpace)ObjectSpace).Session;
            XPCollection collection = new XPCollection(PersistentCriteriaEvaluationBehavior.InTransaction, session, session.GetClassInfo(ObjectTypeInfo.Type), GetTotalCriteria(), true);
            return collection;
        }
    }
}
