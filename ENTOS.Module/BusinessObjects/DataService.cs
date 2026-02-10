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
    [ModelDefault("Caption", "Dịch vụ dữ liệu"), ImageName("DataService")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("DataService VoiceList Hide_None__" , TargetItems = "VoiceList" , Criteria = "[SoftwareServiceType.Code] <> 'TTS'",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Code))]
 
	[MobileColumnAttribute(Context = "DataService_LookupListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class DataService:  DevExpress.Xpo.XPLiteObject , ISourceCode , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public DataService(Session session)
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
				if (VoiceList.IsLoaded)
                {
                    if (VoiceList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(VoiceList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(VoiceList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Voice>(CriteriaOperator.Parse("[DataService.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool voicelist = Session.Query<Module.BusinessObjects.Voice>().Where(x => x.DataService.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(VoiceList), voicelist);
                        if (voicelist)
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
               

		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(0)]		

 		[Size(20)]
		[RuleUniqueValue("UniqueDataServiceCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredDataServiceCode", DefaultContexts.Save)]
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
		[RuleUniqueValue("UniqueDataServiceName", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredDataServiceName", DefaultContexts.Save)]
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

	
       
		//private string _address;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Địa chỉ")]
        [ToolTip("Địa chỉ")]
		//[Index(2)]		

 		[Size(250)]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string Address
        { 
		    get => GetPropertyValue<string>("Address");                         
			set => SetPropertyValue<string>("Address", value); 
			
        }
		//Tooltip for Object
		public object AddressToolTipControllerText(View view)
        {
        //    if (Address != null) 
		//			return Address;
            return null;
        }
		//Get Default Value
        public string GetDefaultAddress(View view = null)
        { 
			return Address;
        }
		//Set Default Value
		public void SetDefaultAddress(View view = null)
        {
            //if (Address is null){
            //    var result = GetDefaultAddress(view);
            //    if (result != null && result != Address){
			//          Address = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AddressIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAddress();
				//if (result != null && Address != null){
				//	return !Address.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _servicecode;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã thực hiện")]
        [ToolTip("Mã thực hiện")]
		//[Index(3)]		

 		[Size(250)]
	    [ModelDefault("PropertyEditorType", "StringComboEditor")]
		public string ServiceCode
        { 
		    get => GetPropertyValue<string>("ServiceCode");                         
			set => SetPropertyValue<string>("ServiceCode", value); 
			
        }
		//Tooltip for Object
		public object ServiceCodeToolTipControllerText(View view)
        {
        //    if (ServiceCode != null) 
		//			return ServiceCode;
            return null;
        }
		//Get Default Value
        public string GetDefaultServiceCode(View view = null)
        { 
			return ServiceCode;
        }
		//Set Default Value
		public void SetDefaultServiceCode(View view = null)
        {
            //if (ServiceCode is null){
            //    var result = GetDefaultServiceCode(view);
            //    if (result != null && result != ServiceCode){
			//          ServiceCode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ServiceCodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultServiceCode();
				//if (result != null && ServiceCode != null){
				//	return !ServiceCode.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DataServiceType _dataservicetype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(4)]		
		public DataServiceType DataServiceType
        { 
		    get => GetPropertyValue<DataServiceType>("DataServiceType");                         
			set => SetPropertyValue<DataServiceType>("DataServiceType", value); 
			
        }
		//Tooltip for Object
		public object DataServiceTypeToolTipControllerText(View view)
        {
        //    if (DataServiceType != null) 
		//			return DataServiceType;
            return null;
        }
		//Get Default Value
        public DataServiceType GetDefaultDataServiceType(View view = null)
        { 
			return DataServiceType;
        }
		//Set Default Value
		public void SetDefaultDataServiceType(View view = null)
        {
            //if (DataServiceType is null){
            //    var result = GetDefaultDataServiceType(view);
            //    if (result != null && result != DataServiceType){
			//          DataServiceType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DataServiceTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDataServiceType();
				//if (result != null && DataServiceType != null){
				//	return !DataServiceType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private ApiMethodType _apimethodtype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại phương thức")]
        [ToolTip("Loại phương thức")]
		//[Index(5)]		
		public ApiMethodType ApiMethodType
        { 
		    get => GetPropertyValue<ApiMethodType>("ApiMethodType");                         
			set => SetPropertyValue<ApiMethodType>("ApiMethodType", value); 
			
        }
		//Tooltip for Object
		public object ApiMethodTypeToolTipControllerText(View view)
        {
        //    if (ApiMethodType != null) 
		//			return ApiMethodType;
            return null;
        }
		//Get Default Value
        public ApiMethodType GetDefaultApiMethodType(View view = null)
        { 
			return ApiMethodType;
        }
		//Set Default Value
		public void SetDefaultApiMethodType(View view = null)
        {
            //if (ApiMethodType is null){
            //    var result = GetDefaultApiMethodType(view);
            //    if (result != null && result != ApiMethodType){
			//          ApiMethodType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ApiMethodTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultApiMethodType();
				//if (result != null && ApiMethodType != null){
				//	return !ApiMethodType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _apikey;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Khóa API")]
        [ToolTip("Khóa API")]
		//[Index(6)]		

 		[Size(250)]
		public string APIKey
        { 
		    get => GetPropertyValue<string>("APIKey");                         
			set => SetPropertyValue<string>("APIKey", value); 
			
        }
		//Tooltip for Object
		public object APIKeyToolTipControllerText(View view)
        {
        //    if (APIKey != null) 
		//			return APIKey;
            return null;
        }
		//Get Default Value
        public string GetDefaultAPIKey(View view = null)
        { 
			return APIKey;
        }
		//Set Default Value
		public void SetDefaultAPIKey(View view = null)
        {
            //if (APIKey is null){
            //    var result = GetDefaultAPIKey(view);
            //    if (result != null && result != APIKey){
			//          APIKey = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool APIKeyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAPIKey();
				//if (result != null && APIKey != null){
				//	return !APIKey.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.DataService _previousdataservice;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dịch vụ trước")]
        [ToolTip("Dịch vụ trước")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PreviousDataServiceCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.DataService PreviousDataService
        { 
		    get => GetPropertyValue<Module.BusinessObjects.DataService>("PreviousDataService");                         
			set => SetPropertyValue<Module.BusinessObjects.DataService>("PreviousDataService", value); 
			
        }
		//Tooltip for Object
		public object PreviousDataServiceToolTipControllerText(View view)
        {
        //    if (PreviousDataService != null) 
		//			return PreviousDataService;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.DataService GetDefaultPreviousDataService(View view = null)
        { 
			return PreviousDataService;
        }
		//Set Default Value
		public void SetDefaultPreviousDataService(View view = null)
        {
            //if (PreviousDataService is null){
            //    var result = GetDefaultPreviousDataService(view);
            //    if (result != null && result != PreviousDataService){
			//          PreviousDataService = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PreviousDataServiceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPreviousDataService();
				//if (result != null && PreviousDataService != null){
				//	return !PreviousDataService.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PreviousDataServiceCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PreviousDataService));
            }
        }
	
       
		//private int? _maxconcurrency;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số luồng đồng thời")]
        [ToolTip("Số luồng đồng thời")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? MaxConcurrency
        { 
		    get => GetPropertyValue<int?>("MaxConcurrency");                         
			set => SetPropertyValue<int?>("MaxConcurrency", value); 
			
        }
		//Tooltip for Object
		public object MaxConcurrencyToolTipControllerText(View view)
        {
        //    if (MaxConcurrency != null) 
		//			return MaxConcurrency;
            return null;
        }
		//Get Default Value
        public int? GetDefaultMaxConcurrency(View view = null)
        { 
			return MaxConcurrency;
        }
		//Set Default Value
		public void SetDefaultMaxConcurrency(View view = null)
        {
            //if (MaxConcurrency is null){
            //    var result = GetDefaultMaxConcurrency(view);
            //    if (result != null && result != MaxConcurrency){
			//          MaxConcurrency = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MaxConcurrencyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMaxConcurrency();
				//if (result != null && MaxConcurrency != null){
				//	return !MaxConcurrency.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.SourceCode _sourcecode;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã nguồn ID")]
        [ToolTip("Mã nguồn ID")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SourceCodeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [ModelDefault("AllowEdit", "False")]
		public Module.BusinessObjects.SourceCode SourceCode
        { 
		    get => GetPropertyValue<Module.BusinessObjects.SourceCode>("SourceCode");                         
			set => SetPropertyValue<Module.BusinessObjects.SourceCode>("SourceCode", value); 
			
        }
		//Tooltip for Object
		public object SourceCodeToolTipControllerText(View view)
        {
        //    if (SourceCode != null) 
		//			return SourceCode;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.SourceCode GetDefaultSourceCode(View view = null)
        { 
			return SourceCode;
        }
		//Set Default Value
		public void SetDefaultSourceCode(View view = null)
        {
            //if (SourceCode is null){
            //    var result = GetDefaultSourceCode(view);
            //    if (result != null && result != SourceCode){
			//          SourceCode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SourceCodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSourceCode();
				//if (result != null && SourceCode != null){
				//	return !SourceCode.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SourceCodeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SourceCode));
            }
        }
	
       
		//private bool _pause;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tạm dừng")]
        [ToolTip("Tạm dừng")]
		//[Index(10)]		
		public bool Pause
        { 
		    get => GetPropertyValue<bool>("Pause");                         
			set => SetPropertyValue<bool>("Pause", value); 
			
        }
		//Tooltip for Object
		public object PauseToolTipControllerText(View view)
        {
        //    if (Pause != null) 
		//			return Pause;
            return null;
        }
		//Get Default Value
        public bool GetDefaultPause(View view = null)
        { 
			return Pause;
        }
		//Set Default Value
		public void SetDefaultPause(View view = null)
        {
            //if (Pause is null){
            //    var result = GetDefaultPause(view);
            //    if (result != null && result != Pause){
			//          Pause = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PauseIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPause();
				//if (result != null && Pause != null){
				//	return !Pause.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _isdefault;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mặc định")]
        [ToolTip("Mặc định")]
		//[Index(11)]		
		public bool IsDefault
        { 
		    get => GetPropertyValue<bool>("IsDefault");                         
			set => SetPropertyValue<bool>("IsDefault", value); 
			
        }
		//Tooltip for Object
		public object IsDefaultToolTipControllerText(View view)
        {
        //    if (IsDefault != null) 
		//			return IsDefault;
            return null;
        }
		//Get Default Value
        public bool GetDefaultIsDefault(View view = null)
        { 
			return IsDefault;
        }
		//Set Default Value
		public void SetDefaultIsDefault(View view = null)
        {
            //if (IsDefault is null){
            //    var result = GetDefaultIsDefault(view);
            //    if (result != null && result != IsDefault){
			//          IsDefault = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool IsDefaultIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultIsDefault();
				//if (result != null && IsDefault != null){
				//	return !IsDefault.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tham số")]
		//[Index(12)]
		[DevExpress.Xpo.Association("DataService-DataServiceParameterList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.DataServiceParameter> DataServiceParameterList
        {      
		    get => GetCollection<Module.BusinessObjects.DataServiceParameter>("DataServiceParameterList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giọng đọc")]
		//[Index(13)]
		[DevExpress.Xpo.Association("DataService-VoiceList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Voice> VoiceList
        {      
		    get => GetCollection<Module.BusinessObjects.Voice>("VoiceList"); 
			
        }
       
		//private Module.BusinessObjects.SoftwareServiceType _softwareservicetype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại dịch vụ")]
        [ToolTip("Loại dịch vụ")]
		//[Index(14)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SoftwareServiceTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("SoftwareServiceType-DataServiceList")]
	 
		public Module.BusinessObjects.SoftwareServiceType SoftwareServiceType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.SoftwareServiceType>("SoftwareServiceType");                         
			set => SetPropertyValue<Module.BusinessObjects.SoftwareServiceType>("SoftwareServiceType", value); 
			
        }
		//Tooltip for Object
		public object SoftwareServiceTypeToolTipControllerText(View view)
        {
        //    if (SoftwareServiceType != null) 
		//			return SoftwareServiceType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.SoftwareServiceType GetDefaultSoftwareServiceType(View view = null)
        { 
			return SoftwareServiceType;
        }
		//Set Default Value
		public void SetDefaultSoftwareServiceType(View view = null)
        {
            //if (SoftwareServiceType is null){
            //    var result = GetDefaultSoftwareServiceType(view);
            //    if (result != null && result != SoftwareServiceType){
			//          SoftwareServiceType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SoftwareServiceTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSoftwareServiceType();
				//if (result != null && SoftwareServiceType != null){
				//	return !SoftwareServiceType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SoftwareServiceTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SoftwareServiceType));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 1502ImportCode
            base.AfterConstruction();
SetDefaultCode();
            #endregion 1502ImportCode
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultAddress(View view = null);
        //SetDefaultServiceCode(View view = null);
        //SetDefaultDataServiceType(View view = null);
        //SetDefaultApiMethodType(View view = null);
        //SetDefaultAPIKey(View view = null);
        //SetDefaultPreviousDataService(View view = null);
        //SetDefaultMaxConcurrency(View view = null);
        //SetDefaultSourceCode(View view = null);
        //SetDefaultPause(View view = null);
        //SetDefaultIsDefault(View view = null);
        //SetDefaultSoftwareServiceType(View view = null);
			
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
            Session.Delete(this.DataServiceParameterList);				
  
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
			//	SetDefaultDataServiceParameterList();
			//	SetDefaultVoiceList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1501ImportCode
		public void SetDefaultCode(View view = null)
        {
            //Code: 1501            Oid: 583b843c-3905-484a-8cc1-6266f728b710
            if(String.IsNullOrEmpty(Code)) Code = GetDefaultCode();

        }
#endregion 1501ImportCode
#region 1500ImportCode
		public string GetDefaultCode(View view = null)
        {
            //Code: 1500            Oid: 0830ac8b-1572-483d-aeb5-ee02c6ee6beb
            var keyCodeObject =
    Module.Helpers.ParameterHelper.GetSettingParameter(Session, "CodeObject");

//Trường hợp chỉ lấy mã trên đối tượng này
Type currentType = this.GetType();
//Trường hợp lấy mã từ đối tượng cha
//Type currentType = typeof(ObjectType);

//Kích thước mặc định là 3 số
int size = 3;
return Tools.GetCode(currentType , this.Session, this.Oid, keyCodeObject != null ? keyCodeObject.Value : "", size,
    " ");
return null;
        }
#endregion 1500ImportCode
        #endregion
//Mã nguồn bổ sung
#region DataServiceImportCode
/*         [Browsable(false)]
        public System.Collections.Generic.IList<string> ServiceCodeDataSource
        {
            get
            {
                var dbSource = Module.Helpers.InterfaceDiscoveryHelper.GetImplementations<ENTOS.Domain.Interfaces.IDataServiceHandle>();
                return dbSource.Select(x => x.GetType().Name).ToList();
                //return null;
            }
        } */
#endregion DataServiceImportCode
		 		 
    }
}
