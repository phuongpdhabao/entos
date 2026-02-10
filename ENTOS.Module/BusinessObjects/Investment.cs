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
    [ModelDefault("Caption", "Đầu tư"), ImageName("Investment")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Date)+ "," + nameof(Update)+ "," + nameof(Updater))]
 
	[MobileColumnAttribute(Context = "ShareHolder_InvestmentList_ListView", TargetItems = nameof(Amount)+ "," + nameof(Currency)+ "," + nameof(Date))]
	[MobileColumnAttribute(Context = "Investment_ListView", TargetItems = nameof(Quantity)+ "," + nameof(Amount)+ "," + nameof(Date)+ "," + nameof(Note)+ "," + nameof(Currency)+ "," + nameof(Update)+ "," + nameof(Percentage))]
	[MobileColumnAttribute(Context = "Investment_LookupListView", TargetItems = nameof(Currency)+ "," + nameof(Quantity)+ "," + nameof(Update)+ "," + nameof(Note)+ "," + nameof(Amount)+ "," + nameof(Percentage)+ "," + nameof(Date))]
	[DefaultProperty("Note")]
 
[OptimisticLocking(true)]
    public partial class Investment:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Investment(Session session)
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
               

		//private DateTime? _date;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(0)]		
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

	
       
		//private decimal? _amount;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số tiền")]
        [ToolTip("Số tiền")]
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

	
       
		//private Module.BusinessObjects.Currency _currency;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tiền tệ")]
        [ToolTip("Tiền tệ")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CurrencyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [NonPersistent()]
	    [NotMapped()]
		public Module.BusinessObjects.Currency Currency
        { 
		    #region 2918ImportCode 
get => ShareHolder.Company.Currency;
#endregion 2918ImportCode
			
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
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số cổ phần")]
        [ToolTip("Số cổ phần")]
		//[Index(3)]		
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

	
       
		//private decimal? _percentage;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tỉ lệ")]
        [ToolTip("Tỉ lệ")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p2")]
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

	
       
		//private string _note;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(5)]		

 		[Size(250)]
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

	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(6)]		
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
		//[Index(7)]		
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
	
       
		//private Module.BusinessObjects.ShareHolder _shareholder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Share Holder")]
        [ToolTip("Share Holder")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ShareHolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("ShareHolder-InvestmentList")]
	 
		public Module.BusinessObjects.ShareHolder ShareHolder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ShareHolder>("ShareHolder");                         
			set => SetPropertyValue<Module.BusinessObjects.ShareHolder>("ShareHolder", value); 
			
        }
		//Tooltip for Object
		public object ShareHolderToolTipControllerText(View view)
        {
        //    if (ShareHolder != null) 
		//			return ShareHolder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ShareHolder GetDefaultShareHolder(View view = null)
        { 
			return ShareHolder;
        }
		//Set Default Value
		public void SetDefaultShareHolder(View view = null)
        {
            //if (ShareHolder is null){
            //    var result = GetDefaultShareHolder(view);
            //    if (result != null && result != ShareHolder){
			//          ShareHolder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ShareHolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultShareHolder();
				//if (result != null && ShareHolder != null){
				//	return !ShareHolder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ShareHolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ShareHolder));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 2917ImportCode
            base.AfterConstruction();
SetDefaultDate();
            #endregion 2917ImportCode
 
        //SetDefaultDate(View view = null);
        //SetDefaultAmount(View view = null);
        //SetDefaultCurrency(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultPercentage(View view = null);
        //SetDefaultNote(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultShareHolder(View view = null);
			
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
            #region 2911ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 2911ImportCode
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
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 2910ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 2910            Oid: 84431e2b-07ae-455e-9252-2c2bd4271fc1
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 2910ImportCode
#region 2916ImportCode
		public void SetDefaultDate(View view = null)
        {
            //Code: 2916            Oid: dba324bf-29eb-435f-abb3-6b9026be9c4a
            if(Date == null) Date = GetDefaultDate();
        }
#endregion 2916ImportCode
#region 2913ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 2913            Oid: 1a637009-e5c6-4ade-8580-6ee0f9b15e86
            Updater = GetDefaultUpdater();
        }
#endregion 2913ImportCode
#region 2915ImportCode
		public DateTime? GetDefaultDate(View view = null)
        {
            //Code: 2915            Oid: 78c224dd-61b2-43ff-a0fe-ad9fd3b05a8f
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 2915ImportCode
#region 2914ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 2914            Oid: 655b008a-50b0-48f6-9730-9f9373cb1c5c
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2914ImportCode
#region 2912ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 2912            Oid: cee2be27-d1f3-4307-9aaa-51edede97577
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 2912ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
