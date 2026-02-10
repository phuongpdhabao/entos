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
    public partial class ChatSessionViewController: BaseViewController<Module.BusinessObjects.ChatSession>
    {      
        
        public ChatSessionViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.ChatSession);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
            #region SendChatOnActivatedCode
		    		    if (View is DetailView)
            {
                var control = ((DetailView)View).FindItem(nameof(Module.BusinessObjects.ChatSession.CreateChat));
                if (control != null)
                {
                    control.ControlCreated += Control_ControlCreated;
                }
                View.CurrentObjectChanged += OnViewCurrentObjectChanged;
                //var listPropertyEditor = ((DetailView)View).FindItem(nameof(Module.BusinessObjects.ChatSession.ChatList)) as ListPropertyEditor;
                //if(listPropertyEditor != null)
                //{
                //    if (listPropertyEditor.Frame is not null)
                //    {
                //        PushFrameToNestedController(listPropertyEditor.Frame);
                //    }
                //    else
                //    {
                //        listPropertyEditor.FrameChanged += ListPropertyEditor_FrameChanged;
                //    }                  
                //}
            }
        }
        private void OnViewCurrentObjectChanged(object sender, EventArgs e)
        {

            if (View.CurrentObject is Module.BusinessObjects.ChatSession chatSession && createChatPropertyEditor != null)
                chatSession.createChatPropertyEditor = createChatPropertyEditor;

        }

        //private void PushFrameToNestedController(Frame frame) => frame.GetController<ChatViewController>()?.AssignMasterFrame(this.Frame);
        //private void ListPropertyEditor_FrameChanged(object sender, EventArgs e) => PushFrameToNestedController(((ListPropertyEditor)sender).Frame);

        public DevExpress.ExpressApp.Editors.PropertyEditor createChatPropertyEditor = null;
        private void Control_ControlCreated(object sender, EventArgs e)
        {
            if (sender is DevExpress.ExpressApp.Editors.PropertyEditor propertyEditor)
            {
                
                createChatPropertyEditor = propertyEditor;                
            }
            #endregion SendChatOnActivatedCode
        }
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
             
            #region SendChatOnViewControlsCreatedCode
		                _chatService = new Module.SystemObjects.ChatService(((DevExpress.ExpressApp.Xpo.XPObjectSpace)View.ObjectSpace).Session, Application);
            //_chatService.StartConnectionAsync().GetAwaiter().GetResult();
            System.Threading.Tasks.Task.Run(async () => await _chatService.StartConnectionAsync()).Wait();
            // Đăng ký các event handler
            _chatService.GroupReceiveMessage += OnGroupReceiveMessage;
            _chatService.PrivateMessageReceived += OnPrivateMessageReceived;
            uiContext = SynchronizationContext.Current;
        }

        private void OnGroupReceiveMessage(string groupId, string fromUser, string messageText)
        {
            try
            {
                //Tránh cross session
                if (uiContext != null)
                {
                    // Chuyển xử lý về UI thread
                    uiContext.Post(_ =>
                    {
                        ProcessMessage(groupId, fromUser, messageText);
                    }, null);
                }
                else
                {
                    // Nếu không có context, xử lý trực tiếp (cẩn thận)
                    ProcessMessage(groupId, fromUser, messageText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnGroupReceiveMessage: {ex.Message}");
            }

        }
        private SynchronizationContext uiContext;
        private SystemObjects.ChatService _chatService;

        private void ProcessMessage(string groupId, string fromUser, string messageText)
        {
            try
            {
                if (View.CurrentObject is Module.BusinessObjects.ChatSession chatSession && chatSession.Oid.ToString().Equals(groupId))
                {
                    if (!fromUser.Equals(SecuritySystem.CurrentUserName))
                    {
                        //chatSession.Reload();
                        chatSession.ChatList.Reload();
                        chatSession.SetDefaultChatListChatRead();
                        //chatSession.NewChat = true;
                        Module.SystemObjects.Tools.RefreshGridView(View);
                    }
                }
                else if (View is ListView listView)
                {
                    var otherChatSession = listView.CollectionSource.List
                        .OfType<Module.BusinessObjects.ChatSession>()
                        .FirstOrDefault(x => x.Oid.Equals(System.Guid.Parse(groupId)));
                    if (otherChatSession != null)
                    {
                        otherChatSession.Reload();
                        otherChatSession.SetDefaultChatListChatRead();
                        // otherChatSession.ChatList.Reload();
                        // otherChatSession.NewChat = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnGroupReceiveMessage: {ex.Message}");
            }
        }


        private void OnPrivateMessageReceived(string toUserId, string fromUser, string message)
        {
            //if (InvokeRequired)
            //{
            //    Invoke(new Action<string, string>(OnPrivateMessageReceived), fromUser, message);
            //    return;
            //}

            //// Hiển thị tin nhắn riêng lên UI
            //privateMessagesListBox.Items.Add($"[From {fromUser}] {message}");
		    #endregion SendChatOnViewControlsCreatedCode
        }
        
        protected override void OnDeactivated()
        {                 
            #region SendChatOnDeactivatedCode
		                if (View is DetailView)
            {
                var control = ((DetailView)View).FindItem(nameof(Module.BusinessObjects.ChatSession.CreateChat));
                if (control != null)
                {
                    control.ControlCreated -= Control_ControlCreated;
                }
                View.CurrentObjectChanged -= OnViewCurrentObjectChanged;
            }
            #endregion SendChatOnDeactivatedCode
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 1036            Oid: 6954e310-856f-409b-9d11-362411ee6123
		private void MemberChat_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(MemberChat), "Nhắn thành viên");              
      
            #region MemberChatImportCode
            var dc = Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>();
            dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs arg)
            {
                var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse(
                            "MemberList[Oid = ?] and MemberList.Count() = ?",
                            SecuritySystem.CurrentUserId, arg.AcceptActionArgs.SelectedObjects.Count + 1);
                foreach (Module.BusinessObjects.Member member in arg.AcceptActionArgs.SelectedObjects)
                {
                    criteria = DevExpress.Data.Filtering.CriteriaOperator.And(criteria,
                        DevExpress.Data.Filtering.CriteriaOperator.Parse(
                            "MemberList[Oid = ?]", member.Oid));
                }
                var newObjectSpace = Application.CreateObjectSpace();
                var chatSession = newObjectSpace.FindObject<Module.BusinessObjects.ChatSession>(criteria);
                if (chatSession is null)
                {
                    chatSession = newObjectSpace.CreateObject<Module.BusinessObjects.ChatSession>();
                    chatSession.MemberList.Add(chatSession.Creator);
                    foreach (Module.BusinessObjects.Member member in arg.AcceptActionArgs.SelectedObjects)
                    {
                        chatSession.MemberList.Add(newObjectSpace.GetObjectByKey<Module.BusinessObjects.Member>(member.Oid));
                    }
                    chatSession.Name = string.Join(", ", chatSession.MemberList.Select(m => m.Name).ToArray());
                    Module.Helpers.XafXpoHelper.CreateDialogControllerDetailView(this, null, chatSession, newObjectSpace);
                }
            };
            Module.Helpers.XafXpoHelper.PopupDialogControllerListView(this, dc, typeof(Module.BusinessObjects.Member), View.ObjectSpace,
                "IsActive", DevExpress.Data.Filtering.CriteriaOperator.Parse("IsActive and Oid <> ?", SecuritySystem.CurrentUserId), false);

            #endregion MemberChatImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 2732            Oid: 7709819e-852c-4b03-97be-2f8429c87b19
		private void SendChat_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(SendChat), "Gửi");              
      
            #region SendChatImportCode
            if (View.CurrentObject is Module.BusinessObjects.ChatSession chatSession)
            {
                var objectSpace = Application.CreateObjectSpace(typeof(Module.BusinessObjects.Chat));
                if (chatSession.Modify != null)
                {
                    // Đang sửa tin nhắn
                    var modifyChat = objectSpace.GetObjectByKey<Module.BusinessObjects.Chat>(chatSession.Modify.Oid);
                    if (modifyChat != null
                                 && !string.IsNullOrEmpty(chatSession.CreateChat)
                                 && modifyChat.Creator.Oid.Equals(SecuritySystem.CurrentUserId)
                                 )
                    {
                        modifyChat.Content = chatSession.CreateChat;
                        modifyChat.Session.CommitTransaction();

                    }
                    // Xóa Modify sau khi sửa xong
                    //chatSession.Modify = null;
                    //chatSession.CreateChat = "";
                    //if (createChatPropertyEditor != null)
                    //    createChatPropertyEditor.PropertyValue = null;
                    chatSession.CreateChat = "";
                    if (createChatPropertyEditor != null)
                        createChatPropertyEditor.ReadValue();
                    System.Threading.Tasks.Task.Run(() => _chatService.SendGroupMessageAsync(chatSession.Oid.ToString(), modifyChat.Creator.UserName, modifyChat.Content)).Wait();
                    chatSession.Reload();
                    View.Refresh();


                }
                else
                {
                    var chat = objectSpace.CreateObject<Module.BusinessObjects.Chat>();
                    chat.ChatSession = objectSpace.GetObjectByKey<Module.BusinessObjects.ChatSession>(chatSession.Oid);
                    if (!string.IsNullOrEmpty(chatSession.CreateChat))
                        chat.Content = chatSession.CreateChat;
                    if (chatSession.Reply != null)
                        chat.Reply = objectSpace.GetObjectByKey<Module.BusinessObjects.Chat>(chatSession.Reply.Oid);
                    chat.Session.CommitTransaction();
                    System.Threading.Tasks.Task.Run(() => _chatService.SendGroupMessageAsync(chatSession.Oid.ToString(), chat.Creator.UserName, chat.Content)).Wait();
                    chatSession.CreateChat = "";
                    if (createChatPropertyEditor != null)
                        createChatPropertyEditor.ReadValue();
                    chatSession.Reload();
                    View.Refresh();


                }


            }

            #endregion SendChatImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 2811            Oid: 0e18dce0-cdb7-414c-ab09-e0f62fa62550
		private void CancelChat_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(CancelChat), "Hủy");              
      
            #region CancelChatImportCode

              if (View.CurrentObject is Module.BusinessObjects.ChatSession chatSession)
        {
                chatSession.CreateChat = "";
                chatSession.Reload();
            View.Refresh();
         }
            #endregion CancelChatImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}