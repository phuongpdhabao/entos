using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
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
    [ModelDefault("Caption", "Dịch"), ImageName("TranslateData")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Context = "DetailView", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete",Visibility = ViewItemVisibility.Hide)]

    //[OptimisticLocking(false)]
    public partial class TranslateData : GlobalFunctionInListView, INoIndexColumn     //, HbBaseObject
    {

        public TranslateData(Session session)
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
        [DevExpress.Xpo.DisplayName("Trường gốc")]
        [Index(7)]
        [ToolTip("Trường gốc")]
        //[LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        //[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        [DataSourceProperty("FieldSource")]
        [ValueConverter(typeof(StringLookupToStringConverter))]
        [ImmediatePostData]
        [Size(100)]

        public StringLookup RootField
        {
            get => GetPropertyValue<StringLookup>("RootField");
            set => SetPropertyValue<StringLookup>("RootField", value);
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
                            if (member.MemberType == typeof(string))
                            {
                                stringObjectList.Add(new StringLookup(CaptionHelper.GetMemberCaption(member), member.Name));
                            }else if(!member.IsList && member.MemberType.IsSubclassOf(typeof(PersistentBase)))
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
        [DevExpress.Xpo.DisplayName("Trường dịch")]
        [Index(8)]
        [ToolTip("Trường dịch")]
        //[LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        //[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        [DataSourceProperty("FieldSource")]
        [ValueConverter(typeof(StringLookupToStringConverter))]
        [ImmediatePostData]
        [Size(100)]

        public StringLookup TranslateField
        {
            get => GetPropertyValue<StringLookup>("TranslateField");
            set => SetPropertyValue<StringLookup>("TranslateField", value);
        }

        public StringLookup GetDefaultField()
        {

            return null;
        }
        //Set Default Value
        public void SetDefaultField()
        {

        }

        //private Field _field;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Ngôn ngữ gốc")]
        [Index(9)]
        [ToolTip("Nếu là tiếng anh thì để trống")]
        //[LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        //[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        [DataSourceProperty("LanguageSource")]
        [ValueConverter(typeof(StringLookupToStringConverter))]
        [ImmediatePostData]
        [Size(100)]

        public StringLookup LanguageOrigine
        {
            get => GetPropertyValue<StringLookup>("LanguageOrigine");
            set => SetPropertyValue<StringLookup>("LanguageOrigine", value);
        }

        [Browsable(false)]
        public IList<StringLookup> LanguageSource
        {
            get
            {
                List<StringLookup> stringObjectList = new List<StringLookup>();
                foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
                {
                    if(!string.IsNullOrEmpty(ci.Name) && !ci.Name.Contains("-"))
                    {
                        stringObjectList.Add(new StringLookup(ci.EnglishName, ci.Name));
                    }
                    //string specName = "(none)";
                    //try { specName = CultureInfo.CreateSpecificCulture(ci.Name).Name; } catch { }
                    //var code = String.Format("{0,-12}{1,-12}{2}", ci.Name, specName, ci.EnglishName);
                    //stringObjectList.Add(new StringLookup(ci.Name, code));
                }                
                return (IList<StringLookup>)stringObjectList;
            }
        }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Ngôn ngữ dịch")]
        [Index(10)]
        [ToolTip("Nếu là tiếng Việt thì để trống")]
        //[LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        //[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        [DataSourceProperty("LanguageSource")]
        [ValueConverter(typeof(StringLookupToStringConverter))]
        [ImmediatePostData]
        [Size(100)]

        public StringLookup LanguageTranslate
        {
            get => GetPropertyValue<StringLookup>("LanguageTranslate");
            set => SetPropertyValue<StringLookup>("LanguageTranslate", value);
        }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("API")]
        [Index(11)]
        //[Index(20)]		
        public TranslateApiOption TranslateApiOption
        {
            get => GetPropertyValue<TranslateApiOption>("TranslateApiOption");
            set => SetPropertyValue<TranslateApiOption>("TranslateApiOption", value);

        }


        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Hỗ trợ Html")]
        [Index(15)]
        [ToolTip("Nhấn vào tùy chọn này khi cần dịch tài liệu Html")]
        //[ImmediatePostData]
        public bool SupportHtml
        {
            get => GetPropertyValue<bool>("SupportHtml");
            set => SetPropertyValue<bool>("SupportHtml", value);
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

    public enum TranslateApiOption
    {
        [XafDisplayName("Google miễn phí")] GoogleFree = 0,
        [XafDisplayName("Google API")] GoogleTranslate = 1,
        [XafDisplayName("OpenAI")] OpenAI = 2,
        [XafDisplayName("Facebook")] Facebook = 3,
    }
}