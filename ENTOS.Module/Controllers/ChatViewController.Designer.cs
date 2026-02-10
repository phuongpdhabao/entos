namespace ENTOS.Module.Controllers
{
    partial class ChatViewController
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
			// ModifyChat
            this.ModifyChat = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ModifyChat
            // 
            this.ModifyChat.Caption = "Chỉnh sửa";
            this.ModifyChat.Category = "Edit";
            this.ModifyChat.ConfirmationMessage = null;
            this.ModifyChat.Id = "ModifyChat";
            this.ModifyChat.ImageName = "Action_ModifyChat";
            this.ModifyChat.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ModifyChat.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ModifyChat.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ModifyChat.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.ModifyChat.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ModifyChat_Execute);
            // 
            // ChatViewController
            // 
            this.Actions.Add(this.ModifyChat);
			// ReplyChat
            this.ReplyChat = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ReplyChat
            // 
            this.ReplyChat.Caption = "Trả lời";
            this.ReplyChat.Category = "Edit";
            this.ReplyChat.ConfirmationMessage = null;
            this.ReplyChat.Id = "ReplyChat";
            this.ReplyChat.ImageName = "Action_ReplyChat";
            this.ReplyChat.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ReplyChat.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.ReplyChat.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.ReplyChat.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.ReplyChat.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReplyChat_Execute);
            // 
            // ChatViewController
            // 
            this.Actions.Add(this.ReplyChat);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction ModifyChat;
		private DevExpress.ExpressApp.Actions.SimpleAction ReplyChat;
    }
}