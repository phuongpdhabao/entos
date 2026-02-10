namespace ENTOS.Module.Controllers
{
    partial class INewObjectSessionViewController
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
			// NewObjectSession
            this.NewObjectSession = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemNewObjectSessionOpen = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemNewObjectSessionCreate = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // NewObjectSession
            // 
            this.NewObjectSession.Caption = "Tạo mới";
            this.NewObjectSession.Category = "ObjectsCreation";
            this.NewObjectSession.ConfirmationMessage = null;
            this.NewObjectSession.Id = "NewObjectSession";
            this.NewObjectSession.ImageName = "Action_NewObjectSession";
            this.NewObjectSession.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.NewObjectSession.TargetViewNesting = DevExpress.ExpressApp.Nesting.Nested;
            this.NewObjectSession.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.NewObjectSession.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
			this.NewObjectSession.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemNewObjectSessionCreate.Caption = "Tạo";
            choiceActionItemNewObjectSessionCreate.Data = "Create";
            choiceActionItemNewObjectSessionCreate.Id = "Create";
            this.NewObjectSession.Items.Add(choiceActionItemNewObjectSessionCreate);

            
            //
            //Root Choice
            choiceActionItemNewObjectSessionOpen.Caption = "Mở";
            choiceActionItemNewObjectSessionOpen.Data = "Open";
            choiceActionItemNewObjectSessionOpen.Id = "Open";
            this.NewObjectSession.Items.Add(choiceActionItemNewObjectSessionOpen);

            this.NewObjectSession.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.NewObjectSession_Execute);
            // 
            // INewObjectSessionViewController
            // 
            this.Actions.Add(this.NewObjectSession);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction NewObjectSession;
    }
}