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
    [ModelDefault("Caption", "Bút toán"), ImageName("AccountEntry")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Date)+ "," + nameof(MemberFolder)+ "," + nameof(Member)+ "," + nameof(Update)+ "," + nameof(Updater)+ "," + nameof(Creator))]
 
	[MobileColumnAttribute(Context = "AccountEntry_LookupListView", TargetItems = nameof(Update)+ "," + nameof(Date))]
	[MobileColumnAttribute(Context = "EntryFolder_PartyAccountList_ListView", TargetItems = nameof(Amount)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "EntryFolder_AllAccountEntryList_ListView", TargetItems = nameof(Amount)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "AccountEntry_ListView", TargetItems = nameof(Amount)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Depreciation_AccountEntries_ListView", TargetItems = nameof(Date)+ "," + nameof(Update))]
	[MobileColumnAttribute(Context = "Asset_AccountEntryList_ListView", TargetItems = nameof(Debit)+ "," + nameof(Date)+ "," + nameof(Amount))]
	[MobileColumnAttribute(Context = "Consume_AccountEntryList_ListView", TargetItems = nameof(Date)+ "," + nameof(Update))]
	[MobileColumnAttribute(Context = "EntryFolder_AccountEntryList_ListView", TargetItems = nameof(Amount)+ "," + nameof(Name))]
	[DefaultProperty("EntryFolder")]
 
[OptimisticLocking(true)]
    public partial class AccountEntry:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public AccountEntry(Session session)
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
		[DevExpress.Xpo.DisplayName("Diễn giải")]
        [ToolTip("Diễn giải")]
		//[Index(0)]		

 		[Size(100)]
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

	
       
		//private decimal? _amount;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giá trị")]
        [ToolTip("Giá trị")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Amount
        { 
		    get => GetPropertyValue<decimal?>("Amount");                         
			set => SetPropertyValue<decimal?>("Amount", value); 
			
        }
		//Tooltip for Object
		public object AmountToolTipControllerText(View view)
        {
        //    if (Amount != null) 
		//			return Amount;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultAmount(View view = null)
        { 
			return Amount;
        }
		//Set Default Value
		public void SetDefaultAmount(View view = null)
        {
            //if (Amount is null){
            //    var result = GetDefaultAmount(view);
            //    if (result != null && result != Amount){
			//          Amount = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AmountIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAmount();
				//if (result != null && Amount != null){
				//	return !Amount.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.EntryFolder _entryfolder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài khoản kế toán")]
        [ToolTip("Tài khoản kế toán")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(EntryFolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("EntryFolder-AccountEntryList")]
	 
		[RuleRequiredField("RequiredAccountEntryEntryFolder", DefaultContexts.Save)]
		public Module.BusinessObjects.EntryFolder EntryFolder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.EntryFolder>("EntryFolder");                         
			set => SetPropertyValue<Module.BusinessObjects.EntryFolder>("EntryFolder", value); 
			
        }
		//Tooltip for Object
		public object EntryFolderToolTipControllerText(View view)
        {
        //    if (EntryFolder != null) 
		//			return EntryFolder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.EntryFolder GetDefaultEntryFolder(View view = null)
        { 
			return EntryFolder;
        }
		//Set Default Value
		public void SetDefaultEntryFolder(View view = null)
        {
            //if (EntryFolder is null){
            //    var result = GetDefaultEntryFolder(view);
            //    if (result != null && result != EntryFolder){
			//          EntryFolder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EntryFolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEntryFolder();
				//if (result != null && EntryFolder != null){
				//	return !EntryFolder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator EntryFolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(EntryFolder));
            }
        }
	
       
		//private Module.BusinessObjects.EntryFolder _partyaccountfolder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài khoản đối ứng")]
        [ToolTip("Tài khoản đối ứng")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PartyAccountFolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("PartyAccountFolder-PartyAccountList")]
	 
		[RuleRequiredField("RequiredAccountEntryPartyAccountFolder", DefaultContexts.Save)]
		public Module.BusinessObjects.EntryFolder PartyAccountFolder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.EntryFolder>("PartyAccountFolder");                         
			set => SetPropertyValue<Module.BusinessObjects.EntryFolder>("PartyAccountFolder", value); 
			
        }
		//Tooltip for Object
		public object PartyAccountFolderToolTipControllerText(View view)
        {
        //    if (PartyAccountFolder != null) 
		//			return PartyAccountFolder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.EntryFolder GetDefaultPartyAccountFolder(View view = null)
        { 
			return PartyAccountFolder;
        }
		//Set Default Value
		public void SetDefaultPartyAccountFolder(View view = null)
        {
            //if (PartyAccountFolder is null){
            //    var result = GetDefaultPartyAccountFolder(view);
            //    if (result != null && result != PartyAccountFolder){
			//          PartyAccountFolder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PartyAccountFolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPartyAccountFolder();
				//if (result != null && PartyAccountFolder != null){
				//	return !PartyAccountFolder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PartyAccountFolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PartyAccountFolder));
            }
        }
	
       
		//private bool _debit;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ghi nợ")]
        [ToolTip("Ghi nợ")]
		//[Index(4)]		
		public bool Debit
        { 
		    get => GetPropertyValue<bool>("Debit");                         
			set => SetPropertyValue<bool>("Debit", value); 
			
        }
		//Tooltip for Object
		public object DebitToolTipControllerText(View view)
        {
        //    if (Debit != null) 
		//			return Debit;
            return null;
        }
		//Get Default Value
        public bool GetDefaultDebit(View view = null)
        { 
			return Debit;
        }
		//Set Default Value
		public void SetDefaultDebit(View view = null)
        {
            //if (Debit is null){
            //    var result = GetDefaultDebit(view);
            //    if (result != null && result != Debit){
			//          Debit = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DebitIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDebit();
				//if (result != null && Debit != null){
				//	return !Debit.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _date;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? Date
        { 
		    get => GetPropertyValue<DateTime?>("Date");                         
			set => SetPropertyValue<DateTime?>("Date", value); 
			
        }
		//Tooltip for Object
		public object DateToolTipControllerText(View view)
        {
        //    if (Date != null) 
		//			return Date;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool DateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDate();
				//if (result != null && Date != null){
				//	return !Date.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Folder _memberfolder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(6)]		
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
	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thành viên")]
        [ToolTip("Thành viên")]
		//[Index(7)]		
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
	
       
		//private string _link;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
        [ToolTip("Liên kết")]
		//[Index(8)]		

 		[Size(1000)]
	    [EditorAlias("FileBrowserPropertyEditor")]
	    [ModelDefault("RowCount","1")]
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

	
       
		//private bool _book1;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thuế")]
        [ToolTip("Thuế")]
		//[Index(9)]		
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
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Nội bộ")]
        [ToolTip("Nội bộ")]
		//[Index(10)]		
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

	
       
		//private Module.BusinessObjects.Order _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đơn hàng")]
        [ToolTip("Đơn hàng")]
		//[Index(12)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OrderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Order-AccountEntryList")]
	 
		public Module.BusinessObjects.Order Order
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Order>("Order");                         
			set => SetPropertyValue<Module.BusinessObjects.Order>("Order", value); 
			
        }
		//Tooltip for Object
		public object OrderToolTipControllerText(View view)
        {
        //    if (Order != null) 
		//			return Order;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Order GetDefaultOrder(View view = null)
        { 
			return Order;
        }
		//Set Default Value
		public void SetDefaultOrder(View view = null)
        {
            //if (Order is null){
            //    var result = GetDefaultOrder(view);
            //    if (result != null && result != Order){
			//          Order = result;
            //	  }
            //}
        }

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

		private CriteriaOperator OrderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Order));
            }
        }
	
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(14)]		
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

	
       
		//private decimal? _statistic;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thống kê")]
        [ToolTip("Thống kê")]
		//[Index(15)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Statistic
        { 
		    #region 1508ImportCode 
get
{
    if (Amount != null)
		if(Debit&&EntryFolder.EntryType == EntryType.Debit)
        		return Amount;
			else return -Amount;
    return null;
}

#endregion 1508ImportCode
			
        }
		//Tooltip for Object
		public object StatisticToolTipControllerText(View view)
        {
        //    if (Statistic != null) 
		//			return Statistic;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultStatistic(View view = null)
        { 
			return Statistic;
        }
		//Set Default Value
		public void SetDefaultStatistic(View view = null)
        {
            //if (Statistic is null){
            //    var result = GetDefaultStatistic(view);
            //    if (result != null && result != Statistic){
			//          Statistic = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StatisticIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStatistic();
				//if (result != null && Statistic != null){
				//	return !Statistic.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _partystatistic;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("TK đối ứng")]
        [ToolTip("TK đối ứng")]
		//[Index(16)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? PartyStatistic
        { 
		    #region 1509ImportCode 
get
{
    if (Amount != null)
		if(Debit&& PartyAccountFolder.EntryType == EntryType.Debit)
        		return -Amount;
			else return Amount;
    return null;
}

#endregion 1509ImportCode
			
        }
		//Tooltip for Object
		public object PartyStatisticToolTipControllerText(View view)
        {
        //    if (PartyStatistic != null) 
		//			return PartyStatistic;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultPartyStatistic(View view = null)
        { 
			return PartyStatistic;
        }
		//Set Default Value
		public void SetDefaultPartyStatistic(View view = null)
        {
            //if (PartyStatistic is null){
            //    var result = GetDefaultPartyStatistic(view);
            //    if (result != null && result != PartyStatistic){
			//          PartyStatistic = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PartyStatisticIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPartyStatistic();
				//if (result != null && PartyStatistic != null){
				//	return !PartyStatistic.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(17)]		
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
		//[Index(18)]		
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
	
       
		//private Module.BusinessObjects.Member _creator;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Người tạo")]
        [ToolTip("Người tạo")]
		//[Index(19)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CreatorCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [ModelDefault("AllowEdit", "False")]
		public Module.BusinessObjects.Member Creator
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Creator");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Creator", value); 
			
        }
		//Tooltip for Object
		public object CreatorToolTipControllerText(View view)
        {
        //    if (Creator != null) 
		//			return Creator;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool CreatorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCreator();
				//if (result != null && Creator != null){
				//	return !Creator.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CreatorCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Creator));
            }
        }
	
       
		//private decimal? _amountdebit;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bên nợ")]
        [ToolTip("Bên nợ")]
		//[Index(20)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
	    [NotMapped()]
	    [NonPersistent()]
		public decimal? AmountDebit
        { 
		    get => GetPropertyValue<decimal?>("AmountDebit");                         
			set => SetPropertyValue<decimal?>("AmountDebit", value); 
			
        }
		//Tooltip for Object
		public object AmountDebitToolTipControllerText(View view)
        {
        //    if (AmountDebit != null) 
		//			return AmountDebit;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultAmountDebit(View view = null)
        { 
			return AmountDebit;
        }
		//Set Default Value
		public void SetDefaultAmountDebit(View view = null)
        {
            //if (AmountDebit is null){
            //    var result = GetDefaultAmountDebit(view);
            //    if (result != null && result != AmountDebit){
			//          AmountDebit = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AmountDebitIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAmountDebit();
				//if (result != null && AmountDebit != null){
				//	return !AmountDebit.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _amountcredit;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bên có")]
        [ToolTip("Bên có")]
		//[Index(21)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
	    [NonPersistent()]
	    [NotMapped()]
		public decimal? AmountCredit
        { 
		    get => GetPropertyValue<decimal?>("AmountCredit");                         
			set => SetPropertyValue<decimal?>("AmountCredit", value); 
			
        }
		//Tooltip for Object
		public object AmountCreditToolTipControllerText(View view)
        {
        //    if (AmountCredit != null) 
		//			return AmountCredit;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultAmountCredit(View view = null)
        { 
			return AmountCredit;
        }
		//Set Default Value
		public void SetDefaultAmountCredit(View view = null)
        {
            //if (AmountCredit is null){
            //    var result = GetDefaultAmountCredit(view);
            //    if (result != null && result != AmountCredit){
			//          AmountCredit = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AmountCreditIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAmountCredit();
				//if (result != null && AmountCredit != null){
				//	return !AmountCredit.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Consume _consume;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tiêu dùng")]
        [ToolTip("Tiêu dùng")]
		//[Index(22)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ConsumeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Consume-AccountEntryList")]
	 
		public Module.BusinessObjects.Consume Consume
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Consume>("Consume");                         
			set => SetPropertyValue<Module.BusinessObjects.Consume>("Consume", value); 
			
        }
		//Tooltip for Object
		public object ConsumeToolTipControllerText(View view)
        {
        //    if (Consume != null) 
		//			return Consume;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Consume GetDefaultConsume(View view = null)
        { 
			return Consume;
        }
		//Set Default Value
		public void SetDefaultConsume(View view = null)
        {
            //if (Consume is null){
            //    var result = GetDefaultConsume(view);
            //    if (result != null && result != Consume){
			//          Consume = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ConsumeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultConsume();
				//if (result != null && Consume != null){
				//	return !Consume.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ConsumeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Consume));
            }
        }
	
       
		//private Module.BusinessObjects.Asset _asset;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài sản")]
        [ToolTip("Tài sản")]
		//[Index(23)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(AssetCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Asset-AccountEntryList")]
	 
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
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 1520ImportCode
            base.AfterConstruction();
SetDefaultDate();
SetDefaultCreator();
            #endregion 1520ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultAmount(View view = null);
        //SetDefaultEntryFolder(View view = null);
        //SetDefaultPartyAccountFolder(View view = null);
        //SetDefaultDebit(View view = null);
        //SetDefaultDate(View view = null);
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultLink(View view = null);
        //SetDefaultBook1(View view = null);
        //SetDefaultBook2(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultInActive(View view = null);
        //SetDefaultStatistic(View view = null);
        //SetDefaultPartyStatistic(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultCreator(View view = null);
        //SetDefaultAmountDebit(View view = null);
        //SetDefaultAmountCredit(View view = null);
        //SetDefaultConsume(View view = null);
        //SetDefaultAsset(View view = null);
			
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
            #region 1513ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 1513ImportCode
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
				
                    case nameof(Asset):
                        OnChangedAsset(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedAsset(object oldValue, object newValue)
        {
            #region 2822ImportCode
            if (newValue is null) return;
SetDefaultFromParentMemberFolder();            
            #endregion 2822ImportCode
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
#region 1512ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 1512            Oid: 45773021-bacd-4bff-af68-3f2a62d616b2
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 1512ImportCode
#region 1514ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 1514            Oid: 9a769021-b579-45f0-9b35-9672ba6e7e94
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1514ImportCode
#region 1543ImportCode
		public Module.BusinessObjects.Member GetDefaultCreator(View view = null)
        {
            //Code: 1543            Oid: 7fddf011-ed62-4b86-bb95-fce2db383a38
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 1543ImportCode
#region 1516ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 1516            Oid: 3fe22c5c-d299-4650-90cf-8ff5142bfcaf
            Updater = GetDefaultUpdater();
        }
#endregion 1516ImportCode
#region 1542ImportCode
		public void SetDefaultCreator(View view = null)
        {
            //Code: 1542            Oid: e4ac4b44-4768-4f3b-b460-7a6a9b307ece
            if(Creator == null) Creator = GetDefaultCreator();
        }
#endregion 1542ImportCode
#region 1511ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 1511            Oid: a04e44a5-8be2-4e18-a0a9-04481dbabeff
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 1511ImportCode
#region 2820ImportCode
		public Module.BusinessObjects.Folder GetDefaultFromParentMemberFolder(View view = null)
        {
            //Code: 2820            Oid: 39dae0ea-4032-48da-8414-1779c82e21ec
            return Asset?.MemberFolder;
return Consume?.MemberFolder;
        }
#endregion 2820ImportCode
#region 1517ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 1517            Oid: 9edf5577-d711-4336-b5a9-92fcb1a6b239
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 1517ImportCode
#region 1519ImportCode
		public void SetDefaultDate(View view = null)
        {
            //Code: 1519            Oid: 54334ac9-b9b9-4c97-a059-ab6c7e483cf2
            if(Date == null) Date = GetDefaultDate();
        }
#endregion 1519ImportCode
#region 1518ImportCode
		public DateTime? GetDefaultDate(View view = null)
        {
            //Code: 1518            Oid: 96a7d4ec-4528-49ed-8083-7fa760c25c7d
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1518ImportCode
#region 1510ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 1510            Oid: ce2c51da-220f-4190-988f-4d99b29a4e88
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 1510ImportCode
#region 2821ImportCode
		public void SetDefaultFromParentMemberFolder(View view = null)
        {
            //Code: 2821            Oid: c914a94e-8f40-49d2-890a-774611549352
            if(MemberFolder == null) MemberFolder = GetDefaultFromParentMemberFolder();
        }
#endregion 2821ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
