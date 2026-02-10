namespace ENTOS.Module.Controllers
{
    partial class AccountEntryViewController
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
			// AccountingTemplate
            this.AccountingTemplate = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemAccountingTemplateCreateAccountingTemplate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // AccountingTemplate
            // 
            this.AccountingTemplate.Caption = "Hạch toán mẫu";
            this.AccountingTemplate.Category = "Edit";
            this.AccountingTemplate.ConfirmationMessage = null;
            this.AccountingTemplate.Id = "AccountingTemplate";
            this.AccountingTemplate.TargetViewNesting = DevExpress.ExpressApp.Nesting.Nested;
            this.AccountingTemplate.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.AccountingTemplate.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
			this.AccountingTemplate.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemAccountingTemplateCreateAccountingTemplate.Caption = "Tạo hạch toán mẫu";
            choiceActionItemAccountingTemplateCreateAccountingTemplate.Data = "CreateAccountingTemplate";
            choiceActionItemAccountingTemplateCreateAccountingTemplate.Id = "CreateAccountingTemplate";
            this.AccountingTemplate.Items.Add(choiceActionItemAccountingTemplateCreateAccountingTemplate);

            this.AccountingTemplate.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.AccountingTemplate_Execute);
            // 
            // AccountEntryViewController
            // 
            this.Actions.Add(this.AccountingTemplate);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction AccountingTemplate;
    }
}