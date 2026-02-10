﻿using System;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Module.SystemControllers
{
    public partial class ShowModifiedViewController : ViewController
    {

        public ShowModifiedViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(PersistentBase);
            TargetViewType = ViewType.DetailView;
        }



        private void ActionShowModified_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {

                var typeDictionary = new System.Collections.Generic.Dictionary<string, int>();
                int delete = 0;
                int isNew = 0;
                foreach (var selectObject in View.ObjectSpace.ModifiedObjects)
                {
                    var type = selectObject.GetType();
                    if (!typeDictionary.ContainsKey(type.Name))
                    {
                        typeDictionary.Add(type.Name, 1);
                    }
                    else
                        typeDictionary[type.Name]++;
                    if (View.ObjectSpace.IsDeletedObject(selectObject))
                        delete++;
                    else if (View.ObjectSpace.IsNewObject(selectObject))
                        isNew++;
                }
                string message = "";
                if (typeDictionary.Count > 1)
                {
                    foreach (var key in typeDictionary.Keys)
                    {
                        if (!string.IsNullOrEmpty(message))
                            message += "\r\n";
                        message += key + ": " + typeDictionary[key];
                    }
                }
                if (isNew > 1)
                {
                    if (!string.IsNullOrEmpty(message))
                        message += "\r\n";
                    message += "Tạo mới: " + isNew;
                }
                if (delete > 1)
                {
                    if (!string.IsNullOrEmpty(message))
                        message += "\r\n";
                    message += "Xóa: " + delete;
                }
                Module.SystemObjects.Tools.ShowMessage(Application, View.ObjectSpace.ModifiedObjects.Count + " đang sửa", message, InformationType.Info, 20000);
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
            this.ActionShowModified = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ActionShowModified
            // 
            this.ActionShowModified.Caption = "Danh sách đang sửa";
            this.ActionShowModified.Category = "Tools";
            this.ActionShowModified.ConfirmationMessage = null;
            this.ActionShowModified.Id = "ActionShowModified";
            this.ActionShowModified.ImageName = "SectionBreaksList_EvenPage";
            this.ActionShowModified.TargetViewId = "";
            this.ActionShowModified.ToolTip = null;
            this.ActionShowModified.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.ActionShowModified.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ActionShowModified_Execute);
            // 
            // TskPlanViewController
            // 
            this.Actions.Add(this.ActionShowModified);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ActionShowModified;
    }
}