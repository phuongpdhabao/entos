namespace ENTOS.Module.Controllers
{
    partial class EntryFolderViewController
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
			// AccountBalance
            this.AccountBalance = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemAccountBalanceBook1Sum = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemAccountBalanceBook1SumBook1Sum = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemAccountBalanceBook1SumBook1Debit = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemAccountBalanceBook1SumBook1Credit = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemAccountBalanceBook2Sum = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemAccountBalanceBook2SumBook2Debit = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemAccountBalanceBook2SumBook2Sum = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemAccountBalanceBook2SumBook2Credit = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // AccountBalance
            // 
            this.AccountBalance.Caption = "Số dư tài khoản";
            this.AccountBalance.Category = "Edit";
            this.AccountBalance.ConfirmationMessage = null;
            this.AccountBalance.Id = "AccountBalance";
			
			this.AccountBalance.ToolTip = "Tính tổng số dư tài khoản (hoặc tổng ghi nợ/ghi có) cho từng thư mục (bao gồm các bút toán của chính nó và tài khoản con)";  
            this.AccountBalance.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.AccountBalance.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.AccountBalance.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.AccountBalance.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemAccountBalanceBook1Sum.Caption = "Thuế";
            choiceActionItemAccountBalanceBook1Sum.Data = "Book1Sum";
            choiceActionItemAccountBalanceBook1Sum.Id = "Book1Sum";
            this.AccountBalance.Items.Add(choiceActionItemAccountBalanceBook1Sum);

            
            //
            //Choice
            choiceActionItemAccountBalanceBook1SumBook1Sum.Caption = "Tổng";
            choiceActionItemAccountBalanceBook1SumBook1Sum.Data = "Book1Sum";
            choiceActionItemAccountBalanceBook1SumBook1Sum.Id = "Book1Sum";
            choiceActionItemAccountBalanceBook1Sum.Items.Add(choiceActionItemAccountBalanceBook1SumBook1Sum);
             
            //
            //Choice
            choiceActionItemAccountBalanceBook1SumBook1Debit.Caption = "Nợ";
            choiceActionItemAccountBalanceBook1SumBook1Debit.Data = "Book1Debit";
            choiceActionItemAccountBalanceBook1SumBook1Debit.Id = "Book1Debit";
            choiceActionItemAccountBalanceBook1Sum.Items.Add(choiceActionItemAccountBalanceBook1SumBook1Debit);
             
            //
            //Choice
            choiceActionItemAccountBalanceBook1SumBook1Credit.Caption = "Có";
            choiceActionItemAccountBalanceBook1SumBook1Credit.Data = "Book1Credit";
            choiceActionItemAccountBalanceBook1SumBook1Credit.Id = "Book1Credit";
            choiceActionItemAccountBalanceBook1Sum.Items.Add(choiceActionItemAccountBalanceBook1SumBook1Credit);
             
            //
            //Root Choice
            choiceActionItemAccountBalanceBook2Sum.Caption = "Nội bộ";
            choiceActionItemAccountBalanceBook2Sum.Data = "Book2Sum";
            choiceActionItemAccountBalanceBook2Sum.Id = "Book2Sum";
            this.AccountBalance.Items.Add(choiceActionItemAccountBalanceBook2Sum);

            
            //
            //Choice
            choiceActionItemAccountBalanceBook2SumBook2Sum.Caption = "Tổng";
            choiceActionItemAccountBalanceBook2SumBook2Sum.Data = "Book2Sum";
            choiceActionItemAccountBalanceBook2SumBook2Sum.Id = "Book2Sum";
            choiceActionItemAccountBalanceBook2Sum.Items.Add(choiceActionItemAccountBalanceBook2SumBook2Sum);
             
            //
            //Choice
            choiceActionItemAccountBalanceBook2SumBook2Debit.Caption = "Nợ";
            choiceActionItemAccountBalanceBook2SumBook2Debit.Data = "Book2Debit";
            choiceActionItemAccountBalanceBook2SumBook2Debit.Id = "Book2Debit";
            choiceActionItemAccountBalanceBook2Sum.Items.Add(choiceActionItemAccountBalanceBook2SumBook2Debit);
             
            //
            //Choice
            choiceActionItemAccountBalanceBook2SumBook2Credit.Caption = "Có";
            choiceActionItemAccountBalanceBook2SumBook2Credit.Data = "Book2Credit";
            choiceActionItemAccountBalanceBook2SumBook2Credit.Id = "Book2Credit";
            choiceActionItemAccountBalanceBook2Sum.Items.Add(choiceActionItemAccountBalanceBook2SumBook2Credit);
             this.AccountBalance.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.AccountBalance_Execute);
            // 
            // EntryFolderViewController
            // 
            this.Actions.Add(this.AccountBalance);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction AccountBalance;
    }
}