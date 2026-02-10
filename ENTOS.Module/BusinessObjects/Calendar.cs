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
    [ModelDefault("Caption", "Lịch"), ImageName("Calendar")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("Calendar EndTime, CalendarType, Content, StartTime, Member, Duration None_None__Color [A=255, R=0, G=128, B=128]" , TargetItems = "EndTime, CalendarType, Content, StartTime, Member, Duration" , Criteria = "GetDay([StartTime]) % 4 = 1 And [EndTime] Is Not Null",AppearanceItemType = "ViewItem", Context = "ListView" , FontColor = "#008080" )]
	[Appearance("Calendar EndTime, CalendarType, Content, StartTime, Member, Duration None_None__Color [A=255, R=65, G=105, B=225]" , TargetItems = "EndTime, CalendarType, Content, StartTime, Member, Duration" , Criteria = "GetDay([StartTime]) % 4 = 2 And [EndTime] Is Not Null",AppearanceItemType = "ViewItem", Context = "ListView" , FontColor = "#4169E1" )]
	[Appearance("Calendar EndTime, Duration, StartTime None_None__Color [A=255, R=255, G=0, B=0]" , TargetItems = "EndTime, Duration, StartTime" , Criteria = "[Duration] > #04:00:00# Or (GetHour([StartTime]) < 12 And GetHour([EndTime]) > 13)",AppearanceItemType = "ViewItem", FontColor = "#FF0000" )]
	[Appearance("Calendar EndTime, CalendarType, Content, StartTime, Member, Duration None_None__Color [A=255, R=0, G=100, B=0]" , TargetItems = "EndTime, CalendarType, Content, StartTime, Member, Duration" , Criteria = "GetDay([StartTime]) % 4 = 3 And [EndTime] Is Not Null",AppearanceItemType = "ViewItem", Context = "ListView" , FontColor = "#006400" )]
	[Appearance("Calendar EndTime, CalendarType, Content, StartTime, Member, Duration None_None__Color [A=255, R=255, G=128, B=0]" , TargetItems = "EndTime, CalendarType, Content, StartTime, Member, Duration" , Criteria = "[EndTime] Is Null And [CalendarType] = ##ToString#Fact#",AppearanceItemType = "ViewItem", Context = "ListView" , FontColor = "#FF8000" )]
	[Appearance("Calendar EndTime IsNotValidate" , TargetItems = "EndTime" , Criteria = "EndTimeIsNotValidate",AppearanceItemType = "ViewItem", Context = "DetailView" , FontColor = "#FF0000" )]
	[Appearance("Calendar StartTime IsNotValidate" , TargetItems = "StartTime" , Criteria = "StartTimeIsNotValidate",AppearanceItemType = "ViewItem", Context = "DetailView" , FontColor = "#FF0000" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
    [ShowToolTipAttribute(TargetItems = nameof(EndTime)+ "," + nameof(Duration)+ "," + nameof(StartTime2))]
 
    [AllowSetDefaultAttribute(TargetItems = nameof(StartTime)+ "," + nameof(Content)+ "," + nameof(Update)+ "," + nameof(Member))]
 
	[MobileColumnAttribute(Context = "MyCalendar_ListView", TargetItems = nameof(StartTime)+ "," + nameof(EndTime)+ "," + nameof(Content)+ "," + nameof(Member))]
	[MobileColumnAttribute(Context = "Work_CalendarList_ListView", TargetItems = nameof(EndTime)+ "," + nameof(Member)+ "," + nameof(StartTime))]
	[MobileColumnAttribute(Context = "Folder_CalendarList_ListView", TargetItems = nameof(StartTime)+ "," + nameof(Content)+ "," + nameof(EndTime))]
	[MobileColumnAttribute(Context = "SchoolYear_CalendarList_ListView", TargetItems = nameof(StartTime)+ "," + nameof(Content)+ "," + nameof(EndTime))]
	[MobileColumnAttribute(Context = "Equipment_CalendarList_ListView", TargetItems = nameof(StartTime)+ "," + nameof(EndTime)+ "," + nameof(Content)+ "," + nameof(Member))]
	[MobileColumnAttribute(Context = "PublicEvent_CalendarList_ListView", TargetItems = nameof(EndTime)+ "," + nameof(StartTime)+ "," + nameof(Content))]
	[MobileColumnAttribute(Context = "TaskDoing_CalendarList_ListView", TargetItems = nameof(EndTime)+ "," + nameof(Content)+ "," + nameof(StartTime))]
	[MobileColumnAttribute(Context = "Calendar_ListView", TargetItems = nameof(EndTime)+ "," + nameof(Member)+ "," + nameof(Content)+ "," + nameof(StartTime))]
	[MobileColumnAttribute(Context = "Asset_CalendarList_ListView", TargetItems = nameof(StartTime)+ "," + nameof(CalendarType)+ "," + nameof(Content))]
	[MobileColumnAttribute(Context = "Calendar_LookupListView", TargetItems = nameof(EndTime)+ "," + nameof(Content)+ "," + nameof(StartTime))]
	[MobileColumnAttribute(Context = "Work2_CalendarList_ListView", TargetItems = nameof(EndTime)+ "," + nameof(Member)+ "," + nameof(StartTime)+ "," + nameof(Content))]
	[DefaultProperty("StartTime")]
 
	
	[CustomFilter("IMember", "Member.Oid = ?")]
	[RuleCriteria(nameof(EndTimeIsNotValidate), DefaultContexts.Save, nameof(EndTimeIsNotValidate), InvertResult = true, ResultType = ValidationResultType.Warning)]
	[RuleCriteria(nameof(StartTimeIsNotValidate), DefaultContexts.Save,  nameof(StartTimeIsNotValidate), InvertResult = true, ResultType =ValidationResultType.Warning)]
	[DevExpress.ExpressApp.SystemModule.ListViewFilter("Thời gian", "", true)]
	[DevExpress.ExpressApp.SystemModule.ListViewFilter("Ngoài giờ làm việc", "GetHour([StartTime]) < 8 or GetHour(AddMinutes([StartTime], 29)) >= 18 or (GetHour(AddMinutes([StartTime], 59)) >= 13 and GetHour(AddMinutes([StartTime], 30)) < 14) or GetHour([EndTime]) < 8 or GetHour(AddMinutes([EndTime], 29)) >= 18 or (GetHour(AddMinutes([EndTime], 59)) >= 13 and GetHour(AddMinutes([EndTime], 30)) < 14)")]
	[DevExpress.ExpressApp.SystemModule.ListViewFilter("Thời lượng > 4h", "[Duration] > #04:00:01#")]
[OptimisticLocking(true)]
    public partial class Calendar:  DevExpress.Xpo.XPLiteObject , IFilterDate ,IMember, INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Calendar(Session session)
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
               

		//private DateTime _starttime;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Từ")]
        [ToolTip("Từ")]
		//[Index(0)]		
		[ModelDefault("DisplayFormat", "hh:mm")]
		[ModelDefault("EditMask", "d/M/yyyy H:mm")]
		public DateTime StartTime
        { 
		    get => GetPropertyValue<DateTime>("StartTime");                         
			set => SetPropertyValue<DateTime>("StartTime", value); 
			
        }
		//Tooltip for Object
		public object StartTimeToolTipControllerText(View view)
        {
        //    if (StartTime != null) 
		//			return StartTime;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool StartTimeIsNotValidate
        {
            get
            {
			#region 0523ImportCode 
if (StartTime != null)
                {
                    //Thứ 2 đến thứ 6 : 8>12 và 1h30>5h30
                    //Thứ 7: 8 > 12 và 1h30 > 4h30
                    if (StartTime.Hour < 8)
                        return true;
                    if (StartTime.Hour > 12 && StartTime.AddMinutes(30).Hour < 14)                        
                        return true;
                    if (StartTime.DayOfWeek == DayOfWeek.Saturday)
                    {
                        if (StartTime.AddMinutes(29).Hour > 16)
                            return true;
                    }
                    else
                    {
                        if(StartTime.AddMinutes(29).Hour > 17)
                            return true;
                    }
                    if (Member != null)
                        return Session.FindObject<Calendar>(PersistentCriteriaEvaluationBehavior.InTransaction, CriteriaOperator.Parse("Oid <> ? and Member.Oid = ? and CalendarType = ? and StartTime < ? and EndTime > ?", Oid, Member.Oid, CalendarType, StartTime, StartTime)) != null;


                }
#endregion 0523ImportCode                
   
                return false;
            }
        }

	
       
		//private DateTime? _endtime;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Đến")]
        [ToolTip("Đến")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "hh:mm")]
		[ModelDefault("EditMask", "d/M/yyyy H:mm")]
		public DateTime? EndTime
        { 
		    get => GetPropertyValue<DateTime?>("EndTime");                         
			set => SetPropertyValue<DateTime?>("EndTime", value); 
			
        }
		//Tooltip for Object
		public object EndTimeToolTipControllerText(View view)
        {
            #region 0309ImportCode 
if(EndTime != null)
return EndTime.Value.ToString("d/M/yyyy H:mm");
#endregion 0309ImportCode
            return null;
        }
		//Get Default Value
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
			#region 0524ImportCode 
//Thứ 2 đến thứ 6 : 8>12 và 1h30>5h30
                //Thứ 7: 8 > 12 và 1h30 > 4h30
                if (EndTime != null)
                {
                    if (EndTime.Value.Hour < 8)
                        return true;
                    if (EndTime.Value.Hour > 12 && EndTime.Value.AddMinutes(30).Hour < 14)
                        return true;
                    if (EndTime.Value.DayOfWeek == DayOfWeek.Saturday)
                    {
                        if (EndTime.Value.AddMinutes(29).Hour > 16)
                            return true;
                    }
                    else
                    {
                        if (EndTime.Value.AddMinutes(29).Hour > 17)
                            return true;
                    }
                    if (EndTime < StartTime) return true;
                    return Session.FindObject<Calendar>(PersistentCriteriaEvaluationBehavior.InTransaction, CriteriaOperator.Parse("Oid <> ? and Member.Oid = ? And CalendarType = ? And EndTime is not null And ((StartTime < ? and EndTime > ?) Or (StartTime > ? and EndTime < ?))", Oid, Member.Oid, CalendarType, EndTime, EndTime, StartTime, EndTime)) != null;

                }
#endregion 0524ImportCode                
   
                return false;
            }
        }

	
       
		//private string _content;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(2)]		

 		[Size(200)]
		public string Content
        { 
		    get => GetPropertyValue<string>("Content");                         
			set => SetPropertyValue<string>("Content", value); 
			
        }
		//Tooltip for Object
		public object ContentToolTipControllerText(View view)
        {
        //    if (Content != null) 
		//			return Content;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool ContentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultContent();
				//if (result != null && Content != null){
				//	return !Content.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _lunarcalendar;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Âm lịch")]
        [ToolTip("Âm lịch")]
		//[Index(3)]		
		public bool LunarCalendar
        { 
		    get => GetPropertyValue<bool>("LunarCalendar");                         
			set => SetPropertyValue<bool>("LunarCalendar", value); 
			
        }
		//Tooltip for Object
		public object LunarCalendarToolTipControllerText(View view)
        {
        //    if (LunarCalendar != null) 
		//			return LunarCalendar;
            return null;
        }
		//Get Default Value
        public bool GetDefaultLunarCalendar(View view = null)
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

	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(4)]		
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

	
       
		//private TimeSpan? _duration;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thời lượng")]
        [ToolTip("Thời lượng")]
		//[Index(5)]		
	    [ModelDefault("EditMaskType", "DateTime")]
		public TimeSpan? Duration
        { 
		    #region 0288ImportCode 
get
            {
                if (EndTime is null)
                    return Module.Helpers.XafXpoHelper.GetDateTimeNowFromServer(Session) - StartTime;
                return EndTime.Value - StartTime;
            }
#endregion 0288ImportCode
			
        }
		//Tooltip for Object
		public object DurationToolTipControllerText(View view)
        {
            #region 0289ImportCode 
if (Member != null)
            {
                var calendars = new XPCollection<Calendar>(Session, CriteriaOperator.Parse("EndTime is not null and CalendarType = ? and Member = ? and StartTime >= ? and StartTime < ?", CalendarType, Member, StartTime.Date , StartTime.Date.AddDays(1)));
                TimeSpan totalDuration = TimeSpan.Zero;
                foreach (var calendar in calendars)
                {
                    if (calendar.EndTime != null)
                        totalDuration += calendar.EndTime.Value - calendar.StartTime;
                }
                return string.Format("{0:n0}:{1:mm}",
                    Math.Round(totalDuration.TotalHours, MidpointRounding.ToNegativeInfinity), totalDuration);
            }
#endregion 0289ImportCode
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

	
       
		//private DateTime? _starttime2;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bắt đầu 2")]
        [ToolTip("Bắt đầu 2")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy H:mm")]
	    [NonPersistent()]
	    [NotMapped()]
		public DateTime? StartTime2
        { 
		    #region 2672ImportCode 
get => StartTime;
#endregion 2672ImportCode
			
        }
		//Tooltip for Object
		public object StartTime2ToolTipControllerText(View view)
        {
            #region 2674ImportCode 
return StartTime2.Value.ToString("d/M/yyyy");
#endregion 2674ImportCode
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultStartTime2(View view = null)
        { 
			return StartTime2;
        }
		//Set Default Value
		public void SetDefaultStartTime2(View view = null)
        {
            //if (StartTime2 is null){
            //    var result = GetDefaultStartTime2(view);
            //    if (result != null && result != StartTime2){
			//          StartTime2 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StartTime2IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStartTime2();
				//if (result != null && StartTime2 != null){
				//	return !StartTime2.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _endtime2;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kết thúc 2")]
        [ToolTip("Kết thúc 2")]
		//[Index(7)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy H:mm")]
	    [NotMapped()]
	    [NonPersistent()]
		public DateTime? EndTime2
        { 
		    get => GetPropertyValue<DateTime?>("EndTime2");                         
			set => SetPropertyValue<DateTime?>("EndTime2", value); 
			
        }
		//Tooltip for Object
		public object EndTime2ToolTipControllerText(View view)
        {
        //    if (EndTime2 != null) 
		//			return EndTime2;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultEndTime2(View view = null)
        { 
			return EndTime2;
        }
		//Set Default Value
		public void SetDefaultEndTime2(View view = null)
        {
            //if (EndTime2 is null){
            //    var result = GetDefaultEndTime2(view);
            //    if (result != null && result != EndTime2){
			//          EndTime2 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EndTime2IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEndTime2();
				//if (result != null && EndTime2 != null){
				//	return !EndTime2.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Work _work;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Công việc")]
        [ToolTip("Công việc")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(WorkCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Work-CalendarList")]
	 
		public Module.BusinessObjects.Work Work
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Work>("Work");                         
			set => SetPropertyValue<Module.BusinessObjects.Work>("Work", value); 
			
        }
		//Tooltip for Object
		public object WorkToolTipControllerText(View view)
        {
        //    if (Work != null) 
		//			return Work;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Work GetDefaultWork(View view = null)
        { 
			return Work;
        }
		//Set Default Value
		public void SetDefaultWork(View view = null)
        {
            //if (Work is null){
            //    var result = GetDefaultWork(view);
            //    if (result != null && result != Work){
			//          Work = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WorkIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWork();
				//if (result != null && Work != null){
				//	return !Work.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator WorkCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Work));
            }
        }
	
       
		//private Module.BusinessObjects.Member _member;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thành viên")]
        [ToolTip("Thành viên")]
		//[Index(9)]		
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
	
       
		//private Module.BusinessObjects.Folder _memberfolder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(10)]		
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
	
       
		//private Module.BusinessObjects.PublicEvent _publicevent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sự kiện")]
        [ToolTip("Sự kiện")]
		//[Index(12)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PublicEventCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("PublicEvent-CalendarList")]
	 
		public Module.BusinessObjects.PublicEvent PublicEvent
        { 
		    get => GetPropertyValue<Module.BusinessObjects.PublicEvent>("PublicEvent");                         
			set => SetPropertyValue<Module.BusinessObjects.PublicEvent>("PublicEvent", value); 
			
        }
		//Tooltip for Object
		public object PublicEventToolTipControllerText(View view)
        {
        //    if (PublicEvent != null) 
		//			return PublicEvent;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.PublicEvent GetDefaultPublicEvent(View view = null)
        { 
			return PublicEvent;
        }
		//Set Default Value
		public void SetDefaultPublicEvent(View view = null)
        {
            //if (PublicEvent is null){
            //    var result = GetDefaultPublicEvent(view);
            //    if (result != null && result != PublicEvent){
			//          PublicEvent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PublicEventIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPublicEvent();
				//if (result != null && PublicEvent != null){
				//	return !PublicEvent.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PublicEventCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PublicEvent));
            }
        }
	
       
		//private Module.BusinessObjects.EventSeries _eventseries;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dòng sự kiện")]
        [ToolTip("Dòng sự kiện")]
		//[Index(13)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(EventSeriesCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.EventSeries EventSeries
        { 
		    get => GetPropertyValue<Module.BusinessObjects.EventSeries>("EventSeries");                         
			set => SetPropertyValue<Module.BusinessObjects.EventSeries>("EventSeries", value); 
			
        }
		//Tooltip for Object
		public object EventSeriesToolTipControllerText(View view)
        {
        //    if (EventSeries != null) 
		//			return EventSeries;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.EventSeries GetDefaultEventSeries(View view = null)
        { 
			return EventSeries;
        }
		//Set Default Value
		public void SetDefaultEventSeries(View view = null)
        {
            //if (EventSeries is null){
            //    var result = GetDefaultEventSeries(view);
            //    if (result != null && result != EventSeries){
			//          EventSeries = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EventSeriesIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEventSeries();
				//if (result != null && EventSeries != null){
				//	return !EventSeries.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator EventSeriesCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(EventSeries));
            }
        }
	
       
		//private Module.BusinessObjects.Asset _asset;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài sản")]
        [ToolTip("Tài sản")]
		//[Index(14)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(AssetCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Asset-CalendarList")]
	 
		public Module.BusinessObjects.Asset Asset
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Asset>("Asset");                         
			set => SetPropertyValue<Module.BusinessObjects.Asset>("Asset", value); 
			
        }
		//Tooltip for Object
		public object AssetToolTipControllerText(View view)
        {
        //    if (Asset != null) 
		//			return Asset;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Asset GetDefaultAsset(View view = null)
        { 
			return Asset;
        }
		//Set Default Value
		public void SetDefaultAsset(View view = null)
        {
            //if (Asset is null){
            //    var result = GetDefaultAsset(view);
            //    if (result != null && result != Asset){
			//          Asset = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AssetIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAsset();
				//if (result != null && Asset != null){
				//	return !Asset.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator AssetCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Asset));
            }
        }
	
       
		//private Module.BusinessObjects.Folder _folder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(15)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#Calendar# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-CalendarList")]
	 
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
	
       
		//private CalendarType _calendartype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại lịch")]
        [ToolTip("Loại lịch")]
		//[Index(16)]		
		public CalendarType CalendarType
        { 
		    get => GetPropertyValue<CalendarType>("CalendarType");                         
			set => SetPropertyValue<CalendarType>("CalendarType", value); 
			
        }
		//Tooltip for Object
		public object CalendarTypeToolTipControllerText(View view)
        {
        //    if (CalendarType != null) 
		//			return CalendarType;
            return null;
        }
		//Get Default Value
        public CalendarType GetDefaultCalendarType(View view = null)
        { 
			return CalendarType;
        }
		//Set Default Value
		public void SetDefaultCalendarType(View view = null)
        {
            //if (CalendarType is null){
            //    var result = GetDefaultCalendarType(view);
            //    if (result != null && result != CalendarType){
			//          CalendarType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CalendarTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCalendarType();
				//if (result != null && CalendarType != null){
				//	return !CalendarType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Equipment _equipment;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thiết bị")]
        [ToolTip("Thiết bị")]
		//[Index(18)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(EquipmentCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Equipment-CalendarList")]
	 
		public Module.BusinessObjects.Equipment Equipment
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Equipment>("Equipment");                         
			set => SetPropertyValue<Module.BusinessObjects.Equipment>("Equipment", value); 
			
        }
		//Tooltip for Object
		public object EquipmentToolTipControllerText(View view)
        {
        //    if (Equipment != null) 
		//			return Equipment;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Equipment GetDefaultEquipment(View view = null)
        { 
			return Equipment;
        }
		//Set Default Value
		public void SetDefaultEquipment(View view = null)
        {
            //if (Equipment is null){
            //    var result = GetDefaultEquipment(view);
            //    if (result != null && result != Equipment){
			//          Equipment = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EquipmentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEquipment();
				//if (result != null && Equipment != null){
				//	return !Equipment.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator EquipmentCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Equipment));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0368ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
SetDefaultStartTime();
            #endregion 0368ImportCode
 
        //SetDefaultStartTime(View view = null);
        //SetDefaultEndTime(View view = null);
        //SetDefaultContent(View view = null);
        //SetDefaultLunarCalendar(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultDuration(View view = null);
        //SetDefaultStartTime2(View view = null);
        //SetDefaultEndTime2(View view = null);
        //SetDefaultWork(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultPublicEvent(View view = null);
        //SetDefaultEventSeries(View view = null);
        //SetDefaultAsset(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultCalendarType(View view = null);
        //SetDefaultEquipment(View view = null);
			
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
            #region 0480ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0480ImportCode
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
				
                    case nameof(EndTime):
                        OnChangedEndTime(oldValue, newValue);
                        break;
				
                    case nameof(Work):
                        OnChangedWork(oldValue, newValue);
                        break;
				
                    case nameof(StartTime):
                        OnChangedStartTime(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedEndTime(object oldValue, object newValue)
        {
            #region 0379ImportCode
            if (Work != null)
                        Work.Duration = Work.GetDefaultDuration();            
            #endregion 0379ImportCode
        }               
        private void OnChangedWork(object oldValue, object newValue)
        {
            #region 0384ImportCode
            if (newValue is null) return;
SetDefaultMember();
if (Work.Status != null && Work.Status.Code == "Doing")
CalendarType = CalendarType.Fact;
else
{
//if (Work.Plan != null)
//Plan = Work.Plan;
}            
            #endregion 0384ImportCode
        }               
        private void OnChangedStartTime(object oldValue, object newValue)
        {
            #region 0431ImportCode
            if (Work != null && EndTime != null)
                    Work.Duration = Work.GetDefaultDuration();            
            #endregion 0431ImportCode
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
#region 0234ImportCode
		public string GetDefaultContent(View view = null)
        {
            //Code: 0234            Oid: 63082039-b14d-4ace-bab9-fc5327a49436
            if(Work != null)
 return Work.Name;
return null;
        }
#endregion 0234ImportCode
#region 0043ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0043            Oid: 7e3d3e26-aa6d-469f-9ad8-4168dbf70f79
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0043ImportCode
#region 0187ImportCode
		public DateTime? GetDefaultEndTime(View view = null)
        {
            //Code: 0187            Oid: cf349cf5-e97f-4932-bbb4-554eaa84e560
            var currentDateTime = (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);

//Test currentDateTime = currentDateTime.AddHours(7);
//Cập nhật End Time của lịch khi Dừng công việc 12h hoặc 5h30 cùng ngày, 4h30 thứ 7
var timeSpan = currentDateTime - StartTime;            
if (StartTime.Hour < 12 && StartTime.Hour >= 8)
{
    //Nếu bắt đầu lớn hơn 
    if (timeSpan.Hours > 4 || (currentDateTime.Hour >= 12 && currentDateTime.AddMinutes(30).Hour < 14))
    {
        return new System.DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 12, 0, 0);
    }
}else if (StartTime.Hour <= 17 && StartTime.Hour >= 13)
{                
    int tempMaxHour = 18;
    //Kiểm tra nếu là thứ 7 thì trừ 1 tiếng
    if (StartTime.DayOfWeek == DayOfWeek.Saturday)
        tempMaxHour = 17;
    if (timeSpan.Hours > 4)
    {
        return new System.DateTime(StartTime.Year, StartTime.Month, StartTime.Day, tempMaxHour - 1, 30, 0);
    }
    else
    {
        var tempStartTime = StartTime.AddMinutes(30);
        //if (tempStartTime.Hour >= 14 && tempStartTime.Hour <= tempMaxHour)
        if (tempStartTime.Hour < tempMaxHour)
        {
            var tempEndTime = currentDateTime.AddMinutes(30);
            //Nếu bắt đầu lớn hơn 

            if (tempEndTime.Hour >= tempMaxHour)
            {
                return new System.DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, tempMaxHour - 1, 30, 0);
            }
        }
    }                
}
return currentDateTime;
        }
#endregion 0187ImportCode
#region 0087ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0087            Oid: ffde0b9b-127a-4e80-8c3e-ae1bb1c46d61
            Update = GetDefaultUpdate();
        }
#endregion 0087ImportCode
#region 0232ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 0232            Oid: 9b882eae-73ff-47f2-bb84-e7eac8498eac
            if(Member == null) Member = GetDefaultMember();

        }
#endregion 0232ImportCode
#region 0237ImportCode
		public DateTime GetDefaultStartTime(View view = null)
        {
            //Code: 0237            Oid: 3c26fd17-7e83-484e-98eb-97470b438481
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);

        }
#endregion 0237ImportCode
#region 0235ImportCode
		public void SetDefaultStartTime(View view = null)
        {
            //Code: 0235            Oid: b0d6eac5-e55e-4933-9bee-5d72dcfbed79
            if(StartTime == new DateTime()) StartTime= GetDefaultStartTime();
        }
#endregion 0235ImportCode
#region 0233ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 0233            Oid: e8566880-c486-4ad2-aacc-526db3e356a6
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 0233ImportCode
#region 0236ImportCode
		public void SetDefaultContent(View view = null)
        {
            //Code: 0236            Oid: 285f4f63-adfb-496d-8f18-d1dd76e07e46
            if(String.IsNullOrEmpty(Content)) Content = GetDefaultContent();

        }
#endregion 0236ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
