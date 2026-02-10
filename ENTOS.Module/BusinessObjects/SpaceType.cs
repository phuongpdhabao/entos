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
    [ModelDefault("Caption", "Loại địa bàn"), ImageName("SpaceType")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "SpaceType_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "SpaceType_ListView", TargetItems = nameof(Country)+ "," + nameof(Level)+ "," + nameof(Name))]
	[DefaultProperty("Name")]
 
//[OptimisticLocking(false)]
    public partial class SpaceType: DevExpress.Persistent.BaseImpl.BaseObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public SpaceType(Session session)
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

               

		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(0)]		

 		[Size(10)]
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
		[RuleRequiredField("RequiredSpaceTypeName", DefaultContexts.Save)]
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

	
       
		//private string _englishname;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tiếng Anh")]
        [ToolTip("Tiếng Anh")]
		//[Index(2)]		

 		[Size(100)]
		public string EnglishName
        { 
		    get => GetPropertyValue<string>("EnglishName");                         
			set => SetPropertyValue<string>("EnglishName", value); 
			
        }
		//Tooltip for Object
		public object EnglishNameToolTipControllerText(View view)
        {
        //    if (EnglishName != null) 
		//			return EnglishName;
            return null;
        }
		//Get Default Value
        public string GetDefaultEnglishName(View view = null)
        { 
			return EnglishName;
        }
		//Set Default Value
		public void SetDefaultEnglishName(View view = null)
        {
            //if (EnglishName is null){
            //    var result = GetDefaultEnglishName(view);
            //    if (result != null && result != EnglishName){
			//          EnglishName = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EnglishNameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEnglishName();
				//if (result != null && EnglishName != null){
				//	return !EnglishName.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _originalname;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bản ngữ")]
        [ToolTip("Bản ngữ")]
		//[Index(3)]		

 		[Size(100)]
		public string OriginalName
        { 
		    get => GetPropertyValue<string>("OriginalName");                         
			set => SetPropertyValue<string>("OriginalName", value); 
			
        }
		//Tooltip for Object
		public object OriginalNameToolTipControllerText(View view)
        {
        //    if (OriginalName != null) 
		//			return OriginalName;
            return null;
        }
		//Get Default Value
        public string GetDefaultOriginalName(View view = null)
        { 
			return OriginalName;
        }
		//Set Default Value
		public void SetDefaultOriginalName(View view = null)
        {
            //if (OriginalName is null){
            //    var result = GetDefaultOriginalName(view);
            //    if (result != null && result != OriginalName){
			//          OriginalName = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OriginalNameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOriginalName();
				//if (result != null && OriginalName != null){
				//	return !OriginalName.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Country _country;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quốc gia")]
        [ToolTip("Quốc gia")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CountryCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Country Country
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Country>("Country");                         
			set => SetPropertyValue<Module.BusinessObjects.Country>("Country", value); 
			
        }
		//Tooltip for Object
		public object CountryToolTipControllerText(View view)
        {
        //    if (Country != null) 
		//			return Country;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Country GetDefaultCountry(View view = null)
        { 
			return Country;
        }
		//Set Default Value
		public void SetDefaultCountry(View view = null)
        {
            //if (Country is null){
            //    var result = GetDefaultCountry(view);
            //    if (result != null && result != Country){
			//          Country = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CountryIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCountry();
				//if (result != null && Country != null){
				//	return !Country.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CountryCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Country));
            }
        }
	
       
		//private int _level;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cấp độ")]
        [ToolTip("Cấp độ")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int Level
        { 
		    get => GetPropertyValue<int>("Level");                         
			set => SetPropertyValue<int>("Level", value); 
			
        }
		//Tooltip for Object
		public object LevelToolTipControllerText(View view)
        {
        //    if (Level != null) 
		//			return Level;
            return null;
        }
		//Get Default Value
        public int GetDefaultLevel(View view = null)
        { 
			return Level;
        }
		//Set Default Value
		public void SetDefaultLevel(View view = null)
        {
            //if (Level is null){
            //    var result = GetDefaultLevel(view);
            //    if (result != null && result != Level){
			//          Level = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LevelIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLevel();
				//if (result != null && Level != null){
				//	return !Level.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int _secondarylevel;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thứ cấp")]
        [ToolTip("Thứ cấp")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int SecondaryLevel
        { 
		    get => GetPropertyValue<int>("SecondaryLevel");                         
			set => SetPropertyValue<int>("SecondaryLevel", value); 
			
        }
		//Tooltip for Object
		public object SecondaryLevelToolTipControllerText(View view)
        {
        //    if (SecondaryLevel != null) 
		//			return SecondaryLevel;
            return null;
        }
		//Get Default Value
        public int GetDefaultSecondaryLevel(View view = null)
        { 
			return SecondaryLevel;
        }
		//Set Default Value
		public void SetDefaultSecondaryLevel(View view = null)
        {
            //if (SecondaryLevel is null){
            //    var result = GetDefaultSecondaryLevel(view);
            //    if (result != null && result != SecondaryLevel){
			//          SecondaryLevel = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SecondaryLevelIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSecondaryLevel();
				//if (result != null && SecondaryLevel != null){
				//	return !SecondaryLevel.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private LengthUnit _lengthunit;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đơn vị")]
        [ToolTip("Đơn vị")]
		//[Index(7)]		
		public LengthUnit LengthUnit
        { 
		    get => GetPropertyValue<LengthUnit>("LengthUnit");                         
			set => SetPropertyValue<LengthUnit>("LengthUnit", value); 
			
        }
		//Tooltip for Object
		public object LengthUnitToolTipControllerText(View view)
        {
        //    if (LengthUnit != null) 
		//			return LengthUnit;
            return null;
        }
		//Get Default Value
        public LengthUnit GetDefaultLengthUnit(View view = null)
        { 
			return LengthUnit;
        }
		//Set Default Value
		public void SetDefaultLengthUnit(View view = null)
        {
            //if (LengthUnit is null){
            //    var result = GetDefaultLengthUnit(view);
            //    if (result != null && result != LengthUnit){
			//          LengthUnit = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LengthUnitIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLengthUnit();
				//if (result != null && LengthUnit != null){
				//	return !LengthUnit.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
 
            base.AfterConstruction();
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultEnglishName(View view = null);
        //SetDefaultOriginalName(View view = null);
        //SetDefaultCountry(View view = null);
        //SetDefaultLevel(View view = null);
        //SetDefaultSecondaryLevel(View view = null);
        //SetDefaultLengthUnit(View view = null);
			
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
