namespace ENTOS.Module.Controllers
{
    partial class MediaViewController
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
			// QuantityMedia
            this.QuantityMedia = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemQuantityMediaSameGroup = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemQuantityMediaChildElement = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemQuantityMediaChildTextbox = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemQuantityMediaTextWord = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // QuantityMedia
            // 
            this.QuantityMedia.Caption = "Số lượng";
            this.QuantityMedia.Category = "Edit";
            this.QuantityMedia.ConfirmationMessage = null;
            this.QuantityMedia.Id = "QuantityMedia";
            this.QuantityMedia.ImageName = "Action_QuantityMedia";
            this.QuantityMedia.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.QuantityMedia.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.QuantityMedia.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.QuantityMedia.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.QuantityMedia.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemQuantityMediaTextWord.Caption = "Từ văn bản trong";
            choiceActionItemQuantityMediaTextWord.Data = "TextWord";
            choiceActionItemQuantityMediaTextWord.Id = "TextWord";
            this.QuantityMedia.Items.Add(choiceActionItemQuantityMediaTextWord);

            
            //
            //Root Choice
            choiceActionItemQuantityMediaChildElement.Caption = "Phần tử";
            choiceActionItemQuantityMediaChildElement.Data = "ChildElement";
            choiceActionItemQuantityMediaChildElement.Id = "ChildElement";
            this.QuantityMedia.Items.Add(choiceActionItemQuantityMediaChildElement);

            
            //
            //Root Choice
            choiceActionItemQuantityMediaChildTextbox.Caption = "Hộp văn bản";
            choiceActionItemQuantityMediaChildTextbox.Data = "ChildTextbox";
            choiceActionItemQuantityMediaChildTextbox.Id = "ChildTextbox";
            this.QuantityMedia.Items.Add(choiceActionItemQuantityMediaChildTextbox);

            
            //
            //Root Choice
            choiceActionItemQuantityMediaSameGroup.Caption = "Cùng group";
            choiceActionItemQuantityMediaSameGroup.Data = "SameGroup";
            choiceActionItemQuantityMediaSameGroup.Id = "SameGroup";
            this.QuantityMedia.Items.Add(choiceActionItemQuantityMediaSameGroup);

            this.QuantityMedia.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.QuantityMedia_Execute);
            // 
            // MediaViewController
            // 
            this.Actions.Add(this.QuantityMedia);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction QuantityMedia;
    }
}