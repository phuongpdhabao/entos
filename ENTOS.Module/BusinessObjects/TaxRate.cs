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
	[NavigationItem("ProductBusiness")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Thuế suất"), ImageName("TaxRate")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update))]
 
	[MobileColumnAttribute(Context = "HScode_TaxRateList_ListView", TargetItems = nameof(Rate)+ "," + "TaxType.[LegalDoc.Date]"+ "," + nameof(TaxType))]
	[MobileColumnAttribute(Context = "TaxRate_LookupListView", TargetItems = nameof(HScode)+ "," + "TaxType.[LegalDoc.Date]"+ "," + nameof(TaxType))]
	[MobileColumnAttribute(Context = "TaxRate_ListView", TargetItems = nameof(Rate)+ "," + nameof(TaxType)+ "," + nameof(HScode))]
	[DefaultProperty("Type")]
 
[OptimisticLocking(true)]
    public partial class TaxRate:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public TaxRate(Session session)
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
               

		//private Module.BusinessObjects.HScode _hscode;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã HS")]
        [ToolTip("Mã HS")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(HScodeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("HScode-TaxRateList")]
	 
		public Module.BusinessObjects.HScode HScode
        { 
		    get => GetPropertyValue<Module.BusinessObjects.HScode>("HScode");                         
			set => SetPropertyValue<Module.BusinessObjects.HScode>("HScode", value); 
			
        }
		//Tooltip for Object
		public object HScodeToolTipControllerText(View view)
        {
        //    if (HScode != null) 
		//			return HScode;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.HScode GetDefaultHScode(View view = null)
        { 
			return HScode;
        }
		//Set Default Value
		public void SetDefaultHScode(View view = null)
        {
            //if (HScode is null){
            //    var result = GetDefaultHScode(view);
            //    if (result != null && result != HScode){
			//          HScode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool HScodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultHScode();
				//if (result != null && HScode != null){
				//	return !HScode.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator HScodeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(HScode));
            }
        }
	
       
		//private decimal _rate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tỉ lệ")]
        [ToolTip("Tỉ lệ")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p0")]
		public decimal Rate
        { 
		    get => GetPropertyValue<decimal>("Rate");                         
			set => SetPropertyValue<decimal>("Rate", value); 
			
        }
		//Tooltip for Object
		public object RateToolTipControllerText(View view)
        {
        //    if (Rate != null) 
		//			return Rate;
            return null;
        }
		//Get Default Value
        public decimal GetDefaultRate(View view = null)
        { 
			return Rate;
        }
		//Set Default Value
		public void SetDefaultRate(View view = null)
        {
            //if (Rate is null){
            //    var result = GetDefaultRate(view);
            //    if (result != null && result != Rate){
			//          Rate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRate();
				//if (result != null && Rate != null){
				//	return !Rate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.TaxType _taxtype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại thuế")]
        [ToolTip("Loại thuế")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TaxTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.TaxType TaxType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.TaxType>("TaxType");                         
			set => SetPropertyValue<Module.BusinessObjects.TaxType>("TaxType", value); 
			
        }
		//Tooltip for Object
		public object TaxTypeToolTipControllerText(View view)
        {
        //    if (TaxType != null) 
		//			return TaxType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.TaxType GetDefaultTaxType(View view = null)
        { 
			return TaxType;
        }
		//Set Default Value
		public void SetDefaultTaxType(View view = null)
        {
            //if (TaxType is null){
            //    var result = GetDefaultTaxType(view);
            //    if (result != null && result != TaxType){
			//          TaxType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TaxTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTaxType();
				//if (result != null && TaxType != null){
				//	return !TaxType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TaxTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(TaxType));
            }
        }
	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(3)]		
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0437ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
            #endregion 0437ImportCode
 
        //SetDefaultHScode(View view = null);
        //SetDefaultRate(View view = null);
        //SetDefaultTaxType(View view = null);
        //SetDefaultUpdate(View view = null);
			
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
            #region 0514ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0514ImportCode
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
#region 0032ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0032            Oid: 10ff2e56-0bcc-4edf-aeb3-b540ae336702
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0032ImportCode
#region 0078ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0078            Oid: a03a7fab-3524-4070-9ce5-8bf6edd0ed89
            Update = GetDefaultUpdate();
        }
#endregion 0078ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
