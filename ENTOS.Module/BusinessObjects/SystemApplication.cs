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
    [ModelDefault("Caption", "Ứng dụng hệ thống"), ImageName("SystemApplication")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(Order)+ "," + nameof(Member))]
 
	[MobileColumnAttribute(Context = "Asset_SystemApplicationList_ListView", TargetItems = nameof(Name)+ "," + nameof(Equipment))]
	[MobileColumnAttribute(Context = "SystemApplication_LookupListView", TargetItems = nameof(Name)+ "," + nameof(Equipment))]
	[MobileColumnAttribute(Context = "SystemApplication_ListView", TargetItems = nameof(Equipment)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "IntegrationSystem_SystemApplicationList_ListView", TargetItems = nameof(Equipment)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Equipment_SystemApplicationList_ListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class SystemApplication:  DevExpress.Xpo.XPLiteObject , IReOrder , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public SystemApplication(Session session)
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

	
       
		//private int? _quantity;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(1)]		
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

	
       
		//private string _link;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
        [ToolTip("Liên kết")]
		//[Index(2)]		

 		[Size(250)]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string Link
        { 
		    get => GetPropertyValue<string>("Link");                         
			set => SetPropertyValue<string>("Link", value); 
			
        }
		//Tooltip for Object
		public object LinkToolTipControllerText(View view)
        {
        //    if (Link != null) 
		//			return Link;
            return null;
        }
		//Get Default Value
        public string GetDefaultLink(View view = null)
        { 
			return Link;
        }
		//Set Default Value
		public void SetDefaultLink(View view = null)
        {
            //if (Link is null){
            //    var result = GetDefaultLink(view);
            //    if (result != null && result != Link){
			//          Link = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LinkIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLink();
				//if (result != null && Link != null){
				//	return !Link.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.MemberFolder _memberfolder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MemberFolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.MemberFolder MemberFolder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.MemberFolder>("MemberFolder");                         
			set => SetPropertyValue<Module.BusinessObjects.MemberFolder>("MemberFolder", value); 
			
        }
		//Tooltip for Object
		public object MemberFolderToolTipControllerText(View view)
        {
        //    if (MemberFolder != null) 
		//			return MemberFolder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.MemberFolder GetDefaultMemberFolder(View view = null)
        { 
			return MemberFolder;
        }
		//Set Default Value
		public void SetDefaultMemberFolder(View view = null)
        {
            //if (MemberFolder is null){
            //    var result = GetDefaultMemberFolder(view);
            //    if (result != null && result != MemberFolder){
			//          MemberFolder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MemberFolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMemberFolder();
				//if (result != null && MemberFolder != null){
				//	return !MemberFolder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator MemberFolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(MemberFolder));
            }
        }
	
       
		//private Module.BusinessObjects.Asset _asset;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài sản")]
        [ToolTip("Tài sản")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(AssetCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Asset-SystemApplicationList")]
	 
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
	
       
		//private Module.BusinessObjects.Equipment _equipment;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thiết bị")]
        [ToolTip("Thiết bị")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(EquipmentCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Equipment-SystemApplicationList")]
	 
		public Module.BusinessObjects.Equipment Equipment
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Equipment>("Equipment");                         
			set => SetPropertyValue<Module.BusinessObjects.Equipment>("Equipment", value); 
			
        }
		//Tooltip for Object
		public object EquipmentToolTipControllerText(View view)
        {
        //    if (Equipment != null) 
		//			return Equipment;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Equipment GetDefaultEquipment(View view = null)
        { 
			return Equipment;
        }
		//Set Default Value
		public void SetDefaultEquipment(View view = null)
        {
            //if (Equipment is null){
            //    var result = GetDefaultEquipment(view);
            //    if (result != null && result != Equipment){
			//          Equipment = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EquipmentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEquipment();
				//if (result != null && Equipment != null){
				//	return !Equipment.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator EquipmentCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Equipment));
            }
        }
	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(6)]		
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

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(7)]		
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

	
       
		//private Module.BusinessObjects.Member _member;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(8)]		
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
	
       
		//private Module.BusinessObjects.IntegrationSystem _integrationsystem;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hệ thống")]
        [ToolTip("Hệ thống")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(IntegrationSystemCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("IntegrationSystem-SystemApplicationList")]
	 
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
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 2551ImportCode
            base.AfterConstruction();
SetDefaultMember();
Quantity = 1;
            #endregion 2551ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultLink(View view = null);
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultAsset(View view = null);
        //SetDefaultEquipment(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultIntegrationSystem(View view = null);
			
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
            #region 0433ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0433ImportCode
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

                switch (propertyName)
                {       
				
                    case nameof(IntegrationSystem):
                        OnChangedIntegrationSystem(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedIntegrationSystem(object oldValue, object newValue)
        {
            #region 2549ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 2549ImportCode
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
#region 2550ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 2550            Oid: bc675067-6359-4d02-adcf-e8e9753b6bd7
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 2550ImportCode
#region 2547ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 2547            Oid: 339b35c4-4167-4902-8c1b-b94ef923e218
            if (IntegrationSystem != null && IntegrationSystem.SystemApplicationList != null)
{
    var lasted = IntegrationSystem.SystemApplicationList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
return null;
        }
#endregion 2547ImportCode
#region 0220ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0220            Oid: 083716cc-8c83-4b05-ba98-630fbb72032d
            Update = GetDefaultUpdate();
        }
#endregion 0220ImportCode
#region 2548ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 2548            Oid: 6dfda8bd-30df-40a0-bca4-8a5779055655
            Order= GetDefaultOrder();
        }
#endregion 2548ImportCode
#region 0219ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0219            Oid: b5e4affe-a7c2-4d68-9400-c9cd709e50b0
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0219ImportCode
#region 2552ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 2552            Oid: 56b3e1c5-adac-4bf1-b526-4d6954e0a626
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2552ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
