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
    [ModelDefault("Caption", "Thuật vị"), ImageName("TermLocation")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("TermLocation Translate None_Disable__" , TargetItems = "Translate" , Criteria = "[Flag] = True",AppearanceItemType = "ViewItem", Enabled = false )]
	[Appearance("TermLocation Sentence, Flag, Element, Term, Location None_None__Color [A=255, R=255, G=0, B=0]" , TargetItems = "Sentence, Flag, Element, Term, Location" , Criteria = "[Flag] = True",AppearanceItemType = "ViewItem", FontColor = "#FF0000" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
    [ShowToolTipAttribute(TargetItems = nameof(Location)+ "," + nameof(MachineTranslate)+ "," + nameof(TranslateLocation)+ "," + nameof(Overlap))]
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Length))]
 
	[MobileColumnAttribute(Context = "TermLocation_ListView", TargetItems = nameof(Translate))]
	[MobileColumnAttribute(Context = "TermLocation_LookupListView", TargetItems = nameof(Location)+ "," + nameof(Term))]
	[MobileColumnAttribute(Context = "TermLocation_TermLocations_ListView", TargetItems = nameof(Translate))]
	[MobileColumnAttribute(Context = "Term_TermLocationList_ListView", TargetItems = nameof(Translate))]
	[MobileColumnAttribute(Context = "Audio_TermLocationList_ListView", TargetItems = nameof(Translate))]
	[MobileColumnAttribute(Context = "Video_TermLocationList_ListView", TargetItems = nameof(Translate))]
	[DefaultProperty("Translate")]
 
[OptimisticLocking(true)]
    public partial class TermLocation:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public TermLocation(Session session)
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
               

		//private Module.BusinessObjects.Term _term;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thuật ngữ")]
        [ToolTip("Thuật ngữ")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TermCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Term-TermLocationList")]
	 
	    [RuleRequiredField("Required TermLocation_Term", DefaultContexts.Save, TargetCriteria = "IsNullOrEmpty(MachineTranslate)")]
		public Module.BusinessObjects.Term Term
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Term>("Term");                         
			set => SetPropertyValue<Module.BusinessObjects.Term>("Term", value); 
			
        }
		//Tooltip for Object
		public object TermToolTipControllerText(View view)
        {
        //    if (Term != null) 
		//			return Term;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Term GetDefaultTerm(View view = null)
        { 
			return Term;
        }
		//Set Default Value
		public void SetDefaultTerm(View view = null)
        {
            //if (Term is null){
            //    var result = GetDefaultTerm(view);
            //    if (result != null && result != Term){
			//          Term = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TermIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTerm();
				//if (result != null && Term != null){
				//	return !Term.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TermCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Term));
            }
        }
	
       
		//private int? _location;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Vị trí")]
        [ToolTip("Vị trí")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Location
        { 
		    get => GetPropertyValue<int?>("Location");                         
			set => SetPropertyValue<int?>("Location", value); 
			
        }
		//Tooltip for Object
		public object LocationToolTipControllerText(View view)
        {
            #region 0590ImportCode 
            if (Audio is null)
                return "Lỗi: Không tìm thấy vị trí thành phần hợp lệ";
            if (string.IsNullOrEmpty(Audio.Content) && Term.Language == Term.Video.LanguageOrigin)
                return "Lỗi: Nội dung thành phần bị trống";
            if (string.IsNullOrEmpty(Audio.Subtitle) && Term.Language == Term.Video.LanguageTranslate)
                return "Lỗi: Nội dung thành phần bị trống";
            //2023-06-07: dấu ngắt câu có thể là: Xuống dòng, dấu chấm, ?, !
            string[] newLineText = new string[] { ". ", "?", "!", "\r\n" };
            string content = Audio.Content;
            string subtitle = Audio.Subtitle;

            if (Term.Language == Term.Video.LanguageTranslate)
            {
                content = Audio.Subtitle;
                subtitle = Audio.Content;
            }

            var rows = content.Split(newLineText, System.StringSplitOptions.RemoveEmptyEntries);
            if (Sentence != null && Sentence > 0 && Sentence - 1 < rows.Length)
            {
                var parrentTerms = new System.Collections.Generic.List<string>();
                if (Term != null && Term.Video != null)
                {
                    foreach (Term otherTerm in Term.Video.TermList)
                    {
                        if (otherTerm.Oid.Equals(Oid) || string.IsNullOrEmpty(otherTerm.Name))
                            continue;
                        if (otherTerm.CheckTermInTerm(Term))
                        {
                            // Nếu term khác có chứa select thì term khác phải giảm số lượng                                                      
                            parrentTerms.Add(otherTerm.Name);
                        }

                    }
                }

                //int? position = null;
                //if(Sentence.Value > 1)
                //{
                //    position = 0;
                //    for (int i = 0; i< rows.Length;i++)
                //    {
                //        if(i < Sentence.Value)
                //        {
                //            position += rows[i].Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //    if (Location != null)
                //        position += Location;
                //}
                //string result = Module.Helpers.TextHelper.HightlightText(rows[Sentence.Value - 1].Trim(), Term.Name, parrentTerms.ToArray(), position != null ? position : Location);
                int? sentenceLocaltion = Location;
                //2023-11-06: vị trí của từ là vị trí của câu luôn
                //if(sentenceLocaltion != null && Sentence.Value > 1)
                //{
                //    for(int j = 0; j < Sentence.Value - 1; j++)
                //    {
                //        var rowCount = rows[j].Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                //        if (rowCount > sentenceLocaltion)
                //            break;
                //        sentenceLocaltion -= rowCount;
                //    }
                //}
                string termName = Term != null ? Term.Name : MachineTranslate;
                string result = Module.Helpers.TextHelper.HightlightText(rows[Sentence.Value - 1].Replace("  ", " ").Trim(), termName, parrentTerms.ToArray(), sentenceLocaltion);
                if (!result.Contains("<b>"))
                {
                    //Dịch chuyển vị trí để hightlight cho đúng
                    result = Module.Helpers.TextHelper.HightlightText(rows[Sentence.Value - 1].Replace("  ", " ").Trim(), termName, parrentTerms.ToArray(), sentenceLocaltion - 1);
                }
                if (!result.Contains("<b>"))
                {
                    //Dịch chuyển vị trí để hightlight cho đúng
                    result = Module.Helpers.TextHelper.HightlightText(rows[Sentence.Value - 1].Replace("  ", " ").Trim(), termName, parrentTerms.ToArray(), sentenceLocaltion - 2);
                }
                if (!result.Contains("<b>"))
                {
                    //Dịch chuyển vị trí để hightlight cho đúng
                    result = Module.Helpers.TextHelper.HightlightText(rows[Sentence.Value - 1].Replace("  ", " ").Trim(), termName, parrentTerms.ToArray(), sentenceLocaltion + 1);
                }
                if (!result.Contains("<b>") && Term is null && !string.IsNullOrEmpty(Translate))
                {
                    //Dịch chuyển vị trí để hightlight cho đúng
                    result = Module.Helpers.TextHelper.HightlightText(rows[Sentence.Value - 1].Replace("  ", " ").Trim(), Translate, parrentTerms.ToArray(), sentenceLocaltion + 0);
                    if (!result.Contains("<b>"))
                    {
                        //Dịch chuyển vị trí để hightlight cho đúng
                        result = Module.Helpers.TextHelper.HightlightText(rows[Sentence.Value - 1].Replace("  ", " ").Trim(), Translate, parrentTerms.ToArray(), sentenceLocaltion + 1);
                    }
                    if (!result.Contains("<b>"))
                    {
                        //Dịch chuyển vị trí để hightlight cho đúng
                        result = Module.Helpers.TextHelper.HightlightText(rows[Sentence.Value - 1].Replace("  ", " ").Trim(), Translate, parrentTerms.ToArray(), sentenceLocaltion - 1);
                    }
                }
                string resultTranslate = null;
                if (!string.IsNullOrEmpty(subtitle))
                {

                    string subtitle1 = "";
                    var subtitleRows = subtitle.Split(newLineText, System.StringSplitOptions.RemoveEmptyEntries);
                    if (Sentence - 1 < subtitleRows.Length)
                    {
                        subtitle1 = subtitleRows[Sentence.Value - 1];
                    }
                    else
                    {
                        subtitle1 = subtitle;
                    }


                    if (!string.IsNullOrEmpty(Translate))
                    {
                        //2024-08-14: Phần hover dịch hiện màu như nội dung
                        //resultTranslate = "<color=gray>" + Module.Helpers.TextHelper.HightlightText(subtitle, Translate, null, TranslateLocation) + "</color>";
                        resultTranslate = Module.Helpers.TextHelper.HightlightText(subtitle1, Translate, null, TranslateLocation);

                    }
                    // 2024 - 08 - 14: Phần hover dịch hiện màu như nội dung
                    if (!string.IsNullOrEmpty(MachineTranslate) && (string.IsNullOrEmpty(resultTranslate) || !resultTranslate.Contains("<b>")))
                        //resultTranslate = "<color=gray>" + Module.Helpers.TextHelper.HightlightText(subtitle, MachineTranslate, null, TranslateLocation) + "</color>";
                        resultTranslate = Module.Helpers.TextHelper.HightlightText(subtitle1, MachineTranslate, null, TranslateLocation);
                    if (string.IsNullOrEmpty(resultTranslate))
                        //resultTranslate = $"<color=gray>{subtitle}</color>";
                        resultTranslate = subtitle1;
                }
                if (!string.IsNullOrEmpty(resultTranslate))
                {
                    result += System.Environment.NewLine + resultTranslate;
                }
                return result;
            }
            return null;
#endregion 0590ImportCode
            return null;
        }
		//Get Default Value
        public int? GetDefaultLocation(View view = null)
        { 
			return Location;
        }
		//Set Default Value
		public void SetDefaultLocation(View view = null)
        {
            //if (Location is null){
            //    var result = GetDefaultLocation(view);
            //    if (result != null && result != Location){
			//          Location = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LocationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLocation();
				//if (result != null && Location != null){
				//	return !Location.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _sentence;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Câu")]
        [ToolTip("Câu")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Sentence
        { 
		    get => GetPropertyValue<int?>("Sentence");                         
			set => SetPropertyValue<int?>("Sentence", value); 
			
        }
		//Tooltip for Object
		public object SentenceToolTipControllerText(View view)
        {
        //    if (Sentence != null) 
		//			return Sentence;
            return null;
        }
		//Get Default Value
        public int? GetDefaultSentence(View view = null)
        { 
			return Sentence;
        }
		//Set Default Value
		public void SetDefaultSentence(View view = null)
        {
            //if (Sentence is null){
            //    var result = GetDefaultSentence(view);
            //    if (result != null && result != Sentence){
			//          Sentence = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SentenceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSentence();
				//if (result != null && Sentence != null){
				//	return !Sentence.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _translate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dịch")]
        [ToolTip("Dịch")]
		//[Index(3)]		

 		[Size(150)]
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

	
       
		//private string _machinetranslate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Máy dịch")]
        [ToolTip("Máy dịch")]
		//[Index(4)]		

 		[Size(150)]
	    [RuleRequiredField("Required TermLocation_MachineTranslate", DefaultContexts.Save, TargetCriteria = "Term is null")]
		public string MachineTranslate
        { 
		    get => GetPropertyValue<string>("MachineTranslate");                         
			set => SetPropertyValue<string>("MachineTranslate", value); 
			
        }
		//Tooltip for Object
		public object MachineTranslateToolTipControllerText(View view)
        {
            #region 1457ImportCode 
if (Audio is null)
    return "Lỗi: Không tìm thấy vị trí thành phần hợp lệ";
if (string.IsNullOrEmpty(Audio.Content))
    return "Lỗi: Nội dung thành phần bị trống";
//2023-06-07: dấu ngắt câu có thể là: Xuống dòng, dấu chấm, ?, !
//058: hover Máy dịch: hiện 3 Thành phần trên/giữa/dưới : đậm và phóng to 200% Máy dịch/Dịch trong thành phần để quan sát sửa lỗi cho dễ
var audioList = Audio.Video.GetAudioListWithSort(Audio.BookMark, true, Audio.TranslateObject);
string hoverText = "<size=18>";
//bool isSubtitle = TermLocation.Term != null;
var beforeAudio = audioList.LastOrDefault(x => x.Start < Audio.Start);
if (beforeAudio != null)
    hoverText += "<color=gray>" + beforeAudio.Content + "</color>";
if (!string.IsNullOrEmpty(hoverText))
    hoverText += "\r\n";
string termName = Term != null ? Term.Name : MachineTranslate;
hoverText += Module.Helpers.TextHelper.HightlightText(Audio.Content, termName, null, null, null);            
var afterAudio = audioList.FirstOrDefault(x => x.Start > Audio.Start);
if (afterAudio != null)
{
    if (!string.IsNullOrEmpty(hoverText))
        hoverText += "\r\n";
    hoverText += "<color=gray>" + afterAudio.Content + "</color>";
}
if (!string.IsNullOrEmpty(Translate) && !string.IsNullOrEmpty(Audio.Subtitle))
{
    if (!string.IsNullOrEmpty(hoverText))
        hoverText += "\r\n";
    if (beforeAudio != null && !string.IsNullOrEmpty(beforeAudio.Subtitle))
        hoverText += "\r\n<color=gray>" + beforeAudio.Subtitle + "</color>";
    hoverText += "\r\n" + Module.Helpers.TextHelper.HightlightText(Audio.Subtitle, Translate, null, null, null);
    if (afterAudio != null && !string.IsNullOrEmpty(afterAudio.Subtitle))
        hoverText += "\r\n<color=gray>" + afterAudio.Subtitle + "</color>";
}
return hoverText + "</size>";
#endregion 1457ImportCode
            return null;
        }
		//Get Default Value
        public string GetDefaultMachineTranslate(View view = null)
        { 
			return MachineTranslate;
        }
		//Set Default Value
		public void SetDefaultMachineTranslate(View view = null)
        {
            //if (MachineTranslate is null){
            //    var result = GetDefaultMachineTranslate(view);
            //    if (result != null && result != MachineTranslate){
			//          MachineTranslate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MachineTranslateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMachineTranslate();
				//if (result != null && MachineTranslate != null){
				//	return !MachineTranslate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _flag;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ")]
        [ToolTip("Cờ")]
		//[Index(5)]		
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

	
       
		//private int? _translatelocation;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vị trí dịch")]
        [ToolTip("Vị trí dịch")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? TranslateLocation
        { 
		    get => GetPropertyValue<int?>("TranslateLocation");                         
			set => SetPropertyValue<int?>("TranslateLocation", value); 
			
        }
		//Tooltip for Object
		public object TranslateLocationToolTipControllerText(View view)
        {
            #region 1399ImportCode 
            if (Term is null || Audio is null || string.IsNullOrEmpty(Audio.Subtitle))
            {
                return null;
            }
            string translate = !string.IsNullOrEmpty(MachineTranslate) ? MachineTranslate : Term.GoogleTranslate;
            if (string.IsNullOrEmpty(translate))
                return null;
            var content = Services.TermLocationService.GetSentenceTextFromContent(this, Audio.Subtitle);
            if (TranslateLocation != null)
            {
                int space = 0;
                //Xóa bỏ 2 dấu cách liền nhau;                
                var hover = "";
                int translateLength = translate.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                int foundIndex = translateLength;
                for (int i = 0; i < content.Length; i++)
                {

                    if (space + 1 == TranslateLocation && foundIndex == translateLength)
                    {
                        hover += "<b>";
                        foundIndex--;
                    }
                    hover += content[i];
                    if (content[i] == ' ')
                    {

                        if (foundIndex == 0)
                            hover += "</b>";
                        if (foundIndex < translateLength)
                            foundIndex--;
                        space++;
                    }
                }
                if (hover.Contains("<b>") && !hover.Contains("</b>"))
                {
                    hover += "</b>";
                }
                return hover;
            }
            else
            {
                int index = 0;
                var indexList = new System.Collections.Generic.List<int>();
                while (index < content.Length)
                {
                    var foundIndex = Module.Helpers.TextHelper.GetIndexWordInContent(translate, content, null, index);
                    if (foundIndex < 0)
                        break;
                    var childContent = content.Substring(0, foundIndex).Trim();
                    indexList.Add(childContent.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length + 1);
                    index = foundIndex + translate.Length;
                }
                if (indexList.Count == 0 && !string.IsNullOrEmpty(Translate))
                {
                    index = 0;
                    while (index < content.Length)
                    {
                        var foundIndex = Module.Helpers.TextHelper.GetIndexWordInContent(Translate, content, null, index);
                        if (foundIndex < 0)
                            break;
                        var childContent = content.Substring(0, foundIndex).Trim();
                        indexList.Add(childContent.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length + 1);
                        index = foundIndex + Translate.Length;
                    }
                }
                if (indexList.Count > 0)
                    return string.Join(", ", indexList);

            }
            return null;
#endregion 1399ImportCode
            return null;
        }
		//Get Default Value
		//Set Default Value
		public void SetDefaultTranslateLocation(View view = null)
        {
            //if (TranslateLocation is null){
            //    var result = GetDefaultTranslateLocation(view);
            //    if (result != null && result != TranslateLocation){
			//          TranslateLocation = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TranslateLocationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTranslateLocation();
				//if (result != null && TranslateLocation != null){
				//	return !TranslateLocation.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _overlap;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đè")]
        [ToolTip("Đè")]
		//[Index(7)]		
		public bool Overlap
        { 
		    get => GetPropertyValue<bool>("Overlap");                         
			set => SetPropertyValue<bool>("Overlap", value); 
			
        }
		//Tooltip for Object
		public object OverlapToolTipControllerText(View view)
        {
            #region 1061ImportCode 
            var overlapList = Services.TermLocationService.GetOverlap(this, false);
            if (overlapList != null)
                return string.Join("\r\n", overlapList.Where(m => m.Term != null && !string.IsNullOrEmpty(m.Term.Name)).OrderBy(m => m.Location).Select(m => m.Term.NumberValue > 0 ? m.Term.NumberValueToolTipControllerText(null) : m.Term.Name).ToList());
#endregion 1061ImportCode
            return null;
        }
		//Get Default Value
        public bool GetDefaultOverlap(View view = null)
        { 
			return Overlap;
        }
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

	
       
		//private Module.BusinessObjects.Audio _audio;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thành phần")]
        [ToolTip("Thành phần")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(AudioCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Audio-TermLocationList")]
	 
		public Module.BusinessObjects.Audio Audio
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Audio>("Audio");                         
			set => SetPropertyValue<Module.BusinessObjects.Audio>("Audio", value); 
			
        }
		//Tooltip for Object
		public object AudioToolTipControllerText(View view)
        {
        //    if (Audio != null) 
		//			return Audio;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Audio GetDefaultAudio(View view = null)
        { 
			return Audio;
        }
		//Set Default Value
		public void SetDefaultAudio(View view = null)
        {
            //if (Audio is null){
            //    var result = GetDefaultAudio(view);
            //    if (result != null && result != Audio){
			//          Audio = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AudioIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAudio();
				//if (result != null && Audio != null){
				//	return !Audio.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator AudioCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Audio));
            }
        }
	
       
		//private bool _replacetranslate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thay dịch")]
        [ToolTip("Thay dịch")]
		//[Index(9)]		
		public bool ReplaceTranslate
        { 
		    get => GetPropertyValue<bool>("ReplaceTranslate");                         
			set => SetPropertyValue<bool>("ReplaceTranslate", value); 
			
        }
		//Tooltip for Object
		public object ReplaceTranslateToolTipControllerText(View view)
        {
        //    if (ReplaceTranslate != null) 
		//			return ReplaceTranslate;
            return null;
        }
		//Get Default Value
        public bool GetDefaultReplaceTranslate(View view = null)
        { 
			return ReplaceTranslate;
        }
		//Set Default Value
		public void SetDefaultReplaceTranslate(View view = null)
        {
            //if (ReplaceTranslate is null){
            //    var result = GetDefaultReplaceTranslate(view);
            //    if (result != null && result != ReplaceTranslate){
			//          ReplaceTranslate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ReplaceTranslateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultReplaceTranslate();
				//if (result != null && ReplaceTranslate != null){
				//	return !ReplaceTranslate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _length;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ký tự")]
        [ToolTip("Ký tự")]
		//[Index(10)]		
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

	
       
		//private string _note;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(11)]		

 		[Size(250)]
	    [NonPersistent()]
	    [NotMapped()]
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

	
       
		//private bool _flag2;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ 2")]
        [ToolTip("Cờ 2")]
		//[Index(12)]		
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

	
       
		//private string _note2;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú 2")]
        [ToolTip("Ghi chú 2")]
		//[Index(13)]		

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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultTerm(View view = null);
        //SetDefaultLocation(View view = null);
        //SetDefaultSentence(View view = null);
        //SetDefaultTranslate(View view = null);
        //SetDefaultMachineTranslate(View view = null);
        //SetDefaultFlag(View view = null);
        //SetDefaultTranslateLocation(View view = null);
        //SetDefaultOverlap(View view = null);
        //SetDefaultAudio(View view = null);
        //SetDefaultReplaceTranslate(View view = null);
        //SetDefaultLength(View view = null);
        //SetDefaultNote(View view = null);
        //SetDefaultFlag2(View view = null);
        //SetDefaultNote2(View view = null);
			
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
				
                    case nameof(MachineTranslate):
                        OnChangedMachineTranslate(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedMachineTranslate(object oldValue, object newValue)
        {
            #region 0915ImportCode
            if (newValue is null) return;
if (MachineTranslate.Length > 150)
    MachineTranslate = MachineTranslate.Substring(0, 150);
SetDefaultLength();            
            #endregion 0915ImportCode
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
#region 0950ImportCode
		public int? GetDefaultTranslateLocation(View view = null)
        {
            //Code: 0950            Oid: 98f214ad-347f-43df-ae64-80795343b453
                        if (!string.IsNullOrEmpty(MachineTranslate))
            {
                //Ưu tiên dịch máy
                string translate = !string.IsNullOrEmpty(MachineTranslate) ? MachineTranslate : Translate;
                if (string.IsNullOrEmpty(translate) || Term is null)
                    return null;
                var audio = GetAudioFromElement();
                if (audio is null)
                    return null;
                if (string.IsNullOrEmpty(audio.Subtitle))
                    return null;
                var index = Module.Services.TermLocationService.GetIndexTranslate(this, audio.Subtitle, translate);
                if (index == 0)
                    return 1;
                else if (index > 0)
                {
                    var firstText = audio.Subtitle.Substring(0, index);
                    var rows = firstText.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
                    int position = 0;
                    for (int m = 0; m < rows.Count(); m++)
                    {
                        var contents = rows[m].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                        //Vị trí của mảng nhỏ hơn 1 so với vị trí thực tế, nên vị trí của từ cũng là vị trí của mảng
                        position += contents.Length;
                    }
                    return position;
                }
            }
            return null;
        }
#endregion 0950ImportCode
#region 1439ImportCode
		public void SetDefaultLength(View view = null)
        {
            //Code: 1439            Oid: dad1c0bc-a456-41ac-b39e-9753f2316a50
            if(Length == null) Length = GetDefaultLength();

        }
#endregion 1439ImportCode
#region 0602ImportCode
		public Module.BusinessObjects.Audio GetAudioFromElement()
        {
            //Code: 0602            Oid: 30db4a48-fa5c-469c-82c1-c730505a60d9
            if (Audio != null)
                return Audio;
                        return null; 
        }
#endregion 0602ImportCode
#region 1438ImportCode
		public int? GetDefaultLength(View view = null)
        {
            //Code: 1438            Oid: e2438d0b-e086-4831-b33f-97a944579175
            if (!string.IsNullOrEmpty(MachineTranslate))
                return MachineTranslate.Length;
            return null;      
        }
#endregion 1438ImportCode
        #endregion
//Mã nguồn bổ sung
#region TermLocationImportCode
        [DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
        [VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Thuật vị liên quan")]
        public XPCollection<Module.BusinessObjects.TermLocation> TermLocations
        {
            get => new XPCollection<Module.BusinessObjects.TermLocation>(Audio.TermLocationList);

        }

        [Browsable(false)]
        [ImmediatePostData]
        public System.Collections.Generic.IList<string> TranslateDataSource
        {
            get
            {
                if (Term != null)
                    return Term.TranslateDataSource;
                return null;
            }
        }
#endregion TermLocationImportCode
		 		 
    }
}
