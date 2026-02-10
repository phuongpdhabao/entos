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
    [ModelDefault("Caption", "Biến thể loại sản phẩm"), ImageName("ProductTypeVariation")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update))]
 
	[MobileColumnAttribute(Context = "ProductTypeVariation_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ProductType_ProductTypeVariationList_ListView", TargetItems = nameof(Update)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "ProductTypeVariation_ListView", TargetItems = "ProductType.Folder"+ "," + nameof(Name)+ "," + nameof(ProductType))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class ProductTypeVariation:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ProductTypeVariation(Session session)
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
		[RuleUniqueValue("UniqueProductTypeVariationName", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
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
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên TA")]
        [ToolTip("Tên TA")]
		//[Index(1)]		

 		[Size(150)]
		[RuleUniqueValue("UniqueProductTypeVariationEnglish", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
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

	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(2)]		
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

	
       
		//private Module.BusinessObjects.ProductType _producttype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại sản phẩm")]
        [ToolTip("Loại sản phẩm")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("ProductType-ProductTypeVariationList")]
	 
		public Module.BusinessObjects.ProductType ProductType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ProductType>("ProductType");                         
			set => SetPropertyValue<Module.BusinessObjects.ProductType>("ProductType", value); 
			
        }
		//Tooltip for Object
		public object ProductTypeToolTipControllerText(View view)
        {
        //    if (ProductType != null) 
		//			return ProductType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ProductType GetDefaultProductType(View view = null)
        { 
			return ProductType;
        }
		//Set Default Value
		public void SetDefaultProductType(View view = null)
        {
            //if (ProductType is null){
            //    var result = GetDefaultProductType(view);
            //    if (result != null && result != ProductType){
			//          ProductType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ProductTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultProductType();
				//if (result != null && ProductType != null){
				//	return !ProductType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ProductTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ProductType));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultEnglish(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultProductType(View view = null);
			
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
            #region 1261ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 1261ImportCode
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
#region 1262ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 1262            Oid: 05fa0f0b-fa7b-492b-8ea6-08baffcdc8fd
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1262ImportCode
#region 1260ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 1260            Oid: 13570f1d-ec7f-4c43-a440-6f321ebd731c
            Update = GetDefaultUpdate();
        }
#endregion 1260ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
