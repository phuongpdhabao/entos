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
	[NavigationItem("Accounting")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Tài khoản kế toán"), ImageName("EntryFolder")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("EntryFolder LowerFolderList, UpperFolder, PermissionPolicyRole Hide_None__" , TargetItems = "LowerFolderList, UpperFolder, PermissionPolicyRole" , Criteria = "[Member.Oid] <> CURRENTUSERID()",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(AllAccountEntryList)+ "," + nameof(Update)+ "," + nameof(Updater)+ "," + nameof(Quantity)+ "," + nameof(MemberFolder)+ "," + nameof(Order))]
 
	[MobileColumnAttribute(Context = "EntryFolder_LookupListView", TargetItems = nameof(Code))]
	[MobileColumnAttribute(Context = "EntryFolder_LowerFolderList_ListView", TargetItems = nameof(PermissionPolicyRole)+ "," + nameof(Code)+ "," + nameof(InActive))]
	[MobileColumnAttribute(Context = "EntryFolder_ListView", TargetItems = nameof(Code))]
	[DefaultProperty("Name")]
 
//[OptimisticLocking(false)]
    public partial class EntryFolder: DevExpress.Persistent.BaseImpl.BaseObject , DevExpress.Persistent.Base.General.ITreeNode , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public EntryFolder(Session session)
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
				if (LowerFolderList.IsLoaded)
                {
                    if (LowerFolderList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(LowerFolderList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(LowerFolderList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.EntryFolder>(CriteriaOperator.Parse("[UpperFolder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool lowerfolderlist = Session.Query<Module.BusinessObjects.EntryFolder>().Where(x => x.UpperFolder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(LowerFolderList), lowerfolderlist);
                        if (lowerfolderlist)
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
                        //if (Session.FindObject<Module.BusinessObjects.AccountEntry>(CriteriaOperator.Parse("[EntryFolder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool accountentrylist = Session.Query<Module.BusinessObjects.AccountEntry>().Where(x => x.EntryFolder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(AccountEntryList), accountentrylist);
                        if (accountentrylist)
                            return true;

                    }                    
                }				
				if (PartyAccountList.IsLoaded)
                {
                    if (PartyAccountList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(PartyAccountList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(PartyAccountList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.AccountEntry>(CriteriaOperator.Parse("[PartyAccountFolder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool partyaccountlist = Session.Query<Module.BusinessObjects.AccountEntry>().Where(x => x.PartyAccountFolder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(PartyAccountList), partyaccountlist);
                        if (partyaccountlist)
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
		//[Index(0)]		

 		[Size(100)]
		[RuleRequiredField("RequiredEntryFolderName", DefaultContexts.Save)]
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

	
       
		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(1)]		

 		[Size(20)]
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

	
       
		//private EntryType _entrytype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(2)]		
		public EntryType EntryType
        { 
		    get => GetPropertyValue<EntryType>("EntryType");                         
			set => SetPropertyValue<EntryType>("EntryType", value); 
			
        }
		//Tooltip for Object
		public object EntryTypeToolTipControllerText(View view)
        {
        //    if (EntryType != null) 
		//			return EntryType;
            return null;
        }
		//Get Default Value
        public EntryType GetDefaultEntryType(View view = null)
        { 
			return EntryType;
        }
		//Set Default Value
		public void SetDefaultEntryType(View view = null)
        {
            //if (EntryType is null){
            //    var result = GetDefaultEntryType(view);
            //    if (result != null && result != EntryType){
			//          EntryType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EntryTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEntryType();
				//if (result != null && EntryType != null){
				//	return !EntryType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole _permissionpolicyrole;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhóm")]
        [ToolTip("Nhóm")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PermissionPolicyRoleCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole PermissionPolicyRole
        { 
		    get => GetPropertyValue<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole>("PermissionPolicyRole");                         
			set => SetPropertyValue<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole>("PermissionPolicyRole", value); 
			
        }
		//Tooltip for Object
		public object PermissionPolicyRoleToolTipControllerText(View view)
        {
        //    if (PermissionPolicyRole != null) 
		//			return PermissionPolicyRole;
            return null;
        }
		//Get Default Value
        public DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole GetDefaultPermissionPolicyRole(View view = null)
        { 
			return PermissionPolicyRole;
        }
		//Set Default Value
		public void SetDefaultPermissionPolicyRole(View view = null)
        {
            //if (PermissionPolicyRole is null){
            //    var result = GetDefaultPermissionPolicyRole(view);
            //    if (result != null && result != PermissionPolicyRole){
			//          PermissionPolicyRole = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PermissionPolicyRoleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPermissionPolicyRole();
				//if (result != null && PermissionPolicyRole != null){
				//	return !PermissionPolicyRole.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PermissionPolicyRoleCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PermissionPolicyRole));
            }
        }
	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(4)]		
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
	
       
		//private bool _inactive;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(5)]		
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
		[DevExpress.Xpo.DisplayName("Cấp dưới")]
		//[Index(6)]
		[DevExpress.Xpo.Association("UpperFolder-LowerFolderList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.EntryFolder> LowerFolderList
        {      
		    get => GetCollection<Module.BusinessObjects.EntryFolder>("LowerFolderList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	

	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bút toán")]
		//[Index(7)]
		[DevExpress.Xpo.Association("EntryFolder-AccountEntryList")]
	    [VisibleInDetailView(false)]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.AccountEntry> AccountEntryList
        {      
		    get => GetCollection<Module.BusinessObjects.AccountEntry>("AccountEntryList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	

	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đối ứng")]
		//[Index(8)]
		[DevExpress.Xpo.Association("PartyAccountFolder-PartyAccountList")]
	    [VisibleInDetailView(false)]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.AccountEntry> PartyAccountList
        {      
		    get => GetCollection<Module.BusinessObjects.AccountEntry>("PartyAccountList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bút toán")]
		//[Index(9)]
		//[DevExpress.Xpo.Association]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.AccountEntry> AllAccountEntryList
        {      

                #region 3359ImportCode 
            get
            {
                if (_allAccountEntryList is null)
                {
                    FillDefaultAllAccountEntryList();
                    SetDefaultAllAccountEntryList();
                }
                return _allAccountEntryList;

            }
		}

        private XPCollection<Module.BusinessObjects.AccountEntry> _allAccountEntryList = null;
        private void FillDefaultAllAccountEntryList()
        {
            _allAccountEntryList = new XPCollection<Module.BusinessObjects.AccountEntry>(PersistentCriteriaEvaluationBehavior.InTransaction, Session, CriteriaOperator.Parse("EntryFolder= ? or PartyAccountFolder = ?", this, this));
            if (_allAccountEntryList.Count == 0 && Session.IsNewObject(this) && (AccountEntryList?.Count > 0 || PartyAccountList?.Count > 0))
            {
                foreach (var accountEntry in AccountEntryList)
                    _allAccountEntryList?.Add(accountEntry);
                foreach (var partyAcount in PartyAccountList)
                    _allAccountEntryList?.Add(partyAcount);
            }

#endregion 3359ImportCode
			
        }
       
		//private DevExpress.Persistent.Base.General.ITreeNode _parent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Parent")]
        [ToolTip("Parent")]
		//[Index(10)]		
	    [Browsable(false)]
		public DevExpress.Persistent.Base.General.ITreeNode Parent
        { 
		    #region 2828ImportCode 
get => UpperFolder;
#endregion 2828ImportCode
			
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
		//[Index(11)]		
	    [Browsable(false)]
		public System.ComponentModel.IBindingList Children
        { 
		    #region 2829ImportCode 
get => LowerFolderList;
#endregion 2829ImportCode
			
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

	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(12)]		
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
		//[Index(13)]		
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
	
       
		//private decimal? _quantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(14)]		
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
		//Set Default Value

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

	
       
		//private Module.BusinessObjects.EntryFolder _upperfolder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(15)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpperFolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("UpperFolder-LowerFolderList")]
	 
		public Module.BusinessObjects.EntryFolder UpperFolder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.EntryFolder>("UpperFolder");                         
			set => SetPropertyValue<Module.BusinessObjects.EntryFolder>("UpperFolder", value); 
			
        }
		//Tooltip for Object
		public object UpperFolderToolTipControllerText(View view)
        {
        //    if (UpperFolder != null) 
		//			return UpperFolder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.EntryFolder GetDefaultUpperFolder(View view = null)
        { 
			return UpperFolder;
        }
		//Set Default Value
		public void SetDefaultUpperFolder(View view = null)
        {
            //if (UpperFolder is null){
            //    var result = GetDefaultUpperFolder(view);
            //    if (result != null && result != UpperFolder){
			//          UpperFolder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperFolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpperFolder();
				//if (result != null && UpperFolder != null){
				//	return !UpperFolder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpperFolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(UpperFolder));
            }
        }
	
       
		//private Module.BusinessObjects.MemberFolder _memberfolder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(16)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MemberFolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [NotMapped()]
	    [NonPersistent()]
	    [ImmediatePostData()]
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
	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(17)]		
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

	
       
		//private bool _flag;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ")]
        [ToolTip("Cờ")]
		//[Index(18)]		
	    [NotMapped()]
	    [NonPersistent()]
		public bool Flag
        { 
		    get => GetPropertyValue<bool>("Flag");                         
			set => SetPropertyValue<bool>("Flag", value); 
			
        }
		//Tooltip for Object
		public object FlagToolTipControllerText(View view)
        {
        //    if (Flag != null) 
		//			return Flag;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFlag(View view = null)
        { 
			return Flag;
        }
		//Set Default Value
		public void SetDefaultFlag(View view = null)
        {
            //if (Flag is null){
            //    var result = GetDefaultFlag(view);
            //    if (result != null && result != Flag){
			//          Flag = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FlagIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFlag();
				//if (result != null && Flag != null){
				//	return !Flag.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
 
            #region 2836ImportCode
            base.AfterConstruction();
SetDefaultMember();
            #endregion 2836ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultEntryType(View view = null);
        //SetDefaultPermissionPolicyRole(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultInActive(View view = null);
        //SetDefaultParent(View view = null);
        //SetDefaultChildren(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultUpperFolder(View view = null);
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultFlag(View view = null);
			
        }
        
        protected override void OnLoading()
        {
            base.OnLoading();
        }
        
        protected override void OnLoaded()
        {
            #region 3366ImportCode
            base.OnLoaded();
SetDefaultMemberFolder();
GetDefaultQuantity();
            #endregion 3366ImportCode
        }

        private bool alreadySaving = false;        
        protected override void OnSaving()
        {
            #region 2831ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 2831ImportCode
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
				
                    case nameof(MemberFolder):
                        OnChangedMemberFolder(oldValue, newValue);
                        break;
				
                    case nameof(UpperFolder):
                        OnChangedUpperFolder(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedMemberFolder(object oldValue, object newValue)
        {
            #region 3368ImportCode
                        
            #endregion 3368ImportCode
        }               
        private void OnChangedUpperFolder(object oldValue, object newValue)
        {
            #region 2855ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 2855ImportCode
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
			//	SetDefaultLowerFolderList();
			//	SetDefaultAccountEntryList();
			//	SetDefaultPartyAccountList();
			//	SetDefaultAllAccountEntryList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 2832ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 2832            Oid: 9eac8745-443f-4be6-ae50-42c4ec306e23
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 2832ImportCode
#region 3365ImportCode
		public Module.BusinessObjects.MemberFolder GetDefaultMemberFolder(View view = null)
        {
            //Code: 3365            Oid: 7be035cb-c4eb-4125-bdc9-d6ec8cb7672a
              
     var memberFolderValue = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(Session, "MemberFolder", "null", SecuritySystem.CurrentUserId);
     if (memberFolderValue != null && !string.IsNullOrEmpty(memberFolderValue.Value))
         return Session.FindObject<MemberFolder>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ?", memberFolderValue.Value));
 
 return null;

        }
#endregion 3365ImportCode
#region 2833ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 2833            Oid: ecb33792-ee5a-435b-aa92-be9292c4b2f8
            Updater = GetDefaultUpdater();
        }
#endregion 2833ImportCode
#region 2834ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 2834            Oid: 07391700-fdbe-4a43-8911-7afd46e82f39
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2834ImportCode
#region 2830ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 2830            Oid: 12b334fd-1b93-423f-a7cd-6c11b91b3c01
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 2830ImportCode
#region 3367ImportCode
		public void SetAccountEntryListFilter()
        {
            //Code: 3367            Oid: 1496e139-fb66-4753-9add-318d457865d6
                 var criteria = GetMemberFolderCriteria(this);
     AccountEntryList.Filter = criteria;
     PartyAccountList.Filter = criteria;     
     AllAccountEntryList.Filter = criteria;
        }
#endregion 3367ImportCode
#region 3363ImportCode
		public void SetDefaultQuantity(View view = null)
        {
            //Code: 3363            Oid: 8482d5db-25de-45ad-81be-8e5170920889
              if (AccountEntryList.Count > 0 || PartyAccountList.Count > 0)
  {
      var quantity = GetDefaultQuantity();
      if (Quantity != quantity)
          Quantity = quantity;
  }

        }
#endregion 3363ImportCode
#region 2853ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 2853            Oid: d2502b6d-6c37-44d1-a251-8138e7844aff
            if (UpperFolder != null && UpperFolder.LowerFolderList != null)
{
    var lasted = UpperFolder.LowerFolderList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
return null;
        }
#endregion 2853ImportCode
#region 2835ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 2835            Oid: 7f3791d9-8a2a-43d6-953c-4ff067622c53
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 2835ImportCode
#region 2837ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 2837            Oid: 6f19bd4b-0fe0-4ac0-8cdd-e96bd51f4b30
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2837ImportCode
#region 3360ImportCode
		public void SetDefaultAllAccountEntryList(View view = null)
        {
            //Code: 3360            Oid: d921e63e-423d-4434-b5b1-2fc7476ae01f
                        //Code: 3360            Oid: d921e63e-423d-4434-b5b1-2fc7476ae01f
            bool mark1 = true;
            bool mark2 = true;

            foreach (var accountEntry in _allAccountEntryList)
            {
                if (accountEntry.EntryFolder.Oid == Oid)
                    accountEntry.PartyAccountFolder = accountEntry.PartyAccountFolder;
                else
                    accountEntry.PartyAccountFolder = this;

                mark1 = ((accountEntry.EntryFolder.Oid == Oid && accountEntry.Debit) || (!(accountEntry.EntryFolder.Oid == Oid) && !(accountEntry.Debit)));
                mark2 = ((accountEntry.EntryFolder.Oid == Oid && !(accountEntry.Debit)) || (!(accountEntry.EntryFolder.Oid == Oid) && accountEntry.Debit));

                if (mark1)
                    accountEntry.AmountDebit = accountEntry.Amount;
                else
                    accountEntry.AmountDebit = 0;

                if (mark2)
                    accountEntry.AmountCredit = accountEntry.Amount;
                else
                    accountEntry.AmountCredit = 0;
            }
        }
#endregion 3360ImportCode
#region 2854ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 2854            Oid: 5ea42328-d5f8-4fcc-ab5a-a09e8fcecb4c
            Order= GetDefaultOrder();
        }
#endregion 2854ImportCode
#region 3364ImportCode
		public void SetDefaultMemberFolder(View view = null)
        {
            //Code: 3364            Oid: 91ee7d50-59ae-406b-8cb8-fb307199c78f
            	MemberFolder = GetDefaultMemberFolder();

        }
#endregion 3364ImportCode
#region 3362ImportCode
		public decimal? GetDefaultQuantity(View view = null)
        {
            //Code: 3362            Oid: 7216b058-9a1e-4d34-a66d-8f2d38d7675b
                            decimal totalValue = 0;
                bool mark = true;
                bool mark2 = true;

                var defaultBook = Module.Helpers.ParameterHelper.GetValue(Session, "BookDefault");

                foreach (var accountEntry in AccountEntryList)
                {
                    var EntryTemp = accountEntry.Amount.Value;
                    if ((accountEntry.Book1 && defaultBook == "Book1") || (accountEntry.Book2 && defaultBook == "Book2"))
                    {
                        mark = (accountEntry.Debit && EntryType == EntryType.Debit) || (!accountEntry.Debit && EntryType != EntryType.Debit);
                        if (mark == true) totalValue += EntryTemp;
                        else totalValue -= EntryTemp;
                    }
                }
                foreach (var partyAccount in PartyAccountList)
                {
                    var PartyTemp = partyAccount.Amount.Value;
                    if ((partyAccount.Book1 && defaultBook == "Book1") || (partyAccount.Book2 && defaultBook == "Book2"))
                    {
                        mark2 = (partyAccount.Debit && partyAccount.EntryFolder.EntryType != EntryType.Debit) || (!partyAccount.Debit && partyAccount.EntryFolder.EntryType == EntryType.Debit);
                        if (mark2 == true) totalValue += PartyTemp;
                        else totalValue -= PartyTemp; ;
                    }
                }
                foreach (var childFolder in LowerFolderList)
                {
                    var childFolderQuantity = childFolder.GetDefaultQuantity();
                    if (childFolderQuantity != null)
                        totalValue += childFolderQuantity.Value;
                }
				Quantity = totalValue;
                return totalValue;
        

        }
#endregion 3362ImportCode
        #endregion
//Mã nguồn bổ sung
#region EntryFolderImportCode
 public DevExpress.Data.Filtering.GroupOperator GetMemberFolderCriteria(Module.BusinessObjects.EntryFolder entryFolder)
        {
            if (entryFolder == null) return null;

            // Tạo điều kiện cho Oid của folder cha
            var criteriaList = new System.Collections.Generic.List<DevExpress.Data.Filtering.CriteriaOperator>
     {
         new DevExpress.Data.Filtering.BinaryOperator("MemberFolder.Oid", entryFolder.MemberFolder.Oid)
     };
            //Mã nguồn bổ sung


            // Đệ quy cho các folder con
            foreach (var childFolder in entryFolder.LowerFolderList)
            {
                var childCriteria = GetMemberFolderCriteria(childFolder);
                if (childCriteria != null)
                {
                    criteriaList.Add(childCriteria);
                }
            }

            // Kết hợp các điều kiện thành GroupOperator
            return new DevExpress.Data.Filtering.GroupOperator(DevExpress.Data.Filtering.GroupOperatorType.Or, criteriaList.ToArray());

        }

#endregion EntryFolderImportCode
		 		 
    }
}
