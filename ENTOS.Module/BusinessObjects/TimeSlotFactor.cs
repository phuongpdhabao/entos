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
    [ModelDefault("Caption", "Hệ số tính công"), ImageName("TimeSlotFactor")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update))]
 
	[MobileColumnAttribute(Context = "TimeSlotFactor_LookupListView", TargetItems = nameof(TimeSlotType))]
	[MobileColumnAttribute(Context = "TimeSlotFactor_ListView", TargetItems = nameof(AdjustmentFactor)+ "," + nameof(TimeSlotType))]
	[DefaultProperty("TimeSlotType")]
 
[OptimisticLocking(true)]
    public partial class TimeSlotFactor:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public TimeSlotFactor(Session session)
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
               

		//private TimeKeepingType _timeslottype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại khoảng")]
        [ToolTip("Loại khoảng")]
		//[Index(0)]		
		public TimeKeepingType TimeSlotType
        { 
		    get => GetPropertyValue<TimeKeepingType>("TimeSlotType");                         
			set => SetPropertyValue<TimeKeepingType>("TimeSlotType", value); 
			
        }
		//Tooltip for Object
		public object TimeSlotTypeToolTipControllerText(View view)
        {
        //    if (TimeSlotType != null) 
		//			return TimeSlotType;
            return null;
        }
		//Get Default Value
        public TimeKeepingType GetDefaultTimeSlotType(View view = null)
        { 
			return TimeSlotType;
        }
		//Set Default Value
		public void SetDefaultTimeSlotType(View view = null)
        {
            //if (TimeSlotType is null){
            //    var result = GetDefaultTimeSlotType(view);
            //    if (result != null && result != TimeSlotType){
			//          TimeSlotType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TimeSlotTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTimeSlotType();
				//if (result != null && TimeSlotType != null){
				//	return !TimeSlotType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _adjustmentfactor;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Hệ số")]
        [ToolTip("Hệ số")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p0")]
		public decimal? AdjustmentFactor
        { 
		    get => GetPropertyValue<decimal?>("AdjustmentFactor");                         
			set => SetPropertyValue<decimal?>("AdjustmentFactor", value); 
			
        }
		//Tooltip for Object
		public object AdjustmentFactorToolTipControllerText(View view)
        {
        //    if (AdjustmentFactor != null) 
		//			return AdjustmentFactor;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultAdjustmentFactor(View view = null)
        { 
			return AdjustmentFactor;
        }
		//Set Default Value
		public void SetDefaultAdjustmentFactor(View view = null)
        {
            //if (AdjustmentFactor is null){
            //    var result = GetDefaultAdjustmentFactor(view);
            //    if (result != null && result != AdjustmentFactor){
			//          AdjustmentFactor = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AdjustmentFactorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAdjustmentFactor();
				//if (result != null && AdjustmentFactor != null){
				//	return !AdjustmentFactor.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime _date;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hiệu lực")]
        [ToolTip("Hiệu lực")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime Date
        { 
		    get => GetPropertyValue<DateTime>("Date");                         
			set => SetPropertyValue<DateTime>("Date", value); 
			
        }
		//Tooltip for Object
		public object DateToolTipControllerText(View view)
        {
        //    if (Date != null) 
		//			return Date;
            return null;
        }
		//Get Default Value
        public DateTime GetDefaultDate(View view = null)
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
 
            #region 0424ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
            #endregion 0424ImportCode
 
        //SetDefaultTimeSlotType(View view = null);
        //SetDefaultAdjustmentFactor(View view = null);
        //SetDefaultDate(View view = null);
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
            #region 0478ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0478ImportCode
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
#region 0026ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0026            Oid: c817c548-6366-41a7-849b-9c92ec98b203
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0026ImportCode
#region 0135ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0135            Oid: a5622c2b-a1ac-420f-8fdc-c87b8dcbf508
            Update = GetDefaultUpdate();
        }
#endregion 0135ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
