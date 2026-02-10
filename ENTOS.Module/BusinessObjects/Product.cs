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
	[NavigationItem("ProductBusiness")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Sản phẩm"), ImageName("Product")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("Product ProductAttributeValueList Hide_None__" , TargetItems = "ProductAttributeValueList" , Criteria = "[ProductParent] Is Null",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
	[Appearance("Product ProductList Hide_None__" , TargetItems = "ProductList" , Criteria = "[Variation] = False",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
	[Appearance("Product ProductList, Variation, Feature, ProductInterfaceList, Introduction, BookMarkList, CustomsDescription, DomainProductList, Specification, FolderList, ProductAttributeList Hide_None__" , TargetItems = "ProductList, Variation, Feature, ProductInterfaceList, Introduction, BookMarkList, CustomsDescription, DomainProductList, Specification, FolderList, ProductAttributeList" , Criteria = "[ProductParent] Is Not Null",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(ProductInterfaceList)+ "," + nameof(Feature)+ "," + nameof(Introduction)+ "," + nameof(Specification)+ "," + nameof(Manual), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(PermissionPolicyRole)+ "," + nameof(Update)+ "," + nameof(Updater)+ "," + nameof(CreatedDate)+ "," + nameof(Org)+ "," + nameof(Order))]
 
	[MobileColumnAttribute(Context = "Product_ProductList_ListView", TargetItems = nameof(Name)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Folder_GroupProductList_ListView", TargetItems = nameof(Image)+ "," + nameof(Name)+ "," + nameof(Update))]
	[MobileColumnAttribute(Context = "Product_ListView", TargetItems = nameof(Image)+ "," + nameof(Name)+ "," + nameof(Brand))]
	[MobileColumnAttribute(Context = "Product_LookupListView", TargetItems = nameof(Image)+ "," + nameof(Name)+ "," + nameof(Brand))]
	[MobileColumnAttribute(Context = "ProductAttributeValue_ProductList_ListView", TargetItems = nameof(Image)+ "," + nameof(Update)+ "," + nameof(Name))]
	[DefaultProperty("Name")]
 
	
	[CustomFilter("IFolderLookup", "FolderList[Oid = ?]")]
	
	[CustomFilter("IFilteringFolderInProduct", "FolderList[Oid = ?]")]
	[RuleCombinationOfPropertiesIsUnique("UniqueRule.ProductName", DefaultContexts.Save, "Name, PermissionPolicyRole")]
	[RuleCombinationOfPropertiesIsUnique("UniqueRule.ProductCode", DefaultContexts.Save, "Code, PermissionPolicyRole")]
[OptimisticLocking(true)]
    public partial class Product:  DevExpress.Xpo.XPLiteObject , IObjectImage, IWebData ,IFolderLookup,IFilteringFolderInProduct, INoIndexColumn, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public Product(Session session)
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
				if (ProductAttributeList.IsLoaded)
                {
                    if (ProductAttributeList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ProductAttributeList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ProductAttributeList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ProductAttribute>(CriteriaOperator.Parse("[Product.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool productattributelist = Session.Query<Module.BusinessObjects.ProductAttribute>().Where(x => x.Product.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ProductAttributeList), productattributelist);
                        if (productattributelist)
                            return true;

                    }                    
                }				
				if (ProductInterfaceList.IsLoaded)
                {
                    if (ProductInterfaceList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ProductInterfaceList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ProductInterfaceList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ProductInterface>(CriteriaOperator.Parse("[Product.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool productinterfacelist = Session.Query<Module.BusinessObjects.ProductInterface>().Where(x => x.Product.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ProductInterfaceList), productinterfacelist);
                        if (productinterfacelist)
                            return true;

                    }                    
                }				
				if (ProductPriceList.IsLoaded)
                {
                    if (ProductPriceList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ProductPriceList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ProductPriceList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ProductPrice>(CriteriaOperator.Parse("[Product.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool productpricelist = Session.Query<Module.BusinessObjects.ProductPrice>().Where(x => x.Product.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ProductPriceList), productpricelist);
                        if (productpricelist)
                            return true;

                    }                    
                }				
				if (ProductList.IsLoaded)
                {
                    if (ProductList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ProductList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ProductList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Product>(CriteriaOperator.Parse("[ProductParent.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool productlist = Session.Query<Module.BusinessObjects.Product>().Where(x => x.ProductParent.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ProductList), productlist);
                        if (productlist)
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
               

		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(0)]		

 		[Size(250)]
		[RuleRequiredField("RequiredProductName", DefaultContexts.Save)]
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
		//[Index(1)]		

 		[Size(150)]
	    [ModelDefault("PropertyEditorType", "StringComboEditor")]
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

	
       
		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(2)]		

 		[Size(50)]
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

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
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
	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ảnh")]
        [ToolTip("Ảnh")]
		//[Index(4)]		
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

	
       
		//private decimal? _price;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giá")]
        [ToolTip("Giá")]
		//[Index(5)]		
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

	
       
		//private decimal? _pricesale;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá KM")]
        [ToolTip("Giá KM")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
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

	
       
		//private DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole _permissionpolicyrole;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhóm")]
        [ToolTip("Nhóm")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PermissionPolicyRoleCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole PermissionPolicyRole
        { 
		    get => GetPropertyValue<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole>("PermissionPolicyRole");                         
			set => SetPropertyValue<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole>("PermissionPolicyRole", value); 
			
        }
		//Tooltip for Object
		public object PermissionPolicyRoleToolTipControllerText(View view)
        {
        //    if (PermissionPolicyRole != null) 
		//			return PermissionPolicyRole;
            return null;
        }
		//Get Default Value
        public DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole GetDefaultPermissionPolicyRole(View view = null)
        { 
			return PermissionPolicyRole;
        }
		//Set Default Value

		//Check Not Validate
		protected bool PermissionPolicyRoleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPermissionPolicyRole();
				//if (result != null && PermissionPolicyRole != null){
				//	return !PermissionPolicyRole.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PermissionPolicyRoleCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PermissionPolicyRole));
            }
        }
	
       
		//private bool _variation;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Có biến thể")]
        [ToolTip("Có biến thể")]
		//[Index(8)]		
		public bool Variation
        { 
		    get => GetPropertyValue<bool>("Variation");                         
			set => SetPropertyValue<bool>("Variation", value); 
			
        }
		//Tooltip for Object
		public object VariationToolTipControllerText(View view)
        {
        //    if (Variation != null) 
		//			return Variation;
            return null;
        }
		//Get Default Value
        public bool GetDefaultVariation(View view = null)
        { 
			return Variation;
        }
		//Set Default Value
		public void SetDefaultVariation(View view = null)
        {
            //if (Variation is null){
            //    var result = GetDefaultVariation(view);
            //    if (result != null && result != Variation){
			//          Variation = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool VariationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultVariation();
				//if (result != null && Variation != null){
				//	return !Variation.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuộc tính")]
		//[Index(9)]
		[DevExpress.Xpo.Association("Product-ProductAttributeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductAttribute> ProductAttributeList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductAttribute>("ProductAttributeList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giao diện")]
		//[Index(10)]
		[DevExpress.Xpo.Association("Product-ProductInterfaceList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductInterface> ProductInterfaceList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductInterface>("ProductInterfaceList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
		//[Index(11)]
		[DevExpress.Xpo.Association("Product-BookMarkList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BookMark> BookMarkList
        {      
		    get => GetCollection<Module.BusinessObjects.BookMark>("BookMarkList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá mua")]
		//[Index(12)]
		[DevExpress.Xpo.Association("Product-ProductPriceList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductPrice> ProductPriceList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductPrice>("ProductPriceList"); 
			
        }
       
		//private string _feature;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đặc điểm")]
        [ToolTip("Đặc điểm")]
		//[Index(14)]		

 		[Size(SizeAttribute.Unlimited)]
	    [EditorAlias(EditorAliases.RichTextPropertyEditor)]
	    [ModelDefault("DocumentStorageFormat", "Html")]
		public string Feature
        { 
		    get => GetPropertyValue<string>("Feature");                         
			set => SetPropertyValue<string>("Feature", value); 
			
        }
		//Tooltip for Object
		public object FeatureToolTipControllerText(View view)
        {
        //    if (Feature != null) 
		//			return Feature;
            return null;
        }
		//Get Default Value
        public string GetDefaultFeature(View view = null)
        { 
			return Feature;
        }
		//Set Default Value
		public void SetDefaultFeature(View view = null)
        {
            //if (Feature is null){
            //    var result = GetDefaultFeature(view);
            //    if (result != null && result != Feature){
			//          Feature = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FeatureIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFeature();
				//if (result != null && Feature != null){
				//	return !Feature.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _introduction;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giới thiệu")]
        [ToolTip("Giới thiệu")]
		//[Index(15)]		

 		[Size(SizeAttribute.Unlimited)]
	    [ModelDefault("DocumentStorageFormat", "Html")]
	    [EditorAlias(EditorAliases.RichTextPropertyEditor)]
		public string Introduction
        { 
		    get => GetPropertyValue<string>("Introduction");                         
			set => SetPropertyValue<string>("Introduction", value); 
			
        }
		//Tooltip for Object
		public object IntroductionToolTipControllerText(View view)
        {
        //    if (Introduction != null) 
		//			return Introduction;
            return null;
        }
		//Get Default Value
        public string GetDefaultIntroduction(View view = null)
        { 
			return Introduction;
        }
		//Set Default Value
		public void SetDefaultIntroduction(View view = null)
        {
            //if (Introduction is null){
            //    var result = GetDefaultIntroduction(view);
            //    if (result != null && result != Introduction){
			//          Introduction = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool IntroductionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultIntroduction();
				//if (result != null && Introduction != null){
				//	return !Introduction.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _specification;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thông số")]
        [ToolTip("Thông số")]
		//[Index(16)]		

 		[Size(SizeAttribute.Unlimited)]
	    [ModelDefault("DocumentStorageFormat", "Html")]
	    [EditorAlias(EditorAliases.RichTextPropertyEditor)]
		public string Specification
        { 
		    get => GetPropertyValue<string>("Specification");                         
			set => SetPropertyValue<string>("Specification", value); 
			
        }
		//Tooltip for Object
		public object SpecificationToolTipControllerText(View view)
        {
        //    if (Specification != null) 
		//			return Specification;
            return null;
        }
		//Get Default Value
        public string GetDefaultSpecification(View view = null)
        { 
			return Specification;
        }
		//Set Default Value
		public void SetDefaultSpecification(View view = null)
        {
            //if (Specification is null){
            //    var result = GetDefaultSpecification(view);
            //    if (result != null && result != Specification){
			//          Specification = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpecificationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpecification();
				//if (result != null && Specification != null){
				//	return !Specification.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _manual;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sử dụng")]
        [ToolTip("Sử dụng")]
		//[Index(17)]		

 		[Size(SizeAttribute.Unlimited)]
	    [ModelDefault("DocumentStorageFormat", "Html")]
	    [EditorAlias(EditorAliases.RichTextPropertyEditor)]
		public string Manual
        { 
		    get => GetPropertyValue<string>("Manual");                         
			set => SetPropertyValue<string>("Manual", value); 
			
        }
		//Tooltip for Object
		public object ManualToolTipControllerText(View view)
        {
        //    if (Manual != null) 
		//			return Manual;
            return null;
        }
		//Get Default Value
        public string GetDefaultManual(View view = null)
        { 
			return Manual;
        }
		//Set Default Value
		public void SetDefaultManual(View view = null)
        {
            //if (Manual is null){
            //    var result = GetDefaultManual(view);
            //    if (result != null && result != Manual){
			//          Manual = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ManualIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultManual();
				//if (result != null && Manual != null){
				//	return !Manual.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Biến thể")]
		//[Index(18)]
		[DevExpress.Xpo.Association("ProductParent-ProductList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Product> ProductList
        {      
		    get => GetCollection<Module.BusinessObjects.Product>("ProductList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá trị thuộc tính SP")]
		//[Index(19)]
		[DataSourceCriteria("Not ProductList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("ProductAttributeValueList-ProductList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductAttributeValue> ProductAttributeValueList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductAttributeValue>("ProductAttributeValueList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
		//[Index(20)]
		[DataSourceCriteria("Not ProductList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("FolderList-ProductList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Folder> FolderList
        {      
		    get => GetCollection<Module.BusinessObjects.Folder>("FolderList"); 
			
        }
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(21)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
	    [ModelDefault("AllowEdit", "False")]
		public DateTime Update
        { 
		    get => GetPropertyValue<DateTime>("Update");                         
			set => SetPropertyValue<DateTime>("Update", value); 
			
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
		//[Index(22)]		
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
		//[Index(23)]		
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

	
       
		//private Module.BusinessObjects.Org _org;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tổ chức")]
        [ToolTip("Tổ chức")]
		//[Index(24)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[OrgType] = ##ToString#Brand#")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Org Org
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Org>("Org");                         
			set => SetPropertyValue<Module.BusinessObjects.Org>("Org", value); 
			
        }
		//Tooltip for Object
		public object OrgToolTipControllerText(View view)
        {
        //    if (Org != null) 
		//			return Org;
            return null;
        }
		//Get Default Value
		//Set Default Value

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

		private CriteriaOperator OrgCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Org));
            }
        }
	
       
		//private Module.BusinessObjects.Folder _folder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(25)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#Product# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-GroupProductList")]
	 
		public Module.BusinessObjects.Folder Folder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Folder>("Folder");                         
			set => SetPropertyValue<Module.BusinessObjects.Folder>("Folder", value); 
			
        }
		//Tooltip for Object
		public object FolderToolTipControllerText(View view)
        {
        //    if (Folder != null) 
		//			return Folder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Folder GetDefaultFolder(View view = null)
        { 
			return Folder;
        }
		//Set Default Value
		public void SetDefaultFolder(View view = null)
        {
            //if (Folder is null){
            //    var result = GetDefaultFolder(view);
            //    if (result != null && result != Folder){
			//          Folder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFolder();
				//if (result != null && Folder != null){
				//	return !Folder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator FolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Folder));
            }
        }
	
       
		//private string _english;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên khác")]
        [ToolTip("Tên khác")]
		//[Index(26)]		

 		[Size(2000)]
		public string English
        { 
		    get => GetPropertyValue<string>("English");                         
			set => SetPropertyValue<string>("English", value); 
			
        }
		//Tooltip for Object
		public object EnglishToolTipControllerText(View view)
        {
        //    if (English != null) 
		//			return English;
            return null;
        }
		//Get Default Value
        public string GetDefaultEnglish(View view = null)
        { 
			return English;
        }
		//Set Default Value
		public void SetDefaultEnglish(View view = null)
        {
            //if (English is null){
            //    var result = GetDefaultEnglish(view);
            //    if (result != null && result != English){
			//          English = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EnglishIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEnglish();
				//if (result != null && English != null){
				//	return !English.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Country _countryorigin;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Xuất xứ")]
        [ToolTip("Xuất xứ")]
		//[Index(27)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CountryOriginCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Country CountryOrigin
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Country>("CountryOrigin");                         
			set => SetPropertyValue<Module.BusinessObjects.Country>("CountryOrigin", value); 
			
        }
		//Tooltip for Object
		public object CountryOriginToolTipControllerText(View view)
        {
        //    if (CountryOrigin != null) 
		//			return CountryOrigin;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Country GetDefaultCountryOrigin(View view = null)
        { 
			return CountryOrigin;
        }
		//Set Default Value
		public void SetDefaultCountryOrigin(View view = null)
        {
            //if (CountryOrigin is null){
            //    var result = GetDefaultCountryOrigin(view);
            //    if (result != null && result != CountryOrigin){
			//          CountryOrigin = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CountryOriginIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCountryOrigin();
				//if (result != null && CountryOrigin != null){
				//	return !CountryOrigin.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CountryOriginCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(CountryOrigin));
            }
        }
	
       
		//private int? _warranty;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bảo hành")]
        [ToolTip("Bảo hành")]
		//[Index(28)]		
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

	
       
		//private Round _round;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tròn giá")]
        [ToolTip("Tròn giá")]
		//[Index(29)]		
		public Round Round
        { 
		    get => GetPropertyValue<Round>("Round");                         
			set => SetPropertyValue<Round>("Round", value); 
			
        }
		//Tooltip for Object
		public object RoundToolTipControllerText(View view)
        {
        //    if (Round != null) 
		//			return Round;
            return null;
        }
		//Get Default Value
        public Round GetDefaultRound(View view = null)
        { 
			return Round;
        }
		//Set Default Value
		public void SetDefaultRound(View view = null)
        {
            //if (Round is null){
            //    var result = GetDefaultRound(view);
            //    if (result != null && result != Round){
			//          Round = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RoundIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRound();
				//if (result != null && Round != null){
				//	return !Round.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _ean;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã EAN")]
        [ToolTip("Mã EAN")]
		//[Index(30)]		

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

	
       
		//private Module.BusinessObjects.Currency _currency;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đồng tiền")]
        [ToolTip("Đồng tiền")]
		//[Index(31)]		
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
	
       
		//private Module.BusinessObjects.HScode _hscode;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã HS")]
        [ToolTip("Mã HS")]
		//[Index(32)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(HsCodeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.HScode HsCode
        { 
		    get => GetPropertyValue<Module.BusinessObjects.HScode>("HsCode");                         
			set => SetPropertyValue<Module.BusinessObjects.HScode>("HsCode", value); 
			
        }
		//Tooltip for Object
		public object HsCodeToolTipControllerText(View view)
        {
        //    if (HsCode != null) 
		//			return HsCode;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.HScode GetDefaultHsCode(View view = null)
        { 
			return HsCode;
        }
		//Set Default Value
		public void SetDefaultHsCode(View view = null)
        {
            //if (HsCode is null){
            //    var result = GetDefaultHsCode(view);
            //    if (result != null && result != HsCode){
			//          HsCode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool HsCodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultHsCode();
				//if (result != null && HsCode != null){
				//	return !HsCode.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator HsCodeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(HsCode));
            }
        }
	
       
		//private string _customsdescription;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mô tả HQ")]
        [ToolTip("Mô tả HQ")]
		//[Index(33)]		

 		[Size(200)]
		public string CustomsDescription
        { 
		    get => GetPropertyValue<string>("CustomsDescription");                         
			set => SetPropertyValue<string>("CustomsDescription", value); 
			
        }
		//Tooltip for Object
		public object CustomsDescriptionToolTipControllerText(View view)
        {
        //    if (CustomsDescription != null) 
		//			return CustomsDescription;
            return null;
        }
		//Get Default Value
        public string GetDefaultCustomsDescription(View view = null)
        { 
			return CustomsDescription;
        }
		//Set Default Value
		public void SetDefaultCustomsDescription(View view = null)
        {
            //if (CustomsDescription is null){
            //    var result = GetDefaultCustomsDescription(view);
            //    if (result != null && result != CustomsDescription){
			//          CustomsDescription = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CustomsDescriptionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCustomsDescription();
				//if (result != null && CustomsDescription != null){
				//	return !CustomsDescription.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _size;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kích thước")]
        [ToolTip("Kích thước")]
		//[Index(34)]		

 		[Size(100)]
		public string Size
        { 
		    get => GetPropertyValue<string>("Size");                         
			set => SetPropertyValue<string>("Size", value); 
			
        }
		//Tooltip for Object
		public object SizeToolTipControllerText(View view)
        {
        //    if (Size != null) 
		//			return Size;
            return null;
        }
		//Get Default Value
        public string GetDefaultSize(View view = null)
        { 
			return Size;
        }
		//Set Default Value
		public void SetDefaultSize(View view = null)
        {
            //if (Size is null){
            //    var result = GetDefaultSize(view);
            //    if (result != null && result != Size){
			//          Size = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SizeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSize();
				//if (result != null && Size != null){
				//	return !Size.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _weight;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Khối lượng")]
        [ToolTip("Khối lượng")]
		//[Index(35)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Weight
        { 
		    get => GetPropertyValue<decimal?>("Weight");                         
			set => SetPropertyValue<decimal?>("Weight", value); 
			
        }
		//Tooltip for Object
		public object WeightToolTipControllerText(View view)
        {
        //    if (Weight != null) 
		//			return Weight;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultWeight(View view = null)
        { 
			return Weight;
        }
		//Set Default Value
		public void SetDefaultWeight(View view = null)
        {
            //if (Weight is null){
            //    var result = GetDefaultWeight(view);
            //    if (result != null && result != Weight){
			//          Weight = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WeightIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWeight();
				//if (result != null && Weight != null){
				//	return !Weight.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _weightconverted;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("KL quy đổi")]
        [ToolTip("KL quy đổi")]
		//[Index(36)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
	    [NotMapped()]
	    [NonPersistent()]
		public decimal? WeightConverted
        { 
		    get => GetPropertyValue<decimal?>("WeightConverted");                         
			set => SetPropertyValue<decimal?>("WeightConverted", value); 
			
        }
		//Tooltip for Object
		public object WeightConvertedToolTipControllerText(View view)
        {
        //    if (WeightConverted != null) 
		//			return WeightConverted;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultWeightConverted(View view = null)
        { 
			return WeightConverted;
        }
		//Set Default Value
		public void SetDefaultWeightConverted(View view = null)
        {
            //if (WeightConverted is null){
            //    var result = GetDefaultWeightConverted(view);
            //    if (result != null && result != WeightConverted){
			//          WeightConverted = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WeightConvertedIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWeightConverted();
				//if (result != null && WeightConverted != null){
				//	return !WeightConverted.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(37)]		
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

	
       
		//private DateTime? _releasedate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Phát hành")]
        [ToolTip("Phát hành")]
		//[Index(38)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? ReleaseDate
        { 
		    get => GetPropertyValue<DateTime?>("ReleaseDate");                         
			set => SetPropertyValue<DateTime?>("ReleaseDate", value); 
			
        }
		//Tooltip for Object
		public object ReleaseDateToolTipControllerText(View view)
        {
        //    if (ReleaseDate != null) 
		//			return ReleaseDate;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultReleaseDate(View view = null)
        { 
			return ReleaseDate;
        }
		//Set Default Value
		public void SetDefaultReleaseDate(View view = null)
        {
            //if (ReleaseDate is null){
            //    var result = GetDefaultReleaseDate(view);
            //    if (result != null && result != ReleaseDate){
			//          ReleaseDate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ReleaseDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultReleaseDate();
				//if (result != null && ReleaseDate != null){
				//	return !ReleaseDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.ProductType _type;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(39)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.ProductType Type
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ProductType>("Type");                         
			set => SetPropertyValue<Module.BusinessObjects.ProductType>("Type", value); 
			
        }
		//Tooltip for Object
		public object TypeToolTipControllerText(View view)
        {
        //    if (Type != null) 
		//			return Type;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ProductType GetDefaultType(View view = null)
        { 
			return Type;
        }
		//Set Default Value
		public void SetDefaultType(View view = null)
        {
            //if (Type is null){
            //    var result = GetDefaultType(view);
            //    if (result != null && result != Type){
			//          Type = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultType();
				//if (result != null && Type != null){
				//	return !Type.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Type));
            }
        }
	
       
		//private Module.BusinessObjects.ProductTypeVariation _producttypevariation;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Biến thể loại SP")]
        [ToolTip("Biến thể loại SP")]
		//[Index(40)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductTypeVariationCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.ProductTypeVariation ProductTypeVariation
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ProductTypeVariation>("ProductTypeVariation");                         
			set => SetPropertyValue<Module.BusinessObjects.ProductTypeVariation>("ProductTypeVariation", value); 
			
        }
		//Tooltip for Object
		public object ProductTypeVariationToolTipControllerText(View view)
        {
        //    if (ProductTypeVariation != null) 
		//			return ProductTypeVariation;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ProductTypeVariation GetDefaultProductTypeVariation(View view = null)
        { 
			return ProductTypeVariation;
        }
		//Set Default Value
		public void SetDefaultProductTypeVariation(View view = null)
        {
            //if (ProductTypeVariation is null){
            //    var result = GetDefaultProductTypeVariation(view);
            //    if (result != null && result != ProductTypeVariation){
			//          ProductTypeVariation = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ProductTypeVariationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultProductTypeVariation();
				//if (result != null && ProductTypeVariation != null){
				//	return !ProductTypeVariation.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ProductTypeVariationCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ProductTypeVariation));
            }
        }
	
       
		//private Module.BusinessObjects.Product _productparent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sản phẩm")]
        [ToolTip("Sản phẩm")]
		//[Index(41)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductParentCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("ProductParent-ProductList")]
	 
		public Module.BusinessObjects.Product ProductParent
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Product>("ProductParent");                         
			set => SetPropertyValue<Module.BusinessObjects.Product>("ProductParent", value); 
			
        }
		//Tooltip for Object
		public object ProductParentToolTipControllerText(View view)
        {
        //    if (ProductParent != null) 
		//			return ProductParent;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Product GetDefaultProductParent(View view = null)
        { 
			return ProductParent;
        }
		//Set Default Value
		public void SetDefaultProductParent(View view = null)
        {
            //if (ProductParent is null){
            //    var result = GetDefaultProductParent(view);
            //    if (result != null && result != ProductParent){
			//          ProductParent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ProductParentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultProductParent();
				//if (result != null && ProductParent != null){
				//	return !ProductParent.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ProductParentCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ProductParent));
            }
        }
	
       
		//private string _priceweb;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá web")]
        [ToolTip("Giá web")]
		//[Index(42)]		

 		[Size(100)]
		public string PriceWeb
        { 
		    get => GetPropertyValue<string>("PriceWeb");                         
			set => SetPropertyValue<string>("PriceWeb", value); 
			
        }
		//Tooltip for Object
		public object PriceWebToolTipControllerText(View view)
        {
        //    if (PriceWeb != null) 
		//			return PriceWeb;
            return null;
        }
		//Get Default Value
        public string GetDefaultPriceWeb(View view = null)
        { 
			return PriceWeb;
        }
		//Set Default Value
		public void SetDefaultPriceWeb(View view = null)
        {
            //if (PriceWeb is null){
            //    var result = GetDefaultPriceWeb(view);
            //    if (result != null && result != PriceWeb){
			//          PriceWeb = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PriceWebIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPriceWeb();
				//if (result != null && PriceWeb != null){
				//	return !PriceWeb.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(43)]		
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
 
            #region 0391ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
SetDefaultMember();
SetDefaultCreatedDate();
SetDefaultPermissionPolicyRole();
            #endregion 0391ImportCode
            Display = true;
 
        //SetDefaultName(View view = null);
        //SetDefaultBrand(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultPrice(View view = null);
        //SetDefaultPriceSale(View view = null);
        //SetDefaultPermissionPolicyRole(View view = null);
        //SetDefaultVariation(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultCreatedDate(View view = null);
        //SetDefaultOrg(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultEnglish(View view = null);
        //SetDefaultCountryOrigin(View view = null);
        //SetDefaultWarranty(View view = null);
        //SetDefaultRound(View view = null);
        //SetDefaultEAN(View view = null);
        //SetDefaultCurrency(View view = null);
        //SetDefaultHsCode(View view = null);
        //SetDefaultCustomsDescription(View view = null);
        //SetDefaultSize(View view = null);
        //SetDefaultWeight(View view = null);
        //SetDefaultWeightConverted(View view = null);
        //SetDefaultInActive(View view = null);
        //SetDefaultReleaseDate(View view = null);
        //SetDefaultType(View view = null);
        //SetDefaultProductTypeVariation(View view = null);
        //SetDefaultProductParent(View view = null);
        //SetDefaultPriceWeb(View view = null);
        //SetDefaultOrder(View view = null);
			
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
            #region 0503ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 0503ImportCode
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
				
                    case nameof(Brand):
                        OnChangedBrand(oldValue, newValue);
                        break;
				
                    case nameof(ProductParent):
                        OnChangedProductParent(oldValue, newValue);
                        break;
				
                    case nameof(Type):
                        OnChangedType(oldValue, newValue);
                        break;
				
                    case nameof(Name):
                        OnChangedName(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedBrand(object oldValue, object newValue)
        {
            #region 1274ImportCode
            if (newValue is null) return;
SetDefaultOrg();            
            #endregion 1274ImportCode
        }               
        private void OnChangedProductParent(object oldValue, object newValue)
        {
            #region 1356ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 1356ImportCode
        }               
        private void OnChangedType(object oldValue, object newValue)
        {
            #region 1268ImportCode
            if (newValue is null) return;
SetDefaultProductAttributeList();            
            #endregion 1268ImportCode
        }               
        private void OnChangedName(object oldValue, object newValue)
        {
            #region 1333ImportCode
            if (string.IsNullOrEmpty(Name))
    return;
 //Xử lý ký tự đặc biệt mã ASCII 160 giống dấu cách
 var newName = Name.Replace(" ", " "); 
if (newName != Name)
    Name = newName;            
            #endregion 1333ImportCode
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
			//	SetDefaultProductAttributeList();
			//	SetDefaultProductInterfaceList();
			//	SetDefaultBookMarkList();
			//	SetDefaultProductPriceList();
			//	SetDefaultDomainProductList();
			//	SetDefaultFeature();
			//	SetDefaultIntroduction();
			//	SetDefaultSpecification();
			//	SetDefaultManual();
			//	SetDefaultProductList();
			//	SetDefaultProductAttributeValueList();
			//	SetDefaultFolderList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1357ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 1357            Oid: 03342373-a30f-4f8f-aac3-d5ab16dc3cb8
            Order= GetDefaultOrder();
        }
#endregion 1357ImportCode
#region 3757ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 3757            Oid: ba8f0f23-4799-4d5b-b303-19e7ce9d8e32
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3757ImportCode
#region 3756ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 3756            Oid: 6712552f-a1d4-47bf-a74e-63e210891694
            Updater = GetDefaultUpdater();
        }
#endregion 3756ImportCode
#region 3762ImportCode
		public void SetDefaultCreatedDate(View view = null)
        {
            //Code: 3762            Oid: 350732ea-0b67-4092-bbb5-52234317978d
            if(CreatedDate == null) CreatedDate = GetDefaultCreatedDate();
        }
#endregion 3762ImportCode
#region 3765ImportCode
		public void SetDefaultPermissionPolicyRole(View view = null)
        {
            //Code: 3765            Oid: 0d9e18fc-898e-4b93-9181-28c08e7db654
            if(Member is not null)
PermissionPolicyRole = Member.MemberFolder.PermissionPolicyRole;
        }
#endregion 3765ImportCode
#region 1272ImportCode
		public Module.BusinessObjects.Org GetDefaultOrg(View view = null)
        {
            //Code: 1272            Oid: a9154544-7503-4cb5-8e90-0ef7284745f8
            //if(Brand != null)
if(!string.IsNullOrEmpty(Brand))
	return Session.FindObject<Org>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Code = ?", Brand));
return null;

        }
#endregion 1272ImportCode
#region 1273ImportCode
		public void SetDefaultOrg(View view = null)
        {
            //Code: 1273            Oid: 3face524-23a0-473e-be6a-36376ce06320
            Org = GetDefaultOrg();

        }
#endregion 1273ImportCode
#region 1355ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 1355            Oid: 95b991b5-aa9b-43a2-aa01-2eafe602053d
            if (Folder != null && Folder.GroupProductList != null)
{
    var lasted = Folder.GroupProductList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
return null;
        }
#endregion 1355ImportCode
#region 2517ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 2517            Oid: dfa01d84-33e9-4c25-a63f-8edc3c78cf7b
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 2517ImportCode
#region 2518ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 2518            Oid: ebce587f-ca02-4040-8007-0bcab81bfe01
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2518ImportCode
#region 0156ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0156            Oid: 6025b938-e351-4bdb-8d1b-eb2a3a992e64
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0156ImportCode
#region 3761ImportCode
		public DateTime? GetDefaultCreatedDate(View view = null)
        {
            //Code: 3761            Oid: 0eefb60d-d179-4069-a280-3f3b131ebf42
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 3761ImportCode
#region 0175ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0175            Oid: 30233bbc-bcd6-469e-94ce-c4b8f2a64de6
            Update = GetDefaultUpdate();
        }
#endregion 0175ImportCode
        #endregion
//Mã nguồn bổ sung
#region ProductImportCode
public void SetDefaultProductAttributeList(bool importValue = false)
{
    if (Type is null)
        return;
    foreach (var productTypeAttribute in Type.ProductTypeAttributeList)
    {
        var productAttribute = ProductAttributeList.FirstOrDefault(m => m.ProductTypeAttribute.Equals(productTypeAttribute));
        if (productAttribute != null)
        {
            //Bổ sung thêm giá trị
        }
        else
        {
            //Tạo giá trị mới
            productAttribute = new Module.BusinessObjects.ProductAttribute(Session);
            ProductAttributeList.Add(productAttribute);
            productAttribute.ProductTypeAttribute = productTypeAttribute;

        }
        if (importValue)
            productAttribute.ProductAttributeValue.AddRange(productTypeAttribute.ProductAttributeValueList);
    }
}
#endregion ProductImportCode
		 		 
    }
}
