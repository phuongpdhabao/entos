namespace ENTOS.Module.Controllers
{
    partial class ElementBatchViewController
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
			// MatchLineBatchElement
            this.MatchLineBatchElement = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMatchLineBatchElementTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMatchLineBatchElementSynchronize = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // MatchLineBatchElement
            // 
            this.MatchLineBatchElement.Caption = "Khớp dòng lô";
            this.MatchLineBatchElement.Category = "Edit";
            this.MatchLineBatchElement.ConfirmationMessage = null;
            this.MatchLineBatchElement.Id = "MatchLineBatchElement";
            this.MatchLineBatchElement.ImageName = "Action_MatchLineBatchElement";
            this.MatchLineBatchElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.MatchLineBatchElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.MatchLineBatchElement.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.MatchLineBatchElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.MatchLineBatchElement.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemMatchLineBatchElementTranslate.Caption = "Dịch ngược";
            choiceActionItemMatchLineBatchElementTranslate.Data = "Translate";
            choiceActionItemMatchLineBatchElementTranslate.Id = "Translate";
            this.MatchLineBatchElement.Items.Add(choiceActionItemMatchLineBatchElementTranslate);

            
            //
            //Root Choice
            choiceActionItemMatchLineBatchElementSynchronize.Caption = "Đồng bộ";
            choiceActionItemMatchLineBatchElementSynchronize.Data = "Synchronize";
            choiceActionItemMatchLineBatchElementSynchronize.Id = "Synchronize";
            this.MatchLineBatchElement.Items.Add(choiceActionItemMatchLineBatchElementSynchronize);

            this.MatchLineBatchElement.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.MatchLineBatchElement_Execute);
            // 
            // ElementBatchViewController
            // 
            this.Actions.Add(this.MatchLineBatchElement);
			// ElementBatchImport
            this.ElementBatchImport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ElementBatchImport
            // 
            this.ElementBatchImport.Caption = "Nạp lô";
            this.ElementBatchImport.Category = "Edit";
            this.ElementBatchImport.ConfirmationMessage = null;
            this.ElementBatchImport.Id = "ElementBatchImport";
            this.ElementBatchImport.ImageName = "Action_ElementBatchImport";
            this.ElementBatchImport.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ElementBatchImport.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ElementBatchImport.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ElementBatchImport.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.ElementBatchImport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ElementBatchImport_Execute);
            // 
            // ElementBatchViewController
            // 
            this.Actions.Add(this.ElementBatchImport);
			// BatchTranslateImportElement
            this.BatchTranslateImportElement = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // BatchTranslateImportElement
            // 
            this.BatchTranslateImportElement.Caption = "Nạp Dịch lô";
            this.BatchTranslateImportElement.Category = "Edit";
            this.BatchTranslateImportElement.ConfirmationMessage = null;
            this.BatchTranslateImportElement.Id = "BatchTranslateImportElement";
            this.BatchTranslateImportElement.ImageName = "Action_BatchTranslateImportElement";
            this.BatchTranslateImportElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.BatchTranslateImportElement.TargetViewId = "Video_ElementBatchList_ListView";  
            this.BatchTranslateImportElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.BatchTranslateImportElement.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.BatchTranslateImportElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.BatchTranslateImportElement.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BatchTranslateImportElement_Execute);
            // 
            // ElementBatchViewController
            // 
            this.Actions.Add(this.BatchTranslateImportElement);
			// BatchTranslateTranslationElement
            this.BatchTranslateTranslationElement = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // BatchTranslateTranslationElement
            // 
            this.BatchTranslateTranslationElement.Caption = "Dịch thuật lô";
            this.BatchTranslateTranslationElement.Category = "Edit";
            this.BatchTranslateTranslationElement.ConfirmationMessage = null;
            this.BatchTranslateTranslationElement.Id = "BatchTranslateTranslationElement";
            this.BatchTranslateTranslationElement.ImageName = "Action_BatchTranslateTranslationElement";
            this.BatchTranslateTranslationElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.BatchTranslateTranslationElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.BatchTranslateTranslationElement.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.BatchTranslateTranslationElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.BatchTranslateTranslationElement.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BatchTranslateTranslationElement_Execute);
            // 
            // ElementBatchViewController
            // 
            this.Actions.Add(this.BatchTranslateTranslationElement);
			// BatchLanguageTranslateElement
            this.BatchLanguageTranslateElement = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // BatchLanguageTranslateElement
            // 
            this.BatchLanguageTranslateElement.Caption = "Dịch ngữ";
            this.BatchLanguageTranslateElement.Category = "Edit";
            this.BatchLanguageTranslateElement.ConfirmationMessage = null;
            this.BatchLanguageTranslateElement.Id = "BatchLanguageTranslateElement";
            this.BatchLanguageTranslateElement.ImageName = "Action_BatchLanguageTranslateElement";
            this.BatchLanguageTranslateElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.BatchLanguageTranslateElement.TargetViewId = "Video_ElementBatchList_ListView";  
            this.BatchLanguageTranslateElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.BatchLanguageTranslateElement.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.BatchLanguageTranslateElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.BatchLanguageTranslateElement.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BatchLanguageTranslateElement_Execute);
            // 
            // ElementBatchViewController
            // 
            this.Actions.Add(this.BatchLanguageTranslateElement);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction MatchLineBatchElement;
		private DevExpress.ExpressApp.Actions.SimpleAction ElementBatchImport;
		private DevExpress.ExpressApp.Actions.SimpleAction BatchLanguageTranslateElement;
		private DevExpress.ExpressApp.Actions.SimpleAction BatchTranslateImportElement;
		private DevExpress.ExpressApp.Actions.SimpleAction BatchTranslateTranslationElement;
    }
}