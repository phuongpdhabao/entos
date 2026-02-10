using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.ConditionalAppearance;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.Security;
using DevExpress.ExpressApp.DC;

namespace ENTOS.Module.SystemObjects
{
    [ModelDefault("Caption", "Popup")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Appearance("Show Only Text", Context = "DetailView", TargetItems = "*, OriginText, ReplaceText, Prefix, Suffix, RemovePrefix, RemoveSuffix, UpperLowerText, ConvertString", Criteria = "AppearanceType = 0", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Show Only Date", Context = "DetailView", TargetItems = "*, Date, DaysAdd", Criteria = "AppearanceType = 1", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Show Only Number", Context = "DetailView", TargetItems = "*, Number, AddNumber", Criteria = "AppearanceType = 2", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Show Only Logic", Context = "DetailView", TargetItems = "*, Logic", Criteria = "AppearanceType = 3", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Show Only EnumObject", Context = "DetailView", TargetItems = "*, EnumObject", Criteria = "AppearanceType = 4", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Show Only Type", Context = "DetailView", TargetItems = "*, ObjectType", Criteria = "AppearanceType = 5", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Show Only TimeSpan", Context = "DetailView", TargetItems = "*, TimeSpan", Criteria = "AppearanceType = 6", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Show Only Color", Context = "DetailView", TargetItems = "*, Color", Criteria = "AppearanceType = 7", Visibility = ViewItemVisibility.Hide)]
    //Cho phép chỉnh sửa
    //[Appearance("Disable UpperAllText LowerText", Context = "DetailView", TargetItems = nameof(UpperAllText) + "," + nameof(LowerText), Criteria = "UpperText", Enabled = false)]
    //[Appearance("Disable UpperText LowerText", Context = "DetailView", TargetItems = nameof(UpperText) + "," + nameof(LowerText), Criteria = "UpperAllText", Enabled = false)]
    //[Appearance("Disable UpperAllText UpperText", Context = "DetailView", TargetItems = nameof(UpperAllText) + "," + nameof(UpperText), Criteria = "LowerText", Enabled = false)]
    //[Appearance("Disable Edit TextSuffix", Context = "DetailView", TargetItems = nameof(Suffix), Criteria = "Not IsNullOrEmpty(ReplaceText)", Enabled = false)]
    //[Appearance("Disable Edit Date", Context = "DetailView", TargetItems = nameof(Date), Criteria = "DaysAdd is not null", Enabled = false)]
    //[Appearance("Disable Edit DaysAdd", Context = "DetailView", TargetItems = nameof(DaysAdd), Criteria = "Date is not null", Enabled = false)]
    //[Appearance("Disable Edit Number", Context = "DetailView", TargetItems = nameof(Number), Criteria = "AddNumber is not null", Enabled = false)]
    //[Appearance("Disable Edit AddNumber", Context = "DetailView", TargetItems = nameof(AddNumber), Criteria = "Number is not null", Enabled = false)]    
    //[Appearance("xxx Validated", TargetItems = "xxx", Criteria = "xxxIsValidate", FontColor = "Red", Context = "DetailView"))]	
    //[Appearance("Disable Delete", Context = "DetailView", Criteria = "BizQuotationItems.Count > 1", AppearanceItemType = "Action", TargetItems = "Delete",Visibility = ViewItemVisibility.Hide)]

    //[OptimisticLocking(false)]
    [NonPersistent]
    public class PopupControlText : INoIndexColumn     //, HbBaseObject
    {

        public PopupControlText(Type inputType)
        {
            InputType = inputType;
        }

        [Browsable(false)]
        public Type InputType { get; set; }

        [Browsable(false)]
        public int AppearanceType
        {
            get
            {
                //Kiểu ngày
                if (InputType == typeof(DateTime) || InputType == typeof(DateTime?))
                    return 1;
                //Kiểu số
                if (InputType == typeof(int) || InputType == typeof(int?) || InputType == typeof(decimal) ||
                    InputType == typeof(decimal?) || InputType == typeof(float) || InputType == typeof(float?))
                    return 2;
                //Kiểu logic
                if (InputType == typeof(bool) || InputType == typeof(bool?))
                    return 3;
                //Kiểu Enum
                if (InputType.IsEnum)
                    return 4;
                if (InputType.FullName == "System.Type")
                    return 5;
                if (InputType == typeof(TimeSpan) || InputType == typeof(TimeSpan?))
                    return 6;
                if (InputType == typeof(System.Drawing.Color) || InputType == typeof(System.Drawing.Color?))
                    return 7;
                if (InputType.Name.StartsWith("Nullable"))
                {
                    Type refType = Nullable.GetUnderlyingType(InputType);
                    if (refType != null && refType.IsEnum)
                    {
                        InputType = refType;
                        return 4;
                    }
                }

                //string 
                return 0;
            }
        }
        //public Inventory(Session session)
        //    : base(session) {              
        //}
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Từ gốc")]
        //[ImmediatePostData]
        public string OriginText { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Thay bằng")]
        //[ImmediatePostData]
        public string ReplaceText { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        //[Browsable(false)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Thêm trước")]
        [ToolTip("Thêm phía trước")]
        //[ImmediatePostData]
        public string Prefix { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]

        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Thêm sau")]
        [ToolTip("Thêm phía sau")]
        //[ImmediatePostData]
        public string Suffix { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Xóa trước")]
        [ToolTip("Xóa phía trước")]
        //[ImmediatePostData]
        public string RemovePrefix { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Xóa sau")]
        [ToolTip("Xóa phía sau")]
        //[ImmediatePostData]
        public string RemoveSuffix { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Chỉnh hoa")]
        [ToolTip("Chỉnh viết hoa")]
        public UpperLowerText UpperLowerText { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Sửa từ")]
        [ToolTip("Sửa từ")]
        public ConvertString ConvertString { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Ngày thay thế")]
        [ToolTip("Ngày thay thế")]
        [ModelDefault("DisplayFormat", "d/M/yyyy")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        //[ImmediatePostData]
        public DateTime? Date { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Thêm ngày")]
        [ToolTip("Trong trường hợp trừ thì dùng số âm")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "n0")]
        //[ImmediatePostData]
        public int? DaysAdd { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Số thay thế")]
        [ToolTip("Số thay thế")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "n2")]
        //[ImmediatePostData]
        public decimal? Number { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Cộng số")]
        [ToolTip("Trong trường hợp trừ thì dùng số âm")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "n2")]
        //[ImmediatePostData]
        public decimal? AddNumber { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Chọn")]
        [ToolTip("Chọn")]
        //[ImmediatePostData]
        [DataSourceProperty("EnumObjectOptions")]
        [ValueConverter(typeof(StringLookupToStringConverter))]
        public StringLookup EnumObject { get; set; }

        private List<StringLookup> EnumObjectOptions
        {
            get
            {
                List<StringLookup> stringObjectList = new List<StringLookup>();
                if (this.InputType != (Type)null && InputType.IsEnum)
                {
                    var listValues = Enum.GetValues(InputType);
                    foreach (var enumValue in listValues)
                    {
                        stringObjectList.Add(new StringLookup(Module.Helpers.XafXpoHelper.GetCaptionEnum(this.InputType, enumValue),
                            enumValue));
                    }

                }
                return stringObjectList;
            }
        }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Logic")]
        [ToolTip("Giá trị True/False")]
        //[ImmediatePostData]
        public bool? Logic { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Đối tượng"), ToolTip("Đối tượng")]
        [LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        [ValueConverter(typeof(TypeToStringConverter)), TypeConverter(typeof(SecurityTargetTypeConverter))]
        [Size(-1)]
        public System.Type ObjectType { get; set; }


        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Thời gian")]
        [ToolTip("Giá trị thời gian")]
        //[ImmediatePostData]
        [ModelDefault("MaskSettings", "AgAAAA9NYXNrTWFuYWdlclR5cGUAgwFEZXZFeHByZXNzLkRhdGEuTWFzay5UaW1lU3Bhbk1hc2tNYW5hZ2VyLCBEZXZFeHByZXNzLkRhdGEudjIyLjEsIFZlcnNpb249MjIuMS4zLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjg4ZDE3NTRkNzAwZTQ5YQRtYXNrBwIMW2QuXWhoOm1tOnNz")]
        public TimeSpan? TimeSpan { get; set; }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Màu sắc"), ToolTip("Màu sắc")]
        [LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        [ValueConverter(typeof(DevExpress.ExpressApp.StateMachine.Xpo.NullableColorConverter))]
        [Size(-1)]
        public System.Drawing.Color? Color { get; set; }

    }

    public enum UpperLowerText
    {
        [XafDisplayName(" ")] None = 0,
        [XafDisplayName("Đầu hoa")] Upper = 1,
        [XafDisplayName("Toàn hoa")] UpperAll = 2,
        [XafDisplayName("Bỏ hoa")] Lower = 3,
    }

    public enum ConvertString
    {
        [XafDisplayName(" ")] None = 0,
        [XafDisplayName("Escape (Thay ký tự đường dẫn)")] Escape = 1,
        [XafDisplayName("Unescape (Giải mã ký tự đường dẫn)")] Unescape = 2,
        [XafDisplayName("Bỏ dấu")] RemoveUnicode = 3,
        [XafDisplayName("Bỏ ký tự đặc biệt")] RemoveSpecialCharacters = 4,
        [XafDisplayName("Chỉ giữ số")] KeepNumber = 5
    }
}