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
    [ModelDefault("Caption", "Thời gian"), ImageName("Time")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "Time_LowerTimeList_ListView", TargetItems = nameof(Name)+ "," + nameof(Date))]
	[MobileColumnAttribute(Context = "Time_ListView", TargetItems = nameof(Date)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Time_LookupListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Time:  DevExpress.Xpo.XPLiteObject , DevExpress.Persistent.Base.General.ITreeNode , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Time(Session session)
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
				if (LowerTimeList.IsLoaded)
                {
                    if (LowerTimeList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(LowerTimeList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(LowerTimeList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Time>(CriteriaOperator.Parse("[UpperTime.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool lowertimelist = Session.Query<Module.BusinessObjects.Time>().Where(x => x.UpperTime.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(LowerTimeList), lowertimelist);
                        if (lowertimelist)
                            return true;

                    }                    
                }				
                                
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
               

		//private MomentType _timeunit;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đơn vị")]
        [ToolTip("Đơn vị")]
		//[Index(0)]		
		public MomentType TimeUnit
        { 
		    get => GetPropertyValue<MomentType>("TimeUnit");                         
			set => SetPropertyValue<MomentType>("TimeUnit", value); 
			
        }
		//Tooltip for Object
		public object TimeUnitToolTipControllerText(View view)
        {
        //    if (TimeUnit != null) 
		//			return TimeUnit;
            return null;
        }
		//Get Default Value
        public MomentType GetDefaultTimeUnit(View view = null)
        { 
			return TimeUnit;
        }
		//Set Default Value
		public void SetDefaultTimeUnit(View view = null)
        {
            //if (TimeUnit is null){
            //    var result = GetDefaultTimeUnit(view);
            //    if (result != null && result != TimeUnit){
			//          TimeUnit = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TimeUnitIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTimeUnit();
				//if (result != null && TimeUnit != null){
				//	return !TimeUnit.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int _value;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá trị")]
        [ToolTip("Giá trị")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int Value
        { 
		    get => GetPropertyValue<int>("Value");                         
			set => SetPropertyValue<int>("Value", value); 
			
        }
		//Tooltip for Object
		public object ValueToolTipControllerText(View view)
        {
        //    if (Value != null) 
		//			return Value;
            return null;
        }
		//Get Default Value
        public int GetDefaultValue(View view = null)
        { 
			return Value;
        }
		//Set Default Value
		public void SetDefaultValue(View view = null)
        {
            //if (Value is null){
            //    var result = GetDefaultValue(view);
            //    if (result != null && result != Value){
			//          Value = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultValue();
				//if (result != null && Value != null){
				//	return !Value.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Time _uppertime;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpperTimeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("UpperTime-LowerTimeList")]
	 
		public Module.BusinessObjects.Time UpperTime
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Time>("UpperTime");                         
			set => SetPropertyValue<Module.BusinessObjects.Time>("UpperTime", value); 
			
        }
		//Tooltip for Object
		public object UpperTimeToolTipControllerText(View view)
        {
        //    if (UpperTime != null) 
		//			return UpperTime;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Time GetDefaultUpperTime(View view = null)
        { 
			return UpperTime;
        }
		//Set Default Value
		public void SetDefaultUpperTime(View view = null)
        {
            //if (UpperTime is null){
            //    var result = GetDefaultUpperTime(view);
            //    if (result != null && result != UpperTime){
			//          UpperTime = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperTimeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpperTime();
				//if (result != null && UpperTime != null){
				//	return !UpperTime.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpperTimeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(UpperTime));
            }
        }
	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(3)]		

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

	
       
		//private DateTime _date;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(4)]		
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

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp dưới")]
		//[Index(5)]
		[DevExpress.Xpo.Association("UpperTime-LowerTimeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Time> LowerTimeList
        {      
		    get => GetCollection<Module.BusinessObjects.Time>("LowerTimeList"); 
			
        }
       
		//private DevExpress.Persistent.Base.General.ITreeNode _parent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Parent")]
        [ToolTip("Parent")]
		//[Index(6)]		
	    [Browsable(false)]
		public DevExpress.Persistent.Base.General.ITreeNode Parent
        { 
		    #region 0291ImportCode 
get => UpperTime;
#endregion 0291ImportCode
			
        }
		//Tooltip for Object
		public object ParentToolTipControllerText(View view)
        {
        //    if (Parent != null) 
		//			return Parent;
            return null;
        }
		//Get Default Value
        public DevExpress.Persistent.Base.General.ITreeNode GetDefaultParent(View view = null)
        { 
			return Parent;
        }
		//Set Default Value
		public void SetDefaultParent(View view = null)
        {
            //if (Parent is null){
            //    var result = GetDefaultParent(view);
            //    if (result != null && result != Parent){
			//          Parent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ParentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultParent();
				//if (result != null && Parent != null){
				//	return !Parent.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private System.ComponentModel.IBindingList _children;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Children")]
        [ToolTip("Children")]
		//[Index(7)]		
	    [Browsable(false)]
		public System.ComponentModel.IBindingList Children
        { 
		    #region 0300ImportCode 
get => LowerTimeList;
#endregion 0300ImportCode
			
        }
		//Tooltip for Object
		public object ChildrenToolTipControllerText(View view)
        {
        //    if (Children != null) 
		//			return Children;
            return null;
        }
		//Get Default Value
        public System.ComponentModel.IBindingList GetDefaultChildren(View view = null)
        { 
			return Children;
        }
		//Set Default Value
		public void SetDefaultChildren(View view = null)
        {
            //if (Children is null){
            //    var result = GetDefaultChildren(view);
            //    if (result != null && result != Children){
			//          Children = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ChildrenIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultChildren();
				//if (result != null && Children != null){
				//	return !Children.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultTimeUnit(View view = null);
        //SetDefaultValue(View view = null);
        //SetDefaultUpperTime(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultDate(View view = null);
        //SetDefaultParent(View view = null);
        //SetDefaultChildren(View view = null);
			
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
			//	SetDefaultLowerTimeList();
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
