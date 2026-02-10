﻿using System;
using DevExpress.ExpressApp;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Module.SystemControllers
{
    public partial class DisplayViewController : ViewController
    {

        public DisplayViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(IDisplay);
            TargetViewType = ViewType.DetailView;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (View != null)
            {
                View.CurrentObjectChanged += ViewOnCurrentObjectChanged;
                View.ObjectSpace.Reloaded +=ObjectSpaceOnReloaded;
                if (View != null && View.CurrentObject != null && View.ObjectSpace.IsNewObject(View.CurrentObject))
                {
                    SetDisplayForObject(GetCurrentDisplay());
                }
                                
            }
        }

        private void ObjectSpaceOnReloaded(object sender, EventArgs e)
        {
            
            SetDisplayForObject(GetCurrentDisplay());
        }

        private void ViewOnCurrentObjectChanged(object sender, EventArgs e)
        {
            SetDisplayForObject(GetCurrentDisplay());
        }


        private bool GetCurrentDisplay()
        {
            if (View != null && View.CurrentObject is IDisplay)
            {
                return ((IDisplay)View.CurrentObject).Display;
            }
            return false;
        }

        protected override void OnDeactivated()
        {
            if (View != null)
            {
                View.CurrentObjectChanged -= ViewOnCurrentObjectChanged;
            }
            base.OnDeactivated();
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }

        

        private void ActionViewMode_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            if (View != null && View.CurrentObject is IDisplay)
            {
                ((IDisplay)View.CurrentObject).Display = !((IDisplay)View.CurrentObject).Display;
                SetDisplayForObject(((IDisplay)View.CurrentObject).Display);
            }
        }

        private void SetDisplayForObject(bool display)
        {
            if (View != null && View.CurrentObject is IDisplay)
            {
                if (display)
                {
                    ActionViewMode.ImageName = "ActionUnDisplay";
                    ActionViewMode.Caption = "Ẩn bớt";
                }
                else
                {
                    ActionViewMode.ImageName = "ActionDisplay";                    
                    ActionViewMode.Caption = "Hiện đủ";
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
            this.ActionViewMode = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ActionViewMode
            // 
            this.ActionViewMode.Caption = "Hiện đủ";
            this.ActionViewMode.Category = "View";
            this.ActionViewMode.ConfirmationMessage = null;
            this.ActionViewMode.Id = "ActionViewMode";
            this.ActionViewMode.ImageName = "ActionDisplay";
            this.ActionViewMode.TargetViewId = "";
            this.ActionViewMode.ToolTip = null;
            this.ActionViewMode.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.ActionViewMode.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ActionViewMode_Execute);
            // 
            // TskPlanViewController
            // 
            this.Actions.Add(this.ActionViewMode);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ActionViewMode;
    }
}