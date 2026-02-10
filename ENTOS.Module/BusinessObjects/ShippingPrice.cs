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
	[NavigationItem("Business")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Giá vận chuyển"), ImageName("ShippingPrice")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "ShippingPrice_ListView", TargetItems = nameof(Currency)+ "," + nameof(Price)+ "," + nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class ShippingPrice:  DevExpress.Xpo.XPLiteObject  , IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ShippingPrice(Session session)
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
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(0)]		

 		[Size(100)]
		[RuleRequiredField("RequiredShippingPriceName", DefaultContexts.Save)]
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

	
       
		//private decimal? _minvalue;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tối thiểu")]
        [ToolTip("Tối thiểu")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? MinValue
        { 
		    get => GetPropertyValue<decimal?>("MinValue");                         
			set => SetPropertyValue<decimal?>("MinValue", value); 
			
        }
		//Tooltip for Object
		public object MinValueToolTipControllerText(View view)
        {
        //    if (MinValue != null) 
		//			return MinValue;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultMinValue(View view = null)
        { 
			return MinValue;
        }
		//Set Default Value
		public void SetDefaultMinValue(View view = null)
        {
            //if (MinValue is null){
            //    var result = GetDefaultMinValue(view);
            //    if (result != null && result != MinValue){
			//          MinValue = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MinValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMinValue();
				//if (result != null && MinValue != null){
				//	return !MinValue.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _maxvalue;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tối đa")]
        [ToolTip("Tối đa")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? MaxValue
        { 
		    get => GetPropertyValue<decimal?>("MaxValue");                         
			set => SetPropertyValue<decimal?>("MaxValue", value); 
			
        }
		//Tooltip for Object
		public object MaxValueToolTipControllerText(View view)
        {
        //    if (MaxValue != null) 
		//			return MaxValue;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultMaxValue(View view = null)
        { 
			return MaxValue;
        }
		//Set Default Value
		public void SetDefaultMaxValue(View view = null)
        {
            //if (MaxValue is null){
            //    var result = GetDefaultMaxValue(view);
            //    if (result != null && result != MaxValue){
			//          MaxValue = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MaxValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMaxValue();
				//if (result != null && MaxValue != null){
				//	return !MaxValue.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _price;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đơn giá")]
        [ToolTip("Đơn giá")]
		//[Index(3)]		
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

	
       
		//private Module.BusinessObjects.Currency _currency;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tiền tệ")]
        [ToolTip("Tiền tệ")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CurrencyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
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
	
       
		//private bool _perpackage;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trọn gói")]
        [ToolTip("Trọn gói")]
		//[Index(5)]		
		public bool PerPackage
        { 
		    get => GetPropertyValue<bool>("PerPackage");                         
			set => SetPropertyValue<bool>("PerPackage", value); 
			
        }
		//Tooltip for Object
		public object PerPackageToolTipControllerText(View view)
        {
        //    if (PerPackage != null) 
		//			return PerPackage;
            return null;
        }
		//Get Default Value
        public bool GetDefaultPerPackage(View view = null)
        { 
			return PerPackage;
        }
		//Set Default Value
		public void SetDefaultPerPackage(View view = null)
        {
            //if (PerPackage is null){
            //    var result = GetDefaultPerPackage(view);
            //    if (result != null && result != PerPackage){
			//          PerPackage = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PerPackageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPerPackage();
				//if (result != null && PerPackage != null){
				//	return !PerPackage.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Shipping _shipping;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vận chuyển")]
        [ToolTip("Vận chuyển")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ShippingCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Shipping-ShippingPrices")]
	 
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
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultMinValue(View view = null);
        //SetDefaultMaxValue(View view = null);
        //SetDefaultPrice(View view = null);
        //SetDefaultCurrency(View view = null);
        //SetDefaultPerPackage(View view = null);
        //SetDefaultShipping(View view = null);
			
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
