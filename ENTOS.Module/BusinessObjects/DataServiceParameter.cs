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
    [ModelDefault("Caption", "Tham số Dịch vụ Dữ liệu"), ImageName("DataServiceParameter")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Order))]
 
	[MobileColumnAttribute(Context = "DataService_DataServiceParameterList_ListView", TargetItems = nameof(Value))]
	[MobileColumnAttribute(Context = "DataServiceParameter_LookupListView", TargetItems = nameof(Value))]
	[MobileColumnAttribute(Context = "DataServiceParameter_ListView", TargetItems = nameof(Value))]
	[DefaultProperty("Value")]
 
[OptimisticLocking(true)]
    public partial class DataServiceParameter:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public DataServiceParameter(Session session)
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
               

		//private DataServiceParameterType _type;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(0)]		
		public DataServiceParameterType Type
        { 
		    get => GetPropertyValue<DataServiceParameterType>("Type");                         
			set => SetPropertyValue<DataServiceParameterType>("Type", value); 
			
        }
		//Tooltip for Object
		public object TypeToolTipControllerText(View view)
        {
        //    if (Type != null) 
		//			return Type;
            return null;
        }
		//Get Default Value
        public DataServiceParameterType GetDefaultType(View view = null)
        { 
			return Type;
        }
		//Set Default Value
		public void SetDefaultType(View view = null)
        {
            //if (Type is null){
            //    var result = GetDefaultType(view);
            //    if (result != null && result != Type){
			//          Type = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultType();
				//if (result != null && Type != null){
				//	return !Type.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(1)]		

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

	
       
		//private string _value;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giá trị")]
        [ToolTip("Giá trị")]
		//[Index(2)]		

 		[Size(SizeAttribute.Unlimited)]
		public string Value
        { 
		    get => GetPropertyValue<string>("Value");                         
			set => SetPropertyValue<string>("Value", value); 
			
        }
		//Tooltip for Object
		public object ValueToolTipControllerText(View view)
        {
        //    if (Value != null) 
		//			return Value;
            return null;
        }
		//Get Default Value
        public string GetDefaultValue(View view = null)
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

	
       
		//private string _note;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(3)]		

 		[Size(250)]
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

	
       
		//private DataServiceParameterOption _dataserviceparameteroption;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Kiểu tham số")]
        [ToolTip("Kiểu tham số")]
		//[Index(4)]		
		public DataServiceParameterOption DataServiceParameterOption
        { 
		    get => GetPropertyValue<DataServiceParameterOption>("DataServiceParameterOption");                         
			set => SetPropertyValue<DataServiceParameterOption>("DataServiceParameterOption", value); 
			
        }
		//Tooltip for Object
		public object DataServiceParameterOptionToolTipControllerText(View view)
        {
        //    if (DataServiceParameterOption != null) 
		//			return DataServiceParameterOption;
            return null;
        }
		//Get Default Value
        public DataServiceParameterOption GetDefaultDataServiceParameterOption(View view = null)
        { 
			return DataServiceParameterOption;
        }
		//Set Default Value
		public void SetDefaultDataServiceParameterOption(View view = null)
        {
            //if (DataServiceParameterOption is null){
            //    var result = GetDefaultDataServiceParameterOption(view);
            //    if (result != null && result != DataServiceParameterOption){
			//          DataServiceParameterOption = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DataServiceParameterOptionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDataServiceParameterOption();
				//if (result != null && DataServiceParameterOption != null){
				//	return !DataServiceParameterOption.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _order;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(5)]		
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

	
       
		//private bool _inactive;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(6)]		
		public bool InActive
        { 
		    get => GetPropertyValue<bool>("InActive");                         
			set => SetPropertyValue<bool>("InActive", value); 
			
        }
		//Tooltip for Object
		public object InActiveToolTipControllerText(View view)
        {
        //    if (InActive != null) 
		//			return InActive;
            return null;
        }
		//Get Default Value
        public bool GetDefaultInActive(View view = null)
        { 
			return InActive;
        }
		//Set Default Value
		public void SetDefaultInActive(View view = null)
        {
            //if (InActive is null){
            //    var result = GetDefaultInActive(view);
            //    if (result != null && result != InActive){
			//          InActive = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool InActiveIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultInActive();
				//if (result != null && InActive != null){
				//	return !InActive.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.DataService _dataservice;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Dịch vụ Dữ liệu")]
        [ToolTip("Dịch vụ Dữ liệu")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DataServiceCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("DataService-DataServiceParameterList")]
	 
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
 
            base.AfterConstruction();
 
        //SetDefaultType(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultValue(View view = null);
        //SetDefaultNote(View view = null);
        //SetDefaultDataServiceParameterOption(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultInActive(View view = null);
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

                switch (propertyName)
                {       
				
                    case nameof(DataService):
                        OnChangedDataService(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedDataService(object oldValue, object newValue)
        {
            #region 3302ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 3302ImportCode
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
#region 2625ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 2625            Oid: 9b1b1f3d-6d16-4bde-a267-6842a15e8b1e
            if (DataService != null && DataService.DataServiceParameterList != null)
{
    var lasted = DataService.DataServiceParameterList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
return null;
        }
#endregion 2625ImportCode
#region 2626ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 2626            Oid: 2a567331-42ae-4ce8-92ef-d7560b9a2775
            Order= GetDefaultOrder();
        }
#endregion 2626ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
