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
    [ModelDefault("Caption", "Tin nhắn"), ImageName("Chat")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("Chat Image, Update, FileUpload, Note, ObjectType, ObjectID, NewChat, CreatedDate, Creator, Content, ChatSession, Reply None_None__" , TargetItems = "Image, Update, FileUpload, Note, ObjectType, ObjectID, NewChat, CreatedDate, Creator, Content, ChatSession, Reply" , Criteria = "[NewChat] = True",AppearanceItemType = "ViewItem", Context = "ListView" , FontStyle = DevExpress.Drawing.DXFontStyle.Bold )]
	[Appearance("Chat Reply None_None__Color [A=255, R=0, G=0, B=255]" , TargetItems = "Reply" , Criteria = "[Reply] Is Not Null",AppearanceItemType = "ViewItem", FontColor = "#0000FF" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
    [ShowToolTipAttribute(TargetItems = nameof(Creator))]
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(Creator)+ "," + nameof(CreatedDate))]
 
	[MobileColumnAttribute(Context = "Chat_LookupListView", TargetItems = nameof(Content)+ "," + nameof(SystemType)+ "," + nameof(Update)+ "," + nameof(FileUpload))]
	[MobileColumnAttribute(Context = "Chat_ListView", TargetItems = nameof(FileUpload)+ "," + nameof(Content)+ "," + nameof(Update)+ "," + nameof(SystemType))]
	[MobileColumnAttribute(Context = "MemberFolder_ChatList_ListView", TargetItems = nameof(Content)+ "," + nameof(SystemType)+ "," + nameof(Update)+ "," + nameof(FileUpload))]
	[MobileColumnAttribute(Context = "ChatSession_ChatList_ListView", TargetItems = nameof(Update)+ "," + nameof(SystemType)+ "," + nameof(FileUpload)+ "," + nameof(Content))]
	[DefaultProperty("Content")]
 
[OptimisticLocking(true)]
    public partial class Chat:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Chat(Session session)
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

            //Code: 2812            Oid: 9d79f93a-da8a-4385-a4be-4c0f60708022
                 if (
    ChatSession != null
		&&
       ChatSession.MemberList.Count == 1
       &&
       ChatSession.MemberFolder == null
       &&
       !string.IsNullOrEmpty(Content))
   {
       bool shouldNotify = true;
       int timeThresholdSeconds = 10;

       var oldChat = GetLatestChat(ChatSession);

       if (oldChat != null && oldChat.CreatedDate.HasValue)
       {
           var timeDiff = (DateTime.Now - oldChat.CreatedDate.Value).TotalSeconds;

           if (timeDiff < timeThresholdSeconds)
           {
               shouldNotify = false;
           }
       }

       if (shouldNotify)
       {
           foreach (var member in ChatSession.MemberList)
           {
               if (member != null && !member.Oid.Equals(SecuritySystem.CurrentUserId))
               {
                   CreateMemberNotify(member.Oid);
               }
               else
               {
                   CreateMemberNotify(ChatSession.Creator.Oid);

               }
           }
       }
   }
           
        }
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)

		[Key(true)]
		[VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]     
        public Guid Oid { get; set; }
               

		//private string _content;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(0)]		

 		[Size(250)]
	    [ModelDefault("PropertyEditorType", "MemoEditStringPropertyEditor")]
	    [ModelDefault("RowCount","-1")]
		public string Content
        { 
		    get => GetPropertyValue<string>("Content");                         
			set => SetPropertyValue<string>("Content", value); 
			
        }
		//Tooltip for Object
		public object ContentToolTipControllerText(View view)
        {
        //    if (Content != null) 
		//			return Content;
            return null;
        }
		//Get Default Value
        public string GetDefaultContent(View view = null)
        { 
			return Content;
        }
		//Set Default Value
		public void SetDefaultContent(View view = null)
        {
            //if (Content is null){
            //    var result = GetDefaultContent(view);
            //    if (result != null && result != Content){
			//          Content = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ContentIsNotValidate
        {
            get
            {
			#region 2778ImportCode 
//ghi vao day
#endregion 2778ImportCode                
   
                return false;
            }
        }

	
       
		//private FileAttachment _fileupload;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Đính kèm")]
        [ToolTip("Đính kèm")]
		//[Index(1)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(FileUploadCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public FileAttachment FileUpload
        { 
		    get => GetPropertyValue<FileAttachment>("FileUpload");                         
			set => SetPropertyValue<FileAttachment>("FileUpload", value); 
			
        }
		//Tooltip for Object
		public object FileUploadToolTipControllerText(View view)
        {
        //    if (FileUpload != null) 
		//			return FileUpload;
            return null;
        }
		//Get Default Value
        public FileAttachment GetDefaultFileUpload(View view = null)
        { 
			return FileUpload;
        }
		//Set Default Value
		public void SetDefaultFileUpload(View view = null)
        {
            //if (FileUpload is null){
            //    var result = GetDefaultFileUpload(view);
            //    if (result != null && result != FileUpload){
			//          FileUpload = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FileUploadIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFileUpload();
				//if (result != null && FileUpload != null){
				//	return !FileUpload.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator FileUploadCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(FileUpload));
            }
        }
	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
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

	
       
		//private string _note;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(3)]		

 		[Size(SizeAttribute.Unlimited)]
	    [EditorAlias("CustomHtmlPropertyEditor")]
		public string Note
        { 
		    get => GetPropertyValue<string>("Note");                         
			set => SetPropertyValue<string>("Note", value); 
			
        }
		//Tooltip for Object
		public object NoteToolTipControllerText(View view)
        {
        //    if (Note != null) 
		//			return Note;
            return null;
        }
		//Get Default Value
        public string GetDefaultNote(View view = null)
        { 
			return Note;
        }
		//Set Default Value
		public void SetDefaultNote(View view = null)
        {
            //if (Note is null){
            //    var result = GetDefaultNote(view);
            //    if (result != null && result != Note){
			//          Note = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NoteIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNote();
				//if (result != null && Note != null){
				//	return !Note.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
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

	
       
		//private Module.BusinessObjects.Member _creator;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Người tạo")]
        [ToolTip("Người tạo")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CreatorCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [ModelDefault("AllowEdit", "False")]
		public Module.BusinessObjects.Member Creator
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Creator");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Creator", value); 
			
        }
		//Tooltip for Object
		public object CreatorToolTipControllerText(View view)
        {
            #region 1025ImportCode 
if( Reply !=null && Reply.Creator != null && Reply.Content != null)
return Reply.Creator.Name + Reply.Content;
#endregion 1025ImportCode
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
	
       
		//private DateTime? _createddate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày tạo")]
        [ToolTip("Ngày tạo")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "d/M h:mm")]
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

	
       
		//private Module.BusinessObjects.Chat _reply;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trả lời")]
        [ToolTip("Trả lời")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ReplyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Chat Reply
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Chat>("Reply");                         
			set => SetPropertyValue<Module.BusinessObjects.Chat>("Reply", value); 
			
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
	
       
		//private Module.BusinessObjects.ChatSession _chatsession;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Phiên tin")]
        [ToolTip("Phiên tin")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ChatSessionCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.ChatSession ChatSession
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ChatSession>("ChatSession");                         
			set => SetPropertyValue<Module.BusinessObjects.ChatSession>("ChatSession", value); 
			
        }
		//Tooltip for Object
		public object ChatSessionToolTipControllerText(View view)
        {
        //    if (ChatSession != null) 
		//			return ChatSession;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ChatSession GetDefaultChatSession(View view = null)
        { 
			return ChatSession;
        }
		//Set Default Value
		public void SetDefaultChatSession(View view = null)
        {
            //if (ChatSession is null){
            //    var result = GetDefaultChatSession(view);
            //    if (result != null && result != ChatSession){
			//          ChatSession = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ChatSessionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultChatSession();
				//if (result != null && ChatSession != null){
				//	return !ChatSession.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ChatSessionCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ChatSession));
            }
        }
	
       
		//private Module.BusinessObjects.MemberFolder _memberfolder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MemberFolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("MemberFolder-ChatList")]
	 
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
	
       
		//private System.Type _systemtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Kiểu hệ thống")]
        [ToolTip("Kiểu hệ thống")]
		//[Index(10)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SystemTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public System.Type SystemType
        { 
		    get => GetPropertyValue<System.Type>("SystemType");                         
			set => SetPropertyValue<System.Type>("SystemType", value); 
			
        }
		//Tooltip for Object
		public object SystemTypeToolTipControllerText(View view)
        {
        //    if (SystemType != null) 
		//			return SystemType;
            return null;
        }
		//Get Default Value
        public System.Type GetDefaultSystemType(View view = null)
        { 
			return SystemType;
        }
		//Set Default Value
		public void SetDefaultSystemType(View view = null)
        {
            //if (SystemType is null){
            //    var result = GetDefaultSystemType(view);
            //    if (result != null && result != SystemType){
			//          SystemType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SystemTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSystemType();
				//if (result != null && SystemType != null){
				//	return !SystemType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SystemTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SystemType));
            }
        }
	
       
		//private System.Guid _objectid;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã đối tượng")]
        [ToolTip("Mã đối tượng")]
		//[Index(11)]		
		public System.Guid ObjectID
        { 
		    get => GetPropertyValue<System.Guid>("ObjectID");                         
			set => SetPropertyValue<System.Guid>("ObjectID", value); 
			
        }
		//Tooltip for Object
		public object ObjectIDToolTipControllerText(View view)
        {
        //    if (ObjectID != null) 
		//			return ObjectID;
            return null;
        }
		//Get Default Value
        public System.Guid GetDefaultObjectID(View view = null)
        { 
			return ObjectID;
        }
		//Set Default Value
		public void SetDefaultObjectID(View view = null)
        {
            //if (ObjectID is null){
            //    var result = GetDefaultObjectID(view);
            //    if (result != null && result != ObjectID){
			//          ObjectID = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ObjectIDIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultObjectID();
				//if (result != null && ObjectID != null){
				//	return !ObjectID.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _newchat;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mới")]
        [ToolTip("Mới")]
		//[Index(12)]		
	    [NotMapped()]
	    [NonPersistent()]
		public bool NewChat
        { 
		    get => GetPropertyValue<bool>("NewChat");                         
			set => SetPropertyValue<bool>("NewChat", value); 
			
        }
		//Tooltip for Object
		public object NewChatToolTipControllerText(View view)
        {
        //    if (NewChat != null) 
		//			return NewChat;
            return null;
        }
		//Get Default Value
        public bool GetDefaultNewChat(View view = null)
        { 
			return NewChat;
        }
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 1019ImportCode
            base.AfterConstruction();
SetDefaultCreatedDate();
SetDefaultCreator();
            #endregion 1019ImportCode
 
        //SetDefaultContent(View view = null);
        //SetDefaultFileUpload(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultCreator(View view = null);
        //SetDefaultCreatedDate(View view = null);
        //SetDefaultReply(View view = null);
        //SetDefaultChatSession(View view = null);
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultSystemType(View view = null);
        //SetDefaultObjectID(View view = null);
        //SetDefaultNewChat(View view = null);
			
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
            #region 1021ImportCode
            base.OnSaving();

          SetDefaultUpdate();
            #endregion 1021ImportCode
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

                switch (propertyName)
                {       
				
                    case nameof(ChatSession):
                        OnChangedChatSession(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedChatSession(object oldValue, object newValue)
        {
            #region 2659ImportCode
            if (newValue is null) return;
//viết code tạ thông báo            
            #endregion 2659ImportCode
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
			//	SetDefaultNote();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1023ImportCode
		public void SetDefaultCreator(View view = null)
        {
            //Code: 1023            Oid: fac298ca-e30d-474c-b512-6d0c21a063d6
            if(Creator == null) Creator = GetDefaultCreator();
        }
#endregion 1023ImportCode
#region 1020ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 1020            Oid: fec3dcad-d6fa-4371-8527-75e16c8eb15d
            Update = GetDefaultUpdate();
        }
#endregion 1020ImportCode
#region 1018ImportCode
		public void SetDefaultCreatedDate(View view = null)
        {
            //Code: 1018            Oid: f655e34d-cbcc-4cde-abc8-9a0761813461
            if(CreatedDate == null) CreatedDate = GetDefaultCreatedDate();
        }
#endregion 1018ImportCode
#region 1017ImportCode
		public DateTime? GetDefaultCreatedDate(View view = null)
        {
            //Code: 1017            Oid: bf07586e-c4a5-413f-943a-7cd03810e5c3
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1017ImportCode
#region 1022ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 1022            Oid: 1c474672-6ac5-4346-b10a-c4463f81c494
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1022ImportCode
#region 1024ImportCode
		public Module.BusinessObjects.Member GetDefaultCreator(View view = null)
        {
            //Code: 1024            Oid: 3c5f7a18-8de2-4165-9626-80c0487f0bf2
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);

        }
#endregion 1024ImportCode
        #endregion
//Mã nguồn bổ sung
#region ChatImportCode
       public Chat GetLatestChat(ChatSession chatSession)
       {
           return Session.Query<Chat>()
               .Where(c => c.ChatSession == chatSession)
               .OrderByDescending(c => c.CreatedDate)
               .FirstOrDefault();
       }


       private void CreateMemberNotify(Guid userId)
       {
           if (string.IsNullOrEmpty(Content) || userId == Guid.Empty)
               return;
           var now = Module.Helpers.XafXpoHelper.GetDateTimeNowFromServer(Session);
           var alarmTime = new DateTime(2000, 1, 1, now.Hour, now.Minute, 0); 
           var userNotify = new Module.SystemObjects.UserNotifications(Session)
           {
               AlarmTime = alarmTime,
               ObjectId = Oid,
               ObjectType = this.GetType().FullName,
               CurrentUserId = userId,
               Subject = $"Nội dung: {Content}"
           };
       }
#endregion ChatImportCode
		 		 
    }
}
