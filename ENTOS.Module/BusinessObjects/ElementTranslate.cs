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
    [ModelDefault("Caption", "Dịch ngữ"), ImageName("ElementTranslate")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Update))]
 
	[DefaultProperty("Content")]
 
	[RuleCombinationOfPropertiesIsUnique("UniqueRule.ElementTranslate", DefaultContexts.Save, "Audio, Language")]
[OptimisticLocking(true)]
    public partial class ElementTranslate:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ElementTranslate(Session session)
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

	
       
		//private Module.BusinessObjects.Voice _voice;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giọng đọc")]
        [ToolTip("Giọng đọc")]
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
	
       
		//private string _content;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(3)]		

 		[Size(2000)]
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

	
       
		//private decimal? _voicespeed;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tốc độ")]
        [ToolTip("Tốc độ")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? VoiceSpeed
        { 
		    get => GetPropertyValue<decimal?>("VoiceSpeed");                         
			set => SetPropertyValue<decimal?>("VoiceSpeed", value); 
			
        }
		//Tooltip for Object
		public object VoiceSpeedToolTipControllerText(View view)
        {
        //    if (VoiceSpeed != null) 
		//			return VoiceSpeed;
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

	
       
		//private string _audiolink;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tệp âm")]
        [ToolTip("Tệp âm")]
		//[Index(5)]		

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
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thời lượng âm")]
        [ToolTip("Thời lượng âm")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
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
        public decimal? GetDefaultAudioDuration(View view = null)
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

	
       
		//private string _spelling;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Phiên âm")]
        [ToolTip("Phiên âm")]
		//[Index(7)]		

 		[Size(2000)]
		public string Spelling
        { 
		    get => GetPropertyValue<string>("Spelling");                         
			set => SetPropertyValue<string>("Spelling", value); 
			
        }
		//Tooltip for Object
		public object SpellingToolTipControllerText(View view)
        {
        //    if (Spelling != null) 
		//			return Spelling;
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
		[DevExpress.Xpo.Association("Audio-ElementTranslateList")]
	 
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
	
       
		//private decimal? _audiorate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Suất âm")]
        [ToolTip("Suất âm")]
		//[Index(9)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p2")]
		public decimal? AudioRate
        { 
		    #region 3333ImportCode 
get
            {
                //Thời lượng  âm / Tốc độ /  Thời lượng * 100%
                if(End != null && Start != null && (Convert.ToDecimal((End.Value - Start.Value).TotalSeconds)) != (decimal)0 && AudioDuration != (decimal)0 && VoiceSpeed != null && VoiceSpeed != (decimal)0)
                {
                    return AudioDuration / VoiceSpeed.Value / (Convert.ToDecimal((End.Value - Start.Value).TotalSeconds));
                }
                return null;
            }
#endregion 3333ImportCode
			
        }
		//Tooltip for Object
		public object AudioRateToolTipControllerText(View view)
        {
        //    if (AudioRate != null) 
		//			return AudioRate;
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

	
       
		//private Module.BusinessObjects.Language _language;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngôn ngữ")]
        [ToolTip("Ngôn ngữ")]
		//[Index(10)]		
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
	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(11)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
	    [ModelDefault("AllowEdit", "False")]
		public DateTime? Update
        { 
		    get => GetPropertyValue<DateTime?>("Update");                         
			set => SetPropertyValue<DateTime?>("Update", value); 
			
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 3337ImportCode
            base.AfterConstruction();
VoiceSpeed = 1;
            #endregion 3337ImportCode
 
        //SetDefaultStart(View view = null);
        //SetDefaultEnd(View view = null);
        //SetDefaultVoice(View view = null);
        //SetDefaultContent(View view = null);
        //SetDefaultVoiceSpeed(View view = null);
        //SetDefaultAudioLink(View view = null);
        //SetDefaultAudioDuration(View view = null);
        //SetDefaultSpelling(View view = null);
        //SetDefaultAudio(View view = null);
        //SetDefaultAudioRate(View view = null);
        //SetDefaultLanguage(View view = null);
        //SetDefaultUpdate(View view = null);
			
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
            #region 3335ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 3335ImportCode
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
#region 3334ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 3334            Oid: d417fa8d-4efe-4fd0-8ed3-7729c40bea94
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 3334ImportCode
#region 3336ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 3336            Oid: 4f8d5b62-7900-43ca-a569-cc288f68ba8e
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 3336ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
