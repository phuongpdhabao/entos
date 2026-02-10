namespace ENTOS.Module.Controllers
{
    partial class WorkDetailViewController
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
			// ImportWorkDetail
            this.ImportWorkDetail = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportWorkDetail
            // 
            this.ImportWorkDetail.Caption = "Nạp";
            this.ImportWorkDetail.Category = "Edit";
            this.ImportWorkDetail.ConfirmationMessage = null;
            this.ImportWorkDetail.Id = "ImportWorkDetail";
            this.ImportWorkDetail.ImageName = "Action_ImportWorkDetail";
            this.ImportWorkDetail.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.ImportWorkDetail.TargetViewId = "Work_WorkDetailList_ListView";  
            this.ImportWorkDetail.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ImportWorkDetail.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ImportWorkDetail.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.ImportWorkDetail.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportWorkDetail_Execute);
            // 
            // WorkDetailViewController
            // 
            this.Actions.Add(this.ImportWorkDetail);
			// WorkDetailRelativeObject
            this.WorkDetailRelativeObject = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // WorkDetailRelativeObject
            // 
            this.WorkDetailRelativeObject.Caption = "Đối tượng";
            this.WorkDetailRelativeObject.Category = "Edit";
            this.WorkDetailRelativeObject.ConfirmationMessage = null;
            this.WorkDetailRelativeObject.Id = "WorkDetailRelativeObject";
            this.WorkDetailRelativeObject.ImageName = "Action_WorkDetailRelativeObject";
            this.WorkDetailRelativeObject.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.WorkDetailRelativeObject.ToolTip = "Đối tượng liên quan";  
            this.WorkDetailRelativeObject.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.WorkDetailRelativeObject.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.WorkDetailRelativeObject.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.WorkDetailRelativeObject.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.WorkDetailRelativeObject.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.WorkDetailRelativeObject_Execute);
            // 
            // WorkDetailViewController
            // 
            this.Actions.Add(this.WorkDetailRelativeObject);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction ImportWorkDetail;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction WorkDetailRelativeObject;
    }
}