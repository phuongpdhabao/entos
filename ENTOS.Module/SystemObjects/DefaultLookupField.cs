using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;


namespace ENTOS.Module.SystemObjects
{
    [NavigationItem("Default")]
    [ModelDefault("Caption", "Giá trị Lookup"), ImageName("DefaultLookupField")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Condition", TargetItems = "Condition", Criteria = "CallDefaultMethod", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    //[Appearance("Hide CallDefaultMethod", TargetItems = "CallDefaultMethod", Criteria = "Not IsNullOrEmpty(Condition)", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Context = "DetailView", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete",Visibility = ViewItemVisibility.Hide)]
    [ModelDefault("ToolTip", "Dùng cho tính năng paste từ clipboard và nhập excel")]
    [OptimisticLocking(true)]
    public partial class DefaultLookupField : DevExpress.Xpo.XPLiteObject, INoIndexColumn     //, HbBaseObject
    {

        public DefaultLookupField(Session session)
            : base(session)
        {
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
        [Browsable(false)]
        public bool AppearanceDisableDelete
        {
            get
            {


                return false;
            }
        }

        [Key(true), Browsable(false)]
        public Guid Oid { get; set; }


        //private System.Type _objecttype;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]

        [VisibleInDetailView(true)]
        [VisibleInListView(true)]

        [VisibleInLookupListView(true)]

        [DevExpress.Xpo.DisplayName("Đối tượng"), ToolTip("Đối tượng")]
        [Index(0)]

        [LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        [DataSourceCriteriaProperty("ObjectTypeCriteria")]
        [ValueConverter(typeof(TypeToStringConverter)), TypeConverter(typeof(SecurityTargetTypeConverter))]
        [Size(-1)]
        //[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        //[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
        //[DevExpress.Xpo.Association]
        //[NoForeignKey]

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
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ObjectType));
            }
        }



        //private Field _field;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Trường")]
        [Index(5)]
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
                if (this.ObjectType != (Type)null)
                {
                    var members = XafTypesInfo.Instance.FindTypeInfo(this.ObjectType).Members;
                    foreach (var member in members)
                        if (member.IsPersistent && !member.IsReadOnly && !member.IsList && (member.IsVisible || member.FindAttribute<DevExpress.ExpressApp.Security.SecurityBrowsableAttribute>() != null))
                        {
                            stringObjectList.Add(new StringLookup(CaptionHelper.GetMemberCaption(member), member.Name));
                        }

                }

                return (IList<StringLookup>)stringObjectList;
            }
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            SetDefaultObjectType();
            //SetDefaultUser();
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
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null))
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


    }
}