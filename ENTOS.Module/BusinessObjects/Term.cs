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
    [ModelDefault("Caption", "Thuật ngữ"), ImageName("Term")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
    [ShowToolTipAttribute(TargetItems = nameof(Name)+ "," + nameof(Quantity)+ "," + nameof(LikeTerm)+ "," + nameof(Overlap)+ "," + nameof(LikeWord))]
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(Length))]
 
	[MobileColumnAttribute(Context = "Video_TermList_ListView", TargetItems = nameof(Update)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Term_ListView", TargetItems = nameof(Name)+ "," + nameof(Update))]
	[MobileColumnAttribute(Context = "Term_LookupListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
	[RuleCombinationOfPropertiesIsUnique("UniqueRule.Term", DefaultContexts.Save, "Video, Name, Language")]
[OptimisticLocking(true)]
    public partial class Term:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Term(Session session)
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

 		[Size(200)]
		[RuleRequiredField("RequiredTermName", DefaultContexts.Save)]
		public string Name
        { 
		    get => GetPropertyValue<string>("Name");                         
			set => SetPropertyValue<string>("Name", value); 
			
        }
		//Tooltip for Object
		public object NameToolTipControllerText(View view)
        {
            #region 1527ImportCode 
            //Cấu trúc mới
            var firstTermLocation = TermLocationList.Where(n => n.Audio != null && n.Audio.Start != null).OrderBy(m => m.Audio.Start).FirstOrDefault();
            if (firstTermLocation != null)
            {
                var result = firstTermLocation.LocationToolTipControllerText(view);
                if (result != null && result is string)
                {
                    result = ((string)result).Replace("<b>", "<size=20><b>").Replace("</b>", "</b></size>");
                    result = "<size=10>" + result + "</size>";
                }
                return result;
            }
#endregion 1527ImportCode
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

	
       
		//private string _translate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dịch")]
        [ToolTip("Dịch")]
		//[Index(1)]		

 		[Size(200)]
	    [DataSourceProperty("TranslateDataSource")]
	    [ModelDefault("PropertyEditorType", "StringComboEditor")]
		public string Translate
        { 
		    get => GetPropertyValue<string>("Translate");                         
			set => SetPropertyValue<string>("Translate", value); 
			
        }
		//Tooltip for Object
		public object TranslateToolTipControllerText(View view)
        {
        //    if (Translate != null) 
		//			return Translate;
            return null;
        }
		//Get Default Value
        public string GetDefaultTranslate(View view = null)
        { 
			return Translate;
        }
		//Set Default Value
		public void SetDefaultTranslate(View view = null)
        {
            //if (Translate is null){
            //    var result = GetDefaultTranslate(view);
            //    if (result != null && result != Translate){
			//          Translate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TranslateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTranslate();
				//if (result != null && Translate != null){
				//	return !Translate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _languagetranslate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngữ dịch")]
        [ToolTip("Ngữ dịch")]
		//[Index(2)]		
		public bool LanguageTranslate
        { 
		    get => GetPropertyValue<bool>("LanguageTranslate");                         
			set => SetPropertyValue<bool>("LanguageTranslate", value); 
			
        }
		//Tooltip for Object
		public object LanguageTranslateToolTipControllerText(View view)
        {
        //    if (LanguageTranslate != null) 
		//			return LanguageTranslate;
            return null;
        }
		//Get Default Value
        public bool GetDefaultLanguageTranslate(View view = null)
        { 
			return LanguageTranslate;
        }
		//Set Default Value
		public void SetDefaultLanguageTranslate(View view = null)
        {
            //if (LanguageTranslate is null){
            //    var result = GetDefaultLanguageTranslate(view);
            //    if (result != null && result != LanguageTranslate){
			//          LanguageTranslate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LanguageTranslateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLanguageTranslate();
				//if (result != null && LanguageTranslate != null){
				//	return !LanguageTranslate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _googletranslate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Máy dịch")]
        [ToolTip("Máy dịch")]
		//[Index(3)]		

 		[Size(200)]
		public string GoogleTranslate
        { 
		    get => GetPropertyValue<string>("GoogleTranslate");                         
			set => SetPropertyValue<string>("GoogleTranslate", value); 
			
        }
		//Tooltip for Object
		public object GoogleTranslateToolTipControllerText(View view)
        {
        //    if (GoogleTranslate != null) 
		//			return GoogleTranslate;
            return null;
        }
		//Get Default Value
        public string GetDefaultGoogleTranslate(View view = null)
        { 
			return GoogleTranslate;
        }
		//Set Default Value
		public void SetDefaultGoogleTranslate(View view = null)
        {
            //if (GoogleTranslate is null){
            //    var result = GetDefaultGoogleTranslate(view);
            //    if (result != null && result != GoogleTranslate){
			//          GoogleTranslate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool GoogleTranslateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultGoogleTranslate();
				//if (result != null && GoogleTranslate != null){
				//	return !GoogleTranslate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private WordType _wordtype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Từ loại")]
        [ToolTip("Từ loại")]
		//[Index(4)]		
		public WordType WordType
        { 
		    get => GetPropertyValue<WordType>("WordType");                         
			set => SetPropertyValue<WordType>("WordType", value); 
			
        }
		//Tooltip for Object
		public object WordTypeToolTipControllerText(View view)
        {
        //    if (WordType != null) 
		//			return WordType;
            return null;
        }
		//Get Default Value
        public WordType GetDefaultWordType(View view = null)
        { 
			return WordType;
        }
		//Set Default Value
		public void SetDefaultWordType(View view = null)
        {
            //if (WordType is null){
            //    var result = GetDefaultWordType(view);
            //    if (result != null && result != WordType){
			//          WordType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WordTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWordType();
				//if (result != null && WordType != null){
				//	return !WordType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuật vị")]
		//[Index(5)]
		[DevExpress.Xpo.Association("Term-TermLocationList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.TermLocation> TermLocationList
        {      
		    get => GetCollection<Module.BusinessObjects.TermLocation>("TermLocationList"); 
			
        }
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
	    [ModelDefault("AllowEdit", "False")]
		public DateTime Update
        { 
		    get => GetPropertyValue<DateTime>("Update");                         
			set => SetPropertyValue<DateTime>("Update", value); 
			
        }
		//Tooltip for Object
		public object UpdateToolTipControllerText(View view)
        {
        //    if (Update != null) 
		//			return Update;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool UpdateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpdate();
				//if (result != null && Update != null){
				//	return !Update.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Video _video;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Video")]
        [ToolTip("Video")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(VideoCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Video-TermList")]
	 
		[RuleRequiredField("RequiredTermVideo", DefaultContexts.Save)]
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
	
       
		//private TermType _termtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(8)]		
		[RuleRequiredField("RequiredTermTermType", DefaultContexts.Save)]
		public TermType TermType
        { 
		    get => GetPropertyValue<TermType>("TermType");                         
			set => SetPropertyValue<TermType>("TermType", value); 
			
        }
		//Tooltip for Object
		public object TermTypeToolTipControllerText(View view)
        {
        //    if (TermType != null) 
		//			return TermType;
            return null;
        }
		//Get Default Value
        public TermType GetDefaultTermType(View view = null)
        { 
			return TermType;
        }
		//Set Default Value
		public void SetDefaultTermType(View view = null)
        {
            //if (TermType is null){
            //    var result = GetDefaultTermType(view);
            //    if (result != null && result != TermType){
			//          TermType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TermTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTermType();
				//if (result != null && TermType != null){
				//	return !TermType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _quantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số thuật vị")]
        [ToolTip("Số thuật vị")]
		//[Index(9)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Quantity
        { 
		    get => GetPropertyValue<int?>("Quantity");                         
			set => SetPropertyValue<int?>("Quantity", value); 
			
        }
		//Tooltip for Object
		public object QuantityToolTipControllerText(View view)
        {
            #region 0598ImportCode 
//2023-06-06: 
            //Hover 5 câu trong thuật vị
            var termsLocationList = TermLocationList.Where(n => n.Audio != null && n.Audio.Start != null).OrderBy(m => m.Audio.Start).ToList();
            string result = "";
            //for (int i = 0; i < termsLocationList.Count && i < 5; i++)
			//2023-06-27: Bỏ hover tối đa 5 câu
			for (int i = 0; i < termsLocationList.Count; i++)
            {
                if (i > 0)
                    result += System.Environment.NewLine;
                result += termsLocationList[i].LocationToolTipControllerText(view);
            }
            return result;
#endregion 0598ImportCode
            return null;
        }
		//Get Default Value
        public int? GetDefaultQuantity(View view = null)
        { 
			return Quantity;
        }
		//Set Default Value
		public void SetDefaultQuantity(View view = null)
        {
            //if (Quantity is null){
            //    var result = GetDefaultQuantity(view);
            //    if (result != null && result != Quantity){
			//          Quantity = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool QuantityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultQuantity();
				//if (result != null && Quantity != null){
				//	return !Quantity.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _flag;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ 1")]
        [ToolTip("Cờ 1")]
		//[Index(10)]		
	    [NotMapped()]
	    [NonPersistent()]
		public bool Flag
        { 
		    get => GetPropertyValue<bool>("Flag");                         
			set => SetPropertyValue<bool>("Flag", value); 
			
        }
		//Tooltip for Object
		public object FlagToolTipControllerText(View view)
        {
        //    if (Flag != null) 
		//			return Flag;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFlag(View view = null)
        { 
			return Flag;
        }
		//Set Default Value
		public void SetDefaultFlag(View view = null)
        {
            //if (Flag is null){
            //    var result = GetDefaultFlag(view);
            //    if (result != null && result != Flag){
			//          Flag = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FlagIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFlag();
				//if (result != null && Flag != null){
				//	return !Flag.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _numbervalue;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trị số")]
        [ToolTip("Trị số")]
		//[Index(11)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? NumberValue
        { 
		    get => GetPropertyValue<decimal?>("NumberValue");                         
			set => SetPropertyValue<decimal?>("NumberValue", value); 
			
        }
		//Tooltip for Object
		public object NumberValueToolTipControllerText(View view)
        {
        //    if (NumberValue != null) 
		//			return NumberValue;
            return null;
        }
		//Get Default Value
		//Set Default Value
		public void SetDefaultNumberValue(View view = null)
        {
            //if (NumberValue is null){
            //    var result = GetDefaultNumberValue(view);
            //    if (result != null && result != NumberValue){
			//          NumberValue = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NumberValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNumberValue();
				//if (result != null && NumberValue != null){
				//	return !NumberValue.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _datevalue;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trị ngày")]
        [ToolTip("Trị ngày")]
		//[Index(12)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? DateValue
        { 
		    get => GetPropertyValue<DateTime?>("DateValue");                         
			set => SetPropertyValue<DateTime?>("DateValue", value); 
			
        }
		//Tooltip for Object
		public object DateValueToolTipControllerText(View view)
        {
        //    if (DateValue != null) 
		//			return DateValue;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultDateValue(View view = null)
        { 
			return DateValue;
        }
		//Set Default Value
		public void SetDefaultDateValue(View view = null)
        {
            //if (DateValue is null){
            //    var result = GetDefaultDateValue(view);
            //    if (result != null && result != DateValue){
			//          DateValue = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DateValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDateValue();
				//if (result != null && DateValue != null){
				//	return !DateValue.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _length;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ký tự")]
        [ToolTip("Ký tự")]
		//[Index(13)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Length
        { 
		    get => GetPropertyValue<int?>("Length");                         
			set => SetPropertyValue<int?>("Length", value); 
			
        }
		//Tooltip for Object
		public object LengthToolTipControllerText(View view)
        {
        //    if (Length != null) 
		//			return Length;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool LengthIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLength();
				//if (result != null && Length != null){
				//	return !Length.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _wordquantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số từ")]
        [ToolTip("Số từ")]
		//[Index(14)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? WordQuantity
        { 
		    #region 1060ImportCode 
		    get
            {
                //char[] separators = new char[] {' ', '-'};
                //2024-12-25: Thống nhất ký tự gạch ngang không để đếm từ
                char[] separators = new char[] { ' ' };
                if (!string.IsNullOrEmpty(Name))
                {
                    return Name.Split(separators, StringSplitOptions.RemoveEmptyEntries).Length;
                }
                return null;
            } 
#endregion 1060ImportCode
			
        }
		//Tooltip for Object
		public object WordQuantityToolTipControllerText(View view)
        {
        //    if (WordQuantity != null) 
		//			return WordQuantity;
            return null;
        }
		//Get Default Value
        public int? GetDefaultWordQuantity(View view = null)
        { 
			return WordQuantity;
        }
		//Set Default Value
		public void SetDefaultWordQuantity(View view = null)
        {
            //if (WordQuantity is null){
            //    var result = GetDefaultWordQuantity(view);
            //    if (result != null && result != WordQuantity){
			//          WordQuantity = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WordQuantityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWordQuantity();
				//if (result != null && WordQuantity != null){
				//	return !WordQuantity.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Status _status;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trạng thái")]
        [ToolTip("Trạng thái")]
		//[Index(15)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(StatusCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Status Status
        { 
		    get => GetPropertyValue<Status>("Status");                         
			set => SetPropertyValue<Status>("Status", value); 
			
        }
		//Tooltip for Object
		public object StatusToolTipControllerText(View view)
        {
        //    if (Status != null) 
		//			return Status;
            return null;
        }
		//Get Default Value
        public Status GetDefaultStatus(View view = null)
        { 
			return Status;
        }
		//Set Default Value
		public void SetDefaultStatus(View view = null)
        {
            //if (Status is null){
            //    var result = GetDefaultStatus(view);
            //    if (result != null && result != Status){
			//          Status = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StatusIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStatus();
				//if (result != null && Status != null){
				//	return !Status.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator StatusCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Status));
            }
        }
	
       
		//private string _note;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(16)]		

 		[Size(250)]
	    [NotMapped()]
	    [NonPersistent()]
		public string Note
        { 
		    get => GetPropertyValue<string>("Note");                         
			set => SetPropertyValue<string>("Note", value); 
			
        }
		//Tooltip for Object
		public object NoteToolTipControllerText(View view)
        {
        //    if (Note != null) 
		//			return Note;
            return null;
        }
		//Get Default Value
        public string GetDefaultNote(View view = null)
        { 
			return Note;
        }
		//Set Default Value
		public void SetDefaultNote(View view = null)
        {
            //if (Note is null){
            //    var result = GetDefaultNote(view);
            //    if (result != null && result != Note){
			//          Note = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NoteIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNote();
				//if (result != null && Note != null){
				//	return !Note.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _liketerm;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đồng dạng")]
        [ToolTip("Đồng dạng")]
		//[Index(17)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? LikeTerm
        { 
		    get => GetPropertyValue<int?>("LikeTerm");                         
			set => SetPropertyValue<int?>("LikeTerm", value); 
			
        }
		//Tooltip for Object
		public object LikeTermToolTipControllerText(View view)
        {
            #region 1437ImportCode 
if (Video != null && !string.IsNullOrEmpty(Name))
{
    var likeTermList = GetLikeTermList();
    if (likeTermList?.Count > 0)
    {
        return "<size=20>" + string.Join(", ", likeTermList) + "</size>";
    }
}

#endregion 1437ImportCode
            return null;
        }
		//Get Default Value
        public int? GetDefaultLikeTerm(View view = null)
        { 
			return LikeTerm;
        }
		//Set Default Value
		public void SetDefaultLikeTerm(View view = null)
        {
            //if (LikeTerm is null){
            //    var result = GetDefaultLikeTerm(view);
            //    if (result != null && result != LikeTerm){
			//          LikeTerm = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LikeTermIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLikeTerm();
				//if (result != null && LikeTerm != null){
				//	return !LikeTerm.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _flag2;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ 2")]
        [ToolTip("Cờ 2")]
		//[Index(18)]		
		public bool Flag2
        { 
		    get => GetPropertyValue<bool>("Flag2");                         
			set => SetPropertyValue<bool>("Flag2", value); 
			
        }
		//Tooltip for Object
		public object Flag2ToolTipControllerText(View view)
        {
        //    if (Flag2 != null) 
		//			return Flag2;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFlag2(View view = null)
        { 
			return Flag2;
        }
		//Set Default Value
		public void SetDefaultFlag2(View view = null)
        {
            //if (Flag2 is null){
            //    var result = GetDefaultFlag2(view);
            //    if (result != null && result != Flag2){
			//          Flag2 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Flag2IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFlag2();
				//if (result != null && Flag2 != null){
				//	return !Flag2.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _overlap;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đè nhau")]
        [ToolTip("Đè nhau")]
		//[Index(19)]		
		public bool Overlap
        { 
		    get => GetPropertyValue<bool>("Overlap");                         
			set => SetPropertyValue<bool>("Overlap", value); 
			
        }
		//Tooltip for Object
		public object OverlapToolTipControllerText(View view)
        {
            #region 1069ImportCode 
//091: khi hover nếu cờ checked thì chỉ hiện các câu của các TV có cờ đè
            if(Overlap)
            {
                var termsLocationList = TermLocationList.Where(n => n.Overlap && n.Audio != null && n.Audio.Start != null).OrderBy(m => m.Audio.Start).ToList();
                string result = "";                
                for (int i = 0; i < termsLocationList.Count; i++)
                {
                    if (i > 0)
                        result += System.Environment.NewLine;
                    result += termsLocationList[i].LocationToolTipControllerText(view);
                }
                return result;
            } 
#endregion 1069ImportCode
            return null;
        }
		//Get Default Value
		//Set Default Value
		public void SetDefaultOverlap(View view = null)
        {
            //if (Overlap is null){
            //    var result = GetDefaultOverlap(view);
            //    if (result != null && result != Overlap){
			//          Overlap = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OverlapIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOverlap();
				//if (result != null && Overlap != null){
				//	return !Overlap.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _note2;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú 2")]
        [ToolTip("Ghi chú 2")]
		//[Index(20)]		

 		[Size(250)]
		public string Note2
        { 
		    get => GetPropertyValue<string>("Note2");                         
			set => SetPropertyValue<string>("Note2", value); 
			
        }
		//Tooltip for Object
		public object Note2ToolTipControllerText(View view)
        {
        //    if (Note2 != null) 
		//			return Note2;
            return null;
        }
		//Get Default Value
        public string GetDefaultNote2(View view = null)
        { 
			return Note2;
        }
		//Set Default Value
		public void SetDefaultNote2(View view = null)
        {
            //if (Note2 is null){
            //    var result = GetDefaultNote2(view);
            //    if (result != null && result != Note2){
			//          Note2 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Note2IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNote2();
				//if (result != null && Note2 != null){
				//	return !Note2.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _likeword;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Từ vựng")]
        [ToolTip("Từ vựng")]
		//[Index(21)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? LikeWord
        { 
		    get => GetPropertyValue<int?>("LikeWord");                         
			set => SetPropertyValue<int?>("LikeWord", value); 
			
        }
		//Tooltip for Object
		public object LikeWordToolTipControllerText(View view)
        {
            #region 1498ImportCode 
            if (Video != null && !string.IsNullOrEmpty(Name))
            {
                var likeWordList = GetLikeWordList();
                if (likeWordList?.Count > 0)
                {
                    return "<size=20>" + string.Join(", ", likeWordList) + "</size>";
                }
                //if (Video.LanguageOrigin is null)
                //    return null;                
                //var dictionary = Video.GetDictionary(null);
                //if (dictionary is null)
                //    return null;
                //var lowerName = Name.ToLower();
                //var termNameLength = lowerName.Split(' ').Length;
                //var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(lowerName);

                //if (dictionary.ContainsKey(termNameLength) && dictionary[termNameLength].ContainsKey(termNoneUnicode))
                //{
                //    return "<size=20>" + string.Join(", ", dictionary[termNameLength][termNoneUnicode]) + "</size>";
                //}
            }
#endregion 1498ImportCode
            return null;
        }
		//Get Default Value
        public int? GetDefaultLikeWord(View view = null)
        { 
			return LikeWord;
        }
		//Set Default Value
		public void SetDefaultLikeWord(View view = null)
        {
            //if (LikeWord is null){
            //    var result = GetDefaultLikeWord(view);
            //    if (result != null && result != LikeWord){
			//          LikeWord = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LikeWordIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLikeWord();
				//if (result != null && LikeWord != null){
				//	return !LikeWord.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Language _language;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngôn ngữ")]
        [ToolTip("Ngôn ngữ")]
		//[Index(22)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LanguageCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Language Language
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Language>("Language");                         
			set => SetPropertyValue<Module.BusinessObjects.Language>("Language", value); 
			
        }
		//Tooltip for Object
		public object LanguageToolTipControllerText(View view)
        {
        //    if (Language != null) 
		//			return Language;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Language GetDefaultLanguage(View view = null)
        { 
			return Language;
        }
		//Set Default Value
		public void SetDefaultLanguage(View view = null)
        {
            //if (Language is null){
            //    var result = GetDefaultLanguage(view);
            //    if (result != null && result != Language){
			//          Language = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LanguageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLanguage();
				//if (result != null && Language != null){
				//	return !Language.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LanguageCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Language));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0410ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
            #endregion 0410ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultTranslate(View view = null);
        //SetDefaultLanguageTranslate(View view = null);
        //SetDefaultGoogleTranslate(View view = null);
        //SetDefaultWordType(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultVideo(View view = null);
        //SetDefaultTermType(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultFlag(View view = null);
        //SetDefaultNumberValue(View view = null);
        //SetDefaultDateValue(View view = null);
        //SetDefaultLength(View view = null);
        //SetDefaultWordQuantity(View view = null);
        //SetDefaultStatus(View view = null);
        //SetDefaultNote(View view = null);
        //SetDefaultLikeTerm(View view = null);
        //SetDefaultFlag2(View view = null);
        //SetDefaultOverlap(View view = null);
        //SetDefaultNote2(View view = null);
        //SetDefaultLikeWord(View view = null);
        //SetDefaultLanguage(View view = null);
			
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
            #region 0513ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0513ImportCode
//            Update = (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
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
            Session.Delete(this.TermLocationList);				
  
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
				
                    case nameof(GoogleTranslate):
                        OnChangedGoogleTranslate(oldValue, newValue);
                        break;
				
                    case nameof(Name):
                        OnChangedName(oldValue, newValue);
                        break;
				
                    case nameof(Translate):
                        OnChangedTranslate(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedGoogleTranslate(object oldValue, object newValue)
        {
            #region 0935ImportCode
            if (newValue is null) return;
                    if (Quantity == 1)
                    {
                        //2023-08-01: Dịch và Máy dịch của Thuật ngữ khi thay đổi sẽ ghi đè Dịch và Máy dịch của thuật vị nếu SL Thuật vị =1
                        foreach (var termLocation in TermLocationList)
                        {
                            termLocation.MachineTranslate = GoogleTranslate;
                        }
                    }            
            #endregion 0935ImportCode
        }               
        private void OnChangedName(object oldValue, object newValue)
        {
            #region 0918ImportCode
            SetDefaultLength();
if (newValue is null) return;
if (Name.Contains(' '))
                    Name = Name.Replace(' ', ' ');            
            #endregion 0918ImportCode
        }               
        private void OnChangedTranslate(object oldValue, object newValue)
        {
            #region 0934ImportCode
            if (newValue is null) return;
                    if(Quantity == 1)
                    {
                        //2023-08-01: Dịch và Máy dịch của Thuật ngữ khi thay đổi sẽ ghi đè Dịch và Máy dịch của thuật vị nếu SL Thuật vị =1
                        foreach(var termLocation in TermLocationList)
                        {
                            termLocation.Translate = Translate;
                        }
                    }            
            #endregion 0934ImportCode
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
			//	SetDefaultTermLocationList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1073ImportCode
		public bool GetDefaultOverlap(View view = null)
        {
            //Code: 1073            Oid: 7cb30f8b-f63b-4bd2-a5a3-4ef5ebfb7662
            return TermLocationList?.FirstOrDefault(m =>m.Overlap) != null;
        }
#endregion 1073ImportCode
#region 0957ImportCode
		public decimal? GetDefaultNumberValue(View view = null)
        {
            //Code: 0957            Oid: 5ac80d81-b7c3-4001-9cf7-f2c2349cd6a2
            if (!string.IsNullOrEmpty(Name))
            {
                return Tools.TryConvertTextToNumber(Name);
            }
            
            return null;
        }
#endregion 0957ImportCode
#region 0948ImportCode
		public int? GetDefaultLength(View view = null)
        {
            //Code: 0948            Oid: ea4758cc-78b5-4407-8305-9be0dd66ba3d
            if (!string.IsNullOrEmpty(Name))
                return Name.Length;
            return null;       
        }
#endregion 0948ImportCode
#region 0949ImportCode
		public void SetDefaultLength(View view = null)
        {
            //Code: 0949            Oid: ffcabccb-b083-4384-abb7-4143d68489cb
            Length = GetDefaultLength();
        }
#endregion 0949ImportCode
#region 0129ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0129            Oid: 40b70e30-391a-47bd-91f3-42a048f6b222
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0129ImportCode
#region 0599ImportCode
		public void TermCount()
        {
            //Code: 0599            Oid: eda77880-8d23-4102-8fe2-a9273ddb5c54
            if (Video is null || string.IsNullOrEmpty(Name))
                return;
            //Tạo danh sách thuật ngữ con nằm trong thuật ngữ cha
            var parrentTerms = new System.Collections.Generic.List<Term>();            
            foreach (Term otherTerm in Video.TermList)
            {
                if (otherTerm.Oid.Equals(Oid))
                    continue;
                if (otherTerm.CheckTermInTerm(this))
                {
                    // Nếu term khác có chứa select thì term khác phải giảm số lượng                                                      
                    parrentTerms.Add(otherTerm);
                }

            }

            int quantity = 0;
            int firstPosition = 0;
            int position = 0;
            //string termName = Name.ToLower();
            string termName = Name;
            foreach (var audio in Video.AudioList.OrderBy(m => m.Start))
            {
                if (string.IsNullOrEmpty(audio.Content))
                    continue;
                //Kiểm tra xem thuật ngữ có sẵn thì loại
                //string content = audio.Content.ToLower();
                //Xóa bỏ 2 dấu cách liền nhau                    
                //Cắt theo dòng
                var rows = audio.Content.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var row in rows)
                {
                    var tempContent = row.Replace("  ", " ");
                    int termIndex = tempContent.IndexOf(termName, System.StringComparison.OrdinalIgnoreCase);
                    int startIndex = 0;
                    while (termIndex >= 0)
                    {

                        bool validate = true;
                        //Kiểm tra xem trước đấy có phải dấu trắng hoặc ký tự đặc biệt không
                        if (termIndex >= 1 && char.IsLetterOrDigit(tempContent[termIndex - 1]))
                            validate = false;
                        //Kiểm tra sau đấy có phải dấu trắng hoặc ký tự đặc biệt không
                        startIndex = termIndex + termName.Length;
                        if (validate && startIndex < tempContent.Length && char.IsLetterOrDigit(tempContent[startIndex]))
                        {
                            validate = false;
                        }
                        if (validate && parrentTerms.Count > 0)
                        {
                            //Nếu vị trí là vị trí của cha thì bỏ qua
                            foreach (var parentTerm in parrentTerms)
                            {
                                if (string.IsNullOrEmpty(parentTerm.Name))
                                    continue;
                                var beforeIndex = parentTerm.Name.IndexOf(Name, StringComparison.OrdinalIgnoreCase);
                                var afterIndex = parentTerm.Name.Length - beforeIndex;
                                if (termIndex >= beforeIndex)
                                {
                                    var parentTermIndex = tempContent.IndexOf(parentTerm.Name, termIndex - beforeIndex);
                                    if (parentTermIndex >= termIndex - beforeIndex && parentTermIndex <= termIndex + afterIndex)
                                    {
                                        if (!(parentTermIndex + parentTerm.Name.Length < tempContent.Length
                                            && char.IsLetterOrDigit(tempContent[parentTermIndex + parentTerm.Name.Length])))
                                        {
                                            validate = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (validate)
                        {
                            quantity++;
                            if (firstPosition == 0)
                            {
                                string beforeContent = row.Substring(0, termIndex);
                                //Thêm 1 là thêm vị trí hiện tại
                                firstPosition = position + beforeContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length + 1;
                            }
                        }
                        termIndex = tempContent.IndexOf(termName, startIndex);
                    }
                    position += row.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;              
                }
            }
            if (quantity > 0)
            {
                Quantity = quantity;
                //Position = firstPosition;
                //2023-06-26: Chỉ giảm trừ số lượng thuật ngữ sẽ select
                //Xử lý những thuật ngữ trùng
                //foreach (var term in Video.TermList)
                //{
                //    string childTermName = term.Name;
                //    if (this.CheckTermInTerm(term))
                //    {
                //        //kiểm tra xem Thuật ngữ hiện tại có chứa thuật ngữ khác đã có chưa
                //        term.Quantity -= Quantity; 
                //    }
                //    else if (term.CheckTermInTerm(this))
                //    {
                //        //Kiểm tra xem thuật ngữ đã có chứa thuật ngữ hiện tại không
                //        Quantity -= term.Quantity;
                //    }
                //}
            }        
        }
#endregion 0599ImportCode
#region 0888ImportCode
		public Module.BusinessObjects.Term GetAudioFromPosition()
        {
            //Code: 0888            Oid: 6095cf0e-353b-4281-94fb-353c4752ed14
            //2023-06-23: Bỏ trường vị trí
            //if (!string.IsNullOrEmpty(Name) && Video != null && Position != null && Position != 0)
            //{
            //    int position = 0;
            //    foreach (var audio in Video.AudioList.OrderBy(m => m.Start))
            //    {
            //        if (string.IsNullOrEmpty(audio.Content))
            //            continue;
            //        //string content = audio.Content.Replace("  ", " ").Replace("\r\n", ". ").Replace("\r", " ").Replace("\n", " ").Replace(" - ", " ").Replace("...", ".").Replace("..", ".").Replace(".,", ",");
            //        //if (!char.IsLetterOrDigit(content[content.Length - 1]))
            //        //    content = content.Substring(0, content.Length - 1);
            //        var contentArray = audio.Content.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            //        var contentLength = contentArray.Length;
            //        if (position <= Position && Position <= position + contentLength)
            //        {
            //            return audio;
            //        }
            //        position += contentLength;
            //    }
            //}
            return null;
        }
#endregion 0888ImportCode
#region 0160ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0160            Oid: f1dac60c-604b-4f64-860a-3c943c780a7b
            Update = GetDefaultUpdate();
        }
#endregion 0160ImportCode
        #endregion
//Mã nguồn bổ sung
#region TermImportCode
        public System.Collections.Generic.List<string> GetLikeTermList(bool firstOnly = false)
        {
            if (Video != null && !string.IsNullOrEmpty(Name))
            {
                var termSelectNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(Name);
                var likeTermList = new System.Collections.Generic.List<string>();
                foreach (var term in Video.TermList)
                {
                    if (!term.Oid.Equals(Oid) && !string.IsNullOrEmpty(term.Name))
                    {
                        var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(term.Name);
                        if (termNoneUnicode.Equals(termSelectNoneUnicode, System.StringComparison.OrdinalIgnoreCase))
                        {
                            likeTermList.Add(term.Name);
                            if (firstOnly)
                                return likeTermList;
                        }

                    }
                }
                return likeTermList;
            }
            return null;
        }

        public System.Collections.Generic.List<string> GetLikeWordList(System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> existDictionary = null)
        {
            if (Video != null && !string.IsNullOrEmpty(Name))
            {
                var dictionary = existDictionary;
                if (dictionary is null)
                {
                    if (Video.LanguageOrigin is null)
                        return null;
                    dictionary = Video.GetDictionary();
                    if (dictionary is null)
                        return null;
                }
                var lowerName = Name.ToLower();
                var termNameLength = lowerName.Split(' ').Length;
                var termNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(lowerName);

                if (dictionary.ContainsKey(termNameLength) && dictionary[termNameLength].ContainsKey(termNoneUnicode))
                {
                    return dictionary[termNameLength][termNoneUnicode];
                }
            }
            return null;
        }
        public void AddTextNode(char charTag, string innerText, bool summaryText = true)
        {
            if (!string.IsNullOrEmpty(Note))
            {
                //Xóa ghi chú tag trước đó
                Note = Module.Helpers.TextHelper.GetTextWithTagNode(Note, charTag, false);
            }
            Note = Module.Helpers.TextHelper.AddTextWithTagNode(Note, charTag, innerText, summaryText);
        }

        public bool CheckTermInTerm(Term childTerm)
        {
            if (Oid.Equals(childTerm.Oid))
                return false;
            //kiểm tra xem Thuật ngữ này có nằm trong các thuật ngữ khác không
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(childTerm.Name))
            {
                //Nếu tên không có dấu cách thì không phải từ ghép
                if (!Name.Contains(" ") && !Name.Contains(" "))
                    return false;
                var index = Name.IndexOf(childTerm.Name, System.StringComparison.OrdinalIgnoreCase);
                if (index < 0)
                {
                    return false;
                }
                else if (index == 0)
                {
                    if (childTerm.Name.Length == Name.Length)
                        return true;
                    //Nếu chứa ngay dòng đầu và sau là dấu cách                    
                    return char.IsWhiteSpace(Name[index + childTerm.Name.Length]);
                }
                if (index > 0)
                {
                    if (!char.IsWhiteSpace(Name[index - 1]))
                    {
                        //Nếu từ trước không phải là ký tự trắng
                        return false;
                    }
                    else if (index + childTerm.Name.Length > Name.Length)
                    {
                        //Nếu từ sau không phải là ký tự trắng
                        return char.IsWhiteSpace(Name[index + childTerm.Name.Length]);
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [Browsable(false)]
        [ImmediatePostData]
        public System.Collections.Generic.IList<string> TranslateDataSource
        {
            get
            {
                if (string.IsNullOrEmpty(Note))
                    return null;
                var result = new System.Collections.Generic.List<string>();
                var data = Note.Replace("[", ";[").Replace("]", "];");
                var dataList = data.Split(';', StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in dataList)
                {
                    if (item.StartsWith('[') && item.EndsWith(']'))
                    {
                        result.Add(item.Substring(1, item.Length - 2));
                    }
                }
                return result;
            }
        }
#endregion TermImportCode
		 		 
    }
}
