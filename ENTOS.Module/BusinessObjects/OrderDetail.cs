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
    [ModelDefault("Caption", "Chi tiết đơn hàng"), ImageName("OrderDetail")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(Org)+ "," + nameof(Unit)+ "," + nameof(Origin)+ "," + nameof(Warranty)+ "," + nameof(EstimationName)+ "," + nameof(Supplier)+ "," + nameof(No)+ "," + nameof(Parent)+ "," + nameof(ImportTax)+ "," + nameof(ListPrice)+ "," + nameof(CostPrice)+ "," + nameof(PricingMethod)+ "," + nameof(Margin)+ "," + nameof(Disccount)+ "," + nameof(Note)+ "," + nameof(Date), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Total))]
 
	[MobileColumnAttribute(Context = "Order_OrderDetailList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "OrderDetail_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "OrderDetail_ListView", TargetItems = "Order.Code"+ "," + "Order.OrderType"+ "," + nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class OrderDetail:  DevExpress.Xpo.XPLiteObject  , IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public OrderDetail(Session session)
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
				if (ProductItemList.IsLoaded)
                {
                    if (ProductItemList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ProductItemList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ProductItemList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ProductItem>(CriteriaOperator.Parse("[OrderDetail.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool productitemlist = Session.Query<Module.BusinessObjects.ProductItem>().Where(x => x.OrderDetail.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ProductItemList), productitemlist);
                        if (productitemlist)
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
               

		//private Module.BusinessObjects.Product _product;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
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

 		[Size(100)]
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
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(2)]		

 		[Size(100)]
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

	
       
		//private string _org;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Hãng")]
        [ToolTip("Hãng")]
		//[Index(3)]		

 		[Size(100)]
		public string Org
        { 
		    get => GetPropertyValue<string>("Org");                         
			set => SetPropertyValue<string>("Org", value); 
			
        }
		//Tooltip for Object
		public object OrgToolTipControllerText(View view)
        {
        //    if (Org != null) 
		//			return Org;
            return null;
        }
		//Get Default Value
        public string GetDefaultOrg(View view = null)
        { 
			return Org;
        }
		//Set Default Value
		public void SetDefaultOrg(View view = null)
        {
            //if (Org is null){
            //    var result = GetDefaultOrg(view);
            //    if (result != null && result != Org){
			//          Org = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OrgIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrg();
				//if (result != null && Org != null){
				//	return !Org.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _unit;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đơn vị")]
        [ToolTip("Đơn vị")]
		//[Index(4)]		

 		[Size(50)]
		public string Unit
        { 
		    get => GetPropertyValue<string>("Unit");                         
			set => SetPropertyValue<string>("Unit", value); 
			
        }
		//Tooltip for Object
		public object UnitToolTipControllerText(View view)
        {
        //    if (Unit != null) 
		//			return Unit;
            return null;
        }
		//Get Default Value
        public string GetDefaultUnit(View view = null)
        { 
			return Unit;
        }
		//Set Default Value
		public void SetDefaultUnit(View view = null)
        {
            //if (Unit is null){
            //    var result = GetDefaultUnit(view);
            //    if (result != null && result != Unit){
			//          Unit = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UnitIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUnit();
				//if (result != null && Unit != null){
				//	return !Unit.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _origin;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Xuất xứ")]
        [ToolTip("Xuất xứ")]
		//[Index(5)]		

 		[Size(100)]
		public string Origin
        { 
		    get => GetPropertyValue<string>("Origin");                         
			set => SetPropertyValue<string>("Origin", value); 
			
        }
		//Tooltip for Object
		public object OriginToolTipControllerText(View view)
        {
        //    if (Origin != null) 
		//			return Origin;
            return null;
        }
		//Get Default Value
        public string GetDefaultOrigin(View view = null)
        { 
			return Origin;
        }
		//Set Default Value
		public void SetDefaultOrigin(View view = null)
        {
            //if (Origin is null){
            //    var result = GetDefaultOrigin(view);
            //    if (result != null && result != Origin){
			//          Origin = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OriginIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrigin();
				//if (result != null && Origin != null){
				//	return !Origin.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _warranty;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bảo hành")]
        [ToolTip("Bảo hành")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Warranty
        { 
		    get => GetPropertyValue<int?>("Warranty");                         
			set => SetPropertyValue<int?>("Warranty", value); 
			
        }
		//Tooltip for Object
		public object WarrantyToolTipControllerText(View view)
        {
        //    if (Warranty != null) 
		//			return Warranty;
            return null;
        }
		//Get Default Value
        public int? GetDefaultWarranty(View view = null)
        { 
			return Warranty;
        }
		//Set Default Value
		public void SetDefaultWarranty(View view = null)
        {
            //if (Warranty is null){
            //    var result = GetDefaultWarranty(view);
            //    if (result != null && result != Warranty){
			//          Warranty = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WarrantyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWarranty();
				//if (result != null && Warranty != null){
				//	return !Warranty.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _estimationname;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên DT")]
        [ToolTip("Tên DT")]
		//[Index(7)]		

 		[Size(200)]
		public string EstimationName
        { 
		    get => GetPropertyValue<string>("EstimationName");                         
			set => SetPropertyValue<string>("EstimationName", value); 
			
        }
		//Tooltip for Object
		public object EstimationNameToolTipControllerText(View view)
        {
        //    if (EstimationName != null) 
		//			return EstimationName;
            return null;
        }
		//Get Default Value
        public string GetDefaultEstimationName(View view = null)
        { 
			return EstimationName;
        }
		//Set Default Value
		public void SetDefaultEstimationName(View view = null)
        {
            //if (EstimationName is null){
            //    var result = GetDefaultEstimationName(view);
            //    if (result != null && result != EstimationName){
			//          EstimationName = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EstimationNameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEstimationName();
				//if (result != null && EstimationName != null){
				//	return !EstimationName.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Org _supplier;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cung cấp")]
        [ToolTip("Cung cấp")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SupplierCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
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
	
       
		//private string _no;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("TT")]
        [ToolTip("TT")]
		//[Index(9)]		

 		[Size(10)]
		public string No
        { 
		    get => GetPropertyValue<string>("No");                         
			set => SetPropertyValue<string>("No", value); 
			
        }
		//Tooltip for Object
		public object NoToolTipControllerText(View view)
        {
        //    if (No != null) 
		//			return No;
            return null;
        }
		//Get Default Value
        public string GetDefaultNo(View view = null)
        { 
			return No;
        }
		//Set Default Value
		public void SetDefaultNo(View view = null)
        {
            //if (No is null){
            //    var result = GetDefaultNo(view);
            //    if (result != null && result != No){
			//          No = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NoIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNo();
				//if (result != null && No != null){
				//	return !No.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.OrderDetail _parent;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(10)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ParentCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.OrderDetail Parent
        { 
		    get => GetPropertyValue<Module.BusinessObjects.OrderDetail>("Parent");                         
			set => SetPropertyValue<Module.BusinessObjects.OrderDetail>("Parent", value); 
			
        }
		//Tooltip for Object
		public object ParentToolTipControllerText(View view)
        {
        //    if (Parent != null) 
		//			return Parent;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.OrderDetail GetDefaultParent(View view = null)
        { 
			return Parent;
        }
		//Set Default Value
		public void SetDefaultParent(View view = null)
        {
            //if (Parent is null){
            //    var result = GetDefaultParent(view);
            //    if (result != null && result != Parent){
			//          Parent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ParentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultParent();
				//if (result != null && Parent != null){
				//	return !Parent.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ParentCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Parent));
            }
        }
	
       
		//private decimal? _price;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đơn giá")]
        [ToolTip("Đơn giá")]
		//[Index(11)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
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

	
       
		//private decimal? _quantity;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(12)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
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

	
       
		//private decimal? _vat;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("VAT")]
        [ToolTip("VAT")]
		//[Index(13)]		
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

	
       
		//private decimal? _importtax;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuế NK")]
        [ToolTip("Thuế NK")]
		//[Index(14)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p0")]
		public decimal? ImportTax
        { 
		    get => GetPropertyValue<decimal?>("ImportTax");                         
			set => SetPropertyValue<decimal?>("ImportTax", value); 
			
        }
		//Tooltip for Object
		public object ImportTaxToolTipControllerText(View view)
        {
        //    if (ImportTax != null) 
		//			return ImportTax;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultImportTax(View view = null)
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

	
       
		//private decimal? _listprice;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá list")]
        [ToolTip("Giá list")]
		//[Index(15)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? ListPrice
        { 
		    get => GetPropertyValue<decimal?>("ListPrice");                         
			set => SetPropertyValue<decimal?>("ListPrice", value); 
			
        }
		//Tooltip for Object
		public object ListPriceToolTipControllerText(View view)
        {
        //    if (ListPrice != null) 
		//			return ListPrice;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultListPrice(View view = null)
        { 
			return ListPrice;
        }
		//Set Default Value
		public void SetDefaultListPrice(View view = null)
        {
            //if (ListPrice is null){
            //    var result = GetDefaultListPrice(view);
            //    if (result != null && result != ListPrice){
			//          ListPrice = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ListPriceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultListPrice();
				//if (result != null && ListPrice != null){
				//	return !ListPrice.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _costprice;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá vốn")]
        [ToolTip("Giá vốn")]
		//[Index(16)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? CostPrice
        { 
		    get => GetPropertyValue<decimal?>("CostPrice");                         
			set => SetPropertyValue<decimal?>("CostPrice", value); 
			
        }
		//Tooltip for Object
		public object CostPriceToolTipControllerText(View view)
        {
        //    if (CostPrice != null) 
		//			return CostPrice;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultCostPrice(View view = null)
        { 
			return CostPrice;
        }
		//Set Default Value
		public void SetDefaultCostPrice(View view = null)
        {
            //if (CostPrice is null){
            //    var result = GetDefaultCostPrice(view);
            //    if (result != null && result != CostPrice){
			//          CostPrice = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CostPriceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCostPrice();
				//if (result != null && CostPrice != null){
				//	return !CostPrice.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _pricingmethod;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tính theo")]
        [ToolTip("Tính theo")]
		//[Index(17)]		

 		[Size(100)]
		public string PricingMethod
        { 
		    get => GetPropertyValue<string>("PricingMethod");                         
			set => SetPropertyValue<string>("PricingMethod", value); 
			
        }
		//Tooltip for Object
		public object PricingMethodToolTipControllerText(View view)
        {
        //    if (PricingMethod != null) 
		//			return PricingMethod;
            return null;
        }
		//Get Default Value
        public string GetDefaultPricingMethod(View view = null)
        { 
			return PricingMethod;
        }
		//Set Default Value
		public void SetDefaultPricingMethod(View view = null)
        {
            //if (PricingMethod is null){
            //    var result = GetDefaultPricingMethod(view);
            //    if (result != null && result != PricingMethod){
			//          PricingMethod = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PricingMethodIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPricingMethod();
				//if (result != null && PricingMethod != null){
				//	return !PricingMethod.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _margin;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lợi nhuận")]
        [ToolTip("Lợi nhuận")]
		//[Index(18)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p2")]
		public decimal? Margin
        { 
		    get => GetPropertyValue<decimal?>("Margin");                         
			set => SetPropertyValue<decimal?>("Margin", value); 
			
        }
		//Tooltip for Object
		public object MarginToolTipControllerText(View view)
        {
        //    if (Margin != null) 
		//			return Margin;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultMargin(View view = null)
        { 
			return Margin;
        }
		//Set Default Value
		public void SetDefaultMargin(View view = null)
        {
            //if (Margin is null){
            //    var result = GetDefaultMargin(view);
            //    if (result != null && result != Margin){
			//          Margin = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MarginIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMargin();
				//if (result != null && Margin != null){
				//	return !Margin.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _disccount;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chiết khấu")]
        [ToolTip("Chiết khấu")]
		//[Index(19)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p2")]
		public decimal? Disccount
        { 
		    get => GetPropertyValue<decimal?>("Disccount");                         
			set => SetPropertyValue<decimal?>("Disccount", value); 
			
        }
		//Tooltip for Object
		public object DisccountToolTipControllerText(View view)
        {
        //    if (Disccount != null) 
		//			return Disccount;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultDisccount(View view = null)
        { 
			return Disccount;
        }
		//Set Default Value
		public void SetDefaultDisccount(View view = null)
        {
            //if (Disccount is null){
            //    var result = GetDefaultDisccount(view);
            //    if (result != null && result != Disccount){
			//          Disccount = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DisccountIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDisccount();
				//if (result != null && Disccount != null){
				//	return !Disccount.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _note;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(20)]		

 		[Size(150)]
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

	
       
		//private DateTime? _date;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(21)]		
		[ModelDefault("DisplayFormat", "d/M")]
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

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hàng hóa")]
		//[Index(22)]
		[DevExpress.Xpo.Association("OrderDetail-ProductItemList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductItem> ProductItemList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductItem>("ProductItemList"); 
			
        }
       
		//private string _description;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mô tả")]
        [ToolTip("Mô tả")]
		//[Index(23)]		

 		[Size(500)]
		public string Description
        { 
		    get => GetPropertyValue<string>("Description");                         
			set => SetPropertyValue<string>("Description", value); 
			
        }
		//Tooltip for Object
		public object DescriptionToolTipControllerText(View view)
        {
        //    if (Description != null) 
		//			return Description;
            return null;
        }
		//Get Default Value
        public string GetDefaultDescription(View view = null)
        { 
			return Description;
        }
		//Set Default Value
		public void SetDefaultDescription(View view = null)
        {
            //if (Description is null){
            //    var result = GetDefaultDescription(view);
            //    if (result != null && result != Description){
			//          Description = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DescriptionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDescription();
				//if (result != null && Description != null){
				//	return !Description.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _estimationdescription;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mô tả DT")]
        [ToolTip("Mô tả DT")]
		//[Index(24)]		

 		[Size(500)]
		public string EstimationDescription
        { 
		    get => GetPropertyValue<string>("EstimationDescription");                         
			set => SetPropertyValue<string>("EstimationDescription", value); 
			
        }
		//Tooltip for Object
		public object EstimationDescriptionToolTipControllerText(View view)
        {
        //    if (EstimationDescription != null) 
		//			return EstimationDescription;
            return null;
        }
		//Get Default Value
        public string GetDefaultEstimationDescription(View view = null)
        { 
			return EstimationDescription;
        }
		//Set Default Value
		public void SetDefaultEstimationDescription(View view = null)
        {
            //if (EstimationDescription is null){
            //    var result = GetDefaultEstimationDescription(view);
            //    if (result != null && result != EstimationDescription){
			//          EstimationDescription = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EstimationDescriptionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEstimationDescription();
				//if (result != null && EstimationDescription != null){
				//	return !EstimationDescription.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Order _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mua bán")]
        [ToolTip("Mua bán")]
		//[Index(25)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OrderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Order-OrderDetailList")]
	 
		public Module.BusinessObjects.Order Order
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Order>("Order");                         
			set => SetPropertyValue<Module.BusinessObjects.Order>("Order", value); 
			
        }
		//Tooltip for Object
		public object OrderToolTipControllerText(View view)
        {
        //    if (Order != null) 
		//			return Order;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Order GetDefaultOrder(View view = null)
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

		private CriteriaOperator OrderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Order));
            }
        }
	
       
		//private int? _globalorder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sắp xếp")]
        [ToolTip("Sắp xếp")]
		//[Index(26)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? GlobalOrder
        { 
		    get => GetPropertyValue<int?>("GlobalOrder");                         
			set => SetPropertyValue<int?>("GlobalOrder", value); 
			
        }
		//Tooltip for Object
		public object GlobalOrderToolTipControllerText(View view)
        {
        //    if (GlobalOrder != null) 
		//			return GlobalOrder;
            return null;
        }
		//Get Default Value
        public int? GetDefaultGlobalOrder(View view = null)
        { 
			return GlobalOrder;
        }
		//Set Default Value
		public void SetDefaultGlobalOrder(View view = null)
        {
            //if (GlobalOrder is null){
            //    var result = GetDefaultGlobalOrder(view);
            //    if (result != null && result != GlobalOrder){
			//          GlobalOrder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool GlobalOrderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultGlobalOrder();
				//if (result != null && GlobalOrder != null){
				//	return !GlobalOrder.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _total;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thành tiền")]
        [ToolTip("Thành tiền")]
		//[Index(27)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Total
        { 
		    get => GetPropertyValue<decimal?>("Total");                         
			set => SetPropertyValue<decimal?>("Total", value); 
			
        }
		//Tooltip for Object
		public object TotalToolTipControllerText(View view)
        {
        //    if (Total != null) 
		//			return Total;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool TotalIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTotal();
				//if (result != null && Total != null){
				//	return !Total.Equals(result);
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
 
        //SetDefaultProduct(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultOrg(View view = null);
        //SetDefaultUnit(View view = null);
        //SetDefaultOrigin(View view = null);
        //SetDefaultWarranty(View view = null);
        //SetDefaultEstimationName(View view = null);
        //SetDefaultSupplier(View view = null);
        //SetDefaultNo(View view = null);
        //SetDefaultParent(View view = null);
        //SetDefaultPrice(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultVAT(View view = null);
        //SetDefaultImportTax(View view = null);
        //SetDefaultListPrice(View view = null);
        //SetDefaultCostPrice(View view = null);
        //SetDefaultPricingMethod(View view = null);
        //SetDefaultMargin(View view = null);
        //SetDefaultDisccount(View view = null);
        //SetDefaultNote(View view = null);
        //SetDefaultDate(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultGlobalOrder(View view = null);
        //SetDefaultTotal(View view = null);
			
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
			//	SetDefaultProductItemList();
			//	SetDefaultDescription();
			//	SetDefaultEstimationDescription();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1560ImportCode
		public void SetDefaultTotal(View view = null)
        {
            //Code: 1560            Oid: 595f52b2-9648-4c5f-b99f-ff3c113b8ae8
            if (Total is null)
{
    var result = GetDefaultTotal();
    if (result != null && result != Total)
    {
        Total = result;
    }
}
        }
#endregion 1560ImportCode
#region 1559ImportCode
		public decimal? GetDefaultTotal(View view = null)
        {
            //Code: 1559            Oid: e2964f86-626f-4ac5-aa05-d6c78568a0c9
            var Total = Quantity * Price;
return Total;
        }
#endregion 1559ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
