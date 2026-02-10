namespace ENTOS.Module.Controllers
{
    partial class ExtractionTemplateViewController
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
			// ExtractionJson
            this.ExtractionJson = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExtractionJson
            // 
            this.ExtractionJson.Caption = "Tạo Json";
            this.ExtractionJson.Category = "Edit";
            this.ExtractionJson.ConfirmationMessage = null;
            this.ExtractionJson.Id = "ExtractionJson";
            this.ExtractionJson.ImageName = "Action_ExtractionJson";
            this.ExtractionJson.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ExtractionJson.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ExtractionJson.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ExtractionJson.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.ExtractionJson.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExtractionJson_Execute);
            // 
            // ExtractionTemplateViewController
            // 
            this.Actions.Add(this.ExtractionJson);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction ExtractionJson;
    }
}