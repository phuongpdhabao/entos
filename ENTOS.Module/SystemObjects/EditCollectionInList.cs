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
using System.Linq;


namespace ENTOS.Module.SystemObjects
{
    //[DefaultClassOptions]
    [NavigationItem("Default")]
    [ModelDefault("Caption", "Đa liên kết"), ImageName("Link_LinkUnLink")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide AutoSave", TargetItems = "AutoSave", Criteria = "IsNullOrEmpty(ViewId)", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    //[Appearance("Hide CallDefaultMethod", TargetItems = "CallDefaultMethod", Criteria = "Not IsNullOrEmpty(Condition)", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]
    //[Appearance("Disable Delete", Context = "DetailView", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide)]
    //[MapInheritance(MapInheritanceType.OwnTable)]
    //[OptimisticLocking(false)]
    //[Persistent]
    public class EditCollectionInList : GlobalFunctionInListView
    {

        public EditCollectionInList(Session session)
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
            get { return false; }
        }

        //private Field _field;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Trường")]
        [Index(3)]
        [ToolTip("Trường")]
        //[LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        //[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        [DataSourceProperty("FieldSource")]
        [ValueConverter(typeof(StringLookupToStringConverter))]
        [ImmediatePostData]
        [Size(100)]
        [RuleRequiredField]

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
                        if ((member.IsPersistent || member.IsList || member.FindAttribute<VisibleInDetailViewAttribute>() != null) && (member.IsVisible ||
                                                                       member
                                                                           .FindAttribute<
                                                                               DevExpress.ExpressApp.Security.
                                                                               SecurityBrowsableAttribute>() != null))
                        {
                            stringObjectList.Add(new StringLookup(CaptionHelper.GetMemberCaption(member), member.Name));
                            if (member.IsList)
                            {
                                var childMembers = XafTypesInfo.Instance.FindTypeInfo(member.ListElementType).Members;
                                foreach (var childMember in childMembers)
                                    if (!childMember.IsList 
                                        && (childMember.IsPersistent || childMember.FindAttribute<VisibleInDetailViewAttribute>() != null)
                                        && (member.IsVisible || member.FindAttribute<DevExpress.ExpressApp.Security.SecurityBrowsableAttribute >() != null))
                                    {
                                        stringObjectList.Add(new StringLookup(CaptionHelper.GetMemberCaption(member),
                                            string.Format("{0}.{1}", member.Name, childMember.Name)));
                                    }
                            }
                        }

                }
                return (IList<StringLookup>)stringObjectList;
            }
        }

  
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Điều kiện")]
        [Index(5)]
        [ToolTip("Điều kiện theo ngữ cảnh của form đang mở")]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor), CriteriaOptions("FieldType")]
        [ModelDefault("RowCount", "1")]
        public string Condition
        {
            get => GetPropertyValue<string>("Condition");
            set => SetPropertyValue<string>("Condition", value);
        }

        [Browsable(false)]
        public Type FieldType
        {
            get
            {
                var member = GetFieldInfo();
                if (member != null)
                {
                    if(member.IsList)
                        return member.ListElementType;
                    else
                        return member.MemberType;
                }
                    
                return null;
            }
        }

        public IMemberInfo GetFieldInfo()
        {
            if (ObjectType != null && Field != null)
            {
                var fieldValue = Field.Value as string;
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    if (fieldValue.Contains(".")){
                        var fieldValueArray = fieldValue.Split('.', StringSplitOptions.RemoveEmptyEntries);
                        var currentMember = XafTypesInfo.Instance.FindTypeInfo(this.ObjectType).FindMember(fieldValueArray[0]);
                        if (currentMember != null && fieldValueArray.Length >= 2)
                        {
                            var childMember = currentMember.IsList ? currentMember.ListElementTypeInfo.FindMember(fieldValueArray[1]) : 
                                                currentMember.MemberTypeInfo.FindMember(fieldValueArray[1]);
                            if(childMember != null)
                            {
                                if (fieldValueArray.Length >= 3)
                                    return childMember.IsList ? childMember.ListElementTypeInfo.FindMember(fieldValueArray[2]) : 
                                        childMember.MemberTypeInfo.FindMember(fieldValueArray[2]);
                                else
                                    return childMember;
                            }
                        }
                        return currentMember;
                    }
                    else
                    {
                        return XafTypesInfo.Instance.FindTypeInfo(this.ObjectType).FindMember(Field.Value as string);
                    }
                       
                }
                               
            }
            return null;
        }

        //[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        //[VisibleInDetailView(true)]
        //[VisibleInListView(true)]
        //[VisibleInLookupListView(false)]
        //[DevExpress.Xpo.DisplayName("Hủy liên kết")]
        //[Index(9)]
        //[ToolTip("Hủy liên kết")]
        ////[ImmediatePostData]
        //public bool AllowUnlink
        //{
        //    get => GetPropertyValue<bool>("AllowUnlink");
        //    set => SetPropertyValue<bool>("AllowUnlink", value);
        //}

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            //SetDefaultObjectType();
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

    }
}
