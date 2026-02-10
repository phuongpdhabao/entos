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
    [ModelDefault("Caption", "Ngôn ngữ"), ImageName("Language")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "Language_LookupListView", TargetItems = "Country.Flag"+ "," + nameof(Name)+ "," + nameof(Code))]
	[MobileColumnAttribute(Context = "Video_LanguageList_ListView", TargetItems = nameof(Name)+ "," + "Country.Flag"+ "," + nameof(Code))]
	[MobileColumnAttribute(Context = "Language_ListView", TargetItems = nameof(Name)+ "," + nameof(Code)+ "," + "Country.Flag")]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Language:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Language(Session session)
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
		[RuleUniqueValue("UniqueLanguageName", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredLanguageName", DefaultContexts.Save)]
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
		//[Index(1)]		

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

	
       
		//private string _originname;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bản ngữ")]
        [ToolTip("Bản ngữ")]
		//[Index(2)]		

 		[Size(100)]
		public string OriginName
        { 
		    get => GetPropertyValue<string>("OriginName");                         
			set => SetPropertyValue<string>("OriginName", value); 
			
        }
		//Tooltip for Object
		public object OriginNameToolTipControllerText(View view)
        {
        //    if (OriginName != null) 
		//			return OriginName;
            return null;
        }
		//Get Default Value
        public string GetDefaultOriginName(View view = null)
        { 
			return OriginName;
        }
		//Set Default Value
		public void SetDefaultOriginName(View view = null)
        {
            //if (OriginName is null){
            //    var result = GetDefaultOriginName(view);
            //    if (result != null && result != OriginName){
			//          OriginName = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OriginNameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOriginName();
				//if (result != null && OriginName != null){
				//	return !OriginName.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã ISO")]
        [ToolTip("Mã ISO")]
		//[Index(3)]		

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

	
       
		//private string _localecode;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã vực")]
        [ToolTip("Mã vực")]
		//[Index(4)]		

 		[Size(10)]
		[RuleUniqueValue("UniqueLanguageLocaleCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		public string LocaleCode
        { 
		    get => GetPropertyValue<string>("LocaleCode");                         
			set => SetPropertyValue<string>("LocaleCode", value); 
			
        }
		//Tooltip for Object
		public object LocaleCodeToolTipControllerText(View view)
        {
        //    if (LocaleCode != null) 
		//			return LocaleCode;
            return null;
        }
		//Get Default Value
        public string GetDefaultLocaleCode(View view = null)
        { 
			return LocaleCode;
        }
		//Set Default Value
		public void SetDefaultLocaleCode(View view = null)
        {
            //if (LocaleCode is null){
            //    var result = GetDefaultLocaleCode(view);
            //    if (result != null && result != LocaleCode){
			//          LocaleCode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LocaleCodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLocaleCode();
				//if (result != null && LocaleCode != null){
				//	return !LocaleCode.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _speaker;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số người")]
        [ToolTip("Số người")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Speaker
        { 
		    get => GetPropertyValue<int?>("Speaker");                         
			set => SetPropertyValue<int?>("Speaker", value); 
			
        }
		//Tooltip for Object
		public object SpeakerToolTipControllerText(View view)
        {
        //    if (Speaker != null) 
		//			return Speaker;
            return null;
        }
		//Get Default Value
        public int? GetDefaultSpeaker(View view = null)
        { 
			return Speaker;
        }
		//Set Default Value
		public void SetDefaultSpeaker(View view = null)
        {
            //if (Speaker is null){
            //    var result = GetDefaultSpeaker(view);
            //    if (result != null && result != Speaker){
			//          Speaker = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpeakerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpeaker();
				//if (result != null && Speaker != null){
				//	return !Speaker.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Country _country;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quốc gia")]
        [ToolTip("Quốc gia")]
		//[Index(6)]		
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
	
       
		//private string _character;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ký tự")]
        [ToolTip("Ký tự")]
		//[Index(7)]		

 		[Size(250)]
		public string Character
        { 
		    get => GetPropertyValue<string>("Character");                         
			set => SetPropertyValue<string>("Character", value); 
			
        }
		//Tooltip for Object
		public object CharacterToolTipControllerText(View view)
        {
        //    if (Character != null) 
		//			return Character;
            return null;
        }
		//Get Default Value
        public string GetDefaultCharacter(View view = null)
        { 
			return Character;
        }
		//Set Default Value
		public void SetDefaultCharacter(View view = null)
        {
            //if (Character is null){
            //    var result = GetDefaultCharacter(view);
            //    if (result != null && result != Character){
			//          Character = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CharacterIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCharacter();
				//if (result != null && Character != null){
				//	return !Character.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _vowel;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nguyên âm")]
        [ToolTip("Nguyên âm")]
		//[Index(8)]		

 		[Size(100)]
		public string Vowel
        { 
		    get => GetPropertyValue<string>("Vowel");                         
			set => SetPropertyValue<string>("Vowel", value); 
			
        }
		//Tooltip for Object
		public object VowelToolTipControllerText(View view)
        {
        //    if (Vowel != null) 
		//			return Vowel;
            return null;
        }
		//Get Default Value
        public string GetDefaultVowel(View view = null)
        { 
			return Vowel;
        }
		//Set Default Value
		public void SetDefaultVowel(View view = null)
        {
            //if (Vowel is null){
            //    var result = GetDefaultVowel(view);
            //    if (result != null && result != Vowel){
			//          Vowel = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool VowelIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultVowel();
				//if (result != null && Vowel != null){
				//	return !Vowel.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _repeatcharacter;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ký tự lặp")]
        [ToolTip("Ký tự lặp")]
		//[Index(9)]		

 		[Size(250)]
		public string RepeatCharacter
        { 
		    get => GetPropertyValue<string>("RepeatCharacter");                         
			set => SetPropertyValue<string>("RepeatCharacter", value); 
			
        }
		//Tooltip for Object
		public object RepeatCharacterToolTipControllerText(View view)
        {
        //    if (RepeatCharacter != null) 
		//			return RepeatCharacter;
            return null;
        }
		//Get Default Value
        public string GetDefaultRepeatCharacter(View view = null)
        { 
			return RepeatCharacter;
        }
		//Set Default Value
		public void SetDefaultRepeatCharacter(View view = null)
        {
            //if (RepeatCharacter is null){
            //    var result = GetDefaultRepeatCharacter(view);
            //    if (result != null && result != RepeatCharacter){
			//          RepeatCharacter = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RepeatCharacterIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRepeatCharacter();
				//if (result != null && RepeatCharacter != null){
				//	return !RepeatCharacter.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _notupcase;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Từ không hoa")]
        [ToolTip("Từ không hoa")]
		//[Index(10)]		

 		[Size(250)]
		public string NotUpCase
        { 
		    get => GetPropertyValue<string>("NotUpCase");                         
			set => SetPropertyValue<string>("NotUpCase", value); 
			
        }
		//Tooltip for Object
		public object NotUpCaseToolTipControllerText(View view)
        {
        //    if (NotUpCase != null) 
		//			return NotUpCase;
            return null;
        }
		//Get Default Value
        public string GetDefaultNotUpCase(View view = null)
        { 
			return NotUpCase;
        }
		//Set Default Value
		public void SetDefaultNotUpCase(View view = null)
        {
            //if (NotUpCase is null){
            //    var result = GetDefaultNotUpCase(view);
            //    if (result != null && result != NotUpCase){
			//          NotUpCase = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NotUpCaseIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNotUpCase();
				//if (result != null && NotUpCase != null){
				//	return !NotUpCase.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	

	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tư liệu")]
		//[Index(11)]
		[DataSourceCriteria("Not LanguageList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("LanguageList-VideoList")]
	    [VisibleInDetailView(false)]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Video> VideoList
        {      
		    get => GetCollection<Module.BusinessObjects.Video>("VideoList"); 
			
        }
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultEnglishName(View view = null);
        //SetDefaultOriginName(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultLocaleCode(View view = null);
        //SetDefaultSpeaker(View view = null);
        //SetDefaultCountry(View view = null);
        //SetDefaultCharacter(View view = null);
        //SetDefaultVowel(View view = null);
        //SetDefaultRepeatCharacter(View view = null);
        //SetDefaultNotUpCase(View view = null);
			
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
			//	SetDefaultVideoList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1425ImportCode
		public string GetLanguageCodeIso6392()
        {
            //Code: 1425            Oid: 29d3264e-edef-4bec-948c-b0e924dd80af
            var cultureInfo = new System.Globalization.CultureInfo(Code);
return cultureInfo.ThreeLetterISOLanguageName;
        }
#endregion 1425ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
