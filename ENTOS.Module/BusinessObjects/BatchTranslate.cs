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
    [ModelDefault("Caption", "Dịch lô"), ImageName("BatchTranslate")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("BatchTranslate TranslateLineQuantity None_None__Color [A=255, R=255, G=0, B=0]" , TargetItems = "TranslateLineQuantity" , Criteria = "[LineQuantity] <> [TranslateLineQuantity]",AppearanceItemType = "ViewItem", FontColor = "#FF0000" )]
	[Appearance("BatchTranslate Translate2LineQuantity None_None__Color [A=255, R=255, G=0, B=0]" , TargetItems = "Translate2LineQuantity" , Criteria = "[LineQuantity] <> [Translate2LineQuantity]",AppearanceItemType = "ViewItem", FontColor = "#FF0000" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(Translate2), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
 
 
	[DefaultProperty("Language")]
 
[OptimisticLocking(true)]
    public partial class BatchTranslate:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public BatchTranslate(Session session)
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
               

		//private Module.BusinessObjects.Language _language;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngữ dịch")]
        [ToolTip("Ngữ dịch")]
		//[Index(0)]		
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
	
       
		//private int? _linequantity;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dòng xuôi")]
        [ToolTip("Dòng xuôi")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? LineQuantity
        { 
		    #region 3332ImportCode 
        get
        {
            if (string.IsNullOrEmpty(Content))
                return null;

            return Content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
        }
#endregion 3332ImportCode
			
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

	
       
		//private string _content;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dịch xuôi")]
        [ToolTip("Dịch xuôi")]
		//[Index(2)]		

 		[Size(SizeAttribute.Unlimited)]
	    [ModelDefault("PropertyEditorType", "SafeCSCodePropertyEditor")]
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

	
       
		//private Module.BusinessObjects.Language _originlanguage;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngữ gốc")]
        [ToolTip("Ngữ gốc")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OriginLanguageCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Language OriginLanguage
        { 
		    #region 3339ImportCode 
        get
        {
            return ElementBatch?.Video?.LanguageOrigin;
        }
#endregion 3339ImportCode
			
        }
		//Tooltip for Object
		public object OriginLanguageToolTipControllerText(View view)
        {
        //    if (OriginLanguage != null) 
		//			return OriginLanguage;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Language GetDefaultOriginLanguage(View view = null)
        { 
			return OriginLanguage;
        }
		//Set Default Value
		public void SetDefaultOriginLanguage(View view = null)
        {
            //if (OriginLanguage is null){
            //    var result = GetDefaultOriginLanguage(view);
            //    if (result != null && result != OriginLanguage){
			//          OriginLanguage = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OriginLanguageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOriginLanguage();
				//if (result != null && OriginLanguage != null){
				//	return !OriginLanguage.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator OriginLanguageCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(OriginLanguage));
            }
        }
	
       
		//private int? _translatelinequantity;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dòng ngược")]
        [ToolTip("Dòng ngược")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? TranslateLineQuantity
        { 
		    #region 3338ImportCode 
        get
        {
            if (string.IsNullOrEmpty(Translate))
                return null;

            return Translate.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
        }
#endregion 3338ImportCode
			
        }
		//Tooltip for Object
		public object TranslateLineQuantityToolTipControllerText(View view)
        {
        //    if (TranslateLineQuantity != null) 
		//			return TranslateLineQuantity;
            return null;
        }
		//Get Default Value
        public int? GetDefaultTranslateLineQuantity(View view = null)
        { 
			return TranslateLineQuantity;
        }
		//Set Default Value
		public void SetDefaultTranslateLineQuantity(View view = null)
        {
            //if (TranslateLineQuantity is null){
            //    var result = GetDefaultTranslateLineQuantity(view);
            //    if (result != null && result != TranslateLineQuantity){
			//          TranslateLineQuantity = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TranslateLineQuantityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTranslateLineQuantity();
				//if (result != null && TranslateLineQuantity != null){
				//	return !TranslateLineQuantity.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _translate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dịch ngược")]
        [ToolTip("Dịch ngược")]
		//[Index(5)]		

 		[Size(SizeAttribute.Unlimited)]
	    [ModelDefault("PropertyEditorType", "SafeCSCodePropertyEditor")]
		public string Translate
        { 
		    get => GetPropertyValue<string>("Translate");                         
			set => SetPropertyValue<string>("Translate", value); 
			
        }
		//Tooltip for Object
		public object TranslateToolTipControllerText(View view)
        {
        //    if (Translate != null) 
		//			return Translate;
            return null;
        }
		//Get Default Value
        public string GetDefaultTranslate(View view = null)
        { 
			return Translate;
        }
		//Set Default Value
		public void SetDefaultTranslate(View view = null)
        {
            //if (Translate is null){
            //    var result = GetDefaultTranslate(view);
            //    if (result != null && result != Translate){
			//          Translate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TranslateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTranslate();
				//if (result != null && Translate != null){
				//	return !Translate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _translate2;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dịch Google")]
        [ToolTip("Dịch Google")]
		//[Index(6)]		

 		[Size(SizeAttribute.Unlimited)]
	    [ModelDefault("PropertyEditorType", "SafeCSCodePropertyEditor")]
		public string Translate2
        { 
		    get => GetPropertyValue<string>("Translate2");                         
			set => SetPropertyValue<string>("Translate2", value); 
			
        }
		//Tooltip for Object
		public object Translate2ToolTipControllerText(View view)
        {
        //    if (Translate2 != null) 
		//			return Translate2;
            return null;
        }
		//Get Default Value
        public string GetDefaultTranslate2(View view = null)
        { 
			return Translate2;
        }
		//Set Default Value
		public void SetDefaultTranslate2(View view = null)
        {
            //if (Translate2 is null){
            //    var result = GetDefaultTranslate2(view);
            //    if (result != null && result != Translate2){
			//          Translate2 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Translate2IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTranslate2();
				//if (result != null && Translate2 != null){
				//	return !Translate2.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.ElementBatch _elementbatch;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lô thành phần")]
        [ToolTip("Lô thành phần")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ElementBatchCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("ElementBatch-BatchTranslateList")]
	 
		public Module.BusinessObjects.ElementBatch ElementBatch
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ElementBatch>("ElementBatch");                         
			set => SetPropertyValue<Module.BusinessObjects.ElementBatch>("ElementBatch", value); 
			
        }
		//Tooltip for Object
		public object ElementBatchToolTipControllerText(View view)
        {
        //    if (ElementBatch != null) 
		//			return ElementBatch;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ElementBatch GetDefaultElementBatch(View view = null)
        { 
			return ElementBatch;
        }
		//Set Default Value
		public void SetDefaultElementBatch(View view = null)
        {
            //if (ElementBatch is null){
            //    var result = GetDefaultElementBatch(view);
            //    if (result != null && result != ElementBatch){
			//          ElementBatch = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ElementBatchIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultElementBatch();
				//if (result != null && ElementBatch != null){
				//	return !ElementBatch.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ElementBatchCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ElementBatch));
            }
        }
	
       
		//private int? _translate2linequantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dòng Google")]
        [ToolTip("Dòng Google")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Translate2LineQuantity
        { 
		    #region 3353ImportCode 
        get
        {
            if (string.IsNullOrEmpty(Translate2))
                return null;

            return Translate2.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
        }
#endregion 3353ImportCode
			
        }
		//Tooltip for Object
		public object Translate2LineQuantityToolTipControllerText(View view)
        {
        //    if (Translate2LineQuantity != null) 
		//			return Translate2LineQuantity;
            return null;
        }
		//Get Default Value
        public int? GetDefaultTranslate2LineQuantity(View view = null)
        { 
			return Translate2LineQuantity;
        }
		//Set Default Value
		public void SetDefaultTranslate2LineQuantity(View view = null)
        {
            //if (Translate2LineQuantity is null){
            //    var result = GetDefaultTranslate2LineQuantity(view);
            //    if (result != null && result != Translate2LineQuantity){
			//          Translate2LineQuantity = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Translate2LineQuantityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTranslate2LineQuantity();
				//if (result != null && Translate2LineQuantity != null){
				//	return !Translate2LineQuantity.Equals(result);
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
 
            base.AfterConstruction();
            Display = true;
 
        //SetDefaultLanguage(View view = null);
        //SetDefaultLineQuantity(View view = null);
        //SetDefaultContent(View view = null);
        //SetDefaultOriginLanguage(View view = null);
        //SetDefaultTranslateLineQuantity(View view = null);
        //SetDefaultTranslate(View view = null);
        //SetDefaultTranslate2(View view = null);
        //SetDefaultElementBatch(View view = null);
        //SetDefaultTranslate2LineQuantity(View view = null);
			
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
