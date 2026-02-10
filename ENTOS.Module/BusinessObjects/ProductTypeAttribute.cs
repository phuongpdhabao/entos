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
    [ModelDefault("Caption", "Thuộc tính loại sản phẩm"), ImageName("ProductTypeAttribute")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Order))]
 
	[MobileColumnAttribute(Context = "ProductType_ProductTypeAttributeList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ProductTypeAttribute_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ProductTypeAttribute_LookupListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class ProductTypeAttribute:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ProductTypeAttribute(Session session)
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
				if (ProductAttributeValueList.IsLoaded)
                {
                    if (ProductAttributeValueList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ProductAttributeValueList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ProductAttributeValueList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ProductAttributeValue>(CriteriaOperator.Parse("[ProductTypeAttribute.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool productattributevaluelist = Session.Query<Module.BusinessObjects.ProductAttributeValue>().Where(x => x.ProductTypeAttribute.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ProductAttributeValueList), productattributevaluelist);
                        if (productattributevaluelist)
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

	
       
		//private string _english;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên TA")]
        [ToolTip("Tên TA")]
		//[Index(1)]		

 		[Size(150)]
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
        public bool GetDefaultDisplay(View view = null)
        { 
			return Display;
        }
		//Set Default Value
		public void SetDefaultDisplay(View view = null)
        {
            //if (Display is null){
            //    var result = GetDefaultDisplay(view);
            //    if (result != null && result != Display){
			//          Display = result;
            //	  }
            //}
        }

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

	
       
		//private bool _variation;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Biến thể")]
        [ToolTip("Biến thể")]
		//[Index(3)]		
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
		[DevExpress.Xpo.DisplayName("Giá trị")]
		//[Index(4)]
		[DevExpress.Xpo.Association("ProductTypeAttribute-ProductAttributeValueList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductAttributeValue> ProductAttributeValueList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductAttributeValue>("ProductAttributeValueList"); 
			
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
		[DevExpress.Xpo.Association("ProductType-ProductTypeAttributeList")]
	 
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
        //SetDefaultEnglish(View view = null);
        //SetDefaultDisplay(View view = null);
        //SetDefaultVariation(View view = null);
        //SetDefaultProductType(View view = null);
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
				
                    case nameof(ProductType):
                        OnChangedProductType(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedProductType(object oldValue, object newValue)
        {
            #region 1253ImportCode
            base.AfterConstruction();
SetDefaultOrder();            
            #endregion 1253ImportCode
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
			//	SetDefaultProductAttributeValueList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1252ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 1252            Oid: 88f556fc-1035-495e-b8f2-c0cda523da3a
            Order= GetDefaultOrder();
        }
#endregion 1252ImportCode
#region 1251ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 1251            Oid: 0d69a1d9-00a0-4427-9da9-a4225ae74210
            if (ProductType != null && ProductType.ProductTypeAttributeList != null)
{
    var lasted = ProductType.ProductTypeAttributeList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
//var parentMember = type.GetProperty("ProductType");
//if(parentMember != null)
//{
//    var parentObjectObject = folderMember.GetValue(this);
//    if (parentObjectObject != null)
//    {                    
//        var list = parentObjectObject.GetPropertyValue("ProductTypeAttributeList") as XPCollection<{TypeName}>;
//        var lasted = list.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
//        if (lasted != null)
//            return lasted.Order + 1;
//        return 1;
//    }
//}
return null;
        }
#endregion 1251ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
