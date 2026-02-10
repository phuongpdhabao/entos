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
	[NavigationItem("Document")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Bài viết"), ImageName("Post")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
    [ShowToolTipAttribute(TargetItems = nameof(Update))]
 
    [AllowSetDefaultAttribute(TargetItems = nameof(NameOrigin)+ "," + nameof(CreatedDate)+ "," + nameof(Member)+ "," + nameof(ContentOrigin)+ "," + nameof(Update))]
 
	[MobileColumnAttribute(Context = "Player_PostList_ListView", TargetItems = nameof(Member)+ "," + nameof(Image)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_PostList_ListView", TargetItems = nameof(Member)+ "," + nameof(Name)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Post_ListView", TargetItems = nameof(Name)+ "," + nameof(Member)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Post_LookupListView", TargetItems = nameof(Name)+ "," + nameof(Image)+ "," + nameof(Member))]
	[MobileColumnAttribute(Context = "TournamentSeason_PostList_ListView", TargetItems = nameof(Image)+ "," + nameof(Name)+ "," + nameof(Member))]
	[MobileColumnAttribute(Context = "Match_PostList_ListView", TargetItems = nameof(Member)+ "," + nameof(Name)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "PublicEvent_PostList_ListView", TargetItems = nameof(Name)+ "," + nameof(Image)+ "," + nameof(Member))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Post:  DevExpress.Xpo.XPLiteObject , INewObjectSession, IWebData , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Post(Session session)
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
				if (ObjectRelationList.IsLoaded)
                {
                    if (ObjectRelationList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ObjectRelationList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ObjectRelationList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ObjectRelation>(CriteriaOperator.Parse("[Post.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool objectrelationlist = Session.Query<Module.BusinessObjects.ObjectRelation>().Where(x => x.Post.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ObjectRelationList), objectrelationlist);
                        if (objectrelationlist)
                            return true;

                    }                    
                }				
                                
                return false;
            }
        }

        public void OnViewObjectSpaceCommitted(View view)
        {

           
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

 		[Size(250)]
		[RuleRequiredField("RequiredPostName", DefaultContexts.Save)]
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

	
       
		//private string _nameorigin;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên gốc")]
        [ToolTip("Tên gốc")]
		//[Index(1)]		

 		[Size(250)]
		public string NameOrigin
        { 
		    get => GetPropertyValue<string>("NameOrigin");                         
			set => SetPropertyValue<string>("NameOrigin", value); 
			
        }
		//Tooltip for Object
		public object NameOriginToolTipControllerText(View view)
        {
        //    if (NameOrigin != null) 
		//			return NameOrigin;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool NameOriginIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNameOrigin();
				//if (result != null && NameOrigin != null){
				//	return !NameOrigin.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _createddate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy H:mm")]
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

	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ảnh")]
        [ToolTip("Ảnh")]
		//[Index(3)]		
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

	
       
		//private string _source;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nguồn")]
        [ToolTip("Nguồn")]
		//[Index(4)]		

 		[Size(250)]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string Source
        { 
		    get => GetPropertyValue<string>("Source");                         
			set => SetPropertyValue<string>("Source", value); 
			
        }
		//Tooltip for Object
		public object SourceToolTipControllerText(View view)
        {
        //    if (Source != null) 
		//			return Source;
            return null;
        }
		//Get Default Value
        public string GetDefaultSource(View view = null)
        { 
			return Source;
        }
		//Set Default Value
		public void SetDefaultSource(View view = null)
        {
            //if (Source is null){
            //    var result = GetDefaultSource(view);
            //    if (result != null && result != Source){
			//          Source = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SourceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSource();
				//if (result != null && Source != null){
				//	return !Source.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MemberCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Member Member
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Member");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Member", value); 
			
        }
		//Tooltip for Object
		public object MemberToolTipControllerText(View view)
        {
        //    if (Member != null) 
		//			return Member;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool MemberIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMember();
				//if (result != null && Member != null){
				//	return !Member.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator MemberCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Member));
            }
        }
	
       
		//private string _content;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(6)]		

 		[Size(SizeAttribute.Unlimited)]
	    [EditorAlias("CustomHtmlPropertyEditor")]
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
                
				//var result = GetDefaultContent();
				//if (result != null && Content != null){
				//	return !Content.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _contentorigin;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nội dung gốc")]
        [ToolTip("Nội dung gốc")]
		//[Index(7)]		

 		[Size(SizeAttribute.Unlimited)]
	    [EditorAlias("CustomHtmlPropertyEditor")]
		public string ContentOrigin
        { 
		    get => GetPropertyValue<string>("ContentOrigin");                         
			set => SetPropertyValue<string>("ContentOrigin", value); 
			
        }
		//Tooltip for Object
		public object ContentOriginToolTipControllerText(View view)
        {
        //    if (ContentOrigin != null) 
		//			return ContentOrigin;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool ContentOriginIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultContentOrigin();
				//if (result != null && ContentOrigin != null){
				//	return !ContentOrigin.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
		//[Index(8)]
		[DevExpress.Xpo.Association("Post-BookMarkList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BookMark> BookMarkList
        {      
		    get => GetCollection<Module.BusinessObjects.BookMark>("BookMarkList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quan hệ")]
		//[Index(9)]
		[DevExpress.Xpo.Association("Post-ObjectRelationList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ObjectRelation> ObjectRelationList
        {      
		    get => GetCollection<Module.BusinessObjects.ObjectRelation>("ObjectRelationList"); 
			
        }
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(10)]		
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
            #region 2670ImportCode 
return Update?.ToString("H:m");
#endregion 2670ImportCode
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

	
       
		//private string _code;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(11)]		

 		[Size(20)]
		[RuleUniqueValue("UniquePostCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		public string Code
        { 
		    get => GetPropertyValue<string>("Code");                         
			set => SetPropertyValue<string>("Code", value); 
			
        }
		//Tooltip for Object
		public object CodeToolTipControllerText(View view)
        {
        //    if (Code != null) 
		//			return Code;
            return null;
        }
		//Get Default Value
        public string GetDefaultCode(View view = null)
        { 
			return Code;
        }
		//Set Default Value
		public void SetDefaultCode(View view = null)
        {
            //if (Code is null){
            //    var result = GetDefaultCode(view);
            //    if (result != null && result != Code){
			//          Code = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCode();
				//if (result != null && Code != null){
				//	return !Code.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(12)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		[RuleUniqueValue("UniquePostOrder", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		public int? Order
        { 
		    get => GetPropertyValue<int?>("Order");                         
			set => SetPropertyValue<int?>("Order", value); 
			
        }
		//Tooltip for Object
		public object OrderToolTipControllerText(View view)
        {
        //    if (Order != null) 
		//			return Order;
            return null;
        }
		//Get Default Value
        public int? GetDefaultOrder(View view = null)
        { 
			return Order;
        }
		//Set Default Value
		public void SetDefaultOrder(View view = null)
        {
            //if (Order is null){
            //    var result = GetDefaultOrder(view);
            //    if (result != null && result != Order){
			//          Order = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OrderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrder();
				//if (result != null && Order != null){
				//	return !Order.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Folder _folder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(13)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#Post# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-PostList")]
	 
		public Module.BusinessObjects.Folder Folder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Folder>("Folder");                         
			set => SetPropertyValue<Module.BusinessObjects.Folder>("Folder", value); 
			
        }
		//Tooltip for Object
		public object FolderToolTipControllerText(View view)
        {
        //    if (Folder != null) 
		//			return Folder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Folder GetDefaultFolder(View view = null)
        { 
			return Folder;
        }
		//Set Default Value
		public void SetDefaultFolder(View view = null)
        {
            //if (Folder is null){
            //    var result = GetDefaultFolder(view);
            //    if (result != null && result != Folder){
			//          Folder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFolder();
				//if (result != null && Folder != null){
				//	return !Folder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator FolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Folder));
            }
        }
	
       
		//private bool _flag;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ")]
        [ToolTip("Cờ")]
		//[Index(14)]		
	    [NonPersistent()]
	    [NotMapped()]
		public bool Flag
        { 
		    get => GetPropertyValue<bool>("Flag");                         
			set => SetPropertyValue<bool>("Flag", value); 
			
        }
		//Tooltip for Object
		public object FlagToolTipControllerText(View view)
        {
        //    if (Flag != null) 
		//			return Flag;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFlag(View view = null)
        { 
			return Flag;
        }
		//Set Default Value
		public void SetDefaultFlag(View view = null)
        {
            //if (Flag is null){
            //    var result = GetDefaultFlag(view);
            //    if (result != null && result != Flag){
			//          Flag = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FlagIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFlag();
				//if (result != null && Flag != null){
				//	return !Flag.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Match _match;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trận đấu")]
        [ToolTip("Trận đấu")]
		//[Index(15)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MatchCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Match-PostList")]
	 
		public Module.BusinessObjects.Match Match
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Match>("Match");                         
			set => SetPropertyValue<Module.BusinessObjects.Match>("Match", value); 
			
        }
		//Tooltip for Object
		public object MatchToolTipControllerText(View view)
        {
        //    if (Match != null) 
		//			return Match;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Match GetDefaultMatch(View view = null)
        { 
			return Match;
        }
		//Set Default Value
		public void SetDefaultMatch(View view = null)
        {
            //if (Match is null){
            //    var result = GetDefaultMatch(view);
            //    if (result != null && result != Match){
			//          Match = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MatchIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMatch();
				//if (result != null && Match != null){
				//	return !Match.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator MatchCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Match));
            }
        }
	
       
		//private Module.BusinessObjects.TournamentSeason _tournamentseason;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mùa giải")]
        [ToolTip("Mùa giải")]
		//[Index(16)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TournamentSeasonCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("TournamentSeason-PostList")]
	 
		public Module.BusinessObjects.TournamentSeason TournamentSeason
        { 
		    get => GetPropertyValue<Module.BusinessObjects.TournamentSeason>("TournamentSeason");                         
			set => SetPropertyValue<Module.BusinessObjects.TournamentSeason>("TournamentSeason", value); 
			
        }
		//Tooltip for Object
		public object TournamentSeasonToolTipControllerText(View view)
        {
        //    if (TournamentSeason != null) 
		//			return TournamentSeason;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.TournamentSeason GetDefaultTournamentSeason(View view = null)
        { 
			return TournamentSeason;
        }
		//Set Default Value
		public void SetDefaultTournamentSeason(View view = null)
        {
            //if (TournamentSeason is null){
            //    var result = GetDefaultTournamentSeason(view);
            //    if (result != null && result != TournamentSeason){
			//          TournamentSeason = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TournamentSeasonIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTournamentSeason();
				//if (result != null && TournamentSeason != null){
				//	return !TournamentSeason.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TournamentSeasonCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(TournamentSeason));
            }
        }
	
       
		//private Module.BusinessObjects.Player _player;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đấu thủ")]
        [ToolTip("Đấu thủ")]
		//[Index(17)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PlayerCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Player-PostList")]
	 
		public Module.BusinessObjects.Player Player
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Player>("Player");                         
			set => SetPropertyValue<Module.BusinessObjects.Player>("Player", value); 
			
        }
		//Tooltip for Object
		public object PlayerToolTipControllerText(View view)
        {
        //    if (Player != null) 
		//			return Player;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Player GetDefaultPlayer(View view = null)
        { 
			return Player;
        }
		//Set Default Value
		public void SetDefaultPlayer(View view = null)
        {
            //if (Player is null){
            //    var result = GetDefaultPlayer(view);
            //    if (result != null && result != Player){
			//          Player = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PlayerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPlayer();
				//if (result != null && Player != null){
				//	return !Player.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PlayerCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Player));
            }
        }
	
       
		//private Module.BusinessObjects.PublicEvent _publicevent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sự kiện")]
        [ToolTip("Sự kiện")]
		//[Index(18)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PublicEventCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("PublicEvent-PostList")]
	 
		public Module.BusinessObjects.PublicEvent PublicEvent
        { 
		    get => GetPropertyValue<Module.BusinessObjects.PublicEvent>("PublicEvent");                         
			set => SetPropertyValue<Module.BusinessObjects.PublicEvent>("PublicEvent", value); 
			
        }
		//Tooltip for Object
		public object PublicEventToolTipControllerText(View view)
        {
        //    if (PublicEvent != null) 
		//			return PublicEvent;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.PublicEvent GetDefaultPublicEvent(View view = null)
        { 
			return PublicEvent;
        }
		//Set Default Value
		public void SetDefaultPublicEvent(View view = null)
        {
            //if (PublicEvent is null){
            //    var result = GetDefaultPublicEvent(view);
            //    if (result != null && result != PublicEvent){
			//          PublicEvent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PublicEventIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPublicEvent();
				//if (result != null && PublicEvent != null){
				//	return !PublicEvent.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PublicEventCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PublicEvent));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 1295ImportCode
            base.AfterConstruction();
SetDefaultCreatedDate();
SetDefaultMember();
            #endregion 1295ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultNameOrigin(View view = null);
        //SetDefaultCreatedDate(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultSource(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultFlag(View view = null);
        //SetDefaultMatch(View view = null);
        //SetDefaultTournamentSeason(View view = null);
        //SetDefaultPlayer(View view = null);
        //SetDefaultPublicEvent(View view = null);
			
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
            #region 1291ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 1291ImportCode
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
            Session.Delete(this.BookMarkList);				
  
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
			//	SetDefaultContent();
			//	SetDefaultContentOrigin();
			//	SetDefaultBookMarkList();
			//	SetDefaultObjectRelationList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1304ImportCode
		public void SetDefaultNameOrigin(View view = null)
        {
            //Code: 1304            Oid: 8bacc760-6297-4e9a-9ed7-4bf3f4da5747
            if(String.IsNullOrEmpty(NameOrigin)) NameOrigin = GetDefaultNameOrigin();

        }
#endregion 1304ImportCode
#region 1306ImportCode
		public void SetDefaultContentOrigin(View view = null)
        {
            //Code: 1306            Oid: 4fddb9c7-5ef4-4544-b0bc-2d6d22af4046
            if(String.IsNullOrEmpty(ContentOrigin)) ContentOrigin = GetDefaultContentOrigin();

        }
#endregion 1306ImportCode
#region 1303ImportCode
		public string GetDefaultNameOrigin(View view = null)
        {
            //Code: 1303            Oid: ee23d7b4-f9bc-4b05-bcbb-21ecf650e144
            return Name;

        }
#endregion 1303ImportCode
#region 1293ImportCode
		public DateTime? GetDefaultCreatedDate(View view = null)
        {
            //Code: 1293            Oid: 66598e4a-2bdd-4974-8d17-6327abf67947
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1293ImportCode
#region 1292ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 1292            Oid: b7063dde-0d38-40a2-9780-42ca89efa348
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1292ImportCode
#region 1296ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 1296            Oid: d313a232-a0eb-44fc-9f8d-011a07014990
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 1296ImportCode
#region 1294ImportCode
		public void SetDefaultCreatedDate(View view = null)
        {
            //Code: 1294            Oid: c08b185c-97c1-4cf4-ac18-17edb7479d20
            if(CreatedDate == null) CreatedDate = GetDefaultCreatedDate();
        }
#endregion 1294ImportCode
#region 1290ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 1290            Oid: 55bc4bb8-f130-488c-9fad-18d93e0a604a
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 1290ImportCode
#region 1297ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 1297            Oid: 085ff789-c5d5-483c-991a-6cecd5a7f1ad
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 1297ImportCode
#region 1305ImportCode
		public string GetDefaultContentOrigin(View view = null)
        {
            //Code: 1305            Oid: 33bcb202-c98d-4270-a48d-5724c3774449
            return null;
        }
#endregion 1305ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
