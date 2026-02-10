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
    [ModelDefault("Caption", "Loại quan hệ"), ImageName("RelationType")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(SystemTypeCode))]
 
	[MobileColumnAttribute(Context = "RelationType_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "RelationType_LookupListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class RelationType:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public RelationType(Session session)
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
               

		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(0)]		

 		[Size(50)]
		[RuleUniqueValue("UniqueRelationTypeCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredRelationTypeCode", DefaultContexts.Save)]
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

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(1)]		

 		[Size(100)]
		[RuleRequiredField("RequiredRelationTypeName", DefaultContexts.Save)]
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

	
       
		//private System.Type _systemtype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Kiểu hệ thống")]
        [ToolTip("Kiểu hệ thống")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SystemTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [ValueConverter(typeof(DevExpress.ExpressApp.Utils.TypeToStringConverter))]
	    [TypeConverter(typeof(DevExpress.Persistent.Base.Security.SecurityTargetTypeConverter))]
		public System.Type SystemType
        { 
		    get => GetPropertyValue<System.Type>("SystemType");                         
			set => SetPropertyValue<System.Type>("SystemType", value); 
			
        }
		//Tooltip for Object
		public object SystemTypeToolTipControllerText(View view)
        {
        //    if (SystemType != null) 
		//			return SystemType;
            return null;
        }
		//Get Default Value
        public System.Type GetDefaultSystemType(View view = null)
        { 
			return SystemType;
        }
		//Set Default Value
		public void SetDefaultSystemType(View view = null)
        {
            //if (SystemType is null){
            //    var result = GetDefaultSystemType(view);
            //    if (result != null && result != SystemType){
			//          SystemType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SystemTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSystemType();
				//if (result != null && SystemType != null){
				//	return !SystemType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SystemTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SystemType));
            }
        }
	
       
		//private string _upper;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Vai trên")]
        [ToolTip("Vai trên")]
		//[Index(3)]		

 		[Size(100)]
		[RuleRequiredField("RequiredRelationTypeUpper", DefaultContexts.Save)]
		public string Upper
        { 
		    get => GetPropertyValue<string>("Upper");                         
			set => SetPropertyValue<string>("Upper", value); 
			
        }
		//Tooltip for Object
		public object UpperToolTipControllerText(View view)
        {
        //    if (Upper != null) 
		//			return Upper;
            return null;
        }
		//Get Default Value
        public string GetDefaultUpper(View view = null)
        { 
			return Upper;
        }
		//Set Default Value
		public void SetDefaultUpper(View view = null)
        {
            //if (Upper is null){
            //    var result = GetDefaultUpper(view);
            //    if (result != null && result != Upper){
			//          Upper = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpper();
				//if (result != null && Upper != null){
				//	return !Upper.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _lower;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Vai dưới")]
        [ToolTip("Vai dưới")]
		//[Index(4)]		

 		[Size(100)]
		[RuleRequiredField("RequiredRelationTypeLower", DefaultContexts.Save)]
		public string Lower
        { 
		    get => GetPropertyValue<string>("Lower");                         
			set => SetPropertyValue<string>("Lower", value); 
			
        }
		//Tooltip for Object
		public object LowerToolTipControllerText(View view)
        {
        //    if (Lower != null) 
		//			return Lower;
            return null;
        }
		//Get Default Value
        public string GetDefaultLower(View view = null)
        { 
			return Lower;
        }
		//Set Default Value
		public void SetDefaultLower(View view = null)
        {
            //if (Lower is null){
            //    var result = GetDefaultLower(view);
            //    if (result != null && result != Lower){
			//          Lower = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LowerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLower();
				//if (result != null && Lower != null){
				//	return !Lower.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(5)]		
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

	
       
		//private string _systemtypecode;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã đối tượng")]
        [ToolTip("Mã đối tượng")]
		//[Index(6)]		

 		[Size(100)]
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
		//Set Default Value

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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0429ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
            #endregion 0429ImportCode
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultSystemType(View view = null);
        //SetDefaultUpper(View view = null);
        //SetDefaultLower(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultSystemTypeCode(View view = null);
			
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
            #region 0462ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0462ImportCode
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
				
                    case nameof(SystemType):
                        OnChangedSystemType(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedSystemType(object oldValue, object newValue)
        {
            #region 1530ImportCode
            if (newValue is null) return;
SetDefaultSystemTypeCode();            
            #endregion 1530ImportCode
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
#region 0084ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0084            Oid: 15c5e90a-301f-4ddd-af32-93f5b8c2ec1b
            Update = GetDefaultUpdate();
        }
#endregion 0084ImportCode
#region 1529ImportCode
		public void SetDefaultSystemTypeCode(View view = null)
        {
            //Code: 1529            Oid: 09a4c5a2-aeb6-4993-b925-922524d3c60e
            if(String.IsNullOrEmpty(SystemTypeCode)) SystemTypeCode = GetDefaultSystemTypeCode();
        }
#endregion 1529ImportCode
#region 0101ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0101            Oid: 6d0680b4-1d00-4309-a0c1-561a15e7a8a3
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0101ImportCode
#region 1528ImportCode
		public string GetDefaultSystemTypeCode(View view = null)
        {
            //Code: 1528            Oid: bee77f38-d41c-4f5f-8266-263204dbbb4a
            if(SystemType != null) return SystemType.Name;
return null;
        }
#endregion 1528ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
