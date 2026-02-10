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
    [ModelDefault("Caption", "Tập thể"), ImageName("MemberFolder")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(Name)+ "," + nameof(Code)+ "," + nameof(PermissionPolicyRole)+ "," + nameof(Member)+ "," + nameof(InActive)+ "," + nameof(LowerFolderList), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(Update)+ "," + nameof(Updater)+ "," + nameof(Order))]
 
	[MobileColumnAttribute(Context = "MemberFolder_LowerFolderList_ListView", TargetItems = nameof(Name)+ "," + nameof(Code)+ "," + nameof(Update)+ "," + nameof(PermissionPolicyRole)+ "," + nameof(InActive)+ "," + nameof(Member))]
	[MobileColumnAttribute(Context = "MemberFolder_LookupListView", TargetItems = nameof(Code)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "MemberFolder_ListView", TargetItems = nameof(Order)+ "," + nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class MemberFolder:  DevExpress.Xpo.XPLiteObject , DevExpress.Persistent.Base.General.ITreeNode , INoIndexColumn, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public MemberFolder(Session session)
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
				if (ChatList.IsLoaded)
                {
                    if (ChatList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ChatList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ChatList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Chat>(CriteriaOperator.Parse("[MemberFolder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool chatlist = Session.Query<Module.BusinessObjects.Chat>().Where(x => x.MemberFolder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ChatList), chatlist);
                        if (chatlist)
                            return true;

                    }                    
                }				
				if (MemberList.IsLoaded)
                {
                    if (MemberList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(MemberList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(MemberList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Member>(CriteriaOperator.Parse("[MemberFolder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool memberlist = Session.Query<Module.BusinessObjects.Member>().Where(x => x.MemberFolder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(MemberList), memberlist);
                        if (memberlist)
                            return true;

                    }                    
                }				
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
                        //if (Session.FindObject<Module.BusinessObjects.MemberFolder>(CriteriaOperator.Parse("[UpperFolder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool lowerfolderlist = Session.Query<Module.BusinessObjects.MemberFolder>().Where(x => x.UpperFolder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(LowerFolderList), lowerfolderlist);
                        if (lowerfolderlist)
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

 		[Size(250)]
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
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
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

	
       
		//private DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole _permissionpolicyrole;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhóm")]
        [ToolTip("Nhóm")]
		//[Index(2)]		
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
		//[Index(3)]		
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
		//[Index(4)]		
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
		[DevExpress.Xpo.DisplayName("Tin nhắn")]
		//[Index(5)]
		[DevExpress.Xpo.Association("MemberFolder-ChatList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Chat> ChatList
        {      
		    get => GetCollection<Module.BusinessObjects.Chat>("ChatList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thành viên")]
		//[Index(6)]
		[DevExpress.Xpo.Association("MemberFolder-MemberList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Member> MemberList
        {      
		    get => GetCollection<Module.BusinessObjects.Member>("MemberList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp dưới")]
		//[Index(7)]
		[DevExpress.Xpo.Association("UpperFolder-LowerFolderList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.MemberFolder> LowerFolderList
        {      
		    get => GetCollection<Module.BusinessObjects.MemberFolder>("LowerFolderList"); 
			
        }
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(8)]		
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
		//[Index(9)]		
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
		//[Index(10)]		
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

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(11)]		
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

	
       
		//private DevExpress.Persistent.Base.General.ITreeNode _parent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Parent")]
        [ToolTip("Parent")]
		//[Index(12)]		
	    [Browsable(false)]
		public DevExpress.Persistent.Base.General.ITreeNode Parent
        { 
		    #region 2847ImportCode 
get => UpperFolder;
#endregion 2847ImportCode
			
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
		//[Index(13)]		
	    [Browsable(false)]
		public System.ComponentModel.IBindingList Children
        { 
		    #region 2848ImportCode 
get => LowerFolderList;
#endregion 2848ImportCode
			
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

	
       
		//private Module.BusinessObjects.MemberFolder _upperfolder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(14)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpperFolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("UpperFolder-LowerFolderList")]
	 
		public Module.BusinessObjects.MemberFolder UpperFolder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.MemberFolder>("UpperFolder");                         
			set => SetPropertyValue<Module.BusinessObjects.MemberFolder>("UpperFolder", value); 
			
        }
		//Tooltip for Object
		public object UpperFolderToolTipControllerText(View view)
        {
        //    if (UpperFolder != null) 
		//			return UpperFolder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.MemberFolder GetDefaultUpperFolder(View view = null)
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
	
       
		//private bool _newchat;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Có tin mới")]
        [ToolTip("Có tin mới")]
		//[Index(15)]		
	    [NotMapped()]
	    [NonPersistent()]
		public bool NewChat
        { 
		    get => GetPropertyValue<bool>("NewChat");                         
			set => SetPropertyValue<bool>("NewChat", value); 
			
        }
		//Tooltip for Object
		public object NewChatToolTipControllerText(View view)
        {
        //    if (NewChat != null) 
		//			return NewChat;
            return null;
        }
		//Get Default Value
        public bool GetDefaultNewChat(View view = null)
        { 
			return NewChat;
        }
		//Set Default Value
		public void SetDefaultNewChat(View view = null)
        {
            //if (NewChat is null){
            //    var result = GetDefaultNewChat(view);
            //    if (result != null && result != NewChat){
			//          NewChat = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NewChatIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNewChat();
				//if (result != null && NewChat != null){
				//	return !NewChat.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _createchat;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tin nhắn")]
        [ToolTip("Tin nhắn")]
		//[Index(16)]		

 		[Size(250)]
	    [NonPersistent()]
	    [NotMapped()]
		public string CreateChat
        { 
		    #region 2873ImportCode 
get;set;
#endregion 2873ImportCode
			
        }
		//Tooltip for Object
		public object CreateChatToolTipControllerText(View view)
        {
        //    if (CreateChat != null) 
		//			return CreateChat;
            return null;
        }
		//Get Default Value
        public string GetDefaultCreateChat(View view = null)
        { 
			return CreateChat;
        }
		//Set Default Value
		public void SetDefaultCreateChat(View view = null)
        {
            //if (CreateChat is null){
            //    var result = GetDefaultCreateChat(view);
            //    if (result != null && result != CreateChat){
			//          CreateChat = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CreateChatIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCreateChat();
				//if (result != null && CreateChat != null){
				//	return !CreateChat.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _readtime;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thời gian đọc")]
        [ToolTip("Thời gian đọc")]
		//[Index(17)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy H:mm")]
		[ModelDefault("EditMask", "d/M/yyyy")]
	    [NonPersistent()]
	    [NotMapped()]
	    [ModelDefault("AllowEdit", "False")]
		public DateTime? ReadTime
        { 
		    get => GetPropertyValue<DateTime?>("ReadTime");                         
			set => SetPropertyValue<DateTime?>("ReadTime", value); 
			
        }
		//Tooltip for Object
		public object ReadTimeToolTipControllerText(View view)
        {
        //    if (ReadTime != null) 
		//			return ReadTime;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultReadTime(View view = null)
        { 
			return ReadTime;
        }
		//Set Default Value
		public void SetDefaultReadTime(View view = null)
        {
            //if (ReadTime is null){
            //    var result = GetDefaultReadTime(view);
            //    if (result != null && result != ReadTime){
			//          ReadTime = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ReadTimeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultReadTime();
				//if (result != null && ReadTime != null){
				//	return !ReadTime.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Chat _reply;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trả lời")]
        [ToolTip("Trả lời")]
		//[Index(18)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ReplyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [ImmediatePostData()]
	    [NotMapped()]
	    [Browsable(false)]
	    [NonPersistent()]
		public Module.BusinessObjects.Chat Reply
        { 
		    #region 2874ImportCode 
get; set;
#endregion 2874ImportCode
			
        }
		//Tooltip for Object
		public object ReplyToolTipControllerText(View view)
        {
        //    if (Reply != null) 
		//			return Reply;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Chat GetDefaultReply(View view = null)
        { 
			return Reply;
        }
		//Set Default Value
		public void SetDefaultReply(View view = null)
        {
            //if (Reply is null){
            //    var result = GetDefaultReply(view);
            //    if (result != null && result != Reply){
			//          Reply = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ReplyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultReply();
				//if (result != null && Reply != null){
				//	return !Reply.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ReplyCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Reply));
            }
        }
	
       
		//private Module.BusinessObjects.Chat _modify;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chỉnh sửa")]
        [ToolTip("Chỉnh sửa")]
		//[Index(19)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ModifyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [NonPersistent()]
	    [NotMapped()]
	    [ImmediatePostData()]
	    [Browsable(false)]
		public Module.BusinessObjects.Chat Modify
        { 
		    #region 2875ImportCode 
get; set;
#endregion 2875ImportCode
			
        }
		//Tooltip for Object
		public object ModifyToolTipControllerText(View view)
        {
        //    if (Modify != null) 
		//			return Modify;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Chat GetDefaultModify(View view = null)
        { 
			return Modify;
        }
		//Set Default Value
		public void SetDefaultModify(View view = null)
        {
            //if (Modify is null){
            //    var result = GetDefaultModify(view);
            //    if (result != null && result != Modify){
			//          Modify = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ModifyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultModify();
				//if (result != null && Modify != null){
				//	return !Modify.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ModifyCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Modify));
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
 
            #region 2845ImportCode
            base.AfterConstruction();
SetDefaultMember();
            #endregion 2845ImportCode
            Display = true;
 
        //SetDefaultName(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultPermissionPolicyRole(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultInActive(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultParent(View view = null);
        //SetDefaultChildren(View view = null);
        //SetDefaultUpperFolder(View view = null);
        //SetDefaultNewChat(View view = null);
        //SetDefaultCreateChat(View view = null);
        //SetDefaultReadTime(View view = null);
        //SetDefaultReply(View view = null);
        //SetDefaultModify(View view = null);
			
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
            #region 2840ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 2840ImportCode
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
				
                    case nameof(UpperFolder):
                        OnChangedUpperFolder(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedUpperFolder(object oldValue, object newValue)
        {
            #region 2858ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 2858ImportCode
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
			//	SetDefaultChatList();
			//	SetDefaultMemberList();
			//	SetDefaultLowerFolderList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 2842ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 2842            Oid: 0300661f-806e-4751-b2d3-2950023b6263
            Updater = GetDefaultUpdater();
        }
#endregion 2842ImportCode
#region 2843ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 2843            Oid: a43af915-3f2a-48c8-9965-432ca759a869
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2843ImportCode
#region 2846ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 2846            Oid: 7ffa595e-23d9-47d5-b247-3e28c43efe10
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2846ImportCode
#region 2841ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 2841            Oid: e1d91886-fee8-48e7-a66b-a6e903c3cfbd
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 2841ImportCode
#region 2839ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 2839            Oid: 9c8578b0-5ae4-4c4f-8c08-9b97432e124e
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 2839ImportCode
#region 2844ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 2844            Oid: a10bdb70-d8d9-4fed-aa0a-9803d2137e37
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 2844ImportCode
#region 2856ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 2856            Oid: 065992a0-5862-4fc2-bc46-c017f2df3e49
            if (UpperFolder != null && UpperFolder.LowerFolderList != null)
{
    var lasted = UpperFolder.LowerFolderList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
return null;
        }
#endregion 2856ImportCode
#region 2857ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 2857            Oid: c1c4d389-2d3c-40d6-81c6-81678cc3774d
            Order= GetDefaultOrder();
        }
#endregion 2857ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
