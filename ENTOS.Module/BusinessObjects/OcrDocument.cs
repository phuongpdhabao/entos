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
	[NavigationItem("DataManagement")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Tài liệu nhận dạng"), ImageName("OcrDocument")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("OcrDocument OcrValueList, Markdown, OcrJson Hide_None__" , TargetItems = "OcrValueList, Markdown, OcrJson" , Criteria = "[MultiPage] = False",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Code)+ "," + nameof(Member)+ "," + nameof(MemberFolder)+ "," + nameof(Update)+ "," + nameof(Updater)+ "," + nameof(CreatedDate))]
 
	[DefaultProperty("DocumentLink")]
 
[OptimisticLocking(true)]
    public partial class OcrDocument:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public OcrDocument(Session session)
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
				if (OcrPageList.IsLoaded)
                {
                    if (OcrPageList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(OcrPageList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(OcrPageList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.OcrPage>(CriteriaOperator.Parse("[OcrDocument.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool ocrpagelist = Session.Query<Module.BusinessObjects.OcrPage>().Where(x => x.OcrDocument.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(OcrPageList), ocrpagelist);
                        if (ocrpagelist)
                            return true;

                    }                    
                }				
				if (OcrValueList.IsLoaded)
                {
                    if (OcrValueList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(OcrValueList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(OcrValueList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.OcrValue>(CriteriaOperator.Parse("[OcrDocument.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool ocrvaluelist = Session.Query<Module.BusinessObjects.OcrValue>().Where(x => x.OcrDocument.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(OcrValueList), ocrvaluelist);
                        if (ocrvaluelist)
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
               

		//private string _documentlink;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tài liệu")]
        [ToolTip("Tài liệu")]
		//[Index(0)]		

 		[Size(250)]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string DocumentLink
        { 
		    get => GetPropertyValue<string>("DocumentLink");                         
			set => SetPropertyValue<string>("DocumentLink", value); 
			
        }
		//Tooltip for Object
		public object DocumentLinkToolTipControllerText(View view)
        {
        //    if (DocumentLink != null) 
		//			return DocumentLink;
            return null;
        }
		//Get Default Value
        public string GetDefaultDocumentLink(View view = null)
        { 
			return DocumentLink;
        }
		//Set Default Value
		public void SetDefaultDocumentLink(View view = null)
        {
            //if (DocumentLink is null){
            //    var result = GetDefaultDocumentLink(view);
            //    if (result != null && result != DocumentLink){
			//          DocumentLink = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DocumentLinkIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDocumentLink();
				//if (result != null && DocumentLink != null){
				//	return !DocumentLink.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.ExtractionTemplate _extractiontemplate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(1)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ExtractionTemplateCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.ExtractionTemplate ExtractionTemplate
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ExtractionTemplate>("ExtractionTemplate");                         
			set => SetPropertyValue<Module.BusinessObjects.ExtractionTemplate>("ExtractionTemplate", value); 
			
        }
		//Tooltip for Object
		public object ExtractionTemplateToolTipControllerText(View view)
        {
        //    if (ExtractionTemplate != null) 
		//			return ExtractionTemplate;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ExtractionTemplate GetDefaultExtractionTemplate(View view = null)
        { 
			return ExtractionTemplate;
        }
		//Set Default Value
		public void SetDefaultExtractionTemplate(View view = null)
        {
            //if (ExtractionTemplate is null){
            //    var result = GetDefaultExtractionTemplate(view);
            //    if (result != null && result != ExtractionTemplate){
			//          ExtractionTemplate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ExtractionTemplateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultExtractionTemplate();
				//if (result != null && ExtractionTemplate != null){
				//	return !ExtractionTemplate.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ExtractionTemplateCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ExtractionTemplate));
            }
        }
	
       
		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(2)]		

 		[Size(20)]
		[RuleUniqueValue("UniqueOcrDocumentCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredOcrDocumentCode", DefaultContexts.Save)]
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
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
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
	
       
		//private Module.BusinessObjects.MemberFolder _memberfolder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(4)]		
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
		//Set Default Value

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
	
       
		//private bool _multipage;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Một đối tượng")]
        [ToolTip("Một đối tượng")]
		//[Index(5)]		
	    [ImmediatePostData()]
		public bool MultiPage
        { 
		    get => GetPropertyValue<bool>("MultiPage");                         
			set => SetPropertyValue<bool>("MultiPage", value); 
			
        }
		//Tooltip for Object
		public object MultiPageToolTipControllerText(View view)
        {
        //    if (MultiPage != null) 
		//			return MultiPage;
            return null;
        }
		//Get Default Value
        public bool GetDefaultMultiPage(View view = null)
        { 
			return MultiPage;
        }
		//Set Default Value
		public void SetDefaultMultiPage(View view = null)
        {
            //if (MultiPage is null){
            //    var result = GetDefaultMultiPage(view);
            //    if (result != null && result != MultiPage){
			//          MultiPage = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MultiPageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMultiPage();
				//if (result != null && MultiPage != null){
				//	return !MultiPage.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trang")]
		//[Index(6)]
		[DevExpress.Xpo.Association("OcrDocument-OcrPageList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.OcrPage> OcrPageList
        {      
		    get => GetCollection<Module.BusinessObjects.OcrPage>("OcrPageList"); 
			
        }
       
		//private string _ocrjson;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Json")]
        [ToolTip("Json")]
		//[Index(7)]		

 		[Size(SizeAttribute.Unlimited)]
		public string OcrJson
        { 
		    get => GetPropertyValue<string>("OcrJson");                         
			set => SetPropertyValue<string>("OcrJson", value); 
			
        }
		//Tooltip for Object
		public object OcrJsonToolTipControllerText(View view)
        {
        //    if (OcrJson != null) 
		//			return OcrJson;
            return null;
        }
		//Get Default Value
        public string GetDefaultOcrJson(View view = null)
        { 
			return OcrJson;
        }
		//Set Default Value
		public void SetDefaultOcrJson(View view = null)
        {
            //if (OcrJson is null){
            //    var result = GetDefaultOcrJson(view);
            //    if (result != null && result != OcrJson){
			//          OcrJson = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OcrJsonIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOcrJson();
				//if (result != null && OcrJson != null){
				//	return !OcrJson.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _markdown;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hiển thị")]
        [ToolTip("Hiển thị")]
		//[Index(8)]		

 		[Size(SizeAttribute.Unlimited)]
	    [NotMapped()]
	    [ModelDefault("PropertyEditorType", "MarkdownPropertyEditor")]
	    [ModelDefault("AllowEdit", "False")]
	    [NonPersistent()]
		public string Markdown
        { 
		    get => GetPropertyValue<string>("Markdown");                         
			set => SetPropertyValue<string>("Markdown", value); 
			
        }
		//Tooltip for Object
		public object MarkdownToolTipControllerText(View view)
        {
        //    if (Markdown != null) 
		//			return Markdown;
            return null;
        }
		//Get Default Value
        public string GetDefaultMarkdown(View view = null)
        { 
			return Markdown;
        }
		//Set Default Value
		public void SetDefaultMarkdown(View view = null)
        {
            //if (Markdown is null){
            //    var result = GetDefaultMarkdown(view);
            //    if (result != null && result != Markdown){
			//          Markdown = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MarkdownIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMarkdown();
				//if (result != null && Markdown != null){
				//	return !Markdown.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá trị")]
		//[Index(9)]
		[DevExpress.Xpo.Association("OcrDocument-OcrValueList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.OcrValue> OcrValueList
        {      
		    get => GetCollection<Module.BusinessObjects.OcrValue>("OcrValueList"); 
			
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
		//[Index(11)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpdaterCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [ModelDefault("AllowEdit", "False")]
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
	
       
		//private DateTime? _createddate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày tạo")]
        [ToolTip("Ngày tạo")]
		//[Index(12)]		
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

	
       
		//private System.Type _systemtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu hệ thống")]
        [ToolTip("Kiểu hệ thống")]
		//[Index(13)]		
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
	
       
		//private System.Guid? _objectid;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã đối tượng")]
        [ToolTip("Mã đối tượng")]
		//[Index(14)]		
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

	
       
		//private string _ocrmarkdown;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ocr Markdown")]
        [ToolTip("Ocr Markdown")]
		//[Index(15)]		

 		[Size(SizeAttribute.Unlimited)]
		public string OcrMarkdown
        { 
		    get => GetPropertyValue<string>("OcrMarkdown");                         
			set => SetPropertyValue<string>("OcrMarkdown", value); 
			
        }
		//Tooltip for Object
		public object OcrMarkdownToolTipControllerText(View view)
        {
        //    if (OcrMarkdown != null) 
		//			return OcrMarkdown;
            return null;
        }
		//Get Default Value
        public string GetDefaultOcrMarkdown(View view = null)
        { 
			return OcrMarkdown;
        }
		//Set Default Value
		public void SetDefaultOcrMarkdown(View view = null)
        {
            //if (OcrMarkdown is null){
            //    var result = GetDefaultOcrMarkdown(view);
            //    if (result != null && result != OcrMarkdown){
			//          OcrMarkdown = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OcrMarkdownIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOcrMarkdown();
				//if (result != null && OcrMarkdown != null){
				//	return !OcrMarkdown.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _valuemarkdown;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Value Markdown")]
        [ToolTip("Value Markdown")]
		//[Index(16)]		

 		[Size(SizeAttribute.Unlimited)]
		public string ValueMarkdown
        { 
		    get => GetPropertyValue<string>("ValueMarkdown");                         
			set => SetPropertyValue<string>("ValueMarkdown", value); 
			
        }
		//Tooltip for Object
		public object ValueMarkdownToolTipControllerText(View view)
        {
        //    if (ValueMarkdown != null) 
		//			return ValueMarkdown;
            return null;
        }
		//Get Default Value
        public string GetDefaultValueMarkdown(View view = null)
        { 
			return ValueMarkdown;
        }
		//Set Default Value
		public void SetDefaultValueMarkdown(View view = null)
        {
            //if (ValueMarkdown is null){
            //    var result = GetDefaultValueMarkdown(view);
            //    if (result != null && result != ValueMarkdown){
			//          ValueMarkdown = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ValueMarkdownIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultValueMarkdown();
				//if (result != null && ValueMarkdown != null){
				//	return !ValueMarkdown.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 3716ImportCode
            base.AfterConstruction();
SetDefaultCreatedDate();
SetDefaultUpdate();
SetDefaultMember();
SetDefaultCode();
            #endregion 3716ImportCode
 
        //SetDefaultDocumentLink(View view = null);
        //SetDefaultExtractionTemplate(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultMultiPage(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultCreatedDate(View view = null);
        //SetDefaultSystemType(View view = null);
        //SetDefaultObjectID(View view = null);
        //SetDefaultOcrMarkdown(View view = null);
        //SetDefaultValueMarkdown(View view = null);
			
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
            #region 3719ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 3719ImportCode
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
				
                    case nameof(ValueMarkdown):
                        OnChangedValueMarkdown(oldValue, newValue);
                        break;
				
                    case nameof(OcrMarkdown):
                        OnChangedOcrMarkdown(oldValue, newValue);
                        break;
				
                    case nameof(Member):
                        OnChangedMember(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedValueMarkdown(object oldValue, object newValue)
        {
            #region 3898ImportCode
            if (newValue is null) return;
Markdown = (string)newValue;            
            #endregion 3898ImportCode
        }               
        private void OnChangedOcrMarkdown(object oldValue, object newValue)
        {
            #region 3897ImportCode
            if (newValue is null) return;
Markdown = (string)newValue;            
            #endregion 3897ImportCode
        }               
        private void OnChangedMember(object oldValue, object newValue)
        {
            #region 3745ImportCode
            if (newValue is null) return;
SetDefaultMemberFolder();            
            #endregion 3745ImportCode
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
			//	SetDefaultOcrPageList();
			//	SetDefaultOcrJson();
			//	SetDefaultMarkdown();
			//	SetDefaultOcrValueList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 3739ImportCode
		public string GetDefaultCode(View view = null)
        {
            //Code: 3739            Oid: 4d434e85-9224-4ee7-a6a9-3384216fa7e3
            if(CreatedDate is null) return null;
var keyCodeObject =
    Module.Helpers.ParameterHelper.GetSettingParameter(Session, "CodeObject");
var parser = string.Format("and CreatedDate >='{0}-01-01' and CreatedDate <'{1}-01-01'",
                    CreatedDate.Value.Year,
                    CreatedDate.Value.Year + 1);
    if (MemberFolder != null) parser += string.Format(" and MemberFolder.Oid = '{0}' ", MemberFolder.Oid);

//Trường hợp chỉ lấy mã trên đối tượng này
Type currentType = this.GetType();
//Trường hợp lấy mã từ đối tượng cha
//Type currentType = typeof(ObjectType);

    //Kích thước mặc định là 4 số
    int size = 3;		
    return Tools.GetCode(currentType , this.Session, this.Oid, keyCodeObject != null ? keyCodeObject.Value : "", size ,
        parser);
return null;
        }
#endregion 3739ImportCode
#region 3740ImportCode
		public DateTime? GetDefaultCreatedDate(View view = null)
        {
            //Code: 3740            Oid: 3fa8951b-f70c-4003-ab74-d92dcc063f89
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 3740ImportCode
#region 3720ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 3720            Oid: c6a8ec57-fd6c-4d27-a82e-4f8798252a3c
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 3720ImportCode
#region 3722ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 3722            Oid: 61eb0dc4-b277-4f9d-a043-400399497382
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3722ImportCode
#region 3744ImportCode
		public void SetDefaultMemberFolder(View view = null)
        {
            //Code: 3744            Oid: ee21250b-e82a-4167-bfe2-f94f3e8d003b
            MemberFolder = GetDefaultMemberFolder();
        }
#endregion 3744ImportCode
#region 3742ImportCode
		public void SetDefaultCode(View view = null)
        {
            //Code: 3742            Oid: 4f3a1b1f-555e-4785-ad56-220e34d76e6c
            Code= GetDefaultCode();
        }
#endregion 3742ImportCode
#region 3741ImportCode
		public void SetDefaultCreatedDate(View view = null)
        {
            //Code: 3741            Oid: 70652d46-8274-48a4-8faf-2b95ec681f64
            if(CreatedDate == null) CreatedDate = GetDefaultCreatedDate();
        }
#endregion 3741ImportCode
#region 3718ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 3718            Oid: 86262b20-59b4-45fd-a7e7-af5164c0bd6a
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 3718ImportCode
#region 3717ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 3717            Oid: acbfeaae-54fd-4a40-a25f-65e219407487
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3717ImportCode
#region 3721ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 3721            Oid: ec437dab-629a-4bbb-8e64-a2216cd895d2
            Updater = GetDefaultUpdater();
        }
#endregion 3721ImportCode
#region 3743ImportCode
		public Module.BusinessObjects.MemberFolder GetDefaultMemberFolder(View view = null)
        {
            //Code: 3743            Oid: 37dc19a8-6341-4e8a-8893-87a3d71b20f3
            if(Member is null)
return null;
return Member.MemberFolder;
        }
#endregion 3743ImportCode
#region 3715ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 3715            Oid: cf11cc4b-3420-4a59-b503-103157b1cbff
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 3715ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
