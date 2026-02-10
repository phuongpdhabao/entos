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
	[NavigationItem("HumanResouce")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Thành viên"), ImageName("Member")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(Updater))]
 
	[MobileColumnAttribute(Context = "ChatSession_MemberList_ListView", TargetItems = nameof(Name)+ "," + nameof(Image)+ "," + nameof(MemberFolder))]
	[MobileColumnAttribute(Context = "Member_ListView", TargetItems = nameof(Image)+ "," + nameof(Name)+ "," + nameof(MemberFolder))]
	[MobileColumnAttribute(Context = "MemberFolder_MemberList_ListView", TargetItems = nameof(Name)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Knowledge_MemberList_ListView", TargetItems = nameof(MemberFolder)+ "," + nameof(Name)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "LoginAccount_MemberList_ListView", TargetItems = nameof(Name)+ "," + nameof(MemberFolder)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "WorkType_MemberList_ListView", TargetItems = nameof(MemberFolder)+ "," + nameof(Name)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Member_LookupListView", TargetItems = nameof(MemberFolder)+ "," + nameof(Image)+ "," + nameof(Name))]
	[DefaultProperty("Name")]
 
	[MapInheritance(MapInheritanceType.ParentTable)]
[OptimisticLocking(true)]
    public partial class Member: DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser  , IObjectImage , INoIndexColumn, IOnViewObjectSpaceCommitted   , IObjectSpaceLink, DevExpress.ExpressApp.Security.ISecurityUserWithLoginInfo    //, HbBaseObject
    {
        public Member(Session session)
            : base(session) {              
        }

		        private System.Collections.Generic.Dictionary<string, bool> _cacheAppearanceDisableDelete;
		[Browsable(false)]
        public bool AppearanceDisableDelete
        {
            get
            {

                if (Session.IsNewObject(this))
                    return false;
				if (MemberObjectSystemTypeList.IsLoaded)
                {
                    if (MemberObjectSystemTypeList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(MemberObjectSystemTypeList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(MemberObjectSystemTypeList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.MemberObjectSystemType>(CriteriaOperator.Parse("[Member.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool memberobjectsystemtypelist = Session.Query<Module.BusinessObjects.MemberObjectSystemType>().Where(x => x.Member.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(MemberObjectSystemTypeList), memberobjectsystemtypelist);
                        if (memberobjectsystemtypelist)
                            return true;

                    }                    
                }				
				if (IncomeList.IsLoaded)
                {
                    if (IncomeList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(IncomeList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(IncomeList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Income>(CriteriaOperator.Parse("[Member.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool incomelist = Session.Query<Module.BusinessObjects.Income>().Where(x => x.Member.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(IncomeList), incomelist);
                        if (incomelist)
                            return true;

                    }                    
                }				
				if (WorkList.IsLoaded)
                {
                    if (WorkList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(WorkList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(WorkList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Work>(CriteriaOperator.Parse("[Member.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool worklist = Session.Query<Module.BusinessObjects.Work>().Where(x => x.Member.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(WorkList), worklist);
                        if (worklist)
                            return true;

                    }                    
                }				
				if (MemberDataServiceList.IsLoaded)
                {
                    if (MemberDataServiceList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(MemberDataServiceList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(MemberDataServiceList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.MemberDataService>(CriteriaOperator.Parse("[Member.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool memberdataservicelist = Session.Query<Module.BusinessObjects.MemberDataService>().Where(x => x.Member.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(MemberDataServiceList), memberdataservicelist);
                        if (memberdataservicelist)
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

               

		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(1)]		

 		[Size(100)]
		[RuleRequiredField("RequiredMemberName", DefaultContexts.Save)]
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

	
       
		//private Module.BusinessObjects.Contact _contact;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên hệ")]
        [ToolTip("Liên hệ")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ContactCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Contact Contact
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Contact>("Contact");                         
			set => SetPropertyValue<Module.BusinessObjects.Contact>("Contact", value); 
			
        }
		//Tooltip for Object
		public object ContactToolTipControllerText(View view)
        {
        //    if (Contact != null) 
		//			return Contact;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Contact GetDefaultContact(View view = null)
        { 
			return Contact;
        }
		//Set Default Value
		public void SetDefaultContact(View view = null)
        {
            //if (Contact is null){
            //    var result = GetDefaultContact(view);
            //    if (result != null && result != Contact){
			//          Contact = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ContactIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultContact();
				//if (result != null && Contact != null){
				//	return !Contact.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ContactCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Contact));
            }
        }
	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ảnh")]
        [ToolTip("Ảnh")]
		//[Index(3)]		
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

	
       
		//private Module.BusinessObjects.Member _manager;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ManagerCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Member Manager
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Manager");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Manager", value); 
			
        }
		//Tooltip for Object
		public object ManagerToolTipControllerText(View view)
        {
        //    if (Manager != null) 
		//			return Manager;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Member GetDefaultManager(View view = null)
        { 
			return Manager;
        }
		//Set Default Value
		public void SetDefaultManager(View view = null)
        {
            //if (Manager is null){
            //    var result = GetDefaultManager(view);
            //    if (result != null && result != Manager){
			//          Manager = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ManagerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultManager();
				//if (result != null && Manager != null){
				//	return !Manager.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ManagerCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Manager));
            }
        }
	
       
		//private Module.BusinessObjects.MemberFolder _memberfolder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MemberFolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("MemberFolder-MemberList")]
	 
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
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại công việc")]
		//[Index(7)]
		[DataSourceCriteria("Not MemberList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("MemberList-WorkTypeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.WorkType> WorkTypeList
        {      
		    get => GetCollection<Module.BusinessObjects.WorkType>("WorkTypeList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đối tượng")]
		//[Index(8)]
		[DevExpress.Xpo.Association("Member-MemberObjectSystemTypeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.MemberObjectSystemType> MemberObjectSystemTypeList
        {      
		    get => GetCollection<Module.BusinessObjects.MemberObjectSystemType>("MemberObjectSystemTypeList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thu nhập")]
		//[Index(9)]
		[DevExpress.Xpo.Association("Member-IncomeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Income> IncomeList
        {      
		    get => GetCollection<Module.BusinessObjects.Income>("IncomeList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	

	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Phiên tin")]
		//[Index(11)]
		[DataSourceCriteria("Not MemberList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("ChatSessionList-MemberList")]
	    [VisibleInDetailView(false)]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ChatSession> ChatSessionList
        {      
		    get => GetCollection<Module.BusinessObjects.ChatSession>("ChatSessionList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đăng nhập")]
		//[Index(12)]
		[DataSourceCriteria("Not MemberList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("LoginAccountList-MemberList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.LoginAccount> LoginAccountList
        {      
		    get => GetCollection<Module.BusinessObjects.LoginAccount>("LoginAccountList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Công việc")]
		//[Index(13)]
		[DevExpress.Xpo.Association("Member-WorkList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Work> WorkList
        {      
		    get => GetCollection<Module.BusinessObjects.Work>("WorkList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiến thức")]
		//[Index(14)]
		[DataSourceCriteria("Not MemberList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("KnowledgeList-MemberList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Knowledge> KnowledgeList
        {      
		    get => GetCollection<Module.BusinessObjects.Knowledge>("KnowledgeList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dịch vụ phần mềm")]
		//[Index(15)]
		[DevExpress.Xpo.Association("Member-MemberDataServiceList")]
	    [RuleCombinationOfPropertiesIsUnique("UniqueRule.MemberDataServiceList", DefaultContexts.Save, "SoftwareServiceType")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.MemberDataService> MemberDataServiceList
        {      
		    get => GetCollection<Module.BusinessObjects.MemberDataService>("MemberDataServiceList"); 
			
        }
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(16)]		
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
		//[Index(17)]		
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
	
       
		//private bool _isuppermember;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Là cấp trên")]
        [ToolTip("Là cấp trên")]
		//[Index(19)]		
	    [NonPersistent()]
	    [NotMapped()]
		public bool IsUpperMember
        { 
		    #region 1432ImportCode 
get
{
    return IsCurrentUserEqualUpperMember(Manager, null);
}
#endregion 1432ImportCode
			
        }
		//Tooltip for Object
		public object IsUpperMemberToolTipControllerText(View view)
        {
        //    if (IsUpperMember != null) 
		//			return IsUpperMember;
            return null;
        }
		//Get Default Value
        public bool GetDefaultIsUpperMember(View view = null)
        { 
			return IsUpperMember;
        }
		//Set Default Value
		public void SetDefaultIsUpperMember(View view = null)
        {
            //if (IsUpperMember is null){
            //    var result = GetDefaultIsUpperMember(view);
            //    if (result != null && result != IsUpperMember){
			//          IsUpperMember = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool IsUpperMemberIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultIsUpperMember();
				//if (result != null && IsUpperMember != null){
				//	return !IsUpperMember.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
 
            #region 3768ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
            #endregion 3768ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultContact(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultManager(View view = null);
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultIsUpperMember(View view = null);
			
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
            #region 3767ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 3767ImportCode
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

   

        #region Nạp phương thức LoginInfo cho đối tượng
        [Browsable(false)]
        [DevExpress.Xpo.Aggregated, DevExpress.Xpo.Association("User-LoginInfo")]
        public XPCollection<ApplicationUserLoginInfo> LoginInfo {
            get { return GetCollection<ApplicationUserLoginInfo>(nameof(LoginInfo)); }
        }

        System.Collections.Generic.IEnumerable<DevExpress.ExpressApp.Security.ISecurityUserLoginInfo> DevExpress.ExpressApp.Security.IOAuthSecurityUser.UserLogins => LoginInfo.OfType<DevExpress.ExpressApp.Security.ISecurityUserLoginInfo>();

        IObjectSpace IObjectSpaceLink.ObjectSpace { get; set; }

        DevExpress.ExpressApp.Security.ISecurityUserLoginInfo DevExpress.ExpressApp.Security.ISecurityUserWithLoginInfo.CreateUserLoginInfo(string loginProviderName, string providerUserKey) {
            ApplicationUserLoginInfo result = ((IObjectSpaceLink)this).ObjectSpace.CreateObject<ApplicationUserLoginInfo>();
            result.LoginProviderName = loginProviderName;
            result.ProviderUserKey = providerUserKey;
            result.User = this;
            return result;
        }
        #endregion

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
			//	SetDefaultWorkTypeList();
			//	SetDefaultMemberObjectSystemTypeList();
			//	SetDefaultIncomeList();
			//	SetDefaultRoles();
			//	SetDefaultChatSessionList();
			//	SetDefaultLoginAccountList();
			//	SetDefaultWorkList();
			//	SetDefaultKnowledgeList();
			//	SetDefaultMemberDataServiceList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 3771ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 3771            Oid: 766a704e-f0b4-4ec6-8b6c-41cad64b0702
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3771ImportCode
#region 3766ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 3766            Oid: 827d8531-9d84-4fe1-94e4-4b78036f4fa1
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 3766ImportCode
#region 3770ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 3770            Oid: 8156df0f-cfb2-4587-91ca-453bf91e8eac
            Updater = GetDefaultUpdater();
        }
#endregion 3770ImportCode
#region 3769ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 3769            Oid: 39148b13-279f-4dbb-9c76-4b4cc5ec476b
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 3769ImportCode
        #endregion
//Mã nguồn bổ sung
#region MemberImportCode
        private bool IsCurrentUserEqualUpperMember(Member member, System.Collections.Generic.List<Member> checkListMember)
        {            
            if (member != null)
            {
                //Chống loop
                if (checkListMember is null)
                {
                    checkListMember = new System.Collections.Generic.List<Member>() { member };
                }
                else if (checkListMember.Contains(member))
                {
                    return false;
                }
                else
                {
                    checkListMember.Add(member);
                }
                if (SecuritySystem.CurrentUserId.Equals(member.Oid))
                    return true;
                return IsCurrentUserEqualUpperMember(member.Manager, checkListMember);
            }
            return false;
        }

       public static Member GetCurrentMember(DevExpress.Xpo.Session session)
       {
           return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(session);
       }
#endregion MemberImportCode
		 		 
    }
}
