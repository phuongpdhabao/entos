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
    [ModelDefault("Caption", "Đầu vào dịch vụ"), ImageName("ServiceInput")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("ServiceInput DataTypeT2 Hide_None__" , TargetItems = "DataTypeT2" , Criteria = "[DataType] Is Null Or [DataType.GenericType] <> ##ToString#Generic2#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
	[Appearance("ServiceInput DataTypeT1 Hide_None__" , TargetItems = "DataTypeT1" , Criteria = "[DataType] Is Null Or [DataType.GenericType] Is Null",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Order))]
 
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class ServiceInput:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ServiceInput(Session session)
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

	
       
		//private Module.BusinessObjects.DataType _datatype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Kiểu")]
        [ToolTip("Kiểu")]
		//[Index(1)]		
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
	
       
		//private Module.BusinessObjects.DataType _datatypet1;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu T1")]
        [ToolTip("Kiểu T1")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DataTypeT1Criteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.DataType DataTypeT1
        { 
		    get => GetPropertyValue<Module.BusinessObjects.DataType>("DataTypeT1");                         
			set => SetPropertyValue<Module.BusinessObjects.DataType>("DataTypeT1", value); 
			
        }
		//Tooltip for Object
		public object DataTypeT1ToolTipControllerText(View view)
        {
        //    if (DataTypeT1 != null) 
		//			return DataTypeT1;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.DataType GetDefaultDataTypeT1(View view = null)
        { 
			return DataTypeT1;
        }
		//Set Default Value
		public void SetDefaultDataTypeT1(View view = null)
        {
            //if (DataTypeT1 is null){
            //    var result = GetDefaultDataTypeT1(view);
            //    if (result != null && result != DataTypeT1){
			//          DataTypeT1 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DataTypeT1IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDataTypeT1();
				//if (result != null && DataTypeT1 != null){
				//	return !DataTypeT1.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator DataTypeT1Criteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(DataTypeT1));
            }
        }
	
       
		//private Module.BusinessObjects.DataType _datatypet2;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu T2")]
        [ToolTip("Kiểu T2")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DataTypeT2Criteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.DataType DataTypeT2
        { 
		    get => GetPropertyValue<Module.BusinessObjects.DataType>("DataTypeT2");                         
			set => SetPropertyValue<Module.BusinessObjects.DataType>("DataTypeT2", value); 
			
        }
		//Tooltip for Object
		public object DataTypeT2ToolTipControllerText(View view)
        {
        //    if (DataTypeT2 != null) 
		//			return DataTypeT2;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.DataType GetDefaultDataTypeT2(View view = null)
        { 
			return DataTypeT2;
        }
		//Set Default Value
		public void SetDefaultDataTypeT2(View view = null)
        {
            //if (DataTypeT2 is null){
            //    var result = GetDefaultDataTypeT2(view);
            //    if (result != null && result != DataTypeT2){
			//          DataTypeT2 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DataTypeT2IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDataTypeT2();
				//if (result != null && DataTypeT2 != null){
				//	return !DataTypeT2.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator DataTypeT2Criteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(DataTypeT2));
            }
        }
	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(4)]		
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
		//Set Default Value

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

	
       
		//private Module.BusinessObjects.SoftwareServiceType _softwareservicetype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại dịch vụ phần mềm")]
        [ToolTip("Loại dịch vụ phần mềm")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SoftwareServiceTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("SoftwareServiceType-ServiceInputList")]
	 
		public Module.BusinessObjects.SoftwareServiceType SoftwareServiceType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.SoftwareServiceType>("SoftwareServiceType");                         
			set => SetPropertyValue<Module.BusinessObjects.SoftwareServiceType>("SoftwareServiceType", value); 
			
        }
		//Tooltip for Object
		public object SoftwareServiceTypeToolTipControllerText(View view)
        {
        //    if (SoftwareServiceType != null) 
		//			return SoftwareServiceType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.SoftwareServiceType GetDefaultSoftwareServiceType(View view = null)
        { 
			return SoftwareServiceType;
        }
		//Set Default Value
		public void SetDefaultSoftwareServiceType(View view = null)
        {
            //if (SoftwareServiceType is null){
            //    var result = GetDefaultSoftwareServiceType(view);
            //    if (result != null && result != SoftwareServiceType){
			//          SoftwareServiceType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SoftwareServiceTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSoftwareServiceType();
				//if (result != null && SoftwareServiceType != null){
				//	return !SoftwareServiceType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SoftwareServiceTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SoftwareServiceType));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultDataType(View view = null);
        //SetDefaultDataTypeT1(View view = null);
        //SetDefaultDataTypeT2(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultSoftwareServiceType(View view = null);
			
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
				
                    case nameof(SoftwareServiceType):
                        OnChangedSoftwareServiceType(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedSoftwareServiceType(object oldValue, object newValue)
        {
            #region 3807ImportCode
            base.AfterConstruction();
SetDefaultOrder();            
            #endregion 3807ImportCode
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
#region 3514ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 3514            Oid: 10307deb-7e04-4f3d-992f-956bfeedbb4a
            if (SoftwareServiceType != null && SoftwareServiceType.ServiceInputList != null)
{
    var lasted = SoftwareServiceType.ServiceInputList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
return null;
        }
#endregion 3514ImportCode
#region 3516ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 3516            Oid: 81831ede-84ed-410e-865b-d99eda58ff1c
            Order = GetDefaultOrder();
        }
#endregion 3516ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
