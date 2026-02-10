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
	[NavigationItem("Location")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Quốc gia"), ImageName("Country")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "TaxType_CountryList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Country_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ProductListing_CountryList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Country_ListView", TargetItems = nameof(Name)+ "," + nameof(Code)+ "," + nameof(Flag))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Country: Module.BusinessObjects.Space   , IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Country(Session session)
            : base(session) {              
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

               

		//private decimal? _gdp;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("GDP")]
        [ToolTip("GDP")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? GDP
        { 
		    get => GetPropertyValue<decimal?>("GDP");                         
			set => SetPropertyValue<decimal?>("GDP", value); 
			
        }
		//Tooltip for Object
		public object GDPToolTipControllerText(View view)
        {
        //    if (GDP != null) 
		//			return GDP;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultGDP(View view = null)
        { 
			return GDP;
        }
		//Set Default Value
		public void SetDefaultGDP(View view = null)
        {
            //if (GDP is null){
            //    var result = GetDefaultGDP(view);
            //    if (result != null && result != GDP){
			//          GDP = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool GDPIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultGDP();
				//if (result != null && GDP != null){
				//	return !GDP.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _capitagdp;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("GDP bình quân")]
        [ToolTip("GDP bình quân")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? CapitaGDP
        { 
		    get => GetPropertyValue<decimal?>("CapitaGDP");                         
			set => SetPropertyValue<decimal?>("CapitaGDP", value); 
			
        }
		//Tooltip for Object
		public object CapitaGDPToolTipControllerText(View view)
        {
        //    if (CapitaGDP != null) 
		//			return CapitaGDP;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultCapitaGDP(View view = null)
        { 
			return CapitaGDP;
        }
		//Set Default Value
		public void SetDefaultCapitaGDP(View view = null)
        {
            //if (CapitaGDP is null){
            //    var result = GetDefaultCapitaGDP(view);
            //    if (result != null && result != CapitaGDP){
			//          CapitaGDP = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CapitaGDPIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCapitaGDP();
				//if (result != null && CapitaGDP != null){
				//	return !CapitaGDP.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _origincode;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã xuất xứ")]
        [ToolTip("Mã xuất xứ")]
		//[Index(11)]		

 		[Size(20)]
		public string OriginCode
        { 
		    get => GetPropertyValue<string>("OriginCode");                         
			set => SetPropertyValue<string>("OriginCode", value); 
			
        }
		//Tooltip for Object
		public object OriginCodeToolTipControllerText(View view)
        {
        //    if (OriginCode != null) 
		//			return OriginCode;
            return null;
        }
		//Get Default Value
        public string GetDefaultOriginCode(View view = null)
        { 
			return OriginCode;
        }
		//Set Default Value
		public void SetDefaultOriginCode(View view = null)
        {
            //if (OriginCode is null){
            //    var result = GetDefaultOriginCode(view);
            //    if (result != null && result != OriginCode){
			//          OriginCode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OriginCodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOriginCode();
				//if (result != null && OriginCode != null){
				//	return !OriginCode.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _callingcode;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã vùng")]
        [ToolTip("Mã vùng")]
		//[Index(12)]		

 		[Size(20)]
		public string CallingCode
        { 
		    get => GetPropertyValue<string>("CallingCode");                         
			set => SetPropertyValue<string>("CallingCode", value); 
			
        }
		//Tooltip for Object
		public object CallingCodeToolTipControllerText(View view)
        {
        //    if (CallingCode != null) 
		//			return CallingCode;
            return null;
        }
		//Get Default Value
        public string GetDefaultCallingCode(View view = null)
        { 
			return CallingCode;
        }
		//Set Default Value
		public void SetDefaultCallingCode(View view = null)
        {
            //if (CallingCode is null){
            //    var result = GetDefaultCallingCode(view);
            //    if (result != null && result != CallingCode){
			//          CallingCode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CallingCodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCallingCode();
				//if (result != null && CallingCode != null){
				//	return !CallingCode.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Shipping _shipping;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vận chuyển")]
        [ToolTip("Vận chuyển")]
		//[Index(13)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ShippingCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Shipping Shipping
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Shipping>("Shipping");                         
			set => SetPropertyValue<Module.BusinessObjects.Shipping>("Shipping", value); 
			
        }
		//Tooltip for Object
		public object ShippingToolTipControllerText(View view)
        {
        //    if (Shipping != null) 
		//			return Shipping;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Shipping GetDefaultShipping(View view = null)
        { 
			return Shipping;
        }
		//Set Default Value
		public void SetDefaultShipping(View view = null)
        {
            //if (Shipping is null){
            //    var result = GetDefaultShipping(view);
            //    if (result != null && result != Shipping){
			//          Shipping = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ShippingIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultShipping();
				//if (result != null && Shipping != null){
				//	return !Shipping.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ShippingCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Shipping));
            }
        }
	
       
		//private decimal? _shippingprice;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá vận chuyển")]
        [ToolTip("Giá vận chuyển")]
		//[Index(14)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? ShippingPrice
        { 
		    get => GetPropertyValue<decimal?>("ShippingPrice");                         
			set => SetPropertyValue<decimal?>("ShippingPrice", value); 
			
        }
		//Tooltip for Object
		public object ShippingPriceToolTipControllerText(View view)
        {
        //    if (ShippingPrice != null) 
		//			return ShippingPrice;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultShippingPrice(View view = null)
        { 
			return ShippingPrice;
        }
		//Set Default Value
		public void SetDefaultShippingPrice(View view = null)
        {
            //if (ShippingPrice is null){
            //    var result = GetDefaultShippingPrice(view);
            //    if (result != null && result != ShippingPrice){
			//          ShippingPrice = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ShippingPriceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultShippingPrice();
				//if (result != null && ShippingPrice != null){
				//	return !ShippingPrice.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _otherexpense;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chi phí")]
        [ToolTip("Chi phí")]
		//[Index(15)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p2")]
		[RuleUniqueValue("UniqueCountryOtherExpense", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		public decimal? OtherExpense
        { 
		    get => GetPropertyValue<decimal?>("OtherExpense");                         
			set => SetPropertyValue<decimal?>("OtherExpense", value); 
			
        }
		//Tooltip for Object
		public object OtherExpenseToolTipControllerText(View view)
        {
        //    if (OtherExpense != null) 
		//			return OtherExpense;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultOtherExpense(View view = null)
        { 
			return OtherExpense;
        }
		//Set Default Value
		public void SetDefaultOtherExpense(View view = null)
        {
            //if (OtherExpense is null){
            //    var result = GetDefaultOtherExpense(view);
            //    if (result != null && result != OtherExpense){
			//          OtherExpense = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OtherExpenseIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOtherExpense();
				//if (result != null && OtherExpense != null){
				//	return !OtherExpense.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _isdefault;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mặc định")]
        [ToolTip("Mặc định")]
		//[Index(16)]		
		public bool IsDefault
        { 
		    get => GetPropertyValue<bool>("IsDefault");                         
			set => SetPropertyValue<bool>("IsDefault", value); 
			
        }
		//Tooltip for Object
		public object IsDefaultToolTipControllerText(View view)
        {
        //    if (IsDefault != null) 
		//			return IsDefault;
            return null;
        }
		//Get Default Value
        public bool GetDefaultIsDefault(View view = null)
        { 
			return IsDefault;
        }
		//Set Default Value
		public void SetDefaultIsDefault(View view = null)
        {
            //if (IsDefault is null){
            //    var result = GetDefaultIsDefault(view);
            //    if (result != null && result != IsDefault){
			//          IsDefault = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool IsDefaultIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultIsDefault();
				//if (result != null && IsDefault != null){
				//	return !IsDefault.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Currency _currency;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đồng tiền")]
        [ToolTip("Đồng tiền")]
		//[Index(28)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CurrencyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		[RuleUniqueValue("UniqueCountryCurrency", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		public Module.BusinessObjects.Currency Currency
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Currency>("Currency");                         
			set => SetPropertyValue<Module.BusinessObjects.Currency>("Currency", value); 
			
        }
		//Tooltip for Object
		public object CurrencyToolTipControllerText(View view)
        {
        //    if (Currency != null) 
		//			return Currency;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Currency GetDefaultCurrency(View view = null)
        { 
			return Currency;
        }
		//Set Default Value
		public void SetDefaultCurrency(View view = null)
        {
            //if (Currency is null){
            //    var result = GetDefaultCurrency(view);
            //    if (result != null && result != Currency){
			//          Currency = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CurrencyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCurrency();
				//if (result != null && Currency != null){
				//	return !Currency.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CurrencyCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Currency));
            }
        }
	
       
		//private Module.BusinessObjects.Language _language;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngôn ngữ chính")]
        [ToolTip("Ngôn ngữ chính")]
		//[Index(29)]		
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
	
       
		//private string _note;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(30)]		

 		[Size(100)]
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

	
       
		//private byte[] _flag;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quốc kỳ")]
        [ToolTip("Quốc kỳ")]
		//[Index(31)]		
		[Appearance("Quốc kỳBackground", BackColor = "Transparent")]
	
		[ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 3)] 
	
		public byte[] Flag
        { 
		    get => GetPropertyValue<byte[]>("Flag");                         
			set => SetPropertyValue<byte[]>("Flag", value); 
			
        }
		//Tooltip for Object
		public object FlagToolTipControllerText(View view)
        {
        //    if (Flag != null) 
		//			return Flag;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultFlag(View view = null)
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

	
       
		//private bool _firstname;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên đầu")]
        [ToolTip("Tên đầu")]
		//[Index(32)]		
		public bool FirstName
        { 
		    get => GetPropertyValue<bool>("FirstName");                         
			set => SetPropertyValue<bool>("FirstName", value); 
			
        }
		//Tooltip for Object
		public object FirstNameToolTipControllerText(View view)
        {
        //    if (FirstName != null) 
		//			return FirstName;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFirstName(View view = null)
        { 
			return FirstName;
        }
		//Set Default Value
		public void SetDefaultFirstName(View view = null)
        {
            //if (FirstName is null){
            //    var result = GetDefaultFirstName(view);
            //    if (result != null && result != FirstName){
			//          FirstName = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FirstNameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFirstName();
				//if (result != null && FirstName != null){
				//	return !FirstName.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại thuế")]
		//[Index(34)]
		[DataSourceCriteria("Not CountryList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("CountryList-TaxTypeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.TaxType> TaxTypeList
        {      
		    get => GetCollection<Module.BusinessObjects.TaxType>("TaxTypeList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Niêm yết sản phẩm")]
		//[Index(39)]
		[DataSourceCriteria("Not CountryList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("CountryList-ProductListingList")]
	    [Browsable(false)]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductListing> ProductListingList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductListing>("ProductListingList"); 
			
        }
       
 


		public override void AfterConstruction()
        {
 
            base.AfterConstruction();
 
        //SetDefaultGDP(View view = null);
        //SetDefaultCapitaGDP(View view = null);
        //SetDefaultOriginCode(View view = null);
        //SetDefaultCallingCode(View view = null);
        //SetDefaultShipping(View view = null);
        //SetDefaultShippingPrice(View view = null);
        //SetDefaultOtherExpense(View view = null);
        //SetDefaultIsDefault(View view = null);
        //SetDefaultCurrency(View view = null);
        //SetDefaultLanguage(View view = null);
        //SetDefaultNote(View view = null);
        //SetDefaultFlag(View view = null);
        //SetDefaultFirstName(View view = null);
			
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
			//	SetDefaultLowerSpaces();
			//	SetDefaultTaxTypeList();
			//	SetDefaultUpperLeftList();
			//	SetDefaultLowerRightList();
			//	SetDefaultEthnicityList();
			//	SetDefaultHistoryList();
			//	SetDefaultProductListingList();
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
