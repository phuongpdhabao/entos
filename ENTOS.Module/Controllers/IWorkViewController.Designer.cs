namespace ENTOS.Module.Controllers 
{
    partial class IWorkViewController
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
			this.Work = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWorkCreate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWorkUnLink = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWorkOpen = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemWorkLink = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // Work
            // 
            this.Work.Caption = "Công việc";
            this.Work.Category = "Edit";
            this.Work.ConfirmationMessage = null;
            this.Work.Id = "Work";
            this.Work.ImageName = "Action_Work";
            this.Work.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.Work.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.Work.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.Work.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.Work.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemWorkOpen.Caption = "Mở";
            choiceActionItemWorkOpen.Data = "Open";
            choiceActionItemWorkOpen.Id = "Open";
            this.Work.Items.Add(choiceActionItemWorkOpen);

            
            //
            //Root Choice
            choiceActionItemWorkCreate.Caption = "Tạo";
            choiceActionItemWorkCreate.Data = "Create";
            choiceActionItemWorkCreate.Id = "Create";
            this.Work.Items.Add(choiceActionItemWorkCreate);

            
            //
            //Root Choice
            choiceActionItemWorkLink.Caption = "Liên kết";
            choiceActionItemWorkLink.Data = "Link";
            choiceActionItemWorkLink.Id = "Link";
            this.Work.Items.Add(choiceActionItemWorkLink);

            
            //
            //Root Choice
            choiceActionItemWorkUnLink.Caption = "Xóa liên kết";
            choiceActionItemWorkUnLink.Data = "UnLink";
            choiceActionItemWorkUnLink.Id = "UnLink";
            this.Work.Items.Add(choiceActionItemWorkUnLink);

            this.Work.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.Work_Execute);
            // 
            // IWorkViewController
            // 
            this.Actions.Add(this.Work);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction Work;
    }
}