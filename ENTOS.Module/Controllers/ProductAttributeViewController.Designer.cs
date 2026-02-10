namespace ENTOS.Module.Controllers
{
    partial class ProductAttributeViewController
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
			// ProductAttributeImport
            this.ProductAttributeImport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ProductAttributeImport
            // 
            this.ProductAttributeImport.Caption = "Nạp thuộc tính";
            this.ProductAttributeImport.Category = "Edit";
            this.ProductAttributeImport.ConfirmationMessage = null;
            this.ProductAttributeImport.Id = "ProductAttributeImport";
            this.ProductAttributeImport.ImageName = "Action_ProductAttributeImport";
            this.ProductAttributeImport.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.ProductAttributeImport.TargetViewId = "Product_ProductAttributeList_ListView";  
            this.ProductAttributeImport.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ProductAttributeImport.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ProductAttributeImport.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.ProductAttributeImport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProductAttributeImport_Execute);
            // 
            // ProductAttributeViewController
            // 
            this.Actions.Add(this.ProductAttributeImport);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction ProductAttributeImport;
    }
}