namespace ENTOS.Module.Controllers
{
    partial class RecognitionObjectViewController
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
			// ObjectVideo
            this.ObjectVideo = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemObjectVideoNoFrame = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItemObjectVideoHasFrame = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ObjectVideo
            // 
            this.ObjectVideo.Caption = "Tạo video";
            this.ObjectVideo.Category = "Edit";
            this.ObjectVideo.ConfirmationMessage = null;
            this.ObjectVideo.Id = "ObjectVideo";
            this.ObjectVideo.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ObjectVideo.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.ObjectVideo.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
			this.ObjectVideo.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            
            //
            //Root Choice
            choiceActionItemObjectVideoNoFrame.Caption = "Không kẻ khung";
            choiceActionItemObjectVideoNoFrame.Data = "NoFrame";
            choiceActionItemObjectVideoNoFrame.Id = "NoFrame";
            this.ObjectVideo.Items.Add(choiceActionItemObjectVideoNoFrame);

            
            //
            //Root Choice
            choiceActionItemObjectVideoHasFrame.Caption = "Kẻ khung";
            choiceActionItemObjectVideoHasFrame.Data = "HasFrame";
            choiceActionItemObjectVideoHasFrame.Id = "HasFrame";
            this.ObjectVideo.Items.Add(choiceActionItemObjectVideoHasFrame);

            this.ObjectVideo.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ObjectVideo_Execute);
            // 
            // RecognitionObjectViewController
            // 
            this.Actions.Add(this.ObjectVideo);
			// ObjectAvatar
            this.ObjectAvatar = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ObjectAvatar
            // 
            this.ObjectAvatar.Caption = "Phóng ảnh";
            this.ObjectAvatar.Category = "Edit";
            this.ObjectAvatar.ConfirmationMessage = null;
            this.ObjectAvatar.Id = "ObjectAvatar";
            this.ObjectAvatar.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ObjectAvatar.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ObjectAvatar.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.ObjectAvatar.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ObjectAvatar_Execute);
            // 
            // RecognitionObjectViewController
            // 
            this.Actions.Add(this.ObjectAvatar);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SingleChoiceAction ObjectVideo;
		private DevExpress.ExpressApp.Actions.SimpleAction ObjectAvatar;
    }
}