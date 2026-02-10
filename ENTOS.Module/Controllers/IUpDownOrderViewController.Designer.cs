namespace ENTOS.Module.Controllers 
{
    partial class IUpDownOrderViewController
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
			this.UpOrder = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpOrderTop = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemUpOrderUp = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // UpOrder
            // 
            this.UpOrder.Caption = "Lên";
            this.UpOrder.Category = "Edit";
            this.UpOrder.ConfirmationMessage = null;
            this.UpOrder.Id = "UpOrder";
            this.UpOrder.ImageName = "Action_UpOrder";
            this.UpOrder.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.UpOrder.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.UpOrder.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.UpOrder.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.UpOrder.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemUpOrderUp.Caption = "Lên";
            choiceActionItemUpOrderUp.Data = "Up";
            choiceActionItemUpOrderUp.Id = "Up";
            this.UpOrder.Items.Add(choiceActionItemUpOrderUp);

            
            //
            //Root Choice
            choiceActionItemUpOrderTop.Caption = "Trên cùng";
            choiceActionItemUpOrderTop.Data = "Top";
            choiceActionItemUpOrderTop.Id = "Top";
            this.UpOrder.Items.Add(choiceActionItemUpOrderTop);

            this.UpOrder.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.UpOrder_Execute);
            // 
            // IUpDownOrderViewController
            // 
            this.Actions.Add(this.UpOrder);
			this.DownOrder = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemDownOrderDown = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemDownOrderBottom = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // DownOrder
            // 
            this.DownOrder.Caption = "Xuống";
            this.DownOrder.Category = "Edit";
            this.DownOrder.ConfirmationMessage = null;
            this.DownOrder.Id = "DownOrder";
            this.DownOrder.ImageName = "Action_DownOrder";
            this.DownOrder.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.DownOrder.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.DownOrder.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.DownOrder.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.DownOrder.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemDownOrderDown.Caption = "Xuống";
            choiceActionItemDownOrderDown.Data = "Down";
            choiceActionItemDownOrderDown.Id = "Down";
            this.DownOrder.Items.Add(choiceActionItemDownOrderDown);

            
            //
            //Root Choice
            choiceActionItemDownOrderBottom.Caption = "Dưới cùng";
            choiceActionItemDownOrderBottom.Data = "Bottom";
            choiceActionItemDownOrderBottom.Id = "Bottom";
            this.DownOrder.Items.Add(choiceActionItemDownOrderBottom);

            this.DownOrder.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.DownOrder_Execute);
            // 
            // IUpDownOrderViewController
            // 
            this.Actions.Add(this.DownOrder);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction UpOrder;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction DownOrder;
    }
}