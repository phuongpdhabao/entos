namespace ENTOS.Module.Controllers 
{
    partial class IQuantityViewController
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
			this.Quantity = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemQuantitySyllable = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemQuantityWord = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemQuantityCharacter = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // Quantity
            // 
            this.Quantity.Caption = "Số lượng";
            this.Quantity.Category = "Edit";
            this.Quantity.ConfirmationMessage = null;
            this.Quantity.Id = "Quantity";
            this.Quantity.ImageName = "Action_Quantity";
            this.Quantity.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.Quantity.ToolTip = "Đếm giá trị của trường kiểu String tại vị trí con trỏ";  
            this.Quantity.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.Quantity.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.Quantity.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.Quantity.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemQuantityWord.Caption = "Từ";
            choiceActionItemQuantityWord.Data = "Word";
            choiceActionItemQuantityWord.Id = "Word";
            this.Quantity.Items.Add(choiceActionItemQuantityWord);

            
            //
            //Root Choice
            choiceActionItemQuantityCharacter.Caption = "Kí tự";
            choiceActionItemQuantityCharacter.Data = "Character";
            choiceActionItemQuantityCharacter.Id = "Character";
            this.Quantity.Items.Add(choiceActionItemQuantityCharacter);

            
            //
            //Root Choice
            choiceActionItemQuantitySyllable.Caption = "Âm tiết";
            choiceActionItemQuantitySyllable.Data = "Syllable";
            choiceActionItemQuantitySyllable.Id = "Syllable";
            this.Quantity.Items.Add(choiceActionItemQuantitySyllable);

            this.Quantity.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.Quantity_Execute);
            // 
            // IQuantityViewController
            // 
            this.Actions.Add(this.Quantity);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction Quantity;
    }
}