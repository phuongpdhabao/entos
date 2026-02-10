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
    [ModelDefault("Caption", "Hình ảnh"), ImageName("Media")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "Paragraph_MediaList_ListView", TargetItems = nameof(Content))]
	[MobileColumnAttribute(Context = "Media_LookupListView", TargetItems = nameof(Content))]
	[MobileColumnAttribute(Context = "Audio_MediaList_ListView", TargetItems = nameof(Content))]
	[MobileColumnAttribute(Context = "Media_ListView", TargetItems = nameof(Content))]
	[MobileColumnAttribute(Context = "Video_MediaList_ListView", TargetItems = nameof(Content))]
	[DefaultProperty("Content")]
 
[OptimisticLocking(true)]
    public partial class Media:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Media(Session session)
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
               

		//private TimeSpan? _start;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Bắt đầu")]
        [ToolTip("Bắt đầu")]
		//[Index(0)]		
	    [ModelDefault("MaskSettings", "AgAAAA9NYXNrTWFuYWdlclR5cGUAgwFEZXZFeHByZXNzLkRhdGEuTWFzay5UaW1lU3Bhbk1hc2tNYW5hZ2VyLCBEZXZFeHByZXNzLkRhdGEudjIyLjEsIFZlcnNpb249MjIuMS4zLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjg4ZDE3NTRkNzAwZTQ5YQRtYXNrBwIMW2QuXWhoOm1tOnNz")]
		public TimeSpan? Start
        { 
		    get => GetPropertyValue<TimeSpan?>("Start");                         
			set => SetPropertyValue<TimeSpan?>("Start", value); 
			
        }
		//Tooltip for Object
		public object StartToolTipControllerText(View view)
        {
        //    if (Start != null) 
		//			return Start;
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
        //    if (End != null) 
		//			return End;
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

	
       
		//private string _content;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(2)]		

 		[Size(250)]
		public string Content
        { 
		    get => GetPropertyValue<string>("Content");                         
			set => SetPropertyValue<string>("Content", value); 
			
        }
		//Tooltip for Object
		public object ContentToolTipControllerText(View view)
        {
        //    if (Content != null) 
		//			return Content;
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

	
       
		//private string _mediafile;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tệp Media")]
        [ToolTip("Tệp Media")]
		//[Index(3)]		

 		[Size(250)]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string MediaFile
        { 
		    get => GetPropertyValue<string>("MediaFile");                         
			set => SetPropertyValue<string>("MediaFile", value); 
			
        }
		//Tooltip for Object
		public object MediaFileToolTipControllerText(View view)
        {
        //    if (MediaFile != null) 
		//			return MediaFile;
            return null;
        }
		//Get Default Value
        public string GetDefaultMediaFile(View view = null)
        { 
			return MediaFile;
        }
		//Set Default Value
		public void SetDefaultMediaFile(View view = null)
        {
            //if (MediaFile is null){
            //    var result = GetDefaultMediaFile(view);
            //    if (result != null && result != MediaFile){
			//          MediaFile = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MediaFileIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMediaFile();
				//if (result != null && MediaFile != null){
				//	return !MediaFile.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private TimeSpan? _mediastart;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bắt đầu Media")]
        [ToolTip("Bắt đầu Media")]
		//[Index(4)]		
	    [ModelDefault("MaskSettings", "AgAAAA9NYXNrTWFuYWdlclR5cGUAgwFEZXZFeHByZXNzLkRhdGEuTWFzay5UaW1lU3Bhbk1hc2tNYW5hZ2VyLCBEZXZFeHByZXNzLkRhdGEudjIyLjEsIFZlcnNpb249MjIuMS4zLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjg4ZDE3NTRkNzAwZTQ5YQRtYXNrBwIMW2QuXWhoOm1tOnNz")]
		public TimeSpan? MediaStart
        { 
		    get => GetPropertyValue<TimeSpan?>("MediaStart");                         
			set => SetPropertyValue<TimeSpan?>("MediaStart", value); 
			
        }
		//Tooltip for Object
		public object MediaStartToolTipControllerText(View view)
        {
        //    if (MediaStart != null) 
		//			return MediaStart;
            return null;
        }
		//Get Default Value
        public TimeSpan? GetDefaultMediaStart(View view = null)
        { 
			return MediaStart;
        }
		//Set Default Value
		public void SetDefaultMediaStart(View view = null)
        {
            //if (MediaStart is null){
            //    var result = GetDefaultMediaStart(view);
            //    if (result != null && result != MediaStart){
			//          MediaStart = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MediaStartIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMediaStart();
				//if (result != null && MediaStart != null){
				//	return !MediaStart.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal _mediaduration;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thời lượng Media")]
        [ToolTip("Thời lượng Media")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal MediaDuration
        { 
		    get => GetPropertyValue<decimal>("MediaDuration");                         
			set => SetPropertyValue<decimal>("MediaDuration", value); 
			
        }
		//Tooltip for Object
		public object MediaDurationToolTipControllerText(View view)
        {
        //    if (MediaDuration != null) 
		//			return MediaDuration;
            return null;
        }
		//Get Default Value
        public decimal GetDefaultMediaDuration(View view = null)
        { 
			return MediaDuration;
        }
		//Set Default Value
		public void SetDefaultMediaDuration(View view = null)
        {
            //if (MediaDuration is null){
            //    var result = GetDefaultMediaDuration(view);
            //    if (result != null && result != MediaDuration){
			//          MediaDuration = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MediaDurationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMediaDuration();
				//if (result != null && MediaDuration != null){
				//	return !MediaDuration.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _mediaspeed;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tốc độ")]
        [ToolTip("Tốc độ")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "{0:n1}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? MediaSpeed
        { 
		    get => GetPropertyValue<decimal?>("MediaSpeed");                         
			set => SetPropertyValue<decimal?>("MediaSpeed", value); 
			
        }
		//Tooltip for Object
		public object MediaSpeedToolTipControllerText(View view)
        {
        //    if (MediaSpeed != null) 
		//			return MediaSpeed;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultMediaSpeed(View view = null)
        { 
			return MediaSpeed;
        }
		//Set Default Value
		public void SetDefaultMediaSpeed(View view = null)
        {
            //if (MediaSpeed is null){
            //    var result = GetDefaultMediaSpeed(view);
            //    if (result != null && result != MediaSpeed){
			//          MediaSpeed = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MediaSpeedIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMediaSpeed();
				//if (result != null && MediaSpeed != null){
				//	return !MediaSpeed.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _photo;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ảnh")]
        [ToolTip("Ảnh")]
		//[Index(7)]		
		public bool Photo
        { 
		    get => GetPropertyValue<bool>("Photo");                         
			set => SetPropertyValue<bool>("Photo", value); 
			
        }
		//Tooltip for Object
		public object PhotoToolTipControllerText(View view)
        {
        //    if (Photo != null) 
		//			return Photo;
            return null;
        }
		//Get Default Value
        public bool GetDefaultPhoto(View view = null)
        { 
			return Photo;
        }
		//Set Default Value
		public void SetDefaultPhoto(View view = null)
        {
            //if (Photo is null){
            //    var result = GetDefaultPhoto(view);
            //    if (result != null && result != Photo){
			//          Photo = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PhotoIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPhoto();
				//if (result != null && Photo != null){
				//	return !Photo.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Paragraph _paragraph;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đoạn văn bản")]
        [ToolTip("Đoạn văn bản")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ParagraphCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Paragraph-MediaList")]
	 
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
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Âm thanh")]
		//[Index(9)]
		[DataSourceCriteria("Not MediaList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("AudioList-MediaList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Audio> AudioList
        {      
		    get => GetCollection<Module.BusinessObjects.Audio>("AudioList"); 
			
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
		[DevExpress.Xpo.Association("Video-MediaList")]
	 
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
	
       
		//private decimal _duration;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thời lượng")]
        [ToolTip("Thời lượng")]
		//[Index(11)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal Duration
        { 
		    get => GetPropertyValue<decimal>("Duration");                         
			set => SetPropertyValue<decimal>("Duration", value); 
			
        }
		//Tooltip for Object
		public object DurationToolTipControllerText(View view)
        {
        //    if (Duration != null) 
		//			return Duration;
            return null;
        }
		//Get Default Value
        public decimal GetDefaultDuration(View view = null)
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

	
       
		//private decimal _audioduration;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thời lượng âm")]
        [ToolTip("Thời lượng âm")]
		//[Index(12)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal AudioDuration
        { 
		    get => GetPropertyValue<decimal>("AudioDuration");                         
			set => SetPropertyValue<decimal>("AudioDuration", value); 
			
        }
		//Tooltip for Object
		public object AudioDurationToolTipControllerText(View view)
        {
        //    if (AudioDuration != null) 
		//			return AudioDuration;
            return null;
        }
		//Get Default Value
        public decimal GetDefaultAudioDuration(View view = null)
        { 
			return AudioDuration;
        }
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

	
       
		//private bool _flag;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ")]
        [ToolTip("Cờ")]
		//[Index(13)]		
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
		//[Index(14)]		
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

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(15)]		
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
        public int? GetDefaultOrder(View view = null)
        { 
			return Order;
        }
		//Set Default Value
		public void SetDefaultOrder(View view = null)
        {
            //if (Order is null){
            //    var result = GetDefaultOrder(view);
            //    if (result != null && result != Order){
			//          Order = result;
            //	  }
            //}
        }

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

	
       
		//private string _text;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Văn bản")]
        [ToolTip("Văn bản")]
		//[Index(16)]		

 		[Size(250)]
		public string Text
        { 
		    get => GetPropertyValue<string>("Text");                         
			set => SetPropertyValue<string>("Text", value); 
			
        }
		//Tooltip for Object
		public object TextToolTipControllerText(View view)
        {
        //    if (Text != null) 
		//			return Text;
            return null;
        }
		//Get Default Value
        public string GetDefaultText(View view = null)
        { 
			return Text;
        }
		//Set Default Value
		public void SetDefaultText(View view = null)
        {
            //if (Text is null){
            //    var result = GetDefaultText(view);
            //    if (result != null && result != Text){
			//          Text = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TextIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultText();
				//if (result != null && Text != null){
				//	return !Text.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _textprevious;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Văn bản trước")]
        [ToolTip("Văn bản trước")]
		//[Index(17)]		

 		[Size(250)]
		public string TextPrevious
        { 
		    get => GetPropertyValue<string>("TextPrevious");                         
			set => SetPropertyValue<string>("TextPrevious", value); 
			
        }
		//Tooltip for Object
		public object TextPreviousToolTipControllerText(View view)
        {
        //    if (TextPrevious != null) 
		//			return TextPrevious;
            return null;
        }
		//Get Default Value
        public string GetDefaultTextPrevious(View view = null)
        { 
			return TextPrevious;
        }
		//Set Default Value
		public void SetDefaultTextPrevious(View view = null)
        {
            //if (TextPrevious is null){
            //    var result = GetDefaultTextPrevious(view);
            //    if (result != null && result != TextPrevious){
			//          TextPrevious = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TextPreviousIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTextPrevious();
				//if (result != null && TextPrevious != null){
				//	return !TextPrevious.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _textnext;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Văn bản sau")]
        [ToolTip("Văn bản sau")]
		//[Index(18)]		

 		[Size(250)]
		public string TextNext
        { 
		    get => GetPropertyValue<string>("TextNext");                         
			set => SetPropertyValue<string>("TextNext", value); 
			
        }
		//Tooltip for Object
		public object TextNextToolTipControllerText(View view)
        {
        //    if (TextNext != null) 
		//			return TextNext;
            return null;
        }
		//Get Default Value
        public string GetDefaultTextNext(View view = null)
        { 
			return TextNext;
        }
		//Set Default Value
		public void SetDefaultTextNext(View view = null)
        {
            //if (TextNext is null){
            //    var result = GetDefaultTextNext(view);
            //    if (result != null && result != TextNext){
			//          TextNext = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TextNextIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTextNext();
				//if (result != null && TextNext != null){
				//	return !TextNext.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.ParagraphStyle _paragraphstyle;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Kiểu cách")]
        [ToolTip("Kiểu cách")]
		//[Index(19)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ParagraphStyleCriteria))]
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
        //    if (ParagraphStyle != null) 
		//			return ParagraphStyle;
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
	
       
		//private MediaType _mediatype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(20)]		
		public MediaType MediaType
        { 
		    get => GetPropertyValue<MediaType>("MediaType");                         
			set => SetPropertyValue<MediaType>("MediaType", value); 
			
        }
		//Tooltip for Object
		public object MediaTypeToolTipControllerText(View view)
        {
        //    if (MediaType != null) 
		//			return MediaType;
            return null;
        }
		//Get Default Value
        public MediaType GetDefaultMediaType(View view = null)
        { 
			return MediaType;
        }
		//Set Default Value
		public void SetDefaultMediaType(View view = null)
        {
            //if (MediaType is null){
            //    var result = GetDefaultMediaType(view);
            //    if (result != null && result != MediaType){
			//          MediaType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MediaTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMediaType();
				//if (result != null && MediaType != null){
				//	return !MediaType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.BookMark _bookmark;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
        [ToolTip("Liên kết")]
		//[Index(21)]		
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
	
       
		//private string _shapetypetext;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại hình")]
        [ToolTip("Loại hình")]
		//[Index(23)]		

 		[Size(50)]
	    [ModelDefault("AllowEdit", "False")]
		public string ShapeTypeText
        { 
		    get => GetPropertyValue<string>("ShapeTypeText");                         
			set => SetPropertyValue<string>("ShapeTypeText", value); 
			
        }
		//Tooltip for Object
		public object ShapeTypeTextToolTipControllerText(View view)
        {
        //    if (ShapeTypeText != null) 
		//			return ShapeTypeText;
            return null;
        }
		//Get Default Value
        public string GetDefaultShapeTypeText(View view = null)
        { 
			return ShapeTypeText;
        }
		//Set Default Value
		public void SetDefaultShapeTypeText(View view = null)
        {
            //if (ShapeTypeText is null){
            //    var result = GetDefaultShapeTypeText(view);
            //    if (result != null && result != ShapeTypeText){
			//          ShapeTypeText = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ShapeTypeTextIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultShapeTypeText();
				//if (result != null && ShapeTypeText != null){
				//	return !ShapeTypeText.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _height;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cao")]
        [ToolTip("Cao")]
		//[Index(24)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
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
		//[Index(25)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
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

	
       
		//private Microsoft.Office.Interop.Word.WdWrapType _textwrappingtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bố cục")]
        [ToolTip("Bố cục")]
		//[Index(26)]		
	    [ModelDefault("AllowEdit", "True")]
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

	
       
		//private bool _allowoverlap;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Được đè")]
        [ToolTip("Được đè")]
		//[Index(27)]		
		public bool AllowOverlap
        { 
		    get => GetPropertyValue<bool>("AllowOverlap");                         
			set => SetPropertyValue<bool>("AllowOverlap", value); 
			
        }
		//Tooltip for Object
		public object AllowOverlapToolTipControllerText(View view)
        {
        //    if (AllowOverlap != null) 
		//			return AllowOverlap;
            return null;
        }
		//Get Default Value
        public bool GetDefaultAllowOverlap(View view = null)
        { 
			return AllowOverlap;
        }
		//Set Default Value
		public void SetDefaultAllowOverlap(View view = null)
        {
            //if (AllowOverlap is null){
            //    var result = GetDefaultAllowOverlap(view);
            //    if (result != null && result != AllowOverlap){
			//          AllowOverlap = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AllowOverlapIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAllowOverlap();
				//if (result != null && AllowOverlap != null){
				//	return !AllowOverlap.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Alignment _alignment;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Căn lề")]
        [ToolTip("Căn lề")]
		//[Index(28)]		
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

	
       
		//private Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition? _alignmentrelative;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mốc căn lề")]
        [ToolTip("Mốc căn lề")]
		//[Index(29)]		
		public Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition? AlignmentRelative
        { 
		    get => GetPropertyValue<Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition?>("AlignmentRelative");                         
			set => SetPropertyValue<Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition?>("AlignmentRelative", value); 
			
        }
		//Tooltip for Object
		public object AlignmentRelativeToolTipControllerText(View view)
        {
        //    if (AlignmentRelative != null) 
		//			return AlignmentRelative;
            return null;
        }
		//Get Default Value
        public Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition? GetDefaultAlignmentRelative(View view = null)
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
		[DevExpress.Xpo.DisplayName("Theo văn bản")]
        [ToolTip("Theo văn bản")]
		//[Index(30)]		
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

	
       
		//private Module.BusinessObjects.Media _uppermedia;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(31)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpperMediaCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Media UpperMedia
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Media>("UpperMedia");                         
			set => SetPropertyValue<Module.BusinessObjects.Media>("UpperMedia", value); 
			
        }
		//Tooltip for Object
		public object UpperMediaToolTipControllerText(View view)
        {
        //    if (UpperMedia != null) 
		//			return UpperMedia;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Media GetDefaultUpperMedia(View view = null)
        { 
			return UpperMedia;
        }
		//Set Default Value
		public void SetDefaultUpperMedia(View view = null)
        {
            //if (UpperMedia is null){
            //    var result = GetDefaultUpperMedia(view);
            //    if (result != null && result != UpperMedia){
			//          UpperMedia = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperMediaIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpperMedia();
				//if (result != null && UpperMedia != null){
				//	return !UpperMedia.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpperMediaCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(UpperMedia));
            }
        }
	
       
		//private Microsoft.Office.Interop.Word.WdWrapType _textwrappingtypenew;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bố cục mới")]
        [ToolTip("Bố cục mới")]
		//[Index(32)]		
		public Microsoft.Office.Interop.Word.WdWrapType TextWrappingTypeNew
        { 
		    get => GetPropertyValue<Microsoft.Office.Interop.Word.WdWrapType>("TextWrappingTypeNew");                         
			set => SetPropertyValue<Microsoft.Office.Interop.Word.WdWrapType>("TextWrappingTypeNew", value); 
			
        }
		//Tooltip for Object
		public object TextWrappingTypeNewToolTipControllerText(View view)
        {
        //    if (TextWrappingTypeNew != null) 
		//			return TextWrappingTypeNew;
            return null;
        }
		//Get Default Value
        public Microsoft.Office.Interop.Word.WdWrapType GetDefaultTextWrappingTypeNew(View view = null)
        { 
			return TextWrappingTypeNew;
        }
		//Set Default Value
		public void SetDefaultTextWrappingTypeNew(View view = null)
        {
            //if (TextWrappingTypeNew is null){
            //    var result = GetDefaultTextWrappingTypeNew(view);
            //    if (result != null && result != TextWrappingTypeNew){
			//          TextWrappingTypeNew = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TextWrappingTypeNewIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTextWrappingTypeNew();
				//if (result != null && TextWrappingTypeNew != null){
				//	return !TextWrappingTypeNew.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Alignment _alignmentnew;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Căn lề mới")]
        [ToolTip("Căn lề mới")]
		//[Index(33)]		
		public Alignment AlignmentNew
        { 
		    get => GetPropertyValue<Alignment>("AlignmentNew");                         
			set => SetPropertyValue<Alignment>("AlignmentNew", value); 
			
        }
		//Tooltip for Object
		public object AlignmentNewToolTipControllerText(View view)
        {
        //    if (AlignmentNew != null) 
		//			return AlignmentNew;
            return null;
        }
		//Get Default Value
        public Alignment GetDefaultAlignmentNew(View view = null)
        { 
			return AlignmentNew;
        }
		//Set Default Value
		public void SetDefaultAlignmentNew(View view = null)
        {
            //if (AlignmentNew is null){
            //    var result = GetDefaultAlignmentNew(view);
            //    if (result != null && result != AlignmentNew){
			//          AlignmentNew = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AlignmentNewIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAlignmentNew();
				//if (result != null && AlignmentNew != null){
				//	return !AlignmentNew.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _shapeid;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Shape Id")]
        [ToolTip("Shape Id")]
		//[Index(34)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? ShapeId
        { 
		    get => GetPropertyValue<int?>("ShapeId");                         
			set => SetPropertyValue<int?>("ShapeId", value); 
			
        }
		//Tooltip for Object
		public object ShapeIdToolTipControllerText(View view)
        {
        //    if (ShapeId != null) 
		//			return ShapeId;
            return null;
        }
		//Get Default Value
        public int? GetDefaultShapeId(View view = null)
        { 
			return ShapeId;
        }
		//Set Default Value
		public void SetDefaultShapeId(View view = null)
        {
            //if (ShapeId is null){
            //    var result = GetDefaultShapeId(view);
            //    if (result != null && result != ShapeId){
			//          ShapeId = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ShapeIdIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultShapeId();
				//if (result != null && ShapeId != null){
				//	return !ShapeId.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _shapename;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên Shape")]
        [ToolTip("Tên Shape")]
		//[Index(35)]		

 		[Size(200)]
		public string ShapeName
        { 
		    get => GetPropertyValue<string>("ShapeName");                         
			set => SetPropertyValue<string>("ShapeName", value); 
			
        }
		//Tooltip for Object
		public object ShapeNameToolTipControllerText(View view)
        {
        //    if (ShapeName != null) 
		//			return ShapeName;
            return null;
        }
		//Get Default Value
        public string GetDefaultShapeName(View view = null)
        { 
			return ShapeName;
        }
		//Set Default Value
		public void SetDefaultShapeName(View view = null)
        {
            //if (ShapeName is null){
            //    var result = GetDefaultShapeName(view);
            //    if (result != null && result != ShapeName){
			//          ShapeName = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ShapeNameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultShapeName();
				//if (result != null && ShapeName != null){
				//	return !ShapeName.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _top;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Phía trên")]
		[ToolTip("Nếu top nhiều thì phải lưu ý trả về vị trí đúng ( >20)")]
		//[Index(36)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Top
        { 
		    get => GetPropertyValue<decimal?>("Top");                         
			set => SetPropertyValue<decimal?>("Top", value); 
			
        }
		//Tooltip for Object
		public object TopToolTipControllerText(View view)
        {
        //    if (Top != null) 
		//			return Top;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultTop(View view = null)
        { 
			return Top;
        }
		//Set Default Value
		public void SetDefaultTop(View view = null)
        {
            //if (Top is null){
            //    var result = GetDefaultTop(view);
            //    if (result != null && result != Top){
			//          Top = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TopIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTop();
				//if (result != null && Top != null){
				//	return !Top.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _pagenumber;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trang số")]
        [ToolTip("Trang số")]
		//[Index(37)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? PageNumber
        { 
		    get => GetPropertyValue<int?>("PageNumber");                         
			set => SetPropertyValue<int?>("PageNumber", value); 
			
        }
		//Tooltip for Object
		public object PageNumberToolTipControllerText(View view)
        {
        //    if (PageNumber != null) 
		//			return PageNumber;
            return null;
        }
		//Get Default Value
        public int? GetDefaultPageNumber(View view = null)
        { 
			return PageNumber;
        }
		//Set Default Value
		public void SetDefaultPageNumber(View view = null)
        {
            //if (PageNumber is null){
            //    var result = GetDefaultPageNumber(view);
            //    if (result != null && result != PageNumber){
			//          PageNumber = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PageNumberIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPageNumber();
				//if (result != null && PageNumber != null){
				//	return !PageNumber.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _quantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(38)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Quantity
        { 
		    get => GetPropertyValue<decimal?>("Quantity");                         
			set => SetPropertyValue<decimal?>("Quantity", value); 
			
        }
		//Tooltip for Object
		public object QuantityToolTipControllerText(View view)
        {
        //    if (Quantity != null) 
		//			return Quantity;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultQuantity(View view = null)
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

	
       
		//private bool _resizewithtext;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giãn theo văn bản")]
        [ToolTip("Giãn theo văn bản")]
		//[Index(39)]		
		public bool ResizeWithText
        { 
		    get => GetPropertyValue<bool>("ResizeWithText");                         
			set => SetPropertyValue<bool>("ResizeWithText", value); 
			
        }
		//Tooltip for Object
		public object ResizeWithTextToolTipControllerText(View view)
        {
        //    if (ResizeWithText != null) 
		//			return ResizeWithText;
            return null;
        }
		//Get Default Value
        public bool GetDefaultResizeWithText(View view = null)
        { 
			return ResizeWithText;
        }
		//Set Default Value
		public void SetDefaultResizeWithText(View view = null)
        {
            //if (ResizeWithText is null){
            //    var result = GetDefaultResizeWithText(view);
            //    if (result != null && result != ResizeWithText){
			//          ResizeWithText = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ResizeWithTextIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultResizeWithText();
				//if (result != null && ResizeWithText != null){
				//	return !ResizeWithText.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Color? _fillcolor;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Màu nền")]
        [ToolTip("Màu nền")]
		//[Index(40)]		
	    [DevExpress.Xpo.Persistent()]
	    [ValueConverter(typeof(DevExpress.ExpressApp.StateMachine.Xpo.NullableColorConverter))]
		public Color? FillColor
        { 
		    get => GetPropertyValue<Color?>("FillColor");                         
			set => SetPropertyValue<Color?>("FillColor", value); 
			
        }
		//Tooltip for Object
		public object FillColorToolTipControllerText(View view)
        {
        //    if (FillColor != null) 
		//			return FillColor;
            return null;
        }
		//Get Default Value
        public Color? GetDefaultFillColor(View view = null)
        { 
			return FillColor;
        }
		//Set Default Value
		public void SetDefaultFillColor(View view = null)
        {
            //if (FillColor is null){
            //    var result = GetDefaultFillColor(view);
            //    if (result != null && result != FillColor){
			//          FillColor = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FillColorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFillColor();
				//if (result != null && FillColor != null){
				//	return !FillColor.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Color? _linecolor;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Màu viền")]
        [ToolTip("Màu viền")]
		//[Index(41)]		
	    [DevExpress.Xpo.Persistent()]
	    [ValueConverter(typeof(DevExpress.ExpressApp.StateMachine.Xpo.NullableColorConverter))]
		public Color? LineColor
        { 
		    get => GetPropertyValue<Color?>("LineColor");                         
			set => SetPropertyValue<Color?>("LineColor", value); 
			
        }
		//Tooltip for Object
		public object LineColorToolTipControllerText(View view)
        {
        //    if (LineColor != null) 
		//			return LineColor;
            return null;
        }
		//Get Default Value
        public Color? GetDefaultLineColor(View view = null)
        { 
			return LineColor;
        }
		//Set Default Value
		public void SetDefaultLineColor(View view = null)
        {
            //if (LineColor is null){
            //    var result = GetDefaultLineColor(view);
            //    if (result != null && result != LineColor){
			//          LineColor = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LineColorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLineColor();
				//if (result != null && LineColor != null){
				//	return !LineColor.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _fillcode;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã nền")]
        [ToolTip("Mã nền")]
		//[Index(42)]		

 		[Size(100)]
	    [NonPersistent()]
	    [NotMapped()]
		public string FillCode
        { 
		    #region 2597ImportCode 
get => FillColor?.ToString();
#endregion 2597ImportCode
			
        }
		//Tooltip for Object
		public object FillCodeToolTipControllerText(View view)
        {
        //    if (FillCode != null) 
		//			return FillCode;
            return null;
        }
		//Get Default Value
        public string GetDefaultFillCode(View view = null)
        { 
			return FillCode;
        }
		//Set Default Value
		public void SetDefaultFillCode(View view = null)
        {
            //if (FillCode is null){
            //    var result = GetDefaultFillCode(view);
            //    if (result != null && result != FillCode){
			//          FillCode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FillCodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFillCode();
				//if (result != null && FillCode != null){
				//	return !FillCode.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _linecode;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã viền")]
        [ToolTip("Mã viền")]
		//[Index(43)]		

 		[Size(100)]
	    [NonPersistent()]
	    [NotMapped()]
		public string LineCode
        { 
		    #region 2598ImportCode 
get => LineColor?.ToString();
#endregion 2598ImportCode
			
        }
		//Tooltip for Object
		public object LineCodeToolTipControllerText(View view)
        {
        //    if (LineCode != null) 
		//			return LineCode;
            return null;
        }
		//Get Default Value
        public string GetDefaultLineCode(View view = null)
        { 
			return LineCode;
        }
		//Set Default Value
		public void SetDefaultLineCode(View view = null)
        {
            //if (LineCode is null){
            //    var result = GetDefaultLineCode(view);
            //    if (result != null && result != LineCode){
			//          LineCode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LineCodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLineCode();
				//if (result != null && LineCode != null){
				//	return !LineCode.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultStart(View view = null);
        //SetDefaultEnd(View view = null);
        //SetDefaultContent(View view = null);
        //SetDefaultMediaFile(View view = null);
        //SetDefaultMediaStart(View view = null);
        //SetDefaultMediaDuration(View view = null);
        //SetDefaultMediaSpeed(View view = null);
        //SetDefaultPhoto(View view = null);
        //SetDefaultParagraph(View view = null);
        //SetDefaultVideo(View view = null);
        //SetDefaultDuration(View view = null);
        //SetDefaultAudioDuration(View view = null);
        //SetDefaultFlag(View view = null);
        //SetDefaultFlag2(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultText(View view = null);
        //SetDefaultTextPrevious(View view = null);
        //SetDefaultTextNext(View view = null);
        //SetDefaultParagraphStyle(View view = null);
        //SetDefaultMediaType(View view = null);
        //SetDefaultBookMark(View view = null);
        //SetDefaultShapeTypeText(View view = null);
        //SetDefaultHeight(View view = null);
        //SetDefaultWidth(View view = null);
        //SetDefaultTextWrappingType(View view = null);
        //SetDefaultAllowOverlap(View view = null);
        //SetDefaultAlignment(View view = null);
        //SetDefaultAlignmentRelative(View view = null);
        //SetDefaultMoveWithText(View view = null);
        //SetDefaultUpperMedia(View view = null);
        //SetDefaultTextWrappingTypeNew(View view = null);
        //SetDefaultAlignmentNew(View view = null);
        //SetDefaultShapeId(View view = null);
        //SetDefaultShapeName(View view = null);
        //SetDefaultTop(View view = null);
        //SetDefaultPageNumber(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultResizeWithText(View view = null);
        //SetDefaultFillColor(View view = null);
        //SetDefaultLineColor(View view = null);
        //SetDefaultFillCode(View view = null);
        //SetDefaultLineCode(View view = null);
			
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
				
                    case nameof(Alignment):
                        OnChangedAlignment(oldValue, newValue);
                        break;
				
                    case nameof(TextWrappingType):
                        OnChangedTextWrappingType(oldValue, newValue);
                        break;
				
                    case nameof(MediaFile):
                        OnChangedMediaFile(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedAlignment(object oldValue, object newValue)
        {
            #region 1561ImportCode
            AlignmentNew = Alignment;            
            #endregion 1561ImportCode
        }               
        private void OnChangedTextWrappingType(object oldValue, object newValue)
        {
            #region 1562ImportCode
            TextWrappingTypeNew = TextWrappingType;            
            #endregion 1562ImportCode
        }               
        private void OnChangedMediaFile(object oldValue, object newValue)
        {
            #region 0913ImportCode
            if (newValue is null) return;
var fileName = MediaFile.ToLower();
                        if (fileName.EndsWith(".png") || fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg") || fileName.EndsWith(".bmp"))
                            this.Photo = true;            
            #endregion 0913ImportCode
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
			//	SetDefaultAudioList();
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
		 		 
    }
}
