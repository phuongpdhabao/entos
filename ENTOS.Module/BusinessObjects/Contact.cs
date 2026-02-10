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
	[NavigationItem("Communication")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Liên hệ"), ImageName("Contact")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("Contact ChildContactList Show_None__" , TargetItems = "ChildContactList" , Criteria = "[ChildContactList][].Count() > 0",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Show )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(Name)+ "," + nameof(WorkPlace)+ "," + nameof(NativePlace)+ "," + nameof(DeathDay)+ "," + nameof(SpouseContact)+ "," + nameof(ParentContact)+ "," + nameof(ChildContactList)+ "," + nameof(PaymentAccountList), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
    [ShowToolTipAttribute(TargetItems = nameof(DeathDay))]
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Org)+ "," + nameof(Update)+ "," + nameof(Member))]
 
	[MobileColumnAttribute(Context = "Org_ContactList_ListView", TargetItems = nameof(Name)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Contact_SpouseContactList_ListView", TargetItems = nameof(Image)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ContactList_ListView", TargetItems = nameof(Birthday)+ "," + nameof(Image)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Contact_ChildContactList_ListView", TargetItems = nameof(Name)+ "," + nameof(Image)+ "," + nameof(Birthday))]
	[MobileColumnAttribute(Context = "Contact_ListView", TargetItems = nameof(WorkPlace)+ "," + nameof(Name)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Contact_LookupListView", TargetItems = nameof(Image)+ "," + nameof(Name))]
	[DefaultProperty("Name")]
 
	
	[CustomFilter("IFilteringFolderInContact", "Folder.Oid = ?")]
	[RuleCombinationOfPropertiesIsUnique("UniqueRule.Contact", DefaultContexts.Save, "Name, WorkPlace")]
[OptimisticLocking(true)]
    public partial class Contact:  DevExpress.Xpo.XPLiteObject , INewObjectSession, DevExpress.Persistent.Base.General.ITreeNode, IWebData, IObjectImage ,IFilteringFolderInContact, INoIndexColumn, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public Contact(Session session)
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
				if (HistoryList.IsLoaded)
                {
                    if (HistoryList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(HistoryList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(HistoryList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.History>(CriteriaOperator.Parse("[Contact.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool historylist = Session.Query<Module.BusinessObjects.History>().Where(x => x.Contact.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(HistoryList), historylist);
                        if (historylist)
                            return true;

                    }                    
                }				
				if (ChildContactList.IsLoaded)
                {
                    if (ChildContactList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ChildContactList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ChildContactList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Contact>(CriteriaOperator.Parse("[ParentContact.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool childcontactlist = Session.Query<Module.BusinessObjects.Contact>().Where(x => x.ParentContact.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ChildContactList), childcontactlist);
                        if (childcontactlist)
                            return true;

                    }                    
                }				
				if (SpouseContactList.IsLoaded)
                {
                    if (SpouseContactList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(SpouseContactList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(SpouseContactList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Contact>(CriteriaOperator.Parse("[SpouseContact.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool spousecontactlist = Session.Query<Module.BusinessObjects.Contact>().Where(x => x.SpouseContact.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(SpouseContactList), spousecontactlist);
                        if (spousecontactlist)
                            return true;

                    }                    
                }				
				if (PaymentAccountList.IsLoaded)
                {
                    if (PaymentAccountList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(PaymentAccountList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(PaymentAccountList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.PaymentAccount>(CriteriaOperator.Parse("[Contact.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool paymentaccountlist = Session.Query<Module.BusinessObjects.PaymentAccount>().Where(x => x.Contact.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(PaymentAccountList), paymentaccountlist);
                        if (paymentaccountlist)
                            return true;

                    }                    
                }				
				if (PlayerList.IsLoaded)
                {
                    if (PlayerList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(PlayerList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(PlayerList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Player>(CriteriaOperator.Parse("[Contact.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool playerlist = Session.Query<Module.BusinessObjects.Player>().Where(x => x.Contact.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(PlayerList), playerlist);
                        if (playerlist)
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
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(0)]		

 		[Size(100)]
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
        public string GetDefaultCode(View view = null)
        { 
			return Code;
        }
		//Set Default Value
		public void SetDefaultCode(View view = null)
        {
            //if (Code is null){
            //    var result = GetDefaultCode(view);
            //    if (result != null && result != Code){
			//          Code = result;
            //	  }
            //}
        }

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

	
       
		//private string _fullname;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Họ tên")]
        [ToolTip("Họ tên")]
		//[Index(1)]		

 		[Size(150)]
		public string FullName
        { 
		    get => GetPropertyValue<string>("FullName");                         
			set => SetPropertyValue<string>("FullName", value); 
			
        }
		//Tooltip for Object
		public object FullNameToolTipControllerText(View view)
        {
        //    if (FullName != null) 
		//			return FullName;
            return null;
        }
		//Get Default Value
        public string GetDefaultFullName(View view = null)
        { 
			return FullName;
        }
		//Set Default Value
		public void SetDefaultFullName(View view = null)
        {
            //if (FullName is null){
            //    var result = GetDefaultFullName(view);
            //    if (result != null && result != FullName){
			//          FullName = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FullNameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFullName();
				//if (result != null && FullName != null){
				//	return !FullName.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(2)]		

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

	
       
		//private string _title;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Chức danh")]
        [ToolTip("Chức danh")]
		//[Index(3)]		

 		[Size(100)]
		public string Title
        { 
		    get => GetPropertyValue<string>("Title");                         
			set => SetPropertyValue<string>("Title", value); 
			
        }
		//Tooltip for Object
		public object TitleToolTipControllerText(View view)
        {
        //    if (Title != null) 
		//			return Title;
            return null;
        }
		//Get Default Value
        public string GetDefaultTitle(View view = null)
        { 
			return Title;
        }
		//Set Default Value
		public void SetDefaultTitle(View view = null)
        {
            //if (Title is null){
            //    var result = GetDefaultTitle(view);
            //    if (result != null && result != Title){
			//          Title = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TitleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTitle();
				//if (result != null && Title != null){
				//	return !Title.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _workplace;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Nơi làm")]
        [ToolTip("Nơi làm")]
		//[Index(4)]		

 		[Size(100)]
		public string WorkPlace
        { 
		    get => GetPropertyValue<string>("WorkPlace");                         
			set => SetPropertyValue<string>("WorkPlace", value); 
			
        }
		//Tooltip for Object
		public object WorkPlaceToolTipControllerText(View view)
        {
        //    if (WorkPlace != null) 
		//			return WorkPlace;
            return null;
        }
		//Get Default Value
        public string GetDefaultWorkPlace(View view = null)
        { 
			return WorkPlace;
        }
		//Set Default Value
		public void SetDefaultWorkPlace(View view = null)
        {
            //if (WorkPlace is null){
            //    var result = GetDefaultWorkPlace(view);
            //    if (result != null && result != WorkPlace){
			//          WorkPlace = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WorkPlaceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWorkPlace();
				//if (result != null && WorkPlace != null){
				//	return !WorkPlace.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _nativeplace;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quê quán")]
        [ToolTip("Quê quán")]
		//[Index(5)]		

 		[Size(100)]
		public string NativePlace
        { 
		    get => GetPropertyValue<string>("NativePlace");                         
			set => SetPropertyValue<string>("NativePlace", value); 
			
        }
		//Tooltip for Object
		public object NativePlaceToolTipControllerText(View view)
        {
        //    if (NativePlace != null) 
		//			return NativePlace;
            return null;
        }
		//Get Default Value
        public string GetDefaultNativePlace(View view = null)
        { 
			return NativePlace;
        }
		//Set Default Value
		public void SetDefaultNativePlace(View view = null)
        {
            //if (NativePlace is null){
            //    var result = GetDefaultNativePlace(view);
            //    if (result != null && result != NativePlace){
			//          NativePlace = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NativePlaceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNativePlace();
				//if (result != null && NativePlace != null){
				//	return !NativePlace.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ảnh")]
        [ToolTip("Ảnh")]
		//[Index(6)]		
		[Appearance("ẢnhBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Image
        { 
		    get => GetPropertyValue<byte[]>("Image");                         
			set => SetPropertyValue<byte[]>("Image", value); 
			
        }
		//Tooltip for Object
		public object ImageToolTipControllerText(View view)
        {
        //    if (Image != null) 
		//			return Image;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultImage(View view = null)
        { 
			return Image;
        }
		//Set Default Value
		public void SetDefaultImage(View view = null)
        {
            //if (Image is null){
            //    var result = GetDefaultImage(view);
            //    if (result != null && result != Image){
			//          Image = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ImageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImage();
				//if (result != null && Image != null){
				//	return !Image.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _birthday;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày sinh")]
        [ToolTip("Ngày sinh")]
		//[Index(7)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? Birthday
        { 
		    get => GetPropertyValue<DateTime?>("Birthday");                         
			set => SetPropertyValue<DateTime?>("Birthday", value); 
			
        }
		//Tooltip for Object
		public object BirthdayToolTipControllerText(View view)
        {
        //    if (Birthday != null) 
		//			return Birthday;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultBirthday(View view = null)
        { 
			return Birthday;
        }
		//Set Default Value
		public void SetDefaultBirthday(View view = null)
        {
            //if (Birthday is null){
            //    var result = GetDefaultBirthday(view);
            //    if (result != null && result != Birthday){
			//          Birthday = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BirthdayIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBirthday();
				//if (result != null && Birthday != null){
				//	return !Birthday.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _deathday;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày mất")]
        [ToolTip("Ngày mất")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? DeathDay
        { 
		    get => GetPropertyValue<DateTime?>("DeathDay");                         
			set => SetPropertyValue<DateTime?>("DeathDay", value); 
			
        }
		//Tooltip for Object
		public object DeathDayToolTipControllerText(View view)
        {
            #region 1523ImportCode 
System.DateTime? day = DeathDay;

if (day.HasValue)
{
    int lunarDay = day.Value.Day;
    int lunarMonth = day.Value.Month;

    int currentYear = System.DateTime.Now.Year;

    System.DateTime lunarDateInCurrentYear = new System.DateTime(currentYear, lunarMonth, lunarDay);

    System.Globalization.ChineseLunisolarCalendar lunarCalendar = new System.Globalization.ChineseLunisolarCalendar();
    System.DateTime solarDate = lunarCalendar.ToDateTime(currentYear, lunarMonth, lunarDay, 0, 0, 0, 0);

    return solarDate.ToString("d/M/yyyy");
}

#endregion 1523ImportCode
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultDeathDay(View view = null)
        { 
			return DeathDay;
        }
		//Set Default Value
		public void SetDefaultDeathDay(View view = null)
        {
            //if (DeathDay is null){
            //    var result = GetDefaultDeathDay(view);
            //    if (result != null && result != DeathDay){
			//          DeathDay = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DeathDayIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDeathDay();
				//if (result != null && DeathDay != null){
				//	return !DeathDay.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Contact _spousecontact;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kết hôn")]
        [ToolTip("Kết hôn")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SpouseContactCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("SpouseContact-SpouseContactList")]
	 
		public Module.BusinessObjects.Contact SpouseContact
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Contact>("SpouseContact");                         
			set => SetPropertyValue<Module.BusinessObjects.Contact>("SpouseContact", value); 
			
        }
		//Tooltip for Object
		public object SpouseContactToolTipControllerText(View view)
        {
        //    if (SpouseContact != null) 
		//			return SpouseContact;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Contact GetDefaultSpouseContact(View view = null)
        { 
			return SpouseContact;
        }
		//Set Default Value
		public void SetDefaultSpouseContact(View view = null)
        {
            //if (SpouseContact is null){
            //    var result = GetDefaultSpouseContact(view);
            //    if (result != null && result != SpouseContact){
			//          SpouseContact = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpouseContactIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpouseContact();
				//if (result != null && SpouseContact != null){
				//	return !SpouseContact.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SpouseContactCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SpouseContact));
            }
        }
	
       
		//private Module.BusinessObjects.Contact _parentcontact;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cha mẹ")]
        [ToolTip("Cha mẹ")]
		//[Index(10)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ParentContactCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("ParentContact-ChildContactList")]
	 
		public Module.BusinessObjects.Contact ParentContact
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Contact>("ParentContact");                         
			set => SetPropertyValue<Module.BusinessObjects.Contact>("ParentContact", value); 
			
        }
		//Tooltip for Object
		public object ParentContactToolTipControllerText(View view)
        {
        //    if (ParentContact != null) 
		//			return ParentContact;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Contact GetDefaultParentContact(View view = null)
        { 
			return ParentContact;
        }
		//Set Default Value
		public void SetDefaultParentContact(View view = null)
        {
            //if (ParentContact is null){
            //    var result = GetDefaultParentContact(view);
            //    if (result != null && result != ParentContact){
			//          ParentContact = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ParentContactIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultParentContact();
				//if (result != null && ParentContact != null){
				//	return !ParentContact.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ParentContactCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ParentContact));
            }
        }
	
       
		//private Module.BusinessObjects.Country _nationality;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quốc tịch")]
        [ToolTip("Quốc tịch")]
		//[Index(11)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(NationalityCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Country Nationality
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Country>("Nationality");                         
			set => SetPropertyValue<Module.BusinessObjects.Country>("Nationality", value); 
			
        }
		//Tooltip for Object
		public object NationalityToolTipControllerText(View view)
        {
        //    if (Nationality != null) 
		//			return Nationality;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Country GetDefaultNationality(View view = null)
        { 
			return Nationality;
        }
		//Set Default Value
		public void SetDefaultNationality(View view = null)
        {
            //if (Nationality is null){
            //    var result = GetDefaultNationality(view);
            //    if (result != null && result != Nationality){
			//          Nationality = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NationalityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNationality();
				//if (result != null && Nationality != null){
				//	return !Nationality.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator NationalityCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Nationality));
            }
        }
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
		//[Index(12)]
		[DevExpress.Xpo.Association("Contact-BookMarkList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BookMark> BookMarkList
        {      
		    get => GetCollection<Module.BusinessObjects.BookMark>("BookMarkList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quá trình")]
		//[Index(13)]
		[DevExpress.Xpo.Association("Contact-HistoryList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.History> HistoryList
        {      
		    get => GetCollection<Module.BusinessObjects.History>("HistoryList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Con cái")]
		//[Index(14)]
		[DevExpress.Xpo.Association("ParentContact-ChildContactList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Contact> ChildContactList
        {      
		    get => GetCollection<Module.BusinessObjects.Contact>("ChildContactList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hôn nhân")]
		//[Index(15)]
		[DevExpress.Xpo.Association("SpouseContact-SpouseContactList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Contact> SpouseContactList
        {      
		    get => GetCollection<Module.BusinessObjects.Contact>("SpouseContactList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài khoản")]
		//[Index(16)]
		[DevExpress.Xpo.Association("Contact-PaymentAccountList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.PaymentAccount> PaymentAccountList
        {      
		    get => GetCollection<Module.BusinessObjects.PaymentAccount>("PaymentAccountList"); 
			
        }
       
		//private string _note;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(17)]		

 		[Size(1000)]
		public string Note
        { 
		    get => GetPropertyValue<string>("Note");                         
			set => SetPropertyValue<string>("Note", value); 
			
        }
		//Tooltip for Object
		public object NoteToolTipControllerText(View view)
        {
        //    if (Note != null) 
		//			return Note;
            return null;
        }
		//Get Default Value
        public string GetDefaultNote(View view = null)
        { 
			return Note;
        }
		//Set Default Value
		public void SetDefaultNote(View view = null)
        {
            //if (Note is null){
            //    var result = GetDefaultNote(view);
            //    if (result != null && result != Note){
			//          Note = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NoteIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNote();
				//if (result != null && Note != null){
				//	return !Note.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Player")]
		//[Index(18)]
		[DevExpress.Xpo.Association("Contact-PlayerList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Player> PlayerList
        {      
		    get => GetCollection<Module.BusinessObjects.Player>("PlayerList"); 
			
        }
       
		//private Module.BusinessObjects.Org _org;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tổ chức")]
        [ToolTip("Tổ chức")]
		//[Index(21)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OrgCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Org-ContactList")]
	 
		public Module.BusinessObjects.Org Org
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Org>("Org");                         
			set => SetPropertyValue<Module.BusinessObjects.Org>("Org", value); 
			
        }
		//Tooltip for Object
		public object OrgToolTipControllerText(View view)
        {
        //    if (Org != null) 
		//			return Org;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool OrgIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrg();
				//if (result != null && Org != null){
				//	return !Org.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator OrgCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Org));
            }
        }
	
       
		//private ContactType _contacttype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(22)]		
		public ContactType ContactType
        { 
		    get => GetPropertyValue<ContactType>("ContactType");                         
			set => SetPropertyValue<ContactType>("ContactType", value); 
			
        }
		//Tooltip for Object
		public object ContactTypeToolTipControllerText(View view)
        {
        //    if (ContactType != null) 
		//			return ContactType;
            return null;
        }
		//Get Default Value
        public ContactType GetDefaultContactType(View view = null)
        { 
			return ContactType;
        }
		//Set Default Value
		public void SetDefaultContactType(View view = null)
        {
            //if (ContactType is null){
            //    var result = GetDefaultContactType(view);
            //    if (result != null && result != ContactType){
			//          ContactType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ContactTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultContactType();
				//if (result != null && ContactType != null){
				//	return !ContactType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Gender _gender;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giới tính")]
        [ToolTip("Giới tính")]
		//[Index(23)]		
		public Gender Gender
        { 
		    get => GetPropertyValue<Gender>("Gender");                         
			set => SetPropertyValue<Gender>("Gender", value); 
			
        }
		//Tooltip for Object
		public object GenderToolTipControllerText(View view)
        {
        //    if (Gender != null) 
		//			return Gender;
            return null;
        }
		//Get Default Value
        public Gender GetDefaultGender(View view = null)
        { 
			return Gender;
        }
		//Set Default Value
		public void SetDefaultGender(View view = null)
        {
            //if (Gender is null){
            //    var result = GetDefaultGender(view);
            //    if (result != null && result != Gender){
			//          Gender = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool GenderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultGender();
				//if (result != null && Gender != null){
				//	return !Gender.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Folder _folder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(24)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[InActive] = False And ([FolderType] = ##ToString#Contact# Or [FolderType] = ##ToString#Org#)")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-ContactList")]
	 
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
	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(25)]		
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

	
       
		//private DevExpress.Persistent.Base.General.ITreeNode _parent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Parent")]
        [ToolTip("Parent")]
		//[Index(26)]		
	    [Browsable(false)]
		public DevExpress.Persistent.Base.General.ITreeNode Parent
        { 
		    #region 1443ImportCode 
get => ParentContact;
#endregion 1443ImportCode
			
        }
		//Tooltip for Object
		public object ParentToolTipControllerText(View view)
        {
        //    if (Parent != null) 
		//			return Parent;
            return null;
        }
		//Get Default Value
        public DevExpress.Persistent.Base.General.ITreeNode GetDefaultParent(View view = null)
        { 
			return Parent;
        }
		//Set Default Value
		public void SetDefaultParent(View view = null)
        {
            //if (Parent is null){
            //    var result = GetDefaultParent(view);
            //    if (result != null && result != Parent){
			//          Parent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ParentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultParent();
				//if (result != null && Parent != null){
				//	return !Parent.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private System.ComponentModel.IBindingList _children;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Children")]
        [ToolTip("Children")]
		//[Index(27)]		
	    [Browsable(false)]
		public System.ComponentModel.IBindingList Children
        { 
		    #region 1444ImportCode 
get => ChildContactList;
#endregion 1444ImportCode
			
        }
		//Tooltip for Object
		public object ChildrenToolTipControllerText(View view)
        {
        //    if (Children != null) 
		//			return Children;
            return null;
        }
		//Get Default Value
        public System.ComponentModel.IBindingList GetDefaultChildren(View view = null)
        { 
			return Children;
        }
		//Set Default Value
		public void SetDefaultChildren(View view = null)
        {
            //if (Children is null){
            //    var result = GetDefaultChildren(view);
            //    if (result != null && result != Children){
			//          Children = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ChildrenIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultChildren();
				//if (result != null && Children != null){
				//	return !Children.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(28)]		
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

	
       
		//private Module.BusinessObjects.Member _member;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(29)]		
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
	
       
		//private bool? _open;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Công khai")]
        [ToolTip("Công khai")]
		//[Index(30)]		
		public bool? Open
        { 
		    get => GetPropertyValue<bool?>("Open");                         
			set => SetPropertyValue<bool?>("Open", value); 
			
        }
		//Tooltip for Object
		public object OpenToolTipControllerText(View view)
        {
        //    if (Open != null) 
		//			return Open;
            return null;
        }
		//Get Default Value
        public bool? GetDefaultOpen(View view = null)
        { 
			return Open;
        }
		//Set Default Value
		public void SetDefaultOpen(View view = null)
        {
            //if (Open is null){
            //    var result = GetDefaultOpen(view);
            //    if (result != null && result != Open){
			//          Open = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OpenIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOpen();
				//if (result != null && Open != null){
				//	return !Open.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
        private bool _display;
        [Browsable(false)]
        [NonPersistent]
        [ImmediatePostData]
        public bool Display
        {
            get { return _display; }
            set { SetPropertyValue("Display", ref _display, value); }
        }
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 1506ImportCode
            base.AfterConstruction();
SetDefaultMember();
            #endregion 1506ImportCode
            Display = true;
 
        //SetDefaultCode(View view = null);
        //SetDefaultFullName(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultTitle(View view = null);
        //SetDefaultWorkPlace(View view = null);
        //SetDefaultNativePlace(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultBirthday(View view = null);
        //SetDefaultDeathDay(View view = null);
        //SetDefaultSpouseContact(View view = null);
        //SetDefaultParentContact(View view = null);
        //SetDefaultNationality(View view = null);
        //SetDefaultOrg(View view = null);
        //SetDefaultContactType(View view = null);
        //SetDefaultGender(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultParent(View view = null);
        //SetDefaultChildren(View view = null);
        //SetDefaultInActive(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultOpen(View view = null);
			
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
            #region 1340ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 1340ImportCode
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
            Session.Delete(this.BookMarkList);				
  
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
				
                    case nameof(WorkPlace):
                        OnChangedWorkPlace(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedWorkPlace(object oldValue, object newValue)
        {
            #region 1277ImportCode
            if (newValue is null) return;
SetDefaultOrg();            
            #endregion 1277ImportCode
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
			//	SetDefaultHistoryList();
			//	SetDefaultChildContactList();
			//	SetDefaultSpouseContactList();
			//	SetDefaultPaymentAccountList();
			//	SetDefaultNote();
			//	SetDefaultPlayerList();
			//	SetDefaultArtistRoleList();
			//	SetDefaultSongList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1505ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 1505            Oid: 6dcfcf47-4d07-48a4-baf3-ff0b71a0d120
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 1505ImportCode
#region 1507ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 1507            Oid: afae5a08-90c2-440f-a2d5-42b54ade6f0a
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 1507ImportCode
#region 1342ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 1342            Oid: a41336aa-b280-4b6c-be91-51647f6d08b8
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1342ImportCode
#region 1276ImportCode
		public void SetDefaultOrg(View view = null)
        {
            //Code: 1276            Oid: 7feccaf0-0dc9-47b5-bcd7-cd358cc4aeb6
            Org = GetDefaultOrg();

        }
#endregion 1276ImportCode
#region 1275ImportCode
		public Module.BusinessObjects.Org GetDefaultOrg(View view = null)
        {
            //Code: 1275            Oid: c7cd15d2-795f-4866-9a3f-eb1efcc6e062
            //if(WorkPlace != null)
if(!string.IsNullOrEmpty(WorkPlace))
	return Session.FindObject<Org>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Code = ?", WorkPlace));
return null;

        }
#endregion 1275ImportCode
#region 1341ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 1341            Oid: 8f37f0ee-3622-4252-9eb2-6b88d1ed4b63
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 1341ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
