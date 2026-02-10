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
    [ModelDefault("Caption", "Tham số tính giá"), ImageName("PriceParameter")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "PriceParameter_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "PriceParameter_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "PricePolicy_PriceParameterList_ListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class PriceParameter:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public PriceParameter(Session session)
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

	
       
		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(1)]		

 		[Size(20)]
		[RuleUniqueValue("UniquePriceParameterCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredPriceParameterCode", DefaultContexts.Save)]
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

	
       
		//private int? _value;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giá trị")]
        [ToolTip("Giá trị")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Value
        { 
		    get => GetPropertyValue<int?>("Value");                         
			set => SetPropertyValue<int?>("Value", value); 
			
        }
		//Tooltip for Object
		public object ValueToolTipControllerText(View view)
        {
        //    if (Value != null) 
		//			return Value;
            return null;
        }
		//Get Default Value
        public int? GetDefaultValue(View view = null)
        { 
			return Value;
        }
		//Set Default Value
		public void SetDefaultValue(View view = null)
        {
            //if (Value is null){
            //    var result = GetDefaultValue(view);
            //    if (result != null && result != Value){
			//          Value = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultValue();
				//if (result != null && Value != null){
				//	return !Value.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _parametertype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(3)]		

 		[Size(150)]
		public string ParameterType
        { 
		    get => GetPropertyValue<string>("ParameterType");                         
			set => SetPropertyValue<string>("ParameterType", value); 
			
        }
		//Tooltip for Object
		public object ParameterTypeToolTipControllerText(View view)
        {
        //    if (ParameterType != null) 
		//			return ParameterType;
            return null;
        }
		//Get Default Value
        public string GetDefaultParameterType(View view = null)
        { 
			return ParameterType;
        }
		//Set Default Value
		public void SetDefaultParameterType(View view = null)
        {
            //if (ParameterType is null){
            //    var result = GetDefaultParameterType(view);
            //    if (result != null && result != ParameterType){
			//          ParameterType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ParameterTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultParameterType();
				//if (result != null && ParameterType != null){
				//	return !ParameterType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.PricePolicy _pricepolicy;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chính sách giá")]
        [ToolTip("Chính sách giá")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PricePolicyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("PricePolicy-PriceParameterList")]
	 
		public Module.BusinessObjects.PricePolicy PricePolicy
        { 
		    get => GetPropertyValue<Module.BusinessObjects.PricePolicy>("PricePolicy");                         
			set => SetPropertyValue<Module.BusinessObjects.PricePolicy>("PricePolicy", value); 
			
        }
		//Tooltip for Object
		public object PricePolicyToolTipControllerText(View view)
        {
        //    if (PricePolicy != null) 
		//			return PricePolicy;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.PricePolicy GetDefaultPricePolicy(View view = null)
        { 
			return PricePolicy;
        }
		//Set Default Value
		public void SetDefaultPricePolicy(View view = null)
        {
            //if (PricePolicy is null){
            //    var result = GetDefaultPricePolicy(view);
            //    if (result != null && result != PricePolicy){
			//          PricePolicy = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PricePolicyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPricePolicy();
				//if (result != null && PricePolicy != null){
				//	return !PricePolicy.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PricePolicyCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PricePolicy));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultValue(View view = null);
        //SetDefaultParameterType(View view = null);
        //SetDefaultPricePolicy(View view = null);
			
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
