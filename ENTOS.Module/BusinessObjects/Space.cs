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
	[NavigationItem("Location")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Địa bàn"), ImageName("Space")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(NativeName)+ "," + nameof(StartYear)+ "," + nameof(Width)+ "," + nameof(Depth)+ "," + nameof(Height)+ "," + nameof(Homepage)+ "," + nameof(Capital)+ "," + nameof(Longitude)+ "," + nameof(Latitude)+ "," + nameof(Angle), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
 
 
	[MobileColumnAttribute(Context = "Space_ListView", TargetItems = nameof(Name)+ "," + nameof(SpaceType)+ "," + nameof(Area))]
	[MobileColumnAttribute(Context = "Space_LowerSpaces_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Ethnicity_SpaceList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Space_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "SpaceGroup_Spaces_ListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
	[ModelDefault("IsCloneable", "True")]
[OptimisticLocking(true)]
    public partial class Space:  DevExpress.Xpo.XPLiteObject , DevExpress.Persistent.Base.General.ITreeNode , INoIndexColumn, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public Space(Session session)
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
				if (LowerSpaces.IsLoaded)
                {
                    if (LowerSpaces.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(LowerSpaces)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(LowerSpaces)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Space>(CriteriaOperator.Parse("[UpperSpace.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool lowerspaces = Session.Query<Module.BusinessObjects.Space>().Where(x => x.UpperSpace.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(LowerSpaces), lowerspaces);
                        if (lowerspaces)
                            return true;

                    }                    
                }				
				if (UpperLeftList.IsLoaded)
                {
                    if (UpperLeftList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(UpperLeftList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(UpperLeftList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.SpaceRelation>(CriteriaOperator.Parse("[Lower.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool upperleftlist = Session.Query<Module.BusinessObjects.SpaceRelation>().Where(x => x.Lower.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(UpperLeftList), upperleftlist);
                        if (upperleftlist)
                            return true;

                    }                    
                }				
				if (LowerRightList.IsLoaded)
                {
                    if (LowerRightList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(LowerRightList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(LowerRightList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.SpaceRelation>(CriteriaOperator.Parse("[Upper.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool lowerrightlist = Session.Query<Module.BusinessObjects.SpaceRelation>().Where(x => x.Upper.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(LowerRightList), lowerrightlist);
                        if (lowerrightlist)
                            return true;

                    }                    
                }				
				if (HistoryList.IsLoaded)
                {
                    if (HistoryList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(HistoryList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(HistoryList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.History>(CriteriaOperator.Parse("[Space.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool historylist = Session.Query<Module.BusinessObjects.History>().Where(x => x.Space.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(HistoryList), historylist);
                        if (historylist)
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

 		[Size(100)]
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

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(1)]		

 		[Size(100)]
		[RuleRequiredField("RequiredSpaceName", DefaultContexts.Save)]
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

	
       
		//private decimal? _area;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Diện tích")]
        [ToolTip("Diện tích")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Area
        { 
		    get => GetPropertyValue<decimal?>("Area");                         
			set => SetPropertyValue<decimal?>("Area", value); 
			
        }
		//Tooltip for Object
		public object AreaToolTipControllerText(View view)
        {
        //    if (Area != null) 
		//			return Area;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultArea(View view = null)
        { 
			return Area;
        }
		//Set Default Value
		public void SetDefaultArea(View view = null)
        {
            //if (Area is null){
            //    var result = GetDefaultArea(view);
            //    if (result != null && result != Area){
			//          Area = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AreaIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultArea();
				//if (result != null && Area != null){
				//	return !Area.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Int64 _priority;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dân số")]
        [ToolTip("Dân số")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public Int64 Priority
        { 
		    get => GetPropertyValue<Int64>("Priority");                         
			set => SetPropertyValue<Int64>("Priority", value); 
			
        }
		//Tooltip for Object
		public object PriorityToolTipControllerText(View view)
        {
        //    if (Priority != null) 
		//			return Priority;
            return null;
        }
		//Get Default Value
        public Int64 GetDefaultPriority(View view = null)
        { 
			return Priority;
        }
		//Set Default Value
		public void SetDefaultPriority(View view = null)
        {
            //if (Priority is null){
            //    var result = GetDefaultPriority(view);
            //    if (result != null && result != Priority){
			//          Priority = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PriorityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPriority();
				//if (result != null && Priority != null){
				//	return !Priority.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _nativename;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên bản ngữ")]
        [ToolTip("Tên bản ngữ")]
		//[Index(4)]		

 		[Size(100)]
		public string NativeName
        { 
		    get => GetPropertyValue<string>("NativeName");                         
			set => SetPropertyValue<string>("NativeName", value); 
			
        }
		//Tooltip for Object
		public object NativeNameToolTipControllerText(View view)
        {
        //    if (NativeName != null) 
		//			return NativeName;
            return null;
        }
		//Get Default Value
        public string GetDefaultNativeName(View view = null)
        { 
			return NativeName;
        }
		//Set Default Value
		public void SetDefaultNativeName(View view = null)
        {
            //if (NativeName is null){
            //    var result = GetDefaultNativeName(view);
            //    if (result != null && result != NativeName){
			//          NativeName = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NativeNameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNativeName();
				//if (result != null && NativeName != null){
				//	return !NativeName.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _startyear;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thành lập")]
        [ToolTip("Thành lập")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? StartYear
        { 
		    get => GetPropertyValue<int?>("StartYear");                         
			set => SetPropertyValue<int?>("StartYear", value); 
			
        }
		//Tooltip for Object
		public object StartYearToolTipControllerText(View view)
        {
        //    if (StartYear != null) 
		//			return StartYear;
            return null;
        }
		//Get Default Value
        public int? GetDefaultStartYear(View view = null)
        { 
			return StartYear;
        }
		//Set Default Value
		public void SetDefaultStartYear(View view = null)
        {
            //if (StartYear is null){
            //    var result = GetDefaultStartYear(view);
            //    if (result != null && result != StartYear){
			//          StartYear = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StartYearIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStartYear();
				//if (result != null && StartYear != null){
				//	return !StartYear.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _width;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chiều rộng")]
        [ToolTip("Chiều rộng")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Width
        { 
		    get => GetPropertyValue<decimal?>("Width");                         
			set => SetPropertyValue<decimal?>("Width", value); 
			
        }
		//Tooltip for Object
		public object WidthToolTipControllerText(View view)
        {
        //    if (Width != null) 
		//			return Width;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultWidth(View view = null)
        { 
			return Width;
        }
		//Set Default Value
		public void SetDefaultWidth(View view = null)
        {
            //if (Width is null){
            //    var result = GetDefaultWidth(view);
            //    if (result != null && result != Width){
			//          Width = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WidthIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWidth();
				//if (result != null && Width != null){
				//	return !Width.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _depth;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chiều dài")]
        [ToolTip("Chiều dài")]
		//[Index(7)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Depth
        { 
		    get => GetPropertyValue<decimal?>("Depth");                         
			set => SetPropertyValue<decimal?>("Depth", value); 
			
        }
		//Tooltip for Object
		public object DepthToolTipControllerText(View view)
        {
        //    if (Depth != null) 
		//			return Depth;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultDepth(View view = null)
        { 
			return Depth;
        }
		//Set Default Value
		public void SetDefaultDepth(View view = null)
        {
            //if (Depth is null){
            //    var result = GetDefaultDepth(view);
            //    if (result != null && result != Depth){
			//          Depth = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DepthIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDepth();
				//if (result != null && Depth != null){
				//	return !Depth.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _height;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chiều cao")]
        [ToolTip("Chiều cao")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Height
        { 
		    get => GetPropertyValue<decimal?>("Height");                         
			set => SetPropertyValue<decimal?>("Height", value); 
			
        }
		//Tooltip for Object
		public object HeightToolTipControllerText(View view)
        {
        //    if (Height != null) 
		//			return Height;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultHeight(View view = null)
        { 
			return Height;
        }
		//Set Default Value
		public void SetDefaultHeight(View view = null)
        {
            //if (Height is null){
            //    var result = GetDefaultHeight(view);
            //    if (result != null && result != Height){
			//          Height = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool HeightIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultHeight();
				//if (result != null && Height != null){
				//	return !Height.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.SpaceType _spacetype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SpaceTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.SpaceType SpaceType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.SpaceType>("SpaceType");                         
			set => SetPropertyValue<Module.BusinessObjects.SpaceType>("SpaceType", value); 
			
        }
		//Tooltip for Object
		public object SpaceTypeToolTipControllerText(View view)
        {
        //    if (SpaceType != null) 
		//			return SpaceType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.SpaceType GetDefaultSpaceType(View view = null)
        { 
			return SpaceType;
        }
		//Set Default Value
		public void SetDefaultSpaceType(View view = null)
        {
            //if (SpaceType is null){
            //    var result = GetDefaultSpaceType(view);
            //    if (result != null && result != SpaceType){
			//          SpaceType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpaceTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpaceType();
				//if (result != null && SpaceType != null){
				//	return !SpaceType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SpaceTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SpaceType));
            }
        }
	
       
		//private Module.BusinessObjects.Space _upperspace;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(10)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpperSpaceCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("UpperSpace-LowerSpaces")]
	 
		public Module.BusinessObjects.Space UpperSpace
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Space>("UpperSpace");                         
			set => SetPropertyValue<Module.BusinessObjects.Space>("UpperSpace", value); 
			
        }
		//Tooltip for Object
		public object UpperSpaceToolTipControllerText(View view)
        {
        //    if (UpperSpace != null) 
		//			return UpperSpace;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Space GetDefaultUpperSpace(View view = null)
        { 
			return UpperSpace;
        }
		//Set Default Value
		public void SetDefaultUpperSpace(View view = null)
        {
            //if (UpperSpace is null){
            //    var result = GetDefaultUpperSpace(view);
            //    if (result != null && result != UpperSpace){
			//          UpperSpace = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperSpaceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpperSpace();
				//if (result != null && UpperSpace != null){
				//	return !UpperSpace.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpperSpaceCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(UpperSpace));
            }
        }
	
       
		//private string _domainname;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên miền")]
        [ToolTip("Tên miền")]
		//[Index(11)]		

 		[Size(100)]
		public string DomainName
        { 
		    get => GetPropertyValue<string>("DomainName");                         
			set => SetPropertyValue<string>("DomainName", value); 
			
        }
		//Tooltip for Object
		public object DomainNameToolTipControllerText(View view)
        {
        //    if (DomainName != null) 
		//			return DomainName;
            return null;
        }
		//Get Default Value
        public string GetDefaultDomainName(View view = null)
        { 
			return DomainName;
        }
		//Set Default Value
		public void SetDefaultDomainName(View view = null)
        {
            //if (DomainName is null){
            //    var result = GetDefaultDomainName(view);
            //    if (result != null && result != DomainName){
			//          DomainName = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DomainNameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDomainName();
				//if (result != null && DomainName != null){
				//	return !DomainName.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ảnh")]
        [ToolTip("Ảnh")]
		//[Index(12)]		
		[Appearance("ẢnhBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Image
        { 
		    get => GetPropertyValue<byte[]>("Image");                         
			set => SetPropertyValue<byte[]>("Image", value); 
			
        }
		//Tooltip for Object
		public object ImageToolTipControllerText(View view)
        {
        //    if (Image != null) 
		//			return Image;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultImage(View view = null)
        { 
			return Image;
        }
		//Set Default Value
		public void SetDefaultImage(View view = null)
        {
            //if (Image is null){
            //    var result = GetDefaultImage(view);
            //    if (result != null && result != Image){
			//          Image = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ImageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImage();
				//if (result != null && Image != null){
				//	return !Image.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _homepage;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trang chủ")]
        [ToolTip("Trang chủ")]
		//[Index(13)]		

 		[Size(200)]
		public string Homepage
        { 
		    get => GetPropertyValue<string>("Homepage");                         
			set => SetPropertyValue<string>("Homepage", value); 
			
        }
		//Tooltip for Object
		public object HomepageToolTipControllerText(View view)
        {
        //    if (Homepage != null) 
		//			return Homepage;
            return null;
        }
		//Get Default Value
        public string GetDefaultHomepage(View view = null)
        { 
			return Homepage;
        }
		//Set Default Value
		public void SetDefaultHomepage(View view = null)
        {
            //if (Homepage is null){
            //    var result = GetDefaultHomepage(view);
            //    if (result != null && result != Homepage){
			//          Homepage = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool HomepageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultHomepage();
				//if (result != null && Homepage != null){
				//	return !Homepage.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Space _capital;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trung tâm")]
        [ToolTip("Trung tâm")]
		//[Index(14)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CapitalCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Space Capital
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Space>("Capital");                         
			set => SetPropertyValue<Module.BusinessObjects.Space>("Capital", value); 
			
        }
		//Tooltip for Object
		public object CapitalToolTipControllerText(View view)
        {
        //    if (Capital != null) 
		//			return Capital;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Space GetDefaultCapital(View view = null)
        { 
			return Capital;
        }
		//Set Default Value
		public void SetDefaultCapital(View view = null)
        {
            //if (Capital is null){
            //    var result = GetDefaultCapital(view);
            //    if (result != null && result != Capital){
			//          Capital = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CapitalIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCapital();
				//if (result != null && Capital != null){
				//	return !Capital.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CapitalCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Capital));
            }
        }
	
       
		//private decimal? _longitude;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tọa độ X")]
        [ToolTip("Tọa độ X")]
		//[Index(15)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Longitude
        { 
		    get => GetPropertyValue<decimal?>("Longitude");                         
			set => SetPropertyValue<decimal?>("Longitude", value); 
			
        }
		//Tooltip for Object
		public object LongitudeToolTipControllerText(View view)
        {
        //    if (Longitude != null) 
		//			return Longitude;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultLongitude(View view = null)
        { 
			return Longitude;
        }
		//Set Default Value
		public void SetDefaultLongitude(View view = null)
        {
            //if (Longitude is null){
            //    var result = GetDefaultLongitude(view);
            //    if (result != null && result != Longitude){
			//          Longitude = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LongitudeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLongitude();
				//if (result != null && Longitude != null){
				//	return !Longitude.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _latitude;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tọa độ Y")]
        [ToolTip("Tọa độ Y")]
		//[Index(16)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Latitude
        { 
		    get => GetPropertyValue<decimal?>("Latitude");                         
			set => SetPropertyValue<decimal?>("Latitude", value); 
			
        }
		//Tooltip for Object
		public object LatitudeToolTipControllerText(View view)
        {
        //    if (Latitude != null) 
		//			return Latitude;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultLatitude(View view = null)
        { 
			return Latitude;
        }
		//Set Default Value
		public void SetDefaultLatitude(View view = null)
        {
            //if (Latitude is null){
            //    var result = GetDefaultLatitude(view);
            //    if (result != null && result != Latitude){
			//          Latitude = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LatitudeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLatitude();
				//if (result != null && Latitude != null){
				//	return !Latitude.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _angle;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Góc")]
        [ToolTip("Góc")]
		//[Index(17)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Angle
        { 
		    get => GetPropertyValue<int?>("Angle");                         
			set => SetPropertyValue<int?>("Angle", value); 
			
        }
		//Tooltip for Object
		public object AngleToolTipControllerText(View view)
        {
        //    if (Angle != null) 
		//			return Angle;
            return null;
        }
		//Get Default Value
        public int? GetDefaultAngle(View view = null)
        { 
			return Angle;
        }
		//Set Default Value
		public void SetDefaultAngle(View view = null)
        {
            //if (Angle is null){
            //    var result = GetDefaultAngle(view);
            //    if (result != null && result != Angle){
			//          Angle = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AngleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAngle();
				//if (result != null && Angle != null){
				//	return !Angle.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp dưới")]
		//[Index(18)]
		[DevExpress.Xpo.Association("UpperSpace-LowerSpaces")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Space> LowerSpaces
        {      
		    get => GetCollection<Module.BusinessObjects.Space>("LowerSpaces"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhóm")]
		//[Index(19)]
		[DataSourceCriteria("Not Spaces[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("SpaceGroups-Spaces")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.SpaceGroup> SpaceGroups
        {      
		    get => GetCollection<Module.BusinessObjects.SpaceGroup>("SpaceGroups"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kề trên trái")]
		//[Index(20)]
		[DevExpress.Xpo.Association("Lower-UpperLeftList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.SpaceRelation> UpperLeftList
        {      
		    get => GetCollection<Module.BusinessObjects.SpaceRelation>("UpperLeftList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kề dưới phải")]
		//[Index(21)]
		[DevExpress.Xpo.Association("Upper-LowerRightList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.SpaceRelation> LowerRightList
        {      
		    get => GetCollection<Module.BusinessObjects.SpaceRelation>("LowerRightList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dân tộc")]
		//[Index(22)]
		[DataSourceCriteria("Not SpaceList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("EthnicityList-SpaceList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Ethnicity> EthnicityList
        {      
		    get => GetCollection<Module.BusinessObjects.Ethnicity>("EthnicityList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lịch sử")]
		//[Index(23)]
		[DevExpress.Xpo.Association("Space-HistoryList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.History> HistoryList
        {      
		    get => GetCollection<Module.BusinessObjects.History>("HistoryList"); 
			
        }
       
		//private string _english;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên khác")]
        [ToolTip("Tên khác")]
		//[Index(24)]		

 		[Size(100)]
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

	
       
		//private int? _endyear;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kết thúc")]
        [ToolTip("Kết thúc")]
		//[Index(25)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? EndYear
        { 
		    get => GetPropertyValue<int?>("EndYear");                         
			set => SetPropertyValue<int?>("EndYear", value); 
			
        }
		//Tooltip for Object
		public object EndYearToolTipControllerText(View view)
        {
        //    if (EndYear != null) 
		//			return EndYear;
            return null;
        }
		//Get Default Value
        public int? GetDefaultEndYear(View view = null)
        { 
			return EndYear;
        }
		//Set Default Value
		public void SetDefaultEndYear(View view = null)
        {
            //if (EndYear is null){
            //    var result = GetDefaultEndYear(view);
            //    if (result != null && result != EndYear){
			//          EndYear = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EndYearIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEndYear();
				//if (result != null && EndYear != null){
				//	return !EndYear.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Folder _memberfolder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(26)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#Member# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Folder MemberFolder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Folder>("MemberFolder");                         
			set => SetPropertyValue<Module.BusinessObjects.Folder>("MemberFolder", value); 
			
        }
		//Tooltip for Object
		public object MemberFolderToolTipControllerText(View view)
        {
        //    if (MemberFolder != null) 
		//			return MemberFolder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Folder GetDefaultMemberFolder(View view = null)
        { 
			return MemberFolder;
        }
		//Set Default Value
		public void SetDefaultMemberFolder(View view = null)
        {
            //if (MemberFolder is null){
            //    var result = GetDefaultMemberFolder(view);
            //    if (result != null && result != MemberFolder){
			//          MemberFolder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MemberFolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMemberFolder();
				//if (result != null && MemberFolder != null){
				//	return !MemberFolder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator MemberFolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(MemberFolder));
            }
        }
	
       
		//private DevExpress.Persistent.Base.General.ITreeNode _parent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Parent")]
        [ToolTip("Parent")]
		//[Index(27)]		
	    [Browsable(false)]
		public DevExpress.Persistent.Base.General.ITreeNode Parent
        { 
		    #region 0286ImportCode 
get => UpperSpace;
#endregion 0286ImportCode
			
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
		//[Index(28)]		
	    [Browsable(false)]
		public System.ComponentModel.IBindingList Children
        { 
		    #region 0270ImportCode 
get => LowerSpaces;
#endregion 0270ImportCode
			
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
 
            base.AfterConstruction();
            Display = true;
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultArea(View view = null);
        //SetDefaultPriority(View view = null);
        //SetDefaultNativeName(View view = null);
        //SetDefaultStartYear(View view = null);
        //SetDefaultWidth(View view = null);
        //SetDefaultDepth(View view = null);
        //SetDefaultHeight(View view = null);
        //SetDefaultSpaceType(View view = null);
        //SetDefaultUpperSpace(View view = null);
        //SetDefaultDomainName(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultHomepage(View view = null);
        //SetDefaultCapital(View view = null);
        //SetDefaultLongitude(View view = null);
        //SetDefaultLatitude(View view = null);
        //SetDefaultAngle(View view = null);
        //SetDefaultEnglish(View view = null);
        //SetDefaultEndYear(View view = null);
        //SetDefaultMemberFolder(View view = null);
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
			//	SetDefaultLowerSpaces();
			//	SetDefaultSpaceGroups();
			//	SetDefaultUpperLeftList();
			//	SetDefaultLowerRightList();
			//	SetDefaultEthnicityList();
			//	SetDefaultHistoryList();
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
