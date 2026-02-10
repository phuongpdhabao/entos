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
    [ModelDefault("Caption", "Từ vựng"), ImageName("Word")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Length))]
 
	[MobileColumnAttribute(Context = "Word_ListView", TargetItems = nameof(Name)+ "," + nameof(Language))]
	[MobileColumnAttribute(Context = "Word_LookupListView", TargetItems = nameof(Name)+ "," + nameof(NoSignWord)+ "," + nameof(Language))]
 
[OptimisticLocking(true)]
    public partial class Word:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Word(Session session)
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

	
       
		//private string _nosignword;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Không dấu")]
        [ToolTip("Không dấu")]
		//[Index(1)]		

 		[Size(100)]
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

	
       
		//private Module.BusinessObjects.Language _language;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ngôn ngữ")]
        [ToolTip("Ngôn ngữ")]
		//[Index(2)]		
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
	
       
		//private int? _wordquantity;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số từ")]
        [ToolTip("Số từ")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? WordQuantity
        { 
		    get => GetPropertyValue<int?>("WordQuantity");                         
			set => SetPropertyValue<int?>("WordQuantity", value); 
			
        }
		//Tooltip for Object
		public object WordQuantityToolTipControllerText(View view)
        {
        //    if (WordQuantity != null) 
		//			return WordQuantity;
            return null;
        }
		//Get Default Value
        public int? GetDefaultWordQuantity(View view = null)
        { 
			return WordQuantity;
        }
		//Set Default Value
		public void SetDefaultWordQuantity(View view = null)
        {
            //if (WordQuantity is null){
            //    var result = GetDefaultWordQuantity(view);
            //    if (result != null && result != WordQuantity){
			//          WordQuantity = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WordQuantityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWordQuantity();
				//if (result != null && WordQuantity != null){
				//	return !WordQuantity.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _length;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ký tự")]
        [ToolTip("Ký tự")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Length
        { 
		    get => GetPropertyValue<int?>("Length");                         
			set => SetPropertyValue<int?>("Length", value); 
			
        }
		//Tooltip for Object
		public object LengthToolTipControllerText(View view)
        {
        //    if (Length != null) 
		//			return Length;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool LengthIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLength();
				//if (result != null && Length != null){
				//	return !Length.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultNoSignWord(View view = null);
        //SetDefaultLanguage(View view = null);
        //SetDefaultWordQuantity(View view = null);
        //SetDefaultLength(View view = null);
			
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

                switch (propertyName)
                {       
				
                    case nameof(Name):
                        OnChangedName(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedName(object oldValue, object newValue)
        {
            #region 1569ImportCode
            SetDefaultLength();
if (newValue is null) return;
if (Name.Contains(' '))
                    Name = Name.Replace(' ', ' ');
            
            #endregion 1569ImportCode
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
#region 1567ImportCode
		public int? GetDefaultLength(View view = null)
        {
            //Code: 1567            Oid: c595d1b3-8f44-48bc-9bae-a18258468247
            if (!string.IsNullOrEmpty(Name))
                return Name.Length;
            return null;       

        }
#endregion 1567ImportCode
#region 1568ImportCode
		public void SetDefaultLength(View view = null)
        {
            //Code: 1568            Oid: c3ab1941-f7f9-440f-b427-f8a1ed8501ef
            Length = GetDefaultLength();

        }
#endregion 1568ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
