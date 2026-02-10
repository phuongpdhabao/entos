using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.Validation;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.Xpo;

namespace ENTOS.Module.SystemObjects
{
    [NavigationItem("Default")]
    [ModelDefault("Caption", "Bộ lọc"), ImageName("FilterCriteria")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Appearance("Hide " + nameof(Field), TargetItems = nameof(Field) + "," + nameof(AllowInherit), Criteria = "IsListView = TRUE", Visibility = ViewItemVisibility.Hide, Context = nameof(DetailView))]
    [Appearance("Hide " + nameof(ViewId), TargetItems = nameof(ViewId) + "," + nameof(TypeCondition)+ "," + nameof(TargetViewNesting) + "," + nameof(DisplayOrder), Criteria = "IsListView = FALSE", Visibility = ViewItemVisibility.Hide, Context = nameof(DetailView))]
    //[Appearance("Disable " + nameof(ObjectType), TargetItems = nameof(ObjectType), Criteria = "ObjectType is not null", Enabled = false, Context = nameof(DetailView))]
    //[Appearance("xxx Validated", TargetItems = "xxx", Criteria = "xxxIsValidate", FontColor = "Red", Context = "DetailView"))]	
    [Appearance("Disable Delete", Criteria = "Active", AppearanceItemType = "Action", TargetItems = "Delete",Visibility = ViewItemVisibility.Hide)]
    [DefaultProperty(nameof(ObjectType))]
 
    [OptimisticLocking(true)]
    public partial class FilterCriteria:  DevExpress.Xpo.XPLiteObject, INoIndexColumn     //, HbBaseObject
    {

        public FilterCriteria(Session session)
            : base(session) {              
        }

		public string ToolTipControllerText()
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

		        //[Browsable(false)]
        //public bool AppearanceDisableDelete
        //{
        //    get
        //    {
        //        if (ConditionPayments != null && ConditionPayments.Count > 1)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}

		[Key(true), Browsable(false)]       
        public Guid Oid { get; set; }
               

		//private bool _islistview;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
		[VisibleInDetailView(true)]
		[VisibleInListView(true)]	 
		[VisibleInLookupListView(false)]
        [ModelDefault("PropertyEditorType", "RadioGroupBooleanPropertyEditor")]
        //[CustomRadioGroupValues("ListView", true)]
        //[CustomRadioGroupValues("DetailView", false)]                   
        [DevExpress.Xpo.DisplayName("ListView")]
		[Index(0)]
		[ToolTip("View")]
        [ImmediatePostData]
        public bool? IsListView
        { 
			get => GetPropertyValue<bool?>("IsListView");                         
			set => SetPropertyValue<bool?>("IsListView", value); 
        }
		//Tooltip for Object
		//Tooltip for Object
		//public string IsListViewToolTipControllerText()
        //{
        //    if (Name != null) return "Name: " + Name;
        //    return null;
        //}
		//Get Default Value
        public bool? GetDefaultIsListView()
        {
    
			return IsListView;
        }
		//Set Default Value
		public void SetDefaultIsListView()
        {
            IsListView = false;
        }
       
		//private Object _object;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)] 
		[VisibleInDetailView(true)]
		[VisibleInListView(true)]	 
		[VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Đối tượng")]
		[Index(1)]
		[ToolTip("Đối tượng")]
        [ValueConverter(typeof(TypeToStringConverter)), TypeConverter(typeof(SecurityTargetTypeConverter))]
        [Size(-1)]
        [RuleRequiredField]        
        [ImmediatePostData]       
		public Type ObjectType
        { 
			get => GetPropertyValue<Type>("ObjectType");                         
			set => SetPropertyValue<Type>("ObjectType", value); 
        }
		//Tooltip for Object
		//Tooltip for Object
		//public string ObjectToolTipControllerText()
        //{
        //    if (Name != null) return "Name: " + Name;
        //    return null;
        //}
		//Get Default Value
        public Object GetDefaultObject()
        {
    
			return ObjectType;
        }
		//Set Default Value
		public void SetDefaultObject()
        {
            
        }
       
		//private Field _field;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
		[VisibleInDetailView(true)]
		[VisibleInListView(true)]	 
		[VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trường")]
		[Index(2)]
		[ToolTip("Trường")]
        //[LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        //[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        [DataSourceProperty("FieldSource")]
        [ValueConverter(typeof(StringLookupToStringConverter))]
        [ImmediatePostData]
        [Size(100)]
        
        public StringLookup Field
        { 
			get => GetPropertyValue<StringLookup>("Field");                         
			set => SetPropertyValue<StringLookup>("Field", value); 
        }
      
        [Browsable(false)]
        public IList<StringLookup> FieldSource
        {
            get
            {
                List<StringLookup> stringObjectList = new List<StringLookup>();
                if (this.ObjectType != (Type) null)
                {
                    var members = XafTypesInfo.Instance.FindTypeInfo(this.ObjectType).Members;
                    foreach (var member in members)
                        if ((member.IsVisible ||
                             member.FindAttribute<DevExpress.ExpressApp.Security.SecurityBrowsableAttribute>() != null
                            ) && member.FindAttribute<DataSourceCriteriaPropertyAttribute>() != null)
                        {
                            stringObjectList.Add(new StringLookup(CaptionHelper.GetMemberCaption(member), member.Name));
                        }
                            
                }

                return (IList<StringLookup>)stringObjectList;
            }
        }
        //Tooltip for Object
        //Tooltip for Object
        //public string FieldToolTipControllerText()
        //{
        //    if (Name != null) return "Name: " + Name;
        //    return null;
        //}
        //Get Default Value
        public StringLookup GetDefaultField()
        {
    
			return Field;
        }
		//Set Default Value
		public void SetDefaultField()
        {
            
        }

        //private bool _islistview;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Cho phép kế thừa")]
        [Index(3)]
        [ToolTip("Các đối tượng thừa kế sẽ bị áp dụng điều kiện này")]
        [ImmediatePostData]
        public bool AllowInherit
        {
            get => GetPropertyValue<bool>("AllowInherit");
            set => SetPropertyValue<bool>("AllowInherit", value);
        }

        //private string _viewid;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
		[VisibleInDetailView(true)]
		[VisibleInListView(true)]	 
		[VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("ViewId")]
		[Index(4)]
		[ToolTip("View hoặc tên trường áp dụng vào tính năng cụ thể của chương trình")]
        //[DataSourceProperty("ViewIdSource")]
        //[ValueConverter(typeof(StringLookupToStringConverter))]
        //[EditorAlias(EditorAliases.LookupPropertyEditor)]
        [ModelDefault("PredefinedValues", "ListView;LookupListView")]
        [ImmediatePostData]
        [Size(100)]
		public string ViewId
        { 
			get => GetPropertyValue<string>("ViewId");                         
			set => SetPropertyValue<string>("ViewId", value); 
        }

        [Browsable(false)]
        public IList<StringLookup> ViewIdSource
        {
            get
            {
                List<StringLookup> stringObjectList = new List<StringLookup>();
                if (this.ObjectType != (Type)null)
                {
                    var members = XafTypesInfo.Instance.FindTypeInfo(this.ObjectType).Members;
                    foreach (var member in members)
                        if ((member.IsVisible ||
                             member.FindAttribute<DevExpress.ExpressApp.Security.SecurityBrowsableAttribute>() != null
                            ) && member.FindAttribute<DataSourceCriteriaPropertyAttribute>() != null)
                        {
                            stringObjectList.Add(new StringLookup(CaptionHelper.GetMemberCaption(member), member.Name));
                        }

                }

                return (IList<StringLookup>)stringObjectList;
            }
        }
   //     Tooltip for Object
   //     Tooltip for Object
   //     public string ViewIdToolTipControllerText()
   //     {
   //         if (Name != null) return "Name: " + Name;
   //         return null;
   //     }
   //     Get Default Value
   //     public StringLookup GetDefaultViewId()
   //     {
    
			//return ViewId;
   //     }
		//Set Default Value
		public void SetDefaultViewId()
        {
            
        }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Lọc theo")]
        [Index(6)]
        [ToolTip("Lọc theo")]
        [ValueConverter(typeof(TypeToStringConverter)), TypeConverter(typeof(SecurityTargetTypeConverter))]
        [Size(-1)]
        //[RuleRequiredField]
        //[ImmediatePostData]
        public Type TypeCondition
        {
            get => GetPropertyValue<Type>("TypeCondition");
            set => SetPropertyValue<Type>("TypeCondition", value);
        }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Hiển thị")]
        [Index(7)]
        [ToolTip("Hiển thị")]   
        public Nesting TargetViewNesting
        {
            get => GetPropertyValue<Nesting>("TargetViewNesting");
            set => SetPropertyValue<Nesting>("TargetViewNesting", value);
        }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Ưu tiên"), ToolTip("Ưu tiên")]
        [Index(8)]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "n0")]
        public int DisplayOrder
        {
            get => GetPropertyValue<int>("DisplayOrder");
            set => SetPropertyValue<int>("DisplayOrder", value);
        }

        //private bool _islistview;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Kích hoạt")]
        [Index(9)]
        [ToolTip("Kích hoạt")]
        //[ImmediatePostData]
        public bool Active
        {
            get => GetPropertyValue<bool>("Active");
            set => SetPropertyValue<bool>("Active", value);
        }

        //private string _condition;
        [DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
        [VisibleInDetailView(true)]
		[VisibleInListView(false)]	 
		[VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Điều kiện")]
		[Index(10)]
		[ToolTip("Điều kiện")]
 		[Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.CriteriaPropertyEditor), CriteriaOptions("TargetType")]                
        public string Condition
        { 
			get => GetPropertyValue<string>("Condition");                         
			set => SetPropertyValue<string>("Condition", value); 
        }

        [Browsable(false)]
        public Type TargetType
        {
            get
            {
                if (ObjectType != null)
                {
                    if (IsListView == true)
                    {
                        return this.ObjectType;
                    }
                    else if (Field != null)
                    {
                        var result = XafTypesInfo.Instance.FindTypeInfo(this.ObjectType)
                            .FindMember((string) Field.Value);
                        if (result != null && result.MemberType.IsClass)
                            return result.MemberType;
                    }                    
                }                
                return null;
            }
        }
        //Tooltip for Object
        //Tooltip for Object
        //public string ConditionToolTipControllerText()
        //{
        //    if (Name != null) return "Name: " + Name;
        //    return null;
        //}
        //Get Default Value
        public string GetDefaultCondition()
        {
    
			return Condition;
        }
		//Set Default Value
		public void SetDefaultCondition()
        {
            
        }

        [DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Code")]
        [Index(8)]
        [ToolTip("Code")]
        [NonPersistent]
        [Size(SizeAttribute.Unlimited)]
        public string ConditionCode
        {
            get => Condition;
            set => Condition = value;
        }

        [DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Mô tả")]
        [Index(9)]
        public string Description
        {
            get => GetPropertyValue<string>("Description");
            set => SetPropertyValue<string>("Description", value);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        SetDefaultIsListView();
        SetDefaultObject();
        SetDefaultField();
        SetDefaultViewId();
        SetDefaultCondition();
			//Condition = Tools.GetValue(Session, Tools.GetModuleName(GetType()), "Condition");
        }
        
        protected override void OnLoading()
        {
            base.OnLoading();
        }
        
        protected override void OnLoaded()
        {
            base.OnLoaded();
        }
        
        protected override void OnSaving()
        {
            base.OnSaving();
			if (!(Session is NestedUnitOfWork)&& (Session.DataLayer != null))
            {
				//if (Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
                //{
                //    //Khi đang mở Object
                //}
                //else if ((Session.ObjectLayer is DevExpress.Xpo.SimpleObjectLayer))
                //{
                //    //Từ popup form con về form chính
                //}
            }
        }
        
        protected override void OnSaved()
        {
            base.OnSaved();
        }

		protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsSaving)
            {
                if (newValue != null)
                {
                }                    
            }
        }

		protected override XPCollection<T> CreateCollection<T>(DevExpress.Xpo.Metadata.XPMemberInfo property)
        {
            var collection = base.CreateCollection<T>(property);
            collection.ListChanged += OnItemListChanged;
            return collection;
        }

        private void OnItemListChanged(object sender, ListChangedEventArgs e)
        {            
            //if (e.ListChangedType == ListChangedType.ItemAdded)
            //{
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        }


		#region Check Validate Value
        [Browsable(false)]
        public bool IsListViewIsValidate
        {
            get
            {
				//var result = GetDefaultIsListView();
				//if (result != null && IsListView != null){
				//	IsListView.Equals(result);
				//} 
                return true;
            }
        }
        [Browsable(false)]
        public bool ObjectTypeIsValidate
        {
            get
            {
				//var result = GetDefaultObject();
				//if (result != null && Object != null){
				//	Object.Equals(result);
				//} 
                return true;
            }
        }
        [Browsable(false)]
        public bool FieldIsValidate
        {
            get
            {
				//var result = GetDefaultField();
				//if (result != null && Field != null){
				//	Field.Equals(result);
				//} 
                return true;
            }
        }
        [Browsable(false)]
        public bool ViewIdIsValidate
        {
            get
            {
				//var result = GetDefaultViewId();
				//if (result != null && ViewId != null){
				//	ViewId.Equals(result);
				//} 
                return true;
            }
        }
        [Browsable(false)]
        public bool ConditionIsValidate
        {
            get
            {
				//var result = GetDefaultCondition();
				//if (result != null && Condition != null){
				//	Condition.Equals(result);
				//} 
                return true;
            }
        }
		#endregion
		 
    }
}