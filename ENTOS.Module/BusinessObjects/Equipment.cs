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
    [ModelDefault("Caption", "Thiết bị"), ImageName("Equipment")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("Equipment SystemApplicationList Hide_None__" , TargetItems = "SystemApplicationList" , Criteria = "[App] = False",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Equipment EndPhysicalConnectionList, StartPhysicalConnectionList Hide_None__" , TargetItems = "EndPhysicalConnectionList, StartPhysicalConnectionList" , Criteria = "[IntegrationSystem] Is Null",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(Update)+ "," + nameof(Updater)+ "," + nameof(Order))]
 
	[MobileColumnAttribute(Context = "IntegrationSystem_EquipmentList_ListView", TargetItems = nameof(Photo)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_EquipmentList_ListView", TargetItems = nameof(Photo)+ "," + nameof(Name)+ "," + nameof(IntegrationSystem))]
	[MobileColumnAttribute(Context = "Equipment_ListView", TargetItems = nameof(Name)+ "," + nameof(Photo)+ "," + nameof(IntegrationSystem))]
	[MobileColumnAttribute(Context = "Asset_EquipmentList_ListView", TargetItems = nameof(Name)+ "," + nameof(IntegrationSystem)+ "," + nameof(Photo))]
	[MobileColumnAttribute(Context = "Equipment_LookupListView", TargetItems = nameof(IntegrationSystem)+ "," + nameof(Name)+ "," + nameof(Photo))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Equipment:  DevExpress.Xpo.XPLiteObject , INewObjectSession, IReOrder , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Equipment(Session session)
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
				if (BookMarkList.IsLoaded)
                {
                    if (BookMarkList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(BookMarkList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(BookMarkList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.BookMark>(CriteriaOperator.Parse("[Equipment.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool bookmarklist = Session.Query<Module.BusinessObjects.BookMark>().Where(x => x.Equipment.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(BookMarkList), bookmarklist);
                        if (bookmarklist)
                            return true;

                    }                    
                }				
				if (StartPhysicalConnectionList.IsLoaded)
                {
                    if (StartPhysicalConnectionList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(StartPhysicalConnectionList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(StartPhysicalConnectionList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.PhysicalConnection>(CriteriaOperator.Parse("[EndDevice.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool startphysicalconnectionlist = Session.Query<Module.BusinessObjects.PhysicalConnection>().Where(x => x.EndDevice.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(StartPhysicalConnectionList), startphysicalconnectionlist);
                        if (startphysicalconnectionlist)
                            return true;

                    }                    
                }				
				if (EndPhysicalConnectionList.IsLoaded)
                {
                    if (EndPhysicalConnectionList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(EndPhysicalConnectionList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(EndPhysicalConnectionList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.PhysicalConnection>(CriteriaOperator.Parse("[StartDevice.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool endphysicalconnectionlist = Session.Query<Module.BusinessObjects.PhysicalConnection>().Where(x => x.StartDevice.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(EndPhysicalConnectionList), endphysicalconnectionlist);
                        if (endphysicalconnectionlist)
                            return true;

                    }                    
                }				
				if (SystemApplicationList.IsLoaded)
                {
                    if (SystemApplicationList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(SystemApplicationList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(SystemApplicationList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.SystemApplication>(CriteriaOperator.Parse("[Equipment.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool systemapplicationlist = Session.Query<Module.BusinessObjects.SystemApplication>().Where(x => x.Equipment.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(SystemApplicationList), systemapplicationlist);
                        if (systemapplicationlist)
                            return true;

                    }                    
                }				
				if (CalendarList.IsLoaded)
                {
                    if (CalendarList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(CalendarList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(CalendarList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Calendar>(CriteriaOperator.Parse("[Equipment.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool calendarlist = Session.Query<Module.BusinessObjects.Calendar>().Where(x => x.Equipment.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(CalendarList), calendarlist);
                        if (calendarlist)
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

	
       
		//private Module.BusinessObjects.Asset _asset;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài sản")]
        [ToolTip("Tài sản")]
		//[Index(1)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(AssetCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Asset-EquipmentList")]
	 
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
	
       
		//private Module.BusinessObjects.IntegrationSystem _integrationsystem;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Hệ thống")]
        [ToolTip("Hệ thống")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(IntegrationSystemCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("IntegrationSystem-EquipmentList")]
	 
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
	
       
		//private Module.BusinessObjects.Folder _folder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#Asset#")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-EquipmentList")]
	 
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
	
       
		//private byte[] _photo;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ảnh")]
        [ToolTip("Ảnh")]
		//[Index(4)]		
		[Appearance("ẢnhBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Photo
        { 
		    get => GetPropertyValue<byte[]>("Photo");                         
			set => SetPropertyValue<byte[]>("Photo", value); 
			
        }
		//Tooltip for Object
		public object PhotoToolTipControllerText(View view)
        {
        //    if (Photo != null) 
		//			return Photo;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultPhoto(View view = null)
        { 
			return Photo;
        }
		//Set Default Value
		public void SetDefaultPhoto(View view = null)
        {
            //if (Photo is null){
            //    var result = GetDefaultPhoto(view);
            //    if (result != null && result != Photo){
			//          Photo = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PhotoIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPhoto();
				//if (result != null && Photo != null){
				//	return !Photo.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(5)]		
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

	
       
		//private bool _app;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Có ứng dụng")]
        [ToolTip("Có ứng dụng")]
		//[Index(7)]		
		public bool App
        { 
		    get => GetPropertyValue<bool>("App");                         
			set => SetPropertyValue<bool>("App", value); 
			
        }
		//Tooltip for Object
		public object AppToolTipControllerText(View view)
        {
        //    if (App != null) 
		//			return App;
            return null;
        }
		//Get Default Value
        public bool GetDefaultApp(View view = null)
        { 
			return App;
        }
		//Set Default Value
		public void SetDefaultApp(View view = null)
        {
            //if (App is null){
            //    var result = GetDefaultApp(view);
            //    if (result != null && result != App){
			//          App = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AppIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultApp();
				//if (result != null && App != null){
				//	return !App.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
		//[Index(8)]
		[DevExpress.Xpo.Association("Equipment-BookMarkList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BookMark> BookMarkList
        {      
		    get => GetCollection<Module.BusinessObjects.BookMark>("BookMarkList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nối đầu")]
		//[Index(9)]
		[DevExpress.Xpo.Association("EndDevice-StartPhysicalConnectionList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.PhysicalConnection> StartPhysicalConnectionList
        {      
		    get => GetCollection<Module.BusinessObjects.PhysicalConnection>("StartPhysicalConnectionList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nối cuối")]
		//[Index(10)]
		[DevExpress.Xpo.Association("StartDevice-EndPhysicalConnectionList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.PhysicalConnection> EndPhysicalConnectionList
        {      
		    get => GetCollection<Module.BusinessObjects.PhysicalConnection>("EndPhysicalConnectionList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ứng dụng")]
		//[Index(11)]
		[DevExpress.Xpo.Association("Equipment-SystemApplicationList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.SystemApplication> SystemApplicationList
        {      
		    get => GetCollection<Module.BusinessObjects.SystemApplication>("SystemApplicationList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lịch")]
		//[Index(12)]
		[DevExpress.Xpo.Association("Equipment-CalendarList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Calendar> CalendarList
        {      
		    get => GetCollection<Module.BusinessObjects.Calendar>("CalendarList"); 
			
        }
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(13)]		
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

	
       
		//private Module.BusinessObjects.Member _updater;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Người cập nhật")]
        [ToolTip("Người cập nhật")]
		//[Index(14)]		
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
	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(15)]		
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
 
            #region 2533ImportCode
            base.AfterConstruction();
SetDefaultMember();
Quantity = 1;
            #endregion 2533ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultAsset(View view = null);
        //SetDefaultIntegrationSystem(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultPhoto(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultApp(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
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
            #region 2541ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 2541ImportCode
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
            #region 2546ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 2546ImportCode
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
			//	SetDefaultBookMarkList();
			//	SetDefaultStartPhysicalConnectionList();
			//	SetDefaultEndPhysicalConnectionList();
			//	SetDefaultSystemApplicationList();
			//	SetDefaultCalendarList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 2544ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 2544            Oid: bdc62d89-68e4-49ed-8e33-7bbcada7adbe
            if (IntegrationSystem != null && IntegrationSystem.EquipmentList != null)
{
    var lasted = IntegrationSystem.EquipmentList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
return null;
        }
#endregion 2544ImportCode
#region 2542ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 2542            Oid: 02d3a074-da15-41b7-9a0b-c55f2b171588
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 2542ImportCode
#region 2540ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 2540            Oid: 2600ca26-cf44-49b7-9a10-782529c1e8c0
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 2540ImportCode
#region 2532ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 2532            Oid: 5b638100-25b4-4c7d-992b-663ff4d9e113
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 2532ImportCode
#region 2545ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 2545            Oid: 723fe356-f9a3-46c7-9589-34aa8e224ed6
            Order= GetDefaultOrder();
        }
#endregion 2545ImportCode
#region 4490ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 4490            Oid: fc26a59a-7a36-491f-8a94-6fb201f62b11
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 4490ImportCode
#region 2534ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 2534            Oid: 0f04045e-819e-4b17-96b4-6afef01547b4
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2534ImportCode
#region 4489ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 4489            Oid: 21d72a6b-259d-475c-be80-75cd185d7a84
            Updater = GetDefaultUpdater();
        }
#endregion 4489ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
