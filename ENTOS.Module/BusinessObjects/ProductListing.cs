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
    [ModelDefault("Caption", "Niêm yết"), ImageName("ProductListing")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(CreatedDate))]
 
	[MobileColumnAttribute(Context = "ProductListing_ListView", TargetItems = nameof(Name)+ "," + nameof(Update)+ "," + nameof(CreatedDate))]
	[MobileColumnAttribute(Context = "Country_ProductListingList_ListView", TargetItems = nameof(Name)+ "," + nameof(Update)+ "," + nameof(CreatedDate))]
	[MobileColumnAttribute(Context = "Folder_ProductListingList_ListView", TargetItems = nameof(CreatedDate)+ "," + nameof(Update)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "ProductListing_LookupListView", TargetItems = nameof(Update)+ "," + nameof(CreatedDate)+ "," + nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class ProductListing:  DevExpress.Xpo.XPLiteObject , IWebData , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ProductListing(Session session)
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
               

		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(0)]		

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

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(1)]		

 		[Size(250)]
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

	
       
		//private string _brand;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Hãng")]
        [ToolTip("Hãng")]
		//[Index(2)]		

 		[Size(100)]
		public string Brand
        { 
		    get => GetPropertyValue<string>("Brand");                         
			set => SetPropertyValue<string>("Brand", value); 
			
        }
		//Tooltip for Object
		public object BrandToolTipControllerText(View view)
        {
        //    if (Brand != null) 
		//			return Brand;
            return null;
        }
		//Get Default Value
        public string GetDefaultBrand(View view = null)
        { 
			return Brand;
        }
		//Set Default Value
		public void SetDefaultBrand(View view = null)
        {
            //if (Brand is null){
            //    var result = GetDefaultBrand(view);
            //    if (result != null && result != Brand){
			//          Brand = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BrandIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBrand();
				//if (result != null && Brand != null){
				//	return !Brand.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _condition;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tình trạng")]
        [ToolTip("Tình trạng")]
		//[Index(3)]		

 		[Size(100)]
		public string Condition
        { 
		    get => GetPropertyValue<string>("Condition");                         
			set => SetPropertyValue<string>("Condition", value); 
			
        }
		//Tooltip for Object
		public object ConditionToolTipControllerText(View view)
        {
        //    if (Condition != null) 
		//			return Condition;
            return null;
        }
		//Get Default Value
        public string GetDefaultCondition(View view = null)
        { 
			return Condition;
        }
		//Set Default Value
		public void SetDefaultCondition(View view = null)
        {
            //if (Condition is null){
            //    var result = GetDefaultCondition(view);
            //    if (result != null && result != Condition){
			//          Condition = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ConditionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCondition();
				//if (result != null && Condition != null){
				//	return !Condition.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _price;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giá bán")]
        [ToolTip("Giá bán")]
		//[Index(4)]		
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

	
       
		//private decimal? _pricesale;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá KM")]
        [ToolTip("Giá KM")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? PriceSale
        { 
		    get => GetPropertyValue<decimal?>("PriceSale");                         
			set => SetPropertyValue<decimal?>("PriceSale", value); 
			
        }
		//Tooltip for Object
		public object PriceSaleToolTipControllerText(View view)
        {
        //    if (PriceSale != null) 
		//			return PriceSale;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultPriceSale(View view = null)
        { 
			return PriceSale;
        }
		//Set Default Value
		public void SetDefaultPriceSale(View view = null)
        {
            //if (PriceSale is null){
            //    var result = GetDefaultPriceSale(view);
            //    if (result != null && result != PriceSale){
			//          PriceSale = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PriceSaleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPriceSale();
				//if (result != null && PriceSale != null){
				//	return !PriceSale.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _seller;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Người bán")]
        [ToolTip("Người bán")]
		//[Index(6)]		

 		[Size(250)]
		public string Seller
        { 
		    get => GetPropertyValue<string>("Seller");                         
			set => SetPropertyValue<string>("Seller", value); 
			
        }
		//Tooltip for Object
		public object SellerToolTipControllerText(View view)
        {
        //    if (Seller != null) 
		//			return Seller;
            return null;
        }
		//Get Default Value
        public string GetDefaultSeller(View view = null)
        { 
			return Seller;
        }
		//Set Default Value
		public void SetDefaultSeller(View view = null)
        {
            //if (Seller is null){
            //    var result = GetDefaultSeller(view);
            //    if (result != null && result != Seller){
			//          Seller = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SellerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSeller();
				//if (result != null && Seller != null){
				//	return !Seller.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ảnh")]
        [ToolTip("Ảnh")]
		//[Index(7)]		
		[Appearance("ẢnhBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Image
        { 
		    get => GetPropertyValue<byte[]>("Image");                         
			set => SetPropertyValue<byte[]>("Image", value); 
			
        }
		//Tooltip for Object
		public object ImageToolTipControllerText(View view)
        {
        //    if (Image != null) 
		//			return Image;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultImage(View view = null)
        { 
			return Image;
        }
		//Set Default Value
		public void SetDefaultImage(View view = null)
        {
            //if (Image is null){
            //    var result = GetDefaultImage(view);
            //    if (result != null && result != Image){
			//          Image = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ImageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImage();
				//if (result != null && Image != null){
				//	return !Image.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _stocklocal;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tồn kho")]
        [ToolTip("Tồn kho")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? StockLocal
        { 
		    get => GetPropertyValue<int?>("StockLocal");                         
			set => SetPropertyValue<int?>("StockLocal", value); 
			
        }
		//Tooltip for Object
		public object StockLocalToolTipControllerText(View view)
        {
        //    if (StockLocal != null) 
		//			return StockLocal;
            return null;
        }
		//Get Default Value
        public int? GetDefaultStockLocal(View view = null)
        { 
			return StockLocal;
        }
		//Set Default Value
		public void SetDefaultStockLocal(View view = null)
        {
            //if (StockLocal is null){
            //    var result = GetDefaultStockLocal(view);
            //    if (result != null && result != StockLocal){
			//          StockLocal = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StockLocalIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStockLocal();
				//if (result != null && StockLocal != null){
				//	return !StockLocal.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Product _product;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sản phẩm")]
        [ToolTip("Sản phẩm")]
		//[Index(9)]		
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
	
       
		//private int? _stockshop;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ký gửi")]
        [ToolTip("Ký gửi")]
		//[Index(10)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? StockShop
        { 
		    get => GetPropertyValue<int?>("StockShop");                         
			set => SetPropertyValue<int?>("StockShop", value); 
			
        }
		//Tooltip for Object
		public object StockShopToolTipControllerText(View view)
        {
        //    if (StockShop != null) 
		//			return StockShop;
            return null;
        }
		//Get Default Value
        public int? GetDefaultStockShop(View view = null)
        { 
			return StockShop;
        }
		//Set Default Value
		public void SetDefaultStockShop(View view = null)
        {
            //if (StockShop is null){
            //    var result = GetDefaultStockShop(view);
            //    if (result != null && result != StockShop){
			//          StockShop = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StockShopIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStockShop();
				//if (result != null && StockShop != null){
				//	return !StockShop.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _sold;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đã bán")]
        [ToolTip("Đã bán")]
		//[Index(11)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Sold
        { 
		    get => GetPropertyValue<int?>("Sold");                         
			set => SetPropertyValue<int?>("Sold", value); 
			
        }
		//Tooltip for Object
		public object SoldToolTipControllerText(View view)
        {
        //    if (Sold != null) 
		//			return Sold;
            return null;
        }
		//Get Default Value
        public int? GetDefaultSold(View view = null)
        { 
			return Sold;
        }
		//Set Default Value
		public void SetDefaultSold(View view = null)
        {
            //if (Sold is null){
            //    var result = GetDefaultSold(view);
            //    if (result != null && result != Sold){
			//          Sold = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SoldIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSold();
				//if (result != null && Sold != null){
				//	return !Sold.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _ratingstar;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đánh giá")]
        [ToolTip("Đánh giá")]
		//[Index(12)]		
		[ModelDefault("DisplayFormat", "{0:n1}")]
		[ModelDefault("EditMask", "n1")]
		public decimal? RatingStar
        { 
		    get => GetPropertyValue<decimal?>("RatingStar");                         
			set => SetPropertyValue<decimal?>("RatingStar", value); 
			
        }
		//Tooltip for Object
		public object RatingStarToolTipControllerText(View view)
        {
        //    if (RatingStar != null) 
		//			return RatingStar;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultRatingStar(View view = null)
        { 
			return RatingStar;
        }
		//Set Default Value
		public void SetDefaultRatingStar(View view = null)
        {
            //if (RatingStar is null){
            //    var result = GetDefaultRatingStar(view);
            //    if (result != null && result != RatingStar){
			//          RatingStar = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RatingStarIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRatingStar();
				//if (result != null && RatingStar != null){
				//	return !RatingStar.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _reviewquantity;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lượng đánh giá")]
        [ToolTip("Lượng đánh giá")]
		//[Index(13)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? ReviewQuantity
        { 
		    get => GetPropertyValue<int?>("ReviewQuantity");                         
			set => SetPropertyValue<int?>("ReviewQuantity", value); 
			
        }
		//Tooltip for Object
		public object ReviewQuantityToolTipControllerText(View view)
        {
        //    if (ReviewQuantity != null) 
		//			return ReviewQuantity;
            return null;
        }
		//Get Default Value
        public int? GetDefaultReviewQuantity(View view = null)
        { 
			return ReviewQuantity;
        }
		//Set Default Value
		public void SetDefaultReviewQuantity(View view = null)
        {
            //if (ReviewQuantity is null){
            //    var result = GetDefaultReviewQuantity(view);
            //    if (result != null && result != ReviewQuantity){
			//          ReviewQuantity = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ReviewQuantityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultReviewQuantity();
				//if (result != null && ReviewQuantity != null){
				//	return !ReviewQuantity.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _auction;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đấu giá")]
        [ToolTip("Đấu giá")]
		//[Index(14)]		
		public bool Auction
        { 
		    get => GetPropertyValue<bool>("Auction");                         
			set => SetPropertyValue<bool>("Auction", value); 
			
        }
		//Tooltip for Object
		public object AuctionToolTipControllerText(View view)
        {
        //    if (Auction != null) 
		//			return Auction;
            return null;
        }
		//Get Default Value
        public bool GetDefaultAuction(View view = null)
        { 
			return Auction;
        }
		//Set Default Value
		public void SetDefaultAuction(View view = null)
        {
            //if (Auction is null){
            //    var result = GetDefaultAuction(view);
            //    if (result != null && result != Auction){
			//          Auction = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AuctionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAuction();
				//if (result != null && Auction != null){
				//	return !Auction.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _description;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mô tả")]
        [ToolTip("Mô tả")]
		//[Index(15)]		

 		[Size(SizeAttribute.Unlimited)]
	    [ModelDefault("DocumentStorageFormat", "Html")]
	    [EditorAlias(EditorAliases.RichTextPropertyEditor)]
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

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
		//[Index(16)]
		[DevExpress.Xpo.Association("ProductListing-BookMarkList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BookMark> BookMarkList
        {      
		    get => GetCollection<Module.BusinessObjects.BookMark>("BookMarkList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quốc gia")]
		//[Index(17)]
		[DataSourceCriteria("Not ProductListingList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("CountryList-ProductListingList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Country> CountryList
        {      
		    get => GetCollection<Module.BusinessObjects.Country>("CountryList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
		//[Index(18)]
		[DataSourceCriteria("Not ProductListingList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("FolderList-ProductListingList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Folder> FolderList
        {      
		    get => GetCollection<Module.BusinessObjects.Folder>("FolderList"); 
			
        }
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(19)]		
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

	
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(20)]		
		public bool InActive
        { 
		    get => GetPropertyValue<bool>("InActive");                         
			set => SetPropertyValue<bool>("InActive", value); 
			
        }
		//Tooltip for Object
		public object InActiveToolTipControllerText(View view)
        {
        //    if (InActive != null) 
		//			return InActive;
            return null;
        }
		//Get Default Value
        public bool GetDefaultInActive(View view = null)
        { 
			return InActive;
        }
		//Set Default Value
		public void SetDefaultInActive(View view = null)
        {
            //if (InActive is null){
            //    var result = GetDefaultInActive(view);
            //    if (result != null && result != InActive){
			//          InActive = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool InActiveIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultInActive();
				//if (result != null && InActive != null){
				//	return !InActive.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _createddate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Từ ngày")]
        [ToolTip("Từ ngày")]
		//[Index(21)]		
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

	
       
		//private Module.BusinessObjects.ShopOnline _onlineshop;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cửa hàng")]
        [ToolTip("Cửa hàng")]
		//[Index(22)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OnlineShopCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.ShopOnline OnlineShop
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ShopOnline>("OnlineShop");                         
			set => SetPropertyValue<Module.BusinessObjects.ShopOnline>("OnlineShop", value); 
			
        }
		//Tooltip for Object
		public object OnlineShopToolTipControllerText(View view)
        {
        //    if (OnlineShop != null) 
		//			return OnlineShop;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ShopOnline GetDefaultOnlineShop(View view = null)
        { 
			return OnlineShop;
        }
		//Set Default Value
		public void SetDefaultOnlineShop(View view = null)
        {
            //if (OnlineShop is null){
            //    var result = GetDefaultOnlineShop(view);
            //    if (result != null && result != OnlineShop){
			//          OnlineShop = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OnlineShopIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOnlineShop();
				//if (result != null && OnlineShop != null){
				//	return !OnlineShop.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator OnlineShopCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(OnlineShop));
            }
        }
	
       
		//private string _mpn;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã sản phẩm")]
        [ToolTip("Mã sản phẩm")]
		//[Index(23)]		

 		[Size(100)]
		public string MPN
        { 
		    get => GetPropertyValue<string>("MPN");                         
			set => SetPropertyValue<string>("MPN", value); 
			
        }
		//Tooltip for Object
		public object MPNToolTipControllerText(View view)
        {
        //    if (MPN != null) 
		//			return MPN;
            return null;
        }
		//Get Default Value
        public string GetDefaultMPN(View view = null)
        { 
			return MPN;
        }
		//Set Default Value
		public void SetDefaultMPN(View view = null)
        {
            //if (MPN is null){
            //    var result = GetDefaultMPN(view);
            //    if (result != null && result != MPN){
			//          MPN = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MPNIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMPN();
				//if (result != null && MPN != null){
				//	return !MPN.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _epid;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã eBay")]
        [ToolTip("Mã eBay")]
		//[Index(24)]		

 		[Size(100)]
		public string ePID
        { 
		    get => GetPropertyValue<string>("ePID");                         
			set => SetPropertyValue<string>("ePID", value); 
			
        }
		//Tooltip for Object
		public object ePIDToolTipControllerText(View view)
        {
        //    if (ePID != null) 
		//			return ePID;
            return null;
        }
		//Get Default Value
        public string GetDefaultePID(View view = null)
        { 
			return ePID;
        }
		//Set Default Value
		public void SetDefaultePID(View view = null)
        {
            //if (ePID is null){
            //    var result = GetDefaultePID(view);
            //    if (result != null && result != ePID){
			//          ePID = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ePIDIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultePID();
				//if (result != null && ePID != null){
				//	return !ePID.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _asin;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã Amazon")]
        [ToolTip("Mã Amazon")]
		//[Index(25)]		

 		[Size(100)]
		public string ASIN
        { 
		    get => GetPropertyValue<string>("ASIN");                         
			set => SetPropertyValue<string>("ASIN", value); 
			
        }
		//Tooltip for Object
		public object ASINToolTipControllerText(View view)
        {
        //    if (ASIN != null) 
		//			return ASIN;
            return null;
        }
		//Get Default Value
        public string GetDefaultASIN(View view = null)
        { 
			return ASIN;
        }
		//Set Default Value
		public void SetDefaultASIN(View view = null)
        {
            //if (ASIN is null){
            //    var result = GetDefaultASIN(view);
            //    if (result != null && result != ASIN){
			//          ASIN = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ASINIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultASIN();
				//if (result != null && ASIN != null){
				//	return !ASIN.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _upc;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã UPC")]
        [ToolTip("Mã UPC")]
		//[Index(26)]		

 		[Size(100)]
		public string UPC
        { 
		    get => GetPropertyValue<string>("UPC");                         
			set => SetPropertyValue<string>("UPC", value); 
			
        }
		//Tooltip for Object
		public object UPCToolTipControllerText(View view)
        {
        //    if (UPC != null) 
		//			return UPC;
            return null;
        }
		//Get Default Value
        public string GetDefaultUPC(View view = null)
        { 
			return UPC;
        }
		//Set Default Value
		public void SetDefaultUPC(View view = null)
        {
            //if (UPC is null){
            //    var result = GetDefaultUPC(view);
            //    if (result != null && result != UPC){
			//          UPC = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UPCIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUPC();
				//if (result != null && UPC != null){
				//	return !UPC.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _ean;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã EAN")]
        [ToolTip("Mã EAN")]
		//[Index(27)]		

 		[Size(100)]
		public string EAN
        { 
		    get => GetPropertyValue<string>("EAN");                         
			set => SetPropertyValue<string>("EAN", value); 
			
        }
		//Tooltip for Object
		public object EANToolTipControllerText(View view)
        {
        //    if (EAN != null) 
		//			return EAN;
            return null;
        }
		//Get Default Value
        public string GetDefaultEAN(View view = null)
        { 
			return EAN;
        }
		//Set Default Value
		public void SetDefaultEAN(View view = null)
        {
            //if (EAN is null){
            //    var result = GetDefaultEAN(view);
            //    if (result != null && result != EAN){
			//          EAN = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EANIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEAN();
				//if (result != null && EAN != null){
				//	return !EAN.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 1318ImportCode
            base.AfterConstruction();
SetDefaultCreatedDate();
            #endregion 1318ImportCode
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultBrand(View view = null);
        //SetDefaultCondition(View view = null);
        //SetDefaultPrice(View view = null);
        //SetDefaultPriceSale(View view = null);
        //SetDefaultSeller(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultStockLocal(View view = null);
        //SetDefaultProduct(View view = null);
        //SetDefaultStockShop(View view = null);
        //SetDefaultSold(View view = null);
        //SetDefaultRatingStar(View view = null);
        //SetDefaultReviewQuantity(View view = null);
        //SetDefaultAuction(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultInActive(View view = null);
        //SetDefaultCreatedDate(View view = null);
        //SetDefaultOnlineShop(View view = null);
        //SetDefaultMPN(View view = null);
        //SetDefaultePID(View view = null);
        //SetDefaultASIN(View view = null);
        //SetDefaultUPC(View view = null);
        //SetDefaultEAN(View view = null);
			
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
            #region 1314ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 1314ImportCode
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
            Session.Delete(this.BookMarkList);				
            Session.Delete(this.CountryList);				
  
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
			//	SetDefaultDescription();
			//	SetDefaultBookMarkList();
			//	SetDefaultCountryList();
			//	SetDefaultFolderList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1316ImportCode
		public DateTime? GetDefaultCreatedDate(View view = null)
        {
            //Code: 1316            Oid: 49af642e-dc9b-44b6-afd8-df9b149583bb
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1316ImportCode
#region 1313ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 1313            Oid: b2116d82-96e2-416c-bfdb-a442e77d3d0c
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 1313ImportCode
#region 1317ImportCode
		public void SetDefaultCreatedDate(View view = null)
        {
            //Code: 1317            Oid: 1c5e2222-7bf4-4511-9cba-8a0cd0514ba2
            if(CreatedDate == null) CreatedDate = GetDefaultCreatedDate();
        }
#endregion 1317ImportCode
#region 1315ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 1315            Oid: 56a97460-30c1-48cc-8dd8-2ea4b54af57b
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1315ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
