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
    [ModelDefault("Caption", "Cổ phần"), ImageName("ShareHolder")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(Updater))]
 
	[MobileColumnAttribute(Context = "Invester_ShareHolderList_ListView", TargetItems = nameof(Update)+ "," + nameof(Company)+ "," + nameof(Percentage)+ "," + nameof(Quantity))]
	[MobileColumnAttribute(Context = "ShareHolder_ListView", TargetItems = nameof(Quantity)+ "," + nameof(Company)+ "," + nameof(Invester)+ "," + nameof(Update)+ "," + nameof(Percentage))]
	[MobileColumnAttribute(Context = "ShareHolder_LookupListView", TargetItems = nameof(Percentage)+ "," + nameof(Company)+ "," + nameof(Quantity)+ "," + nameof(Invester))]
	[MobileColumnAttribute(Context = "Company_ShareHolderList_ListView", TargetItems = nameof(Invester)+ "," + nameof(Percentage)+ "," + "Invester.Image")]
	[DefaultProperty("Quantity")]
 
[OptimisticLocking(true)]
    public partial class ShareHolder:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ShareHolder(Session session)
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
				if (InvestmentList.IsLoaded)
                {
                    if (InvestmentList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(InvestmentList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(InvestmentList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Investment>(CriteriaOperator.Parse("[ShareHolder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool investmentlist = Session.Query<Module.BusinessObjects.Investment>().Where(x => x.ShareHolder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(InvestmentList), investmentlist);
                        if (investmentlist)
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
               

		//private decimal? _percentage;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Sở hữu")]
        [ToolTip("Sở hữu")]
		//[Index(0)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p2")]
	    [NonPersistent()]
	    [NotMapped()]
		public decimal? Percentage
        { 
		    get => GetPropertyValue<decimal?>("Percentage");                         
			set => SetPropertyValue<decimal?>("Percentage", value); 
			
        }
		//Tooltip for Object
		public object PercentageToolTipControllerText(View view)
        {
        //    if (Percentage != null) 
		//			return Percentage;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultPercentage(View view = null)
        { 
			return Percentage;
        }
		//Set Default Value
		public void SetDefaultPercentage(View view = null)
        {
            //if (Percentage is null){
            //    var result = GetDefaultPercentage(view);
            //    if (result != null && result != Percentage){
			//          Percentage = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PercentageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPercentage();
				//if (result != null && Percentage != null){
				//	return !Percentage.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đầu tư")]
		//[Index(1)]
		[DevExpress.Xpo.Association("ShareHolder-InvestmentList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Investment> InvestmentList
        {      
		    get => GetCollection<Module.BusinessObjects.Investment>("InvestmentList"); 
			
        }
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(2)]		
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
		//[Index(3)]		
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
	
       
		//private Module.BusinessObjects.Invester _invester;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Invester")]
        [ToolTip("Invester")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(InvesterCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Invester-ShareHolderList")]
	 
		public Module.BusinessObjects.Invester Invester
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Invester>("Invester");                         
			set => SetPropertyValue<Module.BusinessObjects.Invester>("Invester", value); 
			
        }
		//Tooltip for Object
		public object InvesterToolTipControllerText(View view)
        {
        //    if (Invester != null) 
		//			return Invester;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Invester GetDefaultInvester(View view = null)
        { 
			return Invester;
        }
		//Set Default Value
		public void SetDefaultInvester(View view = null)
        {
            //if (Invester is null){
            //    var result = GetDefaultInvester(view);
            //    if (result != null && result != Invester){
			//          Invester = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool InvesterIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultInvester();
				//if (result != null && Invester != null){
				//	return !Invester.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator InvesterCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Invester));
            }
        }
	
       
		//private Module.BusinessObjects.Company _company;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Company")]
        [ToolTip("Company")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CompanyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Company-ShareHolderList")]
	 
		public Module.BusinessObjects.Company Company
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Company>("Company");                         
			set => SetPropertyValue<Module.BusinessObjects.Company>("Company", value); 
			
        }
		//Tooltip for Object
		public object CompanyToolTipControllerText(View view)
        {
        //    if (Company != null) 
		//			return Company;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Company GetDefaultCompany(View view = null)
        { 
			return Company;
        }
		//Set Default Value
		public void SetDefaultCompany(View view = null)
        {
            //if (Company is null){
            //    var result = GetDefaultCompany(view);
            //    if (result != null && result != Company){
			//          Company = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CompanyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCompany();
				//if (result != null && Company != null){
				//	return !Company.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CompanyCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Company));
            }
        }
	
       
		//private decimal? _amount;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số tiền")]
        [ToolTip("Số tiền")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
	    [NonPersistent()]
	    [NotMapped()]
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

	
       
		//private Module.BusinessObjects.Currency _currency;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tiền tệ")]
        [ToolTip("Tiền tệ")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CurrencyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [NotMapped()]
	    [NonPersistent()]
		public Module.BusinessObjects.Currency Currency
        { 
		    #region 3091ImportCode 
get => Company.Currency;
#endregion 3091ImportCode
			
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
	
       
		//private decimal? _quantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số cổ phần")]
        [ToolTip("Số cổ phần")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
	    [NotMapped()]
	    [NonPersistent()]
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultPercentage(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultInvester(View view = null);
        //SetDefaultCompany(View view = null);
        //SetDefaultAmount(View view = null);
        //SetDefaultCurrency(View view = null);
        //SetDefaultQuantity(View view = null);
			
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
            #region 2906ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 2906ImportCode
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
			//	SetDefaultInvestmentList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 2905ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 2905            Oid: 0f350824-cbb2-41ad-8368-909a6fb05bc5
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 2905ImportCode
#region 2909ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 2909            Oid: c705be3a-c1f4-43d2-b6f8-e3bd18f9eba0
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2909ImportCode
#region 2908ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 2908            Oid: 339d8ec7-fbce-411e-ba78-1009c4698b83
            Updater = GetDefaultUpdater();
        }
#endregion 2908ImportCode
#region 2907ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 2907            Oid: 1723efb4-a2c3-44cb-860f-15562df26c6a
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 2907ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
