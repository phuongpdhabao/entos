using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;


namespace ENTOS.Module.SystemObjects
{
    [NavigationItem("Default")]
    [ModelDefault("Caption", "Chat AI"), ImageName("ChatAI")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Context = "DetailView", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete",Visibility = ViewItemVisibility.Hide)]

    //[OptimisticLocking(false)]
    public partial class ChatAI : GlobalFunctionInListView, INoIndexColumn     //, HbBaseObject
    {

        public ChatAI(Session session)
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




        //private string _name;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Tên"), ToolTip("Tên")]
        [RuleRequiredField]
        [Index(5)]		
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

        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Loại AI"), ToolTip("Loại AI")]
        [Index(6)]
        public AIType AIType
        {
            get => GetPropertyValue<AIType>("AIType");
            set => SetPropertyValue<AIType>("AIType", value);

        }

        ////private string _condition;
        //[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        //[VisibleInDetailView(true)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        //[DevExpress.Xpo.DisplayName("Điều kiện")]
        //[Index(6)]
        //[ToolTip("Điều kiện theo ngữ cảnh của form đang mở")]
        //[Size(SizeAttribute.Unlimited)]
        //[EditorAlias(EditorAliases.PopupCriteriaPropertyEditor), CriteriaOptions("ObjectType")]
        //[ModelDefault("RowCount", "1")]
        //public string Condition
        //{
        //    get => GetPropertyValue<string>("Condition");
        //    set => SetPropertyValue<string>("Condition", value);
        //}

        //private Field _field;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Đầu vào 1")]
        [Index(7)]
        [ToolTip("Đầu vào 1")]
        //[LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        //[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        [DataSourceProperty("FieldSource")]
        [ValueConverter(typeof(StringLookupToStringConverter))]
        [ImmediatePostData]
        [Size(150)]

        public StringLookup InputField1
        {
            get => GetPropertyValue<StringLookup>("InputField1");
            set => SetPropertyValue<StringLookup>("InputField1", value);
        }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Đầu vào 2")]
        [Index(8)]
        [ToolTip("Đầu vào 2")]
        //[LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        //[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        [DataSourceProperty("FieldSource")]
        [ValueConverter(typeof(StringLookupToStringConverter))]
        [ImmediatePostData]
        [Size(150)]

        public StringLookup InputField2
        {
            get => GetPropertyValue<StringLookup>("InputField2");
            set => SetPropertyValue<StringLookup>("InputField2", value);
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
                    {
                        if (member.IsPublic || member.FindAttribute<DevExpress.ExpressApp.Security.SecurityBrowsableAttribute>() != null)
                        {
                            if (member.MemberType.IsSubclassOf(typeof(PersistentBase)))
                            {
                                var childMembers = XafTypesInfo.Instance.FindTypeInfo(member.MemberType).Members;
                                foreach (var childMember in childMembers)
                                    if (childMember.IsPublic || childMember.FindAttribute<DevExpress.ExpressApp.Security.SecurityBrowsableAttribute>() != null)
                                    {
                                        if (childMember.MemberType == typeof(string))
                                        {
                                            stringObjectList.Add(new StringLookup(CaptionHelper.GetMemberCaption(member) + "." + CaptionHelper.GetMemberCaption(childMember), member.Name + "." + childMember.Name));
                                        }
                                    }
                            }
                            else if(!member.IsList)
                            {
                                stringObjectList.Add(new StringLookup(CaptionHelper.GetMemberCaption(member), member.Name));
                            }                            

                        }
                    }
                }
                return (IList<StringLookup>)stringObjectList;
            }
        }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Trường kết quả")]
        [Index(9)]
        [ToolTip("Trường kết quả")]
        //[LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        //[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        [DataSourceProperty("FieldSource")]
        [ValueConverter(typeof(StringLookupToStringConverter))]
        [ImmediatePostData]
        [Size(150)]

        public StringLookup ResultField
        {
            get => GetPropertyValue<StringLookup>("ResultField");
            set => SetPropertyValue<StringLookup>("ResultField", value);
        }

        public StringLookup GetDefaultField()
        {

            return null;
        }
        //Set Default Value
        public void SetDefaultField()
        {

        }


        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Nội dung"), ToolTip("Nội dung cần hỗ trợ, giá trị {0} và {1} sẽ thay thế giá trị đầu vào 1, 2")]
        [RuleRequiredField]
        [Index(10)]
        [Size(-1)]
        [ModelDefault("RowCount","1")]
        public string Content
        {
            get => GetPropertyValue<string>("Content");
            set => SetPropertyValue<string>("Content", value);

        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            //SetDefaultObjectType();
            //SetDefaultField();
            //SetDefaultUser();
            //Condition = Tools.GetValue(Session, Tools.GetModuleName(GetType()), "Condition");
            Content = "{0}{1}";
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

    public enum AIType
    {
        [DevExpress.ExpressApp.DC.XafDisplayName("Google Gemini")] Gemini = 0,
        [DevExpress.ExpressApp.DC.XafDisplayName("Chat GPT")] ChatGPT = 1,
        [DevExpress.ExpressApp.DC.XafDisplayName("Google Search")] GoolgeSearch = 2,
    }

}