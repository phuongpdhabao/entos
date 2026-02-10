namespace ENTOS.Module.Controllers
{
    partial class ParameterViewController
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
			// CloneParameter
            this.CloneParameter = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CloneParameter
            // 
            this.CloneParameter.Caption = "Nạp tham số";
            this.CloneParameter.Category = "Edit";
            this.CloneParameter.ConfirmationMessage = null;
            this.CloneParameter.Id = "CloneParameter";
            this.CloneParameter.ImageName = "Action_CloneParameter";
            this.CloneParameter.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.CloneParameter.TargetViewId = "Parameter_User_ListView";  
            this.CloneParameter.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.CloneParameter.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.CloneParameter.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.CloneParameter.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CloneParameter_Execute);
            // 
            // ParameterViewController
            // 
            this.Actions.Add(this.CloneParameter);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction CloneParameter;
    }
}