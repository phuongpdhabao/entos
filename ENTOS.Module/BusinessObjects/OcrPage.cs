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
    [ModelDefault("Caption", "Trang nhận dạng"), ImageName("OcrPage")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Code)+ "," + nameof(Member)+ "," + nameof(MemberFolder)+ "," + nameof(Update)+ "," + nameof(Updater)+ "," + nameof(CreatedDate)+ "," + nameof(Order))]
 
	[DefaultProperty("PageLink")]
 
[OptimisticLocking(true)]
    public partial class OcrPage:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public OcrPage(Session session)
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
				if (OcrValue.IsLoaded)
                {
                    if (OcrValue.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(OcrValue)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(OcrValue)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.OcrValue>(CriteriaOperator.Parse("[OcrPage.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool ocrvalue = Session.Query<Module.BusinessObjects.OcrValue>().Where(x => x.OcrPage.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(OcrValue), ocrvalue);
                        if (ocrvalue)
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
               

		//private string _pagelink;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Trang")]
        [ToolTip("Trang")]
		//[Index(0)]		

 		[Size(250)]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string PageLink
        { 
		    get => GetPropertyValue<string>("PageLink");                         
			set => SetPropertyValue<string>("PageLink", value); 
			
        }
		//Tooltip for Object
		public object PageLinkToolTipControllerText(View view)
        {
        //    if (PageLink != null) 
		//			return PageLink;
            return null;
        }
		//Get Default Value
        public string GetDefaultPageLink(View view = null)
        { 
			return PageLink;
        }
		//Set Default Value
		public void SetDefaultPageLink(View view = null)
        {
            //if (PageLink is null){
            //    var result = GetDefaultPageLink(view);
            //    if (result != null && result != PageLink){
			//          PageLink = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PageLinkIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPageLink();
				//if (result != null && PageLink != null){
				//	return !PageLink.Equals(result);
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
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(2)]		

 		[Size(20)]
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
	
       
		//private bool _multiobject;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhiều đối tượng")]
        [ToolTip("Nhiều đối tượng")]
		//[Index(5)]		
		public bool MultiObject
        { 
		    get => GetPropertyValue<bool>("MultiObject");                         
			set => SetPropertyValue<bool>("MultiObject", value); 
			
        }
		//Tooltip for Object
		public object MultiObjectToolTipControllerText(View view)
        {
        //    if (MultiObject != null) 
		//			return MultiObject;
            return null;
        }
		//Get Default Value
        public bool GetDefaultMultiObject(View view = null)
        { 
			return MultiObject;
        }
		//Set Default Value
		public void SetDefaultMultiObject(View view = null)
        {
            //if (MultiObject is null){
            //    var result = GetDefaultMultiObject(view);
            //    if (result != null && result != MultiObject){
			//          MultiObject = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MultiObjectIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMultiObject();
				//if (result != null && MultiObject != null){
				//	return !MultiObject.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _ocrjson;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Json")]
        [ToolTip("Json")]
		//[Index(6)]		

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
		//[Index(7)]		

 		[Size(SizeAttribute.Unlimited)]
	    [NonPersistent()]
	    [ModelDefault("PropertyEditorType", "MarkdownPropertyEditor")]
	    [NotMapped()]
	    [ModelDefault("AllowEdit", "False")]
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
		//[Index(8)]
		[DevExpress.Xpo.Association("OcrPage-OcrValue")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.OcrValue> OcrValue
        {      
		    get => GetCollection<Module.BusinessObjects.OcrValue>("OcrValue"); 
			
        }
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(9)]		
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
		//[Index(10)]		
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
		//[Index(11)]		
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

	
       
		//private Module.BusinessObjects.OcrDocument _ocrdocument;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài liệu nhận dạng")]
        [ToolTip("Tài liệu nhận dạng")]
		//[Index(12)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OcrDocumentCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("OcrDocument-OcrPageList")]
	 
		public Module.BusinessObjects.OcrDocument OcrDocument
        { 
		    get => GetPropertyValue<Module.BusinessObjects.OcrDocument>("OcrDocument");                         
			set => SetPropertyValue<Module.BusinessObjects.OcrDocument>("OcrDocument", value); 
			
        }
		//Tooltip for Object
		public object OcrDocumentToolTipControllerText(View view)
        {
        //    if (OcrDocument != null) 
		//			return OcrDocument;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.OcrDocument GetDefaultOcrDocument(View view = null)
        { 
			return OcrDocument;
        }
		//Set Default Value
		public void SetDefaultOcrDocument(View view = null)
        {
            //if (OcrDocument is null){
            //    var result = GetDefaultOcrDocument(view);
            //    if (result != null && result != OcrDocument){
			//          OcrDocument = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OcrDocumentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOcrDocument();
				//if (result != null && OcrDocument != null){
				//	return !OcrDocument.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator OcrDocumentCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(OcrDocument));
            }
        }
	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(13)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
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
		//Set Default Value

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

	
       
		//private System.Type _systemtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu hệ thống")]
        [ToolTip("Kiểu hệ thống")]
		//[Index(14)]		
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
		//[Index(15)]		
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
		//[Index(16)]		

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
		//[Index(17)]		

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
 
            #region 3711ImportCode
            base.AfterConstruction();
SetDefaultCreatedDate();
SetDefaultUpdate();
SetDefaultMember();
            #endregion 3711ImportCode
 
        //SetDefaultPageLink(View view = null);
        //SetDefaultExtractionTemplate(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultMultiObject(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultCreatedDate(View view = null);
        //SetDefaultOcrDocument(View view = null);
        //SetDefaultOrder(View view = null);
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
            #region 3710ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
SetDefaultCode();
            #endregion 3710ImportCode
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
				
                    case nameof(Member):
                        OnChangedMember(oldValue, newValue);
                        break;
				
                    case nameof(OcrDocument):
                        OnChangedOcrDocument(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedMember(object oldValue, object newValue)
        {
            #region 3746ImportCode
            if (newValue is null) return;
SetDefaultMemberFolder();            
            #endregion 3746ImportCode
        }               
        private void OnChangedOcrDocument(object oldValue, object newValue)
        {
            #region 3729ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 3729ImportCode
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
			//	SetDefaultOcrJson();
			//	SetDefaultMarkdown();
			//	SetDefaultOcrValue();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 3733ImportCode
		public void SetDefaultCode(View view = null)
        {
            //Code: 3733            Oid: 6791fbef-cf2b-48ce-bf0c-3fd0c2c0076c
            if(OcrDocument is null)
Code = GetDefaultCode();
        }
#endregion 3733ImportCode
#region 3728ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 3728            Oid: 92696999-cb9c-42df-91d5-a236d3eece69
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3728ImportCode
#region 3738ImportCode
		public void SetDefaultMemberFolder(View view = null)
        {
            //Code: 3738            Oid: 73c0f10a-3dae-4f92-9d9a-e3a4920a421e
            MemberFolder= GetDefaultMemberFolder();
        }
#endregion 3738ImportCode
#region 3714ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 3714            Oid: c15bd6c5-5635-45f1-ab19-c3367b0c3e79
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3714ImportCode
#region 3730ImportCode
		public string GetDefaultCode(View view = null)
        {
            //Code: 3730            Oid: 12fbd047-4dcd-4fc0-b360-fde9b7bdd18f
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
#endregion 3730ImportCode
#region 3709ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 3709            Oid: b32e76cf-a29c-40e4-94e9-8071d3d1b330
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 3709ImportCode
#region 3727ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 3727            Oid: 1f5f5fa3-3791-4ca8-92a9-af7ca9b44fea
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 3727ImportCode
#region 3732ImportCode
		public void SetDefaultCreatedDate(View view = null)
        {
            //Code: 3732            Oid: 1bd01af3-5fec-4777-9285-cab8dc74b4dc
            if(CreatedDate == null) CreatedDate = GetDefaultCreatedDate();
        }
#endregion 3732ImportCode
#region 3713ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 3713            Oid: 933f6e5d-4496-4d2a-833e-94f257c37722
            Updater = GetDefaultUpdater();
        }
#endregion 3713ImportCode
#region 3712ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 3712            Oid: 8d56d4e2-1842-43a2-878e-a416e99ba148
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 3712ImportCode
#region 3708ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 3708            Oid: fb32b851-967c-426e-bc89-55993b18bf33
            Order= GetDefaultOrder();
        }
#endregion 3708ImportCode
#region 3737ImportCode
		public Module.BusinessObjects.MemberFolder GetDefaultMemberFolder(View view = null)
        {
            //Code: 3737            Oid: 2d7786da-9e51-479a-b61d-3875d53bc96b
            if(Member is null)
return null;
return Member.MemberFolder;
        }
#endregion 3737ImportCode
#region 3731ImportCode
		public DateTime? GetDefaultCreatedDate(View view = null)
        {
            //Code: 3731            Oid: 5d657c51-78a2-4ce8-bf1d-87cd6fb97c5d
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 3731ImportCode
#region 3707ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 3707            Oid: 5955a39f-7896-457f-a21f-d3df44d3b1a3
            if (OcrDocument != null && OcrDocument.OcrPageList != null)
{
    var lasted = OcrDocument.OcrPageList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
return null;
        }
#endregion 3707ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
