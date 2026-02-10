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
	[NavigationItem("TaskManagement")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Công việc"), ImageName("Work")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("Work Deadline IsNotValidate" , TargetItems = "Deadline" , Criteria = "DeadlineIsNotValidate",AppearanceItemType = "ViewItem", FontColor = "#FF0000" )]
	[Appearance("Work MemberWorkList Show_None__" , TargetItems = "MemberWorkList" , Criteria = "[MemberWorkList][].Count() >= 1",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Show )]
	[Appearance("Work Code None_None__Color [A=255, R=0, G=255, B=0]" , TargetItems = "Code" , Criteria = "[Status.Code] = 'Doing' Or [MemberWorkList][Status.Code = 'Doing']",AppearanceItemType = "ViewItem", FontColor = "#00FF00" )]
	[Appearance("Work Reference None_None__Color [A=255, R=128, G=0, B=128]" , TargetItems = "Reference" , Criteria = "[Reference] = 'Tư liệu' And ([ObjectID] <> {00000000-0000-0000-0000-000000000000} Or [ObjectID] Is Not Null)",AppearanceItemType = "ViewItem", Context = "ListView" , FontColor = "#800080" )]
	[Appearance("Work Code None_None__Color [A=255, R=0, G=0, B=255]" , TargetItems = "Code" , Criteria = "[Status.Code] = 'NotCompleted' And Not [MemberWorkList][Status.Code <> 'NotCompleted']",AppearanceItemType = "ViewItem", FontColor = "#0000FF" )]
	[Appearance("Work WorkDetailList Show_None__" , TargetItems = "WorkDetailList" , Criteria = "[WorkDetailList][].Count() >= 1",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Show )]
	[Appearance("Work Duration IsNotValidate" , TargetItems = "Duration" , Criteria = "DurationIsNotValidate",AppearanceItemType = "ViewItem", FontColor = "#FF0000" )]
	[Appearance("Work Quantity Hide_None__" , TargetItems = "Quantity" , Criteria = "[WorkType] Is Null",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
	[Appearance("Work Code None_None__Color [A=255, R=0, G=192, B=192]" , TargetItems = "Code" , Criteria = "[Status.Code] = 'NotStarted' And Not [MemberWorkList][Status.Code <> 'NotStarted']",AppearanceItemType = "ViewItem", FontColor = "#00C0C0" )]
	[Appearance("Work Requester None_Disable__" , TargetItems = "Requester" , Criteria = "[Requester.Oid] <> CURRENTUSERID()",AppearanceItemType = "ViewItem", Enabled = false )]
	[Appearance("Work Member None_None__" , TargetItems = "Member" , Criteria = "[MemberWorkList][].Count() >= 1",AppearanceItemType = "ViewItem", FontStyle = DevExpress.Drawing.DXFontStyle.Bold )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(WorkType)+ "," + nameof(Deadline)+ "," + nameof(MemberWorkList)+ "," + nameof(CalendarList), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
    [ShowToolTipAttribute(TargetItems = nameof(WorkType)+ "," + nameof(Quantity)+ "," + nameof(Member)+ "," + nameof(Deadline)+ "," + nameof(Duration))]
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Code)+ "," + nameof(Name)+ "," + nameof(Quantity)+ "," + nameof(Member)+ "," + nameof(Requester)+ "," + nameof(Status)+ "," + nameof(Update)+ "," + nameof(CreatedDate)+ "," + nameof(Order)+ "," + nameof(Folder))]
 
	[MobileColumnAttribute(Context = "Folder_WorkList_ListView", TargetItems = nameof(Member)+ "," + nameof(CreatedDate)+ "," + nameof(Status)+ "," + nameof(Order)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Work_ListView", TargetItems = nameof(CreatedDate)+ "," + nameof(Member)+ "," + nameof(Name)+ "," + nameof(Status))]
	[MobileColumnAttribute(Context = "Work_LookupListView", TargetItems = nameof(CreatedDate)+ "," + nameof(Name)+ "," + nameof(Member)+ "," + nameof(Status))]
	[MobileColumnAttribute(Context = "Member_WorkList_ListView", TargetItems = nameof(Name)+ "," + nameof(CreatedDate)+ "," + nameof(Status))]
	[DefaultProperty("Code")]
 
	
	[CustomFilter("IMember", "[Member.Oid] = ? Or [MemberWorkList][[Member.Oid] = ?]")]
	[UpDownTopBottomOrder(Criteria = "[<Work>][^.Status.Order = Status.Order and ^.Member = Member and Oid = ?]", AscSort = false, ChangeBetweenRow = false, AutoSave = true)]
	[CustomFilter("IFilterDate", "Update")]
	[StatusColor(TargetItems = "Status")]
//[OptimisticLocking(false)]
    public partial class Work: DevExpress.Persistent.BaseImpl.BaseObject , IFilterDate, INewObjectSession ,IMember, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public Work(Session session)
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

               

		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(0)]		

 		[Size(20)]
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
		//Set Default Value

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
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(1)]		

 		[Size(150)]
		[RuleRequiredField("RequiredWorkName", DefaultContexts.Save)]
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

	
       
		//private string _reference;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tham chiếu")]
        [ToolTip("Tham chiếu")]
		//[Index(2)]		

 		[Size(100)]
		public string Reference
        { 
		    get => GetPropertyValue<string>("Reference");                         
			set => SetPropertyValue<string>("Reference", value); 
			
        }
		//Tooltip for Object
		public object ReferenceToolTipControllerText(View view)
        {
        //    if (Reference != null) 
		//			return Reference;
            return null;
        }
		//Get Default Value
        public string GetDefaultReference(View view = null)
        { 
			return Reference;
        }
		//Set Default Value
		public void SetDefaultReference(View view = null)
        {
            //if (Reference is null){
            //    var result = GetDefaultReference(view);
            //    if (result != null && result != Reference){
			//          Reference = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ReferenceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultReference();
				//if (result != null && Reference != null){
				//	return !Reference.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.WorkType _worktype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(WorkTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.WorkType WorkType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.WorkType>("WorkType");                         
			set => SetPropertyValue<Module.BusinessObjects.WorkType>("WorkType", value); 
			
        }
		//Tooltip for Object
		public object WorkTypeToolTipControllerText(View view)
        {
            #region 0460ImportCode 
if(WorkType != null) return WorkType.Name;
#endregion 0460ImportCode
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.WorkType GetDefaultWorkType(View view = null)
        { 
			return WorkType;
        }
		//Set Default Value
		public void SetDefaultWorkType(View view = null)
        {
            //if (WorkType is null){
            //    var result = GetDefaultWorkType(view);
            //    if (result != null && result != WorkType){
			//          WorkType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WorkTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWorkType();
				//if (result != null && WorkType != null){
				//	return !WorkType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator WorkTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(WorkType));
            }
        }
	
       
		//private int? _quantity;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Khối lượng")]
        [ToolTip("Khối lượng")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Quantity
        { 
		    get => GetPropertyValue<int?>("Quantity");                         
			set => SetPropertyValue<int?>("Quantity", value); 
			
        }
		//Tooltip for Object
		public object QuantityToolTipControllerText(View view)
        {
            #region 0312ImportCode 
if (WorkType != null && WorkType.Note != null)
 return WorkType.Note;
#endregion 0312ImportCode
            return null;
        }
		//Get Default Value
		//Set Default Value

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

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thành viên")]
        [ToolTip("Thành viên")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MemberCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Member-WorkList")]
	 
	    [RuleRequiredField("MemberRequired", DefaultContexts.Save)]
		public Module.BusinessObjects.Member Member
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Member");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Member", value); 
			
        }
		//Tooltip for Object
		public object MemberToolTipControllerText(View view)
        {
            #region 1002ImportCode 
if (MemberWorkList != null)
{
    var result = string.Join(
        Environment.NewLine,
        MemberWorkList
            .Where(n => n.Member != null && n.Member.Name != null)
            .OrderBy(n => n.Member.Name)
            .Select(n => n.Member.Name)
    );
    return result;
}
#endregion 1002ImportCode
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
	
       
		//private Module.BusinessObjects.Member _requester;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Đề xuất")]
        [ToolTip("Đề xuất")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(RequesterCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Member Requester
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Requester");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Requester", value); 
			
        }
		//Tooltip for Object
		public object RequesterToolTipControllerText(View view)
        {
        //    if (Requester != null) 
		//			return Requester;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool RequesterIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRequester();
				//if (result != null && Requester != null){
				//	return !Requester.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator RequesterCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Requester));
            }
        }
	
       
		//private Status _status;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Trạng thái")]
        [ToolTip("Trạng thái")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(StatusCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Status Status
        { 
		    get => GetPropertyValue<Status>("Status");                         
			set => SetPropertyValue<Status>("Status", value); 
			
        }
		//Tooltip for Object
		public object StatusToolTipControllerText(View view)
        {
        //    if (Status != null) 
		//			return Status;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool StatusIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStatus();
				//if (result != null && Status != null){
				//	return !Status.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator StatusCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Status));
            }
        }
	
       
		//private DateTime? _deadline;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hạn")]
        [ToolTip("Hạn")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy H:mm")]
		public DateTime? Deadline
        { 
		    get => GetPropertyValue<DateTime?>("Deadline");                         
			set => SetPropertyValue<DateTime?>("Deadline", value); 
			
        }
		//Tooltip for Object
		public object DeadlineToolTipControllerText(View view)
        {
            #region 0307ImportCode 
if(Deadline != null)
return Deadline.Value.ToString("H:mm");
#endregion 0307ImportCode
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultDeadline(View view = null)
        { 
			return Deadline;
        }
		//Set Default Value
		public void SetDefaultDeadline(View view = null)
        {
            //if (Deadline is null){
            //    var result = GetDefaultDeadline(view);
            //    if (result != null && result != Deadline){
			//          Deadline = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DeadlineIsNotValidate
        {
            get
            {
			#region 0521ImportCode 
if (Deadline != null && Status != null && Status.Code != "Completed")
                {
                    return Deadline.Value < Module.Helpers.XafXpoHelper.GetDateTimeNowFromServer(Session);
                }
#endregion 0521ImportCode                
   
                return false;
            }
        }

	
       
		//private string _note;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(9)]		

 		[Size(4000)]
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

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chi tiết")]
		//[Index(10)]
		[DevExpress.Xpo.Association("Work-WorkDetailList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.WorkDetail> WorkDetailList
        {      
		    get => GetCollection<Module.BusinessObjects.WorkDetail>("WorkDetailList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tham gia")]
		//[Index(11)]
		[DevExpress.Xpo.Association("Work-MemberWorkList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.MemberWork> MemberWorkList
        {      
		    get => GetCollection<Module.BusinessObjects.MemberWork>("MemberWorkList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lịch")]
		//[Index(12)]
		[DevExpress.Xpo.Association("Work-CalendarList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Calendar> CalendarList
        {      
		    get => GetCollection<Module.BusinessObjects.Calendar>("CalendarList"); 
			
        }
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(13)]		
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

	
       
		//private DateTime? _createddate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngày tạo")]
        [ToolTip("Ngày tạo")]
		//[Index(14)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy H:mm")]
	    [ModelDefault("AllowEdit", "False")]
		public DateTime? CreatedDate
        { 
		    get => GetPropertyValue<DateTime?>("CreatedDate");                         
			set => SetPropertyValue<DateTime?>("CreatedDate", value); 
			
        }
		//Tooltip for Object
		public object CreatedDateToolTipControllerText(View view)
        {
        //    if (CreatedDate != null) 
		//			return CreatedDate;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool CreatedDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCreatedDate();
				//if (result != null && CreatedDate != null){
				//	return !CreatedDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(15)]		
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
		//Set Default Value

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

	
       
		//private TimeSpan? _duration;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thời lượng")]
        [ToolTip("Thời lượng")]
		//[Index(16)]		
	    [ModelDefault("EditMaskType", "DateTime")]
		public TimeSpan? Duration
        { 
		    get => GetPropertyValue<TimeSpan?>("Duration");                         
			set => SetPropertyValue<TimeSpan?>("Duration", value); 
			
        }
		//Tooltip for Object
		public object DurationToolTipControllerText(View view)
        {
            #region 0272ImportCode 
if (WorkType != null && WorkType.Duration != null && Quantity != null)
{
                var totalDuration = (WorkType.Duration * Quantity);
                return string.Format("{0:n0}:{1:mm}",
                    Math.Round(totalDuration.Value.TotalHours, MidpointRounding.ToNegativeInfinity), totalDuration);
}
#endregion 0272ImportCode
            return null;
        }
		//Get Default Value
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
			#region 0522ImportCode 
if(Duration != null && WorkType != null && WorkType.Duration != null)
                    return Duration > WorkType.Duration;
#endregion 0522ImportCode                
   
                return false;
            }
        }

	
       
		//private System.Type _systemtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu hệ thống")]
        [ToolTip("Kiểu hệ thống")]
		//[Index(17)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SystemTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [TypeConverter(typeof(DevExpress.Persistent.Base.Security.SecurityTargetTypeConverter))]
	    [ValueConverter(typeof(DevExpress.ExpressApp.Utils.TypeToStringConverter))]
		public System.Type SystemType
        { 
		    get => GetPropertyValue<System.Type>("SystemType");                         
			set => SetPropertyValue<System.Type>("SystemType", value); 
			
        }
		//Tooltip for Object
		public object SystemTypeToolTipControllerText(View view)
        {
        //    if (SystemType != null) 
		//			return SystemType;
            return null;
        }
		//Get Default Value
        public System.Type GetDefaultSystemType(View view = null)
        { 
			return SystemType;
        }
		//Set Default Value
		public void SetDefaultSystemType(View view = null)
        {
            //if (SystemType is null){
            //    var result = GetDefaultSystemType(view);
            //    if (result != null && result != SystemType){
			//          SystemType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SystemTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSystemType();
				//if (result != null && SystemType != null){
				//	return !SystemType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SystemTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SystemType));
            }
        }
	
       
		//private System.Guid _objectid;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã đối tượng")]
        [ToolTip("Mã đối tượng")]
		//[Index(18)]		
	    [ModelDefault("AllowEdit", "False")]
		public System.Guid ObjectID
        { 
		    get => GetPropertyValue<System.Guid>("ObjectID");                         
			set => SetPropertyValue<System.Guid>("ObjectID", value); 
			
        }
		//Tooltip for Object
		public object ObjectIDToolTipControllerText(View view)
        {
        //    if (ObjectID != null) 
		//			return ObjectID;
            return null;
        }
		//Get Default Value
        public System.Guid GetDefaultObjectID(View view = null)
        { 
			return ObjectID;
        }
		//Set Default Value
		public void SetDefaultObjectID(View view = null)
        {
            //if (ObjectID is null){
            //    var result = GetDefaultObjectID(view);
            //    if (result != null && result != ObjectID){
			//          ObjectID = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ObjectIDIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultObjectID();
				//if (result != null && ObjectID != null){
				//	return !ObjectID.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Folder _folder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(19)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(FolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-WorkList")]
	 
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
		//Set Default Value

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
	
       
		//private bool _codeunique;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã duy nhất")]
        [ToolTip("Mã duy nhất")]
		//[Index(20)]		
	    [Browsable(false)]
		public bool CodeUnique
        { 
		    #region 0953ImportCode 
get
{
	if (CreatedDate != null)
	{
    		var time = new DateTime(CreatedDate.Value.Year, 1, 1);
		//Trường hợp chỉ lấy mã trên đối tượng này
    		var type = GetType();
		//Trường hợp lấy mã từ đối tượng cha
		//Type type = typeof(Module.BusinessObjects.ObjectType);
    		//var parser = CriteriaOperator.Parse("IsExactType(This,?) and Oid <> ? and Code = ? and CreatedDate >= ? and CreatedDate < ?", type.FullName, Oid,
        	//	Code, time,        time.AddYears(1));
    		var parser = CriteriaOperator.Parse("Oid <> ? and Code = ? and CreatedDate >= ? and CreatedDate < ?", Oid,
        		Code, time,        time.AddYears(1));

    		if (Member != null)
         	parser = CriteriaOperator.And(parser, CriteriaOperator.Parse("Member.Oid = ?", Member.Oid));
    		var result = Session.FindObject(type, parser);
    		return result != null;
	}
	return false;
}

#endregion 0953ImportCode
			
        }
		//Tooltip for Object
		public object CodeUniqueToolTipControllerText(View view)
        {
        //    if (CodeUnique != null) 
		//			return CodeUnique;
            return null;
        }
		//Get Default Value
        public bool GetDefaultCodeUnique(View view = null)
        { 
			return CodeUnique;
        }
		//Set Default Value
		public void SetDefaultCodeUnique(View view = null)
        {
            //if (CodeUnique is null){
            //    var result = GetDefaultCodeUnique(view);
            //    if (result != null && result != CodeUnique){
			//          CodeUnique = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CodeUniqueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCodeUnique();
				//if (result != null && CodeUnique != null){
				//	return !CodeUnique.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
        private bool _display;
        [Browsable(false)]
        [NonPersistent]
        [ImmediatePostData]
        public bool Display
        {
            get { return _display; }
            set { SetPropertyValue("Display", ref _display, value); }
        }
 


		public override void AfterConstruction()
        {
 
            #region 0350ImportCode
            base.AfterConstruction();
SetDefaultOrder();
SetDefaultRequester();
SetDefaultCreatedDate();
SetDefaultUpdate();
SetDefaultMember();
SetDefaultStatus();
SetDefaultQuantity();
SetDefaultCode();
            #endregion 0350ImportCode
            Display = true;
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultReference(View view = null);
        //SetDefaultWorkType(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultRequester(View view = null);
        //SetDefaultStatus(View view = null);
        //SetDefaultDeadline(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultCreatedDate(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultDuration(View view = null);
        //SetDefaultSystemType(View view = null);
        //SetDefaultObjectID(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultCodeUnique(View view = null);
			
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
            #region 0461ImportCode
            base.OnSaving();         
            if (Session.IsNewObject(this) && Member != null)
            {
                //028
                //-Thông báo với Thành viên
                //+Khi có công việc Người khác mới đề xuất
                CreateMemberNotify(Member.Oid);
            }
SetDefaultUpdate();
            #endregion 0461ImportCode
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
            Session.Delete(this.WorkDetailList);				
            Session.Delete(this.MemberWorkList);				
            Session.Delete(this.CalendarList);				
  
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
				
                    case nameof(Status):
                        OnChangedStatus(oldValue, newValue);
                        break;
				
                    case nameof(Requester):
                        OnChangedRequester(oldValue, newValue);
                        break;
				
                    case nameof(CreatedDate):
                        OnChangedCreatedDate(oldValue, newValue);
                        break;
				
                    case nameof(Member):
                        OnChangedMember(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedStatus(object oldValue, object newValue)
        {
            #region 0336ImportCode
                                //2023-08-01: OnChange Trạng thái: STT = STT của top Trạng thái mới + 1
                    if (newValue is null)
                        return;
                    IObjectSpace objectSpace = DevExpress.ExpressApp.Xpo.XPObjectSpace.FindObjectSpaceByObject(SecuritySystem.CurrentUser);
                    var topWork = objectSpace.Evaluate(typeof(Work),
                    DevExpress.Data.Filtering.CriteriaOperator.Parse("Max(Order)"),
                    DevExpress.Data.Filtering.CriteriaOperator.Parse("Status.Oid = ?", Status.Oid)); if (topWork is int)
                    {
                        Order = (int)topWork + 1;
                    }
                    else
                    {
                        Order = 1;
                    }
                    if(oldValue != null)
                    {
                        if (Status?.Code != "Doing")
                            Stop();
                        else
                            Play();
                    }
                    if(Member != null)
                    {
                        //028: Với Công việc
                        //- Thông báo với Thành viên
                        //+Khi có công việc Người khác mới đề xuất
                        //+Khi Trạng thái chuyển sang Đổi, hoặc từ Xong sang Dừng bởi người Đề xuất

                        if (Status.Code == "Changed")
                            CreateMemberNotify(Member.Oid);
                        else if(Status.Code == "NotCompleted" && Requester != null && 
                            Requester.Oid.Equals(SecuritySystem.CurrentUserId) && oldValue != null && 
                            ((Module.SystemObjects.Status)(oldValue)).Code == "Completed")
                        {
                            CreateMemberNotify(Member.Oid);
                        }
                    }
                    if (Requester != null)
                    {
                        //028: Với Công việc
                        //-Thông báo với Đề xuất
                        //+Khi công việc mình Đề xuất chuyển từ Chưa sang trạng thái Đang
                        //+Khi công việc mình Đề xuất chuyển

                        if (Status.Code == "Doing" && oldValue != null &&
                            ((Module.SystemObjects.Status)(oldValue)).Code == "NotStarted")
                        {
                            CreateMemberNotify(Requester.Oid);
                        }
                    }

                    //2023-08-18: Bỏ tiến trình			
                    //if (oldValue != null)
                    //{
                    //    var progress = new Progress(Session);
                    //    progress.Name = Name;
                    //    progress.ObjectType = this.GetType();
                    //    progress.ObjectID = Oid;
                    //    progress.OldStatus = oldValue as Status;
                    //    if (newValue != null)
                    //        progress.NewStatus = newValue as Status;
                    //}
            
            #endregion 0336ImportCode
        }               
        private void OnChangedRequester(object oldValue, object newValue)
        {
            #region 2651ImportCode
                                    //028: Với Công việc
                        //-Thông báo với Đề xuất
                        //+Khi công việc mình Đề xuất chuyển từ Chưa sang trạng thái Đang
                        //+Khi công việc mình Đề xuất chuyển sang hoàn thành

                        if(oldValue != null)
                        {
                            if (Status.Code == "Doing" && ((Module.SystemObjects.Status)(oldValue)).Code == "NotStarted")
                                CreateMemberNotify(Requester.Oid);
                            else if (Status.Code == "Completed")
                                CreateMemberNotify(Requester.Oid);
                        }            
            #endregion 2651ImportCode
        }               
        private void OnChangedCreatedDate(object oldValue, object newValue)
        {
            #region 1377ImportCode
            if (newValue is null) return;
SetDefaultFolder();            
            #endregion 1377ImportCode
        }               
        private void OnChangedMember(object oldValue, object newValue)
        {
            #region 0992ImportCode
            if (newValue is null) return;
SetDefaultCode();            
            #endregion 0992ImportCode
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
			//	SetDefaultNote();
			//	SetDefaultWorkDetailList();
			//	SetDefaultMemberWorkList();
			//	SetDefaultCalendarList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0247ImportCode
		public TimeSpan? GetDefaultDuration(View view = null)
        {
            //Code: 0247            Oid: bae06f55-fe00-4f4c-a754-7a62a9f96c41
                        if (CalendarList != null && CalendarList.Count > 0)
            {
                var totalDuration = TimeSpan.Zero;
                foreach (var calendar in CalendarList)
                {
                    if (calendar.EndTime != null)
                    {
                        totalDuration += calendar.EndTime.Value - calendar.StartTime;
                    }
                }

                if (!totalDuration.Equals(TimeSpan.Zero))
                    return totalDuration;
            }
            return null;
        }
#endregion 0247ImportCode
#region 0991ImportCode
		public void SetDefaultCreatedDate(View view = null)
        {
            //Code: 0991            Oid: bbf63fa3-577d-4cc5-9c48-e44a3c39cc6e
            if(CreatedDate == null) CreatedDate = GetDefaultCreatedDate();
        }
#endregion 0991ImportCode
#region 0253ImportCode
		public void Stop()
        {
            //Code: 0253            Oid: fcaacc61-4773-447b-b986-1f6eb4eda389
            //Dừng lịch của đối tượng này
foreach (var calendar in CalendarList)
{
    if (calendar.EndTime is null)
        calendar.EndTime = calendar.GetDefaultEndTime();
}
//if (Member != null && Member.Oid.Equals(SecuritySystem.CurrentUserId))
//{
//    //Tìm những đối tượng lịch đang chạy của user hiện tại
//    var doingCalendars = new XPCollection<Calendar>(PersistentCriteriaEvaluationBehavior.InTransaction, Session,
//        CriteriaOperator.Parse("EndTime is null and Work.Member.Oid = CurrentUserId()"));
//    foreach (var doingCalendar in doingCalendars)
//    {
//        doingCalendar.EndTime = doingCalendar.GetDefaultEndTime();
//    }
//}
        }
#endregion 0253ImportCode
#region 1375ImportCode
		public Module.BusinessObjects.Folder GetDefaultFolder(View view = null)
        {
            //Code: 1375            Oid: be1c3379-e35d-4c34-81b4-ab709019fbd8
            if(CreatedDate != null)
{
    return Session.FindObject<Folder>(DevExpress.Data.Filtering.CriteriaOperator.Parse("StartsWith(Name, 'Tuần') and CreatedDate < ? and CreatedDate > ?", CreatedDate.Value, CreatedDate.Value.AddDays(-7)));
}
return null;
        }
#endregion 1375ImportCode
#region 0002ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 0002            Oid: 9b6a0b3e-89d3-4fd4-b8a6-341cbd8fa1d5
                        var sort = new DevExpress.Xpo.SortProperty(nameof(CreatedDate), DevExpress.Xpo.DB.SortingDirection.Descending);
            var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Requester.Oid == CurrentUserId()");
            var lastObject = Module.Helpers.XafXpoHelper.GetLastedBySort(Session, this.GetType(), criteria, sort) as Work;
            if (lastObject != null)
                return lastObject.Member;
            return null;
        }
#endregion 0002ImportCode
#region 0254ImportCode
		public Status GetDefaultStatus(View view = null)
        {
            //Code: 0254            Oid: 855e557b-085c-48bd-8af3-f8eb4100d8aa
            return Session.FindObject<Status>(CriteriaOperator.Parse("Code = 'NotStarted'"));
        }
#endregion 0254ImportCode
#region 0952ImportCode
		public void SetDefaultCode(View view = null)
        {
            //Code: 0952            Oid: dae7c0ea-2e09-421d-a24b-4081ff008f0f
            Code= GetDefaultCode();
        }
#endregion 0952ImportCode
#region 0258ImportCode
		public void SetDefaultQuantity(View view = null)
        {
            //Code: 0258            Oid: fd8c5c75-808a-47cd-b336-f7c8d3536a92
            if(Quantity == null) Quantity = GetDefaultQuantity();
        }
#endregion 0258ImportCode
#region 0007ImportCode
		public void SetDefaultRequester(View view = null)
        {
            //Code: 0007            Oid: 5b8eb901-3747-44c1-b049-aa23f8a4adcd
            if(Requester == null) Requester = GetDefaultRequester();
        }
#endregion 0007ImportCode
#region 0005ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 0005            Oid: 6cde8f89-1d89-4186-96f2-6a0c2f5f0d12
            Order= GetDefaultOrder();
        }
#endregion 0005ImportCode
#region 0006ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 0006            Oid: aeb4aa25-1387-4071-88b5-ce931bfa7eb3
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 0006ImportCode
#region 0004ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 0004            Oid: e5901244-ae3f-44fa-b65c-61d0ace7490d
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0004ImportCode
#region 0003ImportCode
		public Module.BusinessObjects.Member GetDefaultRequester(View view = null)
        {
            //Code: 0003            Oid: 4629ff15-209a-491f-a79a-77607ee8f791
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 0003ImportCode
#region 0008ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0008            Oid: a31f33fd-b30a-4a25-933e-28c12670e121
            Update = GetDefaultUpdate();
        }
#endregion 0008ImportCode
#region 0990ImportCode
		public DateTime? GetDefaultCreatedDate(View view = null)
        {
            //Code: 0990            Oid: 1101b653-cc7d-4123-a749-c1c5e489910d
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0990ImportCode
#region 1376ImportCode
		public void SetDefaultFolder(View view = null)
        {
            //Code: 1376            Oid: c733b40b-a849-4319-b0fb-57f0992d65d4
                        if (Folder is null)
            {
                var result = GetDefaultFolder();
                if (result != null && result != Folder)
                {
                    Folder = result;
                }
            }
        }
#endregion 1376ImportCode
#region 0951ImportCode
		public string GetDefaultCode(View view = null)
        {
            //Code: 0951            Oid: a2e70d83-821d-4311-aae6-fd073e1f205c
            if(CreatedDate is null) return null;
var keyCodeObject =
    Module.Helpers.ParameterHelper.GetSettingParameter(Session, "CodeObject");
var parser = string.Format("and CreatedDate >='{0}-01-01' and CreatedDate <'{1}-01-01'",
                    CreatedDate.Value.Year,
                    CreatedDate.Value.Year + 1);
    if (Member != null) parser += string.Format(" and Member.Oid = '{0}' ", Member.Oid);

//Trường hợp chỉ lấy mã trên đối tượng này
Type currentType = this.GetType();
//Trường hợp lấy mã từ đối tượng cha
//Type currentType = typeof(ObjectType);

    //Kích thước mặc định là 4 số
    int size = 3;		
    return Tools.GetCode(currentType , this.Session, this.Oid, keyCodeObject != null ? keyCodeObject.Value : "", size ,
        parser);
return null;
        }
#endregion 0951ImportCode
#region 0255ImportCode
		public void SetDefaultStatus(View view = null)
        {
            //Code: 0255            Oid: 9a31e0bd-a7f8-4df6-86b9-43e0736543f8
            if(Status is null) Status = GetDefaultStatus();
        }
#endregion 0255ImportCode
#region 0145ImportCode
		public void SetDefaultName(View view = null)
        {
            //Code: 0145            Oid: 5115bce8-beaf-46a8-8cab-af89cb555a27
            if(string.IsNullOrEmpty(Name)) Name = GetDefaultName();
        }
#endregion 0145ImportCode
#region 0001ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 0001            Oid: 9516740f-6cc6-4606-8303-e6a1ecadf858
            //if (Plan != null)
//{
    //var lasted = Plan.WorkList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    //if (lasted != null)
    //    return lasted.Order + 1;
    //return 1;
//}
return null;
        }
#endregion 0001ImportCode
#region 0257ImportCode
		public int? GetDefaultQuantity(View view = null)
        {
            //Code: 0257            Oid: 9d57923f-ea34-42a0-8f3c-884892d761d3
                return 1;
        }
#endregion 0257ImportCode
#region 0252ImportCode
		public void Play()
        {
            //Code: 0252            Oid: d83e800a-bcbc-47de-90cd-1b03eb6f3ca3
            if (Status != null && Status.Code == "Doing")
{
    var doingCalendar = new Calendar(Session);
    doingCalendar.Work = this;
    CalendarList.Add(doingCalendar);
    doingCalendar.StartTime = Module.Helpers.XafXpoHelper.GetDateTimeNowFromServer(Session);
    doingCalendar.Content = this.Name;
}
        }
#endregion 0252ImportCode
        #endregion
//Mã nguồn bổ sung
#region WorkImportCode
        private void CreateMemberNotify(Guid userId)
        {
            if (string.IsNullOrEmpty(Name) || Status is null || !userId.Equals(SecuritySystem.CurrentUserId))
                return;
            var userNotify = new Module.SystemObjects.UserNotifications(Session)
            {
                AlarmTime = Module.Helpers.XafXpoHelper.GetDateTimeNowFromServer(Session),
                ObjectId = Oid,
                ObjectType = this.GetType().FullName,
                CurrentUserId = userId,
                Subject = $"{Status.Name} : {Name}"
            };
        }
#endregion WorkImportCode
		 		 
    }
}
