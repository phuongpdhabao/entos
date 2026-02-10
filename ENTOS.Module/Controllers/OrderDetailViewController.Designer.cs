namespace ENTOS.Module.Controllers
{
    partial class OrderDetailViewController
    {
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
			// ActionSplitQuantity
            this.ActionSplitQuantity = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ActionSplitQuantity
            // 
            this.ActionSplitQuantity.Caption = "Tách số lượng";
            this.ActionSplitQuantity.Category = "Edit";
            this.ActionSplitQuantity.ConfirmationMessage = null;
            this.ActionSplitQuantity.Id = "ActionSplitQuantity";
            this.ActionSplitQuantity.ImageName = "Action_ActionSplitQuantity";
            this.ActionSplitQuantity.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ActionSplitQuantity.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ActionSplitQuantity.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ActionSplitQuantity.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.ActionSplitQuantity.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ActionSplitQuantity_Execute);
            // 
            // OrderDetailViewController
            // 
            this.Actions.Add(this.ActionSplitQuantity);
			// FillIpcLineItem
            this.FillIpcLineItem = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // FillIpcLineItem
            // 
            this.FillIpcLineItem.Caption = "Nạp hàng hóa";
            this.FillIpcLineItem.Category = "Edit";
            this.FillIpcLineItem.ConfirmationMessage = null;
            this.FillIpcLineItem.Id = "FillIpcLineItem";
            this.FillIpcLineItem.TargetViewNesting = DevExpress.ExpressApp.Nesting.Nested;
            this.FillIpcLineItem.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.FillIpcLineItem.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.FillIpcLineItem.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FillIpcLineItem_Execute);
            // 
            // OrderDetailViewController
            // 
            this.Actions.Add(this.FillIpcLineItem);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction ActionSplitQuantity;
		private DevExpress.ExpressApp.Actions.SimpleAction FillIpcLineItem;
    }
}