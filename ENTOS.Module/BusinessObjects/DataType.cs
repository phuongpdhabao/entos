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
    [ModelDefault("Caption", "Kiểu dữ liệu"), ImageName("DataType")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("DataType Code, Name None_None__Color [A=255, R=0, G=0, B=255]" , TargetItems = "Code, Name" , Criteria = "[Extract] = True",AppearanceItemType = "ViewItem", Context = "DetailView" , FontColor = "#0000FF" )]
	[Appearance("DataType UserInterfaceType, SoftwareClassType, InheritDataType Hide_None__" , TargetItems = "UserInterfaceType, SoftwareClassType, InheritDataType" , Criteria = "[DataTypeCategory] <> ##ToString#SoftwareClass#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
	[Appearance("DataType FrameworkDataTypeList, Note, SourceCodeBuffer, DataTypeProjectFolderList, SoftwareAttributeList Hide_None__" , TargetItems = "FrameworkDataTypeList, Note, SourceCodeBuffer, DataTypeProjectFolderList, SoftwareAttributeList" , Criteria = "[SoftwareLibrary] Is Not Null Or [SoftwareNameSpace] Is Not Null",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("DataType GenericType Hide_None__" , TargetItems = "GenericType" , Criteria = "[DataTypeCategory] = ##ToString#eNum#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
	[Appearance("DataType Parameter Hide_None__" , TargetItems = "Parameter" , Criteria = "[DataTypeCategory] <> ##ToString#Delegate#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
	[Appearance("DataType DataTypeMemberList Hide_None__" , TargetItems = "DataTypeMemberList" , Criteria = "[DataTypeCategory] = ##ToString#Delegate#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("DataType DataTypeT2 Hide_None__" , TargetItems = "DataTypeT2" , Criteria = "[InheritDataType] Is Null Or [InheritDataType.GenericType] <> ##ToString#Generic2#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
	[Appearance("DataType Link Hide_None__" , TargetItems = "Link" , Criteria = "[SoftwareNameSpace] Is Null",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
	[Appearance("DataType DefaultDataTypeMemberList Hide_None__" , TargetItems = "DefaultDataTypeMemberList" , Criteria = "[DataTypeCategory] <> ##ToString#Struct# And [DataTypeCategory] <> ##ToString#SoftwareClass# Or [DataTypeCategory] = ##ToString#SoftwareClass# And [SoftwareClassType] = ##ToString#SoftwareAttribute#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("DataType DataTypeList Hide_None__" , TargetItems = "DataTypeList" , Criteria = "[DataTypeCategory] <> ##ToString#Interface#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("DataType SoftwareObjectAttribute, TemplateSoftwareAttributeList, FieldAttribute Hide_None__" , TargetItems = "SoftwareObjectAttribute, TemplateSoftwareAttributeList, FieldAttribute" , Criteria = "[SoftwareClassType] <> ##ToString#SoftwareAttribute#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
	[Appearance("DataType ActionList Hide_None__" , TargetItems = "ActionList" , Criteria = "[DataTypeCategory] <> ##ToString#Interface# And [SoftwareObjectType] <> ##ToString#SoftwareObject# Or [DataTypeCategory] = ##ToString#Interface# And Not Contains([SoftwareNameSpace.Name], '{Solution}')",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("DataType InterfaceDataTypeList Hide_None__" , TargetItems = "InterfaceDataTypeList" , Criteria = "[DataTypeCategory] = ##ToString#eNum# Or [DataTypeCategory] = ##ToString#Struct# Or [DataTypeCategory] = ##ToString#Delegate#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("DataType FormatList, DisplayFormat, EditFormat Hide_None__" , TargetItems = "FormatList, DisplayFormat, EditFormat" , Criteria = "[DataTypeCategory] <> ##ToString#Struct#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
	[Appearance("DataType DataTypeT1 Hide_None__" , TargetItems = "DataTypeT1" , Criteria = "[InheritDataType] Is Null Or [InheritDataType.GenericType] Is Null",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Name)+ "," + nameof(FullName)+ "," + nameof(SourceCode)+ "," + nameof(Update)+ "," + nameof(Updater)+ "," + nameof(SoftwareObjectType)+ "," + nameof(CreatedDate)+ "," + nameof(Member))]
 
	[MobileColumnAttribute(Context = "DataType_LookupListView", TargetItems = nameof(Name)+ "," + nameof(DataTypeCategory))]
	[MobileColumnAttribute(Context = "ProjectFile_DataTypeList_ListView", TargetItems = nameof(Name)+ "," + nameof(DataTypeCategory))]
	[MobileColumnAttribute(Context = "SoftwareBusiness_DataTypeList_ListView", TargetItems = nameof(DataTypeCategory)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "DataType_ListView", TargetItems = nameof(DataTypeCategory)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "SoftwareNameSpace_DataTypeList_ListView", TargetItems = nameof(Name)+ "," + nameof(DataTypeCategory))]
	[MobileColumnAttribute(Context = "SoftwareLibrary_DataTypeList_ListView", TargetItems = nameof(DataTypeCategory)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "SoftwareFile_DataTypeList_ListView", TargetItems = nameof(DataTypeCategory)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "DataType_InterfaceDataTypeList_ListView", TargetItems = nameof(Name)+ "," + nameof(DataTypeCategory))]
	[MobileColumnAttribute(Context = "DataType_InheritedDataTypeList_ListView", TargetItems = nameof(Name)+ "," + nameof(DataTypeCategory))]
	[MobileColumnAttribute(Context = "DataType_DataTypeList_ListView", TargetItems = nameof(DataTypeCategory)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Format_DataTypeList_ListView", TargetItems = nameof(Name)+ "," + nameof(DataTypeCategory))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class DataType:  DevExpress.Xpo.XPLiteObject , ISourceCode , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public DataType(Session session)
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
				if (InheritedDataTypeList.IsLoaded)
                {
                    if (InheritedDataTypeList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(InheritedDataTypeList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(InheritedDataTypeList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.DataType>(CriteriaOperator.Parse("[InheritDataType.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool inheriteddatatypelist = Session.Query<Module.BusinessObjects.DataType>().Where(x => x.InheritDataType.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(InheritedDataTypeList), inheriteddatatypelist);
                        if (inheriteddatatypelist)
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
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(0)]		

 		[Size(50)]
		[RuleRequiredField("RequiredDataTypeCode", DefaultContexts.Save)]
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

 		[Size(50)]
		[RuleRequiredField("RequiredDataTypeName", DefaultContexts.Save)]
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

	
       
		//private DataTypeCategory? _datatypecategory;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(2)]		
	    [ImmediatePostData()]
		public DataTypeCategory? DataTypeCategory
        { 
		    get => GetPropertyValue<DataTypeCategory?>("DataTypeCategory");                         
			set => SetPropertyValue<DataTypeCategory?>("DataTypeCategory", value); 
			
        }
		//Tooltip for Object
		public object DataTypeCategoryToolTipControllerText(View view)
        {
        //    if (DataTypeCategory != null) 
		//			return DataTypeCategory;
            return null;
        }
		//Get Default Value
        public DataTypeCategory? GetDefaultDataTypeCategory(View view = null)
        { 
			return DataTypeCategory;
        }
		//Set Default Value
		public void SetDefaultDataTypeCategory(View view = null)
        {
            //if (DataTypeCategory is null){
            //    var result = GetDefaultDataTypeCategory(view);
            //    if (result != null && result != DataTypeCategory){
			//          DataTypeCategory = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DataTypeCategoryIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDataTypeCategory();
				//if (result != null && DataTypeCategory != null){
				//	return !DataTypeCategory.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private SoftwareClassType? _softwareclasstype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại lớp")]
        [ToolTip("Loại lớp")]
		//[Index(3)]		
		public SoftwareClassType? SoftwareClassType
        { 
		    get => GetPropertyValue<SoftwareClassType?>("SoftwareClassType");                         
			set => SetPropertyValue<SoftwareClassType?>("SoftwareClassType", value); 
			
        }
		//Tooltip for Object
		public object SoftwareClassTypeToolTipControllerText(View view)
        {
        //    if (SoftwareClassType != null) 
		//			return SoftwareClassType;
            return null;
        }
		//Get Default Value
        public SoftwareClassType? GetDefaultSoftwareClassType(View view = null)
        { 
			return SoftwareClassType;
        }
		//Set Default Value
		public void SetDefaultSoftwareClassType(View view = null)
        {
            //if (SoftwareClassType is null){
            //    var result = GetDefaultSoftwareClassType(view);
            //    if (result != null && result != SoftwareClassType){
			//          SoftwareClassType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SoftwareClassTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSoftwareClassType();
				//if (result != null && SoftwareClassType != null){
				//	return !SoftwareClassType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.DataType _inheritdatatype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thừa kế")]
        [ToolTip("Thừa kế")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[DataTypeCategory] = ##ToString#SoftwareClass#")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("InheritDataType-InheritedDataTypeList")]
	 
		public Module.BusinessObjects.DataType InheritDataType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.DataType>("InheritDataType");                         
			set => SetPropertyValue<Module.BusinessObjects.DataType>("InheritDataType", value); 
			
        }
		//Tooltip for Object
		public object InheritDataTypeToolTipControllerText(View view)
        {
        //    if (InheritDataType != null) 
		//			return InheritDataType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.DataType GetDefaultInheritDataType(View view = null)
        { 
			return InheritDataType;
        }
		//Set Default Value
		public void SetDefaultInheritDataType(View view = null)
        {
            //if (InheritDataType is null){
            //    var result = GetDefaultInheritDataType(view);
            //    if (result != null && result != InheritDataType){
			//          InheritDataType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool InheritDataTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultInheritDataType();
				//if (result != null && InheritDataType != null){
				//	return !InheritDataType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator InheritDataTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(InheritDataType));
            }
        }
	
       
		//private AccessModifier _accessmodifier;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Phạm vi")]
        [ToolTip("Phạm vi")]
		//[Index(5)]		
		public AccessModifier AccessModifier
        { 
		    get => GetPropertyValue<AccessModifier>("AccessModifier");                         
			set => SetPropertyValue<AccessModifier>("AccessModifier", value); 
			
        }
		//Tooltip for Object
		public object AccessModifierToolTipControllerText(View view)
        {
        //    if (AccessModifier != null) 
		//			return AccessModifier;
            return null;
        }
		//Get Default Value
        public AccessModifier GetDefaultAccessModifier(View view = null)
        { 
			return AccessModifier;
        }
		//Set Default Value
		public void SetDefaultAccessModifier(View view = null)
        {
            //if (AccessModifier is null){
            //    var result = GetDefaultAccessModifier(view);
            //    if (result != null && result != AccessModifier){
			//          AccessModifier = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AccessModifierIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAccessModifier();
				//if (result != null && AccessModifier != null){
				//	return !AccessModifier.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _datatypemodifier;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sửa đổi")]
        [ToolTip("Sửa đổi")]
		//[Index(6)]		

 		[Size(250)]
	    [ModelDefault("PropertyEditorType", "EnumCheckedListBoxPropertyEditor")]
	    [TypeConverter(typeof(DataTypeModifier))]
		public string DataTypeModifier
        { 
		    get => GetPropertyValue<string>("DataTypeModifier");                         
			set => SetPropertyValue<string>("DataTypeModifier", value); 
			
        }
		//Tooltip for Object
		public object DataTypeModifierToolTipControllerText(View view)
        {
        //    if (DataTypeModifier != null) 
		//			return DataTypeModifier;
            return null;
        }
		//Get Default Value
        public string GetDefaultDataTypeModifier(View view = null)
        { 
			return DataTypeModifier;
        }
		//Set Default Value
		public void SetDefaultDataTypeModifier(View view = null)
        {
            //if (DataTypeModifier is null){
            //    var result = GetDefaultDataTypeModifier(view);
            //    if (result != null && result != DataTypeModifier){
			//          DataTypeModifier = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DataTypeModifierIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDataTypeModifier();
				//if (result != null && DataTypeModifier != null){
				//	return !DataTypeModifier.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _parameter;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tham số")]
        [ToolTip("Tham số")]
		//[Index(7)]		

 		[Size(250)]
		public string Parameter
        { 
		    get => GetPropertyValue<string>("Parameter");                         
			set => SetPropertyValue<string>("Parameter", value); 
			
        }
		//Tooltip for Object
		public object ParameterToolTipControllerText(View view)
        {
        //    if (Parameter != null) 
		//			return Parameter;
            return null;
        }
		//Get Default Value
        public string GetDefaultParameter(View view = null)
        { 
			return Parameter;
        }
		//Set Default Value
		public void SetDefaultParameter(View view = null)
        {
            //if (Parameter is null){
            //    var result = GetDefaultParameter(view);
            //    if (result != null && result != Parameter){
			//          Parameter = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ParameterIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultParameter();
				//if (result != null && Parameter != null){
				//	return !Parameter.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _fullname;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên đủ")]
        [ToolTip("Tên đủ")]
		//[Index(14)]		

 		[Size(250)]
		public string FullName
        { 
		    get => GetPropertyValue<string>("FullName");                         
			set => SetPropertyValue<string>("FullName", value); 
			
        }
		//Tooltip for Object
		public object FullNameToolTipControllerText(View view)
        {
        //    if (FullName != null) 
		//			return FullName;
            return null;
        }
		//Get Default Value
        public string GetDefaultFullName(View view = null)
        { 
			return FullName;
        }
		//Set Default Value

		//Check Not Validate
		protected bool FullNameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFullName();
				//if (result != null && FullName != null){
				//	return !FullName.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _link;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
        [ToolTip("Liên kết")]
		//[Index(15)]		

 		[Size(250)]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string Link
        { 
		    get => GetPropertyValue<string>("Link");                         
			set => SetPropertyValue<string>("Link", value); 
			
        }
		//Tooltip for Object
		public object LinkToolTipControllerText(View view)
        {
        //    if (Link != null) 
		//			return Link;
            return null;
        }
		//Get Default Value
        public string GetDefaultLink(View view = null)
        { 
			return Link;
        }
		//Set Default Value
		public void SetDefaultLink(View view = null)
        {
            //if (Link is null){
            //    var result = GetDefaultLink(view);
            //    if (result != null && result != Link){
			//          Link = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LinkIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLink();
				//if (result != null && Link != null){
				//	return !Link.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private GenericType? _generictype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại Generic")]
        [ToolTip("Loại Generic")]
		//[Index(16)]		
		public GenericType? GenericType
        { 
		    get => GetPropertyValue<GenericType?>("GenericType");                         
			set => SetPropertyValue<GenericType?>("GenericType", value); 
			
        }
		//Tooltip for Object
		public object GenericTypeToolTipControllerText(View view)
        {
        //    if (GenericType != null) 
		//			return GenericType;
            return null;
        }
		//Get Default Value
        public GenericType? GetDefaultGenericType(View view = null)
        { 
			return GenericType;
        }
		//Set Default Value
		public void SetDefaultGenericType(View view = null)
        {
            //if (GenericType is null){
            //    var result = GetDefaultGenericType(view);
            //    if (result != null && result != GenericType){
			//          GenericType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool GenericTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultGenericType();
				//if (result != null && GenericType != null){
				//	return !GenericType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.DataType _datatypet1;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu T1")]
        [ToolTip("Kiểu T1")]
		//[Index(17)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DataTypeT1Criteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.DataType DataTypeT1
        { 
		    get => GetPropertyValue<Module.BusinessObjects.DataType>("DataTypeT1");                         
			set => SetPropertyValue<Module.BusinessObjects.DataType>("DataTypeT1", value); 
			
        }
		//Tooltip for Object
		public object DataTypeT1ToolTipControllerText(View view)
        {
        //    if (DataTypeT1 != null) 
		//			return DataTypeT1;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.DataType GetDefaultDataTypeT1(View view = null)
        { 
			return DataTypeT1;
        }
		//Set Default Value
		public void SetDefaultDataTypeT1(View view = null)
        {
            //if (DataTypeT1 is null){
            //    var result = GetDefaultDataTypeT1(view);
            //    if (result != null && result != DataTypeT1){
			//          DataTypeT1 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DataTypeT1IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDataTypeT1();
				//if (result != null && DataTypeT1 != null){
				//	return !DataTypeT1.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator DataTypeT1Criteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(DataTypeT1));
            }
        }
	
       
		//private Module.BusinessObjects.DataType _datatypet2;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu T2")]
        [ToolTip("Kiểu T2")]
		//[Index(18)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DataTypeT2Criteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.DataType DataTypeT2
        { 
		    get => GetPropertyValue<Module.BusinessObjects.DataType>("DataTypeT2");                         
			set => SetPropertyValue<Module.BusinessObjects.DataType>("DataTypeT2", value); 
			
        }
		//Tooltip for Object
		public object DataTypeT2ToolTipControllerText(View view)
        {
        //    if (DataTypeT2 != null) 
		//			return DataTypeT2;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.DataType GetDefaultDataTypeT2(View view = null)
        { 
			return DataTypeT2;
        }
		//Set Default Value
		public void SetDefaultDataTypeT2(View view = null)
        {
            //if (DataTypeT2 is null){
            //    var result = GetDefaultDataTypeT2(view);
            //    if (result != null && result != DataTypeT2){
			//          DataTypeT2 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DataTypeT2IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDataTypeT2();
				//if (result != null && DataTypeT2 != null){
				//	return !DataTypeT2.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator DataTypeT2Criteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(DataTypeT2));
            }
        }
	
       
		//private Module.BusinessObjects.SourceCode _sourcecode;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã nguồn ID")]
        [ToolTip("Mã nguồn ID")]
		//[Index(19)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SourceCodeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [NonCloneable()]
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
	
       
		//private bool _softwareobjectattribute;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đối tượng")]
        [ToolTip("Đối tượng")]
		//[Index(20)]		
		public bool SoftwareObjectAttribute
        { 
		    get => GetPropertyValue<bool>("SoftwareObjectAttribute");                         
			set => SetPropertyValue<bool>("SoftwareObjectAttribute", value); 
			
        }
		//Tooltip for Object
		public object SoftwareObjectAttributeToolTipControllerText(View view)
        {
        //    if (SoftwareObjectAttribute != null) 
		//			return SoftwareObjectAttribute;
            return null;
        }
		//Get Default Value
        public bool GetDefaultSoftwareObjectAttribute(View view = null)
        { 
			return SoftwareObjectAttribute;
        }
		//Set Default Value
		public void SetDefaultSoftwareObjectAttribute(View view = null)
        {
            //if (SoftwareObjectAttribute is null){
            //    var result = GetDefaultSoftwareObjectAttribute(view);
            //    if (result != null && result != SoftwareObjectAttribute){
			//          SoftwareObjectAttribute = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SoftwareObjectAttributeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSoftwareObjectAttribute();
				//if (result != null && SoftwareObjectAttribute != null){
				//	return !SoftwareObjectAttribute.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _fieldattribute;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trường")]
        [ToolTip("Trường")]
		//[Index(21)]		
		public bool FieldAttribute
        { 
		    get => GetPropertyValue<bool>("FieldAttribute");                         
			set => SetPropertyValue<bool>("FieldAttribute", value); 
			
        }
		//Tooltip for Object
		public object FieldAttributeToolTipControllerText(View view)
        {
        //    if (FieldAttribute != null) 
		//			return FieldAttribute;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFieldAttribute(View view = null)
        { 
			return FieldAttribute;
        }
		//Set Default Value
		public void SetDefaultFieldAttribute(View view = null)
        {
            //if (FieldAttribute is null){
            //    var result = GetDefaultFieldAttribute(view);
            //    if (result != null && result != FieldAttribute){
			//          FieldAttribute = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FieldAttributeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFieldAttribute();
				//if (result != null && FieldAttribute != null){
				//	return !FieldAttribute.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Interface")]
		//[Index(27)]
		[DataSourceCriteria("[DataTypeCategory] = ##ToString#Interface#")]
		[DevExpress.Xpo.Association("DataTypeList-InterfaceDataTypeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.DataType> InterfaceDataTypeList
        {      
		    get => GetCollection<Module.BusinessObjects.DataType>("InterfaceDataTypeList"); 
			
        }
       
		//private string _sourcecodebuffer;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã đệm")]
        [ToolTip("Mã đệm")]
		//[Index(30)]		

 		[Size(SizeAttribute.Unlimited)]
	    [ModelDefault("PropertyEditorType", "CSCodePropertyEditor")]
		public string SourceCodeBuffer
        { 
		    get => GetPropertyValue<string>("SourceCodeBuffer");                         
			set => SetPropertyValue<string>("SourceCodeBuffer", value); 
			
        }
		//Tooltip for Object
		public object SourceCodeBufferToolTipControllerText(View view)
        {
        //    if (SourceCodeBuffer != null) 
		//			return SourceCodeBuffer;
            return null;
        }
		//Get Default Value
        public string GetDefaultSourceCodeBuffer(View view = null)
        { 
			return SourceCodeBuffer;
        }
		//Set Default Value
		public void SetDefaultSourceCodeBuffer(View view = null)
        {
            //if (SourceCodeBuffer is null){
            //    var result = GetDefaultSourceCodeBuffer(view);
            //    if (result != null && result != SourceCodeBuffer){
			//          SourceCodeBuffer = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SourceCodeBufferIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSourceCodeBuffer();
				//if (result != null && SourceCodeBuffer != null){
				//	return !SourceCodeBuffer.Equals(result);
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
		//[Index(31)]		

 		[Size(SizeAttribute.Unlimited)]
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
	

	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Được thừa kế Lớp")]
		//[Index(34)]
		[DevExpress.Xpo.Association("InheritDataType-InheritedDataTypeList")]
	    [VisibleInDetailView(false)]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.DataType> InheritedDataTypeList
        {      
		    get => GetCollection<Module.BusinessObjects.DataType>("InheritedDataTypeList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	

	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Được thừa kế Interface")]
		//[Index(35)]
		[DataSourceCriteria("[DataTypeCategory] = ##ToString#SoftwareClass# Or [DataTypeCategory] = ##ToString#SoftwareEditor# Or [DataTypeCategory] = ##ToString#SoftwareAttribute#")]
		[DevExpress.Xpo.Association("DataTypeList-InterfaceDataTypeList")]
	    [VisibleInDetailView(false)]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.DataType> DataTypeList
        {      
		    get => GetCollection<Module.BusinessObjects.DataType>("DataTypeList"); 
			
        }
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(36)]		
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
		//[Index(37)]		
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
	
       
		//private System.Type _systemtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu hệ thống")]
        [ToolTip("Kiểu hệ thống")]
		//[Index(38)]		
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
	
       
		//private SoftwareObjectType _softwareobjecttype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu đối tượng")]
        [ToolTip("Kiểu đối tượng")]
		//[Index(39)]		
		public SoftwareObjectType SoftwareObjectType
        { 
		    get => GetPropertyValue<SoftwareObjectType>("SoftwareObjectType");                         
			set => SetPropertyValue<SoftwareObjectType>("SoftwareObjectType", value); 
			
        }
		//Tooltip for Object
		public object SoftwareObjectTypeToolTipControllerText(View view)
        {
        //    if (SoftwareObjectType != null) 
		//			return SoftwareObjectType;
            return null;
        }
		//Get Default Value
        public SoftwareObjectType GetDefaultSoftwareObjectType(View view = null)
        { 
			return SoftwareObjectType;
        }
		//Set Default Value

		//Check Not Validate
		protected bool SoftwareObjectTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSoftwareObjectType();
				//if (result != null && SoftwareObjectType != null){
				//	return !SoftwareObjectType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _quantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số thành viên")]
        [ToolTip("Số thành viên")]
		//[Index(40)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Quantity
        { 
		    get => GetPropertyValue<decimal?>("Quantity");                         
			set => SetPropertyValue<decimal?>("Quantity", value); 
			
        }
		//Tooltip for Object
		public object QuantityToolTipControllerText(View view)
        {
        //    if (Quantity != null) 
		//			return Quantity;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultQuantity(View view = null)
        { 
			return Quantity;
        }
		//Set Default Value
		public void SetDefaultQuantity(View view = null)
        {
            //if (Quantity is null){
            //    var result = GetDefaultQuantity(view);
            //    if (result != null && result != Quantity){
			//          Quantity = result;
            //	  }
            //}
        }

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

	
       
		//private int? _linequantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dòng mã đệm")]
        [ToolTip("Dòng mã đệm")]
		//[Index(41)]		
		[ModelDefault("EditMask", "n2")]
		public int? LineQuantity
        { 
		    #region 3932ImportCode 
        get
        {
            if (string.IsNullOrEmpty(SourceCodeBuffer))
                return null;

            return SourceCodeBuffer.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
        }
#endregion 3932ImportCode
			
        }
		//Tooltip for Object
		public object LineQuantityToolTipControllerText(View view)
        {
        //    if (LineQuantity != null) 
		//			return LineQuantity;
            return null;
        }
		//Get Default Value
        public int? GetDefaultLineQuantity(View view = null)
        { 
			return LineQuantity;
        }
		//Set Default Value
		public void SetDefaultLineQuantity(View view = null)
        {
            //if (LineQuantity is null){
            //    var result = GetDefaultLineQuantity(view);
            //    if (result != null && result != LineQuantity){
			//          LineQuantity = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LineQuantityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLineQuantity();
				//if (result != null && LineQuantity != null){
				//	return !LineQuantity.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _createddate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày tạo")]
        [ToolTip("Ngày tạo")]
		//[Index(42)]		
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

	
       
		//private Module.BusinessObjects.Member _member;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(43)]		
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
	
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(45)]		
		public bool InActive
        { 
		    get => GetPropertyValue<bool>("InActive");                         
			set => SetPropertyValue<bool>("InActive", value); 
			
        }
		//Tooltip for Object
		public object InActiveToolTipControllerText(View view)
        {
        //    if (InActive != null) 
		//			return InActive;
            return null;
        }
		//Get Default Value
        public bool GetDefaultInActive(View view = null)
        { 
			return InActive;
        }
		//Set Default Value
		public void SetDefaultInActive(View view = null)
        {
            //if (InActive is null){
            //    var result = GetDefaultInActive(view);
            //    if (result != null && result != InActive){
			//          InActive = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool InActiveIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultInActive();
				//if (result != null && InActive != null){
				//	return !InActive.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _flag;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ")]
        [ToolTip("Cờ")]
		//[Index(46)]		
	    [NonPersistent()]
	    [NotMapped()]
		public bool Flag
        { 
		    get => GetPropertyValue<bool>("Flag");                         
			set => SetPropertyValue<bool>("Flag", value); 
			
        }
		//Tooltip for Object
		public object FlagToolTipControllerText(View view)
        {
        //    if (Flag != null) 
		//			return Flag;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFlag(View view = null)
        { 
			return Flag;
        }
		//Set Default Value
		public void SetDefaultFlag(View view = null)
        {
            //if (Flag is null){
            //    var result = GetDefaultFlag(view);
            //    if (result != null && result != Flag){
			//          Flag = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FlagIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFlag();
				//if (result != null && Flag != null){
				//	return !Flag.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _extract;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trích")]
        [ToolTip("Trích")]
		//[Index(47)]		
		public bool Extract
        { 
		    get => GetPropertyValue<bool>("Extract");                         
			set => SetPropertyValue<bool>("Extract", value); 
			
        }
		//Tooltip for Object
		public object ExtractToolTipControllerText(View view)
        {
        //    if (Extract != null) 
		//			return Extract;
            return null;
        }
		//Get Default Value
        public bool GetDefaultExtract(View view = null)
        { 
			return Extract;
        }
		//Set Default Value
		public void SetDefaultExtract(View view = null)
        {
            //if (Extract is null){
            //    var result = GetDefaultExtract(view);
            //    if (result != null && result != Extract){
			//          Extract = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ExtractIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultExtract();
				//if (result != null && Extract != null){
				//	return !Extract.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultDataTypeCategory(View view = null);
        //SetDefaultSoftwareClassType(View view = null);
        //SetDefaultInheritDataType(View view = null);
        //SetDefaultAccessModifier(View view = null);
        //SetDefaultDataTypeModifier(View view = null);
        //SetDefaultParameter(View view = null);
        //SetDefaultFullName(View view = null);
        //SetDefaultLink(View view = null);
        //SetDefaultGenericType(View view = null);
        //SetDefaultDataTypeT1(View view = null);
        //SetDefaultDataTypeT2(View view = null);
        //SetDefaultSourceCode(View view = null);
        //SetDefaultSoftwareObjectAttribute(View view = null);
        //SetDefaultFieldAttribute(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultSystemType(View view = null);
        //SetDefaultSoftwareObjectType(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultLineQuantity(View view = null);
        //SetDefaultCreatedDate(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultInActive(View view = null);
        //SetDefaultFlag(View view = null);
        //SetDefaultExtract(View view = null);
			
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
            #region 3272ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 3272ImportCode
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
            #region 4501ImportCode
            if (SourceCode != null && !SourceCode.IsDeleted)
{
    Session.Delete(SourceCode);
}
            #endregion 4501ImportCode
  
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
				
                    case nameof(SystemType):
                        OnChangedSystemType(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedSystemType(object oldValue, object newValue)
        {
            #region 3640ImportCode
            if (newValue is null) return;
SetDefaultSoftwareObjectType();            
            #endregion 3640ImportCode
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
			//	SetDefaultDataTypeMemberList();
			//	SetDefaultActionList();
			//	SetDefaultSoftwareAttributeList();
			//	SetDefaultTemplateSoftwareAttributeList();
			//	SetDefaultDefaultSoftwareAttributeList();
			//	SetDefaultInterfaceDataTypeList();
			//	SetDefaultFormatList();
			//	SetDefaultDataTypeProjectFolderList();
			//	SetDefaultSourceCodeBuffer();
			//	SetDefaultNote();
			//	SetDefaultFrameworkDataTypeList();
			//	SetDefaultProjectFileList();
			//	SetDefaultInheritedDataTypeList();
			//	SetDefaultDataTypeList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 3878ImportCode
		public void SetDefaultName(View view = null)
        {
            //Code: 3878            Oid: 51ffea21-4f33-4364-9bde-b822697f3531
            if(String.IsNullOrEmpty(Name)) Name = GetDefaultName();
        }
#endregion 3878ImportCode
#region 3877ImportCode
		public string GetDefaultName(View view = null)
        {
            //Code: 3877            Oid: 5a7c0241-4ec3-4a00-ad01-7ccf8307af3c
            if(Code is not null)
return Code;
else
 return Name;
        }
#endregion 3877ImportCode
#region 3275ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 3275            Oid: 29fbd9f4-56c0-459b-b4f4-f0518681dbb6
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3275ImportCode
#region 3274ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 3274            Oid: a9f276b4-6a6c-4f26-97c8-563abcf5f888
            Updater = GetDefaultUpdater();
        }
#endregion 3274ImportCode
#region 3900ImportCode
		public void SetDefaultCreatedDate(View view = null)
        {
            //Code: 3900            Oid: 2d58b5ef-f06f-49e9-83cb-4d3d49f38769
            if(CreatedDate == null) CreatedDate = GetDefaultCreatedDate();
        }
#endregion 3900ImportCode
#region 4057ImportCode
		public void SetDefaultSourceCode(View view = null)
        {
            //Code: 4057            Oid: 24192b59-ba18-411e-bf05-eaa82dad109c
                        if (SourceCode == null)
            {
                var sourceCode = Session.FindObject<SourceCode>(
                    DevExpress.Data.Filtering.CriteriaOperator.Parse("SystemType = ? And ObjectID = ?", this.GetType(), Oid)
                );
                if (sourceCode != null)
                {
                    SourceCode = sourceCode;
                }
                else
                {
                    SourceCode = new SourceCode(Session);
                    SourceCode.SystemType = this.GetType();
                    SourceCode.ObjectID = Oid;
                }
            }
        }
#endregion 4057ImportCode
#region 3271ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 3271            Oid: 3a4435f5-8422-495a-bba9-c4ec5cfd645d
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 3271ImportCode
#region 3638ImportCode
		public void SetDefaultSoftwareObjectType(View view = null)
        {
            //Code: 3638            Oid: c2657aee-2a02-4a82-baee-d896801ff02a
            if (SystemType == null)
    return;

string typeName = SystemType.Name;

if (Enum.TryParse<SoftwareObjectType>(typeName, out var enumValue))
{
    SoftwareObjectType = enumValue;
}

        }
#endregion 3638ImportCode
#region 3899ImportCode
		public DateTime? GetDefaultCreatedDate(View view = null)
        {
            //Code: 3899            Oid: 4a85da17-00ba-4c08-a5df-c3753049c2ee
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 3899ImportCode
#region 3687ImportCode
		public void SetDefaultFullName(View view = null)
        {
            //Code: 3687            Oid: 3322b1b9-c936-4020-9c03-5e350b1c0d1e
            if(String.IsNullOrEmpty(FullName)) FullName = GetDefaultFullName();
        }
#endregion 3687ImportCode
#region 3273ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 3273            Oid: 3a324499-4ac5-4d94-8cb3-94858403c63c
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 3273ImportCode
#region 3901ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 3901            Oid: 8ae7b0a8-19cb-4d24-99e5-31474784775a
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 3901ImportCode
#region 3902ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 3902            Oid: 23007e01-dc0e-45c2-b571-d5bcdb6bba95
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3902ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
