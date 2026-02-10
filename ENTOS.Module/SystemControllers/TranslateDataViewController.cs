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

namespace ENTOS.Module.SystemControllers
{
    public partial class TranslateDataViewController : ViewController<ListView>
    {
              
        public TranslateDataViewController()
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

      
        private IDictionary<string, string> conditionsDictionary = null;
        private IObjectSpace _translateDataObjectSpace = null;
        private IObjectSpace GettranslateDataObjectSpace()
        {
            if (_translateDataObjectSpace is null)
                _translateDataObjectSpace = Application.CreateObjectSpace(typeof(Module.SystemObjects.TranslateData));
            return _translateDataObjectSpace;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (ActionTranslateData != null && ActionTranslateData.Items.Count == 0 &&
                View is ListView && View.ObjectTypeInfo != null && View.ObjectTypeInfo.Type.IsSubclassOf(typeof(PersistentBase)))
            {                
                var translateDatas = GettranslateDataObjectSpace().GetObjects<Module.SystemObjects.TranslateData>(CriteriaOperator.Parse(
                    "Active = True and ObjectType = ? and (IsNullOrEmpty(Trim(ViewId)) or ViewId = ?) ",
                    View.ObjectTypeInfo.Type, View.Id));
                if (translateDatas.Count > 0)
                {
                    var items = translateDatas.OrderBy(m => m.Name);
                    ActionTranslateData.Items.Clear();
                    if (conditionsDictionary == null)
                    {
                        conditionsDictionary = new Dictionary<string, string>();
                    }

                    foreach (var translateData in items)
                    {
                        if ((View.AllowEdit || View.Id.Equals(translateData.ViewId) || string.IsNullOrEmpty(translateData.ViewId) || translateData.AutoSave) && translateData.RootField != null &&
                            translateData.RootField.Value is string && !string.IsNullOrEmpty((string) translateData.RootField.Value) 
                            && translateData.TranslateField != null &&
                            translateData.TranslateField.Value is string && !string.IsNullOrEmpty((string)translateData.TranslateField.Value))
                        {                            
                            var member = View.ObjectTypeInfo.FindMember((string) translateData.RootField.Value);
                            if (member != null && !string.IsNullOrEmpty(translateData.Name))
                            {
                                ActionTranslateData.Items.Add(new ChoiceActionItem(translateData.Oid.ToString(),
                                           translateData.Name,
                                           member));
                            }
                        }
                    }
                }
            }
        }

        private void ActionTranslateData_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(!(e.SelectedChoiceActionItem.Data is IMemberInfo) || View is null)
                return;
            //if (View.ObjectTypeInfo != null && (e.SelectedChoiceActionItem.Caption.EndsWith(defaultText) || e.SelectedChoiceActionItem.Caption.EndsWith(setNullText) || e.SelectedChoiceActionItem.Caption.EndsWith(setNullAndSetDefaultText)))
            var translateData = GettranslateDataObjectSpace().FindObject<Module.SystemObjects.TranslateData>(CriteriaOperator.Parse("Oid =?", Guid.Parse(e.SelectedChoiceActionItem.Id)));
            if (translateData is null)
                return;
           
            if (View.ObjectTypeInfo != null)
            {
                var member = View.ObjectTypeInfo.FindMember(translateData.TranslateField.Value as string);
                if(member is null)
                {
                    Module.SystemObjects.Tools.ShowMessage(Application, "Lỗi", "Không tìm thấy trường dịch", InformationType.Error);
                    return;
                }
                string source = "en", destination = "vi";
                if (translateData.LanguageOrigine != null)
                {
                    var languageCode = translateData.LanguageOrigine.Value as string;
                    if (!string.IsNullOrEmpty(languageCode))
                        source = languageCode;
                }
                if (translateData.LanguageTranslate != null)
                {
                    var languageCode = translateData.LanguageTranslate.Value as string;
                    if (!string.IsNullOrEmpty(languageCode))
                        destination = languageCode;
                }


                foreach (var selectedObject in View.SelectedObjects)
                {
                    var rootText = Module.SystemObjects.Tools.GetPropertyValueInObject(selectedObject, translateData.RootField.Value as string) as string;
                    if (!string.IsNullOrEmpty((string)rootText))
                    {
                        
                        var translateText = translateData.SupportHtml ? Module.SystemObjects.Tools.TranslateHtml(rootText, destination, source) : Module.SystemObjects.Tools.TranslateText(rootText, destination, source);
                        if (!string.IsNullOrEmpty(translateText))
                            Module.SystemObjects.Tools.SetPropertyValueInObject(selectedObject, translateData.TranslateField.Value as string, translateText);
                    }
                }
               
                CallAutoSave();
                return;
            }
        }
        private void CallAutoSave()
        {
            try
            {
                if (ActionTranslateData.SelectedItem != null && ActionTranslateData.SelectedItem.Id != null)
                {
                    var translateData = GettranslateDataObjectSpace().GetObjectByKey<Module.SystemObjects.TranslateData>(Guid.Parse(ActionTranslateData.SelectedItem.Id));
                    if (translateData != null && translateData.AutoSave)
                    {
                        ObjectSpace.CommitChanges();
                    }
                }
            }
            catch (Exception)
            {

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
            this.ActionTranslateData = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // ActionTranslateData
            // 
            this.ActionTranslateData.Caption = "Dịch";
            this.ActionTranslateData.Category = "Edit";
            this.ActionTranslateData.ConfirmationMessage = null;
            this.ActionTranslateData.Id = "ActionTranslateData";
            this.ActionTranslateData.ImageName = "TranslateData";
            this.ActionTranslateData.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ActionTranslateData.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.ActionTranslateData.TargetObjectsCriteria = "";
            this.ActionTranslateData.TargetViewId = "";
            this.ActionTranslateData.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ActionTranslateData.ToolTip = null;
            this.ActionTranslateData.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ActionTranslateData.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ActionTranslateData_Execute);
            // 
            // PopupControlEditMultiViewController
            // 
            this.Actions.Add(this.ActionTranslateData);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction ActionTranslateData;
    }   
}