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
    [ModelDefault("Caption", "Phiên âm"), ImageName("SpellingWord")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Language)+ "," + nameof(Spelling))]
 
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class SpellingWord:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public SpellingWord(Session session)
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
				if (LanguageSpellingList.IsLoaded)
                {
                    if (LanguageSpellingList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(LanguageSpellingList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(LanguageSpellingList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.LanguageSpelling>(CriteriaOperator.Parse("[SpellingWord.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool languagespellinglist = Session.Query<Module.BusinessObjects.LanguageSpelling>().Where(x => x.SpellingWord.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(LanguageSpellingList), languagespellinglist);
                        if (languagespellinglist)
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

	
       
		//private Module.BusinessObjects.Language _language;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngôn ngữ")]
        [ToolTip("Ngôn ngữ")]
		//[Index(1)]		
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
		//Set Default Value

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
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngữ âm")]
		//[Index(2)]
		[DevExpress.Xpo.Association("SpellingWord-LanguageSpellingList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.LanguageSpelling> LanguageSpellingList
        {      
		    get => GetCollection<Module.BusinessObjects.LanguageSpelling>("LanguageSpellingList"); 
			
        }
       
		//private Module.BusinessObjects.Language _spellinglanguage;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngữ phiên")]
        [ToolTip("Ngữ phiên")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SpellingLanguageCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [NotMapped()]
	    [NonPersistent()]
		public Module.BusinessObjects.Language SpellingLanguage
        { 
		    #region 3323ImportCode 
get; set;
#endregion 3323ImportCode
			
        }
		//Tooltip for Object
		public object SpellingLanguageToolTipControllerText(View view)
        {
        //    if (SpellingLanguage != null) 
		//			return SpellingLanguage;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Language GetDefaultSpellingLanguage(View view = null)
        { 
			return SpellingLanguage;
        }
		//Set Default Value
		public void SetDefaultSpellingLanguage(View view = null)
        {
            //if (SpellingLanguage is null){
            //    var result = GetDefaultSpellingLanguage(view);
            //    if (result != null && result != SpellingLanguage){
			//          SpellingLanguage = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpellingLanguageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpellingLanguage();
				//if (result != null && SpellingLanguage != null){
				//	return !SpellingLanguage.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SpellingLanguageCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SpellingLanguage));
            }
        }
	
       
		//private string _spelling;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Phiên âm")]
        [ToolTip("Phiên âm")]
		//[Index(4)]		

 		[Size(250)]
	    [NotMapped()]
	    [NonPersistent()]
		public string Spelling
        { 
		    #region 3324ImportCode 
get; set;
#endregion 3324ImportCode
			
        }
		//Tooltip for Object
		public object SpellingToolTipControllerText(View view)
        {
        //    if (Spelling != null) 
		//			return Spelling;
            return null;
        }
		//Get Default Value
        public string GetDefaultSpelling(View view = null)
        { 
			return Spelling;
        }
		//Set Default Value

		//Check Not Validate
		protected bool SpellingIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpelling();
				//if (result != null && Spelling != null){
				//	return !Spelling.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 3322ImportCode
            base.AfterConstruction();
SetDefaultLanguage();
            #endregion 3322ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultLanguage(View view = null);
        //SetDefaultSpellingLanguage(View view = null);
        //SetDefaultSpelling(View view = null);
			
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
			//	SetDefaultLanguageSpellingList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 3320ImportCode
		public Module.BusinessObjects.Language GetDefaultLanguage(View view = null)
        {
            //Code: 3320            Oid: 6ccbc609-6fd3-436b-9a1d-1023aa31d478
                    return Session.FindObject<Language>(
            DevExpress.Data.Filtering.CriteriaOperator.Parse("LocaleCode = ?", "en-US")
        );
        }
#endregion 3320ImportCode
#region 3321ImportCode
		public void SetDefaultLanguage(View view = null)
        {
            //Code: 3321            Oid: 22830f74-9832-4ea1-b1e6-a16da751e2ce
            Language = GetDefaultLanguage();
        }
#endregion 3321ImportCode
#region 3325ImportCode
		public void SetDefaultSpelling(View view = null)
        {
            //Code: 3325            Oid: 047102a4-3f36-4049-9276-2d2d4885afbd
                if (SpellingLanguage == null || LanguageSpellingList == null)
        return;

    // Tìm LanguageSpelling phù hợp với SpellingLanguage
    var match = LanguageSpellingList.FirstOrDefault(
        ls => ls.Language != null && ls.Language.Oid == SpellingLanguage.Oid
    );

    if (match != null)
    {
        Spelling = match.Name;
    }
        }
#endregion 3325ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
