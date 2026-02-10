namespace ENTOS.Module.Controllers
{
    partial class RecognitionPositionViewController
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
			// ObjectView
            this.ObjectView = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ObjectView
            // 
            this.ObjectView.Caption = "Xem đối tượng";
            this.ObjectView.Category = "Edit";
            this.ObjectView.ConfirmationMessage = null;
            this.ObjectView.Id = "ObjectView";
            this.ObjectView.ImageName = "Action_ObjectView";
            this.ObjectView.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ObjectView.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ObjectView.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ObjectView.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.ObjectView.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ObjectView_Execute);
            // 
            // RecognitionPositionViewController
            // 
            this.Actions.Add(this.ObjectView);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction ObjectView;
    }
}