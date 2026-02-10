using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace ENTOS.Module.SystemObjects
{
    public partial class CustomAudioTranscriptionOptions : OpenAI.Audio.AudioTranscriptionOptions
    {
        public CustomAudioTranscriptionOptions(OpenAI.Audio.AudioTranscriptionFormat? responseFormat, float? temperature = null, OpenAI.Audio.AudioTimestampGranularities granularities = OpenAI.Audio.AudioTimestampGranularities.Default)
        {
            ResponseFormat = responseFormat;
            Temperature = temperature;
            Granularities = granularities;
        }
    }

    [NavigationItem("Default")]
    [ModelDefault("Caption", "Nhật ký OpenAI")]
    [ImageName("ChatGPT")]
    [ModelDefault("DefaultLookupEditorMode", "AllItems")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [DefaultProperty("AIModel")]
    public partial class ChatGPTTokenUsage : XPLiteObject, INoIndexColumn     //, HbBaseObject
    {
        public ChatGPTTokenUsage(Session session) : base(session) { }
        public override void AfterConstruction() 
        {
            base.AfterConstruction();
            Oid = Guid.NewGuid();
            Update = System.DateTime.Now;
            User = Session.GetObjectByKey<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser>(SecuritySystem.CurrentUserId);
        }

        [Key(true)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Guid Oid { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Model"), ToolTip("Model")]
        [ModelDefault("AllowEdit", "False")]
        [Size(50)]
        public string AIModel
        {
            get => GetPropertyValue<string>("AIModel");
            set => SetPropertyValue<string>("AIModel", value);

        }

        private int _InputTokens;
        [DetailViewLayout(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "n0")]
        [ModelDefault("Caption", "Tokens vào")]
        [ModelDefault("AllowEdit", "False")]
        public int InputTokens
        {
            get { return _InputTokens; }
            set { SetPropertyValue(nameof(InputTokens), ref _InputTokens, value); }
        }

        private int _OutputTokens;
        [DetailViewLayout(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "n0")]
        [ModelDefault("Caption", "Tokens ra")]
        [ModelDefault("AllowEdit", "False")]
        public int OutputTokens
        {
            get { return _OutputTokens; }
            set { SetPropertyValue(nameof(OutputTokens), ref _OutputTokens, value); }
        }

        [DetailViewLayout(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [ModelDefault("Caption", "Tổng Tokens")]
        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "n0")]
        public int TotalTokens
        {
            get { return InputTokens + OutputTokens; }           
        }

        private TimeSpan? _Duration;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Thời lượng"), ToolTip("Thời lượng")]
        //[Index(17)]		
        [ModelDefault("MaskSettings", "AgAAAA9NYXNrTWFuYWdlclR5cGUAgwFEZXZFeHByZXNzLkRhdGEuTWFzay5UaW1lU3Bhbk1hc2tNYW5hZ2VyLCBEZXZFeHByZXNzLkRhdGEudjIyLjEsIFZlcnNpb249MjIuMS4zLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjg4ZDE3NTRkNzAwZTQ5YQRtYXNrBwIMW2QuXWhoOm1tOnNz")]
        public TimeSpan? Duration
        {
            get { return _Duration; }
            set { SetPropertyValue(nameof(Duration), ref _Duration, value); }

        }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Nội dung"), ToolTip("Nội dung")]
        [ModelDefault("AllowEdit", "False")]
        [Size(200)]
        public string Content
        {
            get => GetPropertyValue<string>("Content");
            set => SetPropertyValue<string>("Content", value);

        }

        [DetailViewLayout(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Cập nhật"), ToolTip("Cập nhật")]
        //[Index(12)]		
        [ModelDefault("DisplayFormat", "d/M/yyyy HH:mm")]
        [ModelDefault("EditMask", "d/M/yyyy  HH:mm")]
        [ModelDefault("AllowEdit", "False")]
        public DateTime Update
        {
            get => GetPropertyValue<DateTime>("Update");
            set => SetPropertyValue<DateTime>("Update", value);
        }

        [DetailViewLayout(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Người tạo"), ToolTip("Người tạo")]
        [ModelDefault("AllowEdit", "False")]
        public DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser User
        {
            get => GetPropertyValue<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser>("User");
            set => SetPropertyValue<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser>("User", value);
        }
    }
}

