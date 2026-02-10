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
	[NavigationItem("Common")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Mã nguồn"), ImageName("SourceCode")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Code)+ "," + nameof(Member)+ "," + nameof(ProgrammingLanguage)+ "," + nameof(Update)+ "," + nameof(Updater)+ "," + nameof(SoftwareObjectType))]
 
	[MobileColumnAttribute(Context = "SourceCode_ListView", TargetItems = nameof(LineQuantity)+ "," + nameof(ProgrammingLanguage)+ "," + nameof(SystemType)+ "," + nameof(Code))]
	[MobileColumnAttribute(Context = "SourceCode_Edit_ListView", TargetItems = nameof(SystemType)+ "," + nameof(Code)+ "," + nameof(LineQuantity)+ "," + nameof(ProgrammingLanguage))]
	[MobileColumnAttribute(Context = "SoftwareSolution_RuntimeSourceCodeList_ListView", TargetItems = nameof(Code)+ "," + nameof(LineQuantity)+ "," + nameof(ProgrammingLanguage)+ "," + nameof(SystemType))]
	[MobileColumnAttribute(Context = "Work_SourceCode_ListView", TargetItems = nameof(Code)+ "," + nameof(ProgrammingLanguage)+ "," + nameof(SystemType)+ "," + nameof(LineQuantity))]
	[MobileColumnAttribute(Context = "SourceCode_LookupListView", TargetItems = nameof(SystemType)+ "," + nameof(LineQuantity)+ "," + nameof(ProgrammingLanguage)+ "," + nameof(Code))]
	[MobileColumnAttribute(Context = "SoftwareNameSpace_SourceCode_ListView", TargetItems = nameof(SystemType)+ "," + nameof(LineQuantity)+ "," + nameof(Code)+ "," + nameof(ProgrammingLanguage))]
	[DefaultProperty("Code")]
 
[OptimisticLocking(true)]
    public partial class SourceCode:  DevExpress.Xpo.XPLiteObject , IWork, IUpperObject , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public SourceCode(Session session)
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
                        //if (Session.FindObject<Module.BusinessObjects.ObjectRelation>(CriteriaOperator.Parse("[SourceCode.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool objectrelationlist = Session.Query<Module.BusinessObjects.ObjectRelation>().Where(x => x.SourceCode.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ObjectRelationList), objectrelationlist);
                        if (objectrelationlist)
                            return true;

                    }                    
                }				
				if (BookMarkList.IsLoaded)
                {
                    if (BookMarkList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(BookMarkList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(BookMarkList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.BookMark>(CriteriaOperator.Parse("[SourceCode.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool bookmarklist = Session.Query<Module.BusinessObjects.BookMark>().Where(x => x.SourceCode.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(BookMarkList), bookmarklist);
                        if (bookmarklist)
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
		[RuleUniqueValue("UniqueSourceCodeCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredSourceCodeCode", DefaultContexts.Save)]
	    [ModelDefault("AllowEdit", "False")]
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

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(2)]		
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
	
       
		//private ProgrammingLanguage _programminglanguage;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngôn ngữ")]
        [ToolTip("Ngôn ngữ")]
		//[Index(5)]		
		public ProgrammingLanguage ProgrammingLanguage
        { 
		    get => GetPropertyValue<ProgrammingLanguage>("ProgrammingLanguage");                         
			set => SetPropertyValue<ProgrammingLanguage>("ProgrammingLanguage", value); 
			
        }
		//Tooltip for Object
		public object ProgrammingLanguageToolTipControllerText(View view)
        {
        //    if (ProgrammingLanguage != null) 
		//			return ProgrammingLanguage;
            return null;
        }
		//Get Default Value
        public ProgrammingLanguage GetDefaultProgrammingLanguage(View view = null)
        { 
			return ProgrammingLanguage;
        }
		//Set Default Value

		//Check Not Validate
		protected bool ProgrammingLanguageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultProgrammingLanguage();
				//if (result != null && ProgrammingLanguage != null){
				//	return !ProgrammingLanguage.Equals(result);
				//} 
   
                return false;
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
	    [ImmediatePostData()]
	    [DataSourceProperty("SuggestionList")]
	    [ModelDefault("PropertyEditorType", "CSCodePropertyEditor")]
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

	
       
		//private string _name;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mô tả")]
        [ToolTip("Mô tả")]
		//[Index(7)]		

 		[Size(SizeAttribute.Unlimited)]
	    [DataSourceProperty("SuggestionList")]
	    [ModelDefault("PropertyEditorType", "IntelliSensePropertyEditor")]
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

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quan hệ")]
		//[Index(8)]
		[DevExpress.Xpo.Association("SourceCode-ObjectRelationList")]
	    [DevExpress.Xpo.Aggregated()]
	    [RuleCombinationOfPropertiesIsUnique("UniqueRule.ObjectRelationList", DefaultContexts.Save, "ObjectID")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ObjectRelation> ObjectRelationList
        {      
		    get => GetCollection<Module.BusinessObjects.ObjectRelation>("ObjectRelationList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
		//[Index(11)]
		[DevExpress.Xpo.Association("SourceCode-BookMarkList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BookMark> BookMarkList
        {      
		    get => GetCollection<Module.BusinessObjects.BookMark>("BookMarkList"); 
			
        }
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
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

	
       
		//private Module.BusinessObjects.Member _updater;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Người cập nhật")]
        [ToolTip("Người cập nhật")]
		//[Index(13)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpdaterCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Member Updater
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Updater");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Updater", value); 
			
        }
		//Tooltip for Object
		public object UpdaterToolTipControllerText(View view)
        {
        //    if (Updater != null) 
		//			return Updater;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool UpdaterIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpdater();
				//if (result != null && Updater != null){
				//	return !Updater.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpdaterCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Updater));
            }
        }
	
       
		//private System.Type _systemtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(14)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SystemTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [TypeConverter(typeof(DevExpress.Persistent.Base.Security.SecurityTargetTypeConverter))]
	    [ValueConverter(typeof(DevExpress.ExpressApp.Utils.TypeToStringConverter))]
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
	
       
		//private SoftwareObjectType _softwareobjecttype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu đối tượng")]
        [ToolTip("Kiểu đối tượng")]
		//[Index(15)]		
	    [NonPersistent()]
	    [NotMapped()]
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

	
       
		//private System.Guid? _objectid;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("ID cấp trên")]
        [ToolTip("ID cấp trên")]
		//[Index(16)]		
	    [ModelDefault("AllowEdit", "False")]
		public System.Guid? ObjectID
        { 
		    get => GetPropertyValue<System.Guid?>("ObjectID");                         
			set => SetPropertyValue<System.Guid?>("ObjectID", value); 
			
        }
		//Tooltip for Object
		public object ObjectIDToolTipControllerText(View view)
        {
        //    if (ObjectID != null) 
		//			return ObjectID;
            return null;
        }
		//Get Default Value
        public System.Guid? GetDefaultObjectID(View view = null)
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

	
       
		//private string _note;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(17)]		

 		[Size(500)]
	    [NonPersistent()]
	    [NotMapped()]
		public string Note
        { 
		    #region 3432ImportCode 
get;
set;
#endregion 3432ImportCode
			
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

	
       
		//private bool _flag;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ")]
        [ToolTip("Cờ")]
		//[Index(19)]		
	    [NonPersistent()]
	    [NotMapped()]
		public bool Flag
        { 
		    #region 3433ImportCode 
get;
set;
#endregion 3433ImportCode
			
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

	
       
		//private int? _linequantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Dòng mã nguồn")]
        [ToolTip("Dòng mã nguồn")]
		//[Index(20)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? LineQuantity
        { 
		    #region 3654ImportCode 
        get
        {
            if (string.IsNullOrEmpty(Content))
                return null;

            return Content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
        }
#endregion 3654ImportCode
			
        }
		//Tooltip for Object
		public object LineQuantityToolTipControllerText(View view)
        {
        //    if (LineQuantity != null) 
		//			return LineQuantity;
            return null;
        }
		//Get Default Value
        public int? GetDefaultLineQuantity(View view = null)
        { 
			return LineQuantity;
        }
		//Set Default Value
		public void SetDefaultLineQuantity(View view = null)
        {
            //if (LineQuantity is null){
            //    var result = GetDefaultLineQuantity(view);
            //    if (result != null && result != LineQuantity){
			//          LineQuantity = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LineQuantityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLineQuantity();
				//if (result != null && LineQuantity != null){
				//	return !LineQuantity.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _relationquantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số quan hệ")]
        [ToolTip("Số quan hệ")]
		//[Index(21)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? RelationQuantity
        { 
		    #region 4317ImportCode 
get => ObjectRelationList.Count; 
#endregion 4317ImportCode
			
        }
		//Tooltip for Object
		public object RelationQuantityToolTipControllerText(View view)
        {
        //    if (RelationQuantity != null) 
		//			return RelationQuantity;
            return null;
        }
		//Get Default Value
        public int? GetDefaultRelationQuantity(View view = null)
        { 
			return RelationQuantity;
        }
		//Set Default Value
		public void SetDefaultRelationQuantity(View view = null)
        {
            //if (RelationQuantity is null){
            //    var result = GetDefaultRelationQuantity(view);
            //    if (result != null && result != RelationQuantity){
			//          RelationQuantity = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RelationQuantityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRelationQuantity();
				//if (result != null && RelationQuantity != null){
				//	return !RelationQuantity.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.DataType _designdatatype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu thiết kế")]
        [ToolTip("Kiểu thiết kế")]
		//[Index(22)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DesignDataTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.DataType DesignDataType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.DataType>("DesignDataType");                         
			set => SetPropertyValue<Module.BusinessObjects.DataType>("DesignDataType", value); 
			
        }
		//Tooltip for Object
		public object DesignDataTypeToolTipControllerText(View view)
        {
        //    if (DesignDataType != null) 
		//			return DesignDataType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.DataType GetDefaultDesignDataType(View view = null)
        { 
			return DesignDataType;
        }
		//Set Default Value
		public void SetDefaultDesignDataType(View view = null)
        {
            //if (DesignDataType is null){
            //    var result = GetDefaultDesignDataType(view);
            //    if (result != null && result != DesignDataType){
			//          DesignDataType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DesignDataTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDesignDataType();
				//if (result != null && DesignDataType != null){
				//	return !DesignDataType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator DesignDataTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(DesignDataType));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0430ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
SetDefaultCode();
SetDefaultMember();
            #endregion 0430ImportCode
 
        //SetDefaultCode(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultProgrammingLanguage(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultSystemType(View view = null);
        //SetDefaultSoftwareObjectType(View view = null);
        //SetDefaultObjectID(View view = null);
        //SetDefaultNote(View view = null);
        //SetDefaultFlag(View view = null);
        //SetDefaultLineQuantity(View view = null);
        //SetDefaultRelationQuantity(View view = null);
        //SetDefaultDesignDataType(View view = null);
			
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
            #region 0485ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 0485ImportCode
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
				
                    case nameof(SystemType):
                        OnChangedSystemType(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedSystemType(object oldValue, object newValue)
        {
            #region 3635ImportCode
            if (newValue is null) return;
SetDefaultSoftwareObjectType();            
            #endregion 3635ImportCode
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
			//	SetDefaultName();
			//	SetDefaultObjectRelationList();
			//	SetDefaultSoftwareNameSpaceList();
			//	SetDefaultSourceCodeVersionList();
			//	SetDefaultBookMarkList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0624ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 0624            Oid: 9f7ddbc9-1a58-4c7c-bfb4-4a7a33e11c32
            Updater = GetDefaultUpdater();

        }
#endregion 0624ImportCode
#region 3062ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 3062            Oid: 53c13698-1329-4ded-aae8-a5e98395dfa9
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 3062ImportCode
#region 0099ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0099            Oid: a9dbc5af-5f1e-4050-a471-f4b94f9220ba
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0099ImportCode
#region 0128ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0128            Oid: a41f3a33-6191-4e3d-8105-b44259cd5ae4
            Update = GetDefaultUpdate();
        }
#endregion 0128ImportCode
#region 0618ImportCode
		public string GetDefaultCode(View view = null)
        {
            //Code: 0618            Oid: 95d855de-b008-4d6a-8c8d-8bbf8b16eead
            var keyCodeObject =
    Module.Helpers.ParameterHelper.GetSettingParameter(Session, "CodeObject");
//Kích thước mặc định là 3 số
int size = 5;
return Tools.GetCode(this.GetType(), this.Session, this.Oid, keyCodeObject != null ? keyCodeObject.Value : "", size,
    " ");
return null;
        }
#endregion 0618ImportCode
#region 3633ImportCode
		public void SetDefaultSoftwareObjectType(View view = null)
        {
            //Code: 3633            Oid: b65b452a-b6ad-49e4-8cd9-1986baf1acc8
            if (SystemType == null)
    return;

string typeName = SystemType.Name;

if (Enum.TryParse<SoftwareObjectType>(typeName, out var enumValue))
{
    SoftwareObjectType = enumValue;
}

        }
#endregion 3633ImportCode
#region 3063ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 3063            Oid: fbc36274-9da3-444c-858e-93354bac7af5
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3063ImportCode
#region 0625ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 0625            Oid: 23c010a0-99c8-40d8-befb-da3c66b876ac
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 0625ImportCode
#region 0619ImportCode
		public void SetDefaultCode(View view = null)
        {
            //Code: 0619            Oid: 0480f368-4c74-4d88-a2c8-110057b1af31
            if(String.IsNullOrEmpty(Code)) Code = GetDefaultCode();

        }
#endregion 0619ImportCode
#region 4055ImportCode
		public void SetDefaultProgrammingLanguage(View view = null)
        {
            //Code: 4055            Oid: b66e239e-f782-4af0-93f1-5d8b03300eec
            ProgrammingLanguage = GetDefaultProgrammingLanguage();
        }
#endregion 4055ImportCode
        #endregion
//Mã nguồn bổ sung
#region SourceCodeImportCode
        [Browsable(false)]
        public IList<Module.SystemObjects.StringLookup> SuggestionList
        {
            get

            {

                return ObjectRelationList.Select(x => new Module.SystemObjects.StringLookup(Helpers.XafXpoHelper.GetCaptionEnum(typeof(Module.BusinessObjects.SoftwareObjectType), x.SoftwareObjectType), x.Code)).ToList(); //Lưu ý không để cache để khi hiện thì có chọn
            }
        }
#endregion SourceCodeImportCode
		 		 
    }
}
