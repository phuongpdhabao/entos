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
    [ModelDefault("Caption", "Hàng hóa"), ImageName("ProductItem")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "ProductItem_ListView", TargetItems = nameof(Quantity)+ "," + nameof(SerialNumber)+ "," + nameof(SourceProductItem))]
	[DefaultProperty("SerialNumber")]
 
[OptimisticLocking(true)]
    public partial class ProductItem:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ProductItem(Session session)
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
				if (DestinationProductItemList.IsLoaded)
                {
                    if (DestinationProductItemList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(DestinationProductItemList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(DestinationProductItemList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ProductItem>(CriteriaOperator.Parse("[SourceProductItem.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool destinationproductitemlist = Session.Query<Module.BusinessObjects.ProductItem>().Where(x => x.SourceProductItem.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(DestinationProductItemList), destinationproductitemlist);
                        if (destinationproductitemlist)
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
               

		//private string _serialnumber;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số Serial")]
        [ToolTip("Số Serial")]
		//[Index(0)]		

 		[Size(60)]
		public string SerialNumber
        { 
		    get => GetPropertyValue<string>("SerialNumber");                         
			set => SetPropertyValue<string>("SerialNumber", value); 
			
        }
		//Tooltip for Object
		public object SerialNumberToolTipControllerText(View view)
        {
        //    if (SerialNumber != null) 
		//			return SerialNumber;
            return null;
        }
		//Get Default Value
        public string GetDefaultSerialNumber(View view = null)
        { 
			return SerialNumber;
        }
		//Set Default Value
		public void SetDefaultSerialNumber(View view = null)
        {
            //if (SerialNumber is null){
            //    var result = GetDefaultSerialNumber(view);
            //    if (result != null && result != SerialNumber){
			//          SerialNumber = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SerialNumberIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSerialNumber();
				//if (result != null && SerialNumber != null){
				//	return !SerialNumber.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _quantity;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(1)]		
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

	
       
		//private decimal? _remainquantity1;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hiệu dụng 1")]
        [ToolTip("Hiệu dụng 1")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? RemainQuantity1
        { 
		    get => GetPropertyValue<decimal?>("RemainQuantity1");                         
			set => SetPropertyValue<decimal?>("RemainQuantity1", value); 
			
        }
		//Tooltip for Object
		public object RemainQuantity1ToolTipControllerText(View view)
        {
        //    if (RemainQuantity1 != null) 
		//			return RemainQuantity1;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultRemainQuantity1(View view = null)
        { 
			return RemainQuantity1;
        }
		//Set Default Value
		public void SetDefaultRemainQuantity1(View view = null)
        {
            //if (RemainQuantity1 is null){
            //    var result = GetDefaultRemainQuantity1(view);
            //    if (result != null && result != RemainQuantity1){
			//          RemainQuantity1 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RemainQuantity1IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRemainQuantity1();
				//if (result != null && RemainQuantity1 != null){
				//	return !RemainQuantity1.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _remainquantity2;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hiệu dụng 2")]
        [ToolTip("Hiệu dụng 2")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? RemainQuantity2
        { 
		    get => GetPropertyValue<decimal?>("RemainQuantity2");                         
			set => SetPropertyValue<decimal?>("RemainQuantity2", value); 
			
        }
		//Tooltip for Object
		public object RemainQuantity2ToolTipControllerText(View view)
        {
        //    if (RemainQuantity2 != null) 
		//			return RemainQuantity2;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultRemainQuantity2(View view = null)
        { 
			return RemainQuantity2;
        }
		//Set Default Value
		public void SetDefaultRemainQuantity2(View view = null)
        {
            //if (RemainQuantity2 is null){
            //    var result = GetDefaultRemainQuantity2(view);
            //    if (result != null && result != RemainQuantity2){
			//          RemainQuantity2 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RemainQuantity2IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRemainQuantity2();
				//if (result != null && RemainQuantity2 != null){
				//	return !RemainQuantity2.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.ProductItem _sourceproductitem;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nguồn")]
        [ToolTip("Nguồn")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SourceProductItemCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("SourceProductItem-DestinationProductItemList")]
	 
		public Module.BusinessObjects.ProductItem SourceProductItem
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ProductItem>("SourceProductItem");                         
			set => SetPropertyValue<Module.BusinessObjects.ProductItem>("SourceProductItem", value); 
			
        }
		//Tooltip for Object
		public object SourceProductItemToolTipControllerText(View view)
        {
        //    if (SourceProductItem != null) 
		//			return SourceProductItem;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ProductItem GetDefaultSourceProductItem(View view = null)
        { 
			return SourceProductItem;
        }
		//Set Default Value
		public void SetDefaultSourceProductItem(View view = null)
        {
            //if (SourceProductItem is null){
            //    var result = GetDefaultSourceProductItem(view);
            //    if (result != null && result != SourceProductItem){
			//          SourceProductItem = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SourceProductItemIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSourceProductItem();
				//if (result != null && SourceProductItem != null){
				//	return !SourceProductItem.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SourceProductItemCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SourceProductItem));
            }
        }
	
       
		//private InOutType _itemstatus;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trạng thái")]
        [ToolTip("Trạng thái")]
		//[Index(6)]		
		public InOutType ItemStatus
        { 
		    get => GetPropertyValue<InOutType>("ItemStatus");                         
			set => SetPropertyValue<InOutType>("ItemStatus", value); 
			
        }
		//Tooltip for Object
		public object ItemStatusToolTipControllerText(View view)
        {
        //    if (ItemStatus != null) 
		//			return ItemStatus;
            return null;
        }
		//Get Default Value
        public InOutType GetDefaultItemStatus(View view = null)
        { 
			return ItemStatus;
        }
		//Set Default Value
		public void SetDefaultItemStatus(View view = null)
        {
            //if (ItemStatus is null){
            //    var result = GetDefaultItemStatus(view);
            //    if (result != null && result != ItemStatus){
			//          ItemStatus = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ItemStatusIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultItemStatus();
				//if (result != null && ItemStatus != null){
				//	return !ItemStatus.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _unitcost;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đơn giá")]
        [ToolTip("Đơn giá")]
		//[Index(7)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? UnitCost
        { 
		    get => GetPropertyValue<decimal?>("UnitCost");                         
			set => SetPropertyValue<decimal?>("UnitCost", value); 
			
        }
		//Tooltip for Object
		public object UnitCostToolTipControllerText(View view)
        {
        //    if (UnitCost != null) 
		//			return UnitCost;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultUnitCost(View view = null)
        { 
			return UnitCost;
        }
		//Set Default Value
		public void SetDefaultUnitCost(View view = null)
        {
            //if (UnitCost is null){
            //    var result = GetDefaultUnitCost(view);
            //    if (result != null && result != UnitCost){
			//          UnitCost = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UnitCostIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUnitCost();
				//if (result != null && UnitCost != null){
				//	return !UnitCost.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _book1;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi sổ 1")]
        [ToolTip("Ghi sổ 1")]
		//[Index(8)]		
		public bool Book1
        { 
		    get => GetPropertyValue<bool>("Book1");                         
			set => SetPropertyValue<bool>("Book1", value); 
			
        }
		//Tooltip for Object
		public object Book1ToolTipControllerText(View view)
        {
        //    if (Book1 != null) 
		//			return Book1;
            return null;
        }
		//Get Default Value
        public bool GetDefaultBook1(View view = null)
        { 
			return Book1;
        }
		//Set Default Value
		public void SetDefaultBook1(View view = null)
        {
            //if (Book1 is null){
            //    var result = GetDefaultBook1(view);
            //    if (result != null && result != Book1){
			//          Book1 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Book1IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBook1();
				//if (result != null && Book1 != null){
				//	return !Book1.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _book2;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi sổ 2")]
        [ToolTip("Ghi sổ 2")]
		//[Index(9)]		
		public bool Book2
        { 
		    get => GetPropertyValue<bool>("Book2");                         
			set => SetPropertyValue<bool>("Book2", value); 
			
        }
		//Tooltip for Object
		public object Book2ToolTipControllerText(View view)
        {
        //    if (Book2 != null) 
		//			return Book2;
            return null;
        }
		//Get Default Value
        public bool GetDefaultBook2(View view = null)
        { 
			return Book2;
        }
		//Set Default Value
		public void SetDefaultBook2(View view = null)
        {
            //if (Book2 is null){
            //    var result = GetDefaultBook2(view);
            //    if (result != null && result != Book2){
			//          Book2 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Book2IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBook2();
				//if (result != null && Book2 != null){
				//	return !Book2.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đích")]
		//[Index(10)]
		[DevExpress.Xpo.Association("SourceProductItem-DestinationProductItemList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductItem> DestinationProductItemList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductItem>("DestinationProductItemList"); 
			
        }
       
		//private Module.BusinessObjects.OrderDetail _orderdetail;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chi tiết đơn hàng")]
        [ToolTip("Chi tiết đơn hàng")]
		//[Index(12)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OrderDetailCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("OrderDetail-ProductItemList")]
	 
		public Module.BusinessObjects.OrderDetail OrderDetail
        { 
		    get => GetPropertyValue<Module.BusinessObjects.OrderDetail>("OrderDetail");                         
			set => SetPropertyValue<Module.BusinessObjects.OrderDetail>("OrderDetail", value); 
			
        }
		//Tooltip for Object
		public object OrderDetailToolTipControllerText(View view)
        {
        //    if (OrderDetail != null) 
		//			return OrderDetail;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.OrderDetail GetDefaultOrderDetail(View view = null)
        { 
			return OrderDetail;
        }
		//Set Default Value
		public void SetDefaultOrderDetail(View view = null)
        {
            //if (OrderDetail is null){
            //    var result = GetDefaultOrderDetail(view);
            //    if (result != null && result != OrderDetail){
			//          OrderDetail = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OrderDetailIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrderDetail();
				//if (result != null && OrderDetail != null){
				//	return !OrderDetail.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator OrderDetailCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(OrderDetail));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultSerialNumber(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultRemainQuantity1(View view = null);
        //SetDefaultRemainQuantity2(View view = null);
        //SetDefaultSourceProductItem(View view = null);
        //SetDefaultItemStatus(View view = null);
        //SetDefaultUnitCost(View view = null);
        //SetDefaultBook1(View view = null);
        //SetDefaultBook2(View view = null);
        //SetDefaultOrderDetail(View view = null);
			
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
			//	SetDefaultDestinationProductItemList();
			//	SetDefaultDepreciations();
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
