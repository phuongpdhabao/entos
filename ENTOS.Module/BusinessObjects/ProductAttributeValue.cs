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
    [ModelDefault("Caption", "Giá trị thuộc tính SP"), ImageName("ProductAttributeValue")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Order))]
 
	[MobileColumnAttribute(Context = "ProductTypeAttribute_ProductAttributeValueList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ProductAttributeValue_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ProductAttributeValue_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Product_ProductAttributeValueList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ProductAttribute_ProductAttributeValue_ListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class ProductAttributeValue:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ProductAttributeValue(Session session)
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

	
       
		//private decimal? _numbervalue;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá trị số")]
        [ToolTip("Giá trị số")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? NumberValue
        { 
		    get => GetPropertyValue<decimal?>("NumberValue");                         
			set => SetPropertyValue<decimal?>("NumberValue", value); 
			
        }
		//Tooltip for Object
		public object NumberValueToolTipControllerText(View view)
        {
        //    if (NumberValue != null) 
		//			return NumberValue;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultNumberValue(View view = null)
        { 
			return NumberValue;
        }
		//Set Default Value
		public void SetDefaultNumberValue(View view = null)
        {
            //if (NumberValue is null){
            //    var result = GetDefaultNumberValue(view);
            //    if (result != null && result != NumberValue){
			//          NumberValue = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NumberValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNumberValue();
				//if (result != null && NumberValue != null){
				//	return !NumberValue.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool? _logicvalue;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá trị logic")]
        [ToolTip("Giá trị logic")]
		//[Index(2)]		
		public bool? LogicValue
        { 
		    get => GetPropertyValue<bool?>("LogicValue");                         
			set => SetPropertyValue<bool?>("LogicValue", value); 
			
        }
		//Tooltip for Object
		public object LogicValueToolTipControllerText(View view)
        {
        //    if (LogicValue != null) 
		//			return LogicValue;
            return null;
        }
		//Get Default Value
        public bool? GetDefaultLogicValue(View view = null)
        { 
			return LogicValue;
        }
		//Set Default Value
		public void SetDefaultLogicValue(View view = null)
        {
            //if (LogicValue is null){
            //    var result = GetDefaultLogicValue(view);
            //    if (result != null && result != LogicValue){
			//          LogicValue = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LogicValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLogicValue();
				//if (result != null && LogicValue != null){
				//	return !LogicValue.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuộc tính sản phẩm")]
		//[Index(3)]
		[DataSourceCriteria("Not ProductAttributeValue[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("ProductAttributeList-ProductAttributeValue")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductAttribute> ProductAttributeList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductAttribute>("ProductAttributeList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sản phẩm")]
		//[Index(4)]
		[DataSourceCriteria("Not ProductAttributeValueList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("ProductAttributeValueList-ProductList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Product> ProductList
        {      
		    get => GetCollection<Module.BusinessObjects.Product>("ProductList"); 
			
        }
       
		//private Module.BusinessObjects.ProductTypeAttribute _producttypeattribute;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuộc tính loại sản phẩm")]
        [ToolTip("Thuộc tính loại sản phẩm")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductTypeAttributeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("ProductTypeAttribute-ProductAttributeValueList")]
	 
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
	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultNumberValue(View view = null);
        //SetDefaultLogicValue(View view = null);
        //SetDefaultProductTypeAttribute(View view = null);
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
            #region 1256ImportCode
            base.AfterConstruction();
SetDefaultOrder();            
            #endregion 1256ImportCode
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
			//	SetDefaultProductList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1255ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 1255            Oid: e16e3bb8-fc7f-4d81-9fab-dac275322442
            Order= GetDefaultOrder();
        }
#endregion 1255ImportCode
#region 1254ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 1254            Oid: 15a141fb-4b8f-4a4b-a823-268978c2b566
            if (ProductTypeAttribute != null && ProductTypeAttribute.ProductAttributeValueList != null)
{
    var lasted = ProductTypeAttribute.ProductAttributeValueList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
//var parentMember = type.GetProperty("ProductTypeAttribute");
//if(parentMember != null)
//{
//    var parentObjectObject = folderMember.GetValue(this);
//    if (parentObjectObject != null)
//    {                    
//        var list = parentObjectObject.GetPropertyValue("ProductAttributeValueList") as XPCollection<{TypeName}>;
//        var lasted = list.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
//        if (lasted != null)
//            return lasted.Order + 1;
//        return 1;
//    }
//}
return null;
        }
#endregion 1254ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
