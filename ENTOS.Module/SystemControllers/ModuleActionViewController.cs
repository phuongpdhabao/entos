using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.CloneObject;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Module.SystemControllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ModuleActionViewController : ViewController<ListView>
    //ObjectViewController<DetailView, DetailViewActionsObject>
    {
        //private DetailViewActionsObject currentObject;
        private LookupEditorNewObject _lookupEditorNew = LookupEditorNewObject.None;
        public ModuleActionViewController()
        {
            InitializeComponent();

        }


        protected override void OnActivated()
        {
            base.OnActivated();
            if (View.Id.EndsWith("_ListView"))
            {
                CloneObjectViewController cloneObjectController =
                Frame.GetController<CloneObjectViewController>();
                if (cloneObjectController != null)
                {
                    cloneObjectController.CloneObjectAction.Items.Clear();
                    ChoiceActionItem myItem =
                        new ChoiceActionItem(Application.Model.BOModel.GetClass(View.ObjectTypeInfo.Type).Caption,
                            View.ObjectTypeInfo.Type);
                    myItem.ImageName = Application.Model.BOModel.GetClass(View.ObjectTypeInfo.Type).ImageName;
                    cloneObjectController.CloneObjectAction.Items.Add(myItem);
                }
            }

            // Perform various tasks depending on the target View.
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            //currentObject = (DetailViewActionsObject)View.CurrentObject;
            // Access and customize the target View control.
            if (View.Id.EndsWith("_LookupListView"))
            {
                if (_lookupEditorNew == LookupEditorNewObject.None)
                {
                    var value = Tools.GetValue(View.ObjectSpace,
                        "LookupEditorNewObject");
                    Enum.TryParse(value, out _lookupEditorNew);
                }
                if (_lookupEditorNew == LookupEditorNewObject.Disable)
                {
                    View.AllowNew["Info.AllowNew"] = false;
                }
                else if (_lookupEditorNew == LookupEditorNewObject.Enable)
                {
                    View.AllowNew["Info.AllowNew"] = true;
                }
                else if (_lookupEditorNew == LookupEditorNewObject.Module)
                {
                    var parent = View.ObjectSpace.Owner as DetailView;
                    if (parent != null)
                    {
                        View.AllowNew["Info.AllowNew"] = Module.Helpers.ReflectionHelper.GetModuleName(parent.ObjectTypeInfo.Type) ==
                                                         Module.Helpers.ReflectionHelper.GetModuleName(View.ObjectTypeInfo.Type);
                    }
                }
            }

        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        private void updateRoleFromAD_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Type typeInfo = e.CurrentObject.GetType();
            if (typeInfo == typeof(PermissionPolicyRole))
            {
                IObjectSpace objSpace = Application.CreateObjectSpace(typeInfo);
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                GroupPrincipal qbeGroup = new GroupPrincipal(ctx);
                PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);
                var results = srch.FindAll();
                IList<string> groupList = new List<string>();
                foreach (var result in results)
                {
                    string groupDescriptionKey = Module.Helpers.ParameterHelper.GetValue(objSpace, Module.Helpers.ReflectionHelper.GetModuleName(this.GetType()),
                        "GroupDescriptionKey");
                    if (result.Description == groupDescriptionKey)
                    {
                        groupList.Add(result.Name);
                    }
                }
                if (groupList.Count > 0)
                {
                    foreach (var group in groupList)
                    {
                        string parse = string.Format("Name == '{0}'", group);
                        PermissionPolicyRole role =
                            objSpace.FindObject<PermissionPolicyRole>(CriteriaOperator.Parse(parse));
                        if (role == null)
                        {
                            role = objSpace.CreateObject<PermissionPolicyRole>();
                            role.Name = group;
                        }
                    }

                    objSpace.CommitChanges();
                    View.RefreshDataSource();
                }
            }

        }

        private void updateUserFromAD_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Type typeInfo = e.CurrentObject.GetType();
            if (typeInfo == typeof(PermissionPolicyUser))
            {
                PermissionPolicyUser permissionPolicyUser = (PermissionPolicyUser)e.CurrentObject;
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                var rolesList = new DevExpress.Xpo.XPCollection<PermissionPolicyRole>(permissionPolicyUser.Session);
                foreach (var role in rolesList)
                {
                    var group = GroupPrincipal.FindByIdentity(ctx, role.Name);
                    if (group != null)
                    {
                        var users = group.GetMembers(false);
                        foreach (var u in users)
                        {
                            if (u.GetType() == typeof(UserPrincipal))
                            {
                                UserPrincipal userPrincipal = (UserPrincipal)u;
                                if (userPrincipal.Enabled.HasValue)
                                {
                                    if (userPrincipal.Enabled.Value)
                                    {
                                        string username = System.Environment.UserDomainName + "\\" + u.SamAccountName;
                                        string parse = string.Format("UserName == '{0}'", username);
                                        PermissionPolicyUser user =
                                            permissionPolicyUser.Session.FindObject<PermissionPolicyUser>(CriteriaOperator.Parse(parse));
                                        if (user == null)
                                        {
                                            user = new PermissionPolicyUser(permissionPolicyUser.Session);
                                            user.UserName = username;
                                        }
                                        if (!user.Roles.Contains(role))
                                            user.Roles.Add(role);
                                        permissionPolicyUser.Session.CommitTransaction();
                                    }
                                }
                            }
                        }
                    }
                }

                View.RefreshDataSource();
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
            this.updateRoleFromAD = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.updateUserFromAD = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // updateRoleFromAD
            // 
            this.updateRoleFromAD.Caption = "Cập nhật Role từ AD";
            this.updateRoleFromAD.Category = "ObjectsCreation";
            this.updateRoleFromAD.ConfirmationMessage = "Chỉ có quyền administrators trong AD mới được thực thi hành động này. Các Role mớ" +
    "i từ AD hiện tại";
            this.updateRoleFromAD.Id = "638dda43-16f8-4103-b23b-757aa4d95ed1";
            this.updateRoleFromAD.ImageName = "Action_Reload";
            this.updateRoleFromAD.TargetObjectType = typeof(DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole);
            this.updateRoleFromAD.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.updateRoleFromAD.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.updateRoleFromAD.ToolTip = null;
            this.updateRoleFromAD.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.updateRoleFromAD.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.updateRoleFromAD_Execute);
            // 
            // updateUserFromAD
            // 
            this.updateUserFromAD.Caption = "Cập nhật User từ AD";
            this.updateUserFromAD.Category = "ObjectsCreation";
            this.updateUserFromAD.ConfirmationMessage = "Chỉ có quyền administrators trong AD mới được thực thi hành động này. Các User mớ" +
    "i từ AD hiện tại";
            this.updateUserFromAD.Id = "3e3e3fcc-ad9f-44f1-8f4b-6ac402051686";
            this.updateUserFromAD.ImageName = "Action_Reload";
            this.updateUserFromAD.TargetObjectType = typeof(DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser);
            this.updateUserFromAD.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.updateUserFromAD.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.updateUserFromAD.ToolTip = null;
            this.updateUserFromAD.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.updateUserFromAD_Execute);
            // 
            // ModuleActionViewController
            // 
            this.Actions.Add(this.updateRoleFromAD);
            this.Actions.Add(this.updateUserFromAD);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction updateRoleFromAD;
        private DevExpress.ExpressApp.Actions.SimpleAction updateUserFromAD;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public class AutoCreatableObjectAttribute : Attribute
    {
        private bool autoCreatable = true;
        private ViewEditMode viewEditMode = ViewEditMode.Edit;
        public AutoCreatableObjectAttribute()
        {
        }
        public AutoCreatableObjectAttribute(bool autoCreatable)
        {
            this.autoCreatable = autoCreatable;
        }
        public bool AutoCreatable
        {
            get { return autoCreatable; }
        }
        public ViewEditMode ViewEditMode
        {
            get { return viewEditMode; }
            set { viewEditMode = value; }
        }
    }
    public class AutoCreatableObjectController : ViewController<DetailView>
    {
        protected override void OnViewChanging(View view)
        {
            base.OnViewChanging(view);
            Active.SetItemValue("AutoCreatableObject", false);
            if (view != null && view is ObjectView)
            {
                AutoCreatableObjectAttribute attribute = ((ObjectView)view).ObjectTypeInfo.FindAttribute<AutoCreatableObjectAttribute>(true);
                if (attribute != null)
                {
                    Active.SetItemValue("AutoCreatableObject", true);
                }
            }
        }
    }
}