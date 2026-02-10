namespace ENTOS.Module.Controllers
{
    partial class BatchTranslateViewController
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
			// BatchTranslateImport
            this.BatchTranslateImport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // BatchTranslateImport
            // 
            this.BatchTranslateImport.Caption = "Nạp Dịch lô";
            this.BatchTranslateImport.Category = "Edit";
            this.BatchTranslateImport.ConfirmationMessage = null;
            this.BatchTranslateImport.Id = "BatchTranslateImport";
            this.BatchTranslateImport.ImageName = "Action_BatchTranslateImport";
            this.BatchTranslateImport.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.BatchTranslateImport.ToolTip = "Tạo các Dịch lô theo danh sách Ngữ dịch trong tab Ngôn ngữ của Tư liệu";  
			
			this.BatchTranslateImport.TargetViewId = "ElementBatch_BatchTranslateList_ListView";  
            this.BatchTranslateImport.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.BatchTranslateImport.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.BatchTranslateImport.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.BatchTranslateImport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BatchTranslateImport_Execute);
            // 
            // BatchTranslateViewController
            // 
            this.Actions.Add(this.BatchTranslateImport);
			// TranslateCommand
            this.TranslateCommand = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTranslateCommandTranslateSymbol = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTranslateCommandTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemTranslateCommandReverseTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // TranslateCommand
            // 
            this.TranslateCommand.Caption = "Lệnh dịch";
            this.TranslateCommand.Category = "Edit";
            this.TranslateCommand.ConfirmationMessage = null;
            this.TranslateCommand.Id = "TranslateCommand";
            this.TranslateCommand.ImageName = "Action_TranslateCommand";
            this.TranslateCommand.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.TranslateCommand.ToolTip = "Tạo các Prompt để ra lệnh dịch cho AI từ Ghi chú của Tư liệu + Ngữ dịch, Ngữ gốc + Content của các thành phần thuộc lô";  
            this.TranslateCommand.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.TranslateCommand.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.TranslateCommand.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
			this.TranslateCommand.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemTranslateCommandTranslate.Caption = "Dịch xuôi";
            choiceActionItemTranslateCommandTranslate.Data = "Translate";
            choiceActionItemTranslateCommandTranslate.Id = "Translate";
            this.TranslateCommand.Items.Add(choiceActionItemTranslateCommandTranslate);

            
            //
            //Root Choice
            choiceActionItemTranslateCommandReverseTranslate.Caption = "Dịch ngược";
            choiceActionItemTranslateCommandReverseTranslate.Data = "ReverseTranslate";
            choiceActionItemTranslateCommandReverseTranslate.Id = "ReverseTranslate";
            this.TranslateCommand.Items.Add(choiceActionItemTranslateCommandReverseTranslate);

            
            //
            //Root Choice
            choiceActionItemTranslateCommandTranslateSymbol.Caption = "Dịch xuôi ngăn dòng";
            choiceActionItemTranslateCommandTranslateSymbol.Data = "TranslateSymbol";
            choiceActionItemTranslateCommandTranslateSymbol.Id = "TranslateSymbol";
            this.TranslateCommand.Items.Add(choiceActionItemTranslateCommandTranslateSymbol);

            this.TranslateCommand.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.TranslateCommand_Execute);
            // 
            // BatchTranslateViewController
            // 
            this.Actions.Add(this.TranslateCommand);
			// BatchTranslateTranslation
            this.BatchTranslateTranslation = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemBatchTranslateTranslationTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemBatchTranslateTranslationTranslate2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemBatchTranslateTranslationContent = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // BatchTranslateTranslation
            // 
            this.BatchTranslateTranslation.Caption = "Dịch thuật lô";
            this.BatchTranslateTranslation.Category = "Edit";
            this.BatchTranslateTranslation.ConfirmationMessage = null;
            this.BatchTranslateTranslation.Id = "BatchTranslateTranslation";
            this.BatchTranslateTranslation.ImageName = "Action_BatchTranslateTranslation";
            this.BatchTranslateTranslation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.BatchTranslateTranslation.ToolTip = "Thực hiện dịch thuật Dịch lô được chọn theo menu tương ứng";  
            this.BatchTranslateTranslation.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.BatchTranslateTranslation.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.BatchTranslateTranslation.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.BatchTranslateTranslation.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemBatchTranslateTranslationTranslate2.Caption = "Dịch Google";
            choiceActionItemBatchTranslateTranslationTranslate2.Data = "Translate2";
            choiceActionItemBatchTranslateTranslationTranslate2.Id = "Translate2";
            this.BatchTranslateTranslation.Items.Add(choiceActionItemBatchTranslateTranslationTranslate2);

            
            //
            //Root Choice
            choiceActionItemBatchTranslateTranslationContent.Caption = "Dịch xuôi";
            choiceActionItemBatchTranslateTranslationContent.Data = "Content";
            choiceActionItemBatchTranslateTranslationContent.Id = "Content";
            this.BatchTranslateTranslation.Items.Add(choiceActionItemBatchTranslateTranslationContent);

            
            //
            //Root Choice
            choiceActionItemBatchTranslateTranslationTranslate.Caption = "Dịch ngược";
            choiceActionItemBatchTranslateTranslationTranslate.Data = "Translate";
            choiceActionItemBatchTranslateTranslationTranslate.Id = "Translate";
            this.BatchTranslateTranslation.Items.Add(choiceActionItemBatchTranslateTranslationTranslate);

            this.BatchTranslateTranslation.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.BatchTranslateTranslation_Execute);
            // 
            // BatchTranslateViewController
            // 
            this.Actions.Add(this.BatchTranslateTranslation);
			// MatchlineBatch
            this.MatchlineBatch = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMatchlineBatchTranslate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemMatchlineBatchSynchronize = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // MatchlineBatch
            // 
            this.MatchlineBatch.Caption = "Khớp dòng lô";
            this.MatchlineBatch.Category = "Edit";
            this.MatchlineBatch.ConfirmationMessage = null;
            this.MatchlineBatch.Id = "MatchlineBatch";
            this.MatchlineBatch.ImageName = "Action_MatchlineBatch";
            this.MatchlineBatch.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.MatchlineBatch.ToolTip = "Khớp dòng giữa Dịch xuôi và Dịch ngược, đồng bộ dòng theo 1 Dịch lô lấy làm chuẩn (vị trí con trỏ)";  
            this.MatchlineBatch.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.MatchlineBatch.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.MatchlineBatch.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.MatchlineBatch.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemMatchlineBatchTranslate.Caption = "Dịch ngược";
            choiceActionItemMatchlineBatchTranslate.Data = "Translate";
            choiceActionItemMatchlineBatchTranslate.Id = "Translate";
            this.MatchlineBatch.Items.Add(choiceActionItemMatchlineBatchTranslate);

            
            //
            //Root Choice
            choiceActionItemMatchlineBatchSynchronize.Caption = "Đồng bộ";
            choiceActionItemMatchlineBatchSynchronize.Data = "Synchronize";
            choiceActionItemMatchlineBatchSynchronize.Id = "Synchronize";
            this.MatchlineBatch.Items.Add(choiceActionItemMatchlineBatchSynchronize);

            this.MatchlineBatch.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.MatchlineBatch_Execute);
            // 
            // BatchTranslateViewController
            // 
            this.Actions.Add(this.MatchlineBatch);
			// ExportElement
            this.ExportElement = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExportElement
            // 
            this.ExportElement.Caption = "Xuất thành phần";
            this.ExportElement.Category = "Edit";
            this.ExportElement.ConfirmationMessage = null;
            this.ExportElement.Id = "ExportElement";
            this.ExportElement.ImageName = "Action_ExportElement";
            this.ExportElement.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.ExportElement.ToolTip = "Xuất dữ liệu Dịch xuôi + Dịch ngược sang Dịch và Phiên âm và theo STT của Thành phần";  
			
			this.ExportElement.TargetViewId = "ElementBatch_BatchTranslateList_ListView";  
            this.ExportElement.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ExportElement.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ExportElement.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.ExportElement.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportElement_Execute);
            // 
            // BatchTranslateViewController
            // 
            this.Actions.Add(this.ExportElement);
			// BatchLanguageTranslate
            this.BatchLanguageTranslate = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemBatchLanguageTranslateExport = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemBatchLanguageTranslateDelete = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // BatchLanguageTranslate
            // 
            this.BatchLanguageTranslate.Caption = "Dịch ngữ";
            this.BatchLanguageTranslate.Category = "Edit";
            this.BatchLanguageTranslate.ConfirmationMessage = null;
            this.BatchLanguageTranslate.Id = "BatchLanguageTranslate";
            this.BatchLanguageTranslate.ImageName = "Action_BatchLanguageTranslate";
            this.BatchLanguageTranslate.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.BatchLanguageTranslate.ToolTip = "Nếu số dòng Dịch xuôi bằng số Thành phần của Lô thì tạo các Dịch ngữ và copy Dịch xuôi ứng với mỗi Thành phần";  
			
			this.BatchLanguageTranslate.TargetViewId = "ElementBatch_BatchTranslateList_ListView";  
            this.BatchLanguageTranslate.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.BatchLanguageTranslate.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.BatchLanguageTranslate.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.BatchLanguageTranslate.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemBatchLanguageTranslateExport.Caption = "Xuất";
            choiceActionItemBatchLanguageTranslateExport.Data = "Export";
            choiceActionItemBatchLanguageTranslateExport.Id = "Export";
            this.BatchLanguageTranslate.Items.Add(choiceActionItemBatchLanguageTranslateExport);

            
            //
            //Root Choice
            choiceActionItemBatchLanguageTranslateDelete.Caption = "Xóa";
            choiceActionItemBatchLanguageTranslateDelete.Data = "Delete";
            choiceActionItemBatchLanguageTranslateDelete.Id = "Delete";
            this.BatchLanguageTranslate.Items.Add(choiceActionItemBatchLanguageTranslateDelete);

            this.BatchLanguageTranslate.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.BatchLanguageTranslate_Execute);
            // 
            // BatchTranslateViewController
            // 
            this.Actions.Add(this.BatchLanguageTranslate);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction BatchTranslateTranslation;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction MatchlineBatch;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction BatchLanguageTranslate;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction TranslateCommand;
		private DevExpress.ExpressApp.Actions.SimpleAction BatchTranslateImport;
		private DevExpress.ExpressApp.Actions.SimpleAction ExportElement;
    }
}