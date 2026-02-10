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
    [ModelDefault("Caption", "Cấu hình dữ liệu trích xuất"), ImageName("ExtractorDataConfiguration")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("ExtractorDataConfiguration Code None_Disable__" , TargetItems = "Code" , Criteria = "[ExtractorDataBehavior] = ##ToString#Number# Or [ExtractorDataBehavior] = ##ToString#Date#",AppearanceItemType = "ViewItem", Enabled = false )]
	[Appearance("ExtractorDataConfiguration Language None_Disable__" , TargetItems = "Language" , Criteria = "[ExtractorDataBehavior] = ##ToString#Left# And [ExtractorDataBehavior] = ##ToString#Right# And [ExtractorDataBehavior] = ##ToString#Replace#",AppearanceItemType = "ViewItem", Enabled = false )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "WebExtractor_ExtractorDataConfigurationList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ExtractorDataConfiguration_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ExtractorDataConfiguration_ListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class ExtractorDataConfiguration:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ExtractorDataConfiguration(Session session)
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

           
        }
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)

		[Key(true)]
		[VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]     
        public Guid Oid { get; set; }
               

		//private Module.BusinessObjects.ExtractorItem _extractoritem;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Chi tiết Trích web")]
        [ToolTip("Chi tiết Trích web")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ExtractorItemCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		[RuleRequiredField("RequiredExtractorDataConfigurationExtractorItem", DefaultContexts.Save)]
		public Module.BusinessObjects.ExtractorItem ExtractorItem
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ExtractorItem>("ExtractorItem");                         
			set => SetPropertyValue<Module.BusinessObjects.ExtractorItem>("ExtractorItem", value); 
			
        }
		//Tooltip for Object
		public object ExtractorItemToolTipControllerText(View view)
        {
        //    if (ExtractorItem != null) 
		//			return ExtractorItem;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ExtractorItem GetDefaultExtractorItem(View view = null)
        { 
			return ExtractorItem;
        }
		//Set Default Value
		public void SetDefaultExtractorItem(View view = null)
        {
            //if (ExtractorItem is null){
            //    var result = GetDefaultExtractorItem(view);
            //    if (result != null && result != ExtractorItem){
			//          ExtractorItem = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ExtractorItemIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultExtractorItem();
				//if (result != null && ExtractorItem != null){
				//	return !ExtractorItem.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ExtractorItemCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ExtractorItem));
            }
        }
	
       
		//private ExtractorDataBehavior _extractordatabehavior;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Hành xử dữ liệu trích xuất")]
        [ToolTip("Hành xử dữ liệu trích xuất")]
		//[Index(1)]		
		public ExtractorDataBehavior ExtractorDataBehavior
        { 
		    get => GetPropertyValue<ExtractorDataBehavior>("ExtractorDataBehavior");                         
			set => SetPropertyValue<ExtractorDataBehavior>("ExtractorDataBehavior", value); 
			
        }
		//Tooltip for Object
		public object ExtractorDataBehaviorToolTipControllerText(View view)
        {
        //    if (ExtractorDataBehavior != null) 
		//			return ExtractorDataBehavior;
            return null;
        }
		//Get Default Value
        public ExtractorDataBehavior GetDefaultExtractorDataBehavior(View view = null)
        { 
			return ExtractorDataBehavior;
        }
		//Set Default Value
		public void SetDefaultExtractorDataBehavior(View view = null)
        {
            //if (ExtractorDataBehavior is null){
            //    var result = GetDefaultExtractorDataBehavior(view);
            //    if (result != null && result != ExtractorDataBehavior){
			//          ExtractorDataBehavior = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ExtractorDataBehaviorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultExtractorDataBehavior();
				//if (result != null && ExtractorDataBehavior != null){
				//	return !ExtractorDataBehavior.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mô tả")]
        [ToolTip("Mô tả")]
		//[Index(2)]		

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

	
       
		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giá trị")]
		[ToolTip("Giá trị ngăn cách lấy trái, phải (được ngăn bởi dấu ;)")]
		//[Index(3)]		

 		[Size(30)]
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

	
       
		//private Module.BusinessObjects.Language _language;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Định dạng")]
		[ToolTip("Chọn ngôn ngữ muốn hiển thị dữ liệu")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LanguageCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Language Language
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Language>("Language");                         
			set => SetPropertyValue<Module.BusinessObjects.Language>("Language", value); 
			
        }
		//Tooltip for Object
		public object LanguageToolTipControllerText(View view)
        {
        //    if (Language != null) 
		//			return Language;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Language GetDefaultLanguage(View view = null)
        { 
			return Language;
        }
		//Set Default Value
		public void SetDefaultLanguage(View view = null)
        {
            //if (Language is null){
            //    var result = GetDefaultLanguage(view);
            //    if (result != null && result != Language){
			//          Language = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LanguageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLanguage();
				//if (result != null && Language != null){
				//	return !Language.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LanguageCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Language));
            }
        }
	
       
		//private Module.BusinessObjects.WebExtractor _webextractor;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trích xuất Web")]
        [ToolTip("Trích xuất Web")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(WebExtractorCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("WebExtractor-ExtractorDataConfigurationList")]
	 
		public Module.BusinessObjects.WebExtractor WebExtractor
        { 
		    get => GetPropertyValue<Module.BusinessObjects.WebExtractor>("WebExtractor");                         
			set => SetPropertyValue<Module.BusinessObjects.WebExtractor>("WebExtractor", value); 
			
        }
		//Tooltip for Object
		public object WebExtractorToolTipControllerText(View view)
        {
        //    if (WebExtractor != null) 
		//			return WebExtractor;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.WebExtractor GetDefaultWebExtractor(View view = null)
        { 
			return WebExtractor;
        }
		//Set Default Value
		public void SetDefaultWebExtractor(View view = null)
        {
            //if (WebExtractor is null){
            //    var result = GetDefaultWebExtractor(view);
            //    if (result != null && result != WebExtractor){
			//          WebExtractor = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WebExtractorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWebExtractor();
				//if (result != null && WebExtractor != null){
				//	return !WebExtractor.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator WebExtractorCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(WebExtractor));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultExtractorItem(View view = null);
        //SetDefaultExtractorDataBehavior(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultLanguage(View view = null);
        //SetDefaultWebExtractor(View view = null);
			
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
             base.OnSaving();
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
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
