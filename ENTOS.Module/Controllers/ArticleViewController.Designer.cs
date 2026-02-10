namespace ENTOS.Module.Controllers
{
    partial class ArticleViewController
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
			// KnowledgeShare
            this.KnowledgeShare = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // KnowledgeShare
            // 
            this.KnowledgeShare.Caption = "Chia sẻ kiến thức";
            this.KnowledgeShare.Category = "Edit";
            this.KnowledgeShare.ConfirmationMessage = null;
            this.KnowledgeShare.Id = "KnowledgeShare";
            this.KnowledgeShare.ImageName = "Action_KnowledgeShare";
            this.KnowledgeShare.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.KnowledgeShare.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.KnowledgeShare.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.KnowledgeShare.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.KnowledgeShare.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.KnowledgeShare_Execute);
            // 
            // ArticleViewController
            // 
            this.Actions.Add(this.KnowledgeShare);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction KnowledgeShare;
    }
}