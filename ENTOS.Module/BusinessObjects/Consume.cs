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
    [ModelDefault("Caption", "Tiêu dùng"), ImageName("Consume")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(Manager)+ "," + nameof(Update))]
 
	[MobileColumnAttribute(Context = "Consume_ListView", TargetItems = nameof(Name)+ "," + nameof(StartDate)+ "," + nameof(Cycle))]
	[MobileColumnAttribute(Context = "Consume_LookupListView", TargetItems = nameof(Update)+ "," + nameof(Name)+ "," + nameof(StartDate))]
	[MobileColumnAttribute(Context = "Folder_ConsumeList_ListView", TargetItems = nameof(Name)+ "," + nameof(Update)+ "," + nameof(StartDate))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Consume:  DevExpress.Xpo.XPLiteObject , INewObjectSession , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Consume(Session session)
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
                        //if (Session.FindObject<Module.BusinessObjects.AccountEntry>(CriteriaOperator.Parse("[Consume.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool accountentrylist = Session.Query<Module.BusinessObjects.AccountEntry>().Where(x => x.Consume.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(AccountEntryList), accountentrylist);
                        if (accountentrylist)
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

	
       
		//private string _url;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
        [ToolTip("Liên kết")]
		//[Index(1)]		

 		[Size(1000)]
	    [EditorAlias("FileBrowserPropertyEditor")]
	    [ModelDefault("RowCount","1")]
		public string URL
        { 
		    get => GetPropertyValue<string>("URL");                         
			set => SetPropertyValue<string>("URL", value); 
			
        }
		//Tooltip for Object
		public object URLToolTipControllerText(View view)
        {
        //    if (URL != null) 
		//			return URL;
            return null;
        }
		//Get Default Value
        public string GetDefaultURL(View view = null)
        { 
			return URL;
        }
		//Set Default Value
		public void SetDefaultURL(View view = null)
        {
            //if (URL is null){
            //    var result = GetDefaultURL(view);
            //    if (result != null && result != URL){
			//          URL = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool URLIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultURL();
				//if (result != null && URL != null){
				//	return !URL.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _cycle;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Chu kỳ")]
        [ToolTip("Chu kỳ")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "{0:n1}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Cycle
        { 
		    get => GetPropertyValue<decimal?>("Cycle");                         
			set => SetPropertyValue<decimal?>("Cycle", value); 
			
        }
		//Tooltip for Object
		public object CycleToolTipControllerText(View view)
        {
        //    if (Cycle != null) 
		//			return Cycle;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultCycle(View view = null)
        { 
			return Cycle;
        }
		//Set Default Value
		public void SetDefaultCycle(View view = null)
        {
            //if (Cycle is null){
            //    var result = GetDefaultCycle(view);
            //    if (result != null && result != Cycle){
			//          Cycle = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CycleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCycle();
				//if (result != null && Cycle != null){
				//	return !Cycle.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _price;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giá")]
        [ToolTip("Giá")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Price
        { 
		    get => GetPropertyValue<decimal?>("Price");                         
			set => SetPropertyValue<decimal?>("Price", value); 
			
        }
		//Tooltip for Object
		public object PriceToolTipControllerText(View view)
        {
        //    if (Price != null) 
		//			return Price;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultPrice(View view = null)
        { 
			return Price;
        }
		//Set Default Value
		public void SetDefaultPrice(View view = null)
        {
            //if (Price is null){
            //    var result = GetDefaultPrice(view);
            //    if (result != null && result != Price){
			//          Price = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PriceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPrice();
				//if (result != null && Price != null){
				//	return !Price.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Currency _currency;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tiền")]
        [ToolTip("Tiền")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CurrencyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Currency Currency
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Currency>("Currency");                         
			set => SetPropertyValue<Module.BusinessObjects.Currency>("Currency", value); 
			
        }
		//Tooltip for Object
		public object CurrencyToolTipControllerText(View view)
        {
        //    if (Currency != null) 
		//			return Currency;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Currency GetDefaultCurrency(View view = null)
        { 
			return Currency;
        }
		//Set Default Value
		public void SetDefaultCurrency(View view = null)
        {
            //if (Currency is null){
            //    var result = GetDefaultCurrency(view);
            //    if (result != null && result != Currency){
			//          Currency = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CurrencyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCurrency();
				//if (result != null && Currency != null){
				//	return !Currency.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CurrencyCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Currency));
            }
        }
	
       
		//private Module.BusinessObjects.PaymentAccount _paymentaccount;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thanh toán")]
        [ToolTip("Thanh toán")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PaymentAccountCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.PaymentAccount PaymentAccount
        { 
		    get => GetPropertyValue<Module.BusinessObjects.PaymentAccount>("PaymentAccount");                         
			set => SetPropertyValue<Module.BusinessObjects.PaymentAccount>("PaymentAccount", value); 
			
        }
		//Tooltip for Object
		public object PaymentAccountToolTipControllerText(View view)
        {
        //    if (PaymentAccount != null) 
		//			return PaymentAccount;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.PaymentAccount GetDefaultPaymentAccount(View view = null)
        { 
			return PaymentAccount;
        }
		//Set Default Value
		public void SetDefaultPaymentAccount(View view = null)
        {
            //if (PaymentAccount is null){
            //    var result = GetDefaultPaymentAccount(view);
            //    if (result != null && result != PaymentAccount){
			//          PaymentAccount = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PaymentAccountIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPaymentAccount();
				//if (result != null && PaymentAccount != null){
				//	return !PaymentAccount.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PaymentAccountCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PaymentAccount));
            }
        }
	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
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

	
       
		//private DateTime? _startdate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Bắt đầu")]
        [ToolTip("Bắt đầu")]
		//[Index(7)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy H:mm")]
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

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thành viên")]
        [ToolTip("Thành viên")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MemberCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [ModelDefault("AllowEdit", "False")]
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
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(9)]		
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
	
       
		//private Module.BusinessObjects.Member _manager;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(10)]		
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
		//Set Default Value

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
	
       
		//private bool _subscription;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thuê bao")]
        [ToolTip("Thuê bao")]
		//[Index(11)]		
		public bool Subscription
        { 
		    get => GetPropertyValue<bool>("Subscription");                         
			set => SetPropertyValue<bool>("Subscription", value); 
			
        }
		//Tooltip for Object
		public object SubscriptionToolTipControllerText(View view)
        {
        //    if (Subscription != null) 
		//			return Subscription;
            return null;
        }
		//Get Default Value
        public bool GetDefaultSubscription(View view = null)
        { 
			return Subscription;
        }
		//Set Default Value
		public void SetDefaultSubscription(View view = null)
        {
            //if (Subscription is null){
            //    var result = GetDefaultSubscription(view);
            //    if (result != null && result != Subscription){
			//          Subscription = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SubscriptionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSubscription();
				//if (result != null && Subscription != null){
				//	return !Subscription.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hạch toán")]
		//[Index(12)]
		[DevExpress.Xpo.Association("Consume-AccountEntryList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.AccountEntry> AccountEntryList
        {      
		    get => GetCollection<Module.BusinessObjects.AccountEntry>("AccountEntryList"); 
			
        }
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
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

	
       
		//private Module.BusinessObjects.LoginAccount _loginaccount;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đăng nhập")]
        [ToolTip("Đăng nhập")]
		//[Index(14)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LoginAccountCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.LoginAccount LoginAccount
        { 
		    get => GetPropertyValue<Module.BusinessObjects.LoginAccount>("LoginAccount");                         
			set => SetPropertyValue<Module.BusinessObjects.LoginAccount>("LoginAccount", value); 
			
        }
		//Tooltip for Object
		public object LoginAccountToolTipControllerText(View view)
        {
        //    if (LoginAccount != null) 
		//			return LoginAccount;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.LoginAccount GetDefaultLoginAccount(View view = null)
        { 
			return LoginAccount;
        }
		//Set Default Value
		public void SetDefaultLoginAccount(View view = null)
        {
            //if (LoginAccount is null){
            //    var result = GetDefaultLoginAccount(view);
            //    if (result != null && result != LoginAccount){
			//          LoginAccount = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LoginAccountIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLoginAccount();
				//if (result != null && LoginAccount != null){
				//	return !LoginAccount.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LoginAccountCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(LoginAccount));
            }
        }
	
       
		//private Module.BusinessObjects.Folder _folder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(15)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#Consume# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-ConsumeList")]
	 
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
	
       
		//private decimal? _unitamount;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Đơn kỳ VNĐ")]
        [ToolTip("Đơn kỳ VNĐ")]
		//[Index(16)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
	    [NotMapped()]
	    [NonPersistent()]
		public decimal? UnitAmount
        { 
		    #region 1431ImportCode 
get{
	if(Price  != null && Currency != null && Cycle != null)
		return Price * Currency?.ExchangeRate / Cycle;
	return null;
}

#endregion 1431ImportCode
			
        }
		//Tooltip for Object
		public object UnitAmountToolTipControllerText(View view)
        {
        //    if (UnitAmount != null) 
		//			return UnitAmount;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultUnitAmount(View view = null)
        { 
			return UnitAmount;
        }
		//Set Default Value
		public void SetDefaultUnitAmount(View view = null)
        {
            //if (UnitAmount is null){
            //    var result = GetDefaultUnitAmount(view);
            //    if (result != null && result != UnitAmount){
			//          UnitAmount = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UnitAmountIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUnitAmount();
				//if (result != null && UnitAmount != null){
				//	return !UnitAmount.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _close;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Riêng tư")]
        [ToolTip("Riêng tư")]
		//[Index(17)]		
		public bool Close
        { 
		    get => GetPropertyValue<bool>("Close");                         
			set => SetPropertyValue<bool>("Close", value); 
			
        }
		//Tooltip for Object
		public object CloseToolTipControllerText(View view)
        {
        //    if (Close != null) 
		//			return Close;
            return null;
        }
		//Get Default Value
        public bool GetDefaultClose(View view = null)
        { 
			return Close;
        }
		//Set Default Value
		public void SetDefaultClose(View view = null)
        {
            //if (Close is null){
            //    var result = GetDefaultClose(view);
            //    if (result != null && result != Close){
			//          Close = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CloseIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultClose();
				//if (result != null && Close != null){
				//	return !Close.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole _permissionpolicyrole;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhóm")]
        [ToolTip("Nhóm")]
		//[Index(18)]		
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
	
       
		//private DateTime? _enddate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kết thúc")]
        [ToolTip("Kết thúc")]
		//[Index(19)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? EndDate
        { 
		    get => GetPropertyValue<DateTime?>("EndDate");                         
			set => SetPropertyValue<DateTime?>("EndDate", value); 
			
        }
		//Tooltip for Object
		public object EndDateToolTipControllerText(View view)
        {
        //    if (EndDate != null) 
		//			return EndDate;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultEndDate(View view = null)
        { 
			return EndDate;
        }
		//Set Default Value
		public void SetDefaultEndDate(View view = null)
        {
            //if (EndDate is null){
            //    var result = GetDefaultEndDate(view);
            //    if (result != null && result != EndDate){
			//          EndDate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EndDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEndDate();
				//if (result != null && EndDate != null){
				//	return !EndDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(20)]		
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
		[DevExpress.Xpo.DisplayName("Số thống kê")]
        [ToolTip("Số thống kê")]
		//[Index(21)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Statistic
        { 
		    #region 1441ImportCode 
get
{
    if (Price != null && Currency != null && Cycle != null)
        return Price.Value * Currency.ExchangeRate / Cycle;
    return null;
}
#endregion 1441ImportCode
			
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 1427ImportCode
            base.AfterConstruction();
SetDefaultManager();
SetDefaultMember();
            #endregion 1427ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultURL(View view = null);
        //SetDefaultCycle(View view = null);
        //SetDefaultPrice(View view = null);
        //SetDefaultCurrency(View view = null);
        //SetDefaultPaymentAccount(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultStartDate(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultManager(View view = null);
        //SetDefaultSubscription(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultLoginAccount(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultUnitAmount(View view = null);
        //SetDefaultClose(View view = null);
        //SetDefaultPermissionPolicyRole(View view = null);
        //SetDefaultEndDate(View view = null);
        //SetDefaultInActive(View view = null);
        //SetDefaultStatistic(View view = null);
			
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
            #region 1422ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 1422ImportCode
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
			//	SetDefaultAccountEntryList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1428ImportCode
		public Module.BusinessObjects.Member GetDefaultManager(View view = null)
        {
            //Code: 1428            Oid: a7a79645-ee3f-4907-b428-410048c28ee1
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 1428ImportCode
#region 1436ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 1436            Oid: ff48ac13-7b3d-4578-a532-283606c77453
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);

        }
#endregion 1436ImportCode
#region 1423ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 1423            Oid: a78aa82b-9027-400a-b95a-8d0d00d5ac7c
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1423ImportCode
#region 1426ImportCode
		public void SetDefaultManager(View view = null)
        {
            //Code: 1426            Oid: 4585533e-c2ed-44c8-b20a-b5368874e323
            if(Manager == null) Member = GetDefaultManager();
        }
#endregion 1426ImportCode
#region 1421ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 1421            Oid: b408be4d-8be0-4a5c-8c57-5a3a102780de
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 1421ImportCode
#region 1435ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 1435            Oid: ebe71d81-e938-43e2-889b-e32d5c61af65
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 1435ImportCode
        #endregion
//Mã nguồn bổ sung
#region ConsumeImportCode
        private bool IsCurrentUserEqualUpperMember(Member member)
        {
            if (member != null)
            {
                if (SecuritySystem.CurrentUserId.Equals(member.Oid))
                    return true;
                return IsCurrentUserEqualUpperMember(member.Manager);
            }
            return false;
        }	
#endregion ConsumeImportCode
		 		 
    }
}
