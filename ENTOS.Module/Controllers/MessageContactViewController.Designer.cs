namespace ENTOS.Module.Controllers
{
    partial class MessageContactViewController
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
			// ImportContactFromGroup
            this.ImportContactFromGroup = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportContactFromGroup
            // 
            this.ImportContactFromGroup.Caption = "Nhập từ nhóm";
            this.ImportContactFromGroup.Category = "Edit";
            this.ImportContactFromGroup.ConfirmationMessage = null;
            this.ImportContactFromGroup.Id = "ImportContactFromGroup";
            this.ImportContactFromGroup.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ImportContactFromGroup.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ImportContactFromGroup.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.ImportContactFromGroup.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportContactFromGroup_Execute);
            // 
            // MessageContactViewController
            // 
            this.Actions.Add(this.ImportContactFromGroup);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction ImportContactFromGroup;
    }
}