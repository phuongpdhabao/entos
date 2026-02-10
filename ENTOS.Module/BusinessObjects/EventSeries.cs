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
	[NavigationItem("Communication")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Dòng sự kiện"), ImageName("EventSeries")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update))]
 
	[MobileColumnAttribute(Context = "EventSeries_ListView", TargetItems = nameof(Photo)+ "," + nameof(Domain)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "EventSeries_LookupListView", TargetItems = nameof(Name)+ "," + nameof(Domain)+ "," + nameof(Photo))]
	[DefaultProperty("English")]
 
[OptimisticLocking(true)]
    public partial class EventSeries:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public EventSeries(Session session)
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
				if (BusinessRoleList.IsLoaded)
                {
                    if (BusinessRoleList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(BusinessRoleList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(BusinessRoleList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.BusinessRole>(CriteriaOperator.Parse("[EventSeries.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool businessrolelist = Session.Query<Module.BusinessObjects.BusinessRole>().Where(x => x.EventSeries.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(BusinessRoleList), businessrolelist);
                        if (businessrolelist)
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

	
       
		//private string _english;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tiếng Anh")]
        [ToolTip("Tiếng Anh")]
		//[Index(1)]		

 		[Size(100)]
		[RuleUniqueValue("UniqueEventSeriesEnglish", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredEventSeriesEnglish", DefaultContexts.Save)]
		public string English
        { 
		    get => GetPropertyValue<string>("English");                         
			set => SetPropertyValue<string>("English", value); 
			
        }
		//Tooltip for Object
		public object EnglishToolTipControllerText(View view)
        {
        //    if (English != null) 
		//			return English;
            return null;
        }
		//Get Default Value
        public string GetDefaultEnglish(View view = null)
        { 
			return English;
        }
		//Set Default Value
		public void SetDefaultEnglish(View view = null)
        {
            //if (English is null){
            //    var result = GetDefaultEnglish(view);
            //    if (result != null && result != English){
			//          English = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EnglishIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEnglish();
				//if (result != null && English != null){
				//	return !English.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private EventType _eventtype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(2)]		
		public EventType EventType
        { 
		    get => GetPropertyValue<EventType>("EventType");                         
			set => SetPropertyValue<EventType>("EventType", value); 
			
        }
		//Tooltip for Object
		public object EventTypeToolTipControllerText(View view)
        {
        //    if (EventType != null) 
		//			return EventType;
            return null;
        }
		//Get Default Value
        public EventType GetDefaultEventType(View view = null)
        { 
			return EventType;
        }
		//Set Default Value
		public void SetDefaultEventType(View view = null)
        {
            //if (EventType is null){
            //    var result = GetDefaultEventType(view);
            //    if (result != null && result != EventType){
			//          EventType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EventTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEventType();
				//if (result != null && EventType != null){
				//	return !EventType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _org;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tổ chức")]
        [ToolTip("Tổ chức")]
		//[Index(3)]		

 		[Size(100)]
		public string Org
        { 
		    get => GetPropertyValue<string>("Org");                         
			set => SetPropertyValue<string>("Org", value); 
			
        }
		//Tooltip for Object
		public object OrgToolTipControllerText(View view)
        {
        //    if (Org != null) 
		//			return Org;
            return null;
        }
		//Get Default Value
        public string GetDefaultOrg(View view = null)
        { 
			return Org;
        }
		//Set Default Value
		public void SetDefaultOrg(View view = null)
        {
            //if (Org is null){
            //    var result = GetDefaultOrg(view);
            //    if (result != null && result != Org){
			//          Org = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OrgIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrg();
				//if (result != null && Org != null){
				//	return !Org.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Member _manager;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ManagerCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Member Manager
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Manager");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Manager", value); 
			
        }
		//Tooltip for Object
		public object ManagerToolTipControllerText(View view)
        {
        //    if (Manager != null) 
		//			return Manager;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Member GetDefaultManager(View view = null)
        { 
			return Manager;
        }
		//Set Default Value
		public void SetDefaultManager(View view = null)
        {
            //if (Manager is null){
            //    var result = GetDefaultManager(view);
            //    if (result != null && result != Manager){
			//          Manager = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ManagerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultManager();
				//if (result != null && Manager != null){
				//	return !Manager.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ManagerCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Manager));
            }
        }
	
       
		//private byte[] _photo;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ảnh")]
        [ToolTip("Ảnh")]
		//[Index(5)]		
		[Appearance("ẢnhBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Photo
        { 
		    get => GetPropertyValue<byte[]>("Photo");                         
			set => SetPropertyValue<byte[]>("Photo", value); 
			
        }
		//Tooltip for Object
		public object PhotoToolTipControllerText(View view)
        {
        //    if (Photo != null) 
		//			return Photo;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultPhoto(View view = null)
        { 
			return Photo;
        }
		//Set Default Value
		public void SetDefaultPhoto(View view = null)
        {
            //if (Photo is null){
            //    var result = GetDefaultPhoto(view);
            //    if (result != null && result != Photo){
			//          Photo = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PhotoIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPhoto();
				//if (result != null && Photo != null){
				//	return !Photo.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _cycle;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Chu kỳ")]
        [ToolTip("Chu kỳ")]
		//[Index(6)]		

 		[Size(100)]
		public string Cycle
        { 
		    get => GetPropertyValue<string>("Cycle");                         
			set => SetPropertyValue<string>("Cycle", value); 
			
        }
		//Tooltip for Object
		public object CycleToolTipControllerText(View view)
        {
        //    if (Cycle != null) 
		//			return Cycle;
            return null;
        }
		//Get Default Value
        public string GetDefaultCycle(View view = null)
        { 
			return Cycle;
        }
		//Set Default Value
		public void SetDefaultCycle(View view = null)
        {
            //if (Cycle is null){
            //    var result = GetDefaultCycle(view);
            //    if (result != null && result != Cycle){
			//          Cycle = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CycleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCycle();
				//if (result != null && Cycle != null){
				//	return !Cycle.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime _date;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(7)]		
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

	
       
		//private Module.BusinessObjects.Domain _domain;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Chủ đề")]
        [ToolTip("Chủ đề")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DomainCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Domain Domain
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Domain>("Domain");                         
			set => SetPropertyValue<Module.BusinessObjects.Domain>("Domain", value); 
			
        }
		//Tooltip for Object
		public object DomainToolTipControllerText(View view)
        {
        //    if (Domain != null) 
		//			return Domain;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Domain GetDefaultDomain(View view = null)
        { 
			return Domain;
        }
		//Set Default Value
		public void SetDefaultDomain(View view = null)
        {
            //if (Domain is null){
            //    var result = GetDefaultDomain(view);
            //    if (result != null && result != Domain){
			//          Domain = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DomainIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDomain();
				//if (result != null && Domain != null){
				//	return !Domain.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator DomainCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Domain));
            }
        }
	
       
		//private Module.BusinessObjects.Space _space;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Địa bàn")]
        [ToolTip("Địa bàn")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SpaceCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Space Space
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Space>("Space");                         
			set => SetPropertyValue<Module.BusinessObjects.Space>("Space", value); 
			
        }
		//Tooltip for Object
		public object SpaceToolTipControllerText(View view)
        {
        //    if (Space != null) 
		//			return Space;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Space GetDefaultSpace(View view = null)
        { 
			return Space;
        }
		//Set Default Value
		public void SetDefaultSpace(View view = null)
        {
            //if (Space is null){
            //    var result = GetDefaultSpace(view);
            //    if (result != null && result != Space){
			//          Space = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpaceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpace();
				//if (result != null && Space != null){
				//	return !Space.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SpaceCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Space));
            }
        }
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vai trò")]
		//[Index(10)]
		[DevExpress.Xpo.Association("EventSeries-BusinessRoleList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BusinessRole> BusinessRoleList
        {      
		    get => GetCollection<Module.BusinessObjects.BusinessRole>("BusinessRoleList"); 
			
        }
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(11)]		
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

	
       
		//private bool _local;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nội bộ")]
        [ToolTip("Nội bộ")]
		//[Index(12)]		
		public bool Local
        { 
		    get => GetPropertyValue<bool>("Local");                         
			set => SetPropertyValue<bool>("Local", value); 
			
        }
		//Tooltip for Object
		public object LocalToolTipControllerText(View view)
        {
        //    if (Local != null) 
		//			return Local;
            return null;
        }
		//Get Default Value
        public bool GetDefaultLocal(View view = null)
        { 
			return Local;
        }
		//Set Default Value
		public void SetDefaultLocal(View view = null)
        {
            //if (Local is null){
            //    var result = GetDefaultLocal(view);
            //    if (result != null && result != Local){
			//          Local = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LocalIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLocal();
				//if (result != null && Local != null){
				//	return !Local.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool? _gender;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giới tính")]
        [ToolTip("Giới tính")]
		//[Index(13)]		
		public bool? Gender
        { 
		    get => GetPropertyValue<bool?>("Gender");                         
			set => SetPropertyValue<bool?>("Gender", value); 
			
        }
		//Tooltip for Object
		public object GenderToolTipControllerText(View view)
        {
        //    if (Gender != null) 
		//			return Gender;
            return null;
        }
		//Get Default Value
        public bool? GetDefaultGender(View view = null)
        { 
			return Gender;
        }
		//Set Default Value
		public void SetDefaultGender(View view = null)
        {
            //if (Gender is null){
            //    var result = GetDefaultGender(view);
            //    if (result != null && result != Gender){
			//          Gender = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool GenderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultGender();
				//if (result != null && Gender != null){
				//	return !Gender.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0428ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
            #endregion 0428ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultEnglish(View view = null);
        //SetDefaultEventType(View view = null);
        //SetDefaultOrg(View view = null);
        //SetDefaultManager(View view = null);
        //SetDefaultPhoto(View view = null);
        //SetDefaultCycle(View view = null);
        //SetDefaultDate(View view = null);
        //SetDefaultDomain(View view = null);
        //SetDefaultSpace(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultLocal(View view = null);
        //SetDefaultGender(View view = null);
			
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
            #region 0476ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0476ImportCode
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
			//	SetDefaultBusinessRoleList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0083ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0083            Oid: 74e05d35-f240-47ea-803b-2bfc9fd9e250
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0083ImportCode
#region 0133ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0133            Oid: 4b6eb406-3745-4d1f-9c02-4d7db9d3ec58
            Update = GetDefaultUpdate();
        }
#endregion 0133ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
