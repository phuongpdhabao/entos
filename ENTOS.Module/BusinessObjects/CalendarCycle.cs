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
    [ModelDefault("Caption", "Chu kỳ lịch"), ImageName("CalendarCycle")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "CalendarCycle_ListView", TargetItems = nameof(EndTime)+ "," + nameof(StartTime))]
	[MobileColumnAttribute(Context = "CalendarCycle_LookupListView", TargetItems = nameof(EndTime)+ "," + nameof(StartTime))]
	[DefaultProperty("StartTime")]
 
[OptimisticLocking(true)]
    public partial class CalendarCycle:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public CalendarCycle(Session session)
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
               

		//private DateTime? _starttime;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Từ")]
        [ToolTip("Từ")]
		//[Index(0)]		
		[ModelDefault("DisplayFormat", "H:mm")]
		[ModelDefault("EditMask", "H:mm")]
		public DateTime? StartTime
        { 
		    get => GetPropertyValue<DateTime?>("StartTime");                         
			set => SetPropertyValue<DateTime?>("StartTime", value); 
			
        }
		//Tooltip for Object
		public object StartTimeToolTipControllerText(View view)
        {
        //    if (StartTime != null) 
		//			return StartTime;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultStartTime(View view = null)
        { 
			return StartTime;
        }
		//Set Default Value
		public void SetDefaultStartTime(View view = null)
        {
            //if (StartTime is null){
            //    var result = GetDefaultStartTime(view);
            //    if (result != null && result != StartTime){
			//          StartTime = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StartTimeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStartTime();
				//if (result != null && StartTime != null){
				//	return !StartTime.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _endtime;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Đến")]
        [ToolTip("Đến")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "H:mm")]
		[ModelDefault("EditMask", "H:mm")]
		public DateTime? EndTime
        { 
		    get => GetPropertyValue<DateTime?>("EndTime");                         
			set => SetPropertyValue<DateTime?>("EndTime", value); 
			
        }
		//Tooltip for Object
		public object EndTimeToolTipControllerText(View view)
        {
        //    if (EndTime != null) 
		//			return EndTime;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultEndTime(View view = null)
        { 
			return EndTime;
        }
		//Set Default Value
		public void SetDefaultEndTime(View view = null)
        {
            //if (EndTime is null){
            //    var result = GetDefaultEndTime(view);
            //    if (result != null && result != EndTime){
			//          EndTime = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EndTimeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEndTime();
				//if (result != null && EndTime != null){
				//	return !EndTime.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private TimeSpan? _duration;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thời lượng")]
        [ToolTip("Thời lượng")]
		//[Index(2)]		
	    [ModelDefault("EditMaskType", "DateTime")]
		public TimeSpan? Duration
        { 
		    get => GetPropertyValue<TimeSpan?>("Duration");                         
			set => SetPropertyValue<TimeSpan?>("Duration", value); 
			
        }
		//Tooltip for Object
		public object DurationToolTipControllerText(View view)
        {
        //    if (Duration != null) 
		//			return Duration;
            return null;
        }
		//Get Default Value
        public TimeSpan? GetDefaultDuration(View view = null)
        { 
			return Duration;
        }
		//Set Default Value
		public void SetDefaultDuration(View view = null)
        {
            //if (Duration is null){
            //    var result = GetDefaultDuration(view);
            //    if (result != null && result != Duration){
			//          Duration = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DurationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDuration();
				//if (result != null && Duration != null){
				//	return !Duration.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Weekday _weekday;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thứ")]
        [ToolTip("Thứ")]
		//[Index(3)]		
		public Weekday Weekday
        { 
		    get => GetPropertyValue<Weekday>("Weekday");                         
			set => SetPropertyValue<Weekday>("Weekday", value); 
			
        }
		//Tooltip for Object
		public object WeekdayToolTipControllerText(View view)
        {
        //    if (Weekday != null) 
		//			return Weekday;
            return null;
        }
		//Get Default Value
        public Weekday GetDefaultWeekday(View view = null)
        { 
			return Weekday;
        }
		//Set Default Value
		public void SetDefaultWeekday(View view = null)
        {
            //if (Weekday is null){
            //    var result = GetDefaultWeekday(view);
            //    if (result != null && result != Weekday){
			//          Weekday = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WeekdayIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWeekday();
				//if (result != null && Weekday != null){
				//	return !Weekday.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _date;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Date
        { 
		    get => GetPropertyValue<int?>("Date");                         
			set => SetPropertyValue<int?>("Date", value); 
			
        }
		//Tooltip for Object
		public object DateToolTipControllerText(View view)
        {
        //    if (Date != null) 
		//			return Date;
            return null;
        }
		//Get Default Value
        public int? GetDefaultDate(View view = null)
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

	
       
		//private Month _month;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tháng")]
        [ToolTip("Tháng")]
		//[Index(5)]		
		public Month Month
        { 
		    get => GetPropertyValue<Month>("Month");                         
			set => SetPropertyValue<Month>("Month", value); 
			
        }
		//Tooltip for Object
		public object MonthToolTipControllerText(View view)
        {
        //    if (Month != null) 
		//			return Month;
            return null;
        }
		//Get Default Value
        public Month GetDefaultMonth(View view = null)
        { 
			return Month;
        }
		//Set Default Value
		public void SetDefaultMonth(View view = null)
        {
            //if (Month is null){
            //    var result = GetDefaultMonth(view);
            //    if (result != null && result != Month){
			//          Month = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MonthIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMonth();
				//if (result != null && Month != null){
				//	return !Month.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Quarter _quarter;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quý")]
        [ToolTip("Quý")]
		//[Index(6)]		
		public Quarter Quarter
        { 
		    get => GetPropertyValue<Quarter>("Quarter");                         
			set => SetPropertyValue<Quarter>("Quarter", value); 
			
        }
		//Tooltip for Object
		public object QuarterToolTipControllerText(View view)
        {
        //    if (Quarter != null) 
		//			return Quarter;
            return null;
        }
		//Get Default Value
        public Quarter GetDefaultQuarter(View view = null)
        { 
			return Quarter;
        }
		//Set Default Value
		public void SetDefaultQuarter(View view = null)
        {
            //if (Quarter is null){
            //    var result = GetDefaultQuarter(view);
            //    if (result != null && result != Quarter){
			//          Quarter = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool QuarterIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultQuarter();
				//if (result != null && Quarter != null){
				//	return !Quarter.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool? _lunarcalendar;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Âm lịch")]
        [ToolTip("Âm lịch")]
		//[Index(7)]		
		public bool? LunarCalendar
        { 
		    get => GetPropertyValue<bool?>("LunarCalendar");                         
			set => SetPropertyValue<bool?>("LunarCalendar", value); 
			
        }
		//Tooltip for Object
		public object LunarCalendarToolTipControllerText(View view)
        {
        //    if (LunarCalendar != null) 
		//			return LunarCalendar;
            return null;
        }
		//Get Default Value
        public bool? GetDefaultLunarCalendar(View view = null)
        { 
			return LunarCalendar;
        }
		//Set Default Value
		public void SetDefaultLunarCalendar(View view = null)
        {
            //if (LunarCalendar is null){
            //    var result = GetDefaultLunarCalendar(view);
            //    if (result != null && result != LunarCalendar){
			//          LunarCalendar = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LunarCalendarIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLunarCalendar();
				//if (result != null && LunarCalendar != null){
				//	return !LunarCalendar.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Calendar _calendar;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lịch")]
        [ToolTip("Lịch")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CalendarCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Calendar Calendar
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Calendar>("Calendar");                         
			set => SetPropertyValue<Module.BusinessObjects.Calendar>("Calendar", value); 
			
        }
		//Tooltip for Object
		public object CalendarToolTipControllerText(View view)
        {
        //    if (Calendar != null) 
		//			return Calendar;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Calendar GetDefaultCalendar(View view = null)
        { 
			return Calendar;
        }
		//Set Default Value
		public void SetDefaultCalendar(View view = null)
        {
            //if (Calendar is null){
            //    var result = GetDefaultCalendar(view);
            //    if (result != null && result != Calendar){
			//          Calendar = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CalendarIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCalendar();
				//if (result != null && Calendar != null){
				//	return !Calendar.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CalendarCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Calendar));
            }
        }
	
       
		//private int? _repeat;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lần lặp")]
        [ToolTip("Lần lặp")]
		//[Index(9)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Repeat
        { 
		    get => GetPropertyValue<int?>("Repeat");                         
			set => SetPropertyValue<int?>("Repeat", value); 
			
        }
		//Tooltip for Object
		public object RepeatToolTipControllerText(View view)
        {
        //    if (Repeat != null) 
		//			return Repeat;
            return null;
        }
		//Get Default Value
        public int? GetDefaultRepeat(View view = null)
        { 
			return Repeat;
        }
		//Set Default Value
		public void SetDefaultRepeat(View view = null)
        {
            //if (Repeat is null){
            //    var result = GetDefaultRepeat(view);
            //    if (result != null && result != Repeat){
			//          Repeat = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RepeatIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRepeat();
				//if (result != null && Repeat != null){
				//	return !Repeat.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultStartTime(View view = null);
        //SetDefaultEndTime(View view = null);
        //SetDefaultDuration(View view = null);
        //SetDefaultWeekday(View view = null);
        //SetDefaultDate(View view = null);
        //SetDefaultMonth(View view = null);
        //SetDefaultQuarter(View view = null);
        //SetDefaultLunarCalendar(View view = null);
        //SetDefaultCalendar(View view = null);
        //SetDefaultRepeat(View view = null);
			
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
