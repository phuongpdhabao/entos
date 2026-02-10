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
    [ModelDefault("Caption", "Thành phần"), ImageName("Audio")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
    [ShowToolTipAttribute(TargetItems = nameof(Start)+ "," + nameof(End)+ "," + nameof(VoiceSpeed)+ "," + nameof(Content)+ "," + nameof(Subtitle)+ "," + nameof(Spelling)+ "," + nameof(ParagraphStyle)+ "," + nameof(AudioRate)+ "," + nameof(Quantity)+ "," + nameof(TextRate)+ "," + nameof(LinePageContent)+ "," + nameof(LinePageTranslate)+ "," + nameof(LanguageTranslate))]
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update)+ "," + nameof(SubtitleTime)+ "," + nameof(SpellingTime)+ "," + nameof(AudioTime)+ "," + nameof(Order))]
 
	[MobileColumnAttribute(Context = "Media_AudioList_ListView", TargetItems = nameof(Start)+ "," + nameof(Content))]
	[MobileColumnAttribute(Context = "ElementTranslate_AudioList_ListView", TargetItems = nameof(Start)+ "," + nameof(Content))]
	[MobileColumnAttribute(Context = "Audio_LookupListView", TargetItems = nameof(Start)+ "," + nameof(Content))]
	[MobileColumnAttribute(Context = "Audio_ListView", TargetItems = nameof(Start)+ "," + nameof(Content))]
	[MobileColumnAttribute(Context = "Video_AudioList_ListView", TargetItems = nameof(Content)+ "," + nameof(Start))]
	[MobileColumnAttribute(Context = "Paragraph_AudioList_ListView", TargetItems = nameof(Start)+ "," + nameof(Content))]
	[MobileColumnAttribute(Context = "ElementBatch_AudioList_ListView", TargetItems = nameof(Content)+ "," + nameof(Start))]
	[DefaultProperty("Start")]
 
	[RuleCombinationOfPropertiesIsUnique("UniqueRule.Audio", DefaultContexts.Save, "ElementBatch, Order")]
[OptimisticLocking(true)]
    public partial class Audio:  DevExpress.Xpo.XPLiteObject , IQuantity, IReOrder , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Audio(Session session)
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
				if (ElementTranslateList.IsLoaded)
                {
                    if (ElementTranslateList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ElementTranslateList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ElementTranslateList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ElementTranslate>(CriteriaOperator.Parse("[Audio.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool elementtranslatelist = Session.Query<Module.BusinessObjects.ElementTranslate>().Where(x => x.Audio.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ElementTranslateList), elementtranslatelist);
                        if (elementtranslatelist)
                            return true;

                    }                    
                }				
                                
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
               

		//private TimeSpan? _start;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Bắt đầu")]
        [ToolTip("Bắt đầu")]
		//[Index(0)]		
		[RuleRequiredField("RequiredAudioStart", DefaultContexts.Save)]
	    [ModelDefault("MaskSettings", "AgAAAA9NYXNrTWFuYWdlclR5cGUAgwFEZXZFeHByZXNzLkRhdGEuTWFzay5UaW1lU3Bhbk1hc2tNYW5hZ2VyLCBEZXZFeHByZXNzLkRhdGEudjIyLjEsIFZlcnNpb249MjIuMS4zLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjg4ZDE3NTRkNzAwZTQ5YQRtYXNrBwIMW2QuXWhoOm1tOnNz")]
		public TimeSpan? Start
        { 
		    get => GetPropertyValue<TimeSpan?>("Start");                         
			set => SetPropertyValue<TimeSpan?>("Start", value); 
			
        }
		//Tooltip for Object
		public object StartToolTipControllerText(View view)
        {
            #region 0929ImportCode 
            if (Start != null)
            {
                if(End != null)
                    return GetRealTimeSpan(Start).Value.ToString(@"hh\:mm\:ss\,fff");
                else
                    return GetRealTimeSpan(Start).Value.TotalSeconds.ToString("n0");
            }    
#endregion 0929ImportCode
            return null;
        }
		//Get Default Value
        public TimeSpan? GetDefaultStart(View view = null)
        { 
			return Start;
        }
		//Set Default Value
		public void SetDefaultStart(View view = null)
        {
            //if (Start is null){
            //    var result = GetDefaultStart(view);
            //    if (result != null && result != Start){
			//          Start = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StartIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStart();
				//if (result != null && Start != null){
				//	return !Start.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private TimeSpan? _end;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Kết thúc")]
        [ToolTip("Kết thúc")]
		//[Index(1)]		
	    [ModelDefault("MaskSettings", "AgAAAA9NYXNrTWFuYWdlclR5cGUAgwFEZXZFeHByZXNzLkRhdGEuTWFzay5UaW1lU3Bhbk1hc2tNYW5hZ2VyLCBEZXZFeHByZXNzLkRhdGEudjIyLjEsIFZlcnNpb249MjIuMS4zLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjg4ZDE3NTRkNzAwZTQ5YQRtYXNrBwIMW2QuXWhoOm1tOnNz")]
		public TimeSpan? End
        { 
		    get => GetPropertyValue<TimeSpan?>("End");                         
			set => SetPropertyValue<TimeSpan?>("End", value); 
			
        }
		//Tooltip for Object
		public object EndToolTipControllerText(View view)
        {
            #region 1412ImportCode 
            if (End != null)
                return GetRealTimeSpan(End).Value.ToString(@"hh\:mm\:ss\,fff");
#endregion 1412ImportCode
            return null;
        }
		//Get Default Value
        public TimeSpan? GetDefaultEnd(View view = null)
        { 
			return End;
        }
		//Set Default Value
		public void SetDefaultEnd(View view = null)
        {
            //if (End is null){
            //    var result = GetDefaultEnd(view);
            //    if (result != null && result != End){
			//          End = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EndIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEnd();
				//if (result != null && End != null){
				//	return !End.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Voice _voice;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giọng")]
        [ToolTip("Giọng")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(VoiceCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Voice Voice
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Voice>("Voice");                         
			set => SetPropertyValue<Module.BusinessObjects.Voice>("Voice", value); 
			
        }
		//Tooltip for Object
		public object VoiceToolTipControllerText(View view)
        {
        //    if (Voice != null) 
		//			return Voice;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Voice GetDefaultVoice(View view = null)
        { 
			return Voice;
        }
		//Set Default Value
		public void SetDefaultVoice(View view = null)
        {
            //if (Voice is null){
            //    var result = GetDefaultVoice(view);
            //    if (result != null && result != Voice){
			//          Voice = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool VoiceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultVoice();
				//if (result != null && Voice != null){
				//	return !Voice.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator VoiceCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Voice));
            }
        }
	
       
		//private decimal? _voicespeed;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tốc độ")]
        [ToolTip("Tốc độ")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? VoiceSpeed
        { 
		    get => GetPropertyValue<decimal?>("VoiceSpeed");                         
			set => SetPropertyValue<decimal?>("VoiceSpeed", value); 
			
        }
		//Tooltip for Object
		public object VoiceSpeedToolTipControllerText(View view)
        {
            #region 1410ImportCode 
var realEnd = GetRealEnd();
if(realEnd != null)
{
    string result = realEnd.Value.ToString(@"hh\:mm\:ss\,fff");
    if(End != null)
    {
        result += "\r\n<color=gray>" + End.Value.ToString(@"hh\:mm\:ss\,fff") + "</color>";
        result += "\r\n<color=red>" + (realEnd.Value.TotalSeconds - End.Value.TotalSeconds).ToString("n1") + "</color>";
    }
    return result;
}
#endregion 1410ImportCode
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultVoiceSpeed(View view = null)
        { 
			return VoiceSpeed;
        }
		//Set Default Value
		public void SetDefaultVoiceSpeed(View view = null)
        {
            //if (VoiceSpeed is null){
            //    var result = GetDefaultVoiceSpeed(view);
            //    if (result != null && result != VoiceSpeed){
			//          VoiceSpeed = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool VoiceSpeedIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultVoiceSpeed();
				//if (result != null && VoiceSpeed != null){
				//	return !VoiceSpeed.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _content;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(4)]		

 		[Size(2000)]
		public string Content
        { 
		    get => GetPropertyValue<string>("Content");                         
			set => SetPropertyValue<string>("Content", value); 
			
        }
		//Tooltip for Object
		public object ContentToolTipControllerText(View view)
        {
            #region 0577ImportCode 
            if (Video != null)
            {
                // Nếu là paragraph (không có UpperElement)
                if (UpperElement is null)
                {
                    var hover = Content ?? ""; // Nếu Content null thì gán chuỗi rỗng

                    if (!string.IsNullOrEmpty(Note))
                    {
                        var words = Note.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in words)
                        {
                            hover = Module.Helpers.TextHelper.HightlightText(hover, word.Trim());
                        }
                    }

                    if (ParagraphStyle != null)
                    {
                        hover = ParagraphStyle.GetTextWithStyle(hover);
                    }

                    // Tìm dòng kề trên và kề dưới nếu có
                    if (Start != null)
                    {
                        var paragraphStyleColor = ParagraphStyle?.Color ?? System.Drawing.Color.FromName("000000");

                        var audioList = Video.AudioList
                            .Where(m => m.BookMark == BookMark && m.TranslateObject == TranslateObject && m.UpperElement == null);

                        var beforeElement = audioList.OrderByDescending(m => m.Start).FirstOrDefault(m => m.Start < Start);
                        if (beforeElement != null && !string.IsNullOrEmpty(beforeElement.Content))
                        {
                            var beforeContent = beforeElement.ParagraphStyle != null
                                ? beforeElement.ParagraphStyle.GetTextWithStyle(beforeElement.Content, paragraphStyleColor)
                                : $"<color=gray>{beforeElement.Content}</color>";

                            hover = beforeContent + "\r\n" + hover;
                        }

                        var afterElement = audioList.OrderBy(m => m.Start).FirstOrDefault(m => m.Start > Start);
                        if (afterElement != null && !string.IsNullOrEmpty(afterElement.Content))
                        {
                            var afterContent = afterElement.ParagraphStyle != null
                                ? afterElement.ParagraphStyle.GetTextWithStyle(afterElement.Content, paragraphStyleColor)
                                : $"<color=gray>{afterElement.Content}</color>";

                            hover += "\r\n" + afterContent;
                        }
                    }

                    // Trả về nếu toàn bộ nội dung tạo được không rỗng
                    return !string.IsNullOrWhiteSpace(hover) ? hover : null;
                }
                else
                {
                    // Là node con, có UpperElement
                    string hover = "";
                    var listElements = Video.AudioList
                        .Where(m => m.BookMark == BookMark && m.Start != null && m.UpperElement?.Oid == UpperElement.Oid)
                        .OrderBy(m => m.Start);

                    foreach (var element in listElements)
                    {
                        var elementContent = element.Content ?? "";
                        if (element.ParagraphStyle != null)
                            elementContent = element.ParagraphStyle.GetTextWithStyle(elementContent);

                        if (element.Oid == Oid)
                        {
                            if (!string.IsNullOrEmpty(Note))
                            {
                                var words = Note.Split(',', StringSplitOptions.RemoveEmptyEntries);
                                foreach (var word in words)
                                {
                                    elementContent = Module.Helpers.TextHelper.HightlightText(elementContent, word.Trim());
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(hover))
                            hover += "\r\n";

                        hover += elementContent;
                    }

                    return !string.IsNullOrWhiteSpace(hover) ? hover : null;
                }
            }
            return null;
#endregion 0577ImportCode
            return null;
        }
		//Get Default Value
        public string GetDefaultContent(View view = null)
        { 
			return Content;
        }
		//Set Default Value
		public void SetDefaultContent(View view = null)
        {
            //if (Content is null){
            //    var result = GetDefaultContent(view);
            //    if (result != null && result != Content){
			//          Content = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ContentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultContent();
				//if (result != null && Content != null){
				//	return !Content.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _subtitle;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Dịch")]
        [ToolTip("Dịch")]
		//[Index(5)]		

 		[Size(2000)]
		public string Subtitle
        { 
		    get => GetPropertyValue<string>("Subtitle");                         
			set => SetPropertyValue<string>("Subtitle", value); 
			
        }
		//Tooltip for Object
		public object SubtitleToolTipControllerText(View view)
        {
            #region 0578ImportCode 
            if (Video != null)
            {
                // Hover: Nếu là paragraph (Cấp trên trống)
                if (UpperElement is null)
                {
                    var hover = Subtitle ?? ""; // Nếu Subtitle null thì gán chuỗi rỗng

                    if (!string.IsNullOrEmpty(Note))
                    {
                        var words = Note.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in words)
                        {
                            hover = Module.Helpers.TextHelper.HightlightText(hover, word.Trim());
                        }
                    }

                    if (ParagraphStyle != null)
                    {
                        hover = ParagraphStyle.GetTextWithStyle(hover);
                    }

                    // Xử lý hover thêm dòng trước và dòng sau
                    if (Start != null)
                    {
                        var paragraphStyleColor = ParagraphStyle?.Color ?? System.Drawing.Color.FromName("000000");
                        var audioList = Video.AudioList
                            .Where(m => m.BookMark == BookMark && m.TranslateObject == TranslateObject && m.UpperElement == null);

                        var beforeElement = audioList.OrderByDescending(m => m.Start).FirstOrDefault(m => m.Start < Start);
                        if (beforeElement != null && !string.IsNullOrEmpty(beforeElement.Subtitle))
                        {
                            var beforeSubtitle = beforeElement.ParagraphStyle != null
                                ? beforeElement.ParagraphStyle.GetTextWithStyle(beforeElement.Subtitle, paragraphStyleColor)
                                : $"<color=gray>{beforeElement.Subtitle}</color>";

                            hover = beforeSubtitle + "\r\n" + hover;
                        }

                        var afterElement = audioList.OrderBy(m => m.Start).FirstOrDefault(m => m.Start > Start);
                        if (afterElement != null && !string.IsNullOrEmpty(afterElement.Subtitle))
                        {
                            var afterSubtitle = afterElement.ParagraphStyle != null
                                ? afterElement.ParagraphStyle.GetTextWithStyle(afterElement.Subtitle, paragraphStyleColor)
                                : $"<color=gray>{afterElement.Subtitle}</color>";

                            hover += "\r\n" + afterSubtitle;
                        }
                    }

                    return !string.IsNullOrWhiteSpace(hover) ? hover : null;
                }
                else
                {
                    // Hover dòng node con (có UpperElement)
                    string hover = "";
                    var listElements = Video.AudioList
                        .Where(m => m.BookMark == BookMark && m.TranslateObject == TranslateObject && m.Start != null && m.UpperElement?.Oid == UpperElement.Oid)
                        .OrderBy(m => m.Start);

                    foreach (var element in listElements)
                    {
                        var elementSubtitle = element.Subtitle ?? "";
                        if (element.ParagraphStyle != null)
                            elementSubtitle = element.ParagraphStyle.GetTextWithStyle(elementSubtitle);

                        if (element.Oid.Equals(Oid))
                        {
                            if (!string.IsNullOrEmpty(Note))
                            {
                                var words = Note.Split(',', StringSplitOptions.RemoveEmptyEntries);
                                foreach (var word in words)
                                {
                                    elementSubtitle = Module.Helpers.TextHelper.HightlightText(elementSubtitle, word.Trim());
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(hover))
                            hover += "\r\n";

                        hover += elementSubtitle;
                    }

                    return !string.IsNullOrWhiteSpace(hover) ? hover : null;
                }
            }

            return null;
#endregion 0578ImportCode
            return null;
        }
		//Get Default Value
        public string GetDefaultSubtitle(View view = null)
        { 
			return Subtitle;
        }
		//Set Default Value
		public void SetDefaultSubtitle(View view = null)
        {
            //if (Subtitle is null){
            //    var result = GetDefaultSubtitle(view);
            //    if (result != null && result != Subtitle){
			//          Subtitle = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SubtitleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSubtitle();
				//if (result != null && Subtitle != null){
				//	return !Subtitle.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _spelling;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Phiên âm")]
        [ToolTip("Phiên âm")]
		//[Index(6)]		

 		[Size(2000)]
		public string Spelling
        { 
		    get => GetPropertyValue<string>("Spelling");                         
			set => SetPropertyValue<string>("Spelling", value); 
			
        }
		//Tooltip for Object
		public object SpellingToolTipControllerText(View view)
        {
            #region 0579ImportCode 
            if (Video != null)
            {
                // Hover: Nếu là paragraph (Cấp trên trống)
                if (UpperElement is null)
                {
                    var hover = Spelling ?? ""; // Nếu Spelling null thì gán chuỗi rỗng

                    if (!string.IsNullOrEmpty(Note))
                    {
                        var words = Note.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in words)
                        {
                            hover = Module.Helpers.TextHelper.HightlightText(hover, word.Trim());
                        }
                    }

                    if (ParagraphStyle != null)
                    {
                        hover = ParagraphStyle.GetTextWithStyle(hover);
                    }

                    // Xử lý hover thêm dòng trước và dòng sau
                    if (Start != null)
                    {
                        var paragraphStyleColor = ParagraphStyle?.Color ?? System.Drawing.Color.FromName("000000");
                        var audioList = Video.AudioList
                            .Where(m => m.BookMark == BookMark && m.TranslateObject == TranslateObject && m.UpperElement == null);

                        var beforeElement = audioList.OrderByDescending(m => m.Start).FirstOrDefault(m => m.Start < Start);
                        if (beforeElement != null && !string.IsNullOrEmpty(beforeElement.Spelling))
                        {
                            var beforeSpelling = beforeElement.ParagraphStyle != null
                                ? beforeElement.ParagraphStyle.GetTextWithStyle(beforeElement.Spelling, paragraphStyleColor)
                                : $"<color=gray>{beforeElement.Spelling}</color>";

                            hover = beforeSpelling + "\r\n" + hover;
                        }

                        var afterElement = audioList.OrderBy(m => m.Start).FirstOrDefault(m => m.Start > Start);
                        if (afterElement != null && !string.IsNullOrEmpty(afterElement.Spelling))
                        {
                            var afterSpelling = afterElement.ParagraphStyle != null
                                ? afterElement.ParagraphStyle.GetTextWithStyle(afterElement.Spelling, paragraphStyleColor)
                                : $"<color=gray>{afterElement.Spelling}</color>";

                            hover += "\r\n" + afterSpelling;
                        }
                    }

                    return !string.IsNullOrWhiteSpace(hover) ? hover : null;
                }
                else
                {
                    // Hover dòng node con (có UpperElement)
                    string hover = "";
                    var listElements = Video.AudioList
                        .Where(m => m.BookMark == BookMark && m.TranslateObject == TranslateObject && m.Start != null && m.UpperElement?.Oid == UpperElement.Oid)
                        .OrderBy(m => m.Start);

                    foreach (var element in listElements)
                    {
                        var elementSpelling = element.Spelling ?? "";
                        if (element.ParagraphStyle != null)
                            elementSpelling = element.ParagraphStyle.GetTextWithStyle(elementSpelling);

                        if (element.Oid.Equals(Oid))
                        {
                            if (!string.IsNullOrEmpty(Note))
                            {
                                var words = Note.Split(',', StringSplitOptions.RemoveEmptyEntries);
                                foreach (var word in words)
                                {
                                    elementSpelling = Module.Helpers.TextHelper.HightlightText(elementSpelling, word.Trim());
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(hover))
                            hover += "\r\n";

                        hover += elementSpelling;
                    }

                    return !string.IsNullOrWhiteSpace(hover) ? hover : null;
                }
            }

            return null;
#endregion 0579ImportCode
            return null;
        }
		//Get Default Value
        public string GetDefaultSpelling(View view = null)
        { 
			return Spelling;
        }
		//Set Default Value
		public void SetDefaultSpelling(View view = null)
        {
            //if (Spelling is null){
            //    var result = GetDefaultSpelling(view);
            //    if (result != null && result != Spelling){
			//          Spelling = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpellingIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpelling();
				//if (result != null && Spelling != null){
				//	return !Spelling.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dịch ngữ")]
		//[Index(7)]
		[DevExpress.Xpo.Association("Audio-ElementTranslateList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ElementTranslate> ElementTranslateList
        {      
		    get => GetCollection<Module.BusinessObjects.ElementTranslate>("ElementTranslateList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuật vị")]
		//[Index(8)]
		[DevExpress.Xpo.Association("Audio-TermLocationList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.TermLocation> TermLocationList
        {      
		    get => GetCollection<Module.BusinessObjects.TermLocation>("TermLocationList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đoạn video")]
		//[Index(9)]
		[DataSourceCriteria("Not AudioList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("AudioList-MediaList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Media> MediaList
        {      
		    get => GetCollection<Module.BusinessObjects.Media>("MediaList"); 
			
        }
       
		//private Module.BusinessObjects.Video _video;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Video")]
        [ToolTip("Video")]
		//[Index(10)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(VideoCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Video-AudioList")]
	 
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
	
       
		//private Module.BusinessObjects.ParagraphStyle _paragraphstyle;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu cách")]
        [ToolTip("Kiểu cách")]
		//[Index(11)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[Link.Oid] = '@This.BookMark.Oid'")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.ParagraphStyle ParagraphStyle
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ParagraphStyle>("ParagraphStyle");                         
			set => SetPropertyValue<Module.BusinessObjects.ParagraphStyle>("ParagraphStyle", value); 
			
        }
		//Tooltip for Object
		public object ParagraphStyleToolTipControllerText(View view)
        {
            #region 1433ImportCode 
            var audioList = Video.GetAudioListWithSort().Where(x => x.BookMark == BookMark && x.TranslateObject == TranslateObject).ToList();
            for(int i =0; i< audioList.Count; i++)
            {
                if (audioList[i] == this)
                {
                    string result = "";
                    if(i > 0 && audioList[i - 1].ParagraphStyle != null)
                    {
                        var indentRight = audioList[i - 1].ParagraphStyle.IndentRight != null ? string.Format(" ({0:n2})", audioList[i - 1].ParagraphStyle.IndentRight) : "";
                        result += $"<color=gray>{audioList[i - 1].ParagraphStyle.Name}{indentRight}</color>";
                    }
                    if (ParagraphStyle != null)
                    {
                        if(!string.IsNullOrEmpty(result))
                            result += "\r\n";
                        result += ParagraphStyle.Name;
                        if(ParagraphStyle.IndentRight != null)
                        {
                            result += string.Format(" ({0:n2})", audioList[i + 1].ParagraphStyle.IndentRight);
                        }
                    }
                    if (i < audioList.Count - 1 &&  audioList[i + 1].ParagraphStyle != null)
                    {
                        if (!string.IsNullOrEmpty(result))
                            result += "\r\n";
                        var indentRight = audioList[i + 1].ParagraphStyle.IndentRight != null ? string.Format(" ({0:n2})", audioList[i + 1].ParagraphStyle.IndentRight) : "";
                        result += $"<color=gray>{audioList[i + 1].ParagraphStyle.Name}{indentRight}</color>";
                    }
                    return result;
                }
            }
#endregion 1433ImportCode
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ParagraphStyle GetDefaultParagraphStyle(View view = null)
        { 
			return ParagraphStyle;
        }
		//Set Default Value
		public void SetDefaultParagraphStyle(View view = null)
        {
            //if (ParagraphStyle is null){
            //    var result = GetDefaultParagraphStyle(view);
            //    if (result != null && result != ParagraphStyle){
			//          ParagraphStyle = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ParagraphStyleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultParagraphStyle();
				//if (result != null && ParagraphStyle != null){
				//	return !ParagraphStyle.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ParagraphStyleCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ParagraphStyle));
            }
        }
	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(12)]		
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

	
       
		//private string _url;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("URL")]
        [ToolTip("URL")]
		//[Index(13)]		

 		[Size(200)]
		public string URL
        { 
		    get => GetPropertyValue<string>("URL");                         
			set => SetPropertyValue<string>("URL", value); 
			
        }
		//Tooltip for Object
		public object URLToolTipControllerText(View view)
        {
        //    if (URL != null) 
		//			return URL;
            return null;
        }
		//Get Default Value
        public string GetDefaultURL(View view = null)
        { 
			return URL;
        }
		//Set Default Value
		public void SetDefaultURL(View view = null)
        {
            //if (URL is null){
            //    var result = GetDefaultURL(view);
            //    if (result != null && result != URL){
			//          URL = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool URLIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultURL();
				//if (result != null && URL != null){
				//	return !URL.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DevExpress.Persistent.BaseImpl.FileData _filedata;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Âm thanh")]
        [ToolTip("Âm thanh")]
		//[Index(14)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(FileDataCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [NonCloneable()]
		public DevExpress.Persistent.BaseImpl.FileData FileData
        { 
		    get => GetPropertyValue<DevExpress.Persistent.BaseImpl.FileData>("FileData");                         
			set => SetPropertyValue<DevExpress.Persistent.BaseImpl.FileData>("FileData", value); 
			
        }
		//Tooltip for Object
		public object FileDataToolTipControllerText(View view)
        {
        //    if (FileData != null) 
		//			return FileData;
            return null;
        }
		//Get Default Value
        public DevExpress.Persistent.BaseImpl.FileData GetDefaultFileData(View view = null)
        { 
			return FileData;
        }
		//Set Default Value
		public void SetDefaultFileData(View view = null)
        {
            //if (FileData is null){
            //    var result = GetDefaultFileData(view);
            //    if (result != null && result != FileData){
			//          FileData = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FileDataIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFileData();
				//if (result != null && FileData != null){
				//	return !FileData.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator FileDataCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(FileData));
            }
        }
	
       
		//private string _audiolink;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tệp âm")]
		[ToolTip("Đường dẫn tới tệp âm thanh")]
		//[Index(15)]		

 		[Size(250)]
	    [ModelDefault("AllowEdit", "False")]
		public string AudioLink
        { 
		    get => GetPropertyValue<string>("AudioLink");                         
			set => SetPropertyValue<string>("AudioLink", value); 
			
        }
		//Tooltip for Object
		public object AudioLinkToolTipControllerText(View view)
        {
        //    if (AudioLink != null) 
		//			return AudioLink;
            return null;
        }
		//Get Default Value
        public string GetDefaultAudioLink(View view = null)
        { 
			return AudioLink;
        }
		//Set Default Value
		public void SetDefaultAudioLink(View view = null)
        {
            //if (AudioLink is null){
            //    var result = GetDefaultAudioLink(view);
            //    if (result != null && result != AudioLink){
			//          AudioLink = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AudioLinkIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAudioLink();
				//if (result != null && AudioLink != null){
				//	return !AudioLink.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _audioduration;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thời lượng âm")]
		[ToolTip("Thời lượng của file Audio được nạp vào từ AI hoặc MP3")]
		//[Index(16)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? AudioDuration
        { 
		    get => GetPropertyValue<decimal?>("AudioDuration");                         
			set => SetPropertyValue<decimal?>("AudioDuration", value); 
			
        }
		//Tooltip for Object
		public object AudioDurationToolTipControllerText(View view)
        {
        //    if (AudioDuration != null) 
		//			return AudioDuration;
            return null;
        }
		//Get Default Value
		//Set Default Value
		public void SetDefaultAudioDuration(View view = null)
        {
            //if (AudioDuration is null){
            //    var result = GetDefaultAudioDuration(view);
            //    if (result != null && result != AudioDuration){
			//          AudioDuration = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AudioDurationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAudioDuration();
				//if (result != null && AudioDuration != null){
				//	return !AudioDuration.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _audiorate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Suất âm")]
		[ToolTip("Thời lượng  âm / Tốc độ /  Thời lượng * 100%")]
		//[Index(17)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p0")]
		public decimal? AudioRate
        { 
		    #region 0542ImportCode 
get
            {
                //Thời lượng  âm / Tốc độ /  Thời lượng * 100%
                if(Duration != (decimal)0 && AudioDuration != (decimal)0 && VoiceSpeed != null && VoiceSpeed != (decimal)0)
                {
                    return AudioDuration / VoiceSpeed.Value / Duration;
                }
                return null;
            }
#endregion 0542ImportCode
			
        }
		//Tooltip for Object
		public object AudioRateToolTipControllerText(View view)
        {
            #region 1411ImportCode 
if(Video != null && Video.LanguageTranslate != null && Video.LanguageOrigin != null &&
    !string.IsNullOrEmpty(Video.LanguageTranslate.Code) && !string.IsNullOrEmpty(Video.LanguageOrigin.Code) &&
    !string.IsNullOrEmpty(Subtitle) && !string.IsNullOrEmpty(Content))
{
    
    var contentSyllables = Module.Helpers.TextHelper.CountSyllables(Content);
    if(contentSyllables > 0)
    {
        string result = "Âm tiết nội dung: " + contentSyllables.ToString("n0");
        var subtitleSyllables = Module.Helpers.TextHelper.CountSyllables(Subtitle);
        if(subtitleSyllables > 0)
        {
            result += "\r\nÂm tiết dịch: " + subtitleSyllables.ToString("n0");
            if(contentSyllables == subtitleSyllables)
            {
                //return "Âm tiết bằng nhau: " + contentSyllables.ToString("n0");
            }
            else if(contentSyllables < subtitleSyllables)
            {
                result += "\r\nÂm tiết dịch nhiều hơn: " + (subtitleSyllables - contentSyllables).ToString("n0") + " âm tiết";
            
            }
            else
            {
                result += "\r\nÂm tiết dịch ít hơn: " + (contentSyllables - subtitleSyllables).ToString("n0") + " âm tiết";
            }
            
        }
        return result;
    }
    
}
#endregion 1411ImportCode
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultAudioRate(View view = null)
        { 
			return AudioRate;
        }
		//Set Default Value
		public void SetDefaultAudioRate(View view = null)
        {
            //if (AudioRate is null){
            //    var result = GetDefaultAudioRate(view);
            //    if (result != null && result != AudioRate){
			//          AudioRate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AudioRateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAudioRate();
				//if (result != null && AudioRate != null){
				//	return !AudioRate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _duration;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thời lượng")]
		[ToolTip("= End - Start (không lưu)")]
		//[Index(18)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Duration
        { 
		    #region 0541ImportCode 
get
            {
                if (Start != null && End != null)
                    return Convert.ToDecimal((End.Value - Start.Value).TotalSeconds);
                return 0;
            }
#endregion 0541ImportCode
			
        }
		//Tooltip for Object
		public object DurationToolTipControllerText(View view)
        {
        //    if (Duration != null) 
		//			return Duration;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultDuration(View view = null)
        { 
			return Duration;
        }
		//Set Default Value
		public void SetDefaultDuration(View view = null)
        {
            //if (Duration is null){
            //    var result = GetDefaultDuration(view);
            //    if (result != null && result != Duration){
			//          Duration = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DurationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDuration();
				//if (result != null && Duration != null){
				//	return !Duration.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _silencegap;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lặng")]
		[ToolTip("= Start(sau) - End (không lưu)")]
		//[Index(19)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? SilenceGap
        { 
		    #region 0538ImportCode 
get
            {
                if (Video != null && Start != null && End != null)
                {
                    Audio afterSubtitle = null;
                    var audiosWithSort = Video.AudioList.OrderBy(m => m.Start).ToList();
                    for (int i = 0; i < audiosWithSort.Count(); i++)
                    {
                        if (audiosWithSort[i].Start  != null && audiosWithSort[i].Start > Start)
                        {
                            afterSubtitle = audiosWithSort[i];
                            break;
                        }
                    }
                    if (afterSubtitle != null && End != null)
                    {
                        var result = Convert.ToDecimal((afterSubtitle.Start.Value - End.Value).TotalSeconds);
                        if (result != (decimal)0)
                            return result;
                    }
                }
                return null;
            }
#endregion 0538ImportCode
			
        }
		//Tooltip for Object
		public object SilenceGapToolTipControllerText(View view)
        {
        //    if (SilenceGap != null) 
		//			return SilenceGap;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultSilenceGap(View view = null)
        { 
			return SilenceGap;
        }
		//Set Default Value
		public void SetDefaultSilenceGap(View view = null)
        {
            //if (SilenceGap is null){
            //    var result = GetDefaultSilenceGap(view);
            //    if (result != null && result != SilenceGap){
			//          SilenceGap = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SilenceGapIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSilenceGap();
				//if (result != null && SilenceGap != null){
				//	return !SilenceGap.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _quantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(20)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Quantity
        { 
		    get => GetPropertyValue<decimal?>("Quantity");                         
			set => SetPropertyValue<decimal?>("Quantity", value); 
			
        }
		//Tooltip for Object
		public object QuantityToolTipControllerText(View view)
        {
            #region 1035ImportCode 
if (!string.IsNullOrEmpty(Note))
            {
                return Module.Helpers.TextHelper.GetTextWithTagNode(Note, '(', true);
            }
#endregion 1035ImportCode
            return null;
        }
		//Get Default Value
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

	
       
		//private decimal? _textrate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tỉ suất")]
        [ToolTip("Tỉ suất")]
		//[Index(21)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p0")]
		public decimal? TextRate
        { 
		    get => GetPropertyValue<decimal?>("TextRate");                         
			set => SetPropertyValue<decimal?>("TextRate", value); 
			
        }
		//Tooltip for Object
		public object TextRateToolTipControllerText(View view)
        {
            #region 1034ImportCode 
if (!string.IsNullOrEmpty(Note))
{
    return Module.Helpers.TextHelper.GetTextWithTagNode(Note, '|', true);
}
#endregion 1034ImportCode
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultTextRate(View view = null)
        { 
			return TextRate;
        }
		//Set Default Value
		public void SetDefaultTextRate(View view = null)
        {
            //if (TextRate is null){
            //    var result = GetDefaultTextRate(view);
            //    if (result != null && result != TextRate){
			//          TextRate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TextRateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTextRate();
				//if (result != null && TextRate != null){
				//	return !TextRate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _subtitletime;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật dịch")]
        [ToolTip("Cập nhật dịch")]
		//[Index(22)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? SubtitleTime
        { 
		    get => GetPropertyValue<DateTime?>("SubtitleTime");                         
			set => SetPropertyValue<DateTime?>("SubtitleTime", value); 
			
        }
		//Tooltip for Object
		public object SubtitleTimeToolTipControllerText(View view)
        {
        //    if (SubtitleTime != null) 
		//			return SubtitleTime;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool SubtitleTimeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSubtitleTime();
				//if (result != null && SubtitleTime != null){
				//	return !SubtitleTime.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _spellingtime;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật phiên âm")]
        [ToolTip("Cập nhật phiên âm")]
		//[Index(23)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? SpellingTime
        { 
		    get => GetPropertyValue<DateTime?>("SpellingTime");                         
			set => SetPropertyValue<DateTime?>("SpellingTime", value); 
			
        }
		//Tooltip for Object
		public object SpellingTimeToolTipControllerText(View view)
        {
        //    if (SpellingTime != null) 
		//			return SpellingTime;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool SpellingTimeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpellingTime();
				//if (result != null && SpellingTime != null){
				//	return !SpellingTime.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _audiotime;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật âm thanh")]
        [ToolTip("Cập nhật âm thanh")]
		//[Index(24)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? AudioTime
        { 
		    get => GetPropertyValue<DateTime?>("AudioTime");                         
			set => SetPropertyValue<DateTime?>("AudioTime", value); 
			
        }
		//Tooltip for Object
		public object AudioTimeToolTipControllerText(View view)
        {
        //    if (AudioTime != null) 
		//			return AudioTime;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool AudioTimeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAudioTime();
				//if (result != null && AudioTime != null){
				//	return !AudioTime.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _splitted;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tách")]
        [ToolTip("Tách")]
		//[Index(25)]		
		public bool Splitted
        { 
		    get => GetPropertyValue<bool>("Splitted");                         
			set => SetPropertyValue<bool>("Splitted", value); 
			
        }
		//Tooltip for Object
		public object SplittedToolTipControllerText(View view)
        {
        //    if (Splitted != null) 
		//			return Splitted;
            return null;
        }
		//Get Default Value
        public bool GetDefaultSplitted(View view = null)
        { 
			return Splitted;
        }
		//Set Default Value
		public void SetDefaultSplitted(View view = null)
        {
            //if (Splitted is null){
            //    var result = GetDefaultSplitted(view);
            //    if (result != null && result != Splitted){
			//          Splitted = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SplittedIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSplitted();
				//if (result != null && Splitted != null){
				//	return !Splitted.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _note;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(26)]		

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

	
       
		//private bool _delete;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Xóa")]
        [ToolTip("Xóa")]
		//[Index(27)]		
		public bool Delete
        { 
		    get => GetPropertyValue<bool>("Delete");                         
			set => SetPropertyValue<bool>("Delete", value); 
			
        }
		//Tooltip for Object
		public object DeleteToolTipControllerText(View view)
        {
        //    if (Delete != null) 
		//			return Delete;
            return null;
        }
		//Get Default Value
        public bool GetDefaultDelete(View view = null)
        { 
			return Delete;
        }
		//Set Default Value
		public void SetDefaultDelete(View view = null)
        {
            //if (Delete is null){
            //    var result = GetDefaultDelete(view);
            //    if (result != null && result != Delete){
			//          Delete = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DeleteIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDelete();
				//if (result != null && Delete != null){
				//	return !Delete.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Audio _nextelement;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sau")]
        [ToolTip("Sau")]
		//[Index(28)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(NextElementCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [NotMapped()]
	    [NonPersistent()]
		public Module.BusinessObjects.Audio NextElement
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Audio>("NextElement");                         
			set => SetPropertyValue<Module.BusinessObjects.Audio>("NextElement", value); 
			
        }
		//Tooltip for Object
		public object NextElementToolTipControllerText(View view)
        {
        //    if (NextElement != null) 
		//			return NextElement;
            return null;
        }
		//Get Default Value
		//Set Default Value
		public void SetDefaultNextElement(View view = null)
        {
            //if (NextElement is null){
            //    var result = GetDefaultNextElement(view);
            //    if (result != null && result != NextElement){
			//          NextElement = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NextElementIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNextElement();
				//if (result != null && NextElement != null){
				//	return !NextElement.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator NextElementCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(NextElement));
            }
        }
	
       
		//private Module.BusinessObjects.Audio _previouselement;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trước")]
        [ToolTip("Trước")]
		//[Index(29)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PreviousElementCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [NotMapped()]
	    [NonPersistent()]
		public Module.BusinessObjects.Audio PreviousElement
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Audio>("PreviousElement");                         
			set => SetPropertyValue<Module.BusinessObjects.Audio>("PreviousElement", value); 
			
        }
		//Tooltip for Object
		public object PreviousElementToolTipControllerText(View view)
        {
        //    if (PreviousElement != null) 
		//			return PreviousElement;
            return null;
        }
		//Get Default Value
		//Set Default Value
		public void SetDefaultPreviousElement(View view = null)
        {
            //if (PreviousElement is null){
            //    var result = GetDefaultPreviousElement(view);
            //    if (result != null && result != PreviousElement){
			//          PreviousElement = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PreviousElementIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPreviousElement();
				//if (result != null && PreviousElement != null){
				//	return !PreviousElement.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PreviousElementCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PreviousElement));
            }
        }
	
       
		//private Status _status;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trạng thái")]
        [ToolTip("Trạng thái")]
		//[Index(30)]		
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
	
       
		//private Module.BusinessObjects.Audio _upperelement;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(31)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[Video.Oid] = '?Video.Oid' And ([BookMark.Oid] = '?BookMark.Oid' Or [BookMark] Is Null)")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Audio UpperElement
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Audio>("UpperElement");                         
			set => SetPropertyValue<Module.BusinessObjects.Audio>("UpperElement", value); 
			
        }
		//Tooltip for Object
		public object UpperElementToolTipControllerText(View view)
        {
        //    if (UpperElement != null) 
		//			return UpperElement;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Audio GetDefaultUpperElement(View view = null)
        { 
			return UpperElement;
        }
		//Set Default Value
		public void SetDefaultUpperElement(View view = null)
        {
            //if (UpperElement is null){
            //    var result = GetDefaultUpperElement(view);
            //    if (result != null && result != UpperElement){
			//          UpperElement = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperElementIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpperElement();
				//if (result != null && UpperElement != null){
				//	return !UpperElement.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpperElementCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(UpperElement));
            }
        }
	
       
		//private bool _textnode;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nốt")]
        [ToolTip("Nốt")]
		//[Index(32)]		
		public bool TextNode
        { 
		    get => GetPropertyValue<bool>("TextNode");                         
			set => SetPropertyValue<bool>("TextNode", value); 
			
        }
		//Tooltip for Object
		public object TextNodeToolTipControllerText(View view)
        {
        //    if (TextNode != null) 
		//			return TextNode;
            return null;
        }
		//Get Default Value
        public bool GetDefaultTextNode(View view = null)
        { 
			return TextNode;
        }
		//Set Default Value
		public void SetDefaultTextNode(View view = null)
        {
            //if (TextNode is null){
            //    var result = GetDefaultTextNode(view);
            //    if (result != null && result != TextNode){
			//          TextNode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TextNodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTextNode();
				//if (result != null && TextNode != null){
				//	return !TextNode.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _notadjacent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Không kề sau")]
        [ToolTip("Không kề sau")]
		//[Index(33)]		
		public bool NotAdjacent
        { 
		    get => GetPropertyValue<bool>("NotAdjacent");                         
			set => SetPropertyValue<bool>("NotAdjacent", value); 
			
        }
		//Tooltip for Object
		public object NotAdjacentToolTipControllerText(View view)
        {
        //    if (NotAdjacent != null) 
		//			return NotAdjacent;
            return null;
        }
		//Get Default Value
        public bool GetDefaultNotAdjacent(View view = null)
        { 
			return NotAdjacent;
        }
		//Set Default Value
		public void SetDefaultNotAdjacent(View view = null)
        {
            //if (NotAdjacent is null){
            //    var result = GetDefaultNotAdjacent(view);
            //    if (result != null && result != NotAdjacent){
			//          NotAdjacent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NotAdjacentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultNotAdjacent();
				//if (result != null && NotAdjacent != null){
				//	return !NotAdjacent.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _linepagecontent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dòng trang ND")]
        [ToolTip("Dòng trang ND")]
		//[Index(34)]		

 		[Size(100)]
		public string LinePageContent
        { 
		    get => GetPropertyValue<string>("LinePageContent");                         
			set => SetPropertyValue<string>("LinePageContent", value); 
			
        }
		//Tooltip for Object
		public object LinePageContentToolTipControllerText(View view)
        {
            #region 1039ImportCode 
if (!string.IsNullOrEmpty(LinePageContent))
            {
                var linePageSeparator = Module.Helpers.ParameterHelper.GetValueOrDefault(Session, "LinePageSeparator", " , ");
                var linePageArray = LinePageContent.Split(linePageSeparator, StringSplitOptions.RemoveEmptyEntries);
                if (linePageArray.Length < 3)
                    return null;
                return string.Format("Dòng đầu: {0:D}\r\nTổng dòng: {1:D}\r\nTrang: {2:D}", linePageArray[0], linePageArray[1], linePageArray[2]);
            }
#endregion 1039ImportCode
            return null;
        }
		//Get Default Value
        public string GetDefaultLinePageContent(View view = null)
        { 
			return LinePageContent;
        }
		//Set Default Value
		public void SetDefaultLinePageContent(View view = null)
        {
            //if (LinePageContent is null){
            //    var result = GetDefaultLinePageContent(view);
            //    if (result != null && result != LinePageContent){
			//          LinePageContent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LinePageContentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLinePageContent();
				//if (result != null && LinePageContent != null){
				//	return !LinePageContent.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _linepagetranslate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dòng trang Dịch")]
        [ToolTip("Dòng trang Dịch")]
		//[Index(35)]		

 		[Size(100)]
		public string LinePageTranslate
        { 
		    get => GetPropertyValue<string>("LinePageTranslate");                         
			set => SetPropertyValue<string>("LinePageTranslate", value); 
			
        }
		//Tooltip for Object
		public object LinePageTranslateToolTipControllerText(View view)
        {
            #region 1040ImportCode 
if (!string.IsNullOrEmpty(LinePageTranslate))
            {
                var linePageSeparator = Module.Helpers.ParameterHelper.GetValueOrDefault(Session, "LinePageSeparator", " , ");
                var linePageArray = LinePageTranslate.Split(linePageSeparator, StringSplitOptions.RemoveEmptyEntries);
                if (linePageArray.Length < 3)
                    return null;
                return string.Format("Dòng đầu: {0:D}\r\nTổng dòng: {1:D}\r\nTrang: {2:D}", linePageArray[0], linePageArray[1], linePageArray[2]);
            }
#endregion 1040ImportCode
            return null;
        }
		//Get Default Value
        public string GetDefaultLinePageTranslate(View view = null)
        { 
			return LinePageTranslate;
        }
		//Set Default Value
		public void SetDefaultLinePageTranslate(View view = null)
        {
            //if (LinePageTranslate is null){
            //    var result = GetDefaultLinePageTranslate(view);
            //    if (result != null && result != LinePageTranslate){
			//          LinePageTranslate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LinePageTranslateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLinePageTranslate();
				//if (result != null && LinePageTranslate != null){
				//	return !LinePageTranslate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _parenttag;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thẻ cha")]
        [ToolTip("Thẻ cha")]
		//[Index(36)]		

 		[Size(100)]
		public string ParentTag
        { 
		    get => GetPropertyValue<string>("ParentTag");                         
			set => SetPropertyValue<string>("ParentTag", value); 
			
        }
		//Tooltip for Object
		public object ParentTagToolTipControllerText(View view)
        {
        //    if (ParentTag != null) 
		//			return ParentTag;
            return null;
        }
		//Get Default Value
        public string GetDefaultParentTag(View view = null)
        { 
			return ParentTag;
        }
		//Set Default Value
		public void SetDefaultParentTag(View view = null)
        {
            //if (ParentTag is null){
            //    var result = GetDefaultParentTag(view);
            //    if (result != null && result != ParentTag){
			//          ParentTag = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ParentTagIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultParentTag();
				//if (result != null && ParentTag != null){
				//	return !ParentTag.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private CaseType _casetype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu chữ")]
        [ToolTip("Kiểu chữ")]
		//[Index(37)]		
		public CaseType CaseType
        { 
		    get => GetPropertyValue<CaseType>("CaseType");                         
			set => SetPropertyValue<CaseType>("CaseType", value); 
			
        }
		//Tooltip for Object
		public object CaseTypeToolTipControllerText(View view)
        {
        //    if (CaseType != null) 
		//			return CaseType;
            return null;
        }
		//Get Default Value
		//Set Default Value
		public void SetDefaultCaseType(View view = null)
        {
            //if (CaseType is null){
            //    var result = GetDefaultCaseType(view);
            //    if (result != null && result != CaseType){
			//          CaseType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CaseTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCaseType();
				//if (result != null && CaseType != null){
				//	return !CaseType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.BookMark _bookmark;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
        [ToolTip("Liên kết")]
		//[Index(38)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(BookMarkCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.BookMark BookMark
        { 
		    get => GetPropertyValue<Module.BusinessObjects.BookMark>("BookMark");                         
			set => SetPropertyValue<Module.BusinessObjects.BookMark>("BookMark", value); 
			
        }
		//Tooltip for Object
		public object BookMarkToolTipControllerText(View view)
        {
        //    if (BookMark != null) 
		//			return BookMark;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.BookMark GetDefaultBookMark(View view = null)
        { 
			return BookMark;
        }
		//Set Default Value
		public void SetDefaultBookMark(View view = null)
        {
            //if (BookMark is null){
            //    var result = GetDefaultBookMark(view);
            //    if (result != null && result != BookMark){
			//          BookMark = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BookMarkIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBookMark();
				//if (result != null && BookMark != null){
				//	return !BookMark.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator BookMarkCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(BookMark));
            }
        }
	
       
		//private Module.BusinessObjects.TranslateObject _translateobject;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đối tượng dịch")]
        [ToolTip("Đối tượng dịch")]
		//[Index(39)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TranslateObjectCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.TranslateObject TranslateObject
        { 
		    get => GetPropertyValue<Module.BusinessObjects.TranslateObject>("TranslateObject");                         
			set => SetPropertyValue<Module.BusinessObjects.TranslateObject>("TranslateObject", value); 
			
        }
		//Tooltip for Object
		public object TranslateObjectToolTipControllerText(View view)
        {
        //    if (TranslateObject != null) 
		//			return TranslateObject;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.TranslateObject GetDefaultTranslateObject(View view = null)
        { 
			return TranslateObject;
        }
		//Set Default Value
		public void SetDefaultTranslateObject(View view = null)
        {
            //if (TranslateObject is null){
            //    var result = GetDefaultTranslateObject(view);
            //    if (result != null && result != TranslateObject){
			//          TranslateObject = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TranslateObjectIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTranslateObject();
				//if (result != null && TranslateObject != null){
				//	return !TranslateObject.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TranslateObjectCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(TranslateObject));
            }
        }
	
       
		//private bool _flag;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ")]
        [ToolTip("Cờ")]
		//[Index(40)]		
	    [NonPersistent()]
	    [NotMapped()]
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

	
       
		//private bool _flag2;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ 2")]
        [ToolTip("Cờ 2")]
		//[Index(41)]		
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
		//[Index(42)]		

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

	
       
		//private string _speaker;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Người nói")]
        [ToolTip("Người nói")]
		//[Index(43)]		

 		[Size(50)]
		public string Speaker
        { 
		    get => GetPropertyValue<string>("Speaker");                         
			set => SetPropertyValue<string>("Speaker", value); 
			
        }
		//Tooltip for Object
		public object SpeakerToolTipControllerText(View view)
        {
        //    if (Speaker != null) 
		//			return Speaker;
            return null;
        }
		//Get Default Value
        public string GetDefaultSpeaker(View view = null)
        { 
			return Speaker;
        }
		//Set Default Value
		public void SetDefaultSpeaker(View view = null)
        {
            //if (Speaker is null){
            //    var result = GetDefaultSpeaker(view);
            //    if (result != null && result != Speaker){
			//          Speaker = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpeakerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpeaker();
				//if (result != null && Speaker != null){
				//	return !Speaker.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.ElementBatch _elementbatch;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lô")]
        [ToolTip("Lô")]
		//[Index(44)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ElementBatchCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("ElementBatch-AudioList")]
	 
	    [ModelDefault("MaskSettings", "AgAAAA9NYXNrTWFuYWdlclR5cGUAgwFEZXZFeHByZXNzLkRhdGEuTWFzay5UaW1lU3Bhbk1hc2tNYW5hZ2VyLCBEZXZFeHByZXNzLkRhdGEudjIyLjEsIFZlcnNpb249MjIuMS4zLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjg4ZDE3NTRkNzAwZTQ5YQRtYXNrBwIMW2QuXWhoOm1tOnNz")]
		public Module.BusinessObjects.ElementBatch ElementBatch
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ElementBatch>("ElementBatch");                         
			set => SetPropertyValue<Module.BusinessObjects.ElementBatch>("ElementBatch", value); 
			
        }
		//Tooltip for Object
		public object ElementBatchToolTipControllerText(View view)
        {
        //    if (ElementBatch != null) 
		//			return ElementBatch;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ElementBatch GetDefaultElementBatch(View view = null)
        { 
			return ElementBatch;
        }
		//Set Default Value
		public void SetDefaultElementBatch(View view = null)
        {
            //if (ElementBatch is null){
            //    var result = GetDefaultElementBatch(view);
            //    if (result != null && result != ElementBatch){
			//          ElementBatch = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ElementBatchIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultElementBatch();
				//if (result != null && ElementBatch != null){
				//	return !ElementBatch.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ElementBatchCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ElementBatch));
            }
        }
	
       
		//private string _languagetranslate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dịch ngữ")]
        [ToolTip("Dịch ngữ")]
		//[Index(45)]		

 		[Size(2000)]
	    [NonPersistent()]
	    [NotMapped()]
		public string LanguageTranslate
        { 
		    #region 3306ImportCode 
get => string.Join(" ",
         ElementTranslateList?
        .Where(e => e.Language == Video?.LanguageTranslate)
        .Select(e => e.Content)
        .Where(c => !string.IsNullOrWhiteSpace(c))
    ?? Enumerable.Empty<string>());
#endregion 3306ImportCode
			
        }
		//Tooltip for Object
		public object LanguageTranslateToolTipControllerText(View view)
        {
            #region 3301ImportCode 
            if (Video != null)
            {
                // Hover: Nếu là paragraph (Cấp trên trống)
                if (UpperElement is null)
                {
                    var hover = Subtitle ?? ""; // Nếu Subtitle null thì gán chuỗi rỗng

                    if (!string.IsNullOrEmpty(Note))
                    {
                        var words = Note.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in words)
                        {
                            hover = Module.Helpers.TextHelper.HightlightText(hover, word.Trim());
                        }
                    }

                    if (ParagraphStyle != null)
                    {
                        hover = ParagraphStyle.GetTextWithStyle(hover);
                    }

                    // Xử lý hover thêm dòng trước và dòng sau
                    if (Start != null)
                    {
                        var paragraphStyleColor = ParagraphStyle?.Color ?? System.Drawing.Color.FromName("000000");
                        var audioList = Video.AudioList
                            .Where(m => m.BookMark == BookMark && m.TranslateObject == TranslateObject && m.UpperElement == null);

                        var beforeElement = audioList.OrderByDescending(m => m.Start).FirstOrDefault(m => m.Start < Start);
                        if (beforeElement != null && !string.IsNullOrEmpty(beforeElement.Subtitle))
                        {
                            var beforeSubtitle = beforeElement.ParagraphStyle != null
                                ? beforeElement.ParagraphStyle.GetTextWithStyle(beforeElement.Subtitle, paragraphStyleColor)
                                : $"<color=gray>{beforeElement.Subtitle}</color>";

                            hover = beforeSubtitle + "\r\n" + hover;
                        }

                        var afterElement = audioList.OrderBy(m => m.Start).FirstOrDefault(m => m.Start > Start);
                        if (afterElement != null && !string.IsNullOrEmpty(afterElement.Subtitle))
                        {
                            var afterSubtitle = afterElement.ParagraphStyle != null
                                ? afterElement.ParagraphStyle.GetTextWithStyle(afterElement.Subtitle, paragraphStyleColor)
                                : $"<color=gray>{afterElement.Subtitle}</color>";

                            hover += "\r\n" + afterSubtitle;
                        }
                    }

                    return !string.IsNullOrWhiteSpace(hover) ? hover : null;
                }
                else
                {
                    // Hover dòng node con (có UpperElement)
                    string hover = "";
                    var listElements = Video.AudioList
                        .Where(m => m.BookMark == BookMark && m.TranslateObject == TranslateObject && m.Start != null && m.UpperElement?.Oid == UpperElement.Oid)
                        .OrderBy(m => m.Start);

                    foreach (var element in listElements)
                    {
                        var elementSubtitle = element.Subtitle ?? "";
                        if (element.ParagraphStyle != null)
                            elementSubtitle = element.ParagraphStyle.GetTextWithStyle(elementSubtitle);

                        if (element.Oid.Equals(Oid))
                        {
                            if (!string.IsNullOrEmpty(Note))
                            {
                                var words = Note.Split(',', StringSplitOptions.RemoveEmptyEntries);
                                foreach (var word in words)
                                {
                                    elementSubtitle = Module.Helpers.TextHelper.HightlightText(elementSubtitle, word.Trim());
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(hover))
                            hover += "\r\n";

                        hover += elementSubtitle;
                    }

                    return !string.IsNullOrWhiteSpace(hover) ? hover : null;
                }
            }

            return null;
#endregion 3301ImportCode
            return null;
        }
		//Get Default Value
        public string GetDefaultLanguageTranslate(View view = null)
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

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(46)]		
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

	
       
		//private Module.BusinessObjects.Paragraph _paragraph;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đoạn văn bản")]
        [ToolTip("Đoạn văn bản")]
		//[Index(47)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ParagraphCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Paragraph-AudioList")]
	 
		public Module.BusinessObjects.Paragraph Paragraph
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Paragraph>("Paragraph");                         
			set => SetPropertyValue<Module.BusinessObjects.Paragraph>("Paragraph", value); 
			
        }
		//Tooltip for Object
		public object ParagraphToolTipControllerText(View view)
        {
        //    if (Paragraph != null) 
		//			return Paragraph;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Paragraph GetDefaultParagraph(View view = null)
        { 
			return Paragraph;
        }
		//Set Default Value
		public void SetDefaultParagraph(View view = null)
        {
            //if (Paragraph is null){
            //    var result = GetDefaultParagraph(view);
            //    if (result != null && result != Paragraph){
			//          Paragraph = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ParagraphIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultParagraph();
				//if (result != null && Paragraph != null){
				//	return !Paragraph.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ParagraphCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Paragraph));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0378ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
VoiceSpeed = 1;
            #endregion 0378ImportCode
 
        //SetDefaultStart(View view = null);
        //SetDefaultEnd(View view = null);
        //SetDefaultVoice(View view = null);
        //SetDefaultVoiceSpeed(View view = null);
        //SetDefaultContent(View view = null);
        //SetDefaultSubtitle(View view = null);
        //SetDefaultSpelling(View view = null);
        //SetDefaultVideo(View view = null);
        //SetDefaultParagraphStyle(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultURL(View view = null);
        //SetDefaultFileData(View view = null);
        //SetDefaultAudioLink(View view = null);
        //SetDefaultAudioDuration(View view = null);
        //SetDefaultAudioRate(View view = null);
        //SetDefaultDuration(View view = null);
        //SetDefaultSilenceGap(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultTextRate(View view = null);
        //SetDefaultSubtitleTime(View view = null);
        //SetDefaultSpellingTime(View view = null);
        //SetDefaultAudioTime(View view = null);
        //SetDefaultSplitted(View view = null);
        //SetDefaultNote(View view = null);
        //SetDefaultDelete(View view = null);
        //SetDefaultNextElement(View view = null);
        //SetDefaultPreviousElement(View view = null);
        //SetDefaultStatus(View view = null);
        //SetDefaultUpperElement(View view = null);
        //SetDefaultTextNode(View view = null);
        //SetDefaultNotAdjacent(View view = null);
        //SetDefaultLinePageContent(View view = null);
        //SetDefaultLinePageTranslate(View view = null);
        //SetDefaultParentTag(View view = null);
        //SetDefaultCaseType(View view = null);
        //SetDefaultBookMark(View view = null);
        //SetDefaultTranslateObject(View view = null);
        //SetDefaultFlag(View view = null);
        //SetDefaultFlag2(View view = null);
        //SetDefaultNote2(View view = null);
        //SetDefaultSpeaker(View view = null);
        //SetDefaultElementBatch(View view = null);
        //SetDefaultLanguageTranslate(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultParagraph(View view = null);
			
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
            #region 0475ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0475ImportCode
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
				
                    case nameof(Spelling):
                        OnChangedSpelling(oldValue, newValue);
                        break;
				
                    case nameof(Content):
                        OnChangedContent(oldValue, newValue);
                        break;
				
                    case nameof(Video):
                        OnChangedVideo(oldValue, newValue);
                        break;
				
                    case nameof(Subtitle):
                        OnChangedSubtitle(oldValue, newValue);
                        break;
				
                    case nameof(ParagraphStyle):
                        OnChangedParagraphStyle(oldValue, newValue);
                        break;
				
                    case nameof(AudioDuration):
                        OnChangedAudioDuration(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedSpelling(object oldValue, object newValue)
        {
            #region 0536ImportCode
            if (newValue is null) return;
                    //Xử lý ký tự đặc biệt mã ASCII 160 giống dấu cách
                    var newtext = Spelling.Replace(" ", " ");
                    if (!newtext.Equals(Spelling))
                        Spelling= newtext;            
            #endregion 0536ImportCode
        }               
        private void OnChangedContent(object oldValue, object newValue)
        {
            #region 0562ImportCode
                                Quantity = GetDefaultQuantity();
                    if (newValue is null) return;
                    //Xử lý ký tự đặc biệt mã ASCII 160 giống dấu cách
                    var newtext = Content.Replace(" ", " ").Replace("ﬁ", "fi");
                    //if (Module.Helpers.ParameterHelper.GetBooleanOrDefault(Session, "RemovingAccentsDiacritics", false))
                    //    newtext = Module.Helpers.TextHelper.RemoveAccents(newtext);
                    if (!newtext.Equals(Content))
                        Content = newtext;
                    //002: Chuyển vào chức năng FindCaseType
                    //if (Video != null)
                    //    CaseType = GetDefaultCaseType();            
            #endregion 0562ImportCode
        }               
        private void OnChangedVideo(object oldValue, object newValue)
        {
            #region 0561ImportCode
            if (newValue is null) return;
                    if(Quantity is null)
                        Quantity = GetDefaultQuantity();
                    //002: Chuyển vào chức năng FindCaseType
                    //CaseType = GetDefaultCaseType();            
            #endregion 0561ImportCode
        }               
        private void OnChangedSubtitle(object oldValue, object newValue)
        {
            #region 0535ImportCode
            if (newValue is null) return;
var newtext = Subtitle.Replace(" ", " ");
                    if (!newtext.Equals(Subtitle))
                        Subtitle = newtext;            
            #endregion 0535ImportCode
        }               
        private void OnChangedParagraphStyle(object oldValue, object newValue)
        {
            #region 1041ImportCode
            if (newValue != null)
                    {
                        if (((ParagraphStyle)newValue).ElementQuantity is null)
                            ((ParagraphStyle)newValue).ElementQuantity = 0;
                        ((ParagraphStyle)newValue).ElementQuantity++;
                    }                        
                    if (oldValue != null)
                        ((ParagraphStyle)oldValue ).ElementQuantity--;            
            #endregion 1041ImportCode
        }               
        private void OnChangedAudioDuration(object oldValue, object newValue)
        {
            #region 0537ImportCode
            if (newValue is null) return;
SetDefaultAudioTime();            
            #endregion 0537ImportCode
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
			//	SetDefaultElementTranslateList();
			//	SetDefaultTermLocationList();
			//	SetDefaultMediaList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 0533ImportCode
		public void SetDefaultSpellingTime(View view = null)
        {
            //Code: 0533            Oid: 01fc71e7-b842-4f9e-b2eb-8f65a3ff4859
            SpellingTime= GetDefaultSpellingTime();
        }
#endregion 0533ImportCode
#region 0048ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0048            Oid: 0a234f7c-ee1c-4410-9580-c374870c55ef
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 0048ImportCode
#region 1525ImportCode
		public Module.BusinessObjects.Audio GetDefaultPreviousElement(View view = null)
        {
            //Code: 1525            Oid: 342d3aac-e07f-4173-afb8-03746dee9dd4
            if(Video != null && Start != null)
{
    return Video.GetAudioListWithSort(BookMark, false, TranslateObject).FirstOrDefault(x => x.Start < Start);
}

return null;
        }
#endregion 1525ImportCode
#region 1074ImportCode
		public CaseType GetDefaultCaseType(View view = null)
        {
            //Code: 1074            Oid: c82486d0-382f-4e6f-9da5-3433c915eb56
                        if (!string.IsNullOrEmpty(Content) && Video != null)
            {
                if (Module.Helpers.TextHelper.CheckUpperCaseAll(Content))
                    return CaseType.UpperCaseAll;
                if(Module.Services.AudioService.ElementFlagUpperCase(Video, Content))
                    return CaseType.UpperCase;
                if (Module.Services.AudioService.ElementFlagUpperCase(Video, Content, "Content", true))
                    return CaseType.UpperCaseMany;
            }
			return CaseType.General;
        }
#endregion 1074ImportCode
#region 0560ImportCode
		public decimal? GetDefaultQuantity(View view = null)
        {
            //Code: 0560            Oid: 1be69bb7-9333-44a4-8b5a-c46836eda9ac
            //End == null là trường hợp không phải phụ đề
            if (!string.IsNullOrEmpty(Content) && End != null)            {
                string languageCode = (Video != null && Video.LanguageOrigin != null) ? Video.LanguageOrigin.Code : null;
                var result = Module.Helpers.TextHelper.GetWordVowelWeight(languageCode, Content);
                if(result != (decimal)0)
                {
                    return result;
                }
            }
			return null;

        }
#endregion 0560ImportCode
#region 0529ImportCode
		public DateTime? GetDefaultSubtitleTime(View view = null)
        {
            //Code: 0529            Oid: 4738915c-a526-444c-9743-cebfd1712d35
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0529ImportCode
#region 0574ImportCode
		public decimal? GetDefaultAudioDuration(View view = null)
        {
            //Code: 0574            Oid: 42d3b1de-3ad8-457f-82f2-a3b131623217
            if (FileData != null && FileData.Content != null)
            {
                string audioFolder = System.IO.Directory.GetCurrentDirectory() + "\\Audio";
                if (!System.IO.Directory.Exists(audioFolder))
                    System.IO.Directory.CreateDirectory(audioFolder);
                var audioFile = audioFolder + "\\" + FileData.Oid + ".mp3";
                return GetDurationAudio(audioFile);
            }                      
            return 0;
        }
        public decimal GetDurationAudio(string audioFile)
        {            
            if (!string.IsNullOrEmpty(audioFile))
            {
                try
                {
                    //var ffmpegUrl = Module.Helpers.ParameterHelper.GetValueOrDefault(Session, "FfmpegUrl", "\\\\dc\\Setup\\Graphic\\Ffmpeg\\ffmpeg.exe");
                    var ffprobeUrl = Module.Helpers.ParameterHelper.GetValueOrDefault(Session, "FprobeUrl", "\\\\dc\\Setup\\Graphic\\Ffmpeg\\ffprobe.exe");
                    if (System.IO.File.Exists(ffprobeUrl))
                    {
                        System.IO.File.WriteAllBytes(audioFile, FileData.Content);
                        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(ffprobeUrl);
                        psi.Arguments = "-i \"" + audioFile + "\" -show_entries format=duration -v quiet -of csv=\"p=0\"";
                        psi.RedirectStandardOutput = true;
                        psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        psi.UseShellExecute = false;
                        System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi);
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.EnableRaisingEvents = true;
                        process.WaitForExit();
                        ////Create a streamreader to capture the output of ischk
                        System.IO.StreamReader ischkout = process.StandardOutput;
                        process.WaitForExit();
                        if (process.HasExited)
                        {
                            string output = ischkout.ReadToEnd();
                            if (!string.IsNullOrEmpty(output))
                            {
                                output = output.Replace("\r", "").Replace("\n", "");
                                return decimal.Parse(output, new System.Globalization.CultureInfo("en-us"));
                            }
                        }
                        //Xóa file tạm mục bộ nhớ tạm
                        System.IO.File.Delete(audioFile);
                    }
                }
                catch (System.Exception ex)
                {

                }
            }
            return 0;
        }
#endregion 0574ImportCode
#region 0530ImportCode
		public DateTime? GetDefaultSpellingTime(View view = null)
        {
            //Code: 0530            Oid: 6c10ba4b-4d0b-493e-88fc-a04bf3d6edee
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0530ImportCode
#region 0136ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0136            Oid: 5a2a5740-6f37-4cc5-ac23-b2ada42c08fc
            return System.DateTime.Now;
        }
#endregion 0136ImportCode
#region 0531ImportCode
		public DateTime? GetDefaultAudioTime(View view = null)
        {
            //Code: 0531            Oid: ddf39b7a-6632-4789-8373-103fb94480ff
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0531ImportCode
#region 0532ImportCode
		public void SetDefaultSubtitleTime(View view = null)
        {
            //Code: 0532            Oid: 78ed577d-8863-4a5a-b9eb-11643408bc70
            SubtitleTime= GetDefaultSubtitleTime();
        }
#endregion 0532ImportCode
#region 0534ImportCode
		public void SetDefaultAudioTime(View view = null)
        {
            //Code: 0534            Oid: e7982100-4397-44c0-a3f5-f09c226c15b2
            AudioTime= GetDefaultAudioTime();
        }
#endregion 0534ImportCode
#region 1524ImportCode
		public Module.BusinessObjects.Audio GetDefaultNextElement(View view = null)
        {
            //Code: 1524            Oid: c43970ab-9112-4cd4-bb75-c01278e2461c
            if (Video != null && Start != null)
{
    return Video.GetAudioListWithSort(BookMark, true, TranslateObject).FirstOrDefault(x => x.Start > Start);
}
return null;
        }
#endregion 1524ImportCode
        #endregion
//Mã nguồn bổ sung
#region AudioImportCode
      public TimeSpan? GetRealEnd()
      {
          if (AudioDuration > 0 && Start != null)
          {
              decimal voiceSpeed = VoiceSpeed != null ? VoiceSpeed.Value : 1;
              var realDuration = AudioDuration / voiceSpeed;
              return Start.Value.Add(System.TimeSpan.FromSeconds(System.Convert.ToDouble(realDuration)));
          }
          return null;
      }

      public TimeSpan? GetRealTimeSpan(TimeSpan? timeSpan)
      {
          if (timeSpan != null && timeSpan.Value.Days > 0)
              return timeSpan.Value.Add(new TimeSpan(timeSpan.Value.Days * (-1), 0, 0, 0));
          return timeSpan;
      }
#endregion AudioImportCode
		 		 
    }
}
