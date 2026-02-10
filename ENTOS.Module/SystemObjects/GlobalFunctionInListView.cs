using System;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace ENTOS.Module.SystemObjects
{
    [NonPersistent]
    [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
    [Appearance("Hide AutoSave", TargetItems = nameof(AutoSave), Criteria = "IsNullOrEmpty(ViewId)", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    //[MemberDesignTimeVisibility(false)]
    [DefaultProperty(nameof(ObjectType))]
    [OptimisticLocking(true)]
    public abstract class GlobalFunctionInListView : DevExpress.Xpo.XPLiteObject, INoIndexColumn
    {
        public GlobalFunctionInListView(Session session)
            : base(session)
        {

        }
        [Key(true)][Browsable(false)] public Guid Oid { get; set; }

        // private System.Type _objectType;
        [DetailViewLayout(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Đối tượng"), ToolTip("Đối tượng")]
        [Index(0)]
        [LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        [DataSourceCriteriaProperty("ObjectTypeCriteria")]
        [ValueConverter(typeof(TypeToStringConverter)), TypeConverter(typeof(SecurityTargetTypeConverter))]
        [RuleRequiredField]
        [Size(-1)]

        public System.Type ObjectType
        {
            get => GetPropertyValue<System.Type>("ObjectType");
            set => SetPropertyValue<System.Type>("ObjectType", value);
        }

        //Tooltip for Object
        //public string ObjectTypeToolTipControllerText()
        //{
        //    if (ObjectType != null) 
        //			return ObjectType;
        //    return null;
        //}
        //Get Default Value
        public System.Type GetDefaultObjectType()
        {

            return ObjectType;
        }

        //Set Default Value
        public void SetDefaultObjectType()
        {

        }

        //Check Not Validate
        protected bool ObjectTypeIsNotValidate
        {
            get
            {
                //var result = GetDefaultObjectType();
                //if (result != null && ObjectType != null){
                //	return !ObjectType.Equals(result);
                //} 
                return false;
            }
        }

        private CriteriaOperator ObjectTypeCriteria
        {
            get { return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ObjectType)); }
        }

        //private string _viewId;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("ViewId")]
        [Index(1)]
        [ToolTip("View hoặc tên trường áp dụng vào tính năng cụ thể của chương trình")]
        //[DataSourceProperty("ViewIdSource")]
        //[ValueConverter(typeof(StringLookupToStringConverter))]
        //[EditorAlias(EditorAliases.LookupPropertyEditor)]
        [ModelDefault("PredefinedValues", "ListView;Nested_ListView")]
        //[RuleUniqueValue]
        //[ImmediatePostData]
        [Size(200)]
        public string ViewId
        {
            get => GetPropertyValue<string>("ViewId");
            set => SetPropertyValue<string>("ViewId", value);
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
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Tự động lưu")]
        [Index(9)]
        [ToolTip("Dữ liệu sau khi nhập sẽ tự động bị lưu")]
        //[ImmediatePostData]
        public bool AutoSave
        {
            get => GetPropertyValue<bool>("AutoSave");
            set => SetPropertyValue<bool>("AutoSave", value);
        }

        //private bool _active;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Kích hoạt")]
        [Index(10)]
        [ToolTip("Kích hoạt")]
        //[ImmediatePostData]
        public bool Active
        {
            get => GetPropertyValue<bool>("Active");
            set => SetPropertyValue<bool>("Active", value);
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Oid = Guid.NewGuid();
            //SetDefaultUser();
            //Condition = Tools.GetValue(Session, Tools.GetModuleName(GetType()), "Condition");
        }

    }
}
