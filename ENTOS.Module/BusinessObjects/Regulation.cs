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
    [ModelDefault("Caption", "Quy định"), ImageName("Regulation")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Code)+ "," + nameof(Update))]
 
	[MobileColumnAttribute(Context = "Regulation_LookupListView", TargetItems = nameof(Update)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Regulation_ListView", TargetItems = nameof(Code)+ "," + nameof(Status)+ "," + nameof(Name))]
	[DefaultProperty("Name")]
 
	[StatusColor(TargetItems = "Status")]
[OptimisticLocking(true)]
    public partial class Regulation:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Regulation(Session session)
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
		[DevExpress.Xpo.DisplayName("Quy định")]
        [ToolTip("Quy định")]
		//[Index(0)]		

 		[Size(10)]
		[RuleUniqueValue("UniqueRegulationCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredRegulationCode", DefaultContexts.Save)]
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

 		[Size(200)]
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

	
       
		//private DateTime _date;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
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

	
       
		//private Status _status;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Trạng thái")]
        [ToolTip("Trạng thái")]
		//[Index(3)]		
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
        public Status GetDefaultStatus(View view = null)
        { 
			return Status;
        }
		//Set Default Value
		public void SetDefaultStatus(View view = null)
        {
            //if (Status is null){
            //    var result = GetDefaultStatus(view);
            //    if (result != null && result != Status){
			//          Status = result;
            //	  }
            //}
        }

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
	
       
		//private Module.BusinessObjects.Member _approver;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Phê duyệt")]
        [ToolTip("Phê duyệt")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ApproverCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Member Approver
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Approver");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Approver", value); 
			
        }
		//Tooltip for Object
		public object ApproverToolTipControllerText(View view)
        {
        //    if (Approver != null) 
		//			return Approver;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Member GetDefaultApprover(View view = null)
        { 
			return Approver;
        }
		//Set Default Value
		public void SetDefaultApprover(View view = null)
        {
            //if (Approver is null){
            //    var result = GetDefaultApprover(view);
            //    if (result != null && result != Approver){
			//          Approver = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ApproverIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultApprover();
				//if (result != null && Approver != null){
				//	return !Approver.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ApproverCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Approver));
            }
        }
	
       
		//private int? _point;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Điểm")]
        [ToolTip("Điểm")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Point
        { 
		    get => GetPropertyValue<int?>("Point");                         
			set => SetPropertyValue<int?>("Point", value); 
			
        }
		//Tooltip for Object
		public object PointToolTipControllerText(View view)
        {
        //    if (Point != null) 
		//			return Point;
            return null;
        }
		//Get Default Value
        public int? GetDefaultPoint(View view = null)
        { 
			return Point;
        }
		//Set Default Value
		public void SetDefaultPoint(View view = null)
        {
            //if (Point is null){
            //    var result = GetDefaultPoint(view);
            //    if (result != null && result != Point){
			//          Point = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PointIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPoint();
				//if (result != null && Point != null){
				//	return !Point.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _content;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(6)]		

 		[Size(4000)]
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

	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(7)]		
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
 
            #region 0418ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
SetDefaultCode();
            #endregion 0418ImportCode
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultDate(View view = null);
        //SetDefaultStatus(View view = null);
        //SetDefaultApprover(View view = null);
        //SetDefaultPoint(View view = null);
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
            #region 0502ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0502ImportCode
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
			//	SetDefaultContent();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0444ImportCode
		public void SetDefaultCode(View view = null)
        {
            //Code: 0444            Oid: 637bda07-9f42-4bbe-8736-953c3950a55a
            if(String.IsNullOrEmpty(Code)) Code = GetDefaultCode();
        }
#endregion 0444ImportCode
#region 0443ImportCode
		public string GetDefaultCode(View view = null)
        {
            //Code: 0443            Oid: 44c54831-ff9f-4ad4-b47c-1a59a0f2a2ef
            var keyCodeObject =
    Module.Helpers.ParameterHelper.GetSettingParameter(Session, "CodeObject");
//Kích thước mặc định là 3 số
int size = 3;
return Tools.GetCode(this.GetType(), this.Session, this.Oid, keyCodeObject != null ? keyCodeObject.Value : "", size,
    " ");
return null;
        }
#endregion 0443ImportCode
#region 0074ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0074            Oid: d184e1a1-a199-4c13-8f1f-49dbef5b1e04
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0074ImportCode
#region 0162ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0162            Oid: 51bb26fb-9b1f-4d4d-8131-2971d572c278
            Update = GetDefaultUpdate();
        }
#endregion 0162ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
