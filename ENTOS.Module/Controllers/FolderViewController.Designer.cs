namespace ENTOS.Module.Controllers
{
    partial class FolderViewController
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
			// MemberFolder
            this.MemberFolder = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // MemberFolder
            // 
            this.MemberFolder.Caption = "Chọn tập thể";
            this.MemberFolder.Category = "Edit";
            this.MemberFolder.ConfirmationMessage = null;
            this.MemberFolder.Id = "MemberFolder";
            this.MemberFolder.ImageName = "Action_MemberFolder";
            this.MemberFolder.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.MemberFolder.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.MemberFolder.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.MemberFolder.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.MemberFolder.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.MemberFolder.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.MemberFolder_Execute);
            // 
            // FolderViewController
            // 
            this.Actions.Add(this.MemberFolder);
			// ExportComputer
            this.ExportComputer = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemExportComputerWordpress = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemExportComputerComputer = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ExportComputer
            // 
            this.ExportComputer.Caption = "Xuất máy tinh";
            this.ExportComputer.Category = "Edit";
            this.ExportComputer.ConfirmationMessage = null;
            this.ExportComputer.Id = "ExportComputer";
            this.ExportComputer.ImageName = "Action_ExportComputer";
            this.ExportComputer.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
			
			this.ExportComputer.TargetViewId = "Folder_LowerFolder_ListView";  
            this.ExportComputer.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ExportComputer.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ExportComputer.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.ExportComputer.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemExportComputerComputer.Caption = "Máy tính";
            choiceActionItemExportComputerComputer.Data = "Computer";
            choiceActionItemExportComputerComputer.Id = "Computer";
            this.ExportComputer.Items.Add(choiceActionItemExportComputerComputer);

            
            //
            //Root Choice
            choiceActionItemExportComputerWordpress.Caption = "Wordpress";
            choiceActionItemExportComputerWordpress.Data = "Wordpress";
            choiceActionItemExportComputerWordpress.Id = "Wordpress";
            this.ExportComputer.Items.Add(choiceActionItemExportComputerWordpress);

            this.ExportComputer.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ExportComputer_Execute);
            // 
            // FolderViewController
            // 
            this.Actions.Add(this.ExportComputer);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction MemberFolder;
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ExportComputer;
    }
}