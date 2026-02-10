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
    [ModelDefault("Caption", "Giao diện sản phẩm"), ImageName("ProductInterface")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update))]
 
	[MobileColumnAttribute(Context = "ProductInterface_LookupListView", TargetItems = nameof(Name)+ "," + nameof(ConnectorType))]
	[MobileColumnAttribute(Context = "Product_ProductInterfaceList_ListView", TargetItems = nameof(ConnectorType)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "ProductType_ProductInterfaceList_ListView", TargetItems = nameof(ConnectorType)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "ProductInterface_ListView", TargetItems = nameof(ConnectorType)+ "," + nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class ProductInterface:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ProductInterface(Session session)
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

	
       
		//private int _quantity;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int Quantity
        { 
		    get => GetPropertyValue<int>("Quantity");                         
			set => SetPropertyValue<int>("Quantity", value); 
			
        }
		//Tooltip for Object
		public object QuantityToolTipControllerText(View view)
        {
        //    if (Quantity != null) 
		//			return Quantity;
            return null;
        }
		//Get Default Value
        public int GetDefaultQuantity(View view = null)
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

	
       
		//private Module.BusinessObjects.ProductInterfaceType _productinterfacetype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductInterfaceTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.ProductInterfaceType ProductInterfaceType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ProductInterfaceType>("ProductInterfaceType");                         
			set => SetPropertyValue<Module.BusinessObjects.ProductInterfaceType>("ProductInterfaceType", value); 
			
        }
		//Tooltip for Object
		public object ProductInterfaceTypeToolTipControllerText(View view)
        {
        //    if (ProductInterfaceType != null) 
		//			return ProductInterfaceType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ProductInterfaceType GetDefaultProductInterfaceType(View view = null)
        { 
			return ProductInterfaceType;
        }
		//Set Default Value
		public void SetDefaultProductInterfaceType(View view = null)
        {
            //if (ProductInterfaceType is null){
            //    var result = GetDefaultProductInterfaceType(view);
            //    if (result != null && result != ProductInterfaceType){
			//          ProductInterfaceType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ProductInterfaceTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultProductInterfaceType();
				//if (result != null && ProductInterfaceType != null){
				//	return !ProductInterfaceType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ProductInterfaceTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ProductInterfaceType));
            }
        }
	
       
		//private ConnectorType _connectortype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại giắc")]
        [ToolTip("Loại giắc")]
		//[Index(3)]		
		public ConnectorType ConnectorType
        { 
		    get => GetPropertyValue<ConnectorType>("ConnectorType");                         
			set => SetPropertyValue<ConnectorType>("ConnectorType", value); 
			
        }
		//Tooltip for Object
		public object ConnectorTypeToolTipControllerText(View view)
        {
        //    if (ConnectorType != null) 
		//			return ConnectorType;
            return null;
        }
		//Get Default Value
        public ConnectorType GetDefaultConnectorType(View view = null)
        { 
			return ConnectorType;
        }
		//Set Default Value
		public void SetDefaultConnectorType(View view = null)
        {
            //if (ConnectorType is null){
            //    var result = GetDefaultConnectorType(view);
            //    if (result != null && result != ConnectorType){
			//          ConnectorType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ConnectorTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultConnectorType();
				//if (result != null && ConnectorType != null){
				//	return !ConnectorType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(4)]		
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

	
       
		//private Module.BusinessObjects.ProductType _producttype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại sản phẩm")]
        [ToolTip("Loại sản phẩm")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("ProductType-ProductInterfaceList")]
	 
		public Module.BusinessObjects.ProductType ProductType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ProductType>("ProductType");                         
			set => SetPropertyValue<Module.BusinessObjects.ProductType>("ProductType", value); 
			
        }
		//Tooltip for Object
		public object ProductTypeToolTipControllerText(View view)
        {
        //    if (ProductType != null) 
		//			return ProductType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ProductType GetDefaultProductType(View view = null)
        { 
			return ProductType;
        }
		//Set Default Value
		public void SetDefaultProductType(View view = null)
        {
            //if (ProductType is null){
            //    var result = GetDefaultProductType(view);
            //    if (result != null && result != ProductType){
			//          ProductType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ProductTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultProductType();
				//if (result != null && ProductType != null){
				//	return !ProductType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ProductTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ProductType));
            }
        }
	
       
		//private Module.BusinessObjects.Product _product;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sản phẩm")]
        [ToolTip("Sản phẩm")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Product-ProductInterfaceList")]
	 
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
 
        //SetDefaultName(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultProductInterfaceType(View view = null);
        //SetDefaultConnectorType(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultProductType(View view = null);
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
            #region 0388ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0388ImportCode
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
#region 0222ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0222            Oid: ffc5ae20-9d83-4f10-bac3-c172c7afdef9
            Update = GetDefaultUpdate();
        }
#endregion 0222ImportCode
#region 0221ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0221            Oid: 6dbd0ba2-46ff-47bc-903f-4068e0e3dcfe
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0221ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
