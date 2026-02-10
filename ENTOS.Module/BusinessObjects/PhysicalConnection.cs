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
	[NavigationItem("Asset")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Kết nối vật lý"), ImageName("PhysicalConnection")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(Updater))]
 
	[MobileColumnAttribute(Context = "PhysicalConnection_LookupListView", TargetItems = nameof(StartDevice)+ "," + nameof(EndDevice))]
	[MobileColumnAttribute(Context = "IntegrationSystem_PhysicalConnectionList_ListView", TargetItems = nameof(EndDevice)+ "," + nameof(StartDevice))]
	[MobileColumnAttribute(Context = "PhysicalConnection_ListView", TargetItems = nameof(EndDevice)+ "," + nameof(StartDevice))]
 
[OptimisticLocking(true)]
    public partial class PhysicalConnection:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public PhysicalConnection(Session session)
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
               

		//private Module.BusinessObjects.Equipment _startdevice;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thiết bị đầu")]
        [ToolTip("Thiết bị đầu")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(StartDeviceCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("StartDevice-EndPhysicalConnectionList")]
	 
		public Module.BusinessObjects.Equipment StartDevice
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Equipment>("StartDevice");                         
			set => SetPropertyValue<Module.BusinessObjects.Equipment>("StartDevice", value); 
			
        }
		//Tooltip for Object
		public object StartDeviceToolTipControllerText(View view)
        {
        //    if (StartDevice != null) 
		//			return StartDevice;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Equipment GetDefaultStartDevice(View view = null)
        { 
			return StartDevice;
        }
		//Set Default Value
		public void SetDefaultStartDevice(View view = null)
        {
            //if (StartDevice is null){
            //    var result = GetDefaultStartDevice(view);
            //    if (result != null && result != StartDevice){
			//          StartDevice = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StartDeviceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStartDevice();
				//if (result != null && StartDevice != null){
				//	return !StartDevice.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator StartDeviceCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(StartDevice));
            }
        }
	
       
		//private string _startport;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cổng đầu")]
        [ToolTip("Cổng đầu")]
		//[Index(1)]		

 		[Size(250)]
		public string StartPort
        { 
		    get => GetPropertyValue<string>("StartPort");                         
			set => SetPropertyValue<string>("StartPort", value); 
			
        }
		//Tooltip for Object
		public object StartPortToolTipControllerText(View view)
        {
        //    if (StartPort != null) 
		//			return StartPort;
            return null;
        }
		//Get Default Value
        public string GetDefaultStartPort(View view = null)
        { 
			return StartPort;
        }
		//Set Default Value
		public void SetDefaultStartPort(View view = null)
        {
            //if (StartPort is null){
            //    var result = GetDefaultStartPort(view);
            //    if (result != null && result != StartPort){
			//          StartPort = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StartPortIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStartPort();
				//if (result != null && StartPort != null){
				//	return !StartPort.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Equipment _enddevice;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thiết bị cuôi")]
        [ToolTip("Thiết bị cuôi")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(EndDeviceCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("EndDevice-StartPhysicalConnectionList")]
	 
		public Module.BusinessObjects.Equipment EndDevice
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Equipment>("EndDevice");                         
			set => SetPropertyValue<Module.BusinessObjects.Equipment>("EndDevice", value); 
			
        }
		//Tooltip for Object
		public object EndDeviceToolTipControllerText(View view)
        {
        //    if (EndDevice != null) 
		//			return EndDevice;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Equipment GetDefaultEndDevice(View view = null)
        { 
			return EndDevice;
        }
		//Set Default Value
		public void SetDefaultEndDevice(View view = null)
        {
            //if (EndDevice is null){
            //    var result = GetDefaultEndDevice(view);
            //    if (result != null && result != EndDevice){
			//          EndDevice = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EndDeviceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEndDevice();
				//if (result != null && EndDevice != null){
				//	return !EndDevice.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator EndDeviceCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(EndDevice));
            }
        }
	
       
		//private string _endport;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cổng cuối")]
        [ToolTip("Cổng cuối")]
		//[Index(3)]		

 		[Size(250)]
		public string EndPort
        { 
		    get => GetPropertyValue<string>("EndPort");                         
			set => SetPropertyValue<string>("EndPort", value); 
			
        }
		//Tooltip for Object
		public object EndPortToolTipControllerText(View view)
        {
        //    if (EndPort != null) 
		//			return EndPort;
            return null;
        }
		//Get Default Value
        public string GetDefaultEndPort(View view = null)
        { 
			return EndPort;
        }
		//Set Default Value
		public void SetDefaultEndPort(View view = null)
        {
            //if (EndPort is null){
            //    var result = GetDefaultEndPort(view);
            //    if (result != null && result != EndPort){
			//          EndPort = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EndPortIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEndPort();
				//if (result != null && EndPort != null){
				//	return !EndPort.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _cabletype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại cáp")]
        [ToolTip("Loại cáp")]
		//[Index(4)]		

 		[Size(250)]
		public string CableType
        { 
		    get => GetPropertyValue<string>("CableType");                         
			set => SetPropertyValue<string>("CableType", value); 
			
        }
		//Tooltip for Object
		public object CableTypeToolTipControllerText(View view)
        {
        //    if (CableType != null) 
		//			return CableType;
            return null;
        }
		//Get Default Value
        public string GetDefaultCableType(View view = null)
        { 
			return CableType;
        }
		//Set Default Value
		public void SetDefaultCableType(View view = null)
        {
            //if (CableType is null){
            //    var result = GetDefaultCableType(view);
            //    if (result != null && result != CableType){
			//          CableType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CableTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCableType();
				//if (result != null && CableType != null){
				//	return !CableType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _length;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Độ dài")]
        [ToolTip("Độ dài")]
		//[Index(5)]		
		[ModelDefault("EditMask", "n2")]
		public int? Length
        { 
		    get => GetPropertyValue<int?>("Length");                         
			set => SetPropertyValue<int?>("Length", value); 
			
        }
		//Tooltip for Object
		public object LengthToolTipControllerText(View view)
        {
        //    if (Length != null) 
		//			return Length;
            return null;
        }
		//Get Default Value
        public int? GetDefaultLength(View view = null)
        { 
			return Length;
        }
		//Set Default Value
		public void SetDefaultLength(View view = null)
        {
            //if (Length is null){
            //    var result = GetDefaultLength(view);
            //    if (result != null && result != Length){
			//          Length = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LengthIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLength();
				//if (result != null && Length != null){
				//	return !Length.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _quantity;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Quantity
        { 
		    get => GetPropertyValue<int?>("Quantity");                         
			set => SetPropertyValue<int?>("Quantity", value); 
			
        }
		//Tooltip for Object
		public object QuantityToolTipControllerText(View view)
        {
        //    if (Quantity != null) 
		//			return Quantity;
            return null;
        }
		//Get Default Value
        public int? GetDefaultQuantity(View view = null)
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

	
       
		//private bool _inactive;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(7)]		
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

	
       
		//private Module.BusinessObjects.IntegrationSystem _integrationsystem;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hệ thống tích hợp")]
        [ToolTip("Hệ thống tích hợp")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(IntegrationSystemCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("IntegrationSystem-PhysicalConnectionList")]
	 
		public Module.BusinessObjects.IntegrationSystem IntegrationSystem
        { 
		    get => GetPropertyValue<Module.BusinessObjects.IntegrationSystem>("IntegrationSystem");                         
			set => SetPropertyValue<Module.BusinessObjects.IntegrationSystem>("IntegrationSystem", value); 
			
        }
		//Tooltip for Object
		public object IntegrationSystemToolTipControllerText(View view)
        {
        //    if (IntegrationSystem != null) 
		//			return IntegrationSystem;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.IntegrationSystem GetDefaultIntegrationSystem(View view = null)
        { 
			return IntegrationSystem;
        }
		//Set Default Value
		public void SetDefaultIntegrationSystem(View view = null)
        {
            //if (IntegrationSystem is null){
            //    var result = GetDefaultIntegrationSystem(view);
            //    if (result != null && result != IntegrationSystem){
			//          IntegrationSystem = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool IntegrationSystemIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultIntegrationSystem();
				//if (result != null && IntegrationSystem != null){
				//	return !IntegrationSystem.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator IntegrationSystemCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(IntegrationSystem));
            }
        }
	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(9)]		
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
		//[Index(10)]		
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
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultStartDevice(View view = null);
        //SetDefaultStartPort(View view = null);
        //SetDefaultEndDevice(View view = null);
        //SetDefaultEndPort(View view = null);
        //SetDefaultCableType(View view = null);
        //SetDefaultLength(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultInActive(View view = null);
        //SetDefaultIntegrationSystem(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
			
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
            #region 0426ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 0426ImportCode
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
#region 4485ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 4485            Oid: d8d75748-ca6a-4fdb-9d9d-09383036b2ed
            Updater = GetDefaultUpdater();
        }
#endregion 4485ImportCode
#region 0224ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0224            Oid: 9c0d9fff-9638-46af-863d-80e22382749d
            Update = GetDefaultUpdate();
        }
#endregion 0224ImportCode
#region 4486ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 4486            Oid: 8dbf5584-7ba8-4014-9b55-39b9c78df522
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 4486ImportCode
#region 0225ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0225            Oid: 9c1e69ef-6c21-44d2-bb7f-86d0953acc12
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0225ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
