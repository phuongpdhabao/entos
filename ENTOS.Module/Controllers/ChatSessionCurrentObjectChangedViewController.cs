using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using ENTOS.Module.BusinessObjects;
using ListView = DevExpress.ExpressApp.ListView;
using ENTOS.Module.SystemObjects;
using System.Linq;
using System;
using System.Security.Cryptography;


namespace ENTOS.Module.Controllers
{
    public partial class ChatSessionCurrentObjectChangedViewController : ViewController<ListView>
    {

        public ChatSessionCurrentObjectChangedViewController()
        {

            TargetObjectType = typeof(Module.BusinessObjects.ChatSession);
            //TargetViewNesting = Nesting.Nested;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            View.CurrentObjectChanged += OnViewCurrentObjectChanged;
        }

        private void OnViewCurrentObjectChanged(object sender, EventArgs e)
        {
            if (View.CurrentObject is not ChatSession currentSession)
                return;

            var objectSpace = Application.CreateObjectSpace(typeof(ChatRead));


            var chatRead = objectSpace.FindObject<ChatRead>(
                DevExpress.Data.Filtering.CriteriaOperator.Parse(
                    "ChatSession.Oid = ? AND Member.Oid = ?",
                    currentSession.Oid,
                    SecuritySystem.CurrentUserId
                )
            );



            if (chatRead != null)
            {
                var currentTime = Module.Helpers.XafXpoHelper.GetDateTimeNowFromServer(chatRead.Session);
                chatRead.ReadTime = currentTime;
            }
            else
            {
                var currentMember = objectSpace.GetObjectByKey<Member>(SecuritySystem.CurrentUserId);
                var sessionInOS = objectSpace.GetObjectByKey<ChatSession>(currentSession.Oid);
                var currentTime = Module.Helpers.XafXpoHelper.GetDateTimeNowFromServer(sessionInOS.Session);
                chatRead = objectSpace.CreateObject<ChatRead>();
                chatRead.ChatSession = sessionInOS;
                chatRead.Member = currentMember;
                chatRead.ReadTime = currentTime;
            }

            // Lưu thay đổi
            chatRead.Session.CommitTransaction();

         

        }






        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View is ListView)
            {
                // Your logic here
            }
        }

        protected override void OnDeactivated()
        {
            View.CurrentObjectChanged -= OnViewCurrentObjectChanged;
            base.OnDeactivated();
        }





    }
}