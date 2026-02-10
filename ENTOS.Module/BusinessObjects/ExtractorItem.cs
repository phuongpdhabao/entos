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
    [ModelDefault("Caption", "Chi tiết Trích web"), ImageName("ExtractorItem")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("ExtractorItem Column, Row, Exact None_Disable__" , TargetItems = "Column, Row, Exact" , Criteria = "[InsideTable] = False",AppearanceItemType = "ViewItem", Enabled = false )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Order)+ "," + nameof(Column)+ "," + nameof(Row))]
 
	[MobileColumnAttribute(Context = "WebExtractor_ExtractorItemList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ExtractorItem_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ExtractorItem_LookupListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class ExtractorItem:  DevExpress.Xpo.XPLiteObject , IReOrder , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ExtractorItem(Session session)
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
               

		//private int? _order;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(0)]		
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
		//Set Default Value

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

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên cột")]
        [ToolTip("Tên cột")]
		//[Index(1)]		

 		[Size(150)]
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

	
       
		//private ExtractorType _extractortype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại dữ liệu")]
        [ToolTip("Loại dữ liệu")]
		//[Index(2)]		
		public ExtractorType ExtractorType
        { 
		    get => GetPropertyValue<ExtractorType>("ExtractorType");                         
			set => SetPropertyValue<ExtractorType>("ExtractorType", value); 
			
        }
		//Tooltip for Object
		public object ExtractorTypeToolTipControllerText(View view)
        {
        //    if (ExtractorType != null) 
		//			return ExtractorType;
            return null;
        }
		//Get Default Value
        public ExtractorType GetDefaultExtractorType(View view = null)
        { 
			return ExtractorType;
        }
		//Set Default Value
		public void SetDefaultExtractorType(View view = null)
        {
            //if (ExtractorType is null){
            //    var result = GetDefaultExtractorType(view);
            //    if (result != null && result != ExtractorType){
			//          ExtractorType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ExtractorTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultExtractorType();
				//if (result != null && ExtractorType != null){
				//	return !ExtractorType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _cssxpathvalue;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giá trị Css/Xpath")]
        [ToolTip("Giá trị Css/Xpath")]
		//[Index(3)]		

 		[Size(200)]
		public string CssXpathValue
        { 
		    get => GetPropertyValue<string>("CssXpathValue");                         
			set => SetPropertyValue<string>("CssXpathValue", value); 
			
        }
		//Tooltip for Object
		public object CssXpathValueToolTipControllerText(View view)
        {
        //    if (CssXpathValue != null) 
		//			return CssXpathValue;
            return null;
        }
		//Get Default Value
        public string GetDefaultCssXpathValue(View view = null)
        { 
			return CssXpathValue;
        }
		//Set Default Value
		public void SetDefaultCssXpathValue(View view = null)
        {
            //if (CssXpathValue is null){
            //    var result = GetDefaultCssXpathValue(view);
            //    if (result != null && result != CssXpathValue){
			//          CssXpathValue = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CssXpathValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCssXpathValue();
				//if (result != null && CssXpathValue != null){
				//	return !CssXpathValue.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.WebExtractor _webextractor;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trích xuất Web")]
        [ToolTip("Trích xuất Web")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(WebExtractorCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("WebExtractor-ExtractorItemList")]
	 
	    [Browsable(false)]
		public Module.BusinessObjects.WebExtractor WebExtractor
        { 
		    get => GetPropertyValue<Module.BusinessObjects.WebExtractor>("WebExtractor");                         
			set => SetPropertyValue<Module.BusinessObjects.WebExtractor>("WebExtractor", value); 
			
        }
		//Tooltip for Object
		public object WebExtractorToolTipControllerText(View view)
        {
        //    if (WebExtractor != null) 
		//			return WebExtractor;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.WebExtractor GetDefaultWebExtractor(View view = null)
        { 
			return WebExtractor;
        }
		//Set Default Value
		public void SetDefaultWebExtractor(View view = null)
        {
            //if (WebExtractor is null){
            //    var result = GetDefaultWebExtractor(view);
            //    if (result != null && result != WebExtractor){
			//          WebExtractor = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WebExtractorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWebExtractor();
				//if (result != null && WebExtractor != null){
				//	return !WebExtractor.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator WebExtractorCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(WebExtractor));
            }
        }
	
       
		//private string _attribute;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuộc tính")]
        [ToolTip("Thuộc tính")]
		//[Index(5)]		

 		[Size(200)]
		public string Attribute
        { 
		    get => GetPropertyValue<string>("Attribute");                         
			set => SetPropertyValue<string>("Attribute", value); 
			
        }
		//Tooltip for Object
		public object AttributeToolTipControllerText(View view)
        {
        //    if (Attribute != null) 
		//			return Attribute;
            return null;
        }
		//Get Default Value
        public string GetDefaultAttribute(View view = null)
        { 
			return Attribute;
        }
		//Set Default Value
		public void SetDefaultAttribute(View view = null)
        {
            //if (Attribute is null){
            //    var result = GetDefaultAttribute(view);
            //    if (result != null && result != Attribute){
			//          Attribute = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AttributeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAttribute();
				//if (result != null && Attribute != null){
				//	return !Attribute.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _password;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mật khẩu")]
        [ToolTip("Mật khẩu")]
		//[Index(6)]		

 		[Size(100)]
	    [PasswordPropertyText(true)]
		public string Password
        { 
		    get => GetPropertyValue<string>("Password");                         
			set => SetPropertyValue<string>("Password", value); 
			
        }
		//Tooltip for Object
		public object PasswordToolTipControllerText(View view)
        {
        //    if (Password != null) 
		//			return Password;
            return null;
        }
		//Get Default Value
        public string GetDefaultPassword(View view = null)
        { 
			return Password;
        }
		//Set Default Value
		public void SetDefaultPassword(View view = null)
        {
            //if (Password is null){
            //    var result = GetDefaultPassword(view);
            //    if (result != null && result != Password){
			//          Password = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PasswordIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPassword();
				//if (result != null && Password != null){
				//	return !Password.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _insidetable;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trong bảng")]
        [ToolTip("Trong bảng")]
		//[Index(7)]		
		public bool InsideTable
        { 
		    get => GetPropertyValue<bool>("InsideTable");                         
			set => SetPropertyValue<bool>("InsideTable", value); 
			
        }
		//Tooltip for Object
		public object InsideTableToolTipControllerText(View view)
        {
        //    if (InsideTable != null) 
		//			return InsideTable;
            return null;
        }
		//Get Default Value
        public bool GetDefaultInsideTable(View view = null)
        { 
			return InsideTable;
        }
		//Set Default Value
		public void SetDefaultInsideTable(View view = null)
        {
            //if (InsideTable is null){
            //    var result = GetDefaultInsideTable(view);
            //    if (result != null && result != InsideTable){
			//          InsideTable = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool InsideTableIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultInsideTable();
				//if (result != null && InsideTable != null){
				//	return !InsideTable.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _column;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cột")]
        [ToolTip("Cột")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Column
        { 
		    get => GetPropertyValue<int?>("Column");                         
			set => SetPropertyValue<int?>("Column", value); 
			
        }
		//Tooltip for Object
		public object ColumnToolTipControllerText(View view)
        {
        //    if (Column != null) 
		//			return Column;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool ColumnIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultColumn();
				//if (result != null && Column != null){
				//	return !Column.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _row;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dòng")]
        [ToolTip("Dòng")]
		//[Index(9)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Row
        { 
		    get => GetPropertyValue<int?>("Row");                         
			set => SetPropertyValue<int?>("Row", value); 
			
        }
		//Tooltip for Object
		public object RowToolTipControllerText(View view)
        {
        //    if (Row != null) 
		//			return Row;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool RowIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRow();
				//if (result != null && Row != null){
				//	return !Row.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _exact;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tìm chính xác")]
        [ToolTip("Tìm chính xác")]
		//[Index(10)]		
		public bool Exact
        { 
		    get => GetPropertyValue<bool>("Exact");                         
			set => SetPropertyValue<bool>("Exact", value); 
			
        }
		//Tooltip for Object
		public object ExactToolTipControllerText(View view)
        {
        //    if (Exact != null) 
		//			return Exact;
            return null;
        }
		//Get Default Value
        public bool GetDefaultExact(View view = null)
        { 
			return Exact;
        }
		//Set Default Value
		public void SetDefaultExact(View view = null)
        {
            //if (Exact is null){
            //    var result = GetDefaultExact(view);
            //    if (result != null && result != Exact){
			//          Exact = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ExactIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultExact();
				//if (result != null && Exact != null){
				//	return !Exact.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _onetime;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Một lần")]
		[ToolTip("Chạy một lần duy nhất ")]
		//[Index(11)]		
		public bool OneTime
        { 
		    get => GetPropertyValue<bool>("OneTime");                         
			set => SetPropertyValue<bool>("OneTime", value); 
			
        }
		//Tooltip for Object
		public object OneTimeToolTipControllerText(View view)
        {
        //    if (OneTime != null) 
		//			return OneTime;
            return null;
        }
		//Get Default Value
        public bool GetDefaultOneTime(View view = null)
        { 
			return OneTime;
        }
		//Set Default Value
		public void SetDefaultOneTime(View view = null)
        {
            //if (OneTime is null){
            //    var result = GetDefaultOneTime(view);
            //    if (result != null && result != OneTime){
			//          OneTime = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OneTimeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOneTime();
				//if (result != null && OneTime != null){
				//	return !OneTime.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _multirow;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Nhiều dòng")]
        [ToolTip("Nhiều dòng")]
		//[Index(12)]		
		public bool MultiRow
        { 
		    get => GetPropertyValue<bool>("MultiRow");                         
			set => SetPropertyValue<bool>("MultiRow", value); 
			
        }
		//Tooltip for Object
		public object MultiRowToolTipControllerText(View view)
        {
        //    if (MultiRow != null) 
		//			return MultiRow;
            return null;
        }
		//Get Default Value
        public bool GetDefaultMultiRow(View view = null)
        { 
			return MultiRow;
        }
		//Set Default Value
		public void SetDefaultMultiRow(View view = null)
        {
            //if (MultiRow is null){
            //    var result = GetDefaultMultiRow(view);
            //    if (result != null && result != MultiRow){
			//          MultiRow = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MultiRowIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMultiRow();
				//if (result != null && MultiRow != null){
				//	return !MultiRow.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _autotranslate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dịch tự động")]
        [ToolTip("Dịch tự động")]
		//[Index(13)]		
		public bool AutoTranslate
        { 
		    get => GetPropertyValue<bool>("AutoTranslate");                         
			set => SetPropertyValue<bool>("AutoTranslate", value); 
			
        }
		//Tooltip for Object
		public object AutoTranslateToolTipControllerText(View view)
        {
        //    if (AutoTranslate != null) 
		//			return AutoTranslate;
            return null;
        }
		//Get Default Value
        public bool GetDefaultAutoTranslate(View view = null)
        { 
			return AutoTranslate;
        }
		//Set Default Value
		public void SetDefaultAutoTranslate(View view = null)
        {
            //if (AutoTranslate is null){
            //    var result = GetDefaultAutoTranslate(view);
            //    if (result != null && result != AutoTranslate){
			//          AutoTranslate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AutoTranslateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAutoTranslate();
				//if (result != null && AutoTranslate != null){
				//	return !AutoTranslate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _overwrite;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi đè")]
        [ToolTip("Ghi đè")]
		//[Index(14)]		
		public bool Overwrite
        { 
		    get => GetPropertyValue<bool>("Overwrite");                         
			set => SetPropertyValue<bool>("Overwrite", value); 
			
        }
		//Tooltip for Object
		public object OverwriteToolTipControllerText(View view)
        {
        //    if (Overwrite != null) 
		//			return Overwrite;
            return null;
        }
		//Get Default Value
        public bool GetDefaultOverwrite(View view = null)
        { 
			return Overwrite;
        }
		//Set Default Value
		public void SetDefaultOverwrite(View view = null)
        {
            //if (Overwrite is null){
            //    var result = GetDefaultOverwrite(view);
            //    if (result != null && result != Overwrite){
			//          Overwrite = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OverwriteIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOverwrite();
				//if (result != null && Overwrite != null){
				//	return !Overwrite.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(15)]		
		public bool InActive
        { 
		    get => GetPropertyValue<bool>("InActive");                         
			set => SetPropertyValue<bool>("InActive", value); 
			
        }
		//Tooltip for Object
		public object InActiveToolTipControllerText(View view)
        {
        //    if (InActive != null) 
		//			return InActive;
            return null;
        }
		//Get Default Value
        public bool GetDefaultInActive(View view = null)
        { 
			return InActive;
        }
		//Set Default Value
		public void SetDefaultInActive(View view = null)
        {
            //if (InActive is null){
            //    var result = GetDefaultInActive(view);
            //    if (result != null && result != InActive){
			//          InActive = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool InActiveIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultInActive();
				//if (result != null && InActive != null){
				//	return !InActive.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0439ImportCode
            base.AfterConstruction();
Exact = true;
            #endregion 0439ImportCode
 
        //SetDefaultOrder(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultExtractorType(View view = null);
        //SetDefaultCssXpathValue(View view = null);
        //SetDefaultWebExtractor(View view = null);
        //SetDefaultAttribute(View view = null);
        //SetDefaultPassword(View view = null);
        //SetDefaultInsideTable(View view = null);
        //SetDefaultColumn(View view = null);
        //SetDefaultRow(View view = null);
        //SetDefaultExact(View view = null);
        //SetDefaultOneTime(View view = null);
        //SetDefaultMultiRow(View view = null);
        //SetDefaultAutoTranslate(View view = null);
        //SetDefaultOverwrite(View view = null);
        //SetDefaultInActive(View view = null);
			
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

                switch (propertyName)
                {       
				
                    case nameof(WebExtractor):
                        OnChangedWebExtractor(oldValue, newValue);
                        break;
				
                    case nameof(InsideTable):
                        OnChangedInsideTable(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedWebExtractor(object oldValue, object newValue)
        {
            #region 0349ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 0349ImportCode
        }               
        private void OnChangedInsideTable(object oldValue, object newValue)
        {
            #region 0427ImportCode
            if (newValue is null) return;
SetDefaultColumn();
SetDefaultRow();            
            #endregion 0427ImportCode
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
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0021ImportCode
		public void SetDefaultColumn(View view = null)
        {
            //Code: 0021            Oid: 1be998db-46f1-40b6-b521-572b1da937bb
            if(Column == null) Column = GetDefaultColumn();
        }
#endregion 0021ImportCode
#region 0029ImportCode
		public int? GetDefaultColumn(View view = null)
        {
            //Code: 0029            Oid: 74c5b140-c5c7-4788-a460-289f747d770a
            if(InsideTable)
	return 2;
return null;
        }
#endregion 0029ImportCode
#region 0088ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 0088            Oid: c717515f-9a7e-472a-9e10-e0035ee40556
            Order= GetDefaultOrder();
        }
#endregion 0088ImportCode
#region 0108ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 0108            Oid: 9fb2ef97-d81f-4992-8607-7b02c3826a11
            if (WebExtractor != null && WebExtractor.ExtractorItemList != null)
{
    var lasted = WebExtractor.ExtractorItemList.OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
return null;
        }
#endregion 0108ImportCode
#region 0124ImportCode
		public int? GetDefaultRow(View view = null)
        {
            //Code: 0124            Oid: af4244d4-55c3-4402-b591-ac2fb3260b90
            if(InsideTable)
	return 1;
return null;

        }
#endregion 0124ImportCode
#region 0152ImportCode
		public void SetDefaultRow(View view = null)
        {
            //Code: 0152            Oid: 46616f92-8f12-479f-b70e-d2ef957613fd
            if(Row == null) Row = GetDefaultRow();
        }
#endregion 0152ImportCode
        #endregion
//Mã nguồn bổ sung
#region ExtractorItemImportCode
public bool IsGet()
        {
            return ExtractorType == ExtractorType.Text ||
                   ExtractorType == ExtractorType.Image ||
                   ExtractorType == ExtractorType.Link ||
                   ExtractorType == ExtractorType.ImageInLink ||
                   ExtractorType == ExtractorType.Html ||
                   ExtractorType == ExtractorType.Table;
        }	
#endregion ExtractorItemImportCode
		 		 
    }
}
