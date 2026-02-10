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
    [ModelDefault("Caption", "Tiền tệ"), ImageName("Currency")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "Currency_ListView", TargetItems = nameof(Name)+ "," + nameof(ExchangeRate)+ "," + nameof(Code))]
	[MobileColumnAttribute(Context = "Currency_LookupListView", TargetItems = nameof(Code))]
	[DefaultProperty("Code")]
 
[OptimisticLocking(true)]
    public partial class Currency:  DevExpress.Xpo.XPLiteObject  , IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Currency(Session session)
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
               

		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(0)]		

 		[Size(10)]
		[RuleUniqueValue("UniqueCurrencyCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredCurrencyCode", DefaultContexts.Save)]
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

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(1)]		

 		[Size(100)]
		[RuleUniqueValue("UniqueCurrencyName", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredCurrencyName", DefaultContexts.Save)]
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

	
       
		//private decimal _exchangerate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tỉ giá")]
        [ToolTip("Tỉ giá")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		[RuleUniqueValue("UniqueCurrencyExchangeRate", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredCurrencyExchangeRate", DefaultContexts.Save)]
		public decimal ExchangeRate
        { 
		    get => GetPropertyValue<decimal>("ExchangeRate");                         
			set => SetPropertyValue<decimal>("ExchangeRate", value); 
			
        }
		//Tooltip for Object
		public object ExchangeRateToolTipControllerText(View view)
        {
        //    if (ExchangeRate != null) 
		//			return ExchangeRate;
            return null;
        }
		//Get Default Value
        public decimal GetDefaultExchangeRate(View view = null)
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

	
       
		//private bool _default;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mặc định")]
        [ToolTip("Mặc định")]
		//[Index(3)]		
		public bool Default
        { 
		    get => GetPropertyValue<bool>("Default");                         
			set => SetPropertyValue<bool>("Default", value); 
			
        }
		//Tooltip for Object
		public object DefaultToolTipControllerText(View view)
        {
        //    if (Default != null) 
		//			return Default;
            return null;
        }
		//Get Default Value
        public bool GetDefaultDefault(View view = null)
        { 
			return Default;
        }
		//Set Default Value
		public void SetDefaultDefault(View view = null)
        {
            //if (Default is null){
            //    var result = GetDefaultDefault(view);
            //    if (result != null && result != Default){
			//          Default = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DefaultIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDefault();
				//if (result != null && Default != null){
				//	return !Default.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _prefix;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tiền tố")]
        [ToolTip("Tiền tố")]
		//[Index(4)]		

 		[Size(20)]
		[RuleUniqueValue("UniqueCurrencyPrefix", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredCurrencyPrefix", DefaultContexts.Save)]
		public string Prefix
        { 
		    get => GetPropertyValue<string>("Prefix");                         
			set => SetPropertyValue<string>("Prefix", value); 
			
        }
		//Tooltip for Object
		public object PrefixToolTipControllerText(View view)
        {
        //    if (Prefix != null) 
		//			return Prefix;
            return null;
        }
		//Get Default Value
        public string GetDefaultPrefix(View view = null)
        { 
			return Prefix;
        }
		//Set Default Value
		public void SetDefaultPrefix(View view = null)
        {
            //if (Prefix is null){
            //    var result = GetDefaultPrefix(view);
            //    if (result != null && result != Prefix){
			//          Prefix = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PrefixIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPrefix();
				//if (result != null && Prefix != null){
				//	return !Prefix.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _suffix;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hậu tố")]
        [ToolTip("Hậu tố")]
		//[Index(5)]		

 		[Size(20)]
		[RuleUniqueValue("UniqueCurrencySuffix", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredCurrencySuffix", DefaultContexts.Save)]
		public string Suffix
        { 
		    get => GetPropertyValue<string>("Suffix");                         
			set => SetPropertyValue<string>("Suffix", value); 
			
        }
		//Tooltip for Object
		public object SuffixToolTipControllerText(View view)
        {
        //    if (Suffix != null) 
		//			return Suffix;
            return null;
        }
		//Get Default Value
        public string GetDefaultSuffix(View view = null)
        { 
			return Suffix;
        }
		//Set Default Value
		public void SetDefaultSuffix(View view = null)
        {
            //if (Suffix is null){
            //    var result = GetDefaultSuffix(view);
            //    if (result != null && result != Suffix){
			//          Suffix = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SuffixIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSuffix();
				//if (result != null && Suffix != null){
				//	return !Suffix.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultExchangeRate(View view = null);
        //SetDefaultDefault(View view = null);
        //SetDefaultPrefix(View view = null);
        //SetDefaultSuffix(View view = null);
			
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
             base.OnSaving();
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
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
