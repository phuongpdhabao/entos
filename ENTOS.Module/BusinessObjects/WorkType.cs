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
    [ModelDefault("Caption", "Loại công việc"), ImageName("WorkType")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(BookMarkList)+ "," + nameof(MemberList), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Code)+ "," + nameof(Member)+ "," + nameof(Update))]
 
	[MobileColumnAttribute(Context = "Folder_WorkTypeList_ListView", TargetItems = nameof(Name)+ "," + nameof(Member))]
	[MobileColumnAttribute(Context = "WorkType_LookupListView", TargetItems = nameof(Name)+ "," + nameof(Member)+ "," + nameof(Folder))]
	[MobileColumnAttribute(Context = "Member_WorkTypeList_ListView", TargetItems = nameof(Name)+ "," + nameof(Folder))]
	[MobileColumnAttribute(Context = "WorkType_ListView", TargetItems = nameof(Name)+ "," + nameof(Folder))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class WorkType:  DevExpress.Xpo.XPLiteObject , INewObjectSession , INoIndexColumn, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public WorkType(Session session)
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
				if (WorkDetailList.IsLoaded)
                {
                    if (WorkDetailList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(WorkDetailList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(WorkDetailList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.WorkDetail>(CriteriaOperator.Parse("[WorkType.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool workdetaillist = Session.Query<Module.BusinessObjects.WorkDetail>().Where(x => x.WorkType.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(WorkDetailList), workdetaillist);
                        if (workdetaillist)
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
               

		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(0)]		

 		[Size(20)]
		[RuleUniqueValue("UniqueWorkTypeCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredWorkTypeCode", DefaultContexts.Save)]
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
		//Set Default Value

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

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(1)]		

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

	
       
		//private SoftwareObjectType _softwareobjecttype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đối tượng")]
        [ToolTip("Đối tượng")]
		//[Index(2)]		
		public SoftwareObjectType SoftwareObjectType
        { 
		    get => GetPropertyValue<SoftwareObjectType>("SoftwareObjectType");                         
			set => SetPropertyValue<SoftwareObjectType>("SoftwareObjectType", value); 
			
        }
		//Tooltip for Object
		public object SoftwareObjectTypeToolTipControllerText(View view)
        {
        //    if (SoftwareObjectType != null) 
		//			return SoftwareObjectType;
            return null;
        }
		//Get Default Value
        public SoftwareObjectType GetDefaultSoftwareObjectType(View view = null)
        { 
			return SoftwareObjectType;
        }
		//Set Default Value
		public void SetDefaultSoftwareObjectType(View view = null)
        {
            //if (SoftwareObjectType is null){
            //    var result = GetDefaultSoftwareObjectType(view);
            //    if (result != null && result != SoftwareObjectType){
			//          SoftwareObjectType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SoftwareObjectTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSoftwareObjectType();
				//if (result != null && SoftwareObjectType != null){
				//	return !SoftwareObjectType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(3)]		
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
	
       
		//private TimeSpan? _duration;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Năng suất")]
        [ToolTip("Năng suất")]
		//[Index(4)]		
		public TimeSpan? Duration
        { 
		    get => GetPropertyValue<TimeSpan?>("Duration");                         
			set => SetPropertyValue<TimeSpan?>("Duration", value); 
			
        }
		//Tooltip for Object
		public object DurationToolTipControllerText(View view)
        {
        //    if (Duration != null) 
		//			return Duration;
            return null;
        }
		//Get Default Value
        public TimeSpan? GetDefaultDuration(View view = null)
        { 
			return Duration;
        }
		//Set Default Value
		public void SetDefaultDuration(View view = null)
        {
            //if (Duration is null){
            //    var result = GetDefaultDuration(view);
            //    if (result != null && result != Duration){
			//          Duration = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DurationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDuration();
				//if (result != null && Duration != null){
				//	return !Duration.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private TimeCycle _timecycle;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Chu kỳ")]
        [ToolTip("Chu kỳ")]
		//[Index(5)]		
		public TimeCycle TimeCycle
        { 
		    get => GetPropertyValue<TimeCycle>("TimeCycle");                         
			set => SetPropertyValue<TimeCycle>("TimeCycle", value); 
			
        }
		//Tooltip for Object
		public object TimeCycleToolTipControllerText(View view)
        {
        //    if (TimeCycle != null) 
		//			return TimeCycle;
            return null;
        }
		//Get Default Value
        public TimeCycle GetDefaultTimeCycle(View view = null)
        { 
			return TimeCycle;
        }
		//Set Default Value
		public void SetDefaultTimeCycle(View view = null)
        {
            //if (TimeCycle is null){
            //    var result = GetDefaultTimeCycle(view);
            //    if (result != null && result != TimeCycle){
			//          TimeCycle = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TimeCycleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTimeCycle();
				//if (result != null && TimeCycle != null){
				//	return !TimeCycle.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _note;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(6)]		

 		[Size(250)]
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

	
       
		//private bool _open;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Công khai")]
        [ToolTip("Công khai")]
		//[Index(7)]		
		public bool Open
        { 
		    get => GetPropertyValue<bool>("Open");                         
			set => SetPropertyValue<bool>("Open", value); 
			
        }
		//Tooltip for Object
		public object OpenToolTipControllerText(View view)
        {
        //    if (Open != null) 
		//			return Open;
            return null;
        }
		//Get Default Value
        public bool GetDefaultOpen(View view = null)
        { 
			return Open;
        }
		//Set Default Value
		public void SetDefaultOpen(View view = null)
        {
            //if (Open is null){
            //    var result = GetDefaultOpen(view);
            //    if (result != null && result != Open){
			//          Open = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OpenIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOpen();
				//if (result != null && Open != null){
				//	return !Open.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chi tiết")]
		//[Index(8)]
		[DevExpress.Xpo.Association("WorkType-WorkDetailList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.WorkDetail> WorkDetailList
        {      
		    get => GetCollection<Module.BusinessObjects.WorkDetail>("WorkDetailList"); 
			
        }
       
		//private string _content;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(9)]		

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

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
		//[Index(10)]
		[DevExpress.Xpo.Association("WorkType-BookMarkList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BookMark> BookMarkList
        {      
		    get => GetCollection<Module.BusinessObjects.BookMark>("BookMarkList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thành viên")]
		//[Index(11)]
		[DataSourceCriteria("Not WorkTypeList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("MemberList-WorkTypeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Member> MemberList
        {      
		    get => GetCollection<Module.BusinessObjects.Member>("MemberList"); 
			
        }
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(12)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
	    [ModelDefault("AllowEdit", "False")]
		public DateTime Update
        { 
		    get => GetPropertyValue<DateTime>("Update");                         
			set => SetPropertyValue<DateTime>("Update", value); 
			
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

	
       
		//private Module.BusinessObjects.Folder _folder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quy trình")]
        [ToolTip("Quy trình")]
		//[Index(13)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[InActive] = False And [FolderType] = ##ToString#WorkType#")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-WorkTypeList")]
	 
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
	
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(14)]		
		public bool InActive
        { 
		    get => GetPropertyValue<bool>("InActive");                         
			set => SetPropertyValue<bool>("InActive", value); 
			
        }
		//Tooltip for Object
		public object InActiveToolTipControllerText(View view)
        {
        //    if (InActive != null) 
		//			return InActive;
            return null;
        }
		//Get Default Value
        public bool GetDefaultInActive(View view = null)
        { 
			return InActive;
        }
		//Set Default Value
		public void SetDefaultInActive(View view = null)
        {
            //if (InActive is null){
            //    var result = GetDefaultInActive(view);
            //    if (result != null && result != InActive){
			//          InActive = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool InActiveIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultInActive();
				//if (result != null && InActive != null){
				//	return !InActive.Equals(result);
				//} 
   
                return false;
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
 
            #region 0339ImportCode
            base.AfterConstruction();
SetDefaultCode();
SetDefaultMember();
SetDefaultUpdate();
            #endregion 0339ImportCode
            Display = true;
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultSoftwareObjectType(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultDuration(View view = null);
        //SetDefaultTimeCycle(View view = null);
        //SetDefaultNote(View view = null);
        //SetDefaultOpen(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultInActive(View view = null);
			
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
            #region 0481ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0481ImportCode
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
			//	SetDefaultWorkDetailList();
			//	SetDefaultContent();
			//	SetDefaultBookMarkList();
			//	SetDefaultMemberList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0022ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 0022            Oid: 37583318-82c7-4095-997e-7179f3e618e5
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 0022ImportCode
#region 0028ImportCode
		public string GetDefaultCode(View view = null)
        {
            //Code: 0028            Oid: 7ca5142e-a4c9-4c5c-aebf-b720e6abcb90
            var keyCodeObject =
    Module.Helpers.ParameterHelper.GetSettingParameter(Session, "CodeObject");

//Trường hợp chỉ lấy mã trên đối tượng này
Type currentType = this.GetType();
//Trường hợp lấy mã từ đối tượng cha
//Type currentType = typeof(ObjectType);

//Kích thước mặc định là 3 số
int size = 3;
return Tools.GetCode(currentType , this.Session, this.Oid, keyCodeObject != null ? keyCodeObject.Value : "", size,
    " ");
return null;
        }
#endregion 0028ImportCode
#region 0063ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0063            Oid: 18021af4-09e9-4fa7-a517-d31bcab25e3b
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0063ImportCode
#region 0068ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0068            Oid: 974e8e24-efb5-493e-8707-3fdfd108c0fd
            Update = GetDefaultUpdate();
        }
#endregion 0068ImportCode
#region 0147ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 0147            Oid: f42ee0f2-2b80-4fab-8528-e61795ee05fc
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 0147ImportCode
#region 0177ImportCode
		public void SetDefaultCode(View view = null)
        {
            //Code: 0177            Oid: c555437c-5924-42d4-8ea4-b080a42dbd8c
            if(String.IsNullOrEmpty(Code)) Code = GetDefaultCode();
        }
#endregion 0177ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
