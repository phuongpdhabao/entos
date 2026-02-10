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
    [ModelDefault("Caption", "Chi tiết đơn hàng 2"), ImageName("OrderDetail2")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "OrderDetail2_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "OrderDetail2_LookupListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class OrderDetail2:  DevExpress.Xpo.XPLiteObject  , IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public OrderDetail2(Session session)
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
               

		//private Module.BusinessObjects.Product _product;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Sản phẩm")]
        [ToolTip("Sản phẩm")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
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
	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(1)]		

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

	
       
		//private decimal? _quantity;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Quantity
        { 
		    get => GetPropertyValue<decimal?>("Quantity");                         
			set => SetPropertyValue<decimal?>("Quantity", value); 
			
        }
		//Tooltip for Object
		public object QuantityToolTipControllerText(View view)
        {
        //    if (Quantity != null) 
		//			return Quantity;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultQuantity(View view = null)
        { 
			return Quantity;
        }
		//Set Default Value
		public void SetDefaultQuantity(View view = null)
        {
            //if (Quantity is null){
            //    var result = GetDefaultQuantity(view);
            //    if (result != null && result != Quantity){
			//          Quantity = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool QuantityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultQuantity();
				//if (result != null && Quantity != null){
				//	return !Quantity.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _price;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Đơn giá")]
        [ToolTip("Đơn giá")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
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

	
       
		//private decimal? _amount;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thành tiền")]
        [ToolTip("Thành tiền")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Amount
        { 
		    get => GetPropertyValue<decimal?>("Amount");                         
			set => SetPropertyValue<decimal?>("Amount", value); 
			
        }
		//Tooltip for Object
		public object AmountToolTipControllerText(View view)
        {
        //    if (Amount != null) 
		//			return Amount;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultAmount(View view = null)
        { 
			return Amount;
        }
		//Set Default Value
		public void SetDefaultAmount(View view = null)
        {
            //if (Amount is null){
            //    var result = GetDefaultAmount(view);
            //    if (result != null && result != Amount){
			//          Amount = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AmountIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAmount();
				//if (result != null && Amount != null){
				//	return !Amount.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _vat;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thuế GTGT")]
        [ToolTip("Thuế GTGT")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p0")]
		public decimal? VAT
        { 
		    get => GetPropertyValue<decimal?>("VAT");                         
			set => SetPropertyValue<decimal?>("VAT", value); 
			
        }
		//Tooltip for Object
		public object VATToolTipControllerText(View view)
        {
        //    if (VAT != null) 
		//			return VAT;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultVAT(View view = null)
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

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(6)]		
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
        public int? GetDefaultOrder(View view = null)
        { 
			return Order;
        }
		//Set Default Value
		public void SetDefaultOrder(View view = null)
        {
            //if (Order is null){
            //    var result = GetDefaultOrder(view);
            //    if (result != null && result != Order){
			//          Order = result;
            //	  }
            //}
        }

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

	
       
		//private Module.BusinessObjects.Asset _asset;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài sản")]
        [ToolTip("Tài sản")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(AssetCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Asset Asset
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Asset>("Asset");                         
			set => SetPropertyValue<Module.BusinessObjects.Asset>("Asset", value); 
			
        }
		//Tooltip for Object
		public object AssetToolTipControllerText(View view)
        {
        //    if (Asset != null) 
		//			return Asset;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Asset GetDefaultAsset(View view = null)
        { 
			return Asset;
        }
		//Set Default Value
		public void SetDefaultAsset(View view = null)
        {
            //if (Asset is null){
            //    var result = GetDefaultAsset(view);
            //    if (result != null && result != Asset){
			//          Asset = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AssetIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAsset();
				//if (result != null && Asset != null){
				//	return !Asset.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator AssetCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Asset));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultProduct(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultPrice(View view = null);
        //SetDefaultAmount(View view = null);
        //SetDefaultVAT(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultAsset(View view = null);
			
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
