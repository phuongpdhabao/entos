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
    [ModelDefault("Caption", "Giá sản phẩm"), ImageName("ProductPrice")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "ProductPrice_ListView", TargetItems = nameof(Product)+ "," + nameof(Date))]
	[MobileColumnAttribute(Context = "Product_ProductPriceList_ListView", TargetItems = nameof(Date))]
	[MobileColumnAttribute(Context = "ProductPrice_LookupListView", TargetItems = nameof(Product)+ "," + nameof(Product))]
	[DefaultProperty("Product")]
 
[OptimisticLocking(true)]
    public partial class ProductPrice:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ProductPrice(Session session)
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
               

		//private decimal _guarantee;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bảo hành")]
        [ToolTip("Bảo hành")]
		//[Index(0)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public decimal Guarantee
        { 
		    get => GetPropertyValue<decimal>("Guarantee");                         
			set => SetPropertyValue<decimal>("Guarantee", value); 
			
        }
		//Tooltip for Object
		public object GuaranteeToolTipControllerText(View view)
        {
        //    if (Guarantee != null) 
		//			return Guarantee;
            return null;
        }
		//Get Default Value
        public decimal GetDefaultGuarantee(View view = null)
        { 
			return Guarantee;
        }
		//Set Default Value
		public void SetDefaultGuarantee(View view = null)
        {
            //if (Guarantee is null){
            //    var result = GetDefaultGuarantee(view);
            //    if (result != null && result != Guarantee){
			//          Guarantee = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool GuaranteeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultGuarantee();
				//if (result != null && Guarantee != null){
				//	return !Guarantee.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _note;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(1)]		

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

	
       
		//private Module.BusinessObjects.Currency _money;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tiền")]
        [ToolTip("Tiền")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MoneyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Currency Money
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Currency>("Money");                         
			set => SetPropertyValue<Module.BusinessObjects.Currency>("Money", value); 
			
        }
		//Tooltip for Object
		public object MoneyToolTipControllerText(View view)
        {
        //    if (Money != null) 
		//			return Money;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Currency GetDefaultMoney(View view = null)
        { 
			return Money;
        }
		//Set Default Value
		public void SetDefaultMoney(View view = null)
        {
            //if (Money is null){
            //    var result = GetDefaultMoney(view);
            //    if (result != null && result != Money){
			//          Money = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MoneyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMoney();
				//if (result != null && Money != null){
				//	return !Money.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator MoneyCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Money));
            }
        }
	
       
		//private bool _vat;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("VAT")]
        [ToolTip("VAT")]
		//[Index(3)]		
		public bool VAT
        { 
		    get => GetPropertyValue<bool>("VAT");                         
			set => SetPropertyValue<bool>("VAT", value); 
			
        }
		//Tooltip for Object
		public object VATToolTipControllerText(View view)
        {
        //    if (VAT != null) 
		//			return VAT;
            return null;
        }
		//Get Default Value
        public bool GetDefaultVAT(View view = null)
        { 
			return VAT;
        }
		//Set Default Value
		public void SetDefaultVAT(View view = null)
        {
            //if (VAT is null){
            //    var result = GetDefaultVAT(view);
            //    if (result != null && result != VAT){
			//          VAT = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool VATIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultVAT();
				//if (result != null && VAT != null){
				//	return !VAT.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _discount;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chiết khấu")]
        [ToolTip("Chiết khấu")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:p2}")]
		[ModelDefault("EditMask", "p2")]
		public decimal? Discount
        { 
		    get => GetPropertyValue<decimal?>("Discount");                         
			set => SetPropertyValue<decimal?>("Discount", value); 
			
        }
		//Tooltip for Object
		public object DiscountToolTipControllerText(View view)
        {
        //    if (Discount != null) 
		//			return Discount;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultDiscount(View view = null)
        { 
			return Discount;
        }
		//Set Default Value
		public void SetDefaultDiscount(View view = null)
        {
            //if (Discount is null){
            //    var result = GetDefaultDiscount(view);
            //    if (result != null && result != Discount){
			//          Discount = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DiscountIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDiscount();
				//if (result != null && Discount != null){
				//	return !Discount.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _importtax;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuế nhập khẩu")]
        [ToolTip("Thuế nhập khẩu")]
		//[Index(5)]		

 		[Size(100)]
		public string ImportTax
        { 
		    get => GetPropertyValue<string>("ImportTax");                         
			set => SetPropertyValue<string>("ImportTax", value); 
			
        }
		//Tooltip for Object
		public object ImportTaxToolTipControllerText(View view)
        {
        //    if (ImportTax != null) 
		//			return ImportTax;
            return null;
        }
		//Get Default Value
        public string GetDefaultImportTax(View view = null)
        { 
			return ImportTax;
        }
		//Set Default Value
		public void SetDefaultImportTax(View view = null)
        {
            //if (ImportTax is null){
            //    var result = GetDefaultImportTax(view);
            //    if (result != null && result != ImportTax){
			//          ImportTax = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ImportTaxIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImportTax();
				//if (result != null && ImportTax != null){
				//	return !ImportTax.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal _fobprice;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá FOB")]
        [ToolTip("Giá FOB")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal FOBPrice
        { 
		    get => GetPropertyValue<decimal>("FOBPrice");                         
			set => SetPropertyValue<decimal>("FOBPrice", value); 
			
        }
		//Tooltip for Object
		public object FOBPriceToolTipControllerText(View view)
        {
        //    if (FOBPrice != null) 
		//			return FOBPrice;
            return null;
        }
		//Get Default Value
        public decimal GetDefaultFOBPrice(View view = null)
        { 
			return FOBPrice;
        }
		//Set Default Value
		public void SetDefaultFOBPrice(View view = null)
        {
            //if (FOBPrice is null){
            //    var result = GetDefaultFOBPrice(view);
            //    if (result != null && result != FOBPrice){
			//          FOBPrice = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FOBPriceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFOBPrice();
				//if (result != null && FOBPrice != null){
				//	return !FOBPrice.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _date;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(7)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? Date
        { 
		    get => GetPropertyValue<DateTime?>("Date");                         
			set => SetPropertyValue<DateTime?>("Date", value); 
			
        }
		//Tooltip for Object
		public object DateToolTipControllerText(View view)
        {
        //    if (Date != null) 
		//			return Date;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultDate(View view = null)
        { 
			return Date;
        }
		//Set Default Value
		public void SetDefaultDate(View view = null)
        {
            //if (Date is null){
            //    var result = GetDefaultDate(view);
            //    if (result != null && result != Date){
			//          Date = result;
            //	  }
            //}
        }

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

	
       
		//private decimal _pricecost;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá vốn")]
        [ToolTip("Giá vốn")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal PriceCost
        { 
		    get => GetPropertyValue<decimal>("PriceCost");                         
			set => SetPropertyValue<decimal>("PriceCost", value); 
			
        }
		//Tooltip for Object
		public object PriceCostToolTipControllerText(View view)
        {
        //    if (PriceCost != null) 
		//			return PriceCost;
            return null;
        }
		//Get Default Value
        public decimal GetDefaultPriceCost(View view = null)
        { 
			return PriceCost;
        }
		//Set Default Value
		public void SetDefaultPriceCost(View view = null)
        {
            //if (PriceCost is null){
            //    var result = GetDefaultPriceCost(view);
            //    if (result != null && result != PriceCost){
			//          PriceCost = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PriceCostIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPriceCost();
				//if (result != null && PriceCost != null){
				//	return !PriceCost.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Product _product;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Sản phẩm")]
        [ToolTip("Sản phẩm")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Product-ProductPriceList")]
	 
		public Module.BusinessObjects.Product Product
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Product>("Product");                         
			set => SetPropertyValue<Module.BusinessObjects.Product>("Product", value); 
			
        }
		//Tooltip for Object
		public object ProductToolTipControllerText(View view)
        {
        //    if (Product != null) 
		//			return Product;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Product GetDefaultProduct(View view = null)
        { 
			return Product;
        }
		//Set Default Value
		public void SetDefaultProduct(View view = null)
        {
            //if (Product is null){
            //    var result = GetDefaultProduct(view);
            //    if (result != null && result != Product){
			//          Product = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ProductIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultProduct();
				//if (result != null && Product != null){
				//	return !Product.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ProductCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Product));
            }
        }
	
       
		//private Module.BusinessObjects.Org _supplier;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhà cung cấp")]
        [ToolTip("Nhà cung cấp")]
		//[Index(10)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SupplierCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Supplier-ProductPriceList")]
	 
		public Module.BusinessObjects.Org Supplier
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Org>("Supplier");                         
			set => SetPropertyValue<Module.BusinessObjects.Org>("Supplier", value); 
			
        }
		//Tooltip for Object
		public object SupplierToolTipControllerText(View view)
        {
        //    if (Supplier != null) 
		//			return Supplier;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Org GetDefaultSupplier(View view = null)
        { 
			return Supplier;
        }
		//Set Default Value
		public void SetDefaultSupplier(View view = null)
        {
            //if (Supplier is null){
            //    var result = GetDefaultSupplier(view);
            //    if (result != null && result != Supplier){
			//          Supplier = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SupplierIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSupplier();
				//if (result != null && Supplier != null){
				//	return !Supplier.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SupplierCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Supplier));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultGuarantee(View view = null);
        //SetDefaultNote(View view = null);
        //SetDefaultMoney(View view = null);
        //SetDefaultVAT(View view = null);
        //SetDefaultDiscount(View view = null);
        //SetDefaultImportTax(View view = null);
        //SetDefaultFOBPrice(View view = null);
        //SetDefaultDate(View view = null);
        //SetDefaultPriceCost(View view = null);
        //SetDefaultProduct(View view = null);
        //SetDefaultSupplier(View view = null);
			
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
