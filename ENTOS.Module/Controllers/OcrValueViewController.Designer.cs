namespace ENTOS.Module.Controllers
{
    partial class OcrValueViewController
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
			// ViewValue
            this.ViewValue = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ViewValue
            // 
            this.ViewValue.Caption = "Xem giá trị";
            this.ViewValue.Category = "Edit";
            this.ViewValue.ConfirmationMessage = null;
            this.ViewValue.Id = "ViewValue";
            this.ViewValue.ImageName = "Action_ViewValue";
            this.ViewValue.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ViewValue.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ViewValue.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ViewValue.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.ViewValue.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewValue_Execute);
            // 
            // OcrValueViewController
            // 
            this.Actions.Add(this.ViewValue);
			// ValidationCheck
            this.ValidationCheck = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ValidationCheck
            // 
            this.ValidationCheck.Caption = "Kiểm tra hơp lệ";
            this.ValidationCheck.Category = "Edit";
            this.ValidationCheck.ConfirmationMessage = null;
            this.ValidationCheck.Id = "ValidationCheck";
            this.ValidationCheck.ImageName = "Action_ValidationCheck";
            this.ValidationCheck.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ValidationCheck.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ValidationCheck.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ValidationCheck.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.ValidationCheck.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ValidationCheck_Execute);
            // 
            // OcrValueViewController
            // 
            this.Actions.Add(this.ValidationCheck);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction ViewValue;
		private DevExpress.ExpressApp.Actions.SimpleAction ValidationCheck;
    }
}