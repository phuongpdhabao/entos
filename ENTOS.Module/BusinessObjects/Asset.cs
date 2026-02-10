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
    [ModelDefault("Caption", "Tài sản"), ImageName("Asset")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("Asset SystemApplicationList Hide_None__" , TargetItems = "SystemApplicationList" , Criteria = "[SystemApplicationList][].Count() = 0",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Asset EquipmentList Hide_None__" , TargetItems = "EquipmentList" , Criteria = "[EquipmentList][].Count() = 0",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Code)+ "," + nameof(Member)+ "," + nameof(MemberFolder)+ "," + nameof(Update)+ "," + nameof(Updater))]
 
	[MobileColumnAttribute(Context = "Asset_LookupListView", TargetItems = nameof(Value)+ "," + nameof(Photo)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Asset_ListView", TargetItems = nameof(Value)+ "," + nameof(Photo)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_AssetList_ListView", TargetItems = nameof(Value)+ "," + nameof(Name)+ "," + nameof(Photo))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Asset:  DevExpress.Xpo.XPLiteObject , INewObjectSession , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Asset(Session session)
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
                        //if (Session.FindObject<Module.BusinessObjects.BookMark>(CriteriaOperator.Parse("[Asset.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool bookmarklist = Session.Query<Module.BusinessObjects.BookMark>().Where(x => x.Asset.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(BookMarkList), bookmarklist);
                        if (bookmarklist)
                            return true;

                    }                    
                }				
				if (AccountEntryList.IsLoaded)
                {
                    if (AccountEntryList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(AccountEntryList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(AccountEntryList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.AccountEntry>(CriteriaOperator.Parse("[Asset.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool accountentrylist = Session.Query<Module.BusinessObjects.AccountEntry>().Where(x => x.Asset.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(AccountEntryList), accountentrylist);
                        if (accountentrylist)
                            return true;

                    }                    
                }				
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
                        //if (Session.FindObject<Module.BusinessObjects.Equipment>(CriteriaOperator.Parse("[Asset.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool equipmentlist = Session.Query<Module.BusinessObjects.Equipment>().Where(x => x.Asset.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
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
                        //if (Session.FindObject<Module.BusinessObjects.SystemApplication>(CriteriaOperator.Parse("[Asset.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool systemapplicationlist = Session.Query<Module.BusinessObjects.SystemApplication>().Where(x => x.Asset.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
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
                        //if (Session.FindObject<Module.BusinessObjects.Calendar>(CriteriaOperator.Parse("[Asset.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool calendarlist = Session.Query<Module.BusinessObjects.Calendar>().Where(x => x.Asset.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
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
               

		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(0)]		

 		[Size(20)]
		[RuleUniqueValue("UniqueAssetCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredAssetCode", DefaultContexts.Save)]
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
		//Set Default Value

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

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(1)]		

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

	
       
		//private decimal? _value;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Nguyên giá")]
        [ToolTip("Nguyên giá")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Value
        { 
		    get => GetPropertyValue<decimal?>("Value");                         
			set => SetPropertyValue<decimal?>("Value", value); 
			
        }
		//Tooltip for Object
		public object ValueToolTipControllerText(View view)
        {
        //    if (Value != null) 
		//			return Value;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultValue(View view = null)
        { 
			return Value;
        }
		//Set Default Value
		public void SetDefaultValue(View view = null)
        {
            //if (Value is null){
            //    var result = GetDefaultValue(view);
            //    if (result != null && result != Value){
			//          Value = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultValue();
				//if (result != null && Value != null){
				//	return !Value.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _startdate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Từ")]
        [ToolTip("Từ")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? StartDate
        { 
		    get => GetPropertyValue<DateTime?>("StartDate");                         
			set => SetPropertyValue<DateTime?>("StartDate", value); 
			
        }
		//Tooltip for Object
		public object StartDateToolTipControllerText(View view)
        {
        //    if (StartDate != null) 
		//			return StartDate;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultStartDate(View view = null)
        { 
			return StartDate;
        }
		//Set Default Value
		public void SetDefaultStartDate(View view = null)
        {
            //if (StartDate is null){
            //    var result = GetDefaultStartDate(view);
            //    if (result != null && result != StartDate){
			//          StartDate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StartDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStartDate();
				//if (result != null && StartDate != null){
				//	return !StartDate.Equals(result);
				//} 
   
                return false;
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

	
       
		//private Module.BusinessObjects.Folder _folder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#Asset# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-AssetList")]
	 
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
	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(6)]		
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
	
       
		//private Module.BusinessObjects.Folder _memberfolder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#Member# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Folder MemberFolder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Folder>("MemberFolder");                         
			set => SetPropertyValue<Module.BusinessObjects.Folder>("MemberFolder", value); 
			
        }
		//Tooltip for Object
		public object MemberFolderToolTipControllerText(View view)
        {
        //    if (MemberFolder != null) 
		//			return MemberFolder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Folder GetDefaultMemberFolder(View view = null)
        { 
			return MemberFolder;
        }
		//Set Default Value

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
	
       
		//private bool _inactive;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(8)]		
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

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
		//[Index(9)]
		[DevExpress.Xpo.Association("Asset-BookMarkList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BookMark> BookMarkList
        {      
		    get => GetCollection<Module.BusinessObjects.BookMark>("BookMarkList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bút toán")]
		//[Index(10)]
		[DevExpress.Xpo.Association("Asset-AccountEntryList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.AccountEntry> AccountEntryList
        {      
		    get => GetCollection<Module.BusinessObjects.AccountEntry>("AccountEntryList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thiết bị")]
		//[Index(11)]
		[DevExpress.Xpo.Association("Asset-EquipmentList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Equipment> EquipmentList
        {      
		    get => GetCollection<Module.BusinessObjects.Equipment>("EquipmentList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ứng dụng")]
		//[Index(12)]
		[DevExpress.Xpo.Association("Asset-SystemApplicationList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.SystemApplication> SystemApplicationList
        {      
		    get => GetCollection<Module.BusinessObjects.SystemApplication>("SystemApplicationList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lịch")]
		//[Index(13)]
		[DevExpress.Xpo.Association("Asset-CalendarList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Calendar> CalendarList
        {      
		    get => GetCollection<Module.BusinessObjects.Calendar>("CalendarList"); 
			
        }
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(14)]		
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
		//[Index(15)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpdaterCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
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
 
            #region 0347ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
SetDefaultCode();
SetDefaultMember();
            #endregion 0347ImportCode
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultValue(View view = null);
        //SetDefaultStartDate(View view = null);
        //SetDefaultPhoto(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultInActive(View view = null);
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
            #region 0438ImportCode
            base.OnSaving();
SetDefaultUpdater();
            #endregion 0438ImportCode
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
			//	SetDefaultBookMarkList();
			//	SetDefaultAccountEntryList();
			//	SetDefaultEquipmentList();
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
#region 0037ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0037            Oid: 58b37330-8866-4552-b41f-a47e6ea7aa30
            Update = GetDefaultUpdate();
        }
#endregion 0037ImportCode
#region 0259ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 0259            Oid: 64aa80db-224e-42f8-bbc0-b1952893ef0a
            Updater = GetDefaultUpdater();

        }
#endregion 0259ImportCode
#region 2815ImportCode
		public Module.BusinessObjects.Folder SetDefaultMemberFolder(View view = null)
        {
            //Code: 2815            Oid: 47fecaf9-c6af-4d6e-b2aa-377d0fa42feb
            return null;
        }
#endregion 2815ImportCode
#region 0260ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 0260            Oid: 557fba5e-13e5-4671-88fc-62b7a9216fbf
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 0260ImportCode
#region 0122ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0122            Oid: 85d1b108-c956-4dda-8865-9b6e707d64d6
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0122ImportCode
#region 0261ImportCode
		public string GetDefaultCode(View view = null)
        {
            //Code: 0261            Oid: aa09b40f-372c-4eca-8061-7344e54bba8a
            var keyCodeObject =
    Module.Helpers.ParameterHelper.GetSettingParameter(Session, "CodeObject");
if (keyCodeObject != null)
{
    //Kích thước mặc định là 3 số
    int size = 3;		
    return Tools.GetCode(this.GetType(), this.Session, this.Oid, keyCodeObject.Value, size ,
        " ");
}
return null;
        }
#endregion 0261ImportCode
#region 2814ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 2814            Oid: fcb11f97-1c95-4cec-a868-defef438330a
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2814ImportCode
#region 0262ImportCode
		public void SetDefaultCode(View view = null)
        {
            //Code: 0262            Oid: 69b8d804-488e-4547-9172-dae85bafcbac
            if(String.IsNullOrEmpty(Code)) Code = GetDefaultCode();
        }
#endregion 0262ImportCode
#region 2813ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 2813            Oid: 4a64c100-c245-4cfd-a48f-08a5b92ec21d
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 2813ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
