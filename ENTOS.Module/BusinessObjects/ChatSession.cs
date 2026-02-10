using System;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.Xpo;
using System.Linq;
using System.Text;
using ENTOS.Module.SystemObjects;
using ENTOS.Module;
using ENTOS.Domain.Abstractions;
using ENTOS.Module.FilterControllers;


namespace ENTOS.Module.BusinessObjects 
{
	[NavigationItem("TaskManagement")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Phiên nhắn"), ImageName("ChatSession")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("ChatSession Update, Name None_None__Color [A=0, R=0, G=0, B=0]" , TargetItems = "Update, Name" , Criteria = "[NewChat] = True",AppearanceItemType = "ViewItem", Context = "ListView" , FontColor = "#000000" , FontStyle = DevExpress.Drawing.DXFontStyle.Bold )]
	[Appearance("ChatSession Name None_None__Color [A=255, R=0, G=192, B=0]" , TargetItems = "Name" , Criteria = "[NewChat] = True",AppearanceItemType = "ViewItem", Context = "DetailView" , FontColor = "#00C000" )]
	[Appearance("ChatSession Image, ChatBox, Creator, Name Hide_None__" , TargetItems = "Image, ChatBox, Creator, Name" , Criteria = "[Creator.Oid] <> CURRENTUSERID()",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(Name)+ "," + nameof(Creator)+ "," + nameof(Image)+ "," + nameof(ChatBox)+ "," + nameof(MemberList), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Creator)+ "," + nameof(ChatList)+ "," + nameof(Update)+ "," + nameof(CreatedDate))]
 
	[MobileColumnAttribute(Context = "ChatSession_ListView", TargetItems = nameof(Image)+ "," + nameof(Name)+ "," + nameof(NewChat))]
	[MobileColumnAttribute(Context = "ChatSession_LookupListView", TargetItems = nameof(Name)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Member_ChatSessionList_ListView", TargetItems = nameof(Name)+ "," + nameof(Image)+ "," + nameof(Update))]
	[DefaultProperty("Name")]
 
	[RuleCriteria("Unique.ChatSession.MemberFolder-MemberList", DefaultContexts.Save,
    "[MemberFolder] Is Null Or [MemberList].Count() = 0",
    "Tập thể và thành viên không được phép tồn tại cùng lúc")]
[OptimisticLocking(true)]
    public partial class ChatSession:  DevExpress.Xpo.XPLiteObject  , IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public ChatSession(Session session)
            : base(session) {              
        }

				public string ToolTipControllerText(View view)
        {
            var result = "";
            //if (this.Name != null)
            //{
            //    if (!string.IsNullOrEmpty(result))
            //        result += "\r\n";
            //    result += "Tiêu đề:" + Name;
            //}            
            return result;
        }
		        private System.Collections.Generic.Dictionary<string, bool> _cacheAppearanceDisableDelete;
		[Browsable(false)]
        public bool AppearanceDisableDelete
        {
            get
            {

                if (Session.IsNewObject(this))
                    return false;
                                
                return false;
            }
        }

        public void OnViewObjectSpaceCommitted(View view)
        {

            //Code: 2788            Oid: d12d6753-9aa9-4648-8345-ca1865b299b1
            /* if (MemberFolder != null)
{
    var members = new System.Collections.Generic.List<Member>();
    var membersInFolder = MemberFolder.GetTotalMemberList(MemberFolder, members);
    // Gộp với MemberList và loại bỏ trùng
    var totalMembers = membersInFolder
        .Concat(MemberList)
        .Distinct()
        .ToList()
        .Where(m => m.AppearanceDisableDelete == true);

    if (totalMembers != null && Name != null)
    {
        foreach (var member in totalMembers)
        {
            if (member != null && !member.Oid.Equals(SecuritySystem.CurrentUserId))
            {
                CreateMemberNotify(member.Oid);
            }
        }
    }
}
else if (MemberList != null)
{
    foreach (var member in MemberList)
    {
         if (member != null && !member.Oid.Equals(SecuritySystem.CurrentUserId))
        {
            CreateMemberNotify(member.Oid);
        }
    }

}
 */
           
        }
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)

		[Key(true)]
		[VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]     
        public Guid Oid { get; set; }
               

		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(0)]		

 		[Size(150)]
		public string Name
        { 
		    get => GetPropertyValue<string>("Name");                         
			set => SetPropertyValue<string>("Name", value); 
			
        }
		//Tooltip for Object
		public object NameToolTipControllerText(View view)
        {
        //    if (Name != null) 
		//			return Name;
            return null;
        }
		//Get Default Value
        public string GetDefaultName(View view = null)
        { 
			return Name;
        }
		//Set Default Value
		public void SetDefaultName(View view = null)
        {
            //if (Name is null){
            //    var result = GetDefaultName(view);
            //    if (result != null && result != Name){
			//          Name = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultName();
				//if (result != null && Name != null){
				//	return !Name.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Member _creator;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(1)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CreatorCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Member Creator
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Creator");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Creator", value); 
			
        }
		//Tooltip for Object
		public object CreatorToolTipControllerText(View view)
        {
        //    if (Creator != null) 
		//			return Creator;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool CreatorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCreator();
				//if (result != null && Creator != null){
				//	return !Creator.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CreatorCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Creator));
            }
        }
	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ảnh")]
        [ToolTip("Ảnh")]
		//[Index(2)]		
		[Appearance("ẢnhBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Image
        { 
		    get => GetPropertyValue<byte[]>("Image");                         
			set => SetPropertyValue<byte[]>("Image", value); 
			
        }
		//Tooltip for Object
		public object ImageToolTipControllerText(View view)
        {
        //    if (Image != null) 
		//			return Image;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultImage(View view = null)
        { 
			return Image;
        }
		//Set Default Value
		public void SetDefaultImage(View view = null)
        {
            //if (Image is null){
            //    var result = GetDefaultImage(view);
            //    if (result != null && result != Image){
			//          Image = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ImageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImage();
				//if (result != null && Image != null){
				//	return !Image.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.MemberFolder _memberfolder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MemberFolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.MemberFolder MemberFolder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.MemberFolder>("MemberFolder");                         
			set => SetPropertyValue<Module.BusinessObjects.MemberFolder>("MemberFolder", value); 
			
        }
		//Tooltip for Object
		public object MemberFolderToolTipControllerText(View view)
        {
        //    if (MemberFolder != null) 
		//			return MemberFolder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.MemberFolder GetDefaultMemberFolder(View view = null)
        { 
			return MemberFolder;
        }
		//Set Default Value
		public void SetDefaultMemberFolder(View view = null)
        {
            //if (MemberFolder is null){
            //    var result = GetDefaultMemberFolder(view);
            //    if (result != null && result != MemberFolder){
			//          MemberFolder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MemberFolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMemberFolder();
				//if (result != null && MemberFolder != null){
				//	return !MemberFolder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator MemberFolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(MemberFolder));
            }
        }
	
       
		//private bool _chatbox;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hộp tin")]
        [ToolTip("Hộp tin")]
		//[Index(4)]		
		public bool ChatBox
        { 
		    get => GetPropertyValue<bool>("ChatBox");                         
			set => SetPropertyValue<bool>("ChatBox", value); 
			
        }
		//Tooltip for Object
		public object ChatBoxToolTipControllerText(View view)
        {
        //    if (ChatBox != null) 
		//			return ChatBox;
            return null;
        }
		//Get Default Value
        public bool GetDefaultChatBox(View view = null)
        { 
			return ChatBox;
        }
		//Set Default Value
		public void SetDefaultChatBox(View view = null)
        {
            //if (ChatBox is null){
            //    var result = GetDefaultChatBox(view);
            //    if (result != null && result != ChatBox){
			//          ChatBox = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ChatBoxIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultChatBox();
				//if (result != null && ChatBox != null){
				//	return !ChatBox.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chat")]
		//[Index(5)]
		//[DevExpress.Xpo.Association]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Chat> ChatList
        {      

                #region 2730ImportCode 
            get
            {
                SetDefaultChatList();
                return _chatList;
            }
            set
            {                
                _chatList = value;                
            }
        }
        private XPCollection<Module.BusinessObjects.Chat> _chatList = null;
        private void SetDefaultChatList()
        {
            if (_chatList == null)
            {
                _chatList = new XPCollection<Module.BusinessObjects.Chat>(Session, CriteriaOperator.Parse("ChatSession.Oid = ?", Oid));
                SetDefaultChatListChatRead();
            }
			
        }

        public void SetDefaultChatListChatRead()
        {
            var user = Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
            var chatRead = Session.FindObject<ChatRead>(CriteriaOperator.Parse("Member.Oid = ? AND ChatSession.Oid = ?", user.Oid, Oid));
            if (chatRead != null && chatRead.ReadTime != null)
            {
                var nonReads = new XPCollection<Module.BusinessObjects.Chat>(_chatList, CriteriaOperator.Parse("Update >= ? AND Creator.Oid != ?", chatRead.ReadTime, user.Oid));
                if(nonReads.Count > 0)
                {
                    if (_newChat is null || !_newChat.Value)
                        _newChat = true;
                    foreach (var chat in nonReads)
                    {
                        chat.NewChat = true;
                    }
                }
                
            }
#endregion 2730ImportCode
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thành viên")]
		//[Index(6)]
		[DataSourceCriteria("Not ChatSessionList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("ChatSessionList-MemberList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Member> MemberList
        {      
		    get => GetCollection<Module.BusinessObjects.Member>("MemberList"); 
			
        }
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(7)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy H:mm")]
		[ModelDefault("EditMask", "d/M/yyyy H:mm")]
	    [ModelDefault("AllowEdit", "False")]
		public DateTime? Update
        { 
		    get => GetPropertyValue<DateTime?>("Update");                         
			set => SetPropertyValue<DateTime?>("Update", value); 
			
        }
		//Tooltip for Object
		public object UpdateToolTipControllerText(View view)
        {
        //    if (Update != null) 
		//			return Update;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool UpdateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpdate();
				//if (result != null && Update != null){
				//	return !Update.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _createddate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày tạo")]
        [ToolTip("Ngày tạo")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy H:mm")]
	    [ModelDefault("AllowEdit", "False")]
		public DateTime? CreatedDate
        { 
		    get => GetPropertyValue<DateTime?>("CreatedDate");                         
			set => SetPropertyValue<DateTime?>("CreatedDate", value); 
			
        }
		//Tooltip for Object
		public object CreatedDateToolTipControllerText(View view)
        {
        //    if (CreatedDate != null) 
		//			return CreatedDate;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool CreatedDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCreatedDate();
				//if (result != null && CreatedDate != null){
				//	return !CreatedDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _newchat;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Có tin mới")]
        [ToolTip("Có tin mới")]
		//[Index(9)]		
	    [NotMapped()]
	    [NonPersistent()]
		public bool NewChat
        { 
		    #region 2776ImportCode 
    get
    {
        if (_newChat is null)
            SetNewChatValue();
       return _newChat.Value;

    }
}
private bool? _newChat = null;
private void SetNewChatValue()
{
    _newChat = GetDefaultNewChat(); ;
#endregion 2776ImportCode
			
        }
		//Tooltip for Object
		public object NewChatToolTipControllerText(View view)
        {
        //    if (NewChat != null) 
		//			return NewChat;
            return null;
        }
		//Get Default Value
		//Set Default Value
		public void SetDefaultNewChat(View view = null)
        {
            //if (NewChat is null){
            //    var result = GetDefaultNewChat(view);
            //    if (result != null && result != NewChat){
			//          NewChat = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NewChatIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNewChat();
				//if (result != null && NewChat != null){
				//	return !NewChat.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _readtime;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thời gian đọc")]
        [ToolTip("Thời gian đọc")]
		//[Index(10)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy H:mm")]
		[ModelDefault("EditMask", "d/M/yyyy")]
	    [NotMapped()]
	    [NonPersistent()]
		public DateTime? ReadTime
        { 
		    get => GetPropertyValue<DateTime?>("ReadTime");                         
			set => SetPropertyValue<DateTime?>("ReadTime", value); 
			
        }
		//Tooltip for Object
		public object ReadTimeToolTipControllerText(View view)
        {
        //    if (ReadTime != null) 
		//			return ReadTime;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultReadTime(View view = null)
        { 
			return ReadTime;
        }
		//Set Default Value
		public void SetDefaultReadTime(View view = null)
        {
            //if (ReadTime is null){
            //    var result = GetDefaultReadTime(view);
            //    if (result != null && result != ReadTime){
			//          ReadTime = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ReadTimeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultReadTime();
				//if (result != null && ReadTime != null){
				//	return !ReadTime.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _createchat;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tin nhắn")]
        [ToolTip("Tin nhắn")]
		//[Index(11)]		

 		[Size(250)]
	    [NotMapped()]
	    [NonPersistent()]
		public string CreateChat
        { 
		    #region 2777ImportCode 
get;set;
#endregion 2777ImportCode
			
        }
		//Tooltip for Object
		public object CreateChatToolTipControllerText(View view)
        {
        //    if (CreateChat != null) 
		//			return CreateChat;
            return null;
        }
		//Get Default Value
        public string GetDefaultCreateChat(View view = null)
        { 
			return CreateChat;
        }
		//Set Default Value
		public void SetDefaultCreateChat(View view = null)
        {
            //if (CreateChat is null){
            //    var result = GetDefaultCreateChat(view);
            //    if (result != null && result != CreateChat){
			//          CreateChat = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CreateChatIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCreateChat();
				//if (result != null && CreateChat != null){
				//	return !CreateChat.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Chat _reply;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trả lời")]
        [ToolTip("Trả lời")]
		//[Index(12)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ReplyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [ImmediatePostData()]
	    [Browsable(false)]
	    [NonPersistent()]
	    [NotMapped()]
		public Module.BusinessObjects.Chat Reply
        { 
		    #region 2809ImportCode 
get; set;
#endregion 2809ImportCode
			
        }
		//Tooltip for Object
		public object ReplyToolTipControllerText(View view)
        {
        //    if (Reply != null) 
		//			return Reply;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Chat GetDefaultReply(View view = null)
        { 
			return Reply;
        }
		//Set Default Value
		public void SetDefaultReply(View view = null)
        {
            //if (Reply is null){
            //    var result = GetDefaultReply(view);
            //    if (result != null && result != Reply){
			//          Reply = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ReplyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultReply();
				//if (result != null && Reply != null){
				//	return !Reply.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ReplyCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Reply));
            }
        }
	
       
		//private Module.BusinessObjects.Chat _modify;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chỉnh sửa")]
        [ToolTip("Chỉnh sửa")]
		//[Index(13)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ModifyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [NonPersistent()]
	    [NotMapped()]
	    [Browsable(false)]
	    [ImmediatePostData()]
		public Module.BusinessObjects.Chat Modify
        { 
		    #region 2810ImportCode 
get; set;
#endregion 2810ImportCode
			
        }
		//Tooltip for Object
		public object ModifyToolTipControllerText(View view)
        {
        //    if (Modify != null) 
		//			return Modify;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Chat GetDefaultModify(View view = null)
        { 
			return Modify;
        }
		//Set Default Value
		public void SetDefaultModify(View view = null)
        {
            //if (Modify is null){
            //    var result = GetDefaultModify(view);
            //    if (result != null && result != Modify){
			//          Modify = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ModifyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultModify();
				//if (result != null && Modify != null){
				//	return !Modify.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ModifyCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Modify));
            }
        }
	
       
        private bool _display;
        [Browsable(false)]
        [NonPersistent]
        [ImmediatePostData]
        public bool Display
        {
            get { return _display; }
            set { SetPropertyValue("Display", ref _display, value); }
        }
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 1010ImportCode
            base.AfterConstruction();
SetDefaultCreatedDate();
SetDefaultCreator();
            #endregion 1010ImportCode
            Display = true;
 
        //SetDefaultName(View view = null);
        //SetDefaultCreator(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultChatBox(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultCreatedDate(View view = null);
        //SetDefaultNewChat(View view = null);
        //SetDefaultReadTime(View view = null);
        //SetDefaultCreateChat(View view = null);
        //SetDefaultReply(View view = null);
        //SetDefaultModify(View view = null);
			
        }
        
        protected override void OnLoading()
        {
            base.OnLoading();
        }
        
        protected override void OnLoaded()
        {
            base.OnLoaded();
        }

        private bool alreadySaving = false;        
        protected override void OnSaving()
        {
            #region 1014ImportCode
            base.OnSaving();
         
     SetDefaultUpdate();
            #endregion 1014ImportCode
//            Update = (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
    		if (!(Session is NestedUnitOfWork)&& (Session.DataLayer != null))
            {
   //             if (Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
   //             {
   //                 //Khi đang mở Object
   //             }
   //             else if ((Session.ObjectLayer is DevExpress.Xpo.SimpleObjectLayer))
   //             {
   //                 //Từ popup form con về form chính
   //             }
             }
        }
        
        protected override void OnSaved()
        {
             base.OnSaved();
        }

        protected override void OnDeleting()
        {
             base.OnDeleting();
  
        }

        protected override void OnDeleted()
        {
             base.OnDeleted();
            
        }

		protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsSaving)
            {

                  
            }
        }

   


		//protected override XPCollection<T> CreateCollection<T>(DevExpress.Xpo.Metadata.XPMemberInfo property)
        //{
        //    var collection = base.CreateCollection<T>(property);
        //    collection.ListChanged += OnItemListChanged;
        //    return collection;
        //}

        //private void OnItemListChanged(object sender, ListChangedEventArgs e)
        //{            
            //if (e.ListChangedType == ListChangedType.ItemAdded)
            //{
			//	SetDefaultChatList();
			//	SetDefaultMemberList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1012ImportCode
		public Module.BusinessObjects.Member GetDefaultCreator(View view = null)
        {
            //Code: 1012            Oid: ed7921c5-c3a2-42b8-8520-0f1aeeea0413
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);

        }
#endregion 1012ImportCode
#region 1015ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 1015            Oid: e6f47154-f1a4-45dd-8531-8e7dc273b949
                       //Code: 1015            Oid: e6f47154-f1a4-45dd-8531-8e7dc273b949
           var check = GetDefaultNewChat();
           var newChat = Session.Query<Chat>()
           .Where(c => c.ChatSession.Oid == Oid)
           .OrderByDescending(c => c.Update)
           .FirstOrDefault();
           if (check = true && newChat != null)
           {

               return newChat.Update.Value;


           }
           else
           {
               return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);

           }
        }
#endregion 1015ImportCode
#region 1011ImportCode
		public void SetDefaultCreator(View view = null)
        {
            //Code: 1011            Oid: 1a11373d-f052-47fd-9add-3cbc2d803402
            if(Creator == null) Creator = GetDefaultCreator();

        }
#endregion 1011ImportCode
#region 1009ImportCode
		public void SetDefaultCreatedDate(View view = null)
        {
            //Code: 1009            Oid: 002ecfdd-19d0-4b31-9201-8ca5e0b5cb8b
            if(CreatedDate == null) CreatedDate = GetDefaultCreatedDate();
        }
#endregion 1009ImportCode
#region 1008ImportCode
		public DateTime? GetDefaultCreatedDate(View view = null)
        {
            //Code: 1008            Oid: 9d06cebb-6498-4bdf-aa6e-819b41031a3b
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1008ImportCode
#region 2662ImportCode
		public bool GetDefaultNewChat(View view = null)
        {
            //Code: 2662            Oid: b1e64e5d-cddd-4ea6-a1c1-ce864e4c24d8
             
       // Lấy thông tin ChatRead của người dùng hiện tại trong ChatSession
       var chatRead = Session.FindObject<ChatRead>(CriteriaOperator.Parse(
           "Member.Oid = ? AND ChatSession.Oid = ?",
           SecuritySystem.CurrentUserId,
           Oid
       ));

       // Nếu không tìm thấy ChatRead, trả về false
       if (chatRead == null || chatRead.ReadTime == null)
       {
           return true;
       }


       var chats = Session.Query<Chat>()
          .Where(c => c.ChatSession.Oid == Oid && c.Update > chatRead.ReadTime)
          .ToList();
       bool hasOtherMemberChats = chats.Any(c => c.Creator != null && !c.Creator.Oid.Equals(SecuritySystem.CurrentUserId));

       return hasOtherMemberChats;
        }
#endregion 2662ImportCode
#region 1013ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 1013            Oid: 0169a34a-77e6-49ad-9065-c16047ba840f
            Update = GetDefaultUpdate();
        }
#endregion 1013ImportCode
        #endregion
//Mã nguồn bổ sung
#region ChatSessionImportCode
   private void CreateMemberNotify(Guid userId)
   {
       if (string.IsNullOrEmpty(Name) || userId == Guid.Empty)
           return;
       var now = Module.Helpers.XafXpoHelper.GetDateTimeNowFromServer(Session);
       var alarmTime = new DateTime(2000, 1, 1, now.Hour, now.Minute, 0); // chỉ lấy giờ phút
       var userNotify = new Module.SystemObjects.UserNotifications(Session)
       {
           AlarmTime = alarmTime,
           ObjectId = Oid,
           ObjectType = this.GetType().FullName,
           CurrentUserId = userId,
           Subject = $"Phiên nhắn mới: {Name}"
       };
   }

[Browsable(false)]
[NonPersistent()]
[NotMapped()]
public DevExpress.ExpressApp.Editors.PropertyEditor createChatPropertyEditor = null;
#endregion ChatSessionImportCode
		 		 
    }
}
