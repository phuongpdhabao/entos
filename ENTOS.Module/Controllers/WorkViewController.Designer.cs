namespace ENTOS.Module.Controllers
{
    partial class WorkViewController
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
			// WorkRelativeObject
            this.WorkRelativeObject = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // WorkRelativeObject
            // 
            this.WorkRelativeObject.Caption = "Đối tượng";
            this.WorkRelativeObject.Category = "Edit";
            this.WorkRelativeObject.ConfirmationMessage = null;
            this.WorkRelativeObject.Id = "WorkRelativeObject";
            this.WorkRelativeObject.ImageName = "Action_WorkRelativeObject";
            this.WorkRelativeObject.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.WorkRelativeObject.ToolTip = "Đối tượng liên quan";  
            this.WorkRelativeObject.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.WorkRelativeObject.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.WorkRelativeObject.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.WorkRelativeObject.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.WorkRelativeObject.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.WorkRelativeObject_Execute);
            // 
            // WorkViewController
            // 
            this.Actions.Add(this.WorkRelativeObject);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction WorkRelativeObject;
    }
}