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
	[NavigationItem("ApplicationBusiness")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Giá ứng dụng"), ImageName("AppPrice")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(PurchaseCurrency), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update))]
 
	[MobileColumnAttribute(Context = "App_AppPriceList_ListView", TargetItems = nameof(Update)+ "," + nameof(Price))]
	[MobileColumnAttribute(Context = "AppPrice_ListView", TargetItems = nameof(CurrencyType)+ "," + nameof(Price)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "AppPrice_LookupListView", TargetItems = nameof(Update)+ "," + nameof(Price))]
	[DefaultProperty("Price")]
 
[OptimisticLocking(true)]
    public partial class AppPrice:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public AppPrice(Session session)
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
               

		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(0)]		

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

	
       
		//private decimal? _price;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giá")]
        [ToolTip("Giá")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Price
        { 
		    get => GetPropertyValue<decimal?>("Price");                         
			set => SetPropertyValue<decimal?>("Price", value); 
			
        }
		//Tooltip for Object
		public object PriceToolTipControllerText(View view)
        {
        //    if (Price != null) 
		//			return Price;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultPrice(View view = null)
        { 
			return Price;
        }
		//Set Default Value
		public void SetDefaultPrice(View view = null)
        {
            //if (Price is null){
            //    var result = GetDefaultPrice(view);
            //    if (result != null && result != Price){
			//          Price = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PriceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPrice();
				//if (result != null && Price != null){
				//	return !Price.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private CurrencyType _currencytype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tiền")]
        [ToolTip("Tiền")]
		//[Index(2)]		
		public CurrencyType CurrencyType
        { 
		    get => GetPropertyValue<CurrencyType>("CurrencyType");                         
			set => SetPropertyValue<CurrencyType>("CurrencyType", value); 
			
        }
		//Tooltip for Object
		public object CurrencyTypeToolTipControllerText(View view)
        {
        //    if (CurrencyType != null) 
		//			return CurrencyType;
            return null;
        }
		//Get Default Value
        public CurrencyType GetDefaultCurrencyType(View view = null)
        { 
			return CurrencyType;
        }
		//Set Default Value
		public void SetDefaultCurrencyType(View view = null)
        {
            //if (CurrencyType is null){
            //    var result = GetDefaultCurrencyType(view);
            //    if (result != null && result != CurrencyType){
			//          CurrencyType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CurrencyTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCurrencyType();
				//if (result != null && CurrencyType != null){
				//	return !CurrencyType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private SubscriptionCycle _subscriptioncycle;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Chu kỳ")]
        [ToolTip("Chu kỳ")]
		//[Index(3)]		
		public SubscriptionCycle SubscriptionCycle
        { 
		    get => GetPropertyValue<SubscriptionCycle>("SubscriptionCycle");                         
			set => SetPropertyValue<SubscriptionCycle>("SubscriptionCycle", value); 
			
        }
		//Tooltip for Object
		public object SubscriptionCycleToolTipControllerText(View view)
        {
        //    if (SubscriptionCycle != null) 
		//			return SubscriptionCycle;
            return null;
        }
		//Get Default Value
        public SubscriptionCycle GetDefaultSubscriptionCycle(View view = null)
        { 
			return SubscriptionCycle;
        }
		//Set Default Value
		public void SetDefaultSubscriptionCycle(View view = null)
        {
            //if (SubscriptionCycle is null){
            //    var result = GetDefaultSubscriptionCycle(view);
            //    if (result != null && result != SubscriptionCycle){
			//          SubscriptionCycle = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SubscriptionCycleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSubscriptionCycle();
				//if (result != null && SubscriptionCycle != null){
				//	return !SubscriptionCycle.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private AppLicenceUnit _licenceunit;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đơn vị")]
        [ToolTip("Đơn vị")]
		//[Index(4)]		
		public AppLicenceUnit LicenceUnit
        { 
		    get => GetPropertyValue<AppLicenceUnit>("LicenceUnit");                         
			set => SetPropertyValue<AppLicenceUnit>("LicenceUnit", value); 
			
        }
		//Tooltip for Object
		public object LicenceUnitToolTipControllerText(View view)
        {
        //    if (LicenceUnit != null) 
		//			return LicenceUnit;
            return null;
        }
		//Get Default Value
        public AppLicenceUnit GetDefaultLicenceUnit(View view = null)
        { 
			return LicenceUnit;
        }
		//Set Default Value
		public void SetDefaultLicenceUnit(View view = null)
        {
            //if (LicenceUnit is null){
            //    var result = GetDefaultLicenceUnit(view);
            //    if (result != null && result != LicenceUnit){
			//          LicenceUnit = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LicenceUnitIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLicenceUnit();
				//if (result != null && LicenceUnit != null){
				//	return !LicenceUnit.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private UserCategory _usercategory;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đối tượng")]
        [ToolTip("Đối tượng")]
		//[Index(5)]		
		public UserCategory UserCategory
        { 
		    get => GetPropertyValue<UserCategory>("UserCategory");                         
			set => SetPropertyValue<UserCategory>("UserCategory", value); 
			
        }
		//Tooltip for Object
		public object UserCategoryToolTipControllerText(View view)
        {
        //    if (UserCategory != null) 
		//			return UserCategory;
            return null;
        }
		//Get Default Value
        public UserCategory GetDefaultUserCategory(View view = null)
        { 
			return UserCategory;
        }
		//Set Default Value
		public void SetDefaultUserCategory(View view = null)
        {
            //if (UserCategory is null){
            //    var result = GetDefaultUserCategory(view);
            //    if (result != null && result != UserCategory){
			//          UserCategory = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UserCategoryIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUserCategory();
				//if (result != null && UserCategory != null){
				//	return !UserCategory.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.App _app;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ứng dụng")]
        [ToolTip("Ứng dụng")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(AppCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("App-AppPriceList")]
	 
		public Module.BusinessObjects.App App
        { 
		    get => GetPropertyValue<Module.BusinessObjects.App>("App");                         
			set => SetPropertyValue<Module.BusinessObjects.App>("App", value); 
			
        }
		//Tooltip for Object
		public object AppToolTipControllerText(View view)
        {
        //    if (App != null) 
		//			return App;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.App GetDefaultApp(View view = null)
        { 
			return App;
        }
		//Set Default Value
		public void SetDefaultApp(View view = null)
        {
            //if (App is null){
            //    var result = GetDefaultApp(view);
            //    if (result != null && result != App){
			//          App = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AppIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultApp();
				//if (result != null && App != null){
				//	return !App.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator AppCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(App));
            }
        }
	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(7)]		
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

	
       
		//private CurrencyType _purchasecurrency;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tiền nhập")]
        [ToolTip("Tiền nhập")]
		//[Index(8)]		
		public CurrencyType PurchaseCurrency
        { 
		    get => GetPropertyValue<CurrencyType>("PurchaseCurrency");                         
			set => SetPropertyValue<CurrencyType>("PurchaseCurrency", value); 
			
        }
		//Tooltip for Object
		public object PurchaseCurrencyToolTipControllerText(View view)
        {
        //    if (PurchaseCurrency != null) 
		//			return PurchaseCurrency;
            return null;
        }
		//Get Default Value
        public CurrencyType GetDefaultPurchaseCurrency(View view = null)
        { 
			return PurchaseCurrency;
        }
		//Set Default Value
		public void SetDefaultPurchaseCurrency(View view = null)
        {
            //if (PurchaseCurrency is null){
            //    var result = GetDefaultPurchaseCurrency(view);
            //    if (result != null && result != PurchaseCurrency){
			//          PurchaseCurrency = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PurchaseCurrencyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPurchaseCurrency();
				//if (result != null && PurchaseCurrency != null){
				//	return !PurchaseCurrency.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _purchaseprice;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá nhập")]
        [ToolTip("Giá nhập")]
		//[Index(9)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? PurchasePrice
        { 
		    get => GetPropertyValue<int?>("PurchasePrice");                         
			set => SetPropertyValue<int?>("PurchasePrice", value); 
			
        }
		//Tooltip for Object
		public object PurchasePriceToolTipControllerText(View view)
        {
        //    if (PurchasePrice != null) 
		//			return PurchasePrice;
            return null;
        }
		//Get Default Value
        public int? GetDefaultPurchasePrice(View view = null)
        { 
			return PurchasePrice;
        }
		//Set Default Value
		public void SetDefaultPurchasePrice(View view = null)
        {
            //if (PurchasePrice is null){
            //    var result = GetDefaultPurchasePrice(view);
            //    if (result != null && result != PurchasePrice){
			//          PurchasePrice = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PurchasePriceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPurchasePrice();
				//if (result != null && PurchasePrice != null){
				//	return !PurchasePrice.Equals(result);
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
 
        //SetDefaultName(View view = null);
        //SetDefaultPrice(View view = null);
        //SetDefaultCurrencyType(View view = null);
        //SetDefaultSubscriptionCycle(View view = null);
        //SetDefaultLicenceUnit(View view = null);
        //SetDefaultUserCategory(View view = null);
        //SetDefaultApp(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultPurchaseCurrency(View view = null);
        //SetDefaultPurchasePrice(View view = null);
			
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
            #region 1418ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 1418ImportCode
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
#region 1419ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 1419            Oid: 1426dc49-f7e6-433c-8608-3b8163af000f
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1419ImportCode
#region 1417ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 1417            Oid: 514ce364-00d2-4a98-86ab-4a23594b62fd
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 1417ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
