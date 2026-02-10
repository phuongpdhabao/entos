namespace ENTOS.Module.Controllers
{
    partial class WorkflowViewController
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
			// WorkflowMermaid
            this.WorkflowMermaid = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // WorkflowMermaid
            // 
            this.WorkflowMermaid.Caption = "Tạo mã lưu đồ";
            this.WorkflowMermaid.Category = "Edit";
            this.WorkflowMermaid.ConfirmationMessage = null;
            this.WorkflowMermaid.Id = "WorkflowMermaid";
            this.WorkflowMermaid.ImageName = "Action_WorkflowMermaid";
            this.WorkflowMermaid.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.WorkflowMermaid.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.WorkflowMermaid.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.WorkflowMermaid.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.WorkflowMermaid.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.WorkflowMermaid_Execute);
            // 
            // WorkflowViewController
            // 
            this.Actions.Add(this.WorkflowMermaid);
			// WorkflowShare
            this.WorkflowShare = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // WorkflowShare
            // 
            this.WorkflowShare.Caption = "Chia sẻ lưu đồ";
            this.WorkflowShare.Category = "Edit";
            this.WorkflowShare.ConfirmationMessage = null;
            this.WorkflowShare.Id = "WorkflowShare";
            this.WorkflowShare.ImageName = "Action_WorkflowShare";
            this.WorkflowShare.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.WorkflowShare.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.WorkflowShare.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.WorkflowShare.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.WorkflowShare.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.WorkflowShare_Execute);
            // 
            // WorkflowViewController
            // 
            this.Actions.Add(this.WorkflowShare);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction WorkflowMermaid;
		private DevExpress.ExpressApp.Actions.SimpleAction WorkflowShare;
    }
}