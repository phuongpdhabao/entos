namespace ENTOS.Module.Controllers
{
    partial class PaymentAccountViewController
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
			// TransactionSynchronization
            this.TransactionSynchronization = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // TransactionSynchronization
            // 
            this.TransactionSynchronization.Caption = "Đồng bộ";
            this.TransactionSynchronization.Category = "Edit";
            this.TransactionSynchronization.ConfirmationMessage = null;
            this.TransactionSynchronization.Id = "TransactionSynchronization";
            this.TransactionSynchronization.ImageName = "Action_TransactionSynchronization";
            this.TransactionSynchronization.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.TransactionSynchronization.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.TransactionSynchronization.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.TransactionSynchronization.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
			this.TransactionSynchronization.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.TransactionSynchronization.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.TransactionSynchronization_Execute);
            // 
            // PaymentAccountViewController
            // 
            this.Actions.Add(this.TransactionSynchronization);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction TransactionSynchronization;
    }
}