namespace ENTOS.Module.Controllers
{
    partial class ExtractionKeyViewController
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
			// ExtractionKeyImport
            this.ExtractionKeyImport = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemExtractionKeyImportTable2Object = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemExtractionKeyImportMainObject = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemExtractionKeyImportTableObject = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ExtractionKeyImport
            // 
            this.ExtractionKeyImport.Caption = "Nạp khóa";
            this.ExtractionKeyImport.Category = "Edit";
            this.ExtractionKeyImport.ConfirmationMessage = null;
            this.ExtractionKeyImport.Id = "ExtractionKeyImport";
            this.ExtractionKeyImport.ImageName = "Action_ExtractionKeyImport";
            this.ExtractionKeyImport.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ExtractionKeyImport.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ExtractionKeyImport.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ExtractionKeyImport.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
			this.ExtractionKeyImport.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemExtractionKeyImportMainObject.Caption = "Đối tượng chính";
            choiceActionItemExtractionKeyImportMainObject.Data = "MainObject";
            choiceActionItemExtractionKeyImportMainObject.Id = "MainObject";
            this.ExtractionKeyImport.Items.Add(choiceActionItemExtractionKeyImportMainObject);

            
            //
            //Root Choice
            choiceActionItemExtractionKeyImportTableObject.Caption = "Đối tượng bảng";
            choiceActionItemExtractionKeyImportTableObject.Data = "TableObject";
            choiceActionItemExtractionKeyImportTableObject.Id = "TableObject";
            this.ExtractionKeyImport.Items.Add(choiceActionItemExtractionKeyImportTableObject);

            
            //
            //Root Choice
            choiceActionItemExtractionKeyImportTable2Object.Caption = "Đối tượng bảng 2";
            choiceActionItemExtractionKeyImportTable2Object.Data = "Table2Object";
            choiceActionItemExtractionKeyImportTable2Object.Id = "Table2Object";
            this.ExtractionKeyImport.Items.Add(choiceActionItemExtractionKeyImportTable2Object);

            this.ExtractionKeyImport.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ExtractionKeyImport_Execute);
            // 
            // ExtractionKeyViewController
            // 
            this.Actions.Add(this.ExtractionKeyImport);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ExtractionKeyImport;
    }
}