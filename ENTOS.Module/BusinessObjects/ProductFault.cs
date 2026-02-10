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
	[NavigationItem("ProductBusiness")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Lỗi sản phẩm"), ImageName("ProductFault")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "ProductFault_LookupListView", TargetItems = nameof(English)+ "," + nameof(English))]
	[MobileColumnAttribute(Context = "ProductFault_ListView", TargetItems = nameof(Organization)+ "," + nameof(Categoy)+ "," + nameof(Name))]
	[DefaultProperty("English")]
 
[OptimisticLocking(true)]
    public partial class ProductFault:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ProductFault(Session session)
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

 		[Size(100)]
		[RuleRequiredField("RequiredProductFaultName", DefaultContexts.Save)]
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

	
       
		//private string _english;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tiếng Anh")]
        [ToolTip("Tiếng Anh")]
		//[Index(1)]		

 		[Size(240)]
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

	
       
		//private string _categoy;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(2)]		

 		[Size(100)]
		public string Categoy
        { 
		    get => GetPropertyValue<string>("Categoy");                         
			set => SetPropertyValue<string>("Categoy", value); 
			
        }
		//Tooltip for Object
		public object CategoyToolTipControllerText(View view)
        {
        //    if (Categoy != null) 
		//			return Categoy;
            return null;
        }
		//Get Default Value
        public string GetDefaultCategoy(View view = null)
        { 
			return Categoy;
        }
		//Set Default Value
		public void SetDefaultCategoy(View view = null)
        {
            //if (Categoy is null){
            //    var result = GetDefaultCategoy(view);
            //    if (result != null && result != Categoy){
			//          Categoy = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CategoyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCategoy();
				//if (result != null && Categoy != null){
				//	return !Categoy.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Org _organization;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hãng")]
        [ToolTip("Hãng")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OrganizationCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Org Organization
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Org>("Organization");                         
			set => SetPropertyValue<Module.BusinessObjects.Org>("Organization", value); 
			
        }
		//Tooltip for Object
		public object OrganizationToolTipControllerText(View view)
        {
        //    if (Organization != null) 
		//			return Organization;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Org GetDefaultOrganization(View view = null)
        { 
			return Organization;
        }
		//Set Default Value
		public void SetDefaultOrganization(View view = null)
        {
            //if (Organization is null){
            //    var result = GetDefaultOrganization(view);
            //    if (result != null && result != Organization){
			//          Organization = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OrganizationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrganization();
				//if (result != null && Organization != null){
				//	return !Organization.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator OrganizationCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Organization));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultEnglish(View view = null);
        //SetDefaultCategoy(View view = null);
        //SetDefaultOrganization(View view = null);
			
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
