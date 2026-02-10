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
    [ModelDefault("Caption", "Hệ thống tích hợp"), ImageName("IntegrationSystem")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("IntegrationSystem SystemApplicationList Hide_None__" , TargetItems = "SystemApplicationList" , Criteria = "[SystemApplicationList][].Count() = 0",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(Update)+ "," + nameof(Updater))]
 
	[MobileColumnAttribute(Context = "IntegrationSystem_LookupListView", TargetItems = nameof(Member)+ "," + nameof(Folder)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_IntegrationSystemList_ListView", TargetItems = nameof(Name)+ "," + nameof(Member))]
	[MobileColumnAttribute(Context = "IntegrationSystem_ListView", TargetItems = nameof(Name)+ "," + nameof(Member)+ "," + nameof(Folder))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class IntegrationSystem:  DevExpress.Xpo.XPLiteObject , INewObjectSession , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public IntegrationSystem(Session session)
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
				if (EquipmentList.IsLoaded)
                {
                    if (EquipmentList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(EquipmentList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(EquipmentList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Equipment>(CriteriaOperator.Parse("[IntegrationSystem.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool equipmentlist = Session.Query<Module.BusinessObjects.Equipment>().Where(x => x.IntegrationSystem.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(EquipmentList), equipmentlist);
                        if (equipmentlist)
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
                        //if (Session.FindObject<Module.BusinessObjects.SystemApplication>(CriteriaOperator.Parse("[IntegrationSystem.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool systemapplicationlist = Session.Query<Module.BusinessObjects.SystemApplication>().Where(x => x.IntegrationSystem.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(SystemApplicationList), systemapplicationlist);
                        if (systemapplicationlist)
                            return true;

                    }                    
                }				
				if (PhysicalConnectionList.IsLoaded)
                {
                    if (PhysicalConnectionList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(PhysicalConnectionList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(PhysicalConnectionList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.PhysicalConnection>(CriteriaOperator.Parse("[IntegrationSystem.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool physicalconnectionlist = Session.Query<Module.BusinessObjects.PhysicalConnection>().Where(x => x.IntegrationSystem.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(PhysicalConnectionList), physicalconnectionlist);
                        if (physicalconnectionlist)
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

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(1)]		
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
	
       
		//private Module.BusinessObjects.MemberFolder _memberfolder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(2)]		
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
	
       
		//private Module.BusinessObjects.Folder _folder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#Asset# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-IntegrationSystemList")]
	 
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
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thiết bị")]
		//[Index(4)]
		[DevExpress.Xpo.Association("IntegrationSystem-EquipmentList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Equipment> EquipmentList
        {      
		    get => GetCollection<Module.BusinessObjects.Equipment>("EquipmentList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ứng dụng")]
		//[Index(5)]
		[DevExpress.Xpo.Association("IntegrationSystem-SystemApplicationList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.SystemApplication> SystemApplicationList
        {      
		    get => GetCollection<Module.BusinessObjects.SystemApplication>("SystemApplicationList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kết nối")]
		//[Index(6)]
		[DevExpress.Xpo.Association("IntegrationSystem-PhysicalConnectionList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.PhysicalConnection> PhysicalConnectionList
        {      
		    get => GetCollection<Module.BusinessObjects.PhysicalConnection>("PhysicalConnectionList"); 
			
        }
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(7)]		
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
		//[Index(8)]		
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
 
            #region 2538ImportCode
            base.AfterConstruction();
SetDefaultMember();
            #endregion 2538ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultFolder(View view = null);
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
            #region 0352ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 0352ImportCode
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
			//	SetDefaultEquipmentList();
			//	SetDefaultSystemApplicationList();
			//	SetDefaultPhysicalConnectionList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 2537ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 2537            Oid: 01ef7cfd-8109-4b98-8243-5462dc92149c
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 2537ImportCode
#region 4487ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 4487            Oid: 614190b6-c8bf-4bde-8836-94323aee0513
            Updater = GetDefaultUpdater();
        }
#endregion 4487ImportCode
#region 4488ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 4488            Oid: 15eb0a46-1530-4e79-a709-0ca010dad07c
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 4488ImportCode
#region 2539ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 2539            Oid: 812addc1-22a6-43c6-8ee4-0988efde5385
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2539ImportCode
#region 0216ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0216            Oid: 25725a08-6fc4-4d80-8bb2-6741a13f3c45
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0216ImportCode
#region 0218ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0218            Oid: 11fe6c6b-e571-46f0-93de-40170e77fded
            Update = GetDefaultUpdate();
        }
#endregion 0218ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
