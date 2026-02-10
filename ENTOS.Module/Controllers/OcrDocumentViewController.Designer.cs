namespace ENTOS.Module.Controllers
{
    partial class OcrDocumentViewController
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
			// OcrDocumentStructure
            this.OcrDocumentStructure = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // OcrDocumentStructure
            // 
            this.OcrDocumentStructure.Caption = "Nhận dạng cấu trúc";
            this.OcrDocumentStructure.Category = "Edit";
            this.OcrDocumentStructure.ConfirmationMessage = null;
            this.OcrDocumentStructure.Id = "OcrDocumentStructure";
            this.OcrDocumentStructure.ImageName = "Action_OcrDocumentStructure";
            this.OcrDocumentStructure.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.OcrDocumentStructure.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OcrDocumentStructure.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.OcrDocumentStructure.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.OcrDocumentStructure.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OcrDocumentStructure_Execute);
            // 
            // OcrDocumentViewController
            // 
            this.Actions.Add(this.OcrDocumentStructure);
			// OcrDocumentExtract
            this.OcrDocumentExtract = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // OcrDocumentExtract
            // 
            this.OcrDocumentExtract.Caption = "Trích thông tin";
            this.OcrDocumentExtract.Category = "Edit";
            this.OcrDocumentExtract.ConfirmationMessage = null;
            this.OcrDocumentExtract.Id = "OcrDocumentExtract";
            this.OcrDocumentExtract.ImageName = "Action_OcrDocumentExtract";
            this.OcrDocumentExtract.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.OcrDocumentExtract.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OcrDocumentExtract.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.OcrDocumentExtract.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.OcrDocumentExtract.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OcrDocumentExtract_Execute);
            // 
            // OcrDocumentViewController
            // 
            this.Actions.Add(this.OcrDocumentExtract);
			// OcrDocumentObject
            this.OcrDocumentObject = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOcrDocumentObjectOpen = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemOcrDocumentObjectCreate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // OcrDocumentObject
            // 
            this.OcrDocumentObject.Caption = "Đối tượng";
            this.OcrDocumentObject.Category = "Edit";
            this.OcrDocumentObject.ConfirmationMessage = null;
            this.OcrDocumentObject.Id = "OcrDocumentObject";
            this.OcrDocumentObject.ImageName = "Action_OcrDocumentObject";
            this.OcrDocumentObject.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.OcrDocumentObject.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OcrDocumentObject.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.OcrDocumentObject.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.OcrDocumentObject.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemOcrDocumentObjectCreate.Caption = "Tạo";
            choiceActionItemOcrDocumentObjectCreate.Data = "Create";
            choiceActionItemOcrDocumentObjectCreate.Id = "Create";
            this.OcrDocumentObject.Items.Add(choiceActionItemOcrDocumentObjectCreate);

            
            //
            //Root Choice
            choiceActionItemOcrDocumentObjectOpen.Caption = "Mở";
            choiceActionItemOcrDocumentObjectOpen.Data = "Open";
            choiceActionItemOcrDocumentObjectOpen.Id = "Open";
            this.OcrDocumentObject.Items.Add(choiceActionItemOcrDocumentObjectOpen);

            this.OcrDocumentObject.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.OcrDocumentObject_Execute);
            // 
            // OcrDocumentViewController
            // 
            this.Actions.Add(this.OcrDocumentObject);
			// OcrDocumentMarkdown
            this.OcrDocumentMarkdown = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // OcrDocumentMarkdown
            // 
            this.OcrDocumentMarkdown.Caption = "Hoán đổi markdown hiển thị";
            this.OcrDocumentMarkdown.Category = "Edit";
            this.OcrDocumentMarkdown.ConfirmationMessage = null;
            this.OcrDocumentMarkdown.Id = "OcrDocumentMarkdown";
            this.OcrDocumentMarkdown.ImageName = "Action_OcrDocumentMarkdown";
            this.OcrDocumentMarkdown.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.OcrDocumentMarkdown.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.OcrDocumentMarkdown.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.OcrDocumentMarkdown.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.OcrDocumentMarkdown.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OcrDocumentMarkdown_Execute);
            // 
            // OcrDocumentViewController
            // 
            this.Actions.Add(this.OcrDocumentMarkdown);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction OcrDocumentMarkdown;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction OcrDocumentObject;
		private DevExpress.ExpressApp.Actions.SimpleAction OcrDocumentExtract;
		private DevExpress.ExpressApp.Actions.SimpleAction OcrDocumentStructure;
    }
}