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
	[NavigationItem("DataManagement")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Loại dịch vụ"), ImageName("SoftwareServiceType")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(Update)+ "," + nameof(Updater))]
 
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class SoftwareServiceType:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public SoftwareServiceType(Session session)
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
				if (DataServiceList.IsLoaded)
                {
                    if (DataServiceList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(DataServiceList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(DataServiceList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.DataService>(CriteriaOperator.Parse("[SoftwareServiceType.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool dataservicelist = Session.Query<Module.BusinessObjects.DataService>().Where(x => x.SoftwareServiceType.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(DataServiceList), dataservicelist);
                        if (dataservicelist)
                            return true;

                    }                    
                }				
				if (ServiceInputList.IsLoaded)
                {
                    if (ServiceInputList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ServiceInputList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ServiceInputList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ServiceInput>(CriteriaOperator.Parse("[SoftwareServiceType.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool serviceinputlist = Session.Query<Module.BusinessObjects.ServiceInput>().Where(x => x.SoftwareServiceType.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ServiceInputList), serviceinputlist);
                        if (serviceinputlist)
                            return true;

                    }                    
                }				
				if (ServiceOutputList.IsLoaded)
                {
                    if (ServiceOutputList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ServiceOutputList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ServiceOutputList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ServiceOutput>(CriteriaOperator.Parse("[SoftwareServiceType.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool serviceoutputlist = Session.Query<Module.BusinessObjects.ServiceOutput>().Where(x => x.SoftwareServiceType.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ServiceOutputList), serviceoutputlist);
                        if (serviceoutputlist)
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

	
       
		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(1)]		

 		[Size(20)]
		[RuleUniqueValue("UniqueSoftwareServiceTypeCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredSoftwareServiceTypeCode", DefaultContexts.Save)]
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

	
       
		//private Module.BusinessObjects.DataService _dataservice;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mặc định")]
        [ToolTip("Mặc định")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[SoftwareServiceType.Oid] = ?Oid")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.DataService DataService
        { 
		    get => GetPropertyValue<Module.BusinessObjects.DataService>("DataService");                         
			set => SetPropertyValue<Module.BusinessObjects.DataService>("DataService", value); 
			
        }
		//Tooltip for Object
		public object DataServiceToolTipControllerText(View view)
        {
        //    if (DataService != null) 
		//			return DataService;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.DataService GetDefaultDataService(View view = null)
        { 
			return DataService;
        }
		//Set Default Value
		public void SetDefaultDataService(View view = null)
        {
            //if (DataService is null){
            //    var result = GetDefaultDataService(view);
            //    if (result != null && result != DataService){
			//          DataService = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DataServiceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDataService();
				//if (result != null && DataService != null){
				//	return !DataService.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator DataServiceCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(DataService));
            }
        }
	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(3)]		
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
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dịch vụ")]
		//[Index(4)]
		[DevExpress.Xpo.Association("SoftwareServiceType-DataServiceList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.DataService> DataServiceList
        {      
		    get => GetCollection<Module.BusinessObjects.DataService>("DataServiceList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đầu vào")]
		//[Index(5)]
		[DevExpress.Xpo.Association("SoftwareServiceType-ServiceInputList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ServiceInput> ServiceInputList
        {      
		    get => GetCollection<Module.BusinessObjects.ServiceInput>("ServiceInputList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đầu ra")]
		//[Index(6)]
		[DevExpress.Xpo.Association("SoftwareServiceType-ServiceOutputList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ServiceOutput> ServiceOutputList
        {      
		    get => GetCollection<Module.BusinessObjects.ServiceOutput>("ServiceOutputList"); 
			
        }
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(7)]		
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

	
       
		//private Module.BusinessObjects.Member _updater;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Người cập nhật")]
        [ToolTip("Người cập nhật")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpdaterCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [ModelDefault("AllowEdit", "False")]
		public Module.BusinessObjects.Member Updater
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Updater");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Updater", value); 
			
        }
		//Tooltip for Object
		public object UpdaterToolTipControllerText(View view)
        {
        //    if (Updater != null) 
		//			return Updater;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool UpdaterIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpdater();
				//if (result != null && Updater != null){
				//	return !Updater.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpdaterCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Updater));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 3535ImportCode
            base.AfterConstruction();
SetDefaultMember();
            #endregion 3535ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultDataService(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
			
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
            #region 3526ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 3526ImportCode
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
			//	SetDefaultDataServiceList();
			//	SetDefaultServiceInputList();
			//	SetDefaultServiceOutputList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 3531ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 3531            Oid: 34f422a5-bb42-4592-80b4-ac87287959c1
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3531ImportCode
#region 3536ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 3536            Oid: 3f8442ea-c5ab-4fee-b516-83319262184d
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3536ImportCode
#region 3533ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 3533            Oid: 2fa19702-7edc-4611-9fb0-ab1720e32203
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 3533ImportCode
#region 3524ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 3524            Oid: a9e3c837-792d-482c-b8a3-a7017015c0ad
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 3524ImportCode
#region 3527ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 3527            Oid: 105ba830-bf58-4551-b487-c345e243aadf
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 3527ImportCode
#region 3529ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 3529            Oid: b8ed695e-f4ce-463e-a2db-4846aab42d5a
            Updater = GetDefaultUpdater();
        }
#endregion 3529ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
