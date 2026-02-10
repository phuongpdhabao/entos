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
    [ModelDefault("Caption", "Tư liệu"), ImageName("Video")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("Video DocumentType None_None__Color [A=255, R=255, G=128, B=0]" , TargetItems = "DocumentType" , Criteria = "[DocumentType] = ##ToString#Analysis#",AppearanceItemType = "ViewItem", FontColor = "#FF8000" )]
	[Appearance("Video DocumentType None_None__Color [A=255, R=65, G=105, B=225]" , TargetItems = "DocumentType" , Criteria = "[DocumentType] = ##ToString#Translation#",AppearanceItemType = "ViewItem", FontColor = "#4169E1" )]
	[Appearance("Video TermLocationList Hide_None__" , TargetItems = "TermLocationList" , Criteria = "[AbbyyTermLocation] = False",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Video DocumentType None_None__Color [A=255, R=0, G=192, B=0]" , TargetItems = "DocumentType" , Criteria = "[DocumentType] = ##ToString#VoiceOver#",AppearanceItemType = "ViewItem", FontColor = "#00C000" )]
	[Appearance("Video DocumentType None_None__Color [A=255, R=0, G=192, B=192]" , TargetItems = "DocumentType" , Criteria = "[DocumentType] = ##ToString#Editing#",AppearanceItemType = "ViewItem", FontColor = "#00C0C0" )]
	[Appearance("Video Spacing, Alignment, UpperElementImport, NodeFontBold, FontBold, FootNote, FontUnderline, NodeFontUnderline, NodeLink, ElementSpacing, BlankSpacing, FontColor, NodeFontColor, ImportParagraph, ImportByNode, NodeFontItalic, FontItalic, Outline, Number, NodeSuper, Indent, BrLine None_Disable__" , TargetItems = "Spacing, Alignment, UpperElementImport, NodeFontBold, FontBold, FootNote, FontUnderline, NodeFontUnderline, NodeLink, ElementSpacing, BlankSpacing, FontColor, NodeFontColor, ImportParagraph, ImportByNode, NodeFontItalic, FontItalic, Outline, Number, NodeSuper, Indent, BrLine" , Criteria = "[AudioList][].Count() <> 0",AppearanceItemType = "ViewItem", Context = "DetailView" , Enabled = false )]
	[Appearance("Video DocumentType None_None__Color [A=255, R=128, G=0, B=128]" , TargetItems = "DocumentType" , Criteria = "[DocumentType] = ##ToString#Video#",AppearanceItemType = "ViewItem", FontColor = "#800080" )]
	[Appearance("Video ParagraphStyleList Hide_None__" , TargetItems = "ParagraphStyleList" , Criteria = "[ParagraphStyleList][].Count() = 0",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Code)+ "," + nameof(Member)+ "," + nameof(Date)+ "," + nameof(ElementSpacing)+ "," + nameof(Update))]
 
	[MobileColumnAttribute(Context = "Language_VideoList_ListView", TargetItems = nameof(Name)+ "," + nameof(Member)+ "," + nameof(DocumentType))]
	[MobileColumnAttribute(Context = "Video_LookupListView", TargetItems = nameof(Member)+ "," + nameof(DocumentType)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Video_ListView", TargetItems = nameof(Member)+ "," + nameof(DocumentType)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_VideoList_ListView", TargetItems = nameof(Name)+ "," + nameof(Member)+ "," + nameof(DocumentType))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Video:  DevExpress.Xpo.XPLiteObject , IWork , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Video(Session session)
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
				if (AudioList.IsLoaded)
                {
                    if (AudioList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(AudioList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(AudioList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Audio>(CriteriaOperator.Parse("[Video.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool audiolist = Session.Query<Module.BusinessObjects.Audio>().Where(x => x.Video.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(AudioList), audiolist);
                        if (audiolist)
                            return true;

                    }                    
                }				
				if (ElementBatchList.IsLoaded)
                {
                    if (ElementBatchList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ElementBatchList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ElementBatchList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ElementBatch>(CriteriaOperator.Parse("[Video.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool elementbatchlist = Session.Query<Module.BusinessObjects.ElementBatch>().Where(x => x.Video.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ElementBatchList), elementbatchlist);
                        if (elementbatchlist)
                            return true;

                    }                    
                }				
				if (MediaList.IsLoaded)
                {
                    if (MediaList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(MediaList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(MediaList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Media>(CriteriaOperator.Parse("[Video.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool medialist = Session.Query<Module.BusinessObjects.Media>().Where(x => x.Video.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(MediaList), medialist);
                        if (medialist)
                            return true;

                    }                    
                }				
				if (ParagraphList.IsLoaded)
                {
                    if (ParagraphList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ParagraphList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ParagraphList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Paragraph>(CriteriaOperator.Parse("[Video.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool paragraphlist = Session.Query<Module.BusinessObjects.Paragraph>().Where(x => x.Video.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ParagraphList), paragraphlist);
                        if (paragraphlist)
                            return true;

                    }                    
                }				
				if (TranslateObjectList.IsLoaded)
                {
                    if (TranslateObjectList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(TranslateObjectList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(TranslateObjectList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.TranslateObject>(CriteriaOperator.Parse("[Video.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool translateobjectlist = Session.Query<Module.BusinessObjects.TranslateObject>().Where(x => x.Video.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(TranslateObjectList), translateobjectlist);
                        if (translateobjectlist)
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
		[RuleUniqueValue("UniqueVideoCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredVideoCode", DefaultContexts.Save)]
	    [RuleUniqueValue("Video.Code.Unique", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, TargetCriteria = "CodeUnique")]
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

 		[Size(250)]
		[RuleRequiredField("RequiredVideoName", DefaultContexts.Save)]
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

	
       
		//private DocumentType _documenttype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(2)]		
		public DocumentType DocumentType
        { 
		    get => GetPropertyValue<DocumentType>("DocumentType");                         
			set => SetPropertyValue<DocumentType>("DocumentType", value); 
			
        }
		//Tooltip for Object
		public object DocumentTypeToolTipControllerText(View view)
        {
        //    if (DocumentType != null) 
		//			return DocumentType;
            return null;
        }
		//Get Default Value
        public DocumentType GetDefaultDocumentType(View view = null)
        { 
			return DocumentType;
        }
		//Set Default Value
		public void SetDefaultDocumentType(View view = null)
        {
            //if (DocumentType is null){
            //    var result = GetDefaultDocumentType(view);
            //    if (result != null && result != DocumentType){
			//          DocumentType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DocumentTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDocumentType();
				//if (result != null && DocumentType != null){
				//	return !DocumentType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
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
	
       
		//private DateTime _date;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime Date
        { 
		    get => GetPropertyValue<DateTime>("Date");                         
			set => SetPropertyValue<DateTime>("Date", value); 
			
        }
		//Tooltip for Object
		public object DateToolTipControllerText(View view)
        {
        //    if (Date != null) 
		//			return Date;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool DateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDate();
				//if (result != null && Date != null){
				//	return !Date.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Language _languageorigin;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngữ gốc")]
        [ToolTip("Ngữ gốc")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LanguageOriginCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Language LanguageOrigin
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Language>("LanguageOrigin");                         
			set => SetPropertyValue<Module.BusinessObjects.Language>("LanguageOrigin", value); 
			
        }
		//Tooltip for Object
		public object LanguageOriginToolTipControllerText(View view)
        {
        //    if (LanguageOrigin != null) 
		//			return LanguageOrigin;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Language GetDefaultLanguageOrigin(View view = null)
        { 
			return LanguageOrigin;
        }
		//Set Default Value
		public void SetDefaultLanguageOrigin(View view = null)
        {
            //if (LanguageOrigin is null){
            //    var result = GetDefaultLanguageOrigin(view);
            //    if (result != null && result != LanguageOrigin){
			//          LanguageOrigin = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LanguageOriginIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLanguageOrigin();
				//if (result != null && LanguageOrigin != null){
				//	return !LanguageOrigin.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LanguageOriginCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(LanguageOrigin));
            }
        }
	
       
		//private Module.BusinessObjects.Language _languagetranslate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngữ dịch")]
        [ToolTip("Ngữ dịch")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LanguageTranslateCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Language LanguageTranslate
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Language>("LanguageTranslate");                         
			set => SetPropertyValue<Module.BusinessObjects.Language>("LanguageTranslate", value); 
			
        }
		//Tooltip for Object
		public object LanguageTranslateToolTipControllerText(View view)
        {
        //    if (LanguageTranslate != null) 
		//			return LanguageTranslate;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Language GetDefaultLanguageTranslate(View view = null)
        { 
			return LanguageTranslate;
        }
		//Set Default Value
		public void SetDefaultLanguageTranslate(View view = null)
        {
            //if (LanguageTranslate is null){
            //    var result = GetDefaultLanguageTranslate(view);
            //    if (result != null && result != LanguageTranslate){
			//          LanguageTranslate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LanguageTranslateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLanguageTranslate();
				//if (result != null && LanguageTranslate != null){
				//	return !LanguageTranslate.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LanguageTranslateCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(LanguageTranslate));
            }
        }
	
       
		//private string _path;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đường dẫn")]
        [ToolTip("Đường dẫn")]
		//[Index(7)]		

 		[Size(250)]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string Path
        { 
		    get => GetPropertyValue<string>("Path");                         
			set => SetPropertyValue<string>("Path", value); 
			
        }
		//Tooltip for Object
		public object PathToolTipControllerText(View view)
        {
        //    if (Path != null) 
		//			return Path;
            return null;
        }
		//Get Default Value
        public string GetDefaultPath(View view = null)
        { 
			return Path;
        }
		//Set Default Value
		public void SetDefaultPath(View view = null)
        {
            //if (Path is null){
            //    var result = GetDefaultPath(view);
            //    if (result != null && result != Path){
			//          Path = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PathIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPath();
				//if (result != null && Path != null){
				//	return !Path.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Status _status;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Trạng thái")]
        [ToolTip("Trạng thái")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(StatusCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Status Status
        { 
		    get => GetPropertyValue<Status>("Status");                         
			set => SetPropertyValue<Status>("Status", value); 
			
        }
		//Tooltip for Object
		public object StatusToolTipControllerText(View view)
        {
        //    if (Status != null) 
		//			return Status;
            return null;
        }
		//Get Default Value
        public Status GetDefaultStatus(View view = null)
        { 
			return Status;
        }
		//Set Default Value
		public void SetDefaultStatus(View view = null)
        {
            //if (Status is null){
            //    var result = GetDefaultStatus(view);
            //    if (result != null && result != Status){
			//          Status = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StatusIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStatus();
				//if (result != null && Status != null){
				//	return !Status.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator StatusCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Status));
            }
        }
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thành phần")]
		//[Index(9)]
		[DevExpress.Xpo.Association("Video-AudioList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Audio> AudioList
        {      
		    get => GetCollection<Module.BusinessObjects.Audio>("AudioList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lô")]
		//[Index(10)]
		[DevExpress.Xpo.Association("Video-ElementBatchList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ElementBatch> ElementBatchList
        {      
		    get => GetCollection<Module.BusinessObjects.ElementBatch>("ElementBatchList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuật ngữ")]
		//[Index(11)]
		[DevExpress.Xpo.Association("Video-TermList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Term> TermList
        {      
		    get => GetCollection<Module.BusinessObjects.Term>("TermList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuật vị")]
		//[Index(12)]
		//[DevExpress.Xpo.Association]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.TermLocation> TermLocationList
        {      

                #region 1424ImportCode 
    get
    {
        if(_termLocationList is null)
        {
            FillDefaultTermLocationList();
        }
        return _termLocationList;
        
    }

}
private XPCollection<Module.BusinessObjects.TermLocation> _termLocationList = null;
private void FillDefaultTermLocationList()
{
    _termLocationList = new XPCollection<Module.BusinessObjects.TermLocation>(PersistentCriteriaEvaluationBehavior.InTransaction, Session, CriteriaOperator.Parse("Audio.Video = ?", this));
    if (_termLocationList.Count == 0 && Session.IsNewObject(this) && AudioList?.Count > 0)
    {
        foreach (var audio in AudioList)
            if (audio.TermLocationList?.Count > 0)
                _termLocationList?.AddRange(audio.TermLocationList);
    }
#endregion 1424ImportCode
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hình ảnh")]
		//[Index(13)]
		[DevExpress.Xpo.Association("Video-MediaList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Media> MediaList
        {      
		    get => GetCollection<Module.BusinessObjects.Media>("MediaList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đoạn văn")]
		//[Index(14)]
		[DevExpress.Xpo.Association("Video-ParagraphList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Paragraph> ParagraphList
        {      
		    get => GetCollection<Module.BusinessObjects.Paragraph>("ParagraphList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu cách")]
		//[Index(15)]
		[DevExpress.Xpo.Association("Video-ParagraphStyleList")]
	    [RuleCombinationOfPropertiesIsUnique("UniqueRule.ParagraphStyleList", DefaultContexts.Save, "Name, Video, Link")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ParagraphStyle> ParagraphStyleList
        {      
		    get => GetCollection<Module.BusinessObjects.ParagraphStyle>("ParagraphStyleList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngôn ngữ")]
		//[Index(16)]
		[DataSourceCriteria("Not VideoList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("LanguageList-VideoList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Language> LanguageList
        {      
		    get => GetCollection<Module.BusinessObjects.Language>("LanguageList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đối tượng")]
		//[Index(17)]
		[DevExpress.Xpo.Association("Video-TranslateObjectList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.TranslateObject> TranslateObjectList
        {      
		    get => GetCollection<Module.BusinessObjects.TranslateObject>("TranslateObjectList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
		//[Index(18)]
		[DevExpress.Xpo.Association("Video-FileList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BookMark> FileList
        {      
		    get => GetCollection<Module.BusinessObjects.BookMark>("FileList"); 
			
        }
       
		//private string _note;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(19)]		

 		[Size(SizeAttribute.Unlimited)]
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

	
       
		//private bool _fontcolor;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Màu phông")]
        [ToolTip("Màu phông")]
		//[Index(20)]		
		public bool FontColor
        { 
		    get => GetPropertyValue<bool>("FontColor");                         
			set => SetPropertyValue<bool>("FontColor", value); 
			
        }
		//Tooltip for Object
		public object FontColorToolTipControllerText(View view)
        {
        //    if (FontColor != null) 
		//			return FontColor;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFontColor(View view = null)
        { 
			return FontColor;
        }
		//Set Default Value
		public void SetDefaultFontColor(View view = null)
        {
            //if (FontColor is null){
            //    var result = GetDefaultFontColor(view);
            //    if (result != null && result != FontColor){
			//          FontColor = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FontColorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFontColor();
				//if (result != null && FontColor != null){
				//	return !FontColor.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _fontbold;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đậm phông")]
        [ToolTip("Đậm phông")]
		//[Index(21)]		
		public bool FontBold
        { 
		    get => GetPropertyValue<bool>("FontBold");                         
			set => SetPropertyValue<bool>("FontBold", value); 
			
        }
		//Tooltip for Object
		public object FontBoldToolTipControllerText(View view)
        {
        //    if (FontBold != null) 
		//			return FontBold;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFontBold(View view = null)
        { 
			return FontBold;
        }
		//Set Default Value
		public void SetDefaultFontBold(View view = null)
        {
            //if (FontBold is null){
            //    var result = GetDefaultFontBold(view);
            //    if (result != null && result != FontBold){
			//          FontBold = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FontBoldIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFontBold();
				//if (result != null && FontBold != null){
				//	return !FontBold.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _fontitalic;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nghiêng phông")]
        [ToolTip("Nghiêng phông")]
		//[Index(22)]		
		public bool FontItalic
        { 
		    get => GetPropertyValue<bool>("FontItalic");                         
			set => SetPropertyValue<bool>("FontItalic", value); 
			
        }
		//Tooltip for Object
		public object FontItalicToolTipControllerText(View view)
        {
        //    if (FontItalic != null) 
		//			return FontItalic;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFontItalic(View view = null)
        { 
			return FontItalic;
        }
		//Set Default Value
		public void SetDefaultFontItalic(View view = null)
        {
            //if (FontItalic is null){
            //    var result = GetDefaultFontItalic(view);
            //    if (result != null && result != FontItalic){
			//          FontItalic = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FontItalicIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFontItalic();
				//if (result != null && FontItalic != null){
				//	return !FontItalic.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _fontunderline;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Gạch phông")]
        [ToolTip("Gạch phông")]
		//[Index(23)]		
		public bool FontUnderline
        { 
		    get => GetPropertyValue<bool>("FontUnderline");                         
			set => SetPropertyValue<bool>("FontUnderline", value); 
			
        }
		//Tooltip for Object
		public object FontUnderlineToolTipControllerText(View view)
        {
        //    if (FontUnderline != null) 
		//			return FontUnderline;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFontUnderline(View view = null)
        { 
			return FontUnderline;
        }
		//Set Default Value
		public void SetDefaultFontUnderline(View view = null)
        {
            //if (FontUnderline is null){
            //    var result = GetDefaultFontUnderline(view);
            //    if (result != null && result != FontUnderline){
			//          FontUnderline = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FontUnderlineIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFontUnderline();
				//if (result != null && FontUnderline != null){
				//	return !FontUnderline.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _outline;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Outline")]
        [ToolTip("Outline")]
		//[Index(24)]		
		public bool Outline
        { 
		    get => GetPropertyValue<bool>("Outline");                         
			set => SetPropertyValue<bool>("Outline", value); 
			
        }
		//Tooltip for Object
		public object OutlineToolTipControllerText(View view)
        {
        //    if (Outline != null) 
		//			return Outline;
            return null;
        }
		//Get Default Value
        public bool GetDefaultOutline(View view = null)
        { 
			return Outline;
        }
		//Set Default Value
		public void SetDefaultOutline(View view = null)
        {
            //if (Outline is null){
            //    var result = GetDefaultOutline(view);
            //    if (result != null && result != Outline){
			//          Outline = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OutlineIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOutline();
				//if (result != null && Outline != null){
				//	return !Outline.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _alignment;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Căn lề")]
        [ToolTip("Căn lề")]
		//[Index(25)]		
		public bool Alignment
        { 
		    get => GetPropertyValue<bool>("Alignment");                         
			set => SetPropertyValue<bool>("Alignment", value); 
			
        }
		//Tooltip for Object
		public object AlignmentToolTipControllerText(View view)
        {
        //    if (Alignment != null) 
		//			return Alignment;
            return null;
        }
		//Get Default Value
        public bool GetDefaultAlignment(View view = null)
        { 
			return Alignment;
        }
		//Set Default Value
		public void SetDefaultAlignment(View view = null)
        {
            //if (Alignment is null){
            //    var result = GetDefaultAlignment(view);
            //    if (result != null && result != Alignment){
			//          Alignment = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AlignmentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAlignment();
				//if (result != null && Alignment != null){
				//	return !Alignment.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _spacing;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cách dòng")]
        [ToolTip("Cách dòng")]
		//[Index(26)]		
		public bool Spacing
        { 
		    get => GetPropertyValue<bool>("Spacing");                         
			set => SetPropertyValue<bool>("Spacing", value); 
			
        }
		//Tooltip for Object
		public object SpacingToolTipControllerText(View view)
        {
        //    if (Spacing != null) 
		//			return Spacing;
            return null;
        }
		//Get Default Value
        public bool GetDefaultSpacing(View view = null)
        { 
			return Spacing;
        }
		//Set Default Value
		public void SetDefaultSpacing(View view = null)
        {
            //if (Spacing is null){
            //    var result = GetDefaultSpacing(view);
            //    if (result != null && result != Spacing){
			//          Spacing = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpacingIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpacing();
				//if (result != null && Spacing != null){
				//	return !Spacing.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _indent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thụt")]
        [ToolTip("Thụt")]
		//[Index(27)]		
		public bool Indent
        { 
		    get => GetPropertyValue<bool>("Indent");                         
			set => SetPropertyValue<bool>("Indent", value); 
			
        }
		//Tooltip for Object
		public object IndentToolTipControllerText(View view)
        {
        //    if (Indent != null) 
		//			return Indent;
            return null;
        }
		//Get Default Value
        public bool GetDefaultIndent(View view = null)
        { 
			return Indent;
        }
		//Set Default Value
		public void SetDefaultIndent(View view = null)
        {
            //if (Indent is null){
            //    var result = GetDefaultIndent(view);
            //    if (result != null && result != Indent){
			//          Indent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool IndentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultIndent();
				//if (result != null && Indent != null){
				//	return !Indent.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _nodefontcolor;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Màu phông")]
        [ToolTip("Màu phông")]
		//[Index(28)]		
		public bool NodeFontColor
        { 
		    get => GetPropertyValue<bool>("NodeFontColor");                         
			set => SetPropertyValue<bool>("NodeFontColor", value); 
			
        }
		//Tooltip for Object
		public object NodeFontColorToolTipControllerText(View view)
        {
        //    if (NodeFontColor != null) 
		//			return NodeFontColor;
            return null;
        }
		//Get Default Value
        public bool GetDefaultNodeFontColor(View view = null)
        { 
			return NodeFontColor;
        }
		//Set Default Value
		public void SetDefaultNodeFontColor(View view = null)
        {
            //if (NodeFontColor is null){
            //    var result = GetDefaultNodeFontColor(view);
            //    if (result != null && result != NodeFontColor){
			//          NodeFontColor = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NodeFontColorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNodeFontColor();
				//if (result != null && NodeFontColor != null){
				//	return !NodeFontColor.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _nodefontbold;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đậm phông")]
        [ToolTip("Đậm phông")]
		//[Index(29)]		
		public bool NodeFontBold
        { 
		    get => GetPropertyValue<bool>("NodeFontBold");                         
			set => SetPropertyValue<bool>("NodeFontBold", value); 
			
        }
		//Tooltip for Object
		public object NodeFontBoldToolTipControllerText(View view)
        {
        //    if (NodeFontBold != null) 
		//			return NodeFontBold;
            return null;
        }
		//Get Default Value
        public bool GetDefaultNodeFontBold(View view = null)
        { 
			return NodeFontBold;
        }
		//Set Default Value
		public void SetDefaultNodeFontBold(View view = null)
        {
            //if (NodeFontBold is null){
            //    var result = GetDefaultNodeFontBold(view);
            //    if (result != null && result != NodeFontBold){
			//          NodeFontBold = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NodeFontBoldIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNodeFontBold();
				//if (result != null && NodeFontBold != null){
				//	return !NodeFontBold.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _nodefontitalic;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nghiêng phông")]
        [ToolTip("Nghiêng phông")]
		//[Index(30)]		
		public bool NodeFontItalic
        { 
		    get => GetPropertyValue<bool>("NodeFontItalic");                         
			set => SetPropertyValue<bool>("NodeFontItalic", value); 
			
        }
		//Tooltip for Object
		public object NodeFontItalicToolTipControllerText(View view)
        {
        //    if (NodeFontItalic != null) 
		//			return NodeFontItalic;
            return null;
        }
		//Get Default Value
        public bool GetDefaultNodeFontItalic(View view = null)
        { 
			return NodeFontItalic;
        }
		//Set Default Value
		public void SetDefaultNodeFontItalic(View view = null)
        {
            //if (NodeFontItalic is null){
            //    var result = GetDefaultNodeFontItalic(view);
            //    if (result != null && result != NodeFontItalic){
			//          NodeFontItalic = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NodeFontItalicIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNodeFontItalic();
				//if (result != null && NodeFontItalic != null){
				//	return !NodeFontItalic.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _nodefontunderline;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Gạch phông")]
        [ToolTip("Gạch phông")]
		//[Index(31)]		
		public bool NodeFontUnderline
        { 
		    get => GetPropertyValue<bool>("NodeFontUnderline");                         
			set => SetPropertyValue<bool>("NodeFontUnderline", value); 
			
        }
		//Tooltip for Object
		public object NodeFontUnderlineToolTipControllerText(View view)
        {
        //    if (NodeFontUnderline != null) 
		//			return NodeFontUnderline;
            return null;
        }
		//Get Default Value
        public bool GetDefaultNodeFontUnderline(View view = null)
        { 
			return NodeFontUnderline;
        }
		//Set Default Value
		public void SetDefaultNodeFontUnderline(View view = null)
        {
            //if (NodeFontUnderline is null){
            //    var result = GetDefaultNodeFontUnderline(view);
            //    if (result != null && result != NodeFontUnderline){
			//          NodeFontUnderline = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NodeFontUnderlineIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNodeFontUnderline();
				//if (result != null && NodeFontUnderline != null){
				//	return !NodeFontUnderline.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _nodelink;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hyper Link")]
        [ToolTip("Hyper Link")]
		//[Index(32)]		
		public bool NodeLink
        { 
		    get => GetPropertyValue<bool>("NodeLink");                         
			set => SetPropertyValue<bool>("NodeLink", value); 
			
        }
		//Tooltip for Object
		public object NodeLinkToolTipControllerText(View view)
        {
        //    if (NodeLink != null) 
		//			return NodeLink;
            return null;
        }
		//Get Default Value
        public bool GetDefaultNodeLink(View view = null)
        { 
			return NodeLink;
        }
		//Set Default Value
		public void SetDefaultNodeLink(View view = null)
        {
            //if (NodeLink is null){
            //    var result = GetDefaultNodeLink(view);
            //    if (result != null && result != NodeLink){
			//          NodeLink = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NodeLinkIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNodeLink();
				//if (result != null && NodeLink != null){
				//	return !NodeLink.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _nodesuper;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Super Script")]
        [ToolTip("Super Script")]
		//[Index(33)]		
		public bool NodeSuper
        { 
		    get => GetPropertyValue<bool>("NodeSuper");                         
			set => SetPropertyValue<bool>("NodeSuper", value); 
			
        }
		//Tooltip for Object
		public object NodeSuperToolTipControllerText(View view)
        {
        //    if (NodeSuper != null) 
		//			return NodeSuper;
            return null;
        }
		//Get Default Value
        public bool GetDefaultNodeSuper(View view = null)
        { 
			return NodeSuper;
        }
		//Set Default Value
		public void SetDefaultNodeSuper(View view = null)
        {
            //if (NodeSuper is null){
            //    var result = GetDefaultNodeSuper(view);
            //    if (result != null && result != NodeSuper){
			//          NodeSuper = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NodeSuperIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNodeSuper();
				//if (result != null && NodeSuper != null){
				//	return !NodeSuper.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _upperelementimport;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(34)]		
		public bool UpperElementImport
        { 
		    get => GetPropertyValue<bool>("UpperElementImport");                         
			set => SetPropertyValue<bool>("UpperElementImport", value); 
			
        }
		//Tooltip for Object
		public object UpperElementImportToolTipControllerText(View view)
        {
        //    if (UpperElementImport != null) 
		//			return UpperElementImport;
            return null;
        }
		//Get Default Value
        public bool GetDefaultUpperElementImport(View view = null)
        { 
			return UpperElementImport;
        }
		//Set Default Value
		public void SetDefaultUpperElementImport(View view = null)
        {
            //if (UpperElementImport is null){
            //    var result = GetDefaultUpperElementImport(view);
            //    if (result != null && result != UpperElementImport){
			//          UpperElementImport = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperElementImportIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpperElementImport();
				//if (result != null && UpperElementImport != null){
				//	return !UpperElementImport.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _number;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số và kí tự")]
        [ToolTip("Số và kí tự")]
		//[Index(35)]		
		public bool Number
        { 
		    get => GetPropertyValue<bool>("Number");                         
			set => SetPropertyValue<bool>("Number", value); 
			
        }
		//Tooltip for Object
		public object NumberToolTipControllerText(View view)
        {
        //    if (Number != null) 
		//			return Number;
            return null;
        }
		//Get Default Value
        public bool GetDefaultNumber(View view = null)
        { 
			return Number;
        }
		//Set Default Value
		public void SetDefaultNumber(View view = null)
        {
            //if (Number is null){
            //    var result = GetDefaultNumber(view);
            //    if (result != null && result != Number){
			//          Number = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NumberIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNumber();
				//if (result != null && Number != null){
				//	return !Number.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _checkspelling;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lỗi chính tả")]
        [ToolTip("Lỗi chính tả")]
		//[Index(36)]		
		public bool CheckSpelling
        { 
		    get => GetPropertyValue<bool>("CheckSpelling");                         
			set => SetPropertyValue<bool>("CheckSpelling", value); 
			
        }
		//Tooltip for Object
		public object CheckSpellingToolTipControllerText(View view)
        {
        //    if (CheckSpelling != null) 
		//			return CheckSpelling;
            return null;
        }
		//Get Default Value
        public bool GetDefaultCheckSpelling(View view = null)
        { 
			return CheckSpelling;
        }
		//Set Default Value
		public void SetDefaultCheckSpelling(View view = null)
        {
            //if (CheckSpelling is null){
            //    var result = GetDefaultCheckSpelling(view);
            //    if (result != null && result != CheckSpelling){
			//          CheckSpelling = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CheckSpellingIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCheckSpelling();
				//if (result != null && CheckSpelling != null){
				//	return !CheckSpelling.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _withtermposition;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kèm thuật vị")]
        [ToolTip("Kèm thuật vị")]
		//[Index(37)]		
		public bool WithTermPosition
        { 
		    get => GetPropertyValue<bool>("WithTermPosition");                         
			set => SetPropertyValue<bool>("WithTermPosition", value); 
			
        }
		//Tooltip for Object
		public object WithTermPositionToolTipControllerText(View view)
        {
        //    if (WithTermPosition != null) 
		//			return WithTermPosition;
            return null;
        }
		//Get Default Value
        public bool GetDefaultWithTermPosition(View view = null)
        { 
			return WithTermPosition;
        }
		//Set Default Value
		public void SetDefaultWithTermPosition(View view = null)
        {
            //if (WithTermPosition is null){
            //    var result = GetDefaultWithTermPosition(view);
            //    if (result != null && result != WithTermPosition){
			//          WithTermPosition = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WithTermPositionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWithTermPosition();
				//if (result != null && WithTermPosition != null){
				//	return !WithTermPosition.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _elementspacing;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Khoảng tách")]
        [ToolTip("Khoảng tách")]
		//[Index(38)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? ElementSpacing
        { 
		    get => GetPropertyValue<int?>("ElementSpacing");                         
			set => SetPropertyValue<int?>("ElementSpacing", value); 
			
        }
		//Tooltip for Object
		public object ElementSpacingToolTipControllerText(View view)
        {
        //    if (ElementSpacing != null) 
		//			return ElementSpacing;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool ElementSpacingIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultElementSpacing();
				//if (result != null && ElementSpacing != null){
				//	return !ElementSpacing.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int _blankspacing;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Khoảng trắng")]
        [ToolTip("Khoảng trắng")]
		//[Index(39)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int BlankSpacing
        { 
		    get => GetPropertyValue<int>("BlankSpacing");                         
			set => SetPropertyValue<int>("BlankSpacing", value); 
			
        }
		//Tooltip for Object
		public object BlankSpacingToolTipControllerText(View view)
        {
        //    if (BlankSpacing != null) 
		//			return BlankSpacing;
            return null;
        }
		//Get Default Value
        public int GetDefaultBlankSpacing(View view = null)
        { 
			return BlankSpacing;
        }
		//Set Default Value
		public void SetDefaultBlankSpacing(View view = null)
        {
            //if (BlankSpacing is null){
            //    var result = GetDefaultBlankSpacing(view);
            //    if (result != null && result != BlankSpacing){
			//          BlankSpacing = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BlankSpacingIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBlankSpacing();
				//if (result != null && BlankSpacing != null){
				//	return !BlankSpacing.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(40)]		
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

	
       
		//private bool _codeunique;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã duy nhất")]
        [ToolTip("Mã duy nhất")]
		//[Index(41)]		
	    [Browsable(false)]
		public bool CodeUnique
        { 
		    #region 0962ImportCode 
get
{
	if (Update != null)
	{
    		var time = new DateTime(Update.Value.Year, 1, 1);
    		var type = GetType();
    		var parser = CriteriaOperator.Parse("Oid <> ? and Code = ? and Update >= ? and Update < ?", Oid,
        		Code, time,        time.AddYears(1));
    		var result = Session.FindObject(type, parser);
    		return result != null;
	}
	return false;
}
#endregion 0962ImportCode
			
        }
		//Tooltip for Object
		public object CodeUniqueToolTipControllerText(View view)
        {
        //    if (CodeUnique != null) 
		//			return CodeUnique;
            return null;
        }
		//Get Default Value
        public bool GetDefaultCodeUnique(View view = null)
        { 
			return CodeUnique;
        }
		//Set Default Value
		public void SetDefaultCodeUnique(View view = null)
        {
            //if (CodeUnique is null){
            //    var result = GetDefaultCodeUnique(view);
            //    if (result != null && result != CodeUnique){
			//          CodeUnique = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CodeUniqueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCodeUnique();
				//if (result != null && CodeUnique != null){
				//	return !CodeUnique.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _upcasenumbering;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tự động hoa")]
        [ToolTip("Tự động hoa")]
		//[Index(42)]		
		public bool UpcaseNumbering
        { 
		    get => GetPropertyValue<bool>("UpcaseNumbering");                         
			set => SetPropertyValue<bool>("UpcaseNumbering", value); 
			
        }
		//Tooltip for Object
		public object UpcaseNumberingToolTipControllerText(View view)
        {
        //    if (UpcaseNumbering != null) 
		//			return UpcaseNumbering;
            return null;
        }
		//Get Default Value
        public bool GetDefaultUpcaseNumbering(View view = null)
        { 
			return UpcaseNumbering;
        }
		//Set Default Value
		public void SetDefaultUpcaseNumbering(View view = null)
        {
            //if (UpcaseNumbering is null){
            //    var result = GetDefaultUpcaseNumbering(view);
            //    if (result != null && result != UpcaseNumbering){
			//          UpcaseNumbering = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpcaseNumberingIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpcaseNumbering();
				//if (result != null && UpcaseNumbering != null){
				//	return !UpcaseNumbering.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _abbyytermlocation;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuật vị lỗi")]
        [ToolTip("Thuật vị lỗi")]
		//[Index(43)]		
	    [ImmediatePostData()]
		public bool AbbyyTermLocation
        { 
		    get => GetPropertyValue<bool>("AbbyyTermLocation");                         
			set => SetPropertyValue<bool>("AbbyyTermLocation", value); 
			
        }
		//Tooltip for Object
		public object AbbyyTermLocationToolTipControllerText(View view)
        {
        //    if (AbbyyTermLocation != null) 
		//			return AbbyyTermLocation;
            return null;
        }
		//Get Default Value
        public bool GetDefaultAbbyyTermLocation(View view = null)
        { 
			return AbbyyTermLocation;
        }
		//Set Default Value
		public void SetDefaultAbbyyTermLocation(View view = null)
        {
            //if (AbbyyTermLocation is null){
            //    var result = GetDefaultAbbyyTermLocation(view);
            //    if (result != null && result != AbbyyTermLocation){
			//          AbbyyTermLocation = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AbbyyTermLocationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAbbyyTermLocation();
				//if (result != null && AbbyyTermLocation != null){
				//	return !AbbyyTermLocation.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _keepspace;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giữ dấu cách")]
        [ToolTip("Giữ dấu cách")]
		//[Index(44)]		
		public bool KeepSpace
        { 
		    get => GetPropertyValue<bool>("KeepSpace");                         
			set => SetPropertyValue<bool>("KeepSpace", value); 
			
        }
		//Tooltip for Object
		public object KeepSpaceToolTipControllerText(View view)
        {
        //    if (KeepSpace != null) 
		//			return KeepSpace;
            return null;
        }
		//Get Default Value
        public bool GetDefaultKeepSpace(View view = null)
        { 
			return KeepSpace;
        }
		//Set Default Value
		public void SetDefaultKeepSpace(View view = null)
        {
            //if (KeepSpace is null){
            //    var result = GetDefaultKeepSpace(view);
            //    if (result != null && result != KeepSpace){
			//          KeepSpace = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool KeepSpaceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultKeepSpace();
				//if (result != null && KeepSpace != null){
				//	return !KeepSpace.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _importbynode;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nạp theo Node")]
		[ToolTip("Mặc định là nạp theo đoạn")]
		//[Index(45)]		
	    [ImmediatePostData()]
		public bool ImportByNode
        { 
		    get => GetPropertyValue<bool>("ImportByNode");                         
			set => SetPropertyValue<bool>("ImportByNode", value); 
			
        }
		//Tooltip for Object
		public object ImportByNodeToolTipControllerText(View view)
        {
        //    if (ImportByNode != null) 
		//			return ImportByNode;
            return null;
        }
		//Get Default Value
        public bool GetDefaultImportByNode(View view = null)
        { 
			return ImportByNode;
        }
		//Set Default Value
		public void SetDefaultImportByNode(View view = null)
        {
            //if (ImportByNode is null){
            //    var result = GetDefaultImportByNode(view);
            //    if (result != null && result != ImportByNode){
			//          ImportByNode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ImportByNodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImportByNode();
				//if (result != null && ImportByNode != null){
				//	return !ImportByNode.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _importparagraph;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nạp đoạn văn bản")]
        [ToolTip("Nạp đoạn văn bản")]
		//[Index(46)]		
		public bool ImportParagraph
        { 
		    get => GetPropertyValue<bool>("ImportParagraph");                         
			set => SetPropertyValue<bool>("ImportParagraph", value); 
			
        }
		//Tooltip for Object
		public object ImportParagraphToolTipControllerText(View view)
        {
        //    if (ImportParagraph != null) 
		//			return ImportParagraph;
            return null;
        }
		//Get Default Value
        public bool GetDefaultImportParagraph(View view = null)
        { 
			return ImportParagraph;
        }
		//Set Default Value
		public void SetDefaultImportParagraph(View view = null)
        {
            //if (ImportParagraph is null){
            //    var result = GetDefaultImportParagraph(view);
            //    if (result != null && result != ImportParagraph){
			//          ImportParagraph = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ImportParagraphIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImportParagraph();
				//if (result != null && ImportParagraph != null){
				//	return !ImportParagraph.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _rightindent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thụt phải")]
        [ToolTip("Thụt phải")]
		//[Index(47)]		
		public bool RightIndent
        { 
		    get => GetPropertyValue<bool>("RightIndent");                         
			set => SetPropertyValue<bool>("RightIndent", value); 
			
        }
		//Tooltip for Object
		public object RightIndentToolTipControllerText(View view)
        {
        //    if (RightIndent != null) 
		//			return RightIndent;
            return null;
        }
		//Get Default Value
        public bool GetDefaultRightIndent(View view = null)
        { 
			return RightIndent;
        }
		//Set Default Value
		public void SetDefaultRightIndent(View view = null)
        {
            //if (RightIndent is null){
            //    var result = GetDefaultRightIndent(view);
            //    if (result != null && result != RightIndent){
			//          RightIndent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RightIndentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRightIndent();
				//if (result != null && RightIndent != null){
				//	return !RightIndent.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _originstyleexport;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Xuất giữ kiểu")]
		[ToolTip("khi tích vào và xuất tư liệu sẽ không thay đổi Style và format gì của tài liệu gốc(đã tối ưu theo Option)")]
		//[Index(48)]		
	    [ImmediatePostData()]
		public bool OriginStyleExport
        { 
		    get => GetPropertyValue<bool>("OriginStyleExport");                         
			set => SetPropertyValue<bool>("OriginStyleExport", value); 
			
        }
		//Tooltip for Object
		public object OriginStyleExportToolTipControllerText(View view)
        {
        //    if (OriginStyleExport != null) 
		//			return OriginStyleExport;
            return null;
        }
		//Get Default Value
        public bool GetDefaultOriginStyleExport(View view = null)
        { 
			return OriginStyleExport;
        }
		//Set Default Value
		public void SetDefaultOriginStyleExport(View view = null)
        {
            //if (OriginStyleExport is null){
            //    var result = GetDefaultOriginStyleExport(view);
            //    if (result != null && result != OriginStyleExport){
			//          OriginStyleExport = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OriginStyleExportIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOriginStyleExport();
				//if (result != null && OriginStyleExport != null){
				//	return !OriginStyleExport.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _createwordstyle;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tạo Word Style")]
		[ToolTip("Thay thế OriginStyleExport")]
		//[Index(49)]		
	    [ImmediatePostData()]
		public bool CreateWordStyle
        { 
		    get => GetPropertyValue<bool>("CreateWordStyle");                         
			set => SetPropertyValue<bool>("CreateWordStyle", value); 
			
        }
		//Tooltip for Object
		public object CreateWordStyleToolTipControllerText(View view)
        {
        //    if (CreateWordStyle != null) 
		//			return CreateWordStyle;
            return null;
        }
		//Get Default Value
        public bool GetDefaultCreateWordStyle(View view = null)
        { 
			return CreateWordStyle;
        }
		//Set Default Value
		public void SetDefaultCreateWordStyle(View view = null)
        {
            //if (CreateWordStyle is null){
            //    var result = GetDefaultCreateWordStyle(view);
            //    if (result != null && result != CreateWordStyle){
			//          CreateWordStyle = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CreateWordStyleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCreateWordStyle();
				//if (result != null && CreateWordStyle != null){
				//	return !CreateWordStyle.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _open;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Công khai")]
        [ToolTip("Công khai")]
		//[Index(50)]		
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

	
       
		//private Module.BusinessObjects.Folder _folder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(51)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#Video# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-VideoList")]
	 
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
	
       
		//private bool _brline;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Xuống dòng BR")]
        [ToolTip("Xuống dòng BR")]
		//[Index(52)]		
		public bool BrLine
        { 
		    get => GetPropertyValue<bool>("BrLine");                         
			set => SetPropertyValue<bool>("BrLine", value); 
			
        }
		//Tooltip for Object
		public object BrLineToolTipControllerText(View view)
        {
        //    if (BrLine != null) 
		//			return BrLine;
            return null;
        }
		//Get Default Value
        public bool GetDefaultBrLine(View view = null)
        { 
			return BrLine;
        }
		//Set Default Value
		public void SetDefaultBrLine(View view = null)
        {
            //if (BrLine is null){
            //    var result = GetDefaultBrLine(view);
            //    if (result != null && result != BrLine){
			//          BrLine = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BrLineIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBrLine();
				//if (result != null && BrLine != null){
				//	return !BrLine.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _isphoto;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ảnh")]
        [ToolTip("Ảnh")]
		//[Index(53)]		
		public bool IsPhoto
        { 
		    get => GetPropertyValue<bool>("IsPhoto");                         
			set => SetPropertyValue<bool>("IsPhoto", value); 
			
        }
		//Tooltip for Object
		public object IsPhotoToolTipControllerText(View view)
        {
        //    if (IsPhoto != null) 
		//			return IsPhoto;
            return null;
        }
		//Get Default Value
        public bool GetDefaultIsPhoto(View view = null)
        { 
			return IsPhoto;
        }
		//Set Default Value
		public void SetDefaultIsPhoto(View view = null)
        {
            //if (IsPhoto is null){
            //    var result = GetDefaultIsPhoto(view);
            //    if (result != null && result != IsPhoto){
			//          IsPhoto = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool IsPhotoIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultIsPhoto();
				//if (result != null && IsPhoto != null){
				//	return !IsPhoto.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _textobjectgroup;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Văn bản nhóm ")]
        [ToolTip("Văn bản nhóm ")]
		//[Index(54)]		
		public bool TextObjectGroup
        { 
		    get => GetPropertyValue<bool>("TextObjectGroup");                         
			set => SetPropertyValue<bool>("TextObjectGroup", value); 
			
        }
		//Tooltip for Object
		public object TextObjectGroupToolTipControllerText(View view)
        {
        //    if (TextObjectGroup != null) 
		//			return TextObjectGroup;
            return null;
        }
		//Get Default Value
        public bool GetDefaultTextObjectGroup(View view = null)
        { 
			return TextObjectGroup;
        }
		//Set Default Value
		public void SetDefaultTextObjectGroup(View view = null)
        {
            //if (TextObjectGroup is null){
            //    var result = GetDefaultTextObjectGroup(view);
            //    if (result != null && result != TextObjectGroup){
			//          TextObjectGroup = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TextObjectGroupIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTextObjectGroup();
				//if (result != null && TextObjectGroup != null){
				//	return !TextObjectGroup.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _footnote;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("FootNote")]
        [ToolTip("FootNote")]
		//[Index(55)]		
	    [ImmediatePostData()]
		public bool FootNote
        { 
		    get => GetPropertyValue<bool>("FootNote");                         
			set => SetPropertyValue<bool>("FootNote", value); 
			
        }
		//Tooltip for Object
		public object FootNoteToolTipControllerText(View view)
        {
        //    if (FootNote != null) 
		//			return FootNote;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFootNote(View view = null)
        { 
			return FootNote;
        }
		//Set Default Value
		public void SetDefaultFootNote(View view = null)
        {
            //if (FootNote is null){
            //    var result = GetDefaultFootNote(view);
            //    if (result != null && result != FootNote){
			//          FootNote = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FootNoteIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFootNote();
				//if (result != null && FootNote != null){
				//	return !FootNote.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0440ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
SetDefaultDate();
SetDefaultCode();
SetDefaultMember();
SetDefaultElementSpacing();
            #endregion 0440ImportCode
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultDocumentType(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultDate(View view = null);
        //SetDefaultLanguageOrigin(View view = null);
        //SetDefaultLanguageTranslate(View view = null);
        //SetDefaultPath(View view = null);
        //SetDefaultStatus(View view = null);
        //SetDefaultFontColor(View view = null);
        //SetDefaultFontBold(View view = null);
        //SetDefaultFontItalic(View view = null);
        //SetDefaultFontUnderline(View view = null);
        //SetDefaultOutline(View view = null);
        //SetDefaultAlignment(View view = null);
        //SetDefaultSpacing(View view = null);
        //SetDefaultIndent(View view = null);
        //SetDefaultNodeFontColor(View view = null);
        //SetDefaultNodeFontBold(View view = null);
        //SetDefaultNodeFontItalic(View view = null);
        //SetDefaultNodeFontUnderline(View view = null);
        //SetDefaultNodeLink(View view = null);
        //SetDefaultNodeSuper(View view = null);
        //SetDefaultUpperElementImport(View view = null);
        //SetDefaultNumber(View view = null);
        //SetDefaultCheckSpelling(View view = null);
        //SetDefaultWithTermPosition(View view = null);
        //SetDefaultElementSpacing(View view = null);
        //SetDefaultBlankSpacing(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultCodeUnique(View view = null);
        //SetDefaultUpcaseNumbering(View view = null);
        //SetDefaultAbbyyTermLocation(View view = null);
        //SetDefaultKeepSpace(View view = null);
        //SetDefaultImportByNode(View view = null);
        //SetDefaultImportParagraph(View view = null);
        //SetDefaultRightIndent(View view = null);
        //SetDefaultOriginStyleExport(View view = null);
        //SetDefaultCreateWordStyle(View view = null);
        //SetDefaultOpen(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultBrLine(View view = null);
        //SetDefaultIsPhoto(View view = null);
        //SetDefaultTextObjectGroup(View view = null);
        //SetDefaultFootNote(View view = null);
			
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
            #region 0960ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0960ImportCode
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
            Session.Delete(this.TermList);				
            Session.Delete(this.ParagraphStyleList);				
            Session.Delete(this.FileList);				
  
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
				
                    case nameof(FootNote):
                        OnChangedFootNote(oldValue, newValue);
                        break;
				
                    case nameof(ImportByNode):
                        OnChangedImportByNode(oldValue, newValue);
                        break;
				
                    case nameof(OriginStyleExport):
                        OnChangedOriginStyleExport(oldValue, newValue);
                        break;
				
                    case nameof(CreateWordStyle):
                        OnChangedCreateWordStyle(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedFootNote(object oldValue, object newValue)
        {
            #region 1575ImportCode
            if (newValue is null) return;
                    if (ImportByNode && FootNote)
                        ImportByNode = false;            
            #endregion 1575ImportCode
        }               
        private void OnChangedImportByNode(object oldValue, object newValue)
        {
            #region 1574ImportCode
            if (newValue is null) return;
                    if (ImportByNode && FootNote)
                        FootNote = false;            
            #endregion 1574ImportCode
        }               
        private void OnChangedOriginStyleExport(object oldValue, object newValue)
        {
            #region 1582ImportCode
            if (newValue is null) return;
                    if (OriginStyleExport && CreateWordStyle)
                        CreateWordStyle = false;            
            #endregion 1582ImportCode
        }               
        private void OnChangedCreateWordStyle(object oldValue, object newValue)
        {
            #region 1583ImportCode
            if (newValue is null) return;
                    if (OriginStyleExport && CreateWordStyle)
                        OriginStyleExport = false;            
            #endregion 1583ImportCode
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
			//	SetDefaultAudioList();
			//	SetDefaultElementBatchList();
			//	SetDefaultTermList();
			//	SetDefaultTermLocationList();
			//	SetDefaultMediaList();
			//	SetDefaultParagraphList();
			//	SetDefaultParagraphStyleList();
			//	SetDefaultLanguageList();
			//	SetDefaultTranslateObjectList();
			//	SetDefaultFileList();
			//	SetDefaultNote();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0959ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0959            Oid: 35976f5e-63b8-45c0-9bce-56c38470f735
            Update= GetDefaultUpdate();
        }
#endregion 0959ImportCode
#region 1043ImportCode
		public void SetDefaultElementSpacing(View view = null)
        {
            //Code: 1043            Oid: dac8aa01-d3fe-4a65-ad95-5fcfb93b88c3
            if(ElementSpacing == null) ElementSpacing = GetDefaultElementSpacing();

        }
#endregion 1043ImportCode
#region 0961ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 0961            Oid: c1cab016-e695-4d93-934b-b77f4dea224f
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0961ImportCode
#region 1042ImportCode
		public int? GetDefaultElementSpacing(View view = null)
        {
            //Code: 1042            Oid: afc14cbf-7aef-4c77-92c8-9f3ab8653714
            return 500;
        }
#endregion 1042ImportCode
#region 0965ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 0965            Oid: 3aaaf203-e103-4f3a-96e2-1be38032599a
            if(Member == null) Member = GetDefaultMember();

        }
#endregion 0965ImportCode
#region 0964ImportCode
		public void SetDefaultCode(View view = null)
        {
            //Code: 0964            Oid: 518b400b-102b-453a-a004-02e71f17adb9
            if(String.IsNullOrEmpty(Code)) Code = GetDefaultCode();
        }
#endregion 0964ImportCode
#region 0926ImportCode
		public void SetDefaultDate(View view = null)
        {
            //Code: 0926            Oid: 00a0ce6b-dc50-4756-95ff-302fa92a5d31
            Date= GetDefaultDate();
        }
#endregion 0926ImportCode
#region 0925ImportCode
		public DateTime GetDefaultDate(View view = null)
        {
            //Code: 0925            Oid: 0d8f4daa-b67a-43e9-96d8-ce9a1cae333e
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0925ImportCode
#region 0963ImportCode
		public string GetDefaultCode(View view = null)
        {
            //Code: 0963            Oid: a06c5e41-3636-4e95-9d10-01cc2cc1e043
            var keyCodeObject = Module.Helpers.ParameterHelper.GetSettingParameter(Session, "CodeObject");
            var parser = string.Format("and Update >='{0}-01-01' and Update <'{1}-01-01'",
                            Update.Value.Year,
                            Update.Value.Year + 1);
            //Kích thước mặc định là 4 số
            int size = 3;
            return Tools.GetCode(this.GetType(), this.Session, this.Oid, keyCodeObject != null ? keyCodeObject.Value : "", size,
                parser, "Code", false);
            return null;
        }
#endregion 0963ImportCode
#region 0966ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 0966            Oid: 0d3ac554-c7fa-41fd-976c-04f12a2ae1ce
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);

        }
#endregion 0966ImportCode
        #endregion
//Mã nguồn bổ sung
#region VideoImportCode
        public System.Collections.Generic.List<Module.BusinessObjects.Audio> GetAudioListWithSort(bool asc = true)
        {
            //2025-02-22: Bỏ lựa chọn theo UpperElement
            //bool useUpperElement = AudioList.FirstOrDefault(m => m.UpperElement != null) != null;
            System.Collections.Generic.IEnumerable<Module.BusinessObjects.Audio> audioListWithSort = ImportByNode ? AudioList.Where(m => m.UpperElement != null) : AudioList;
            if (asc)
                return audioListWithSort.OrderBy(m => m.Start).ToList();
            else
                return audioListWithSort.OrderByDescending(m => m.Start).ToList();
        }

        public System.Collections.Generic.List<Module.BusinessObjects.Audio> GetAudioListWithSort(BookMark bookMark, bool asc = true, TranslateObject translateObject = null)
        {
            System.Collections.Generic.IEnumerable<Module.BusinessObjects.Audio> audioListWithSort = AudioList.Where(m => m.TranslateObject == translateObject && m.BookMark == bookMark);
            if (ImportByNode)
                audioListWithSort = audioListWithSort.Where(m => m.UpperElement != null);
            if (asc)
                return audioListWithSort.OrderBy(m => m.Start).ToList();
            else
                return audioListWithSort.OrderByDescending(m => m.Start).ToList();
        }

        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> dictionary = null;
        public System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> GetDictionary()
        {
            if (dictionary is null)
            {
                var criteria = LanguageOrigin != null ? DevExpress.Data.Filtering.CriteriaOperator.Parse("Language.Oid = ?", LanguageOrigin.Oid) : null;
                var words = new XPCollection<Word>(Session, criteria);
                if (words.Count > 0)
                {
                    //Nạp từ điển từ database
                    dictionary = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>>();
                    foreach (var word in words)
                    {
                        int wordLength = word.Name.Split(' ').Length;
                        if (!dictionary.ContainsKey(wordLength))
                        {
                            dictionary.Add(wordLength, new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>());
                        }
                        if (!dictionary[wordLength].ContainsKey(word.NoSignWord))
                        {
                            dictionary[wordLength].Add(word.NoSignWord, new System.Collections.Generic.List<string>());

                        }
                        dictionary[wordLength][word.NoSignWord].Add(word.Name);
                    }
                }

            }

            if (dictionary is null)
            {
                if (LanguageOrigin is null)
                {
                    throw new UserFriendlyException("Ngữ gốc không được phép trống");
                    return null;
                }
                string folder = Helpers.ParameterHelper.GetValueOrDefault(Session, "CheckDictionaryFolder", "\\\\rd\\CodeGen\\packages\\Dictionary");
                if (string.IsNullOrEmpty(folder))
                {

                    throw new UserFriendlyException("Không tìm thấy thư mục chứa từ điển, vui lòng kiểm tra lại tham số");
                    return null;
                }
                if (!folder.EndsWith("\\"))
                    folder += "\\";
                string fileName = folder + LanguageOrigin.Code + "Compounds.txt";
                if (!System.IO.File.Exists(fileName))
                {

                    throw new UserFriendlyException("Không tìm thấy từ điển từ ghép, vui lòng kiểm tra lại");
                    return null;
                }
                var wordsText = System.IO.File.ReadAllText(fileName);
                dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>>>(wordsText);
            }
            return dictionary;
        }

        public System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> GetDictionarySubtitle()
        {
            if (dictionary is null)
            {
                var criteria = LanguageTranslate != null ? DevExpress.Data.Filtering.CriteriaOperator.Parse("Language.Oid = ?", LanguageTranslate.Oid) : null;
                var words = new XPCollection<Word>(Session, criteria);
                if (words.Count > 0)
                {
                    //Nạp từ điển từ database
                    dictionary = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>>();
                    foreach (var word in words)
                    {
                        int wordLength = word.Name.Split(' ').Length;
                        if (!dictionary.ContainsKey(wordLength))
                        {
                            dictionary.Add(wordLength, new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>());
                        }
                        if (!dictionary[wordLength].ContainsKey(word.NoSignWord))
                        {
                            dictionary[wordLength].Add(word.NoSignWord, new System.Collections.Generic.List<string>());

                        }
                        dictionary[wordLength][word.NoSignWord].Add(word.Name);
                    }
                }

            }

            if (dictionary is null)
            {
                if (LanguageTranslate is null)
                {

                    throw new UserFriendlyException("Ngữ gốc không được phép trống");
                    return null;
                }
                string folder = Helpers.ParameterHelper.GetValueOrDefault(Session, "CheckDictionaryFolder", "\\\\rd\\CodeGen\\packages\\Dictionary");
                if (string.IsNullOrEmpty(folder))
                {

                    throw new UserFriendlyException("Không tìm thấy thư mục chứa từ điển, vui lòng kiểm tra lại tham số");
                    return null;
                }
                if (!folder.EndsWith("\\"))
                    folder += "\\";
                string fileName = folder + LanguageTranslate.Code + "Compounds.txt";
                if (!System.IO.File.Exists(fileName))
                {

                    throw new UserFriendlyException("Không tìm thấy từ điển từ ghép, vui lòng kiểm tra lại");
                    return null;
                }
                var wordsText = System.IO.File.ReadAllText(fileName);
                dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>>>(wordsText);
            }
            return dictionary;
        }
        private char[] splitChar = new[] { ',', '\r', '\n' };
        private string[] upperCaseAcceptWordsOrigin = null;
        private string[] upperCaseAcceptWordsTranslate = null;
        public string[] GetUpperCaseAcceptWords(bool origin)
        {
            if (origin)
            {
                if (upperCaseAcceptWordsOrigin != null)
                    return upperCaseAcceptWordsOrigin;
            }
            else
            {
                if (upperCaseAcceptWordsTranslate != null)
                    return upperCaseAcceptWordsTranslate;
            }
            var languageCode = origin ? LanguageOrigin?.Code : LanguageTranslate?.Code;
            if (!string.IsNullOrEmpty(languageCode))
            {
                var defaultValue = languageCode.Equals("VN", StringComparison.OrdinalIgnoreCase) ? "và, hoặc, của, qua, trong, với, tới, cho" : "and, or, of, via, in, with, to, for";
                var key = "UpperCaseAcceptWords" + languageCode;
                string upperCaseAcceptWords = Helpers.ParameterHelper.GetValueOrDefault(Session, key, defaultValue);
                upperCaseAcceptWords = upperCaseAcceptWords.Replace(" ", "");
                if (origin)
                    upperCaseAcceptWordsOrigin = upperCaseAcceptWords.Split(splitChar, System.StringSplitOptions.RemoveEmptyEntries);
                else
                    upperCaseAcceptWordsTranslate = upperCaseAcceptWords.Split(splitChar, System.StringSplitOptions.RemoveEmptyEntries);
            }
            if (origin)
                return upperCaseAcceptWordsOrigin;
            else
                return upperCaseAcceptWordsTranslate;
        }
#endregion VideoImportCode
		 		 
    }
}
