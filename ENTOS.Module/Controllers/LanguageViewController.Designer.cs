namespace ENTOS.Module.Controllers
{
    partial class LanguageViewController
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
			// TranslateAllElement
            this.TranslateAllElement = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // TranslateAllElement
            // 
            this.TranslateAllElement.Caption = "Dịch toàn bộ";
            this.TranslateAllElement.Category = "Edit";
            this.TranslateAllElement.ConfirmationMessage = null;
            this.TranslateAllElement.Id = "TranslateAllElement";
            this.TranslateAllElement.ImageName = "Action_TranslateAllElement";
            this.TranslateAllElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.TranslateAllElement.TargetViewId = "Video_LanguageList_ListView";  
            this.TranslateAllElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.TranslateAllElement.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.TranslateAllElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.TranslateAllElement.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TranslateAllElement_Execute);
            // 
            // LanguageViewController
            // 
            this.Actions.Add(this.TranslateAllElement);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction TranslateAllElement;
    }
}