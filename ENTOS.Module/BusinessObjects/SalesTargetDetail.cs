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
    [ModelDefault("Caption", "Chi tiết mục tiêu doanh số"), ImageName("SalesTargetDetail")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "SalesTargetDetail_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "SalesTargetDetail_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "SalesTarget_SalesTargetDetailList_ListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class SalesTargetDetail:  DevExpress.Xpo.XPLiteObject  , IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public SalesTargetDetail(Session session)
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

	
       
		//private decimal? _targetnumber;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mục tiêu")]
        [ToolTip("Mục tiêu")]
		//[Index(1)]		
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
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thực tế")]
        [ToolTip("Thực tế")]
		//[Index(2)]		
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

	
       
		//private decimal? _bonus;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thưởng")]
        [ToolTip("Thưởng")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
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

	
       
		//private decimal? _percent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Phần trăm")]
        [ToolTip("Phần trăm")]
		//[Index(4)]		
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

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(5)]		
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
        public int? GetDefaultOrder(View view = null)
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

	
       
		//private Module.BusinessObjects.SalesTarget _salestarget;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mục tiêu doanh số")]
        [ToolTip("Mục tiêu doanh số")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SalesTargetCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("SalesTarget-SalesTargetDetailList")]
	 
		public Module.BusinessObjects.SalesTarget SalesTarget
        { 
		    get => GetPropertyValue<Module.BusinessObjects.SalesTarget>("SalesTarget");                         
			set => SetPropertyValue<Module.BusinessObjects.SalesTarget>("SalesTarget", value); 
			
        }
		//Tooltip for Object
		public object SalesTargetToolTipControllerText(View view)
        {
        //    if (SalesTarget != null) 
		//			return SalesTarget;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.SalesTarget GetDefaultSalesTarget(View view = null)
        { 
			return SalesTarget;
        }
		//Set Default Value
		public void SetDefaultSalesTarget(View view = null)
        {
            //if (SalesTarget is null){
            //    var result = GetDefaultSalesTarget(view);
            //    if (result != null && result != SalesTarget){
			//          SalesTarget = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SalesTargetIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSalesTarget();
				//if (result != null && SalesTarget != null){
				//	return !SalesTarget.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SalesTargetCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SalesTarget));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultTargetNumber(View view = null);
        //SetDefaultActualNumber(View view = null);
        //SetDefaultBonus(View view = null);
        //SetDefaultPercent(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultSalesTarget(View view = null);
			
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
