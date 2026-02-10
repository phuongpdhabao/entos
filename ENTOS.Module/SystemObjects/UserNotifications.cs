using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace ENTOS.Module.SystemObjects
{
    [ModelDefault("Caption", "Thông báo")]
    [ImageName("Action_Bell")]
    //[ModelDefault("DefaultLookupEditorMode", "AllItemsWithSearch")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Disable edit Content", TargetItems = "Content", Criteria = "VtvNguoiDung.Oid <> CurrentUserId()", Enabled = false)]
    //[Appearance("Readed Status", TargetItems = "NotificationMessage, AlarmTime", Criteria = "Readed = True", FontColor = "Red")]
    //[Appearance("ValueHasTax Valided", TargetItems = "ValueHasTax", Criteria = "ValueHasTaxIsNotValided", FontColor = "Red")]	
    //[DefaultClassOptions]
    //[DefaultProperty("Code")]
    //[OptimisticLocking(false)]
    public class UserNotifications : XPLiteObject, ISupportNotifications
    {
        private DateTime? alarmTime;

        public UserNotifications(Session session)
            : base(session)
        {
        }

        [Key(true)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Guid Oid { get; set; }

        //HB Edit 
        [DetailViewLayout(LayoutColumnPosition.Left, " ", 0)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [ModelDefault("Caption", "Người dùng")]
        [ToolTip("Nhân viên")]
        [ModelDefault("AllowEdit", "False")]
        public Guid CurrentUserId { get; set; }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [ImmediatePostData]
        public bool Readed { get; set; }

        [ModelDefault("Caption", "Loại đối tượng")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string ObjectType { get; set; }

        [ModelDefault("Caption", "Đối tượng")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [NoForeignKey]
        public Guid ObjectId { get; set; }

        [DetailViewLayout(LayoutColumnPosition.Left, " ", 0)]
        [VisibleInDetailView(false)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [ModelDefault("Caption", "Thời gian")]
        [ToolTip("Thời gian")]
        [ModelDefault("DisplayFormat", "d/MM/yyyy H:mm")]
        [ModelDefault("EditMask", "d/MM/yyyy H:mm")]
        public DateTime DueDate { get; set; }

        //End HB Edit

        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [ModelDefault("Caption", "Tiêu đề")]
        [ToolTip("Tiêu đề")]
        [ModelDefault("AllowEdit", "False")]
        [Size(500)]
        public string Subject { get; set; }

        public TimeSpan? RemindIn { get; set; }

        [Browsable(false)]
        public object UniqueId
        {
            get { return Oid; }
        }

        [Browsable(false)]
        public DateTime? AlarmTime
        {
            get { return alarmTime; }
            set
            {
                SetPropertyValue("AlarmTime", ref alarmTime, value);
                if (value == null)
                {
                    RemindIn = null;
                    IsPostponed = false;
                }
            }
        }

        [Browsable(false)]
        public string NotificationMessage
        {
            get { return Subject; }
        }

        [Browsable(false)]
        public bool IsPostponed { get; set; }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Oid = Guid.NewGuid();
            DueDate = DateTime.Now;
            //AlarmTime = DateTime.Now + TimeSpan.FromSeconds(1);
            Readed = false;
            RemindIn = TimeSpan.FromSeconds(1);
            CurrentUserId = (Guid)SecuritySystem.CurrentUserId;
        }

        public Type GetObjectType()
        {
            if (!string.IsNullOrEmpty(ObjectType))
                return Type.GetType(ObjectType);
            return null;
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (RemindIn.HasValue)
            {
                if (AlarmTime == null || AlarmTime < DueDate - RemindIn.Value)
                    AlarmTime = DueDate - RemindIn.Value;
            }
            else
            {
                AlarmTime = null;
            }
            if (AlarmTime == null)
            {
                RemindIn = null;
                IsPostponed = false;
            }
        }
    }
}