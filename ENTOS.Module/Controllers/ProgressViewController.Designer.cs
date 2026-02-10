namespace ENTOS.Module.Controllers
{
    partial class ProgressViewController
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
			// OpenReferenceProgress
            this.OpenReferenceProgress = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // OpenReferenceProgress
            // 
            this.OpenReferenceProgress.Caption = "Mở đối tượng";
            this.OpenReferenceProgress.Category = "Edit";
            this.OpenReferenceProgress.ConfirmationMessage = null;
            this.OpenReferenceProgress.Id = "OpenReferenceProgress";
            this.OpenReferenceProgress.ImageName = "Action_OpenReferenceProgress";
            this.OpenReferenceProgress.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.OpenReferenceProgress.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OpenReferenceProgress.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.OpenReferenceProgress.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.OpenReferenceProgress.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OpenReferenceProgress_Execute);
            // 
            // ProgressViewController
            // 
            this.Actions.Add(this.OpenReferenceProgress);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction OpenReferenceProgress;
    }
}