namespace ENTOS.Module.Controllers
{
    partial class ChatSessionViewController
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
			// MemberChat
            this.MemberChat = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // MemberChat
            // 
            this.MemberChat.Caption = "Nhắn thành viên";
            this.MemberChat.Category = "Edit";
            this.MemberChat.ConfirmationMessage = null;
            this.MemberChat.Id = "MemberChat";
            this.MemberChat.ImageName = "Action_MemberChat";
            this.MemberChat.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.MemberChat.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.MemberChat.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;            
			this.MemberChat.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.Independent;   
            this.MemberChat.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.MemberChat_Execute);
            // 
            // ChatSessionViewController
            // 
            this.Actions.Add(this.MemberChat);
			// SendChat
            this.SendChat = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SendChat
            // 
            this.SendChat.Caption = "Gửi";
            this.SendChat.Category = "Edit";
            this.SendChat.ConfirmationMessage = null;
            this.SendChat.Id = "SendChat";
            this.SendChat.ImageName = "Action_SendChat";
            this.SendChat.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
		
            this.SendChat.Shortcut = "Enter";
            this.SendChat.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.SendChat.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;            
			this.SendChat.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.SendChat.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SendChat_Execute);
            // 
            // ChatSessionViewController
            // 
            this.Actions.Add(this.SendChat);
			// CancelChat
            this.CancelChat = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CancelChat
            // 
            this.CancelChat.Caption = "Hủy";
            this.CancelChat.Category = "Edit";
            this.CancelChat.ConfirmationMessage = null;
            this.CancelChat.Id = "CancelChat";
            this.CancelChat.ImageName = "Action_CancelChat";
            this.CancelChat.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
		
            this.CancelChat.Shortcut = "Enter";
			
			this.CancelChat.TargetObjectsCriteria = "[Reply] Is Not Null Or [Modify] Is Not Null";  
            this.CancelChat.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.CancelChat.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;            
			this.CancelChat.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.CancelChat.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CancelChat_Execute);
            // 
            // ChatSessionViewController
            // 
            this.Actions.Add(this.CancelChat);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction CancelChat;
		private DevExpress.ExpressApp.Actions.SimpleAction SendChat;
		private DevExpress.ExpressApp.Actions.SimpleAction MemberChat;
    }
}