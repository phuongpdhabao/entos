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
    [ModelDefault("Caption", "Chấm công"), ImageName("TimeSlot")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("TimeSlot Type IsNotValidate" , TargetItems = "Type" , Criteria = "TypeIsNotValidate",AppearanceItemType = "ViewItem", Context = "DetailView" , FontColor = "#FF0000" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
    [ShowToolTipAttribute(TargetItems = nameof(Date))]
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(Creator))]
 
	[MobileColumnAttribute(Context = "TimeSlot_ListView", TargetItems = nameof(Date)+ "," + nameof(StartTime)+ "," + nameof(Member))]
	[MobileColumnAttribute(Context = "TimeSlot_LookupListView", TargetItems = nameof(StartTime)+ "," + nameof(EndTime)+ "," + nameof(Content)+ "," + nameof(Date))]
	[DefaultProperty("Content")]
 
	[RuleCriteria(nameof(TypeIsNotValidate), DefaultContexts.Save, nameof(TypeIsNotValidate), InvertResult = true, ResultType = ValidationResultType.Warning, CustomMessageTemplate = "Số giờ phép không hợp lệ")]
[OptimisticLocking(true)]
    public partial class TimeSlot:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public TimeSlot(Session session)
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
		[DevExpress.Xpo.DisplayName("Bắt đầu")]
        [ToolTip("Bắt đầu")]
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
		[DevExpress.Xpo.DisplayName("Kết thúc")]
        [ToolTip("Kết thúc")]
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

	
       
		//private DateTime _date;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime Date
        { 
		    get => GetPropertyValue<DateTime>("Date");                         
			set => SetPropertyValue<DateTime>("Date", value); 
			
        }
		//Tooltip for Object
		public object DateToolTipControllerText(View view)
        {
            #region 0455ImportCode 
if (Date != null)
                return Date.ToString("d/M/yyyy");
            return null;
#endregion 0455ImportCode
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

	
       
		//private string _content;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(3)]		

 		[Size(150)]
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
        public string GetDefaultContent(View view = null)
        { 
			return Content;
        }
		//Set Default Value
		public void SetDefaultContent(View view = null)
        {
            //if (Content is null){
            //    var result = GetDefaultContent(view);
            //    if (result != null && result != Content){
			//          Content = result;
            //	  }
            //}
        }

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

	
       
		//private TimeKeepingType _type;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(4)]		
		public TimeKeepingType Type
        { 
		    get => GetPropertyValue<TimeKeepingType>("Type");                         
			set => SetPropertyValue<TimeKeepingType>("Type", value); 
			
        }
		//Tooltip for Object
		public object TypeToolTipControllerText(View view)
        {
        //    if (Type != null) 
		//			return Type;
            return null;
        }
		//Get Default Value
        public TimeKeepingType GetDefaultType(View view = null)
        { 
			return Type;
        }
		//Set Default Value
		public void SetDefaultType(View view = null)
        {
            //if (Type is null){
            //    var result = GetDefaultType(view);
            //    if (result != null && result != Type){
			//          Type = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TypeIsNotValidate
        {
            get
            {
			#region 0525ImportCode 
if(EndTime != null && StartTime != null && Member != null && Type == TimeKeepingType.OnLeave)
                {
                    var currentTime = Module.Helpers.XafXpoHelper.GetDateTimeNowFromServer(Session);
                    var lastedTimeSlots = new XPCollection<TimeSlot>(Session,
                        CriteriaOperator.Parse("EndTime is not null And Type = ? And Member.Oid = ? And StartTime >= ?",
                        Type, Member.Oid, new DateTime(currentTime.Year, 1, 1)));
                    if(lastedTimeSlots.Count > 0)
                    {                        
                        var maxHours = currentTime.Month * 8;
                        var totalHours = (EndTime.Value - StartTime).Value.TotalHours;
                        if(StartTime.Value.Hour < 12 && EndTime.Value.AddMinutes(30).Hour > 14)
                        {
                            totalHours -= (double)1.5;
                        }
                                
                        foreach (var timeSlot in lastedTimeSlots)
                        {
                            if (!timeSlot.Oid.Equals(Oid))
                            {
                                var duration = (timeSlot.EndTime.Value - timeSlot.StartTime).Value.TotalHours;
                                if (timeSlot.StartTime.Value.Hour < 12 && timeSlot.EndTime.Value.AddMinutes(30).Hour > 14)
                                {
                                    duration -= (double)1.5;
                                }
                                totalHours += duration;
                            }
                        }
                        return totalHours > maxHours;
                    }
                    
                }
#endregion 0525ImportCode                
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thành viên")]
        [ToolTip("Thành viên")]
		//[Index(5)]		
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
        public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        { 
			return Member;
        }
		//Set Default Value
		public void SetDefaultMember(View view = null)
        {
            //if (Member is null){
            //    var result = GetDefaultMember(view);
            //    if (result != null && result != Member){
			//          Member = result;
            //	  }
            //}
        }

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
	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(6)]		
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

	
       
		//private Module.BusinessObjects.Member _creator;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Người tạo")]
        [ToolTip("Người tạo")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CreatorCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Member Creator
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Creator");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Creator", value); 
			
        }
		//Tooltip for Object
		public object CreatorToolTipControllerText(View view)
        {
        //    if (Creator != null) 
		//			return Creator;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool CreatorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCreator();
				//if (result != null && Creator != null){
				//	return !Creator.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CreatorCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Creator));
            }
        }
	
       
		//private TimeSpan? _duration;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thời lượng")]
        [ToolTip("Thời lượng")]
		//[Index(8)]		
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0408ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
SetDefaultCreator();
            #endregion 0408ImportCode
 
        //SetDefaultStartTime(View view = null);
        //SetDefaultEndTime(View view = null);
        //SetDefaultDate(View view = null);
        //SetDefaultContent(View view = null);
        //SetDefaultType(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultCreator(View view = null);
        //SetDefaultDuration(View view = null);
			
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
            #region 0464ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0464ImportCode
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
#region 0457ImportCode
		public Module.BusinessObjects.Member GetDefaultCreator(View view = null)
        {
            //Code: 0457            Oid: e15f0664-a5f2-4719-9697-ac1471a7cc4b
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);

        }
#endregion 0457ImportCode
#region 0100ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0100            Oid: 3a2b77ec-099d-40e9-aa99-e3ed7a98b407
            Update = GetDefaultUpdate();
        }
#endregion 0100ImportCode
#region 0150ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0150            Oid: 4e8135fd-a66a-4555-9f57-b4ba9f86428b
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0150ImportCode
#region 0456ImportCode
		public void SetDefaultCreator(View view = null)
        {
            //Code: 0456            Oid: 1891ad7d-0a46-46a2-9ce6-08af917608c5
            if(Creator == null) Creator = GetDefaultCreator();

        }
#endregion 0456ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
