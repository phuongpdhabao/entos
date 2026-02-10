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
	[NavigationItem("Business")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Đơn hàng"), ImageName("Order")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("Order Code Hide_None__" , TargetItems = "Code" , Criteria = "IsNullOrEmpty([Code])",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(Currency)+ "," + nameof(ExchangeRate)+ "," + nameof(Round)+ "," + nameof(PricingMethod)+ "," + nameof(Margin)+ "," + nameof(Disccount)+ "," + nameof(DocumentCode)+ "," + nameof(DocumentDate)+ "," + nameof(Book1)+ "," + nameof(Book2)+ "," + nameof(PartnerContact)+ "," + nameof(Folder), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Code)+ "," + nameof(Member)+ "," + nameof(Amount)+ "," + nameof(VAT)+ "," + nameof(Update)+ "," + nameof(SoftwareObjectType))]
 
	[MobileColumnAttribute(Context = "Order_LookupListView", TargetItems = nameof(Code)+ "," + nameof(Date))]
	[MobileColumnAttribute(Context = "Order_ListView", TargetItems = nameof(PartnerOrg)+ "," + nameof(Code)+ "," + nameof(Date))]
	[DefaultProperty("Code")]
 
[OptimisticLocking(true)]
    public partial class Order:  DevExpress.Xpo.XPLiteObject , IWork , IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public Order(Session session)
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
				if (OrderDetailList.IsLoaded)
                {
                    if (OrderDetailList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(OrderDetailList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(OrderDetailList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.OrderDetail>(CriteriaOperator.Parse("[Order.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool orderdetaillist = Session.Query<Module.BusinessObjects.OrderDetail>().Where(x => x.Order.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(OrderDetailList), orderdetaillist);
                        if (orderdetaillist)
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
                        //if (Session.FindObject<Module.BusinessObjects.AccountEntry>(CriteriaOperator.Parse("[Order.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool accountentrylist = Session.Query<Module.BusinessObjects.AccountEntry>().Where(x => x.Order.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
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
               

		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Đơn hàng")]
        [ToolTip("Đơn hàng")]
		//[Index(0)]		

 		[Size(100)]
	    [RuleUniqueValue("Order.Code.Unique", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, TargetCriteria = "CodeUniqueDate")]
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
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(1)]		

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

	
       
		//private string _partner;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đối tác")]
        [ToolTip("Đối tác")]
		//[Index(2)]		

 		[Size(250)]
		public string Partner
        { 
		    get => GetPropertyValue<string>("Partner");                         
			set => SetPropertyValue<string>("Partner", value); 
			
        }
		//Tooltip for Object
		public object PartnerToolTipControllerText(View view)
        {
        //    if (Partner != null) 
		//			return Partner;
            return null;
        }
		//Get Default Value
        public string GetDefaultPartner(View view = null)
        { 
			return Partner;
        }
		//Set Default Value
		public void SetDefaultPartner(View view = null)
        {
            //if (Partner is null){
            //    var result = GetDefaultPartner(view);
            //    if (result != null && result != Partner){
			//          Partner = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PartnerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPartner();
				//if (result != null && Partner != null){
				//	return !Partner.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
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
	
       
		//private Module.BusinessObjects.Currency _currency;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
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
	
       
		//private decimal? _exchangerate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tỉ giá")]
        [ToolTip("Tỉ giá")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? ExchangeRate
        { 
		    get => GetPropertyValue<decimal?>("ExchangeRate");                         
			set => SetPropertyValue<decimal?>("ExchangeRate", value); 
			
        }
		//Tooltip for Object
		public object ExchangeRateToolTipControllerText(View view)
        {
        //    if (ExchangeRate != null) 
		//			return ExchangeRate;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultExchangeRate(View view = null)
        { 
			return ExchangeRate;
        }
		//Set Default Value
		public void SetDefaultExchangeRate(View view = null)
        {
            //if (ExchangeRate is null){
            //    var result = GetDefaultExchangeRate(view);
            //    if (result != null && result != ExchangeRate){
			//          ExchangeRate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ExchangeRateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultExchangeRate();
				//if (result != null && ExchangeRate != null){
				//	return !ExchangeRate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Round _round;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Làm tròn")]
        [ToolTip("Làm tròn")]
		//[Index(6)]		
		public Round Round
        { 
		    get => GetPropertyValue<Round>("Round");                         
			set => SetPropertyValue<Round>("Round", value); 
			
        }
		//Tooltip for Object
		public object RoundToolTipControllerText(View view)
        {
        //    if (Round != null) 
		//			return Round;
            return null;
        }
		//Get Default Value
        public Round GetDefaultRound(View view = null)
        { 
			return Round;
        }
		//Set Default Value
		public void SetDefaultRound(View view = null)
        {
            //if (Round is null){
            //    var result = GetDefaultRound(view);
            //    if (result != null && result != Round){
			//          Round = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RoundIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRound();
				//if (result != null && Round != null){
				//	return !Round.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Status _status;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Trạng thái")]
        [ToolTip("Trạng thái")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(StatusCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Status Status
        { 
		    get => GetPropertyValue<Status>("Status");                         
			set => SetPropertyValue<Status>("Status", value); 
			
        }
		//Tooltip for Object
		public object StatusToolTipControllerText(View view)
        {
        //    if (Status != null) 
		//			return Status;
            return null;
        }
		//Get Default Value
        public Status GetDefaultStatus(View view = null)
        { 
			return Status;
        }
		//Set Default Value
		public void SetDefaultStatus(View view = null)
        {
            //if (Status is null){
            //    var result = GetDefaultStatus(view);
            //    if (result != null && result != Status){
			//          Status = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StatusIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStatus();
				//if (result != null && Status != null){
				//	return !Status.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator StatusCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Status));
            }
        }
	
       
		//private DateTime? _date;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M")]
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
        public DateTime? GetDefaultDate(View view = null)
        { 
			return Date;
        }
		//Set Default Value
		public void SetDefaultDate(View view = null)
        {
            //if (Date is null){
            //    var result = GetDefaultDate(view);
            //    if (result != null && result != Date){
			//          Date = result;
            //	  }
            //}
        }

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

	
       
		//private string _pricingmethod;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tính theo")]
        [ToolTip("Tính theo")]
		//[Index(12)]		

 		[Size(100)]
		public string PricingMethod
        { 
		    get => GetPropertyValue<string>("PricingMethod");                         
			set => SetPropertyValue<string>("PricingMethod", value); 
			
        }
		//Tooltip for Object
		public object PricingMethodToolTipControllerText(View view)
        {
        //    if (PricingMethod != null) 
		//			return PricingMethod;
            return null;
        }
		//Get Default Value
        public string GetDefaultPricingMethod(View view = null)
        { 
			return PricingMethod;
        }
		//Set Default Value
		public void SetDefaultPricingMethod(View view = null)
        {
            //if (PricingMethod is null){
            //    var result = GetDefaultPricingMethod(view);
            //    if (result != null && result != PricingMethod){
			//          PricingMethod = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PricingMethodIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPricingMethod();
				//if (result != null && PricingMethod != null){
				//	return !PricingMethod.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _margin;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lợi nhuận")]
        [ToolTip("Lợi nhuận")]
		//[Index(13)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p2")]
		public decimal? Margin
        { 
		    get => GetPropertyValue<decimal?>("Margin");                         
			set => SetPropertyValue<decimal?>("Margin", value); 
			
        }
		//Tooltip for Object
		public object MarginToolTipControllerText(View view)
        {
        //    if (Margin != null) 
		//			return Margin;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultMargin(View view = null)
        { 
			return Margin;
        }
		//Set Default Value
		public void SetDefaultMargin(View view = null)
        {
            //if (Margin is null){
            //    var result = GetDefaultMargin(view);
            //    if (result != null && result != Margin){
			//          Margin = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MarginIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMargin();
				//if (result != null && Margin != null){
				//	return !Margin.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _disccount;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chiết khấu")]
        [ToolTip("Chiết khấu")]
		//[Index(14)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p2")]
		public decimal? Disccount
        { 
		    get => GetPropertyValue<decimal?>("Disccount");                         
			set => SetPropertyValue<decimal?>("Disccount", value); 
			
        }
		//Tooltip for Object
		public object DisccountToolTipControllerText(View view)
        {
        //    if (Disccount != null) 
		//			return Disccount;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultDisccount(View view = null)
        { 
			return Disccount;
        }
		//Set Default Value
		public void SetDefaultDisccount(View view = null)
        {
            //if (Disccount is null){
            //    var result = GetDefaultDisccount(view);
            //    if (result != null && result != Disccount){
			//          Disccount = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DisccountIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDisccount();
				//if (result != null && Disccount != null){
				//	return !Disccount.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _documentcode;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chứng từ")]
        [ToolTip("Chứng từ")]
		//[Index(15)]		

 		[Size(100)]
		public string DocumentCode
        { 
		    get => GetPropertyValue<string>("DocumentCode");                         
			set => SetPropertyValue<string>("DocumentCode", value); 
			
        }
		//Tooltip for Object
		public object DocumentCodeToolTipControllerText(View view)
        {
        //    if (DocumentCode != null) 
		//			return DocumentCode;
            return null;
        }
		//Get Default Value
        public string GetDefaultDocumentCode(View view = null)
        { 
			return DocumentCode;
        }
		//Set Default Value
		public void SetDefaultDocumentCode(View view = null)
        {
            //if (DocumentCode is null){
            //    var result = GetDefaultDocumentCode(view);
            //    if (result != null && result != DocumentCode){
			//          DocumentCode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DocumentCodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDocumentCode();
				//if (result != null && DocumentCode != null){
				//	return !DocumentCode.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _documentdate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày CT")]
        [ToolTip("Ngày CT")]
		//[Index(16)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? DocumentDate
        { 
		    get => GetPropertyValue<DateTime?>("DocumentDate");                         
			set => SetPropertyValue<DateTime?>("DocumentDate", value); 
			
        }
		//Tooltip for Object
		public object DocumentDateToolTipControllerText(View view)
        {
        //    if (DocumentDate != null) 
		//			return DocumentDate;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultDocumentDate(View view = null)
        { 
			return DocumentDate;
        }
		//Set Default Value
		public void SetDefaultDocumentDate(View view = null)
        {
            //if (DocumentDate is null){
            //    var result = GetDefaultDocumentDate(view);
            //    if (result != null && result != DocumentDate){
			//          DocumentDate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DocumentDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDocumentDate();
				//if (result != null && DocumentDate != null){
				//	return !DocumentDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _book1;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sổ 1")]
        [ToolTip("Sổ 1")]
		//[Index(17)]		
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
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sổ 2")]
        [ToolTip("Sổ 2")]
		//[Index(18)]		
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

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chi tiết")]
		//[Index(19)]
		[DevExpress.Xpo.Association("Order-OrderDetailList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.OrderDetail> OrderDetailList
        {      
		    get => GetCollection<Module.BusinessObjects.OrderDetail>("OrderDetailList"); 
			
        }
       
		//private string _note;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(20)]		

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

	
       
		//private string _conditionpaymentdetaillist;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Công nợ")]
        [ToolTip("Công nợ")]
		//[Index(21)]		

 		[Size(SizeAttribute.Unlimited)]
		public string ConditionPaymentDetailList
        { 
		    get => GetPropertyValue<string>("ConditionPaymentDetailList");                         
			set => SetPropertyValue<string>("ConditionPaymentDetailList", value); 
			
        }
		//Tooltip for Object
		public object ConditionPaymentDetailListToolTipControllerText(View view)
        {
        //    if (ConditionPaymentDetailList != null) 
		//			return ConditionPaymentDetailList;
            return null;
        }
		//Get Default Value
        public string GetDefaultConditionPaymentDetailList(View view = null)
        { 
			return ConditionPaymentDetailList;
        }
		//Set Default Value
		public void SetDefaultConditionPaymentDetailList(View view = null)
        {
            //if (ConditionPaymentDetailList is null){
            //    var result = GetDefaultConditionPaymentDetailList(view);
            //    if (result != null && result != ConditionPaymentDetailList){
			//          ConditionPaymentDetailList = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ConditionPaymentDetailListIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultConditionPaymentDetailList();
				//if (result != null && ConditionPaymentDetailList != null){
				//	return !ConditionPaymentDetailList.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hạch toán")]
		//[Index(24)]
		[DevExpress.Xpo.Association("Order-AccountEntryList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.AccountEntry> AccountEntryList
        {      
		    get => GetCollection<Module.BusinessObjects.AccountEntry>("AccountEntryList"); 
			
        }
       
		//private decimal? _amount;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thành tiền")]
        [ToolTip("Thành tiền")]
		//[Index(25)]		
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
		//Set Default Value

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

	
       
		//private decimal? _totalvalue;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tổng giá trị")]
        [ToolTip("Tổng giá trị")]
		//[Index(26)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? TotalValue
        { 
		    get => GetPropertyValue<decimal?>("TotalValue");                         
			set => SetPropertyValue<decimal?>("TotalValue", value); 
			
        }
		//Tooltip for Object
		public object TotalValueToolTipControllerText(View view)
        {
        //    if (TotalValue != null) 
		//			return TotalValue;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultTotalValue(View view = null)
        { 
			return TotalValue;
        }
		//Set Default Value
		public void SetDefaultTotalValue(View view = null)
        {
            //if (TotalValue is null){
            //    var result = GetDefaultTotalValue(view);
            //    if (result != null && result != TotalValue){
			//          TotalValue = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TotalValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTotalValue();
				//if (result != null && TotalValue != null){
				//	return !TotalValue.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _vat;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("VAT")]
        [ToolTip("VAT")]
		//[Index(27)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? VAT
        { 
		    get => GetPropertyValue<decimal?>("VAT");                         
			set => SetPropertyValue<decimal?>("VAT", value); 
			
        }
		//Tooltip for Object
		public object VATToolTipControllerText(View view)
        {
        //    if (VAT != null) 
		//			return VAT;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool VATIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultVAT();
				//if (result != null && VAT != null){
				//	return !VAT.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _payment;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thanh toán")]
        [ToolTip("Thanh toán")]
		//[Index(28)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Payment
        { 
		    get => GetPropertyValue<decimal?>("Payment");                         
			set => SetPropertyValue<decimal?>("Payment", value); 
			
        }
		//Tooltip for Object
		public object PaymentToolTipControllerText(View view)
        {
        //    if (Payment != null) 
		//			return Payment;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultPayment(View view = null)
        { 
			return Payment;
        }
		//Set Default Value
		public void SetDefaultPayment(View view = null)
        {
            //if (Payment is null){
            //    var result = GetDefaultPayment(view);
            //    if (result != null && result != Payment){
			//          Payment = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PaymentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPayment();
				//if (result != null && Payment != null){
				//	return !Payment.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _paymentpercent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thanh toán %")]
        [ToolTip("Thanh toán %")]
		//[Index(29)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p2")]
		public decimal? PaymentPercent
        { 
		    get => GetPropertyValue<decimal?>("PaymentPercent");                         
			set => SetPropertyValue<decimal?>("PaymentPercent", value); 
			
        }
		//Tooltip for Object
		public object PaymentPercentToolTipControllerText(View view)
        {
        //    if (PaymentPercent != null) 
		//			return PaymentPercent;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultPaymentPercent(View view = null)
        { 
			return PaymentPercent;
        }
		//Set Default Value
		public void SetDefaultPaymentPercent(View view = null)
        {
            //if (PaymentPercent is null){
            //    var result = GetDefaultPaymentPercent(view);
            //    if (result != null && result != PaymentPercent){
			//          PaymentPercent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PaymentPercentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPaymentPercent();
				//if (result != null && PaymentPercent != null){
				//	return !PaymentPercent.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _importtax;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuế NK")]
        [ToolTip("Thuế NK")]
		//[Index(30)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? ImportTax
        { 
		    get => GetPropertyValue<decimal?>("ImportTax");                         
			set => SetPropertyValue<decimal?>("ImportTax", value); 
			
        }
		//Tooltip for Object
		public object ImportTaxToolTipControllerText(View view)
        {
        //    if (ImportTax != null) 
		//			return ImportTax;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultImportTax(View view = null)
        { 
			return ImportTax;
        }
		//Set Default Value
		public void SetDefaultImportTax(View view = null)
        {
            //if (ImportTax is null){
            //    var result = GetDefaultImportTax(view);
            //    if (result != null && result != ImportTax){
			//          ImportTax = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ImportTaxIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImportTax();
				//if (result != null && ImportTax != null){
				//	return !ImportTax.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(31)]		
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

	
       
		//private OrderType _ordertype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại đơn hàng")]
        [ToolTip("Loại đơn hàng")]
		//[Index(32)]		
		public OrderType OrderType
        { 
		    get => GetPropertyValue<OrderType>("OrderType");                         
			set => SetPropertyValue<OrderType>("OrderType", value); 
			
        }
		//Tooltip for Object
		public object OrderTypeToolTipControllerText(View view)
        {
        //    if (OrderType != null) 
		//			return OrderType;
            return null;
        }
		//Get Default Value
        public OrderType GetDefaultOrderType(View view = null)
        { 
			return OrderType;
        }
		//Set Default Value
		public void SetDefaultOrderType(View view = null)
        {
            //if (OrderType is null){
            //    var result = GetDefaultOrderType(view);
            //    if (result != null && result != OrderType){
			//          OrderType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OrderTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrderType();
				//if (result != null && OrderType != null){
				//	return !OrderType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _codeuniquedate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã theo năm")]
        [ToolTip("Mã theo năm")]
		//[Index(33)]		
	    [Browsable(false)]
		public bool CodeUniqueDate
        { 
		    #region 0954ImportCode 
get
{
	if (Update != null)
	{
    		var time = new DateTime(Update.Value.Year, 1, 1);
    		var parser = CriteriaOperator.Parse("Oid <> ? and Code = ? and Update >= ? and Update < ?", Oid,
        		Code, time,        time.AddYears(1));
    		var result = Session.FindObject(GetType(), parser);
    		return result != null;
	}
	return false;
}
#endregion 0954ImportCode
			
        }
		//Tooltip for Object
		public object CodeUniqueDateToolTipControllerText(View view)
        {
        //    if (CodeUniqueDate != null) 
		//			return CodeUniqueDate;
            return null;
        }
		//Get Default Value
        public bool GetDefaultCodeUniqueDate(View view = null)
        { 
			return CodeUniqueDate;
        }
		//Set Default Value
		public void SetDefaultCodeUniqueDate(View view = null)
        {
            //if (CodeUniqueDate is null){
            //    var result = GetDefaultCodeUniqueDate(view);
            //    if (result != null && result != CodeUniqueDate){
			//          CodeUniqueDate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CodeUniqueDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCodeUniqueDate();
				//if (result != null && CodeUniqueDate != null){
				//	return !CodeUniqueDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Org _partnerorg;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tổ chức đối tác")]
        [ToolTip("Tổ chức đối tác")]
		//[Index(34)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PartnerOrgCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Org PartnerOrg
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Org>("PartnerOrg");                         
			set => SetPropertyValue<Module.BusinessObjects.Org>("PartnerOrg", value); 
			
        }
		//Tooltip for Object
		public object PartnerOrgToolTipControllerText(View view)
        {
        //    if (PartnerOrg != null) 
		//			return PartnerOrg;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Org GetDefaultPartnerOrg(View view = null)
        { 
			return PartnerOrg;
        }
		//Set Default Value
		public void SetDefaultPartnerOrg(View view = null)
        {
            //if (PartnerOrg is null){
            //    var result = GetDefaultPartnerOrg(view);
            //    if (result != null && result != PartnerOrg){
			//          PartnerOrg = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PartnerOrgIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPartnerOrg();
				//if (result != null && PartnerOrg != null){
				//	return !PartnerOrg.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PartnerOrgCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PartnerOrg));
            }
        }
	
       
		//private Module.BusinessObjects.Contact _partnercontact;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên hệ đối tác")]
        [ToolTip("Liên hệ đối tác")]
		//[Index(35)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PartnerContactCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Contact PartnerContact
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Contact>("PartnerContact");                         
			set => SetPropertyValue<Module.BusinessObjects.Contact>("PartnerContact", value); 
			
        }
		//Tooltip for Object
		public object PartnerContactToolTipControllerText(View view)
        {
        //    if (PartnerContact != null) 
		//			return PartnerContact;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Contact GetDefaultPartnerContact(View view = null)
        { 
			return PartnerContact;
        }
		//Set Default Value
		public void SetDefaultPartnerContact(View view = null)
        {
            //if (PartnerContact is null){
            //    var result = GetDefaultPartnerContact(view);
            //    if (result != null && result != PartnerContact){
			//          PartnerContact = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PartnerContactIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPartnerContact();
				//if (result != null && PartnerContact != null){
				//	return !PartnerContact.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PartnerContactCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PartnerContact));
            }
        }
	
       
		//private Module.BusinessObjects.Folder _folder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đơn vị")]
        [ToolTip("Đơn vị")]
		//[Index(36)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#Member# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
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
	
       
		//private System.Guid? _ocrid;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("OcrID")]
        [ToolTip("OcrID")]
		//[Index(37)]		
		public System.Guid? OcrID
        { 
		    get => GetPropertyValue<System.Guid?>("OcrID");                         
			set => SetPropertyValue<System.Guid?>("OcrID", value); 
			
        }
		//Tooltip for Object
		public object OcrIDToolTipControllerText(View view)
        {
        //    if (OcrID != null) 
		//			return OcrID;
            return null;
        }
		//Get Default Value
        public System.Guid? GetDefaultOcrID(View view = null)
        { 
			return OcrID;
        }
		//Set Default Value
		public void SetDefaultOcrID(View view = null)
        {
            //if (OcrID is null){
            //    var result = GetDefaultOcrID(view);
            //    if (result != null && result != OcrID){
			//          OcrID = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OcrIDIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOcrID();
				//if (result != null && OcrID != null){
				//	return !OcrID.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private System.Type _systemtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu hệ thống")]
        [ToolTip("Kiểu hệ thống")]
		//[Index(38)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SystemTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [TypeConverter(typeof(DevExpress.Persistent.Base.Security.SecurityTargetTypeConverter))]
	    [ValueConverter(typeof(DevExpress.ExpressApp.Utils.TypeToStringConverter))]
		public System.Type SystemType
        { 
		    get => GetPropertyValue<System.Type>("SystemType");                         
			set => SetPropertyValue<System.Type>("SystemType", value); 
			
        }
		//Tooltip for Object
		public object SystemTypeToolTipControllerText(View view)
        {
        //    if (SystemType != null) 
		//			return SystemType;
            return null;
        }
		//Get Default Value
        public System.Type GetDefaultSystemType(View view = null)
        { 
			return SystemType;
        }
		//Set Default Value
		public void SetDefaultSystemType(View view = null)
        {
            //if (SystemType is null){
            //    var result = GetDefaultSystemType(view);
            //    if (result != null && result != SystemType){
			//          SystemType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SystemTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSystemType();
				//if (result != null && SystemType != null){
				//	return !SystemType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SystemTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SystemType));
            }
        }
	
       
		//private SoftwareObjectType _softwareobjecttype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu đối tượng")]
        [ToolTip("Kiểu đối tượng")]
		//[Index(39)]		
		public SoftwareObjectType SoftwareObjectType
        { 
		    get => GetPropertyValue<SoftwareObjectType>("SoftwareObjectType");                         
			set => SetPropertyValue<SoftwareObjectType>("SoftwareObjectType", value); 
			
        }
		//Tooltip for Object
		public object SoftwareObjectTypeToolTipControllerText(View view)
        {
        //    if (SoftwareObjectType != null) 
		//			return SoftwareObjectType;
            return null;
        }
		//Get Default Value
        public SoftwareObjectType GetDefaultSoftwareObjectType(View view = null)
        { 
			return SoftwareObjectType;
        }
		//Set Default Value

		//Check Not Validate
		protected bool SoftwareObjectTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSoftwareObjectType();
				//if (result != null && SoftwareObjectType != null){
				//	return !SoftwareObjectType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _year;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Năm")]
        [ToolTip("Năm")]
		//[Index(40)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Year
        { 
		    get => GetPropertyValue<int?>("Year");                         
			set => SetPropertyValue<int?>("Year", value); 
			
        }
		//Tooltip for Object
		public object YearToolTipControllerText(View view)
        {
        //    if (Year != null) 
		//			return Year;
            return null;
        }
		//Get Default Value
        public int? GetDefaultYear(View view = null)
        { 
			return Year;
        }
		//Set Default Value
		public void SetDefaultYear(View view = null)
        {
            //if (Year is null){
            //    var result = GetDefaultYear(view);
            //    if (result != null && result != Year){
			//          Year = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool YearIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultYear();
				//if (result != null && Year != null){
				//	return !Year.Equals(result);
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
 
            #region 0405ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
SetDefaultMember();
Date = DateTime.Today;
// Gán SystemType là kiểu thực tế của đối tượng hiện tại
SystemType = GetType();
            #endregion 0405ImportCode
            Display = true;
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultPartner(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultCurrency(View view = null);
        //SetDefaultExchangeRate(View view = null);
        //SetDefaultRound(View view = null);
        //SetDefaultStatus(View view = null);
        //SetDefaultDate(View view = null);
        //SetDefaultPricingMethod(View view = null);
        //SetDefaultMargin(View view = null);
        //SetDefaultDisccount(View view = null);
        //SetDefaultDocumentCode(View view = null);
        //SetDefaultDocumentDate(View view = null);
        //SetDefaultBook1(View view = null);
        //SetDefaultBook2(View view = null);
        //SetDefaultAmount(View view = null);
        //SetDefaultTotalValue(View view = null);
        //SetDefaultVAT(View view = null);
        //SetDefaultPayment(View view = null);
        //SetDefaultPaymentPercent(View view = null);
        //SetDefaultImportTax(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultOrderType(View view = null);
        //SetDefaultCodeUniqueDate(View view = null);
        //SetDefaultPartnerOrg(View view = null);
        //SetDefaultPartnerContact(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultOcrID(View view = null);
        //SetDefaultSystemType(View view = null);
        //SetDefaultSoftwareObjectType(View view = null);
        //SetDefaultYear(View view = null);
			
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
            #region 0473ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultCode();
            #endregion 0473ImportCode
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
				
                    case nameof(Date):
                        OnChangedDate(oldValue, newValue);
                        break;
				
                    case nameof(SystemType):
                        OnChangedSystemType(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedDate(object oldValue, object newValue)
        {
            #region 3925ImportCode
            Year = Date.Value.Year;            
            #endregion 3925ImportCode
        }               
        private void OnChangedSystemType(object oldValue, object newValue)
        {
            #region 3924ImportCode
            if (newValue is null) return;
SetDefaultSoftwareObjectType();            
            #endregion 3924ImportCode
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
			//	SetDefaultOrderDetailList();
			//	SetDefaultNote();
			//	SetDefaultConditionPaymentDetailList();
			//	SetDefaultInvoicePaymentDetailList();
			//	SetDefaultContractPaymentDetailList();
			//	SetDefaultAccountEntryList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1556ImportCode
		public void SetDefaultAmount(View view = null)
        {
            //Code: 1556            Oid: a5b7c44f-e825-4c23-876f-142d567fe78e
             if (Amount is null){
     var result = GetDefaultAmount();
     if (result != null && result != Amount){
	          Amount = result;
    	  }
 }
        }
#endregion 1556ImportCode
#region 0035ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 0035            Oid: fca9dd81-e8a5-49e5-9568-139203b9a94d
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0035ImportCode
#region 1555ImportCode
		public decimal? GetDefaultAmount(View view = null)
        {
            //Code: 1555            Oid: 6fa85786-f737-4c37-8963-202afbbbbcc2
            decimal? Amount = 0;
foreach (OrderDetail onderDetail in OrderDetailList)
{
    Amount += onderDetail.Total;
}
return Amount;
        }
#endregion 1555ImportCode
#region 3914ImportCode
		public void SetDefaultCode(View view = null)
        {
            //Code: 3914            Oid: 81aac18b-041e-416b-afdb-d1438b7dfa4a
            if (view is ListView)
	return;
if(String.IsNullOrEmpty(Code)) Code = GetDefaultCode();
        }
#endregion 3914ImportCode
#region 1558ImportCode
		public void SetDefaultVAT(View view = null)
        {
            //Code: 1558            Oid: 45ae17e1-7872-4589-9bfc-78692d4c42ef
             if (VAT is null){
     var result = GetDefaultVAT();
     if (result != null && result != VAT){
	          VAT = result;
    	  }
 }
        }
#endregion 1558ImportCode
#region 3923ImportCode
		public void SetDefaultSoftwareObjectType(View view = null)
        {
            //Code: 3923            Oid: 73d0a180-61fd-430c-ac3e-c5356254a7bb
            if (SystemType == null)
    return;

string typeName = SystemType.Name;

if (Enum.TryParse<SoftwareObjectType>(typeName, out var enumValue))
{
    SoftwareObjectType = enumValue;
}

        }
#endregion 3923ImportCode
#region 3913ImportCode
		public string GetDefaultCode(View view = null)
        {
            //Code: 3913            Oid: ef04efec-3556-4a6b-8161-586643744752
            if(Date is null) return null;
var keyCodeObject =
    Tools.GetSettingParameter(Session, "CodeObject");
var parser = string.Format("and Date >='{0}-01-01' and Date <'{1}-01-01'",
                    Date.Value.Year,
                    Date.Value.Year + 1);

//Trường hợp chỉ lấy mã trên đối tượng này
Type currentType = this.GetType();
//Trường hợp lấy mã từ đối tượng cha
//Type currentType = typeof(ObjectType);

    //Kích thước mặc định là 4 số
    int size = 3;		
    return Tools.GetCode(currentType , this.Session, this.Oid, keyCodeObject != null ? keyCodeObject.Value : "", size ,
        parser);
return null;
        }
#endregion 3913ImportCode
#region 1454ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 1454            Oid: 66e8cce0-5f99-4d7f-9a6f-0122a989ffac
            if(Member == null) Member = GetDefaultMember();

        }
#endregion 1454ImportCode
#region 1455ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 1455            Oid: 5f930e23-8736-4122-b339-73f795d5ad2c
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);

        }
#endregion 1455ImportCode
#region 0144ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0144            Oid: e08eb0c0-e137-4233-9d49-0f2c434782e8
            Update = GetDefaultUpdate();
        }
#endregion 0144ImportCode
#region 1557ImportCode
		public decimal? GetDefaultVAT(View view = null)
        {
            //Code: 1557            Oid: de51ed0d-cf38-4fab-a819-5bb15748c722
            decimal? VAT = 0;
foreach (OrderDetail onderDetail in OrderDetailList)
    {
        VAT += onderDetail.Total * onderDetail.VAT;
    }
return VAT;
        }
#endregion 1557ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
