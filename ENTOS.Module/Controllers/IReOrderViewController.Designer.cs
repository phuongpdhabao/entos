namespace ENTOS.Module.Controllers 
{
    partial class IReOrderViewController
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
			this.ReOrder = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ReOrder
            // 
            this.ReOrder.Caption = "Đánh số lại";
            this.ReOrder.Category = "Edit";
            this.ReOrder.ConfirmationMessage = null;
            this.ReOrder.Id = "ReOrder";
            this.ReOrder.ImageName = "Action_ReOrder";
            this.ReOrder.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ReOrder.TargetViewNesting = DevExpress.ExpressApp.Nesting.Nested;
            this.ReOrder.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.ReOrder.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.ReOrder.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReOrder_Execute);
            // 
            // IReOrderViewController
            // 
            this.Actions.Add(this.ReOrder);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction ReOrder;
    }
}