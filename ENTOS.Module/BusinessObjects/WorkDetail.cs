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
    [ModelDefault("Caption", "Chi tiết công việc"), ImageName("WorkDetail")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(Order))]
 
	[MobileColumnAttribute(Context = "Work2_WorkDetailList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "WorkDetail_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Work_WorkDetailList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "WorkType_WorkDetailList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "WorkDetail_ListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
	[UpDownTopBottomOrder(Criteria = "[<WorkDetail>][(^.WorkType = WorkType Or ^.Process = Process) and Oid = ?]", AscSort = true, ChangeBetweenRow = false, AutoSave = false)]
[OptimisticLocking(true)]
    public partial class WorkDetail:  DevExpress.Xpo.XPLiteObject , IReOrder , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public WorkDetail(Session session)
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

 		[Size(250)]
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

	
       
		//private string _note;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(1)]		

 		[Size(4000)]
	    [ModelDefault("DocumentStorageFormat", "Html")]
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

	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(2)]		
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

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(3)]		
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

	
       
		//private bool _done;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Xong")]
        [ToolTip("Xong")]
		//[Index(4)]		
		public bool Done
        { 
		    get => GetPropertyValue<bool>("Done");                         
			set => SetPropertyValue<bool>("Done", value); 
			
        }
		//Tooltip for Object
		public object DoneToolTipControllerText(View view)
        {
        //    if (Done != null) 
		//			return Done;
            return null;
        }
		//Get Default Value
        public bool GetDefaultDone(View view = null)
        { 
			return Done;
        }
		//Set Default Value
		public void SetDefaultDone(View view = null)
        {
            //if (Done is null){
            //    var result = GetDefaultDone(view);
            //    if (result != null && result != Done){
			//          Done = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DoneIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDone();
				//if (result != null && Done != null){
				//	return !Done.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Work _work;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Công việc")]
        [ToolTip("Công việc")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(WorkCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Work-WorkDetailList")]
	 
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
	
       
		//private Module.BusinessObjects.WorkType _worktype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại công việc")]
        [ToolTip("Loại công việc")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(WorkTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("WorkType-WorkDetailList")]
	 
		public Module.BusinessObjects.WorkType WorkType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.WorkType>("WorkType");                         
			set => SetPropertyValue<Module.BusinessObjects.WorkType>("WorkType", value); 
			
        }
		//Tooltip for Object
		public object WorkTypeToolTipControllerText(View view)
        {
        //    if (WorkType != null) 
		//			return WorkType;
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
	
       
		//private System.Type _systemtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu hệ thống")]
        [ToolTip("Kiểu hệ thống")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SystemTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [ValueConverter(typeof(DevExpress.ExpressApp.Utils.TypeToStringConverter))]
	    [TypeConverter(typeof(DevExpress.Persistent.Base.Security.SecurityTargetTypeConverter))]
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
	
       
		//private System.Guid? _objectid;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã đối tượng")]
        [ToolTip("Mã đối tượng")]
		//[Index(9)]		
		public System.Guid? ObjectID
        { 
		    get => GetPropertyValue<System.Guid?>("ObjectID");                         
			set => SetPropertyValue<System.Guid?>("ObjectID", value); 
			
        }
		//Tooltip for Object
		public object ObjectIDToolTipControllerText(View view)
        {
        //    if (ObjectID != null) 
		//			return ObjectID;
            return null;
        }
		//Get Default Value
        public System.Guid? GetDefaultObjectID(View view = null)
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0420ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
SetDefaultOrder();
            #endregion 0420ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultDone(View view = null);
        //SetDefaultWork(View view = null);
        //SetDefaultWorkType(View view = null);
        //SetDefaultSystemType(View view = null);
        //SetDefaultObjectID(View view = null);
			
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
				
                    case nameof(WorkType):
                        OnChangedWorkType(oldValue, newValue);
                        break;
				
                    case nameof(Work):
                        OnChangedWork(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedWorkType(object oldValue, object newValue)
        {
            #region 0996ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 0996ImportCode
        }               
        private void OnChangedWork(object oldValue, object newValue)
        {
            #region 1284ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 1284ImportCode
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
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0071ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0071            Oid: 912d05f5-2262-4dea-8762-767e41de0b56
            Update = GetDefaultUpdate();
        }
#endregion 0071ImportCode
#region 0993ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 0993            Oid: 03e047af-bd4f-40f9-8d52-e394c00fe1b0
            //if (Process != null && Process.WorkDetailList != null)
//{
//    var lasted = Process.WorkDetailList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
//    if (lasted != null)
//        return lasted.Order + 1;
//    return 1;
//}
if (WorkType!= null && WorkType.WorkDetailList != null)
{
    var lasted = WorkType.WorkDetailList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
if (Work!= null && Work.WorkDetailList != null)
{
    var lasted = Work.WorkDetailList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
return null;
        }
#endregion 0993ImportCode
#region 0098ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0098            Oid: be93d6be-5ead-4019-acd9-273af30331ea
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0098ImportCode
#region 0994ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 0994            Oid: e10567e5-0df8-4320-ac92-e4d941165e0d
            Order= GetDefaultOrder();
        }
#endregion 0994ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
