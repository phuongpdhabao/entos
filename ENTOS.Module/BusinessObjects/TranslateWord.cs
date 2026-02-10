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
    [ModelDefault("Caption", "Từ dịch"), ImageName("TranslateWord")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(Creator))]
 
	[MobileColumnAttribute(Context = "DictionaryWord_TranslateWordList_ListView", TargetItems = nameof(Update)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "TranslateWord_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "TranslateWord_ListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
	[RuleCriteria("RuleLanguage", DefaultContexts.Save, "Language <> DictionaryWord.LanguageOrigin",   "Ngôn ngữ không hợp lệ", SkipNullOrEmptyValues = false)]
[OptimisticLocking(true)]
    public partial class TranslateWord:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public TranslateWord(Session session)
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
	
       
		//private string _context;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngữ cảnh")]
        [ToolTip("Ngữ cảnh")]
		//[Index(2)]		

 		[Size(250)]
		public string Context
        { 
		    get => GetPropertyValue<string>("Context");                         
			set => SetPropertyValue<string>("Context", value); 
			
        }
		//Tooltip for Object
		public object ContextToolTipControllerText(View view)
        {
        //    if (Context != null) 
		//			return Context;
            return null;
        }
		//Get Default Value
        public string GetDefaultContext(View view = null)
        { 
			return Context;
        }
		//Set Default Value
		public void SetDefaultContext(View view = null)
        {
            //if (Context is null){
            //    var result = GetDefaultContext(view);
            //    if (result != null && result != Context){
			//          Context = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ContextIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultContext();
				//if (result != null && Context != null){
				//	return !Context.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(3)]		
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

	
       
		//private Module.BusinessObjects.Member _creator;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Người tạo")]
        [ToolTip("Người tạo")]
		//[Index(4)]		
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
	
       
		//private Module.BusinessObjects.DictionaryWord _dictionaryword;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Từ ngữ")]
        [ToolTip("Từ ngữ")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DictionaryWordCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("DictionaryWord-TranslateWordList")]
	 
		public Module.BusinessObjects.DictionaryWord DictionaryWord
        { 
		    get => GetPropertyValue<Module.BusinessObjects.DictionaryWord>("DictionaryWord");                         
			set => SetPropertyValue<Module.BusinessObjects.DictionaryWord>("DictionaryWord", value); 
			
        }
		//Tooltip for Object
		public object DictionaryWordToolTipControllerText(View view)
        {
        //    if (DictionaryWord != null) 
		//			return DictionaryWord;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.DictionaryWord GetDefaultDictionaryWord(View view = null)
        { 
			return DictionaryWord;
        }
		//Set Default Value
		public void SetDefaultDictionaryWord(View view = null)
        {
            //if (DictionaryWord is null){
            //    var result = GetDefaultDictionaryWord(view);
            //    if (result != null && result != DictionaryWord){
			//          DictionaryWord = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DictionaryWordIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDictionaryWord();
				//if (result != null && DictionaryWord != null){
				//	return !DictionaryWord.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator DictionaryWordCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(DictionaryWord));
            }
        }
	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Order
        { 
		    get => GetPropertyValue<int?>("Order");                         
			set => SetPropertyValue<int?>("Order", value); 
			
        }
		//Tooltip for Object
		public object OrderToolTipControllerText(View view)
        {
        //    if (Order != null) 
		//			return Order;
            return null;
        }
		//Get Default Value
        public int? GetDefaultOrder(View view = null)
        { 
			return Order;
        }
		//Set Default Value
		public void SetDefaultOrder(View view = null)
        {
            //if (Order is null){
            //    var result = GetDefaultOrder(view);
            //    if (result != null && result != Order){
			//          Order = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OrderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrder();
				//if (result != null && Order != null){
				//	return !Order.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0587ImportCode
            base.AfterConstruction();
SetDefaultCreator();
            #endregion 0587ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultLanguage(View view = null);
        //SetDefaultContext(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultCreator(View view = null);
        //SetDefaultDictionaryWord(View view = null);
        //SetDefaultOrder(View view = null);
			
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
            #region 0584ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0584ImportCode
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
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0588ImportCode
		public Module.BusinessObjects.Member GetDefaultCreator(View view = null)
        {
            //Code: 0588            Oid: 8186a28e-a6ae-401d-b6ab-547813dd7fda
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);

        }
#endregion 0588ImportCode
#region 0586ImportCode
		public void SetDefaultCreator(View view = null)
        {
            //Code: 0586            Oid: c16a82ee-9fd1-49c0-99da-337f463c410a
            if(Creator == null) Creator = GetDefaultCreator();

        }
#endregion 0586ImportCode
#region 0583ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0583            Oid: bd799cc0-eeb3-45e7-b01b-f81892d76ec7
            Update = GetDefaultUpdate();
        }
#endregion 0583ImportCode
#region 0585ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0585            Oid: 753d545b-e8e9-4d2e-87f5-3ec2a4258f3e
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0585ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
