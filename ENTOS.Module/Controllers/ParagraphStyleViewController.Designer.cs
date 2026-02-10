namespace ENTOS.Module.Controllers
{
    partial class ParagraphStyleViewController
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
			// AssignFont
            this.AssignFont = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AssignFont
            // 
            this.AssignFont.Caption = "Gán phông";
            this.AssignFont.Category = "Edit";
            this.AssignFont.ConfirmationMessage = null;
            this.AssignFont.Id = "AssignFont";
            this.AssignFont.ImageName = "Action_AssignFont";
            this.AssignFont.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.AssignFont.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.AssignFont.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.AssignFont.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.AssignFont.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AssignFont_Execute);
            // 
            // ParagraphStyleViewController
            // 
            this.Actions.Add(this.AssignFont);
			// AdjustName
            this.AdjustName = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AdjustName
            // 
            this.AdjustName.Caption = "Chỉnh tên";
            this.AdjustName.Category = "Edit";
            this.AdjustName.ConfirmationMessage = null;
            this.AdjustName.Id = "AdjustName";
            this.AdjustName.ImageName = "Action_AdjustName";
            this.AdjustName.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.AdjustName.TargetViewNesting = DevExpress.ExpressApp.Nesting.Nested;
            this.AdjustName.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.AdjustName.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.AdjustName.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AdjustName_Execute);
            // 
            // ParagraphStyleViewController
            // 
            this.Actions.Add(this.AdjustName);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction AssignFont;
		private DevExpress.ExpressApp.Actions.SimpleAction AdjustName;
    }
}