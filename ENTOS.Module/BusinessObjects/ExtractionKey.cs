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
    [ModelDefault("Caption", "Khóa trích xuất"), ImageName("ExtractionKey")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(Updater))]
 
	[DefaultProperty("Code")]
 
	[RuleCombinationOfPropertiesIsUnique("UniqueRule.ExtractionKey.Name", DefaultContexts.Save, "Name, DataLayout, ExtractionTemplate")]
	[RuleCombinationOfPropertiesIsUnique("UniqueRule.ExtractionKey.Code", DefaultContexts.Save, "Code, DataLayout, ExtractionTemplate")]
[OptimisticLocking(true)]
    public partial class ExtractionKey:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ExtractionKey(Session session)
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
		[DevExpress.Xpo.DisplayName("Tên trường Json")]
        [ToolTip("Tên trường Json")]
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

	
       
		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã trường")]
        [ToolTip("Mã trường")]
		//[Index(1)]		

 		[Size(250)]
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

	
       
		//private DataLayout _datalayout;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Trực thuộc")]
        [ToolTip("Trực thuộc")]
		//[Index(2)]		
		public DataLayout DataLayout
        { 
		    get => GetPropertyValue<DataLayout>("DataLayout");                         
			set => SetPropertyValue<DataLayout>("DataLayout", value); 
			
        }
		//Tooltip for Object
		public object DataLayoutToolTipControllerText(View view)
        {
        //    if (DataLayout != null) 
		//			return DataLayout;
            return null;
        }
		//Get Default Value
        public DataLayout GetDefaultDataLayout(View view = null)
        { 
			return DataLayout;
        }
		//Set Default Value
		public void SetDefaultDataLayout(View view = null)
        {
            //if (DataLayout is null){
            //    var result = GetDefaultDataLayout(view);
            //    if (result != null && result != DataLayout){
			//          DataLayout = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DataLayoutIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDataLayout();
				//if (result != null && DataLayout != null){
				//	return !DataLayout.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.DataType _datatype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu")]
        [ToolTip("Kiểu")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DataTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.DataType DataType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.DataType>("DataType");                         
			set => SetPropertyValue<Module.BusinessObjects.DataType>("DataType", value); 
			
        }
		//Tooltip for Object
		public object DataTypeToolTipControllerText(View view)
        {
        //    if (DataType != null) 
		//			return DataType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.DataType GetDefaultDataType(View view = null)
        { 
			return DataType;
        }
		//Set Default Value
		public void SetDefaultDataType(View view = null)
        {
            //if (DataType is null){
            //    var result = GetDefaultDataType(view);
            //    if (result != null && result != DataType){
			//          DataType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DataTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDataType();
				//if (result != null && DataType != null){
				//	return !DataType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator DataTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(DataType));
            }
        }
	
       
		//private string _validation;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Điều kiện")]
        [ToolTip("Điều kiện")]
		//[Index(4)]		

 		[Size(250)]
		public string Validation
        { 
		    get => GetPropertyValue<string>("Validation");                         
			set => SetPropertyValue<string>("Validation", value); 
			
        }
		//Tooltip for Object
		public object ValidationToolTipControllerText(View view)
        {
        //    if (Validation != null) 
		//			return Validation;
            return null;
        }
		//Get Default Value
        public string GetDefaultValidation(View view = null)
        { 
			return Validation;
        }
		//Set Default Value
		public void SetDefaultValidation(View view = null)
        {
            //if (Validation is null){
            //    var result = GetDefaultValidation(view);
            //    if (result != null && result != Validation){
			//          Validation = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ValidationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultValidation();
				//if (result != null && Validation != null){
				//	return !Validation.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(5)]		
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
		//[Index(6)]		
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
	
       
		//private string _systemtypecode;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã đối tượng")]
        [ToolTip("Mã đối tượng")]
		//[Index(7)]		

 		[Size(250)]
		public string SystemTypeCode
        { 
		    get => GetPropertyValue<string>("SystemTypeCode");                         
			set => SetPropertyValue<string>("SystemTypeCode", value); 
			
        }
		//Tooltip for Object
		public object SystemTypeCodeToolTipControllerText(View view)
        {
        //    if (SystemTypeCode != null) 
		//			return SystemTypeCode;
            return null;
        }
		//Get Default Value
        public string GetDefaultSystemTypeCode(View view = null)
        { 
			return SystemTypeCode;
        }
		//Set Default Value
		public void SetDefaultSystemTypeCode(View view = null)
        {
            //if (SystemTypeCode is null){
            //    var result = GetDefaultSystemTypeCode(view);
            //    if (result != null && result != SystemTypeCode){
			//          SystemTypeCode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SystemTypeCodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSystemTypeCode();
				//if (result != null && SystemTypeCode != null){
				//	return !SystemTypeCode.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DataTypeCategory? _datatypecategory;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại kiểu dữ liệu")]
        [ToolTip("Loại kiểu dữ liệu")]
		//[Index(8)]		
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

	
       
		//private Module.BusinessObjects.ExtractionTemplate _extractiontemplate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mẫu trích xuất")]
        [ToolTip("Mẫu trích xuất")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ExtractionTemplateCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("ExtractionTemplate-ExtractionKeyList")]
	 
		public Module.BusinessObjects.ExtractionTemplate ExtractionTemplate
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ExtractionTemplate>("ExtractionTemplate");                         
			set => SetPropertyValue<Module.BusinessObjects.ExtractionTemplate>("ExtractionTemplate", value); 
			
        }
		//Tooltip for Object
		public object ExtractionTemplateToolTipControllerText(View view)
        {
        //    if (ExtractionTemplate != null) 
		//			return ExtractionTemplate;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ExtractionTemplate GetDefaultExtractionTemplate(View view = null)
        { 
			return ExtractionTemplate;
        }
		//Set Default Value
		public void SetDefaultExtractionTemplate(View view = null)
        {
            //if (ExtractionTemplate is null){
            //    var result = GetDefaultExtractionTemplate(view);
            //    if (result != null && result != ExtractionTemplate){
			//          ExtractionTemplate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ExtractionTemplateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultExtractionTemplate();
				//if (result != null && ExtractionTemplate != null){
				//	return !ExtractionTemplate.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ExtractionTemplateCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ExtractionTemplate));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 3864ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
            #endregion 3864ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultDataLayout(View view = null);
        //SetDefaultDataType(View view = null);
        //SetDefaultValidation(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultSystemTypeCode(View view = null);
        //SetDefaultDataTypeCategory(View view = null);
        //SetDefaultExtractionTemplate(View view = null);
			
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
            #region 3863ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 3863ImportCode
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
#region 3862ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 3862            Oid: dbeca896-b5e1-4f64-a2db-bac15ed0b66c
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 3862ImportCode
#region 3865ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 3865            Oid: ba8a5591-827d-4030-9a0c-366b54b4c11a
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 3865ImportCode
#region 3867ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 3867            Oid: f3f892c9-8af9-41c7-92f9-93ebe7103bcb
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3867ImportCode
#region 3866ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 3866            Oid: f20ff438-9a1d-43e5-aac1-c656547926a0
            Updater = GetDefaultUpdater();
        }
#endregion 3866ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
