using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using Microsoft.Extensions.DependencyInjection;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;
using ENTOS.Module.Services;
using ListView = DevExpress.ExpressApp.ListView;


namespace ENTOS.Module.Controllers 
{
    public partial class ChatViewController: BaseViewController<Module.BusinessObjects.Chat>
    {      
        
        public ChatViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Chat);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
             
            #region ModifyChatOnViewControlsCreatedCode
		            }
        private Frame masterFrame;
        public void AssignMasterFrame(Frame masterFrame)
        {
            this.masterFrame = masterFrame;
            // Use this Frame to get Controllers and Actions. 
		    #endregion ModifyChatOnViewControlsCreatedCode
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 2823            Oid: 9ea71197-1871-4c56-b6b6-3c2a81d8f038
		private void ModifyChat_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ModifyChat), "Chỉnh sửa");              
      
            #region ModifyChatImportCode
            if (View.CurrentObject is Chat chat && ObjectSpace != null)
            {
                bool isModified = ObjectSpace.IsModified;
                chat.ChatSession.Modify = chat;
                //chat.ChatSession.FieldChange("Modify");
                chat.ChatSession.CreateChat = chat.Content;
                if(chat.ChatSession.createChatPropertyEditor != null)
                {
                    //chat.ChatSession.createChatPropertyEditor.RefreshDataSource();                    
                    chat.ChatSession.createChatPropertyEditor.ReadValue();
                    Module.SystemObjects.Tools.CallObjectMethod(chat.ChatSession.createChatPropertyEditor.Control, "Focus");                    
                }
                //if(masterFrame != null)
                //{
                //    var chatSessionViewController = masterFrame.GetController<ChatSessionViewController>();
                //    if (chatSessionViewController?.createChatPropertyEditor != null)
                //    {
                //        //chatSessionViewController.createChatPropertyEditor.RefreshDataSource();
                //        //Module.SystemObjects.Tools.SetPropertyValueInObject(chatSessionViewController.createChatPropertyEditor.Control, "EditValue", chat.Content);
                //        chatSessionViewController.createChatPropertyEditor.ReadValue();
                //        Module.SystemObjects.Tools.CallObjectMethod(chatSessionViewController.createChatPropertyEditor.Control, "Focus");
                //    }
                //}                
                ObjectSpace.SetModified(chat.ChatSession);
                if (!isModified)
                {
                    ObjectSpace.RemoveFromModifiedObjects(chat.ChatSession);
                }
                chat.ChatSession.Reload();
            }

            #endregion ModifyChatImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 2824            Oid: 59b4d2e6-7b27-46a2-9067-3f34adb09749
		private void ReplyChat_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ReplyChat), "Trả lời");              
      
            #region ReplyChatImportCode
  if (View.CurrentObject is Chat chat)
  {
      chat.ChatSession.CreateChat = null;
      bool isModified = ObjectSpace.IsModified;
      chat.ChatSession.Reply = chat;
                if (chat.ChatSession.createChatPropertyEditor != null)
                {
                    //chat.ChatSession.createChatPropertyEditor.RefreshDataSource();                    
                    chat.ChatSession.createChatPropertyEditor.ReadValue();
                    Module.SystemObjects.Tools.CallObjectMethod(chat.ChatSession.createChatPropertyEditor.Control, "Focus");
                }
                ObjectSpace.SetModified(chat.ChatSession);
      if (!isModified)
      {
          ObjectSpace.RemoveFromModifiedObjects(chat.ChatSession);
      }
      chat.ChatSession.Reload();
  }
            #endregion ReplyChatImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}