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
    [ModelDefault("Caption", "Nhập dữ liệu excel"), ImageName("ExcelImport")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]    
    //[Appearance("Hide Condition", TargetItems = "Condition", Criteria = "CallDefaultMethod", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    //[Appearance("Hide CallDefaultMethod", TargetItems = "CallDefaultMethod", Criteria = "Not IsNullOrEmpty(Condition)", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Context = "DetailView", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete",Visibility = ViewItemVisibility.Hide)]

    //[OptimisticLocking(false)]
    public partial class ImportExportFromExcel : GlobalFunctionInListView     //, HbBaseObject
    {

        public ImportExportFromExcel(Session session)
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
				[Browsable(false)]
        public bool AppearanceDisableDelete
        {
            get
            {

                                
                return false;
            }
        }
        //      //private HBF.BIZ.BusinessObjects.Member _user;
        //      [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]

        //[VisibleInDetailView(true)]
        //[VisibleInListView(false)]	 
        //[VisibleInLookupListView(false)]

        //[DevExpress.Xpo.DisplayName("Người dùng"),ToolTip("Người dùng")]
        ////[Index(2)]

        //[LookupEditorMode(LookupEditorMode.Auto)]
        ////[ModelDefault("LookupProperty", "")]
        //[DataSourceCriteriaProperty("UserCriteria")]
        ////[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        ////[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
        ////[DevExpress.Xpo.Association]
        ////[NoForeignKey]

        //public HBF.BIZ.BusinessObjects.Bu User
        //      { 
        //	get => GetPropertyValue<HBF.BIZ.BusinessObjects.Member>("User");                         
        //	set => SetPropertyValue<HBF.BIZ.BusinessObjects.Member>("User", value); 
        //      }
        ////Tooltip for Object
        ////public string UserToolTipControllerText()
        //      //{
        //      //    if (User != null) 
        ////			return User;
        //      //    return null;
        //      //}
        ////Get Default Value
        //      public HBF.BIZ.BusinessObjects.Member GetDefaultUser()
        //      {

        //	return User;
        //      }
        ////Set Default Value
        //public void SetDefaultUser()
        //      {

        //      }
        ////Check Not Validate
        //protected bool UserIsNotValidate
        //      {
        //          get
        //          {
        //		//var result = GetDefaultUser();
        //		//if (result != null && User != null){
        //		//	return !User.Equals(result);
        //		//} 
        //              return false;
        //          }
        //      }

        //private CriteriaOperator UserCriteria
        //      {
        //          get
        //          {
        //              return Tools.GetCriteriaOperator(this.GetType(), nameof(User));
        //          }
        //      }

        //private Field _field;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Đối tượng chính")]
        [Index(5)]
        [ToolTip("Đối tượng chính sẽ được tạo cùng")]
        //[LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        //[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        [DataSourceProperty("FieldSource")]
        [ValueConverter(typeof(StringLookupToStringConverter))]
        [ImmediatePostData]
        [Size(100)]

        public StringLookup AutoCreateObject
        {
            get => GetPropertyValue<StringLookup>("AutoCreateObject");
            set => SetPropertyValue<StringLookup>("AutoCreateObject", value);
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
                        if (member.IsPersistent && !member.IsReadOnly && (member.IsVisible || member.FindAttribute<DevExpress.ExpressApp.Security.SecurityBrowsableAttribute>() != null))
                        {
                            stringObjectList.Add(new StringLookup(CaptionHelper.GetMemberCaption(member), member.Name));
                        }

                }

                return (IList<StringLookup>)stringObjectList;
            }
        }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Khóa trùng")]
        [ToolTip("Trường kiểm tra trùng dữ liệu")]
        [Index(6)]
        [Size(200)]
        //[EditorAlias(EditorAliases.PopupExpressionPropertyEditor), CriteriaOptions("ObjectType")]
        [ModelDefault("PredefinedValues", "Code")]
        //[ImmediatePostData]
        public string KeyField
        {
            get { return GetPropertyValue<string>(nameof(KeyField)); }
            set { SetPropertyValue(nameof(KeyField), value); }
        }



        [DetailViewLayout("Tab", LayoutGroupType.TabbedGroup, 5)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Trường tham chiếu")]
        [ToolTip("Trường tham chiếu")]
        [Association]
        [DevExpress.Xpo.Aggregated] 
        public XPCollection<MappingField> MappingFields
        {
            get { return GetCollection<MappingField>(nameof(MappingFields)); }
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
		 
    }

    [ModelDefault("Caption", "Tham chiếu cột Excel")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]  
    [DefaultProperty(nameof(RootField))]
    //[RuleCombinationOfPropertiesIsUnique("ImportExportFromExcelMappingField", DefaultContexts.Save, "ImportExportFromExcel, RootField")]
    //[Appearance("Code Is Not Validated", TargetItems = nameof(Name), Criteria = nameof(NameIsNotValidated), FontColor = "Red")]
    [OptimisticLocking(true)]
    public class MappingField : XPLiteObject, INoIndexColumn//, IDisableDetailViewInListView
    {
        public MappingField(Session session) : base(session)
        {
        }

        [Key(true), Browsable(false)] public Guid Oid { get; set; }

        [DetailViewLayout(LayoutColumnPosition.Left, " ", 0)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [ModelDefault("Caption", "Nhập dữ liệu excel")]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        [ModelDefault("DefaultListViewNewItemRowPosition", "None")]
        [Association]
        [RuleRequiredField]
        [ModelDefault("AllowEdit", "False")]
        public ImportExportFromExcel ImportExportFromExcel
        {
            get => GetPropertyValue<ImportExportFromExcel>("ImportExportFromExcel");
            set => SetPropertyValue<ImportExportFromExcel>("ImportExportFromExcel", value);
        }


        [DetailViewLayout(LayoutColumnPosition.Left, " ", 0)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Trường")]
        [ToolTip("Trường")]
        [Size(SizeAttribute.Unlimited)]
        [ModelDefault("RowCount", "1")]
        //[ModelDefault("PropertyEditorType", "StringComboEditor")]
        [DataSourceProperty("AvailablePropertyNames")]
        //[ModelDefault("PredefinedValues", "Code")]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor), CriteriaOptions("ImportExportFromExcel.ObjectType")]
        [ImmediatePostData]
        [RuleRequiredField]
        public string RootField
        {
            get { return GetPropertyValue<string>(nameof(RootField)); }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Contains(" = ?"))
                {
                    //value = value.Replace("?", "");
                    value = value.Replace(" = ?", "");
                    value = value.Trim(new char[] {'[', ']'});
                }
                SetPropertyValue(nameof(RootField), value);
            }
        }


        [Browsable(false)]
        public IList<string> AvailablePropertyNames
        {
            get
            {
                if (this.ImportExportFromExcel != null && this.ImportExportFromExcel.ObjectType != null)
                {
                    List<string> stringList = new List<string>();
                    ITypeInfo typeInfo = XafTypesInfo.Instance.FindTypeInfo(this.ImportExportFromExcel.ObjectType);
                    if (typeInfo != null)
                    {
                        foreach (IMemberInfo member in typeInfo.Members)
                        {
                            if (member.IsPublic)
                                stringList.Add(member.Name);
                        }
                    }
                    return (IList<string>)stringList;
                }
                return null;
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Mã trường")]
        [ToolTip("Mã trường")]
        [NonPersistent]
        public string FieldCode
        {
            get { return GetPropertyValue<string>(nameof(RootField)); }
            set {SetPropertyValue(nameof(RootField), value);}
        }

        [DetailViewLayout(LayoutColumnPosition.Left, " ", 0)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Tên thay thế")]
        [ToolTip("Tên")]
        [RuleRequiredField]
        [Size(100)]
        public string Name
        {
            get { return GetPropertyValue<string>(nameof(Name)); }
            set { SetPropertyValue(nameof(Name), value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public bool NameIsNotValidated
        {
            get
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    if (AvailablePropertyNames.Contains(Name))
                        return true;
                }
                return false;
            }
        }

        [DetailViewLayout(LayoutColumnPosition.Left, " ", 0)]

        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Đối tượng con"), ToolTip("Đối tượng con")]     
        public bool IsSubObject
        {
            get => GetPropertyValue<bool>("IsSubObject");
            set => SetPropertyValue<bool>("IsSubObject", value);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsSaving)
            {
                
                //var fd = this.Session;

            }
        }

    }
}