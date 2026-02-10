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
    [ModelDefault("Caption", "Từ ngữ"), ImageName("DictionaryWord")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
    [ShowToolTipAttribute(TargetItems = nameof(Name))]
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Creator)+ "," + nameof(Update))]
 
	[MobileColumnAttribute(Context = "DictionaryWord_ListView", TargetItems = nameof(Update)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "DictionaryWord_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Dictionary_DictionaryWordList_ListView", TargetItems = nameof(Update)+ "," + nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class DictionaryWord:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public DictionaryWord(Session session)
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
				if (TranslateWordList.IsLoaded)
                {
                    if (TranslateWordList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(TranslateWordList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(TranslateWordList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.TranslateWord>(CriteriaOperator.Parse("[DictionaryWord.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool translatewordlist = Session.Query<Module.BusinessObjects.TranslateWord>().Where(x => x.DictionaryWord.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(TranslateWordList), translatewordlist);
                        if (translatewordlist)
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

 		[Size(150)]
		public string Name
        { 
		    get => GetPropertyValue<string>("Name");                         
			set => SetPropertyValue<string>("Name", value); 
			
        }
		//Tooltip for Object
		public object NameToolTipControllerText(View view)
        {
            #region 0941ImportCode 
return Sentence;
#endregion 0941ImportCode
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

	
       
		//private string _translate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Dịch")]
        [ToolTip("Dịch")]
		//[Index(1)]		

 		[Size(150)]
		public string Translate
        { 
		    get => GetPropertyValue<string>("Translate");                         
			set => SetPropertyValue<string>("Translate", value); 
			
        }
		//Tooltip for Object
		public object TranslateToolTipControllerText(View view)
        {
        //    if (Translate != null) 
		//			return Translate;
            return null;
        }
		//Get Default Value
        public string GetDefaultTranslate(View view = null)
        { 
			return Translate;
        }
		//Set Default Value
		public void SetDefaultTranslate(View view = null)
        {
            //if (Translate is null){
            //    var result = GetDefaultTranslate(view);
            //    if (result != null && result != Translate){
			//          Translate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TranslateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTranslate();
				//if (result != null && Translate != null){
				//	return !Translate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _sentence;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngữ cảnh")]
        [ToolTip("Ngữ cảnh")]
		//[Index(2)]		

 		[Size(250)]
		public string Sentence
        { 
		    get => GetPropertyValue<string>("Sentence");                         
			set => SetPropertyValue<string>("Sentence", value); 
			
        }
		//Tooltip for Object
		public object SentenceToolTipControllerText(View view)
        {
        //    if (Sentence != null) 
		//			return Sentence;
            return null;
        }
		//Get Default Value
        public string GetDefaultSentence(View view = null)
        { 
			return Sentence;
        }
		//Set Default Value
		public void SetDefaultSentence(View view = null)
        {
            //if (Sentence is null){
            //    var result = GetDefaultSentence(view);
            //    if (result != null && result != Sentence){
			//          Sentence = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SentenceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSentence();
				//if (result != null && Sentence != null){
				//	return !Sentence.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Language _languageorigin;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngữ gốc")]
        [ToolTip("Ngữ gốc")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LanguageOriginCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Language LanguageOrigin
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Language>("LanguageOrigin");                         
			set => SetPropertyValue<Module.BusinessObjects.Language>("LanguageOrigin", value); 
			
        }
		//Tooltip for Object
		public object LanguageOriginToolTipControllerText(View view)
        {
        //    if (LanguageOrigin != null) 
		//			return LanguageOrigin;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Language GetDefaultLanguageOrigin(View view = null)
        { 
			return LanguageOrigin;
        }
		//Set Default Value
		public void SetDefaultLanguageOrigin(View view = null)
        {
            //if (LanguageOrigin is null){
            //    var result = GetDefaultLanguageOrigin(view);
            //    if (result != null && result != LanguageOrigin){
			//          LanguageOrigin = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LanguageOriginIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLanguageOrigin();
				//if (result != null && LanguageOrigin != null){
				//	return !LanguageOrigin.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LanguageOriginCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(LanguageOrigin));
            }
        }
	
       
		//private Module.BusinessObjects.Language _languagetranslate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngữ dịch")]
        [ToolTip("Ngữ dịch")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LanguageTranslateCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Language LanguageTranslate
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Language>("LanguageTranslate");                         
			set => SetPropertyValue<Module.BusinessObjects.Language>("LanguageTranslate", value); 
			
        }
		//Tooltip for Object
		public object LanguageTranslateToolTipControllerText(View view)
        {
        //    if (LanguageTranslate != null) 
		//			return LanguageTranslate;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Language GetDefaultLanguageTranslate(View view = null)
        { 
			return LanguageTranslate;
        }
		//Set Default Value
		public void SetDefaultLanguageTranslate(View view = null)
        {
            //if (LanguageTranslate is null){
            //    var result = GetDefaultLanguageTranslate(view);
            //    if (result != null && result != LanguageTranslate){
			//          LanguageTranslate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LanguageTranslateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLanguageTranslate();
				//if (result != null && LanguageTranslate != null){
				//	return !LanguageTranslate.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LanguageTranslateCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(LanguageTranslate));
            }
        }
	
       
		//private WordType _wordtype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(5)]		
		public WordType WordType
        { 
		    get => GetPropertyValue<WordType>("WordType");                         
			set => SetPropertyValue<WordType>("WordType", value); 
			
        }
		//Tooltip for Object
		public object WordTypeToolTipControllerText(View view)
        {
        //    if (WordType != null) 
		//			return WordType;
            return null;
        }
		//Get Default Value
        public WordType GetDefaultWordType(View view = null)
        { 
			return WordType;
        }
		//Set Default Value
		public void SetDefaultWordType(View view = null)
        {
            //if (WordType is null){
            //    var result = GetDefaultWordType(view);
            //    if (result != null && result != WordType){
			//          WordType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WordTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWordType();
				//if (result != null && WordType != null){
				//	return !WordType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dịch ngữ")]
		//[Index(6)]
		[DevExpress.Xpo.Association("DictionaryWord-TranslateWordList")]
	    [RuleCombinationOfPropertiesIsUnique("UniqueRule.TranslateWord", DefaultContexts.Save, "Language")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.TranslateWord> TranslateWordList
        {      
		    get => GetCollection<Module.BusinessObjects.TranslateWord>("TranslateWordList"); 
			
        }
       
		//private Module.BusinessObjects.Dictionary _dictionary;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Từ điển")]
        [ToolTip("Từ điển")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DictionaryCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Dictionary-DictionaryWordList")]
	 
		public Module.BusinessObjects.Dictionary Dictionary
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Dictionary>("Dictionary");                         
			set => SetPropertyValue<Module.BusinessObjects.Dictionary>("Dictionary", value); 
			
        }
		//Tooltip for Object
		public object DictionaryToolTipControllerText(View view)
        {
        //    if (Dictionary != null) 
		//			return Dictionary;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Dictionary GetDefaultDictionary(View view = null)
        { 
			return Dictionary;
        }
		//Set Default Value
		public void SetDefaultDictionary(View view = null)
        {
            //if (Dictionary is null){
            //    var result = GetDefaultDictionary(view);
            //    if (result != null && result != Dictionary){
			//          Dictionary = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DictionaryIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDictionary();
				//if (result != null && Dictionary != null){
				//	return !Dictionary.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator DictionaryCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Dictionary));
            }
        }
	
       
		//private Module.BusinessObjects.Member _creator;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Người tạo")]
        [ToolTip("Người tạo")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CreatorCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Member Creator
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Creator");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Creator", value); 
			
        }
		//Tooltip for Object
		public object CreatorToolTipControllerText(View view)
        {
        //    if (Creator != null) 
		//			return Creator;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool CreatorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCreator();
				//if (result != null && Creator != null){
				//	return !Creator.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CreatorCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Creator));
            }
        }
	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
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

	
       
		//private string _nosignword;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Không dấu")]
        [ToolTip("Không dấu")]
		//[Index(10)]		

 		[Size(150)]
		public string NoSignWord
        { 
		    get => GetPropertyValue<string>("NoSignWord");                         
			set => SetPropertyValue<string>("NoSignWord", value); 
			
        }
		//Tooltip for Object
		public object NoSignWordToolTipControllerText(View view)
        {
        //    if (NoSignWord != null) 
		//			return NoSignWord;
            return null;
        }
		//Get Default Value
        public string GetDefaultNoSignWord(View view = null)
        { 
			return NoSignWord;
        }
		//Set Default Value
		public void SetDefaultNoSignWord(View view = null)
        {
            //if (NoSignWord is null){
            //    var result = GetDefaultNoSignWord(view);
            //    if (result != null && result != NoSignWord){
			//          NoSignWord = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NoSignWordIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNoSignWord();
				//if (result != null && NoSignWord != null){
				//	return !NoSignWord.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0581ImportCode
            base.AfterConstruction();
SetDefaultCreator();
            #endregion 0581ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultTranslate(View view = null);
        //SetDefaultSentence(View view = null);
        //SetDefaultLanguageOrigin(View view = null);
        //SetDefaultLanguageTranslate(View view = null);
        //SetDefaultWordType(View view = null);
        //SetDefaultDictionary(View view = null);
        //SetDefaultCreator(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultNoSignWord(View view = null);
			
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
            #region 0545ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0545ImportCode
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
			//	SetDefaultTranslateWordList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0580ImportCode
		public void SetDefaultCreator(View view = null)
        {
            //Code: 0580            Oid: 9507e261-3654-426f-bea8-1326ebbaf825
            if(Creator == null) Creator = GetDefaultCreator();

        }
#endregion 0580ImportCode
#region 0582ImportCode
		public Module.BusinessObjects.Member GetDefaultCreator(View view = null)
        {
            //Code: 0582            Oid: 74dc958f-022f-491c-909c-3d87457735a1
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);

        }
#endregion 0582ImportCode
#region 0546ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0546            Oid: 03e7f65c-c327-45cd-acbb-0a4fa1333023
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0546ImportCode
#region 0544ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0544            Oid: 5791ab39-beff-4847-8e66-2ff0497649b8
            Update = GetDefaultUpdate();
        }
#endregion 0544ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
