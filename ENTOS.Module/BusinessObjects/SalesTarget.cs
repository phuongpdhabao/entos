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
    [ModelDefault("Caption", "Mục tiêu doanh số"), ImageName("SalesTarget")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(Update))]
 
	[MobileColumnAttribute(Context = "SalesTarget_ListView", TargetItems = nameof(MemberFolder)+ "," + nameof(TargetNumber)+ "," + nameof(Year))]
	[MobileColumnAttribute(Context = "SalesTarget_LookupListView", TargetItems = nameof(Year)+ "," + nameof(Update))]
 
[OptimisticLocking(true)]
    public partial class SalesTarget:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public SalesTarget(Session session)
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
				if (SalesTargetDetailList.IsLoaded)
                {
                    if (SalesTargetDetailList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(SalesTargetDetailList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(SalesTargetDetailList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.SalesTargetDetail>(CriteriaOperator.Parse("[SalesTarget.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool salestargetdetaillist = Session.Query<Module.BusinessObjects.SalesTargetDetail>().Where(x => x.SalesTarget.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(SalesTargetDetailList), salestargetdetaillist);
                        if (salestargetdetaillist)
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
               

		//private Module.BusinessObjects.Folder _memberfolder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(0)]		
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
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thành viên")]
        [ToolTip("Thành viên")]
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
	
       
		//private DateTime? _year;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Năm")]
        [ToolTip("Năm")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "yyyy")]
		[ModelDefault("EditMask", "yyyy")]
		public DateTime? Year
        { 
		    get => GetPropertyValue<DateTime?>("Year");                         
			set => SetPropertyValue<DateTime?>("Year", value); 
			
        }
		//Tooltip for Object
		public object YearToolTipControllerText(View view)
        {
        //    if (Year != null) 
		//			return Year;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultYear(View view = null)
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

	
       
		//private TransactionType _transactiontype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giao dịch")]
        [ToolTip("Giao dịch")]
		//[Index(3)]		
		public TransactionType TransactionType
        { 
		    get => GetPropertyValue<TransactionType>("TransactionType");                         
			set => SetPropertyValue<TransactionType>("TransactionType", value); 
			
        }
		//Tooltip for Object
		public object TransactionTypeToolTipControllerText(View view)
        {
        //    if (TransactionType != null) 
		//			return TransactionType;
            return null;
        }
		//Get Default Value
        public TransactionType GetDefaultTransactionType(View view = null)
        { 
			return TransactionType;
        }
		//Set Default Value
		public void SetDefaultTransactionType(View view = null)
        {
            //if (TransactionType is null){
            //    var result = GetDefaultTransactionType(view);
            //    if (result != null && result != TransactionType){
			//          TransactionType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TransactionTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTransactionType();
				//if (result != null && TransactionType != null){
				//	return !TransactionType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Org _org;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Hãng")]
        [ToolTip("Hãng")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OrgCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
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
        public Module.BusinessObjects.Org GetDefaultOrg(View view = null)
        { 
			return Org;
        }
		//Set Default Value
		public void SetDefaultOrg(View view = null)
        {
            //if (Org is null){
            //    var result = GetDefaultOrg(view);
            //    if (result != null && result != Org){
			//          Org = result;
            //	  }
            //}
        }

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
	
       
		//private Module.BusinessObjects.ProductType _producttype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại sản phẩm")]
        [ToolTip("Loại sản phẩm")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.ProductType ProductType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ProductType>("ProductType");                         
			set => SetPropertyValue<Module.BusinessObjects.ProductType>("ProductType", value); 
			
        }
		//Tooltip for Object
		public object ProductTypeToolTipControllerText(View view)
        {
        //    if (ProductType != null) 
		//			return ProductType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ProductType GetDefaultProductType(View view = null)
        { 
			return ProductType;
        }
		//Set Default Value
		public void SetDefaultProductType(View view = null)
        {
            //if (ProductType is null){
            //    var result = GetDefaultProductType(view);
            //    if (result != null && result != ProductType){
			//          ProductType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ProductTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultProductType();
				//if (result != null && ProductType != null){
				//	return !ProductType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ProductTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ProductType));
            }
        }
	
       
		//private decimal? _targetnumber;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mục tiêu")]
        [ToolTip("Mục tiêu")]
		//[Index(7)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public decimal? TargetNumber
        { 
		    get => GetPropertyValue<decimal?>("TargetNumber");                         
			set => SetPropertyValue<decimal?>("TargetNumber", value); 
			
        }
		//Tooltip for Object
		public object TargetNumberToolTipControllerText(View view)
        {
        //    if (TargetNumber != null) 
		//			return TargetNumber;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultTargetNumber(View view = null)
        { 
			return TargetNumber;
        }
		//Set Default Value
		public void SetDefaultTargetNumber(View view = null)
        {
            //if (TargetNumber is null){
            //    var result = GetDefaultTargetNumber(view);
            //    if (result != null && result != TargetNumber){
			//          TargetNumber = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TargetNumberIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTargetNumber();
				//if (result != null && TargetNumber != null){
				//	return !TargetNumber.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _actualnumber;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thực tế")]
        [ToolTip("Thực tế")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public decimal? ActualNumber
        { 
		    get => GetPropertyValue<decimal?>("ActualNumber");                         
			set => SetPropertyValue<decimal?>("ActualNumber", value); 
			
        }
		//Tooltip for Object
		public object ActualNumberToolTipControllerText(View view)
        {
        //    if (ActualNumber != null) 
		//			return ActualNumber;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultActualNumber(View view = null)
        { 
			return ActualNumber;
        }
		//Set Default Value
		public void SetDefaultActualNumber(View view = null)
        {
            //if (ActualNumber is null){
            //    var result = GetDefaultActualNumber(view);
            //    if (result != null && result != ActualNumber){
			//          ActualNumber = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ActualNumberIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultActualNumber();
				//if (result != null && ActualNumber != null){
				//	return !ActualNumber.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Currency _currency;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tiền")]
        [ToolTip("Tiền")]
		//[Index(9)]		
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
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chi tiết")]
		//[Index(10)]
		[DevExpress.Xpo.Association("SalesTarget-SalesTargetDetailList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.SalesTargetDetail> SalesTargetDetailList
        {      
		    get => GetCollection<Module.BusinessObjects.SalesTargetDetail>("SalesTargetDetailList"); 
			
        }
       
		//private decimal? _percent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Phần trăm")]
        [ToolTip("Phần trăm")]
		//[Index(11)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p2")]
		public decimal? Percent
        { 
		    get => GetPropertyValue<decimal?>("Percent");                         
			set => SetPropertyValue<decimal?>("Percent", value); 
			
        }
		//Tooltip for Object
		public object PercentToolTipControllerText(View view)
        {
        //    if (Percent != null) 
		//			return Percent;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultPercent(View view = null)
        { 
			return Percent;
        }
		//Set Default Value
		public void SetDefaultPercent(View view = null)
        {
            //if (Percent is null){
            //    var result = GetDefaultPercent(view);
            //    if (result != null && result != Percent){
			//          Percent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PercentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPercent();
				//if (result != null && Percent != null){
				//	return !Percent.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(12)]		
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

	
       
		//private decimal? _bonus;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thưởng")]
        [ToolTip("Thưởng")]
		//[Index(13)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Bonus
        { 
		    get => GetPropertyValue<decimal?>("Bonus");                         
			set => SetPropertyValue<decimal?>("Bonus", value); 
			
        }
		//Tooltip for Object
		public object BonusToolTipControllerText(View view)
        {
        //    if (Bonus != null) 
		//			return Bonus;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultBonus(View view = null)
        { 
			return Bonus;
        }
		//Set Default Value
		public void SetDefaultBonus(View view = null)
        {
            //if (Bonus is null){
            //    var result = GetDefaultBonus(view);
            //    if (result != null && result != Bonus){
			//          Bonus = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BonusIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBonus();
				//if (result != null && Bonus != null){
				//	return !Bonus.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultYear(View view = null);
        //SetDefaultTransactionType(View view = null);
        //SetDefaultOrg(View view = null);
        //SetDefaultProductType(View view = null);
        //SetDefaultTargetNumber(View view = null);
        //SetDefaultActualNumber(View view = null);
        //SetDefaultCurrency(View view = null);
        //SetDefaultPercent(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultBonus(View view = null);
			
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
            #region 0335ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0335ImportCode
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
			//	SetDefaultSalesTargetDetailList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0226ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 0226            Oid: 81a45b5c-fe89-4a77-90ac-116dfa732258
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 0226ImportCode
#region 0228ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0228            Oid: 2dc5c333-fb99-40b0-b3c8-95ad7ae11cc6
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0228ImportCode
#region 0229ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0229            Oid: ec722449-44b5-4d73-a7f0-e494c4d7425c
            Update = GetDefaultUpdate();
        }
#endregion 0229ImportCode
#region 0227ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 0227            Oid: b8bf005b-373e-4bfb-a8d6-5d3bd6fa02ea
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 0227ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
