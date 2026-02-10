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
    [ModelDefault("Caption", "Kiểu cách"), ImageName("ParagraphStyle")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
    [ShowToolTipAttribute(TargetItems = nameof(Font)+ "," + nameof(Size))]
 
 
	[MobileColumnAttribute(Context = "ParagraphStyle_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ParagraphStyle_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Video_ParagraphStyleList_ListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class ParagraphStyle:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ParagraphStyle(Session session)
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
               

		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(0)]		

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

	
       
		//private string _font;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Phông")]
        [ToolTip("Phông")]
		//[Index(1)]		

 		[Size(150)]
		public string Font
        { 
		    get => GetPropertyValue<string>("Font");                         
			set => SetPropertyValue<string>("Font", value); 
			
        }
		//Tooltip for Object
		public object FontToolTipControllerText(View view)
        {
            #region 1031ImportCode 
if (Font != null)
                return Font;
            if (UpperStyle != null)
                return UpperStyle.FontToolTipControllerText(view);
#endregion 1031ImportCode
            return null;
        }
		//Get Default Value
        public string GetDefaultFont(View view = null)
        { 
			return Font;
        }
		//Set Default Value
		public void SetDefaultFont(View view = null)
        {
            //if (Font is null){
            //    var result = GetDefaultFont(view);
            //    if (result != null && result != Font){
			//          Font = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FontIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFont();
				//if (result != null && Font != null){
				//	return !Font.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _translatefont;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Phông dịch")]
        [ToolTip("Phông dịch")]
		//[Index(2)]		

 		[Size(100)]
	    [ModelDefault("PropertyEditorType", "StringComboEditor")]
		public string TranslateFont
        { 
		    get => GetPropertyValue<string>("TranslateFont");                         
			set => SetPropertyValue<string>("TranslateFont", value); 
			
        }
		//Tooltip for Object
		public object TranslateFontToolTipControllerText(View view)
        {
        //    if (TranslateFont != null) 
		//			return TranslateFont;
            return null;
        }
		//Get Default Value
        public string GetDefaultTranslateFont(View view = null)
        { 
			return TranslateFont;
        }
		//Set Default Value
		public void SetDefaultTranslateFont(View view = null)
        {
            //if (TranslateFont is null){
            //    var result = GetDefaultTranslateFont(view);
            //    if (result != null && result != TranslateFont){
			//          TranslateFont = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TranslateFontIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTranslateFont();
				//if (result != null && TranslateFont != null){
				//	return !TranslateFont.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _size;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cỡ")]
        [ToolTip("Cỡ")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Size
        { 
		    get => GetPropertyValue<decimal?>("Size");                         
			set => SetPropertyValue<decimal?>("Size", value); 
			
        }
		//Tooltip for Object
		public object SizeToolTipControllerText(View view)
        {
            #region 1030ImportCode 
 if (Size != null)
                return Size.ToString();
            if (UpperStyle != null)
                return UpperStyle.FontToolTipControllerText(view);
#endregion 1030ImportCode
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultSize(View view = null)
        { 
			return Size;
        }
		//Set Default Value
		public void SetDefaultSize(View view = null)
        {
            //if (Size is null){
            //    var result = GetDefaultSize(view);
            //    if (result != null && result != Size){
			//          Size = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SizeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSize();
				//if (result != null && Size != null){
				//	return !Size.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Color? _color;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Màu")]
        [ToolTip("Màu")]
		//[Index(4)]		
	    [ValueConverter(typeof(DevExpress.ExpressApp.StateMachine.Xpo.NullableColorConverter))]
	    [DevExpress.Xpo.Persistent()]
		public Color? Color
        { 
		    get => GetPropertyValue<Color?>("Color");                         
			set => SetPropertyValue<Color?>("Color", value); 
			
        }
		//Tooltip for Object
		public object ColorToolTipControllerText(View view)
        {
        //    if (Color != null) 
		//			return Color;
            return null;
        }
		//Get Default Value
        public Color? GetDefaultColor(View view = null)
        { 
			return Color;
        }
		//Set Default Value
		public void SetDefaultColor(View view = null)
        {
            //if (Color is null){
            //    var result = GetDefaultColor(view);
            //    if (result != null && result != Color){
			//          Color = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ColorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultColor();
				//if (result != null && Color != null){
				//	return !Color.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _bold;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Đậm")]
        [ToolTip("Đậm")]
		//[Index(5)]		
		public bool Bold
        { 
		    get => GetPropertyValue<bool>("Bold");                         
			set => SetPropertyValue<bool>("Bold", value); 
			
        }
		//Tooltip for Object
		public object BoldToolTipControllerText(View view)
        {
        //    if (Bold != null) 
		//			return Bold;
            return null;
        }
		//Get Default Value
        public bool GetDefaultBold(View view = null)
        { 
			return Bold;
        }
		//Set Default Value
		public void SetDefaultBold(View view = null)
        {
            //if (Bold is null){
            //    var result = GetDefaultBold(view);
            //    if (result != null && result != Bold){
			//          Bold = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BoldIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBold();
				//if (result != null && Bold != null){
				//	return !Bold.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _italic;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nghiêng")]
        [ToolTip("Nghiêng")]
		//[Index(6)]		
		public bool Italic
        { 
		    get => GetPropertyValue<bool>("Italic");                         
			set => SetPropertyValue<bool>("Italic", value); 
			
        }
		//Tooltip for Object
		public object ItalicToolTipControllerText(View view)
        {
        //    if (Italic != null) 
		//			return Italic;
            return null;
        }
		//Get Default Value
        public bool GetDefaultItalic(View view = null)
        { 
			return Italic;
        }
		//Set Default Value
		public void SetDefaultItalic(View view = null)
        {
            //if (Italic is null){
            //    var result = GetDefaultItalic(view);
            //    if (result != null && result != Italic){
			//          Italic = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ItalicIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultItalic();
				//if (result != null && Italic != null){
				//	return !Italic.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _underline;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Gạch dưới")]
        [ToolTip("Gạch dưới")]
		//[Index(7)]		
		public bool Underline
        { 
		    get => GetPropertyValue<bool>("Underline");                         
			set => SetPropertyValue<bool>("Underline", value); 
			
        }
		//Tooltip for Object
		public object UnderlineToolTipControllerText(View view)
        {
        //    if (Underline != null) 
		//			return Underline;
            return null;
        }
		//Get Default Value
        public bool GetDefaultUnderline(View view = null)
        { 
			return Underline;
        }
		//Set Default Value
		public void SetDefaultUnderline(View view = null)
        {
            //if (Underline is null){
            //    var result = GetDefaultUnderline(view);
            //    if (result != null && result != Underline){
			//          Underline = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UnderlineIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUnderline();
				//if (result != null && Underline != null){
				//	return !Underline.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _outline;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Outline")]
        [ToolTip("Outline")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Outline
        { 
		    get => GetPropertyValue<int?>("Outline");                         
			set => SetPropertyValue<int?>("Outline", value); 
			
        }
		//Tooltip for Object
		public object OutlineToolTipControllerText(View view)
        {
        //    if (Outline != null) 
		//			return Outline;
            return null;
        }
		//Get Default Value
        public int? GetDefaultOutline(View view = null)
        { 
			return Outline;
        }
		//Set Default Value
		public void SetDefaultOutline(View view = null)
        {
            //if (Outline is null){
            //    var result = GetDefaultOutline(view);
            //    if (result != null && result != Outline){
			//          Outline = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OutlineIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOutline();
				//if (result != null && Outline != null){
				//	return !Outline.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Alignment _alignment;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Căn lề")]
        [ToolTip("Căn lề")]
		//[Index(9)]		
		public Alignment Alignment
        { 
		    get => GetPropertyValue<Alignment>("Alignment");                         
			set => SetPropertyValue<Alignment>("Alignment", value); 
			
        }
		//Tooltip for Object
		public object AlignmentToolTipControllerText(View view)
        {
        //    if (Alignment != null) 
		//			return Alignment;
            return null;
        }
		//Get Default Value
        public Alignment GetDefaultAlignment(View view = null)
        { 
			return Alignment;
        }
		//Set Default Value
		public void SetDefaultAlignment(View view = null)
        {
            //if (Alignment is null){
            //    var result = GetDefaultAlignment(view);
            //    if (result != null && result != Alignment){
			//          Alignment = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AlignmentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAlignment();
				//if (result != null && Alignment != null){
				//	return !Alignment.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _spacingbefore;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cách trước")]
        [ToolTip("Cách trước")]
		//[Index(10)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? SpacingBefore
        { 
		    get => GetPropertyValue<decimal?>("SpacingBefore");                         
			set => SetPropertyValue<decimal?>("SpacingBefore", value); 
			
        }
		//Tooltip for Object
		public object SpacingBeforeToolTipControllerText(View view)
        {
        //    if (SpacingBefore != null) 
		//			return SpacingBefore;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultSpacingBefore(View view = null)
        { 
			return SpacingBefore;
        }
		//Set Default Value
		public void SetDefaultSpacingBefore(View view = null)
        {
            //if (SpacingBefore is null){
            //    var result = GetDefaultSpacingBefore(view);
            //    if (result != null && result != SpacingBefore){
			//          SpacingBefore = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpacingBeforeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpacingBefore();
				//if (result != null && SpacingBefore != null){
				//	return !SpacingBefore.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _spacingafter;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cách sau")]
        [ToolTip("Cách sau")]
		//[Index(11)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? SpacingAfter
        { 
		    get => GetPropertyValue<decimal?>("SpacingAfter");                         
			set => SetPropertyValue<decimal?>("SpacingAfter", value); 
			
        }
		//Tooltip for Object
		public object SpacingAfterToolTipControllerText(View view)
        {
        //    if (SpacingAfter != null) 
		//			return SpacingAfter;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultSpacingAfter(View view = null)
        { 
			return SpacingAfter;
        }
		//Set Default Value
		public void SetDefaultSpacingAfter(View view = null)
        {
            //if (SpacingAfter is null){
            //    var result = GetDefaultSpacingAfter(view);
            //    if (result != null && result != SpacingAfter){
			//          SpacingAfter = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpacingAfterIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpacingAfter();
				//if (result != null && SpacingAfter != null){
				//	return !SpacingAfter.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _spacingline;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cách dòng")]
        [ToolTip("Cách dòng")]
		//[Index(12)]		

 		[Size(100)]
	    [ModelDefault("PropertyEditorType", "StringComboEditor")]
		public string SpacingLine
        { 
		    get => GetPropertyValue<string>("SpacingLine");                         
			set => SetPropertyValue<string>("SpacingLine", value); 
			
        }
		//Tooltip for Object
		public object SpacingLineToolTipControllerText(View view)
        {
        //    if (SpacingLine != null) 
		//			return SpacingLine;
            return null;
        }
		//Get Default Value
        public string GetDefaultSpacingLine(View view = null)
        { 
			return SpacingLine;
        }
		//Set Default Value
		public void SetDefaultSpacingLine(View view = null)
        {
            //if (SpacingLine is null){
            //    var result = GetDefaultSpacingLine(view);
            //    if (result != null && result != SpacingLine){
			//          SpacingLine = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpacingLineIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpacingLine();
				//if (result != null && SpacingLine != null){
				//	return !SpacingLine.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _spacinglineat;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chính xác")]
        [ToolTip("Chính xác")]
		//[Index(13)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? SpacingLineAt
        { 
		    get => GetPropertyValue<decimal?>("SpacingLineAt");                         
			set => SetPropertyValue<decimal?>("SpacingLineAt", value); 
			
        }
		//Tooltip for Object
		public object SpacingLineAtToolTipControllerText(View view)
        {
        //    if (SpacingLineAt != null) 
		//			return SpacingLineAt;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultSpacingLineAt(View view = null)
        { 
			return SpacingLineAt;
        }
		//Set Default Value
		public void SetDefaultSpacingLineAt(View view = null)
        {
            //if (SpacingLineAt is null){
            //    var result = GetDefaultSpacingLineAt(view);
            //    if (result != null && result != SpacingLineAt){
			//          SpacingLineAt = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpacingLineAtIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpacingLineAt();
				//if (result != null && SpacingLineAt != null){
				//	return !SpacingLineAt.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _indentleft;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thụt trái")]
        [ToolTip("Thụt trái")]
		//[Index(14)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? IndentLeft
        { 
		    get => GetPropertyValue<decimal?>("IndentLeft");                         
			set => SetPropertyValue<decimal?>("IndentLeft", value); 
			
        }
		//Tooltip for Object
		public object IndentLeftToolTipControllerText(View view)
        {
        //    if (IndentLeft != null) 
		//			return IndentLeft;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultIndentLeft(View view = null)
        { 
			return IndentLeft;
        }
		//Set Default Value
		public void SetDefaultIndentLeft(View view = null)
        {
            //if (IndentLeft is null){
            //    var result = GetDefaultIndentLeft(view);
            //    if (result != null && result != IndentLeft){
			//          IndentLeft = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool IndentLeftIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultIndentLeft();
				//if (result != null && IndentLeft != null){
				//	return !IndentLeft.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _indentright;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thụt phải")]
        [ToolTip("Thụt phải")]
		//[Index(15)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? IndentRight
        { 
		    get => GetPropertyValue<decimal?>("IndentRight");                         
			set => SetPropertyValue<decimal?>("IndentRight", value); 
			
        }
		//Tooltip for Object
		public object IndentRightToolTipControllerText(View view)
        {
        //    if (IndentRight != null) 
		//			return IndentRight;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultIndentRight(View view = null)
        { 
			return IndentRight;
        }
		//Set Default Value
		public void SetDefaultIndentRight(View view = null)
        {
            //if (IndentRight is null){
            //    var result = GetDefaultIndentRight(view);
            //    if (result != null && result != IndentRight){
			//          IndentRight = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool IndentRightIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultIndentRight();
				//if (result != null && IndentRight != null){
				//	return !IndentRight.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _indentfirstline;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thụt đầu")]
        [ToolTip("Thụt đầu")]
		//[Index(16)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? IndentFirstLine
        { 
		    get => GetPropertyValue<decimal?>("IndentFirstLine");                         
			set => SetPropertyValue<decimal?>("IndentFirstLine", value); 
			
        }
		//Tooltip for Object
		public object IndentFirstLineToolTipControllerText(View view)
        {
        //    if (IndentFirstLine != null) 
		//			return IndentFirstLine;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultIndentFirstLine(View view = null)
        { 
			return IndentFirstLine;
        }
		//Set Default Value
		public void SetDefaultIndentFirstLine(View view = null)
        {
            //if (IndentFirstLine is null){
            //    var result = GetDefaultIndentFirstLine(view);
            //    if (result != null && result != IndentFirstLine){
			//          IndentFirstLine = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool IndentFirstLineIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultIndentFirstLine();
				//if (result != null && IndentFirstLine != null){
				//	return !IndentFirstLine.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.ParagraphStyle _upperstyle;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(17)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpperStyleCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.ParagraphStyle UpperStyle
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ParagraphStyle>("UpperStyle");                         
			set => SetPropertyValue<Module.BusinessObjects.ParagraphStyle>("UpperStyle", value); 
			
        }
		//Tooltip for Object
		public object UpperStyleToolTipControllerText(View view)
        {
        //    if (UpperStyle != null) 
		//			return UpperStyle;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ParagraphStyle GetDefaultUpperStyle(View view = null)
        { 
			return UpperStyle;
        }
		//Set Default Value
		public void SetDefaultUpperStyle(View view = null)
        {
            //if (UpperStyle is null){
            //    var result = GetDefaultUpperStyle(view);
            //    if (result != null && result != UpperStyle){
			//          UpperStyle = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperStyleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpperStyle();
				//if (result != null && UpperStyle != null){
				//	return !UpperStyle.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpperStyleCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(UpperStyle));
            }
        }
	
       
		//private Module.BusinessObjects.Video _video;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tư liệu")]
        [ToolTip("Tư liệu")]
		//[Index(18)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(VideoCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Video-ParagraphStyleList")]
	 
		public Module.BusinessObjects.Video Video
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Video>("Video");                         
			set => SetPropertyValue<Module.BusinessObjects.Video>("Video", value); 
			
        }
		//Tooltip for Object
		public object VideoToolTipControllerText(View view)
        {
        //    if (Video != null) 
		//			return Video;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Video GetDefaultVideo(View view = null)
        { 
			return Video;
        }
		//Set Default Value
		public void SetDefaultVideo(View view = null)
        {
            //if (Video is null){
            //    var result = GetDefaultVideo(view);
            //    if (result != null && result != Video){
			//          Video = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool VideoIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultVideo();
				//if (result != null && Video != null){
				//	return !Video.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator VideoCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Video));
            }
        }
	
       
		//private int? _elementquantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(19)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? ElementQuantity
        { 
		    get => GetPropertyValue<int?>("ElementQuantity");                         
			set => SetPropertyValue<int?>("ElementQuantity", value); 
			
        }
		//Tooltip for Object
		public object ElementQuantityToolTipControllerText(View view)
        {
        //    if (ElementQuantity != null) 
		//			return ElementQuantity;
            return null;
        }
		//Get Default Value
        public int? GetDefaultElementQuantity(View view = null)
        { 
			return ElementQuantity;
        }
		//Set Default Value
		public void SetDefaultElementQuantity(View view = null)
        {
            //if (ElementQuantity is null){
            //    var result = GetDefaultElementQuantity(view);
            //    if (result != null && result != ElementQuantity){
			//          ElementQuantity = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ElementQuantityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultElementQuantity();
				//if (result != null && ElementQuantity != null){
				//	return !ElementQuantity.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.BookMark _link;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
        [ToolTip("Liên kết")]
		//[Index(20)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LinkCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.BookMark Link
        { 
		    get => GetPropertyValue<Module.BusinessObjects.BookMark>("Link");                         
			set => SetPropertyValue<Module.BusinessObjects.BookMark>("Link", value); 
			
        }
		//Tooltip for Object
		public object LinkToolTipControllerText(View view)
        {
        //    if (Link != null) 
		//			return Link;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.BookMark GetDefaultLink(View view = null)
        { 
			return Link;
        }
		//Set Default Value
		public void SetDefaultLink(View view = null)
        {
            //if (Link is null){
            //    var result = GetDefaultLink(view);
            //    if (result != null && result != Link){
			//          Link = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LinkIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLink();
				//if (result != null && Link != null){
				//	return !Link.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LinkCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Link));
            }
        }
	
       
		//private decimal? _height;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cao")]
        [ToolTip("Cao")]
		//[Index(21)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Height
        { 
		    get => GetPropertyValue<decimal?>("Height");                         
			set => SetPropertyValue<decimal?>("Height", value); 
			
        }
		//Tooltip for Object
		public object HeightToolTipControllerText(View view)
        {
        //    if (Height != null) 
		//			return Height;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultHeight(View view = null)
        { 
			return Height;
        }
		//Set Default Value
		public void SetDefaultHeight(View view = null)
        {
            //if (Height is null){
            //    var result = GetDefaultHeight(view);
            //    if (result != null && result != Height){
			//          Height = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool HeightIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultHeight();
				//if (result != null && Height != null){
				//	return !Height.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _width;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Rộng")]
        [ToolTip("Rộng")]
		//[Index(22)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Width
        { 
		    get => GetPropertyValue<decimal?>("Width");                         
			set => SetPropertyValue<decimal?>("Width", value); 
			
        }
		//Tooltip for Object
		public object WidthToolTipControllerText(View view)
        {
        //    if (Width != null) 
		//			return Width;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultWidth(View view = null)
        { 
			return Width;
        }
		//Set Default Value
		public void SetDefaultWidth(View view = null)
        {
            //if (Width is null){
            //    var result = GetDefaultWidth(view);
            //    if (result != null && result != Width){
			//          Width = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WidthIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWidth();
				//if (result != null && Width != null){
				//	return !Width.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private ObjectLayout _objectlayout;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bố cục")]
        [ToolTip("Bố cục")]
		//[Index(23)]		
		public ObjectLayout ObjectLayout
        { 
		    get => GetPropertyValue<ObjectLayout>("ObjectLayout");                         
			set => SetPropertyValue<ObjectLayout>("ObjectLayout", value); 
			
        }
		//Tooltip for Object
		public object ObjectLayoutToolTipControllerText(View view)
        {
        //    if (ObjectLayout != null) 
		//			return ObjectLayout;
            return null;
        }
		//Get Default Value
        public ObjectLayout GetDefaultObjectLayout(View view = null)
        { 
			return ObjectLayout;
        }
		//Set Default Value
		public void SetDefaultObjectLayout(View view = null)
        {
            //if (ObjectLayout is null){
            //    var result = GetDefaultObjectLayout(view);
            //    if (result != null && result != ObjectLayout){
			//          ObjectLayout = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ObjectLayoutIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultObjectLayout();
				//if (result != null && ObjectLayout != null){
				//	return !ObjectLayout.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private AlignmentRelative _alignmentrelative;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mốc căn lề")]
        [ToolTip("Mốc căn lề")]
		//[Index(24)]		
		public AlignmentRelative AlignmentRelative
        { 
		    get => GetPropertyValue<AlignmentRelative>("AlignmentRelative");                         
			set => SetPropertyValue<AlignmentRelative>("AlignmentRelative", value); 
			
        }
		//Tooltip for Object
		public object AlignmentRelativeToolTipControllerText(View view)
        {
        //    if (AlignmentRelative != null) 
		//			return AlignmentRelative;
            return null;
        }
		//Get Default Value
        public AlignmentRelative GetDefaultAlignmentRelative(View view = null)
        { 
			return AlignmentRelative;
        }
		//Set Default Value
		public void SetDefaultAlignmentRelative(View view = null)
        {
            //if (AlignmentRelative is null){
            //    var result = GetDefaultAlignmentRelative(view);
            //    if (result != null && result != AlignmentRelative){
			//          AlignmentRelative = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AlignmentRelativeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAlignmentRelative();
				//if (result != null && AlignmentRelative != null){
				//	return !AlignmentRelative.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _movewithtext;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dịch chuyển theo")]
        [ToolTip("Dịch chuyển theo")]
		//[Index(25)]		
		public bool MoveWithText
        { 
		    get => GetPropertyValue<bool>("MoveWithText");                         
			set => SetPropertyValue<bool>("MoveWithText", value); 
			
        }
		//Tooltip for Object
		public object MoveWithTextToolTipControllerText(View view)
        {
        //    if (MoveWithText != null) 
		//			return MoveWithText;
            return null;
        }
		//Get Default Value
        public bool GetDefaultMoveWithText(View view = null)
        { 
			return MoveWithText;
        }
		//Set Default Value
		public void SetDefaultMoveWithText(View view = null)
        {
            //if (MoveWithText is null){
            //    var result = GetDefaultMoveWithText(view);
            //    if (result != null && result != MoveWithText){
			//          MoveWithText = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MoveWithTextIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMoveWithText();
				//if (result != null && MoveWithText != null){
				//	return !MoveWithText.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Microsoft.Office.Interop.Word.WdWrapType _textwrappingtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bố cục hình")]
        [ToolTip("Bố cục hình")]
		//[Index(26)]		
		public Microsoft.Office.Interop.Word.WdWrapType TextWrappingType
        { 
		    get => GetPropertyValue<Microsoft.Office.Interop.Word.WdWrapType>("TextWrappingType");                         
			set => SetPropertyValue<Microsoft.Office.Interop.Word.WdWrapType>("TextWrappingType", value); 
			
        }
		//Tooltip for Object
		public object TextWrappingTypeToolTipControllerText(View view)
        {
        //    if (TextWrappingType != null) 
		//			return TextWrappingType;
            return null;
        }
		//Get Default Value
        public Microsoft.Office.Interop.Word.WdWrapType GetDefaultTextWrappingType(View view = null)
        { 
			return TextWrappingType;
        }
		//Set Default Value
		public void SetDefaultTextWrappingType(View view = null)
        {
            //if (TextWrappingType is null){
            //    var result = GetDefaultTextWrappingType(view);
            //    if (result != null && result != TextWrappingType){
			//          TextWrappingType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TextWrappingTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTextWrappingType();
				//if (result != null && TextWrappingType != null){
				//	return !TextWrappingType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private ParagraphStyleType _paragraphstyletype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại kiểu cách")]
        [ToolTip("Loại kiểu cách")]
		//[Index(27)]		
		public ParagraphStyleType ParagraphStyleType
        { 
		    get => GetPropertyValue<ParagraphStyleType>("ParagraphStyleType");                         
			set => SetPropertyValue<ParagraphStyleType>("ParagraphStyleType", value); 
			
        }
		//Tooltip for Object
		public object ParagraphStyleTypeToolTipControllerText(View view)
        {
        //    if (ParagraphStyleType != null) 
		//			return ParagraphStyleType;
            return null;
        }
		//Get Default Value
        public ParagraphStyleType GetDefaultParagraphStyleType(View view = null)
        { 
			return ParagraphStyleType;
        }
		//Set Default Value
		public void SetDefaultParagraphStyleType(View view = null)
        {
            //if (ParagraphStyleType is null){
            //    var result = GetDefaultParagraphStyleType(view);
            //    if (result != null && result != ParagraphStyleType){
			//          ParagraphStyleType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ParagraphStyleTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultParagraphStyleType();
				//if (result != null && ParagraphStyleType != null){
				//	return !ParagraphStyleType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultFont(View view = null);
        //SetDefaultTranslateFont(View view = null);
        //SetDefaultSize(View view = null);
        //SetDefaultColor(View view = null);
        //SetDefaultBold(View view = null);
        //SetDefaultItalic(View view = null);
        //SetDefaultUnderline(View view = null);
        //SetDefaultOutline(View view = null);
        //SetDefaultAlignment(View view = null);
        //SetDefaultSpacingBefore(View view = null);
        //SetDefaultSpacingAfter(View view = null);
        //SetDefaultSpacingLine(View view = null);
        //SetDefaultSpacingLineAt(View view = null);
        //SetDefaultIndentLeft(View view = null);
        //SetDefaultIndentRight(View view = null);
        //SetDefaultIndentFirstLine(View view = null);
        //SetDefaultUpperStyle(View view = null);
        //SetDefaultVideo(View view = null);
        //SetDefaultElementQuantity(View view = null);
        //SetDefaultLink(View view = null);
        //SetDefaultHeight(View view = null);
        //SetDefaultWidth(View view = null);
        //SetDefaultObjectLayout(View view = null);
        //SetDefaultAlignmentRelative(View view = null);
        //SetDefaultMoveWithText(View view = null);
        //SetDefaultTextWrappingType(View view = null);
        //SetDefaultParagraphStyleType(View view = null);
			
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

                  
            }
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
        #endregion
//Mã nguồn bổ sung
#region ParagraphStyleImportCode
        public string GetTextWithStyle(string text, System.Drawing.Color? refColor = null)
        {            
            if (string.IsNullOrEmpty(text))
                return text;
            if (Size != null)
                text = "<size=" + Size.Value.ToString("n0") + ">" + text + "</size>";
            if (Bold)
                text = "<b>" + text + "</b>";
            if (Italic)
                text = "<i>" + text + "</i>";
            if (Underline)
                text = "<u>" + text + "</u>";
            //2023-09-19: Chỉnh lại: Không thể hiện màu thực tế, màu nền luôn trắng, Thành phần chính luôn đen, kề trên và kề dưới độ xám 50%, các giá trị Style khác giữ như gốc
            if(refColor != null)
            {
                text = "<color=gray>" + text + "</color>";
            }
            //if (Color != null || refColor != null)
            //{
            //    //Với cấp trên Null: thì hover cả Paragraph trên và dưới, trong đó Paragraph chính thì đúng format, trên và dưới thì chuyển 1 màu tương phản nền kém hơn 50 % so với Paragraph chính
            //    //Màu nền được tính độ xám tương phản với màu phông chính của Paragraph
            //    //Với cấp trên khác null: thì hiện cả Paragraph theo đúng format, màu nền cũng tính như trên
            //    System.Drawing.Color? textColor = null;
            //    if (refColor != null)
            //    {

            //        int change = 50;
            //        //var colorback = System.Drawing.Color.FromName("000000");
            //        int red = refColor.Value.R < 250 ? Convert.ToInt32(refColor.Value.R + change) : refColor.Value.R - change;
            //        if (red > 255)
            //            red = 255;
            //        int green = refColor.Value.G < 250 ? Convert.ToInt32(refColor.Value.G + change) : refColor.Value.G - change;
            //        if (green > 255)
            //            green = 255;
            //        int blue = refColor.Value.B < 250 ? Convert.ToInt32(refColor.Value.B + change) : refColor.Value.B - change;
            //        if (blue > 255)
            //            blue = 255;
            //        textColor = System.Drawing.Color.FromArgb(red, green, blue);
            //        //var color = System.Drawing.Color.FromArgb(Convert.ToInt32(colorback.ToArgb() * 1.8));
            //        //textColor = color;
            //    }
            //    else if(Color != null)
            //    {
            //        textColor = Color;
            //    }
            //    if(textColor != null)
            //    {
            //        text = "<color=#" + textColor.Value.Name + ">" + text + "</color>";
            //        var refBackColor = refColor != null ? refColor.Value : Color;
            //        var backColor = System.Drawing.Color.FromArgb(255 - refBackColor.Value.R, 255 - refBackColor.Value.G, 255 - refBackColor.Value.B);
            //        text = "<backcolor=#" + backColor.Name + ">" + text + "</backcolor>";
            //    }
                               
            //}
                
            return text;
        }
#endregion ParagraphStyleImportCode
		 		 
    }
}
