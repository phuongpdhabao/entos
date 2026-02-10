namespace ENTOS.Module.Controllers
{
    partial class TranslateObjectViewController
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
			// ExportTranslateObject
            this.ExportTranslateObject = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExportTranslateObject
            // 
            this.ExportTranslateObject.Caption = "Xuất dịch";
            this.ExportTranslateObject.Category = "Edit";
            this.ExportTranslateObject.ConfirmationMessage = null;
            this.ExportTranslateObject.Id = "ExportTranslateObject";
            this.ExportTranslateObject.ImageName = "Action_ExportTranslateObject";
            this.ExportTranslateObject.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ExportTranslateObject.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ExportTranslateObject.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ExportTranslateObject.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.ExportTranslateObject.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportTranslateObject_Execute);
            // 
            // TranslateObjectViewController
            // 
            this.Actions.Add(this.ExportTranslateObject);
			// CheckLinkImage
            this.CheckLinkImage = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CheckLinkImage
            // 
            this.CheckLinkImage.Caption = "Tồn tại liên kết";
            this.CheckLinkImage.Category = "Edit";
            this.CheckLinkImage.ConfirmationMessage = null;
            this.CheckLinkImage.Id = "CheckLinkImage";
            this.CheckLinkImage.ImageName = "Action_CheckLinkImage";
            this.CheckLinkImage.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.CheckLinkImage.ToolTip = "Kiểm tra trong thẻ paragraph trường nội dung có tồn tại liên kết hoặc ảnh không?";  
            this.CheckLinkImage.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.CheckLinkImage.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.CheckLinkImage.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.CheckLinkImage.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CheckLinkImage_Execute);
            // 
            // TranslateObjectViewController
            // 
            this.Actions.Add(this.CheckLinkImage);
			// ImportTranslateObject
            this.ImportTranslateObject = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTranslateObjectProduct = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemImportTranslateObjectPost = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ImportTranslateObject
            // 
            this.ImportTranslateObject.Caption = "Nạp";
            this.ImportTranslateObject.Category = "Edit";
            this.ImportTranslateObject.ConfirmationMessage = null;
            this.ImportTranslateObject.Id = "ImportTranslateObject";
            this.ImportTranslateObject.ImageName = "Action_ImportTranslateObject";
            this.ImportTranslateObject.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ImportTranslateObject.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ImportTranslateObject.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ImportTranslateObject.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
			this.ImportTranslateObject.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemImportTranslateObjectProduct.Caption = "Sản phẩm";
            choiceActionItemImportTranslateObjectProduct.Data = "Product";
            choiceActionItemImportTranslateObjectProduct.Id = "Product";
            this.ImportTranslateObject.Items.Add(choiceActionItemImportTranslateObjectProduct);

            
            //
            //Root Choice
            choiceActionItemImportTranslateObjectPost.Caption = "Bài viết";
            choiceActionItemImportTranslateObjectPost.Data = "Post";
            choiceActionItemImportTranslateObjectPost.Id = "Post";
            this.ImportTranslateObject.Items.Add(choiceActionItemImportTranslateObjectPost);

            this.ImportTranslateObject.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ImportTranslateObject_Execute);
            // 
            // TranslateObjectViewController
            // 
            this.Actions.Add(this.ImportTranslateObject);
			// ImportObjectElement
            this.ImportObjectElement = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportObjectElement
            // 
            this.ImportObjectElement.Caption = "Nạp thành phần";
            this.ImportObjectElement.Category = "Edit";
            this.ImportObjectElement.ConfirmationMessage = null;
            this.ImportObjectElement.Id = "ImportObjectElement";
            this.ImportObjectElement.ImageName = "Action_ImportObjectElement";
            this.ImportObjectElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.ImportObjectElement.ToolTip = "Dựng trường cờ để kiểm tra có liên kết trong nội dung của node";  
            this.ImportObjectElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ImportObjectElement.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ImportObjectElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.ImportObjectElement.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportObjectElement_Execute);
            // 
            // TranslateObjectViewController
            // 
            this.Actions.Add(this.ImportObjectElement);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction ExportTranslateObject;
		private DevExpress.ExpressApp.Actions.SimpleAction CheckLinkImage;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ImportTranslateObject;
		private DevExpress.ExpressApp.Actions.SimpleAction ImportObjectElement;
    }
}