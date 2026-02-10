namespace ENTOS.Module.Controllers
{
    partial class CorrectionOptionViewController
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
			// TermLocationCorrect
            this.TermLocationCorrect = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // TermLocationCorrect
            // 
            this.TermLocationCorrect.Caption = "Sửa thuật vị";
            this.TermLocationCorrect.Category = "Edit";
            this.TermLocationCorrect.ConfirmationMessage = null;
            this.TermLocationCorrect.Id = "TermLocationCorrect";
            this.TermLocationCorrect.ImageName = "Action_TermLocationCorrect";
            this.TermLocationCorrect.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.TermLocationCorrect.TargetObjectsCriteria = "TermLocationCorrection.TermLocation.Term is not null";  
            this.TermLocationCorrect.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.TermLocationCorrect.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.TermLocationCorrect.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.TermLocationCorrect.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TermLocationCorrect_Execute);
            // 
            // CorrectionOptionViewController
            // 
            this.Actions.Add(this.TermLocationCorrect);
			// TermCorrect
            this.TermCorrect = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // TermCorrect
            // 
            this.TermCorrect.Caption = "Sửa thuật ngữ";
            this.TermCorrect.Category = "Edit";
            this.TermCorrect.ConfirmationMessage = null;
            this.TermCorrect.Id = "TermCorrect";
            this.TermCorrect.ImageName = "Action_TermCorrect";
            this.TermCorrect.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.TermCorrect.TargetObjectsCriteria = "TermLocationCorrection.TermLocation.Term is not null";  
            this.TermCorrect.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.TermCorrect.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.TermCorrect.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.TermCorrect.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TermCorrect_Execute);
            // 
            // CorrectionOptionViewController
            // 
            this.Actions.Add(this.TermCorrect);
			// DeleteWord
            this.DeleteWord = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // DeleteWord
            // 
            this.DeleteWord.Caption = "Xóa từ vựng";
            this.DeleteWord.Category = "Edit";
            this.DeleteWord.ConfirmationMessage = null;
            this.DeleteWord.Id = "DeleteWord";
            this.DeleteWord.ImageName = "Action_DeleteWord";
            this.DeleteWord.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.DeleteWord.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.DeleteWord.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.DeleteWord.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.DeleteWord.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DeleteWord_Execute);
            // 
            // CorrectionOptionViewController
            // 
            this.Actions.Add(this.DeleteWord);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction TermCorrect;
		private DevExpress.ExpressApp.Actions.SimpleAction DeleteWord;
		private DevExpress.ExpressApp.Actions.SimpleAction TermLocationCorrect;
    }
}