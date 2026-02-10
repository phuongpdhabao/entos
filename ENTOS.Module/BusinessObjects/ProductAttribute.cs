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
    [ModelDefault("Caption", "Thuộc tính sản phẩm"), ImageName("ProductAttribute")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Variation)+ "," + nameof(Display))]
 
	[MobileColumnAttribute(Context = "ProductAttribute_ListView", TargetItems = nameof(Product)+ "," + nameof(ProductTypeAttribute))]
	[MobileColumnAttribute(Context = "Product_ProductAttributeList_ListView", TargetItems = nameof(ProductTypeAttribute)+ "," + "ProductTypeAttribute.English"+ "," + "ProductTypeAttribute.Order")]
	[MobileColumnAttribute(Context = "ProductAttributeValue_ProductAttributeList_ListView", TargetItems = nameof(ProductTypeAttribute))]
	[MobileColumnAttribute(Context = "ProductAttribute_LookupListView", TargetItems = nameof(ProductTypeAttribute))]
 
[OptimisticLocking(true)]
    public partial class ProductAttribute:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ProductAttribute(Session session)
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
               

		//private Module.BusinessObjects.ProductTypeAttribute _producttypeattribute;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thuộc tính loại sản phẩm")]
        [ToolTip("Thuộc tính loại sản phẩm")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductTypeAttributeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.ProductTypeAttribute ProductTypeAttribute
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ProductTypeAttribute>("ProductTypeAttribute");                         
			set => SetPropertyValue<Module.BusinessObjects.ProductTypeAttribute>("ProductTypeAttribute", value); 
			
        }
		//Tooltip for Object
		public object ProductTypeAttributeToolTipControllerText(View view)
        {
        //    if (ProductTypeAttribute != null) 
		//			return ProductTypeAttribute;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ProductTypeAttribute GetDefaultProductTypeAttribute(View view = null)
        { 
			return ProductTypeAttribute;
        }
		//Set Default Value
		public void SetDefaultProductTypeAttribute(View view = null)
        {
            //if (ProductTypeAttribute is null){
            //    var result = GetDefaultProductTypeAttribute(view);
            //    if (result != null && result != ProductTypeAttribute){
			//          ProductTypeAttribute = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ProductTypeAttributeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultProductTypeAttribute();
				//if (result != null && ProductTypeAttribute != null){
				//	return !ProductTypeAttribute.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ProductTypeAttributeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ProductTypeAttribute));
            }
        }
	
       
		//private bool _variation;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Biển thể")]
        [ToolTip("Biển thể")]
		//[Index(1)]		
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
		//Set Default Value

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

	
       
		//private bool _display;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hiển thị")]
        [ToolTip("Hiển thị")]
		//[Index(2)]		
		public bool Display
        { 
		    get => GetPropertyValue<bool>("Display");                         
			set => SetPropertyValue<bool>("Display", value); 
			
        }
		//Tooltip for Object
		public object DisplayToolTipControllerText(View view)
        {
        //    if (Display != null) 
		//			return Display;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool DisplayIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDisplay();
				//if (result != null && Display != null){
				//	return !Display.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá trị")]
		//[Index(3)]
		[DataSourceCriteria("Not ProductAttributeList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("ProductAttributeList-ProductAttributeValue")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductAttributeValue> ProductAttributeValue
        {      
		    get => GetCollection<Module.BusinessObjects.ProductAttributeValue>("ProductAttributeValue"); 
			
        }
       
		//private Module.BusinessObjects.Product _product;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sản phẩm")]
        [ToolTip("Sản phẩm")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Product-ProductAttributeList")]
	 
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
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultProductTypeAttribute(View view = null);
        //SetDefaultVariation(View view = null);
        //SetDefaultDisplay(View view = null);
        //SetDefaultProduct(View view = null);
			
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

                switch (propertyName)
                {       
				
                    case nameof(ProductTypeAttribute):
                        OnChangedProductTypeAttribute(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedProductTypeAttribute(object oldValue, object newValue)
        {
            #region 1267ImportCode
            if (newValue is null) return;
SetDefaultVariation();
SetDefaultDisplay();            
            #endregion 1267ImportCode
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
			//	SetDefaultProductAttributeValue();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1264ImportCode
		public void SetDefaultVariation(View view = null)
        {
            //Code: 1264            Oid: ead1db1f-a0d3-45f4-85e5-c3614f633ade
            Variation = GetDefaultVariation();

        }
#endregion 1264ImportCode
#region 1263ImportCode
		public bool GetDefaultVariation(View view = null)
        {
            //Code: 1263            Oid: 7e12fe1e-e151-42da-b261-b8c7e3dff63f
            if (ProductTypeAttribute != null)
    return ProductTypeAttribute.Variation;
return false;

        }
#endregion 1263ImportCode
#region 1266ImportCode
		public void SetDefaultDisplay(View view = null)
        {
            //Code: 1266            Oid: b2ace46f-8958-4c5e-8efa-801708b0c258
            Display = GetDefaultDisplay();
        }
#endregion 1266ImportCode
#region 1265ImportCode
		public bool GetDefaultDisplay(View view = null)
        {
            //Code: 1265            Oid: aa65f4c1-4019-441e-a06e-a44f11001585
            if (ProductTypeAttribute != null)
    return ProductTypeAttribute.Display;
return false;
        }
#endregion 1265ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
