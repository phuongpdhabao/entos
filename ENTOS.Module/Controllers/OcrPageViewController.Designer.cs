namespace ENTOS.Module.Controllers
{
    partial class OcrPageViewController
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
			// OcrPageImport
            this.OcrPageImport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // OcrPageImport
            // 
            this.OcrPageImport.Caption = "Nạp trang";
            this.OcrPageImport.Category = "Edit";
            this.OcrPageImport.ConfirmationMessage = null;
            this.OcrPageImport.Id = "OcrPageImport";
            this.OcrPageImport.ImageName = "Action_OcrPageImport";
            this.OcrPageImport.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.OcrPageImport.TargetViewId = "OcrDocument_OcrPageList_ListView";  
            this.OcrPageImport.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OcrPageImport.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.OcrPageImport.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.OcrPageImport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OcrPageImport_Execute);
            // 
            // OcrPageViewController
            // 
            this.Actions.Add(this.OcrPageImport);
			// OcrPageStructure
            this.OcrPageStructure = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // OcrPageStructure
            // 
            this.OcrPageStructure.Caption = "Nhận dạng cấu trúc";
            this.OcrPageStructure.Category = "Edit";
            this.OcrPageStructure.ConfirmationMessage = null;
            this.OcrPageStructure.Id = "OcrPageStructure";
            this.OcrPageStructure.ImageName = "Action_OcrPageStructure";
            this.OcrPageStructure.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.OcrPageStructure.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OcrPageStructure.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.OcrPageStructure.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.OcrPageStructure.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OcrPageStructure_Execute);
            // 
            // OcrPageViewController
            // 
            this.Actions.Add(this.OcrPageStructure);
			// OcrPageExtract
            this.OcrPageExtract = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // OcrPageExtract
            // 
            this.OcrPageExtract.Caption = "Trích thông tin";
            this.OcrPageExtract.Category = "Edit";
            this.OcrPageExtract.ConfirmationMessage = null;
            this.OcrPageExtract.Id = "OcrPageExtract";
            this.OcrPageExtract.ImageName = "Action_OcrPageExtract";
            this.OcrPageExtract.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.OcrPageExtract.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OcrPageExtract.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.OcrPageExtract.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.OcrPageExtract.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OcrPageExtract_Execute);
            // 
            // OcrPageViewController
            // 
            this.Actions.Add(this.OcrPageExtract);
			// OcrPageObject
            this.OcrPageObject = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOcrPageObjectOpen = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOcrPageObjectCreate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // OcrPageObject
            // 
            this.OcrPageObject.Caption = "Đối tượng";
            this.OcrPageObject.Category = "Edit";
            this.OcrPageObject.ConfirmationMessage = null;
            this.OcrPageObject.Id = "OcrPageObject";
            this.OcrPageObject.ImageName = "Action_OcrPageObject";
            this.OcrPageObject.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.OcrPageObject.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OcrPageObject.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.OcrPageObject.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.OcrPageObject.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemOcrPageObjectCreate.Caption = "Tạo";
            choiceActionItemOcrPageObjectCreate.Data = "Create";
            choiceActionItemOcrPageObjectCreate.Id = "Create";
            this.OcrPageObject.Items.Add(choiceActionItemOcrPageObjectCreate);

            
            //
            //Root Choice
            choiceActionItemOcrPageObjectOpen.Caption = "Mở";
            choiceActionItemOcrPageObjectOpen.Data = "Open";
            choiceActionItemOcrPageObjectOpen.Id = "Open";
            this.OcrPageObject.Items.Add(choiceActionItemOcrPageObjectOpen);

            this.OcrPageObject.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.OcrPageObject_Execute);
            // 
            // OcrPageViewController
            // 
            this.Actions.Add(this.OcrPageObject);
			// OcrPageMarkdown
            this.OcrPageMarkdown = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // OcrPageMarkdown
            // 
            this.OcrPageMarkdown.Caption = "Hoán đổi markdown hiển thị";
            this.OcrPageMarkdown.Category = "Edit";
            this.OcrPageMarkdown.ConfirmationMessage = null;
            this.OcrPageMarkdown.Id = "OcrPageMarkdown";
            this.OcrPageMarkdown.ImageName = "Action_OcrPageMarkdown";
            this.OcrPageMarkdown.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.OcrPageMarkdown.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OcrPageMarkdown.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.OcrPageMarkdown.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.OcrPageMarkdown.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OcrPageMarkdown_Execute);
            // 
            // OcrPageViewController
            // 
            this.Actions.Add(this.OcrPageMarkdown);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction OcrPageObject;
		private DevExpress.ExpressApp.Actions.SimpleAction OcrPageImport;
		private DevExpress.ExpressApp.Actions.SimpleAction OcrPageStructure;
		private DevExpress.ExpressApp.Actions.SimpleAction OcrPageMarkdown;
		private DevExpress.ExpressApp.Actions.SimpleAction OcrPageExtract;
    }
}