namespace ENTOS.Module.Controllers
{
    partial class ProductViewController
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
			// ProductVariationImport
            this.ProductVariationImport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ProductVariationImport
            // 
            this.ProductVariationImport.Caption = "Nạp biến thể";
            this.ProductVariationImport.Category = "Edit";
            this.ProductVariationImport.ConfirmationMessage = null;
            this.ProductVariationImport.Id = "ProductVariationImport";
            this.ProductVariationImport.ImageName = "Action_ProductVariationImport";
            this.ProductVariationImport.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.ProductVariationImport.TargetViewId = "Product_ProductList_ListView";  
            this.ProductVariationImport.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ProductVariationImport.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ProductVariationImport.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.ProductVariationImport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProductVariationImport_Execute);
            // 
            // ProductViewController
            // 
            this.Actions.Add(this.ProductVariationImport);
			// CheckDomainShare
            this.CheckDomainShare = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CheckDomainShare
            // 
            this.CheckDomainShare.Caption = "Thị phần";
            this.CheckDomainShare.Category = "Edit";
            this.CheckDomainShare.ConfirmationMessage = null;
            this.CheckDomainShare.Id = "CheckDomainShare";
            this.CheckDomainShare.ImageName = "Action_CheckDomainShare";
            this.CheckDomainShare.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.CheckDomainShare.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.CheckDomainShare.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.CheckDomainShare.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.CheckDomainShare.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CheckDomainShare_Execute);
            // 
            // ProductViewController
            // 
            this.Actions.Add(this.CheckDomainShare);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction CheckDomainShare;
		private DevExpress.ExpressApp.Actions.SimpleAction ProductVariationImport;
    }
}