using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.Security;
using DevExpress.Xpo;


namespace ENTOS.Module.SystemObjects
{
    [NavigationItem("Default")]
    [ModelDefault("Caption", "Nhập liệu"), ImageName("DataInput")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Appearance("Hide Condition", TargetItems = "Condition", Criteria = "CallDefaultMethod", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    [Appearance("Hide CallDefaultMethod", TargetItems = "CallDefaultMethod", Criteria = "Not IsNullOrEmpty(Condition)", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Context = "DetailView", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete",Visibility = ViewItemVisibility.Hide)]

    //[OptimisticLocking(false)]
    public partial class DataInput: GlobalFunctionInListView, INoIndexColumn     //, HbBaseObject
    {

        public DataInput(Session session)
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
        [RuleRequiredField]
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
                        if (!member.IsReadOnly && (member.IsVisible || member.FindAttribute<DevExpress.ExpressApp.Security.SecurityBrowsableAttribute>() != null))
                        {
                            stringObjectList.Add(new StringLookup(CaptionHelper.GetMemberCaption(member), member.Name));
                        }

                }

                return (IList<StringLookup>)stringObjectList;
            }
        }
        [Browsable(false)]
        public System.Type FieldType
        {
            get
            {
                if (this.ObjectType != null && Field != null)
                {
                    var member = XafTypesInfo.Instance.FindTypeInfo(this.ObjectType).FindMember(Field.Value as string);
                    if (member != null)
                        return member.MemberType;

                }
                return null;
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


        //[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        //[VisibleInDetailView(true)]
        //[VisibleInListView(true)]
        //[VisibleInLookupListView(false)]
        //[DevExpress.Xpo.DisplayName("Trường nguồn")]
        //[Index(4)]
        //[ToolTip("Chỉ định trường nguồn nếu muốn copy dữ liệu từ trường nguồn ra trường hiện tại")]
        ////[LookupEditorMode(LookupEditorMode.Auto)]
        ////[ModelDefault("LookupProperty", "")]
        ////[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        //[DataSourceProperty("FieldSource")]
        //[ValueConverter(typeof(StringLookupToStringConverter))]
        //[ImmediatePostData]
        //[Size(150)]

        //public StringLookup SourceField
        //{
        //    get => GetPropertyValue<StringLookup>("SourceField");
        //    set => SetPropertyValue<StringLookup>("SourceField", value);
        //}

        //private string _condition;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Biểu thức nguồn")]
        [Index(5)]
        [ToolTip("Lấy dữ liệu từ nguồn")]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        //[CriteriaOptions("ObjectType")]
        //[ModelDefault("PropertyEditorType", "ExtendedPopupExpressionPropertyEditor")]
        [DevExpress.ExpressApp.Core.ElementTypeProperty("ObjectType")]
        //[DataSourceCriteriaProperty("ObjectType")]
        [ModelDefault("RowCount", "1")]
        public string SourceCondition
        {            
            get => GetPropertyValue<string>("SourceCondition");
            set => SetPropertyValue<string>("SourceCondition", value);
        }

        //      //private ENTOS.BIZ.BusinessObjects.Member _user;
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

        //public ENTOS.BIZ.BusinessObjects.Bu User
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
        //      public ENTOS.BIZ.BusinessObjects.Member GetDefaultUser()
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
        //private string _name;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Tên"), ToolTip("Tên")]
        //[Index(1)]		
        [Size(150)]
        public string Name
        {
            get => GetPropertyValue<string>("Name");
            set => SetPropertyValue<string>("Name", value);

        }
        //Tooltip for Object
        public string NameToolTipControllerText(View view)
        {
            //    if (Name != null) 
            //			return Name;
            return null;
        }
        //Get Default Value
        public string GetDefaultName()
        {
            return Name;
        }
        //Set Default Value
        public void SetDefaultName()
        {
            //if (Name is null){
            //    var result = GetDefaultName();
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

        //private string _condition;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Điều kiện")]
        [Index(7)]
        [ToolTip("Điều kiện theo ngữ cảnh của form đang mở")]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor), CriteriaOptions("FieldType")]
        [ModelDefault("RowCount","1")]
        public string Condition
        {
            get => GetPropertyValue<string>("Condition");
            set => SetPropertyValue<string>("Condition", value);
        }


        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Xóa trắng")]
        [Index(8)]
        [ToolTip("Xóa trắng dữ liệu")]
        //[ImmediatePostData]
        public bool SetNull
        {
            get => GetPropertyValue<bool>("SetNull");
            set => SetPropertyValue<bool>("SetNull", value);
        }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Gọi phương thức")]
        [Index(9)]
        [ToolTip("Gọi phương thức sửa giá trị mặc định của trường này")]
        //[ImmediatePostData]
        public bool CallDefaultMethod
        {
            get => GetPropertyValue<bool>("CallDefaultMethod");
            set => SetPropertyValue<bool>("CallDefaultMethod", value);
        }



        public override void AfterConstruction()
        {
            base.AfterConstruction();
        SetDefaultObjectType();
        SetDefaultField();
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

   
}