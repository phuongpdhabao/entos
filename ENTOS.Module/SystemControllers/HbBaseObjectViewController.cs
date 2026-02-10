﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Module.SystemControllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class HbBaseObjectViewController : ViewController
    {
        public HbBaseObjectViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(HbBaseObject);
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

        private void btnDisplayAuditTrail_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null && View is DetailView)
            {
                if (View.CurrentObject != null && View.CurrentObject is HbBaseObject)
                {
                    HbBaseObject baseObject = View.CurrentObject as HbBaseObject;
                    baseObject.DisplayAuditTrail = !baseObject.DisplayAuditTrail;
                    btnDisplayAuditTrail.Caption = baseObject.DisplayAuditTrail ? "Ẩn Log" : "Hiện Log";
                }
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
            this.btnDisplayAuditTrail = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // btnDisplayAuditTrail
            // 
            this.btnDisplayAuditTrail.Caption = "Log";
            this.btnDisplayAuditTrail.Category = "View";
            this.btnDisplayAuditTrail.ConfirmationMessage = null;
            this.btnDisplayAuditTrail.Id = "btnDisplayAuditTrail";
            this.btnDisplayAuditTrail.ImageName = "BO_Audit_ChangeHistory";
            this.btnDisplayAuditTrail.TargetObjectsCriteria = "IsCurrentUserInRole(\'Administrators\')";
            this.btnDisplayAuditTrail.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.btnDisplayAuditTrail.ToolTip = null;
            this.btnDisplayAuditTrail.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.btnDisplayAuditTrail.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnDisplayAuditTrail_Execute);
            // 
            // HbBaseObjectViewController
            // 
            this.Actions.Add(this.btnDisplayAuditTrail);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction btnDisplayAuditTrail;
    }
}
