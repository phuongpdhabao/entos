namespace ENTOS.Module.Controllers
{
    partial class TermLocationCorrectionViewController
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
			// AutoCorrect
            this.AutoCorrect = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AutoCorrect
            // 
            this.AutoCorrect.Caption = "Sửa tự động";
            this.AutoCorrect.Category = "Edit";
            this.AutoCorrect.ConfirmationMessage = null;
            this.AutoCorrect.Id = "AutoCorrect";
            this.AutoCorrect.ImageName = "Action_AutoCorrect";
            this.AutoCorrect.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.AutoCorrect.ToolTip = "Sủa tự động khi chỉ có 1 trường hợp đúng chính tả";  
			
			this.AutoCorrect.TargetObjectsCriteria = "TermLocation.Term is not null";  
            this.AutoCorrect.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.AutoCorrect.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.AutoCorrect.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.AutoCorrect.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AutoCorrect_Execute);
            // 
            // TermLocationCorrectionViewController
            // 
            this.Actions.Add(this.AutoCorrect);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction AutoCorrect;
    }
}