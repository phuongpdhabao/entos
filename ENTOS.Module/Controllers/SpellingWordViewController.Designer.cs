namespace ENTOS.Module.Controllers
{
    partial class SpellingWordViewController
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
			// DisplaySpelling
            this.DisplaySpelling = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // DisplaySpelling
            // 
            this.DisplaySpelling.Caption = "Hiển thị phiên âm";
            this.DisplaySpelling.Category = "Edit";
            this.DisplaySpelling.ConfirmationMessage = null;
            this.DisplaySpelling.Id = "DisplaySpelling";
            this.DisplaySpelling.ImageName = "Action_DisplaySpelling";
            this.DisplaySpelling.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.DisplaySpelling.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.DisplaySpelling.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.DisplaySpelling.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.DisplaySpelling.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DisplaySpelling_Execute);
            // 
            // SpellingWordViewController
            // 
            this.Actions.Add(this.DisplaySpelling);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction DisplaySpelling;
    }
}