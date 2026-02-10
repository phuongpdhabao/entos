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
    [ModelDefault("Caption", "Trích Web"), ImageName("WebExtractor")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("WebExtractor Start, End Hide_None__" , TargetItems = "Start, End" , Criteria = "[Repeat] = False",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(SystemType), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(ConnectTimeOut)+ "," + nameof(Update))]
 
	[MobileColumnAttribute(Context = "WebExtractor_ListView", TargetItems = nameof(Name)+ "," + nameof(URL))]
	[MobileColumnAttribute(Context = "WebExtractor_LookupListView", TargetItems = nameof(Update)+ "," + nameof(URL))]
	[DefaultProperty("URL")]
 
[OptimisticLocking(true)]
    public partial class WebExtractor:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public WebExtractor(Session session)
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
				if (ExtractorItemList.IsLoaded)
                {
                    if (ExtractorItemList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ExtractorItemList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ExtractorItemList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ExtractorItem>(CriteriaOperator.Parse("[WebExtractor.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool extractoritemlist = Session.Query<Module.BusinessObjects.ExtractorItem>().Where(x => x.WebExtractor.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ExtractorItemList), extractoritemlist);
                        if (extractoritemlist)
                            return true;

                    }                    
                }				
				if (ExtractorDataConfigurationList.IsLoaded)
                {
                    if (ExtractorDataConfigurationList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ExtractorDataConfigurationList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ExtractorDataConfigurationList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ExtractorDataConfiguration>(CriteriaOperator.Parse("[WebExtractor.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool extractordataconfigurationlist = Session.Query<Module.BusinessObjects.ExtractorDataConfiguration>().Where(x => x.WebExtractor.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ExtractorDataConfigurationList), extractordataconfigurationlist);
                        if (extractordataconfigurationlist)
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
               

		//private string _url;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Trang web")]
        [ToolTip("Trang web")]
		//[Index(0)]		

 		[Size(100)]
		[RuleRequiredField("RequiredWebExtractorURL", DefaultContexts.Save)]
		public string URL
        { 
		    get => GetPropertyValue<string>("URL");                         
			set => SetPropertyValue<string>("URL", value); 
			
        }
		//Tooltip for Object
		public object URLToolTipControllerText(View view)
        {
        //    if (URL != null) 
		//			return URL;
            return null;
        }
		//Get Default Value
        public string GetDefaultURL(View view = null)
        { 
			return URL;
        }
		//Set Default Value
		public void SetDefaultURL(View view = null)
        {
            //if (URL is null){
            //    var result = GetDefaultURL(view);
            //    if (result != null && result != URL){
			//          URL = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool URLIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultURL();
				//if (result != null && URL != null){
				//	return !URL.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(1)]		

 		[Size(100)]
		[RuleRequiredField("RequiredWebExtractorName", DefaultContexts.Save)]
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

	
       
		//private System.Type _systemtype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Kiểu hệ thống")]
        [ToolTip("Kiểu hệ thống")]
		//[Index(2)]		
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
	
       
		//private int _connecttimeout;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thời gian đợi")]
        [ToolTip("Thời gian đợi")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int ConnectTimeOut
        { 
		    get => GetPropertyValue<int>("ConnectTimeOut");                         
			set => SetPropertyValue<int>("ConnectTimeOut", value); 
			
        }
		//Tooltip for Object
		public object ConnectTimeOutToolTipControllerText(View view)
        {
        //    if (ConnectTimeOut != null) 
		//			return ConnectTimeOut;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool ConnectTimeOutIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultConnectTimeOut();
				//if (result != null && ConnectTimeOut != null){
				//	return !ConnectTimeOut.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _automatic;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tự động")]
        [ToolTip("Tự động")]
		//[Index(4)]		
		public bool Automatic
        { 
		    get => GetPropertyValue<bool>("Automatic");                         
			set => SetPropertyValue<bool>("Automatic", value); 
			
        }
		//Tooltip for Object
		public object AutomaticToolTipControllerText(View view)
        {
        //    if (Automatic != null) 
		//			return Automatic;
            return null;
        }
		//Get Default Value
        public bool GetDefaultAutomatic(View view = null)
        { 
			return Automatic;
        }
		//Set Default Value
		public void SetDefaultAutomatic(View view = null)
        {
            //if (Automatic is null){
            //    var result = GetDefaultAutomatic(view);
            //    if (result != null && result != Automatic){
			//          Automatic = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AutomaticIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAutomatic();
				//if (result != null && Automatic != null){
				//	return !Automatic.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _repeat;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lặp lại")]
        [ToolTip("Lặp lại")]
		//[Index(5)]		
		public bool Repeat
        { 
		    get => GetPropertyValue<bool>("Repeat");                         
			set => SetPropertyValue<bool>("Repeat", value); 
			
        }
		//Tooltip for Object
		public object RepeatToolTipControllerText(View view)
        {
        //    if (Repeat != null) 
		//			return Repeat;
            return null;
        }
		//Get Default Value
        public bool GetDefaultRepeat(View view = null)
        { 
			return Repeat;
        }
		//Set Default Value
		public void SetDefaultRepeat(View view = null)
        {
            //if (Repeat is null){
            //    var result = GetDefaultRepeat(view);
            //    if (result != null && result != Repeat){
			//          Repeat = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RepeatIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRepeat();
				//if (result != null && Repeat != null){
				//	return !Repeat.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chi tiết")]
		//[Index(6)]
		[DevExpress.Xpo.Association("WebExtractor-ExtractorItemList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ExtractorItem> ExtractorItemList
        {      
		    get => GetCollection<Module.BusinessObjects.ExtractorItem>("ExtractorItemList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấu hình")]
		//[Index(7)]
		[DevExpress.Xpo.Association("WebExtractor-ExtractorDataConfigurationList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ExtractorDataConfiguration> ExtractorDataConfigurationList
        {      
		    get => GetCollection<Module.BusinessObjects.ExtractorDataConfiguration>("ExtractorDataConfigurationList"); 
			
        }
       
		//private string _addresses;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Danh sách")]
        [ToolTip("Danh sách")]
		//[Index(8)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Addresses
        { 
		    get => GetPropertyValue<string>("Addresses");                         
			set => SetPropertyValue<string>("Addresses", value); 
			
        }
		//Tooltip for Object
		public object AddressesToolTipControllerText(View view)
        {
        //    if (Addresses != null) 
		//			return Addresses;
            return null;
        }
		//Get Default Value
        public string GetDefaultAddresses(View view = null)
        { 
			return Addresses;
        }
		//Set Default Value
		public void SetDefaultAddresses(View view = null)
        {
            //if (Addresses is null){
            //    var result = GetDefaultAddresses(view);
            //    if (result != null && result != Addresses){
			//          Addresses = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AddressesIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAddresses();
				//if (result != null && Addresses != null){
				//	return !Addresses.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(9)]		
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

	
       
		//private int _start;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bắt đầu")]
        [ToolTip("Bắt đầu")]
		//[Index(10)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int Start
        { 
		    get => GetPropertyValue<int>("Start");                         
			set => SetPropertyValue<int>("Start", value); 
			
        }
		//Tooltip for Object
		public object StartToolTipControllerText(View view)
        {
        //    if (Start != null) 
		//			return Start;
            return null;
        }
		//Get Default Value
        public int GetDefaultStart(View view = null)
        { 
			return Start;
        }
		//Set Default Value
		public void SetDefaultStart(View view = null)
        {
            //if (Start is null){
            //    var result = GetDefaultStart(view);
            //    if (result != null && result != Start){
			//          Start = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StartIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStart();
				//if (result != null && Start != null){
				//	return !Start.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int _end;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kết thúc")]
        [ToolTip("Kết thúc")]
		//[Index(11)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int End
        { 
		    get => GetPropertyValue<int>("End");                         
			set => SetPropertyValue<int>("End", value); 
			
        }
		//Tooltip for Object
		public object EndToolTipControllerText(View view)
        {
        //    if (End != null) 
		//			return End;
            return null;
        }
		//Get Default Value
        public int GetDefaultEnd(View view = null)
        { 
			return End;
        }
		//Set Default Value
		public void SetDefaultEnd(View view = null)
        {
            //if (End is null){
            //    var result = GetDefaultEnd(view);
            //    if (result != null && result != End){
			//          End = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EndIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEnd();
				//if (result != null && End != null){
				//	return !End.Equals(result);
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
            Oid = Guid.NewGuid();
 
            #region 0392ImportCode
            base.AfterConstruction();
SetDefaultConnectTimeOut();
SetDefaultUpdate();
            #endregion 0392ImportCode
            Display = true;
 
        //SetDefaultURL(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultSystemType(View view = null);
        //SetDefaultConnectTimeOut(View view = null);
        //SetDefaultAutomatic(View view = null);
        //SetDefaultRepeat(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultStart(View view = null);
        //SetDefaultEnd(View view = null);
			
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
            #region 0516ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0516ImportCode
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
			//	SetDefaultExtractorItemList();
			//	SetDefaultExtractorDataConfigurationList();
			//	SetDefaultAddresses();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0033ImportCode
		public int GetDefaultConnectTimeOut(View view = null)
        {
            //Code: 0033            Oid: 80ac292f-b470-4061-a4f3-e7b351b40aa5
            return 20;
        }
#endregion 0033ImportCode
#region 0038ImportCode
		public void SetDefaultConnectTimeOut(View view = null)
        {
            //Code: 0038            Oid: 17237acf-6f7b-4213-834a-1ddd11fdab95
            if(ConnectTimeOut == 0) ConnectTimeOut = GetDefaultConnectTimeOut();
        }
#endregion 0038ImportCode
#region 0039ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0039            Oid: 846312c2-b879-482d-8862-c72d74941769
            Update = GetDefaultUpdate();
        }
#endregion 0039ImportCode
#region 0079ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0079            Oid: aadb2ebe-4582-47bf-9634-5a8375af5b57
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0079ImportCode
        #endregion
//Mã nguồn bổ sung
#region WebExtractorImportCode
#region WebExtractorImportCode


        public System.Collections.Generic.IList<Module.BusinessObjects.ExtractorItem> ExtractorItemListWithSort()
        {
            return ExtractorItemList.Where(x => !x.InActive).OrderBy(m => m.Order).ToList();
        }
        //Các phương thức lấy dữ liệu
        private string _chromeDriverPath;
        [Browsable(false)]
        public string ChromeDriverPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_chromeDriverPath))
                {
                    _chromeDriverPath = Module.Helpers.ParameterHelper.GetValue(Session, "ChromeDriverPath");
                }
                if (string.IsNullOrEmpty(_chromeDriverPath))
                {
                    _chromeDriverPath = @"\\rd\CodeGen\packages\chromedriver_win32";
                }
                return _chromeDriverPath;
            }
        }

        private System.Collections.Generic.IList<DataTables> _dataTablesResult;
        [Browsable(false)]
        public System.Collections.Generic.IList<DataTables> DataTablesResult
        {
            get
            {
                if (_dataTablesResult is null)
                {
                    _dataTablesResult = new System.Collections.Generic.List<DataTables>();
                }
                return _dataTablesResult;
            }
        }

        OpenQA.Selenium.Chrome.ChromeDriver driverOpening = null;
        public OpenQA.Selenium.Chrome.ChromeDriver OpenChrome(XafApplication application)
        {
            var options = new OpenQA.Selenium.Chrome.ChromeOptions();
            options.AddArgument("no-sandbox");
            // Chạy ngầm không pop up trình duyệt ra ngoài 
            //options.AddArgument("headless");
            var chromeDriverService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService();
            driverOpening = new OpenQA.Selenium.Chrome.ChromeDriver(chromeDriverService, options);
            driverOpening.Url = this.URL;
            var extractorItemListWithSort = ExtractorItemListWithSort();
            for (int i = 0, startGetItem = 0; i < extractorItemListWithSort.Count && startGetItem < 20; i++)
            {
                try
                {
                    if (extractorItemListWithSort[i].OneTime && i > 0)
                        continue;
                    if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Wait)
                    {
                        int wait = 10;
                        if (!string.IsNullOrEmpty(extractorItemListWithSort[i].CssXpathValue) && !Int32.TryParse(extractorItemListWithSort[i].CssXpathValue, out wait))
                            continue;
                        //Tools.ShowMessage(Application, "Thông báo", "Bắt đầu đợi " + wait + " giây");
                        if (!string.IsNullOrEmpty(extractorItemListWithSort[i].Name))
                        {
                            driverOpening.ExecuteScript("alert('" + extractorItemListWithSort[i].Name + "');");
                        }

                        System.Threading.Thread.Sleep(wait * 1000);
                        continue;
                    }
                    else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.RunJavascript)
                    {
                        if (!string.IsNullOrEmpty(extractorItemListWithSort[i].CssXpathValue))
                        {
                            driverOpening.ExecuteScript(extractorItemListWithSort[i].CssXpathValue);
                        }
                        continue;
                    }
                    else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Replace)
                    {
                        //driver.Re
                        //doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace(extractorItem.Name, extractorItem.CssXpathValue);
                        if (!string.IsNullOrEmpty(extractorItemListWithSort[i].Name) && !string.IsNullOrEmpty(extractorItemListWithSort[i].CssXpathValue))
                        {
                            var element = driverOpening.FindElement(OpenQA.Selenium.By.TagName("body"));
                            if (element != null)
                            {
                                var bodyHtml = element.GetAttribute("innerHTML");
                                if (!string.IsNullOrEmpty(bodyHtml))
                                {
                                    driverOpening.ExecuteScript("arguments[0].innerHTML = '" + bodyHtml + "'", element);
                                }
                            }
                        }
                        continue;
                    }

                    System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement> elementResults = null;
                    OpenQA.Selenium.IWebElement elementResult = null;
                    int waitSecond = 0;
                    while (elementResult is null && waitSecond < ConnectTimeOut)
                    {
                        if (string.IsNullOrEmpty(extractorItemListWithSort[i].CssXpathValue))
                            break;
                        if (extractorItemListWithSort[i].CssXpathValue.Contains("/"))
                            elementResults = driverOpening.FindElements(OpenQA.Selenium.By.XPath(extractorItemListWithSort[i].CssXpathValue));
                        else if (extractorItemListWithSort[i].CssXpathValue.StartsWith("="))
                            elementResults = driverOpening.FindElements(OpenQA.Selenium.By.LinkText(extractorItemListWithSort[i].CssXpathValue.Substring(1)));
                        else
                            elementResults = driverOpening.FindElements(OpenQA.Selenium.By.CssSelector(extractorItemListWithSort[i].CssXpathValue));
                        elementResult = elementResults.FirstOrDefault();
                        //Nếu là lấy dữ liệu thì không phải đợi
                        if (extractorItemListWithSort[i].IsGet())
                            break;
                        if (startGetItem > 0)
                            break;
                        waitSecond++;
                        System.Threading.Thread.Sleep(waitSecond * 1000);
                    }
                    if (elementResult is null)
                    {
                        //Debug: Kiểm tra xem có tìm thấy đổi tượng ko?
                    }

                    if (elementResult != null)
                    {
                        if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Delete)
                        {
                            //var elements = doc.DocumentNode.SelectNodes(extractorItem.CssXpathValue);
                            //for (int i = elements.Count - 1; i >= 0; i--)
                            //    elements[i].Remove();
                            driverOpening.ExecuteScript("arguments[0].remove();", elementResult);
                            continue;
                        }
                        else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Table)
                        {
                        }
                        else if (extractorItemListWithSort[i].InsideTable)
                        {
                            if (extractorItemListWithSort[i].Row != null &&
                                extractorItemListWithSort[i].Column != null)
                                elementResult = GetElementInsideTable(elementResult,
                                    extractorItemListWithSort[i]);
                            else
                                continue;
                        }
                    }
                    else
                    {

                        //Không tìm thấy dữ liệu
                    }
                    //if (extractorItemListWithSort[i].IsGet())
                    //{

                    //}
                    //else 
                    if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Input)
                    {
                        //Gửi dữ liệu
                        if (elementResult != null)
                            elementResult.SendKeys(extractorItemListWithSort[i].Name);

                    }
                    else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Password)
                    {
                        if (elementResult != null)
                            elementResult.SendKeys(extractorItemListWithSort[i].Password);
                    }
                    else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Button)
                    {
                        if (elementResult != null)
                            //Gửi dữ liệu click
                            elementResult.Click();

                    }

                }
                catch (Exception ex)
                {

                }

            }
            return driverOpening;
        }
        public bool DownloadsExecute(XafApplication application, string folderPath, string choiceId)
        {
            bool success = false;
            System.Net.Http.HttpClientHandler handler = null;
            if (choiceId.Contains("Login"))
            {
                var driver = OpenChrome(application);
                var cookies = driverOpening.Manage().Cookies.AllCookies;
                var cookieContainer = new System.Net.CookieContainer();
                foreach (var cookie in cookies)
                {
                    cookieContainer.Add(new Uri(driverOpening.Url), new System.Net.Cookie(cookie.Name, cookie.Value));
                }
                handler = new System.Net.Http.HttpClientHandler { CookieContainer = cookieContainer };
            }
            var links = Addresses.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (links.Length > 0)
            {
                var countString = Module.SystemObjects.Tools.GetNumberMaxLength(links.Length);
                using (var client = handler != null ? new System.Net.Http.HttpClient(handler) : new System.Net.Http.HttpClient())
                {
                    System.Random random = new System.Random();                                            // Gửi yêu cầu GET để tải ảnh
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Referer", URL); // Đảm bảo Referer hợp lệ
                    if (choiceId.Contains("Image"))
                        client.DefaultRequestHeaders.Add("Accept", "image/webp,image/apng,image/*,*/*;q=0.8");
                    else if (choiceId.Contains("Pdf"))
                        client.DefaultRequestHeaders.Add("Accept", "application/pdf");
                    else
                        client.DefaultRequestHeaders.Add("Accept", "*/*");
                    //Không dùng Async để tăng tốc độ, vì có thể sẽ lỗi nếu tải nhiều ảnh cùng lúc
                    for (int i = 0; i < links.Length; i++)
                    {
                        if (handler != null)
                            System.Threading.Thread.Sleep(random.Next(300, 2000)); //Đợi để tránh server nghi ngờ bot
                        var url = links[i];
                        var fileName = System.IO.Path.GetFileName(url);
                        var fileNamePosition = Module.SystemObjects.Tools.GetNumberCode(i + 1, countString);
                        var newPath = System.IO.Path.Combine(folderPath, fileNamePosition + "-" + fileName);

                        {

                            //client.BaseAddress = new System.Uri(uri.Host);
                            var response = client.GetAsync(url).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                // Đọc dữ liệu ảnh và lưu vào file
                                var imageBytes = response.Content.ReadAsByteArrayAsync().Result;
                                System.IO.File.WriteAllBytes(newPath, imageBytes);
                            }
                            else
                            {
                                //Console.WriteLine($"Không thể tải ảnh. Mã lỗi: {response.StatusCode}");
                            }
                        }
                        if (!success && System.IO.File.Exists(newPath))
                            success = true;
                    }
                }
            }
            return success;
        }




        public void QuickResult_Execute(XafApplication application, System.Collections.Generic.IList<string> tableHeaderRow)
        {
            //Xóa kết quả cũ
            if (DataTablesResult.Count > 0)
                DataTablesResult.Clear();
            var extractorItemListWithSort = ExtractorItemListWithSort();
            var web = new HtmlAgilityPack.HtmlWeb();
            web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            var links = Addresses.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var url in links)
            {
                bool addRecord = false;
                var dataTableRecord = new DataTables();
                dataTableRecord.CurrentAddress = url;
                string cacheUrl = url;
                if (url.StartsWith("http") || url.StartsWith("www"))
                {
                    var cacheFile = Module.Helpers.NameHelper.GetCacheFileName(Session, url);
                    if (!string.IsNullOrEmpty(cacheFile))
                        cacheUrl = cacheFile;
                }
                HtmlAgilityPack.HtmlDocument doc = web.Load(cacheUrl);
                if (doc.DocumentNode is null || doc.DocumentNode?.OuterLength == 0)
                {
                    //Đợi 1s
                    System.Threading.Thread.Sleep(1000);
                }
                //Chỉ hiển thị giá trị trường Get
                for (int i = 0, startGetItem = 0; i < extractorItemListWithSort.Count && startGetItem < 20; i++)
                {
                    try
                    {
                        if (extractorItemListWithSort[i].OneTime && i > 0)
                            continue;
                        if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Wait)
                        {
                            int wait = 10;
                            if (!string.IsNullOrEmpty(extractorItemListWithSort[i].CssXpathValue) && !Int32.TryParse(extractorItemListWithSort[i].CssXpathValue, out wait))
                            {

                                //Tools.ShowMessage(Application, "Lỗi", "Thứ tự: " + extractorItemListWithSort[i].Order +". Giá trị trường đợi không phải là số");
                                return;
                            }
                            //Tools.ShowMessage(Application, "Thông báo", "Bắt đầu đợi " + wait + " giây");
                            if (!string.IsNullOrEmpty(extractorItemListWithSort[i].Name))
                            {
                                //Không hỗ trợ
                            }

                            System.Threading.Thread.Sleep(wait * 1000);
                            continue;
                        }
                        else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.RunJavascript)
                        {
                            if (!string.IsNullOrEmpty(extractorItemListWithSort[i].CssXpathValue))
                            {
                                //Không hỗ trợ
                                //driver.ExecuteScript(extractorItemListWithSort[i].CssXpathValue);
                            }
                            continue;
                        }
                        else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Input)
                        {
                            continue;
                        }
                        else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Replace)
                        {
                            doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace(extractorItemListWithSort[i].Name, extractorItemListWithSort[i].CssXpathValue);
                            continue;
                        }
                        if (extractorItemListWithSort[i].MultiRow)
                        {
                            if (extractorItemListWithSort[i].IsGet())
                            {
                                var elements = doc.DocumentNode.SelectNodes(extractorItemListWithSort[i].CssXpathValue);
                                //Lấy dữ liệu về
                                foreach (var element in elements)
                                {
                                    var elementResultText =
                                        GetElementValue(element, extractorItemListWithSort[i]);
                                    if (!string.IsNullOrEmpty(elementResultText))
                                    {
                                        var rowDataTableRecord = new DataTables();
                                        rowDataTableRecord.SetMemberValue("Col" + startGetItem, GetTextByBehavior(elementResultText, extractorItemListWithSort[i]));
                                        rowDataTableRecord.CurrentAddress = url;
                                        DataTablesResult.Add(rowDataTableRecord);
                                        //addRecord = true;
                                    }

                                }

                            }
                        }
                        else
                        {
                            var element = doc.DocumentNode.SelectSingleNode(extractorItemListWithSort[i].CssXpathValue);
                            if (element != null)
                            {
                                if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Delete)
                                {
                                    var elements = doc.DocumentNode.SelectNodes(extractorItemListWithSort[i].CssXpathValue);
                                    for (int j = elements.Count - 1; j >= 0; j--)
                                        elements[j].Remove();
                                    //element.Remove();
                                    continue;
                                }
                                else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Table)
                                {
                                    var tableResult = element.Name.Equals("table")
                                        ? element
                                        : element.SelectSingleNode("//table");
                                    if (tableResult != null)
                                    {
                                        var tableRows = tableResult.SelectNodes("//tr");
                                        if (tableRows.Count() > 1)
                                        {
                                            //Nạp tiêu đề cho cột
                                            var currentColumnIndex = startGetItem;
                                            var headersRow = tableRows[0].ChildNodes;
                                            if (headersRow.Count > 0)
                                            {
                                                foreach (var headerElement in headersRow)
                                                {
                                                    tableHeaderRow.Add(headerElement.InnerText);
                                                    if (!string.IsNullOrEmpty(headerElement.InnerText))
                                                    {
                                                        dataTableRecord.IsHeader = true;
                                                        if (!string.IsNullOrEmpty(headerElement.InnerText))
                                                            dataTableRecord.SetMemberValue("Col" + startGetItem, headerElement.InnerText);
                                                        startGetItem++;
                                                    }
                                                }
                                                addRecord = true;
                                            }

                                            for (int r = 1; r < tableRows.Count; r++)
                                            {
                                                //Nạp nội dung cho table
                                                if (string.IsNullOrEmpty(tableRows[r].InnerText))
                                                    continue;
                                                var tdElements = tableRows[r].ChildNodes;
                                                if (tdElements.Count > 0)
                                                {
                                                    var rowRecord = new DataTables();
                                                    rowRecord.CurrentAddress = url;
                                                    var newColumnIndex = currentColumnIndex;
                                                    foreach (var tdElement in tdElements)
                                                    {
                                                        if (!string.IsNullOrEmpty(tdElement.InnerText))
                                                        {
                                                            //Cột trong lập trình bắt đầu từ 0,
                                                            //Cột trong định nghĩa bắt đầu từ 1
                                                            if (!string.IsNullOrEmpty(tdElement.InnerText))
                                                                rowRecord.SetMemberValue("Col" + newColumnIndex, GetTextByBehavior(tdElement.InnerText, extractorItemListWithSort[i]));
                                                        }
                                                        var colspan = tdElement.GetAttributeValue("colspan", null);
                                                        if (!string.IsNullOrEmpty(colspan))
                                                            //Nếu Merger cột thì phải cộng thêm số cột merger
                                                            newColumnIndex += Int32.Parse(colspan);
                                                        else
                                                            newColumnIndex++;

                                                    }

                                                    DataTablesResult.Add(rowRecord);
                                                }


                                            }

                                        }

                                        //if (i + 1 != extractorItemListWithSort.Count)
                                        //{
                                        //    Tools.ShowMessage(Application,"Thông báo", "Không thể lấy dữ liệu sau khi lấy dữ liệu bảng");
                                        //    break;
                                        //}

                                    }

                                }
                                else if (extractorItemListWithSort[i].InsideTable)
                                {
                                    //if (extractorItemListWithSort[i].Row != null &&
                                    //    extractorItemListWithSort[i].Column != null)
                                    //    elementResult = GetElementInsideTable(elementResult,
                                    //        extractorItemListWithSort[i]);
                                    //else
                                    continue;
                                }
                            }
                            else
                            {

                                //Không tìm thấy dữ liệu
                            }
                            //var fd = driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div[2]/table/tbody/tr[9]/td/table"));
                            if (extractorItemListWithSort[i].IsGet())
                            {
                                //Lấy dữ liệu về
                                if (element != null)
                                {
                                    var elementResultText =
                                        GetElementValue(element, extractorItemListWithSort[i]);
                                    if (!string.IsNullOrEmpty(elementResultText))
                                    {
                                        dataTableRecord.SetMemberValue("Col" + startGetItem, GetTextByBehavior(elementResultText, extractorItemListWithSort[i]));
                                        addRecord = true;
                                    }

                                }
                                startGetItem++;
                            }
                            else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Input)
                            {
                                //Gửi dữ liệu
                                //elementResult.SendKeys(extractorItemListWithSort[i].Name);
                            }
                            else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Password)
                            {
                                //elementResult.SendKeys(extractorItemListWithSort[i].Password);
                            }
                            else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Button)
                            {
                                //Gửi dữ liệu click
                                //elementResult.Click();

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        if (application != null && extractorItemListWithSort[i].ExtractorType != ExtractorType.Delete)
                            Tools.ShowMessage(application, "Thông báo", "Thứ tự: " + extractorItemListWithSort[i].Order + ". Không tìm thấy " + extractorItemListWithSort[i].Name + ". Bạn có thể để thêm thời gian đợi trước quá trình tìm đối tượng", InformationType.Warning, 10);
                    }
                    //var test = driver.FindElementByXPath(extractorItemListWithSort[i].CssXpathValue);
                }
                if (addRecord)
                    DataTablesResult.Add(dataTableRecord);
                else
                {
                    //Lỗi
                }
                //FillHtmlContentAsync(url, false);

            }
            //Đóng kết nối


            //driver.Close();

        }

        public string GetElementValue(HtmlAgilityPack.HtmlNode webElement, ExtractorItem extractorItem)
        {
            if (webElement != null)
            {
                if (extractorItem.ExtractorType == ExtractorType.Text)
                {
                    return webElement.InnerText;
                }
                else if (extractorItem.ExtractorType == ExtractorType.Image)
                {
                    //Lấy ảnh
                    if (webElement.Name == "img")
                        return webElement.GetAttributeValue("src", null);
                    else
                        return GetElementValue(webElement.SelectSingleNode("//img"), extractorItem);

                }
                else if (extractorItem.ExtractorType == ExtractorType.Link)
                {
                    if (webElement.Name == "a")
                        return webElement.GetAttributeValue("href", null);
                    else
                        return GetElementValue(webElement.SelectSingleNode("//a"), extractorItem);
                }
                else if (extractorItem.ExtractorType == ExtractorType.ImageInLink)
                {
                    if (webElement.Name == "a")
                    {
                        var href = webElement.GetAttributeValue("href", null);
                        if (!string.IsNullOrEmpty(href))
                        {
                            var childHtmlWeb = new HtmlAgilityPack.HtmlWeb();
                            var childDoc = childHtmlWeb.Load(href);
                            var imgElement = childDoc.DocumentNode.SelectSingleNode("//img");

                            if (imgElement != null)
                                return imgElement.GetAttributeValue("src", null);

                            return null;
                        }
                    }
                    else
                    {
                        return GetElementValue(webElement.SelectSingleNode("//a"), extractorItem);
                    }
                }
                else if (extractorItem.ExtractorType == ExtractorType.Html)
                {
                    return webElement.InnerHtml;
                }
            }

            return null;
        }

        public void GetResult_Execute(XafApplication application, System.Collections.Generic.IList<string> tableHeaderRow)
        {
            //Xóa kết quả cũ
            if (DataTablesResult.Count > 0)
                DataTablesResult.Clear();
            var extractorItemListWithSort = ExtractorItemListWithSort();

            //Nạp dữ liệu
            var options = new OpenQA.Selenium.Chrome.ChromeOptions();
            options.AddArgument("no-sandbox");
            // Chạy ngầm không pop up trình duyệt ra ngoài 
            //options.AddArgument("headless");
            //var driver = new OpenQA.Selenium.Chrome.ChromeDriver(ChromeDriverPath, options, System.TimeSpan.FromSeconds(ConnectTimeOut));
            var chromeDriverService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService();
            var driver = new OpenQA.Selenium.Chrome.ChromeDriver(chromeDriverService, options, System.TimeSpan.FromSeconds(ConnectTimeOut));
            //driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(currentObject.ConnectTimeOut);
            //Tạo kết quả
            var links = Addresses.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var url in links)
            {
                string cacheUrl = url;
                if (url.StartsWith("http") || url.StartsWith("www"))
                {
                    var cacheFile = Module.Helpers.NameHelper.GetCacheFileName(Session, url);
                    if (!string.IsNullOrEmpty(cacheFile))
                        cacheUrl = cacheFile;
                }
                bool addRecord = false;
                var dataTableRecord = new DataTables();
                dataTableRecord.CurrentAddress = url;
                driver.Url = cacheUrl;
                //Chỉ hiển thị giá trị trường Get                
                for (int i = 0, startGetItem = 0; i < extractorItemListWithSort.Count && startGetItem < 20; i++)
                {
                    try
                    {
                        if (extractorItemListWithSort[i].OneTime && i > 0)
                            continue;
                        if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Wait)
                        {
                            int wait = 10;
                            if (!string.IsNullOrEmpty(extractorItemListWithSort[i].CssXpathValue) && !Int32.TryParse(extractorItemListWithSort[i].CssXpathValue, out wait))
                            {

                                //Tools.ShowMessage(Application, "Lỗi", "Thứ tự: " + extractorItemListWithSort[i].Order +". Giá trị trường đợi không phải là số");
                                return;
                            }
                            //Tools.ShowMessage(Application, "Thông báo", "Bắt đầu đợi " + wait + " giây");
                            if (!string.IsNullOrEmpty(extractorItemListWithSort[i].Name))
                            {
                                driver.ExecuteScript("alert('" + extractorItemListWithSort[i].Name + "');");
                            }

                            System.Threading.Thread.Sleep(wait * 1000);
                            continue;
                        }
                        else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.RunJavascript)
                        {
                            if (!string.IsNullOrEmpty(extractorItemListWithSort[i].CssXpathValue))
                            {
                                driver.ExecuteScript(extractorItemListWithSort[i].CssXpathValue);
                            }
                            continue;
                        }
                        else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Replace)
                        {
                            //driver.Re
                            //doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace(extractorItem.Name, extractorItem.CssXpathValue);
                            if (!string.IsNullOrEmpty(extractorItemListWithSort[i].Name) && !string.IsNullOrEmpty(extractorItemListWithSort[i].CssXpathValue))
                            {
                                var element = driver.FindElement(OpenQA.Selenium.By.TagName("body"));
                                if (element != null)
                                {
                                    var bodyHtml = element.GetAttribute("innerHTML");
                                    if (!string.IsNullOrEmpty(bodyHtml))
                                    {
                                        driver.ExecuteScript("arguments[0].innerHTML = '" + bodyHtml + "'", element);
                                    }
                                }
                            }
                            continue;
                        }

                        System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement> elementResults = null;
                        OpenQA.Selenium.IWebElement elementResult = null;
                        int waitSecond = 0;
                        while (elementResult is null && waitSecond < ConnectTimeOut)
                        {
                            if (string.IsNullOrEmpty(extractorItemListWithSort[i].CssXpathValue))
                                break;
                            if (extractorItemListWithSort[i].CssXpathValue.Contains("/"))
                                elementResults = driver.FindElements(OpenQA.Selenium.By.XPath(extractorItemListWithSort[i].CssXpathValue));
                            else if (extractorItemListWithSort[i].CssXpathValue.StartsWith("="))
                                elementResults = driver.FindElements(OpenQA.Selenium.By.LinkText(extractorItemListWithSort[i].CssXpathValue.Substring(1)));
                            else
                                elementResults = driver.FindElements(OpenQA.Selenium.By.CssSelector(extractorItemListWithSort[i].CssXpathValue));
                            elementResult = elementResults.FirstOrDefault();
                            //Nếu là lấy dữ liệu thì không phải đợi
                            if (extractorItemListWithSort[i].IsGet())
                                break;
                            if (startGetItem > 0)
                                break;
                            waitSecond++;
                            System.Threading.Thread.Sleep(waitSecond * 1000);
                        }
                        if (elementResult is null)
                        {
                            //Debug: Kiểm tra xem có tìm thấy đổi tượng ko?
                        }

                        if (elementResult != null)
                        {
                            if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Delete)
                            {
                                //var elements = doc.DocumentNode.SelectNodes(extractorItem.CssXpathValue);
                                //for (int i = elements.Count - 1; i >= 0; i--)
                                //    elements[i].Remove();
                                driver.ExecuteScript("arguments[0].remove();", elementResult);
                                continue;
                            }
                            else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Table)
                            {
                                var tableResult = elementResult.TagName.Equals("table")
                                    ? elementResult
                                    : elementResult.FindElement(OpenQA.Selenium.By.TagName("table"));
                                if (tableResult != null)
                                {
                                    var tableRows = tableResult.FindElements(OpenQA.Selenium.By.TagName("tr"));
                                    if (tableRows.Count > 1)
                                    {
                                        //Nạp tiêu đề cho cột
                                        var currentColumnIndex = startGetItem;
                                        var headersRow = tableRows[0].FindElements(OpenQA.Selenium.By.XPath("./*"));
                                        if (headersRow.Count > 0)
                                        {
                                            foreach (var headerElement in headersRow)
                                            {
                                                tableHeaderRow.Add(headerElement.Text);
                                                if (!string.IsNullOrEmpty(headerElement.Text))
                                                {
                                                    dataTableRecord.IsHeader = true;
                                                    if (!string.IsNullOrEmpty(headerElement.Text))
                                                        dataTableRecord.SetMemberValue("Col" + startGetItem, headerElement.Text);
                                                    startGetItem++;
                                                }
                                            }
                                            addRecord = true;
                                        }

                                        for (int r = 1; r < tableRows.Count; r++)
                                        {
                                            //Nạp nội dung cho table
                                            if (string.IsNullOrEmpty(tableRows[r].Text))
                                                continue;
                                            var tdElements = tableRows[r].FindElements(OpenQA.Selenium.By.XPath("./*"));
                                            if (tdElements.Count > 0)
                                            {
                                                var rowRecord = new DataTables();
                                                rowRecord.CurrentAddress = url;
                                                var newColumnIndex = currentColumnIndex;
                                                foreach (var tdElement in tdElements)
                                                {
                                                    if (!string.IsNullOrEmpty(tdElement.Text))
                                                    {
                                                        //Cột trong lập trình bắt đầu từ 0,
                                                        //Cột trong định nghĩa bắt đầu từ 1
                                                        if (!string.IsNullOrEmpty(tdElement.Text))
                                                            rowRecord.SetMemberValue("Col" + newColumnIndex, GetTextByBehavior(tdElement.Text, extractorItemListWithSort[i]));
                                                    }
                                                    var colspan = tdElement.GetAttribute("colspan");
                                                    if (!string.IsNullOrEmpty(colspan))
                                                        //Nếu Merger cột thì phải cộng thêm số cột merger
                                                        newColumnIndex += Int32.Parse(colspan);
                                                    else
                                                        newColumnIndex++;

                                                }

                                                DataTablesResult.Add(rowRecord);
                                            }


                                        }

                                    }

                                    //if (i + 1 != extractorItemListWithSort.Count)
                                    //{
                                    //    Tools.ShowMessage(Application,"Thông báo", "Không thể lấy dữ liệu sau khi lấy dữ liệu bảng");
                                    //    break;
                                    //}

                                }

                            }
                            else if (extractorItemListWithSort[i].InsideTable)
                            {
                                if (extractorItemListWithSort[i].Row != null &&
                                    extractorItemListWithSort[i].Column != null)
                                    elementResult = GetElementInsideTable(elementResult,
                                        extractorItemListWithSort[i]);
                                else
                                    continue;
                            }
                        }
                        else
                        {

                            //Không tìm thấy dữ liệu
                        }
                        //var fd = driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div[2]/table/tbody/tr[9]/td/table"));
                        if (extractorItemListWithSort[i].IsGet())
                        {
                            //Lấy dữ liệu về
                            if (elementResult != null)
                            {
                                if (!extractorItemListWithSort[i].MultiRow)
                                {
                                    var elementResultText =
                                                                        GetElementValue(elementResult, extractorItemListWithSort[i], ConnectTimeOut);
                                    if (!string.IsNullOrEmpty(elementResultText))
                                    {
                                        dataTableRecord.SetMemberValue("Col" + startGetItem, GetTextByBehavior(elementResultText, extractorItemListWithSort[i]));
                                        addRecord = true;
                                    }
                                }
                                else if (elementResults?.Count > 0)
                                {
                                    foreach (var element in elementResults)
                                    {

                                        var elementResultText = GetElementValue(element, extractorItemListWithSort[i], ConnectTimeOut);
                                        if (!string.IsNullOrEmpty(elementResultText))
                                        {
                                            var rowRecord = new DataTables();
                                            rowRecord.CurrentAddress = url;
                                            rowRecord.SetMemberValue("Col" + startGetItem, GetTextByBehavior(elementResultText, extractorItemListWithSort[i]));
                                            DataTablesResult.Add(rowRecord);
                                        }
                                    }

                                }
                                startGetItem++;
                            }

                        }
                        else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Input)
                        {
                            //Gửi dữ liệu
                            if (elementResult != null)
                                elementResult.SendKeys(extractorItemListWithSort[i].Name);

                        }
                        else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Password)
                        {
                            if (elementResult != null)
                                elementResult.SendKeys(extractorItemListWithSort[i].Password);
                        }
                        else if (extractorItemListWithSort[i].ExtractorType == ExtractorType.Button)
                        {
                            if (elementResult != null)
                                //Gửi dữ liệu click
                                elementResult.Click();

                        }

                    }
                    catch (Exception ex)
                    {
                        if (application != null && extractorItemListWithSort[i].ExtractorType != ExtractorType.Delete)
                            Tools.ShowMessage(application, "Thông báo", "Thứ tự: " + extractorItemListWithSort[i].Order + ". Không tìm thấy " + extractorItemListWithSort[i].Name + ". Bạn có thể để thêm thời gian đợi trước quá trình tìm đối tượng", InformationType.Warning, 10);
                    }
                    //var test = driver.FindElementByXPath(extractorItemListWithSort[i].CssXpathValue);
                }
                if (addRecord)
                    DataTablesResult.Add(dataTableRecord);
                //FillHtmlContentAsync(url, false);

            }
            //Đóng kết nối

            driver.Quit();
            //driver.Close();

        }




        public OpenQA.Selenium.IWebElement GetElementInsideTable(OpenQA.Selenium.IWebElement webElement, ExtractorItem extractorItem)
        {
            if (webElement != null)
            {
                if (webElement.TagName.Equals("table"))
                {
                    var rowsElement = webElement.FindElements(OpenQA.Selenium.By.TagName("tr"));

                    var caption = string.Format(">{0}<", extractorItem.Name);
                    var otherCaption = System.Web.HttpUtility.HtmlEncode(extractorItem.Name);
                    var captions = extractorItem.Name.Split(new[] { ' ', '?', '*' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int index = 0; index < rowsElement.Count - extractorItem.Row + 1; index++)
                    {
                        var innerHtmlElement = rowsElement[index].GetAttribute("innerHTML");
                        if (innerHtmlElement is null)
                            continue;
                        if (innerHtmlElement.Contains(caption) || innerHtmlElement.Contains(otherCaption) ||
                             (!extractorItem.Exact && CheckInnerHtmlContainsArray(innerHtmlElement, captions)))
                        {
                            var rowElement = rowsElement[index + Convert.ToInt32(extractorItem.Row - 1)];
                            var columnsElement = rowElement.FindElements(OpenQA.Selenium.By.XPath("./*"));
                            //var columnsElement = rowElement.FindElements(By.TagName("td"));
                            if (columnsElement.Count > extractorItem.Column - 1)
                                return columnsElement[Convert.ToInt32(extractorItem.Column - 1)];
                        }
                    }
                }
                else
                {
                    //Lấy table thật
                    return GetElementInsideTable(webElement.FindElement(OpenQA.Selenium.By.TagName("table")), extractorItem);
                }
            }

            return null;
        }
        public string GetElementValue(OpenQA.Selenium.IWebElement webElement, ExtractorItem extractorItem, int requestTimeOut)
        {
            if (webElement != null)
            {
                if (webElement.TagName == "meta")
                    return webElement.GetAttribute("content");
                if (extractorItem.ExtractorType == ExtractorType.Text)
                {
                    return webElement.Text;
                }
                else if (extractorItem.ExtractorType == ExtractorType.Image)
                {
                    //Lấy ảnh
                    if (webElement.TagName == "img")
                        return webElement.GetAttribute("src");
                    else
                        return GetElementValue(webElement.FindElement(OpenQA.Selenium.By.TagName("img")), extractorItem, requestTimeOut);

                }
                else if (extractorItem.ExtractorType == ExtractorType.Link)
                {
                    if (webElement.TagName == "a")
                        return webElement.GetAttribute("href");
                    else
                        return GetElementValue(webElement.FindElement(OpenQA.Selenium.By.TagName("a")), extractorItem, requestTimeOut);
                }
                else if (extractorItem.ExtractorType == ExtractorType.ImageInLink)
                {
                    if (webElement.TagName == "a")
                    {
                        var href = webElement.GetAttribute("href");
                        if (!string.IsNullOrEmpty(href))
                        {
                            var options = new OpenQA.Selenium.Chrome.ChromeOptions();
                            options.AddArgument("no-sandbox");
                            // Chạy ngầm không pop up trình duyệt ra ngoài 
                            //options.AddArgument("headless");
                            var driver = new OpenQA.Selenium.Chrome.ChromeDriver(ChromeDriverPath, options, System.TimeSpan.FromSeconds(requestTimeOut));
                            string result = null;
                            try
                            {
                                driver.Url = href;
                                var imgElement = driver.FindElement(OpenQA.Selenium.By.TagName("img"));

                                if (imgElement != null)
                                    result = imgElement.GetAttribute("src");
                            }
                            catch { }

                            driver.Quit();
                            //ChromeDrivers.Add(driver);
                            //driver.Close();

                            return result;
                        }
                    }
                    else
                    {
                        return GetElementValue(webElement.FindElement(OpenQA.Selenium.By.TagName("a")), extractorItem, requestTimeOut);
                    }
                }
                else if (extractorItem.ExtractorType == ExtractorType.Html)
                {
                    return webElement.GetAttribute("innerHTML");
                }
            }

            return null;
        }
        private bool CheckInnerHtmlContainsArray(string cellValue, string[] array)
        {
            if (!string.IsNullOrEmpty(cellValue) && array.Length > 0)
            {
                foreach (string text in array)
                {
                    if (!cellValue.Contains(">" + text) && !cellValue.Contains(text + "<") && !cellValue.Contains(" " + text + " "))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private string GetTextByBehavior(string text, ExtractorItem extractorItem)
        {
            //Cột trong định nghĩa bắt đầu từ 1
            if (!string.IsNullOrEmpty(text) && ExtractorDataConfigurationList != null)
            {
                text = text.Trim();
                var extractorDataConfigurations = ExtractorDataConfigurationList.Where(m => m.ExtractorItem?.Oid == extractorItem?.Oid).OrderBy(m => m.Name);
                foreach (var extractorDataConfiguration in extractorDataConfigurations)
                {
                    if (extractorDataConfiguration.ExtractorDataBehavior == ExtractorDataBehavior.Number)
                    {
                        string numberText = string.Empty;
                        for (int i = 0; i < text.Length; i++)
                        {
                            if (text[i].Equals('.') || text[i].Equals(',') || Char.IsDigit(text[i]))
                                numberText += text[i];
                            else
                                break;
                        }

                        text = numberText;
                    }
                    else if (!string.IsNullOrEmpty(extractorDataConfiguration.Code))
                    {
                        //Giá trị cần lấy và thay thế là trường chuỗi
                        var valuesText = extractorDataConfiguration.Code.Split(';');
                        foreach (var valueText in valuesText)
                        {
                            int valueTextIndex = text.IndexOf(valueText);
                            if (valueTextIndex > 0)
                            {
                                if (extractorDataConfiguration.ExtractorDataBehavior == ExtractorDataBehavior.Left)
                                {
                                    text = text.Substring(0, valueTextIndex).Trim();
                                    break;
                                }
                                else if (extractorDataConfiguration.ExtractorDataBehavior == ExtractorDataBehavior.Right)
                                {
                                    text = text.Substring(text.LastIndexOf(valueText) + 1).Trim();
                                    break;
                                }
                                else if (extractorDataConfiguration.ExtractorDataBehavior == ExtractorDataBehavior.Replace)
                                {
                                    text = text.Replace(valueText, "").Trim();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return text;
        }


#endregion WebExtractorImportCode
#endregion WebExtractorImportCode
		 		 
    }
}
