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
	[NavigationItem("Document")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Giọng đọc"), ImageName("Voice")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Voice:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Voice(Session session)
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
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
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

	
       
		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(1)]		

 		[Size(30)]
		[RuleRequiredField("RequiredVoiceCode", DefaultContexts.Save)]
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

	
       
		//private Gender _gender;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giới tính")]
        [ToolTip("Giới tính")]
		//[Index(2)]		
		public Gender Gender
        { 
		    get => GetPropertyValue<Gender>("Gender");                         
			set => SetPropertyValue<Gender>("Gender", value); 
			
        }
		//Tooltip for Object
		public object GenderToolTipControllerText(View view)
        {
        //    if (Gender != null) 
		//			return Gender;
            return null;
        }
		//Get Default Value
        public Gender GetDefaultGender(View view = null)
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

	
       
		//private DevExpress.Persistent.BaseImpl.FileData _template;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mẫu")]
        [ToolTip("Mẫu")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TemplateCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public DevExpress.Persistent.BaseImpl.FileData Template
        { 
		    get => GetPropertyValue<DevExpress.Persistent.BaseImpl.FileData>("Template");                         
			set => SetPropertyValue<DevExpress.Persistent.BaseImpl.FileData>("Template", value); 
			
        }
		//Tooltip for Object
		public object TemplateToolTipControllerText(View view)
        {
        //    if (Template != null) 
		//			return Template;
            return null;
        }
		//Get Default Value
        public DevExpress.Persistent.BaseImpl.FileData GetDefaultTemplate(View view = null)
        { 
			return Template;
        }
		//Set Default Value
		public void SetDefaultTemplate(View view = null)
        {
            //if (Template is null){
            //    var result = GetDefaultTemplate(view);
            //    if (result != null && result != Template){
			//          Template = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TemplateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTemplate();
				//if (result != null && Template != null){
				//	return !Template.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TemplateCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Template));
            }
        }
	
       
		//private Module.BusinessObjects.Language _language;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngôn ngữ")]
        [ToolTip("Ngôn ngữ")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LanguageCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Language Language
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Language>("Language");                         
			set => SetPropertyValue<Module.BusinessObjects.Language>("Language", value); 
			
        }
		//Tooltip for Object
		public object LanguageToolTipControllerText(View view)
        {
        //    if (Language != null) 
		//			return Language;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Language GetDefaultLanguage(View view = null)
        { 
			return Language;
        }
		//Set Default Value
		public void SetDefaultLanguage(View view = null)
        {
            //if (Language is null){
            //    var result = GetDefaultLanguage(view);
            //    if (result != null && result != Language){
			//          Language = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LanguageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLanguage();
				//if (result != null && Language != null){
				//	return !Language.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LanguageCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Language));
            }
        }
	
       
		//private string _note;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(5)]		

 		[Size(100)]
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

	
       
		//private decimal _vowelspeed;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tốc độ")]
        [ToolTip("Tốc độ")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "{0:n1}")]
		[ModelDefault("EditMask", "n2")]
		public decimal VowelSpeed
        { 
		    get => GetPropertyValue<decimal>("VowelSpeed");                         
			set => SetPropertyValue<decimal>("VowelSpeed", value); 
			
        }
		//Tooltip for Object
		public object VowelSpeedToolTipControllerText(View view)
        {
        //    if (VowelSpeed != null) 
		//			return VowelSpeed;
            return null;
        }
		//Get Default Value
        public decimal GetDefaultVowelSpeed(View view = null)
        { 
			return VowelSpeed;
        }
		//Set Default Value
		public void SetDefaultVowelSpeed(View view = null)
        {
            //if (VowelSpeed is null){
            //    var result = GetDefaultVowelSpeed(view);
            //    if (result != null && result != VowelSpeed){
			//          VowelSpeed = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool VowelSpeedIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultVowelSpeed();
				//if (result != null && VowelSpeed != null){
				//	return !VowelSpeed.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _speelingminutes;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Âm phút")]
        [ToolTip("Âm phút")]
		//[Index(7)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? SpeelingMinutes
        { 
		    get => GetPropertyValue<decimal?>("SpeelingMinutes");                         
			set => SetPropertyValue<decimal?>("SpeelingMinutes", value); 
			
        }
		//Tooltip for Object
		public object SpeelingMinutesToolTipControllerText(View view)
        {
        //    if (SpeelingMinutes != null) 
		//			return SpeelingMinutes;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultSpeelingMinutes(View view = null)
        { 
			return SpeelingMinutes;
        }
		//Set Default Value
		public void SetDefaultSpeelingMinutes(View view = null)
        {
            //if (SpeelingMinutes is null){
            //    var result = GetDefaultSpeelingMinutes(view);
            //    if (result != null && result != SpeelingMinutes){
			//          SpeelingMinutes = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpeelingMinutesIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpeelingMinutes();
				//if (result != null && SpeelingMinutes != null){
				//	return !SpeelingMinutes.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.DataService _dataservice;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Dịch vụ Dữ liệu")]
        [ToolTip("Dịch vụ Dữ liệu")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DataServiceCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("DataService-VoiceList")]
	 
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
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0572ImportCode
            base.AfterConstruction();
VowelSpeed = 1;
            #endregion 0572ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultGender(View view = null);
        //SetDefaultTemplate(View view = null);
        //SetDefaultLanguage(View view = null);
        //SetDefaultNote(View view = null);
        //SetDefaultVowelSpeed(View view = null);
        //SetDefaultSpeelingMinutes(View view = null);
        //SetDefaultDataService(View view = null);
			
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
