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
	[NavigationItem("Default")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Tham số"), ImageName("Parameter")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(Updater)+ "," + nameof(Order))]
 
	[MobileColumnAttribute(Context = "Parameter_LookupListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
	[UpDownTopBottomOrder(AscSort = true, ChangeBetweenRow = false, AutoSave = false)]
	[RuleCombinationOfPropertiesIsUnique("UniqueRule.Parameter", DefaultContexts.Save, "Name, PermissionPolicyUser")]
[OptimisticLocking(true)]
    public partial class Parameter:  DevExpress.Xpo.XPLiteObject , IReOrder , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Parameter(Session session)
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
		[RuleRequiredField("RequiredParameterName", DefaultContexts.Save)]
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

	
       
		//private string _category;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Phân loại")]
        [ToolTip("Phân loại")]
		//[Index(1)]		

 		[Size(250)]
	    [ModelDefault("PropertyEditorType", "StringComboEditor")]
		public string Category
        { 
		    get => GetPropertyValue<string>("Category");                         
			set => SetPropertyValue<string>("Category", value); 
			
        }
		//Tooltip for Object
		public object CategoryToolTipControllerText(View view)
        {
        //    if (Category != null) 
		//			return Category;
            return null;
        }
		//Get Default Value
        public string GetDefaultCategory(View view = null)
        { 
			return Category;
        }
		//Set Default Value
		public void SetDefaultCategory(View view = null)
        {
            //if (Category is null){
            //    var result = GetDefaultCategory(view);
            //    if (result != null && result != Category){
			//          Category = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CategoryIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCategory();
				//if (result != null && Category != null){
				//	return !Category.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private ParameterFormat _parameterformat;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Định dạng")]
        [ToolTip("Định dạng")]
		//[Index(2)]		
		public ParameterFormat ParameterFormat
        { 
		    get => GetPropertyValue<ParameterFormat>("ParameterFormat");                         
			set => SetPropertyValue<ParameterFormat>("ParameterFormat", value); 
			
        }
		//Tooltip for Object
		public object ParameterFormatToolTipControllerText(View view)
        {
        //    if (ParameterFormat != null) 
		//			return ParameterFormat;
            return null;
        }
		//Get Default Value
        public ParameterFormat GetDefaultParameterFormat(View view = null)
        { 
			return ParameterFormat;
        }
		//Set Default Value
		public void SetDefaultParameterFormat(View view = null)
        {
            //if (ParameterFormat is null){
            //    var result = GetDefaultParameterFormat(view);
            //    if (result != null && result != ParameterFormat){
			//          ParameterFormat = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ParameterFormatIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultParameterFormat();
				//if (result != null && ParameterFormat != null){
				//	return !ParameterFormat.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _note;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
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

	
       
		//private bool _user;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cá nhân")]
        [ToolTip("Cá nhân")]
		//[Index(4)]		
		public bool User
        { 
		    get => GetPropertyValue<bool>("User");                         
			set => SetPropertyValue<bool>("User", value); 
			
        }
		//Tooltip for Object
		public object UserToolTipControllerText(View view)
        {
        //    if (User != null) 
		//			return User;
            return null;
        }
		//Get Default Value
        public bool GetDefaultUser(View view = null)
        { 
			return User;
        }
		//Set Default Value
		public void SetDefaultUser(View view = null)
        {
            //if (User is null){
            //    var result = GetDefaultUser(view);
            //    if (result != null && result != User){
			//          User = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UserIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUser();
				//if (result != null && User != null){
				//	return !User.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _value;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giá trị")]
        [ToolTip("Giá trị")]
		//[Index(5)]		

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

	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(6)]		
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

	
       
		//private Module.BusinessObjects.Member _updater;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Người cập nhật")]
        [ToolTip("Người cập nhật")]
		//[Index(7)]		
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
	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(8)]		
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

	
       
		//private DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser _permissionpolicyuser;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Người dùng")]
        [ToolTip("Người dùng")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PermissionPolicyUserCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser PermissionPolicyUser
        { 
		    get => GetPropertyValue<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser>("PermissionPolicyUser");                         
			set => SetPropertyValue<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser>("PermissionPolicyUser", value); 
			
        }
		//Tooltip for Object
		public object PermissionPolicyUserToolTipControllerText(View view)
        {
        //    if (PermissionPolicyUser != null) 
		//			return PermissionPolicyUser;
            return null;
        }
		//Get Default Value
        public DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser GetDefaultPermissionPolicyUser(View view = null)
        { 
			return PermissionPolicyUser;
        }
		//Set Default Value
		public void SetDefaultPermissionPolicyUser(View view = null)
        {
            //if (PermissionPolicyUser is null){
            //    var result = GetDefaultPermissionPolicyUser(view);
            //    if (result != null && result != PermissionPolicyUser){
			//          PermissionPolicyUser = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PermissionPolicyUserIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPermissionPolicyUser();
				//if (result != null && PermissionPolicyUser != null){
				//	return !PermissionPolicyUser.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PermissionPolicyUserCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PermissionPolicyUser));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultCategory(View view = null);
        //SetDefaultParameterFormat(View view = null);
        //SetDefaultNote(View view = null);
        //SetDefaultUser(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultPermissionPolicyUser(View view = null);
			
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
            #region 0508ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 0508ImportCode
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

                switch (propertyName)
                {       
				
                    case nameof(Category):
                        OnChangedCategory(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedCategory(object oldValue, object newValue)
        {
            #region 0362ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 0362ImportCode
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
			//	SetDefaultValue();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0020ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0020            Oid: 403798e0-28db-4626-a719-41ff765f08ff
            Update = GetDefaultUpdate();
        }
#endregion 0020ImportCode
#region 1106ImportCode
		public int GetIntValue()
        {
            //Code: 1106            Oid: 1b5b2837-3a2d-4b49-a45e-35b34a126258
            if (!string.IsNullOrEmpty(Value))
    return Convert.ToInt32(Value, new System.Globalization.CultureInfo("en-US"));
return 0;
        }
#endregion 1106ImportCode
#region 3584ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 3584            Oid: c48bce00-b2be-47e6-96b0-25454d8ea343
            if(Order == null) Order = GetDefaultOrder();
        }
#endregion 3584ImportCode
#region 1107ImportCode
		public decimal GetDecimalValue()
        {
            //Code: 1107            Oid: c96346a3-4835-44ff-9223-7d911cd5157b
            if (!string.IsNullOrEmpty(Value))
    return Convert.ToDecimal(Value, new System.Globalization.CultureInfo("en-US"));
return 0;
        }
#endregion 1107ImportCode
#region 0047ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0047            Oid: 619a1e2b-1e7f-4a0f-b0c1-cd1e0f5a07ff
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0047ImportCode
#region 3580ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 3580            Oid: 344e35b5-0117-4c64-a923-1dd2def03652
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3580ImportCode
#region 3582ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 3582            Oid: a5233364-b16b-40d2-ad02-24ff639d8af7
                        //Code: 3582            Oid: a5233364-b16b-40d2-ad02-24ff639d8af7
            var sort = new DevExpress.Xpo.SortProperty(nameof(Order), DevExpress.Xpo.DB.SortingDirection.Descending);
            var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Category= ?", Category);
            var lastObject = Module.Helpers.XafXpoHelper.GetLastedBySort(Session, this.GetType(), criteria, sort) as Parameter;
            if (lastObject != null)
                return lastObject.Order + 1;
            return null;
        }
#endregion 3582ImportCode
#region 3578ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 3578            Oid: 9e98a438-5455-4c2a-9dee-fd4d85005802
            Updater = GetDefaultUpdater();
        }
#endregion 3578ImportCode
#region 1108ImportCode
		public bool GetBooleanValue()
        {
            //Code: 1108            Oid: 4cc0d0fc-5f52-4e60-8e77-49159f0cd578
            if (!string.IsNullOrEmpty(Value))
    return Convert.ToBoolean(Value);
return false;
        }
#endregion 1108ImportCode
        #endregion
//Mã nguồn bổ sung
#region ParameterImportCode
        public double GetDoubleValue()
        {
            if (!string.IsNullOrEmpty(Value))
                return Convert.ToDouble(Value, new System.Globalization.CultureInfo("en-US"));
            return 0;
        }
        public Module.BusinessObjects.Parameter CopyToNewParameter()
        {
            return Module.Helpers.XafXpoHelper.CopyObject<Module.BusinessObjects.Parameter>(this, this.Session);
            //var parameter = new Module.BusinessObjects.Parameter(Session);
            //parameter = Module.Helpers.XafXpoHelper.CopyObject<Module.BusinessObjects.Parameter>(this,);
            //parameter.Name = Name;
            //parameter.Value = Value;
            //parameter.SoftwareBusiness = SoftwareBusiness;
            //parameter.Note = Note;
            //parameter.Order = Order;
            //parameter.User = false;
            //return parameter;
        }
#endregion ParameterImportCode
		 		 
    }
}
