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
    [ModelDefault("Caption", "Vai trò công việc"), ImageName("BusinessRole")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Order)+ "," + nameof(Update))]
 
	[MobileColumnAttribute(Context = "BusinessRole_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ContentType_BusinessRole_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "BusinessRole_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "EventSeries_BusinessRoleList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Domain_BusinessRoleList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Org_BusinessRoleList_ListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class BusinessRole:  DevExpress.Xpo.XPLiteObject , IReOrder , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public BusinessRole(Session session)
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

 		[Size(100)]
		[RuleUniqueValue("UniqueBusinessRoleName", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredBusinessRoleName", DefaultContexts.Save)]
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

	
       
		//private string _namee;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tiếng Anh")]
        [ToolTip("Tiếng Anh")]
		//[Index(1)]		

 		[Size(100)]
		[RuleUniqueValue("UniqueBusinessRoleNameE", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		public string NameE
        { 
		    get => GetPropertyValue<string>("NameE");                         
			set => SetPropertyValue<string>("NameE", value); 
			
        }
		//Tooltip for Object
		public object NameEToolTipControllerText(View view)
        {
        //    if (NameE != null) 
		//			return NameE;
            return null;
        }
		//Get Default Value
        public string GetDefaultNameE(View view = null)
        { 
			return NameE;
        }
		//Set Default Value
		public void SetDefaultNameE(View view = null)
        {
            //if (NameE is null){
            //    var result = GetDefaultNameE(view);
            //    if (result != null && result != NameE){
			//          NameE = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NameEIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNameE();
				//if (result != null && NameE != null){
				//	return !NameE.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(2)]		

 		[Size(20)]
		[RuleUniqueValue("UniqueBusinessRoleCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredBusinessRoleCode", DefaultContexts.Save)]
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
        public string GetDefaultCode(View view = null)
        { 
			return Code;
        }
		//Set Default Value
		public void SetDefaultCode(View view = null)
        {
            //if (Code is null){
            //    var result = GetDefaultCode(view);
            //    if (result != null && result != Code){
			//          Code = result;
            //	  }
            //}
        }

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

	
       
		//private string _group;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Nhóm")]
        [ToolTip("Nhóm")]
		//[Index(3)]		

 		[Size(150)]
		public string Group
        { 
		    get => GetPropertyValue<string>("Group");                         
			set => SetPropertyValue<string>("Group", value); 
			
        }
		//Tooltip for Object
		public object GroupToolTipControllerText(View view)
        {
        //    if (Group != null) 
		//			return Group;
            return null;
        }
		//Get Default Value
        public string GetDefaultGroup(View view = null)
        { 
			return Group;
        }
		//Set Default Value
		public void SetDefaultGroup(View view = null)
        {
            //if (Group is null){
            //    var result = GetDefaultGroup(view);
            //    if (result != null && result != Group){
			//          Group = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool GroupIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultGroup();
				//if (result != null && Group != null){
				//	return !Group.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(4)]		
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

	
       
		//private Module.BusinessObjects.EventSeries _eventseries;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dòng sự kiện")]
        [ToolTip("Dòng sự kiện")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(EventSeriesCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("EventSeries-BusinessRoleList")]
	 
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
	
       
		//private Module.BusinessObjects.Org _org;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tổ chức")]
        [ToolTip("Tổ chức")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OrgCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Org-BusinessRoleList")]
	 
		public Module.BusinessObjects.Org Org
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Org>("Org");                         
			set => SetPropertyValue<Module.BusinessObjects.Org>("Org", value); 
			
        }
		//Tooltip for Object
		public object OrgToolTipControllerText(View view)
        {
        //    if (Org != null) 
		//			return Org;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Org GetDefaultOrg(View view = null)
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

		private CriteriaOperator OrgCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Org));
            }
        }
	
       
		//private Module.BusinessObjects.Domain _domain;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lĩnh vực")]
        [ToolTip("Lĩnh vực")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DomainCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Domain-BusinessRoleList")]
	 
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
	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(9)]		
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultNameE(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultGroup(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultEventSeries(View view = null);
        //SetDefaultOrg(View view = null);
        //SetDefaultDomain(View view = null);
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
            #region 2490ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 2490ImportCode
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
				
                    case nameof(Org):
                        OnChangedOrg(oldValue, newValue);
                        break;
				
                    case nameof(Domain):
                        OnChangedDomain(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedOrg(object oldValue, object newValue)
        {
            #region 2503ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 2503ImportCode
        }               
        private void OnChangedDomain(object oldValue, object newValue)
        {
            #region 2497ImportCode
            base.AfterConstruction();
SetDefaultOrder();            
            #endregion 2497ImportCode
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
#region 2491ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 2491            Oid: 95096b03-8542-49d4-84ac-dcf7918e3c05
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 2491ImportCode
#region 2494ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 2494            Oid: b7a8fb2b-1c8d-413a-9abd-5c2b99d561b1
            Order= GetDefaultOrder();
        }
#endregion 2494ImportCode
#region 2493ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 2493            Oid: 2dc1217a-cc0a-4cbe-a3d4-b76c30d162e4
            if (Org != null && Org.BusinessRoleList != null)
{
    var lasted = Org.BusinessRoleList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
if (Domain != null && Domain.BusinessRoleList != null)
{
    var lasted = Domain.BusinessRoleList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
if (EventSeries != null && EventSeries.BusinessRoleList != null)
{
    var lasted = EventSeries.BusinessRoleList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
return null;

        }
#endregion 2493ImportCode
#region 2489ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 2489            Oid: efc4314d-6e06-4aae-9845-480a9a7a271f
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 2489ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
