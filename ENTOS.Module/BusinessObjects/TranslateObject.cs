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
    [ModelDefault("Caption", "Đối tượng dịch"), ImageName("TranslateObject")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "TranslateObject_ListView", TargetItems = nameof(SystemType)+ "," + nameof(Name)+ "," + nameof(ObjectID))]
	[MobileColumnAttribute(Context = "Video_TranslateObjectList_ListView", TargetItems = nameof(SystemType)+ "," + nameof(Name)+ "," + nameof(Field))]
	[MobileColumnAttribute(Context = "TranslateObject_LookupListView", TargetItems = nameof(ObjectID)+ "," + nameof(Name)+ "," + nameof(SystemType))]
	[DefaultProperty("Order")]
 
[OptimisticLocking(true)]
    public partial class TranslateObject:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public TranslateObject(Session session)
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

	
       
		//private string _content;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(1)]		

 		[Size(SizeAttribute.Unlimited)]
	    [EditorAlias("CustomHtmlPropertyEditor")]
		public string Content
        { 
		    get => GetPropertyValue<string>("Content");                         
			set => SetPropertyValue<string>("Content", value); 
			
        }
		//Tooltip for Object
		public object ContentToolTipControllerText(View view)
        {
        //    if (Content != null) 
		//			return Content;
            return null;
        }
		//Get Default Value
        public string GetDefaultContent(View view = null)
        { 
			return Content;
        }
		//Set Default Value
		public void SetDefaultContent(View view = null)
        {
            //if (Content is null){
            //    var result = GetDefaultContent(view);
            //    if (result != null && result != Content){
			//          Content = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ContentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultContent();
				//if (result != null && Content != null){
				//	return !Content.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private System.Guid _objectid;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã đối tượng")]
        [ToolTip("Mã đối tượng")]
		//[Index(2)]		
	    [ModelDefault("AllowEdit", "False")]
		public System.Guid ObjectID
        { 
		    get => GetPropertyValue<System.Guid>("ObjectID");                         
			set => SetPropertyValue<System.Guid>("ObjectID", value); 
			
        }
		//Tooltip for Object
		public object ObjectIDToolTipControllerText(View view)
        {
        //    if (ObjectID != null) 
		//			return ObjectID;
            return null;
        }
		//Get Default Value
        public System.Guid GetDefaultObjectID(View view = null)
        { 
			return ObjectID;
        }
		//Set Default Value
		public void SetDefaultObjectID(View view = null)
        {
            //if (ObjectID is null){
            //    var result = GetDefaultObjectID(view);
            //    if (result != null && result != ObjectID){
			//          ObjectID = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ObjectIDIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultObjectID();
				//if (result != null && ObjectID != null){
				//	return !ObjectID.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private System.Type _systemtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Kiểu hệ thống")]
        [ToolTip("Kiểu hệ thống")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SystemTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [TypeConverter(typeof(DevExpress.Persistent.Base.Security.SecurityTargetTypeConverter))]
	    [ValueConverter(typeof(DevExpress.ExpressApp.Utils.TypeToStringConverter))]
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
	
       
		//private StringLookup _field;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Trường")]
        [ToolTip("Trường")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(FieldCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [ValueConverter(typeof(StringLookupToStringConverter))]
	    [DataSourceProperty("FieldSource")]
	    [ImmediatePostData()]
		public StringLookup Field
        { 
		    get => GetPropertyValue<StringLookup>("Field");                         
			set => SetPropertyValue<StringLookup>("Field", value); 
			
        }
		//Tooltip for Object
		public object FieldToolTipControllerText(View view)
        {
        //    if (Field != null) 
		//			return Field;
            return null;
        }
		//Get Default Value
        public StringLookup GetDefaultField(View view = null)
        { 
			return Field;
        }
		//Set Default Value
		public void SetDefaultField(View view = null)
        {
            //if (Field is null){
            //    var result = GetDefaultField(view);
            //    if (result != null && result != Field){
			//          Field = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FieldIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultField();
				//if (result != null && Field != null){
				//	return !Field.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator FieldCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Field));
            }
        }
	
       
		//private Module.BusinessObjects.Video _video;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tư liệu")]
        [ToolTip("Tư liệu")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(VideoCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Video-TranslateObjectList")]
	 
		public Module.BusinessObjects.Video Video
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Video>("Video");                         
			set => SetPropertyValue<Module.BusinessObjects.Video>("Video", value); 
			
        }
		//Tooltip for Object
		public object VideoToolTipControllerText(View view)
        {
        //    if (Video != null) 
		//			return Video;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Video GetDefaultVideo(View view = null)
        { 
			return Video;
        }
		//Set Default Value
		public void SetDefaultVideo(View view = null)
        {
            //if (Video is null){
            //    var result = GetDefaultVideo(view);
            //    if (result != null && result != Video){
			//          Video = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool VideoIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultVideo();
				//if (result != null && Video != null){
				//	return !Video.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator VideoCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Video));
            }
        }
	
       
		//private bool _flag;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ")]
        [ToolTip("Cờ")]
		//[Index(6)]		
		public bool Flag
        { 
		    get => GetPropertyValue<bool>("Flag");                         
			set => SetPropertyValue<bool>("Flag", value); 
			
        }
		//Tooltip for Object
		public object FlagToolTipControllerText(View view)
        {
        //    if (Flag != null) 
		//			return Flag;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFlag(View view = null)
        { 
			return Flag;
        }
		//Set Default Value
		public void SetDefaultFlag(View view = null)
        {
            //if (Flag is null){
            //    var result = GetDefaultFlag(view);
            //    if (result != null && result != Flag){
			//          Flag = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FlagIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFlag();
				//if (result != null && Flag != null){
				//	return !Flag.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(7)]		
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
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultObjectID(View view = null);
        //SetDefaultSystemType(View view = null);
        //SetDefaultField(View view = null);
        //SetDefaultVideo(View view = null);
        //SetDefaultFlag(View view = null);
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
			//	SetDefaultContent();
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
#region TranslateObjectImportCode
        [Browsable(false)]
        public System.Collections.Generic.IList<StringLookup> FieldSource
        {
            get
            {
                System.Collections.Generic.List<StringLookup> stringObjectList = new System.Collections.Generic.List<StringLookup>();
                if (this.SystemType != (Type)null)
                {
                    var members = XafTypesInfo.Instance.FindTypeInfo(this.SystemType).Members;
                    foreach (var member in members)
                        if (!member.IsReadOnly && member.MemberType == typeof(string) && (member.IsVisible || member.FindAttribute<DevExpress.ExpressApp.Security.SecurityBrowsableAttribute>() != null))
                        {
                            stringObjectList.Add(new StringLookup(DevExpress.ExpressApp.Utils.CaptionHelper.GetMemberCaption(member), member.Name));
                            //stringObjectList.Add(member.Name);
                        }
                }
                return (System.Collections.Generic.IList<StringLookup>)stringObjectList;
            }
        }
#endregion TranslateObjectImportCode
		 		 
    }
}
