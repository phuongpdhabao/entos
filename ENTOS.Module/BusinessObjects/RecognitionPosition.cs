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
    [ModelDefault("Caption", "Vị trí nhận dạng"), ImageName("RecognitionPosition")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "RecognitionObject_RecognitionPositionList_ListView", TargetItems = nameof(Link))]
	[MobileColumnAttribute(Context = "RecognitionPosition_LookupListView", TargetItems = nameof(Begin))]
	[MobileColumnAttribute(Context = "RecognitionPosition_ListView", TargetItems = nameof(Link))]
	[DefaultProperty("Link")]
 
[OptimisticLocking(true)]
    public partial class RecognitionPosition:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public RecognitionPosition(Session session)
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
               

		//private string _link;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
        [ToolTip("Liên kết")]
		//[Index(0)]		

 		[Size(1000)]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string Link
        { 
		    get => GetPropertyValue<string>("Link");                         
			set => SetPropertyValue<string>("Link", value); 
			
        }
		//Tooltip for Object
		public object LinkToolTipControllerText(View view)
        {
        //    if (Link != null) 
		//			return Link;
            return null;
        }
		//Get Default Value
        public string GetDefaultLink(View view = null)
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

	
       
		//private int? _vertical;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vị trí dọc")]
        [ToolTip("Vị trí dọc")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Vertical
        { 
		    get => GetPropertyValue<int?>("Vertical");                         
			set => SetPropertyValue<int?>("Vertical", value); 
			
        }
		//Tooltip for Object
		public object VerticalToolTipControllerText(View view)
        {
        //    if (Vertical != null) 
		//			return Vertical;
            return null;
        }
		//Get Default Value
        public int? GetDefaultVertical(View view = null)
        { 
			return Vertical;
        }
		//Set Default Value
		public void SetDefaultVertical(View view = null)
        {
            //if (Vertical is null){
            //    var result = GetDefaultVertical(view);
            //    if (result != null && result != Vertical){
			//          Vertical = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool VerticalIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultVertical();
				//if (result != null && Vertical != null){
				//	return !Vertical.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _horizontal;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vị trí ngang")]
        [ToolTip("Vị trí ngang")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Horizontal
        { 
		    get => GetPropertyValue<int?>("Horizontal");                         
			set => SetPropertyValue<int?>("Horizontal", value); 
			
        }
		//Tooltip for Object
		public object HorizontalToolTipControllerText(View view)
        {
        //    if (Horizontal != null) 
		//			return Horizontal;
            return null;
        }
		//Get Default Value
        public int? GetDefaultHorizontal(View view = null)
        { 
			return Horizontal;
        }
		//Set Default Value
		public void SetDefaultHorizontal(View view = null)
        {
            //if (Horizontal is null){
            //    var result = GetDefaultHorizontal(view);
            //    if (result != null && result != Horizontal){
			//          Horizontal = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool HorizontalIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultHorizontal();
				//if (result != null && Horizontal != null){
				//	return !Horizontal.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _reliability;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Độ tin cậy")]
        [ToolTip("Độ tin cậy")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p2")]
		public decimal? Reliability
        { 
		    get => GetPropertyValue<decimal?>("Reliability");                         
			set => SetPropertyValue<decimal?>("Reliability", value); 
			
        }
		//Tooltip for Object
		public object ReliabilityToolTipControllerText(View view)
        {
        //    if (Reliability != null) 
		//			return Reliability;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultReliability(View view = null)
        { 
			return Reliability;
        }
		//Set Default Value
		public void SetDefaultReliability(View view = null)
        {
            //if (Reliability is null){
            //    var result = GetDefaultReliability(view);
            //    if (result != null && result != Reliability){
			//          Reliability = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ReliabilityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultReliability();
				//if (result != null && Reliability != null){
				//	return !Reliability.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _size;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kích thước")]
        [ToolTip("Kích thước")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Size
        { 
		    get => GetPropertyValue<int?>("Size");                         
			set => SetPropertyValue<int?>("Size", value); 
			
        }
		//Tooltip for Object
		public object SizeToolTipControllerText(View view)
        {
        //    if (Size != null) 
		//			return Size;
            return null;
        }
		//Get Default Value
        public int? GetDefaultSize(View view = null)
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

	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ảnh")]
        [ToolTip("Ảnh")]
		//[Index(5)]		
		[Appearance("ẢnhBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Image
        { 
		    get => GetPropertyValue<byte[]>("Image");                         
			set => SetPropertyValue<byte[]>("Image", value); 
			
        }
		//Tooltip for Object
		public object ImageToolTipControllerText(View view)
        {
        //    if (Image != null) 
		//			return Image;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultImage(View view = null)
        { 
			return Image;
        }
		//Set Default Value
		public void SetDefaultImage(View view = null)
        {
            //if (Image is null){
            //    var result = GetDefaultImage(view);
            //    if (result != null && result != Image){
			//          Image = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ImageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImage();
				//if (result != null && Image != null){
				//	return !Image.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private TimeSpan? _begin;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Bắt đầu")]
        [ToolTip("Bắt đầu")]
		//[Index(6)]		
	    [ModelDefault("MaskSettings", "AgAAAA9NYXNrTWFuYWdlclR5cGUAgwFEZXZFeHByZXNzLkRhdGEuTWFzay5UaW1lU3Bhbk1hc2tNYW5hZ2VyLCBEZXZFeHByZXNzLkRhdGEudjIyLjEsIFZlcnNpb249MjIuMS4zLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjg4ZDE3NTRkNzAwZTQ5YQRtYXNrBwIMW2QuXWhoOm1tOnNz")]
		public TimeSpan? Begin
        { 
		    get => GetPropertyValue<TimeSpan?>("Begin");                         
			set => SetPropertyValue<TimeSpan?>("Begin", value); 
			
        }
		//Tooltip for Object
		public object BeginToolTipControllerText(View view)
        {
        //    if (Begin != null) 
		//			return Begin;
            return null;
        }
		//Get Default Value
        public TimeSpan? GetDefaultBegin(View view = null)
        { 
			return Begin;
        }
		//Set Default Value
		public void SetDefaultBegin(View view = null)
        {
            //if (Begin is null){
            //    var result = GetDefaultBegin(view);
            //    if (result != null && result != Begin){
			//          Begin = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BeginIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBegin();
				//if (result != null && Begin != null){
				//	return !Begin.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private TimeSpan? _end;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kết thúc")]
        [ToolTip("Kết thúc")]
		//[Index(7)]		
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

	
       
		//private int? _beginframe;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Khung đầu")]
        [ToolTip("Khung đầu")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? BeginFrame
        { 
		    get => GetPropertyValue<int?>("BeginFrame");                         
			set => SetPropertyValue<int?>("BeginFrame", value); 
			
        }
		//Tooltip for Object
		public object BeginFrameToolTipControllerText(View view)
        {
        //    if (BeginFrame != null) 
		//			return BeginFrame;
            return null;
        }
		//Get Default Value
        public int? GetDefaultBeginFrame(View view = null)
        { 
			return BeginFrame;
        }
		//Set Default Value
		public void SetDefaultBeginFrame(View view = null)
        {
            //if (BeginFrame is null){
            //    var result = GetDefaultBeginFrame(view);
            //    if (result != null && result != BeginFrame){
			//          BeginFrame = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BeginFrameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBeginFrame();
				//if (result != null && BeginFrame != null){
				//	return !BeginFrame.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _endframe;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Khung cuối")]
        [ToolTip("Khung cuối")]
		//[Index(9)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? EndFrame
        { 
		    get => GetPropertyValue<int?>("EndFrame");                         
			set => SetPropertyValue<int?>("EndFrame", value); 
			
        }
		//Tooltip for Object
		public object EndFrameToolTipControllerText(View view)
        {
        //    if (EndFrame != null) 
		//			return EndFrame;
            return null;
        }
		//Get Default Value
        public int? GetDefaultEndFrame(View view = null)
        { 
			return EndFrame;
        }
		//Set Default Value
		public void SetDefaultEndFrame(View view = null)
        {
            //if (EndFrame is null){
            //    var result = GetDefaultEndFrame(view);
            //    if (result != null && result != EndFrame){
			//          EndFrame = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EndFrameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEndFrame();
				//if (result != null && EndFrame != null){
				//	return !EndFrame.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.RecognitionObject _recognitionobject;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhận dạng")]
        [ToolTip("Nhận dạng")]
		//[Index(10)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(RecognitionObjectCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("RecognitionObject-RecognitionPositionList")]
	 
		public Module.BusinessObjects.RecognitionObject RecognitionObject
        { 
		    get => GetPropertyValue<Module.BusinessObjects.RecognitionObject>("RecognitionObject");                         
			set => SetPropertyValue<Module.BusinessObjects.RecognitionObject>("RecognitionObject", value); 
			
        }
		//Tooltip for Object
		public object RecognitionObjectToolTipControllerText(View view)
        {
        //    if (RecognitionObject != null) 
		//			return RecognitionObject;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.RecognitionObject GetDefaultRecognitionObject(View view = null)
        { 
			return RecognitionObject;
        }
		//Set Default Value
		public void SetDefaultRecognitionObject(View view = null)
        {
            //if (RecognitionObject is null){
            //    var result = GetDefaultRecognitionObject(view);
            //    if (result != null && result != RecognitionObject){
			//          RecognitionObject = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RecognitionObjectIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRecognitionObject();
				//if (result != null && RecognitionObject != null){
				//	return !RecognitionObject.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator RecognitionObjectCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(RecognitionObject));
            }
        }
	
       
		//private int? _imageframe;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Khung ảnh")]
        [ToolTip("Khung ảnh")]
		//[Index(11)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? ImageFrame
        { 
		    get => GetPropertyValue<int?>("ImageFrame");                         
			set => SetPropertyValue<int?>("ImageFrame", value); 
			
        }
		//Tooltip for Object
		public object ImageFrameToolTipControllerText(View view)
        {
        //    if (ImageFrame != null) 
		//			return ImageFrame;
            return null;
        }
		//Get Default Value
        public int? GetDefaultImageFrame(View view = null)
        { 
			return ImageFrame;
        }
		//Set Default Value
		public void SetDefaultImageFrame(View view = null)
        {
            //if (ImageFrame is null){
            //    var result = GetDefaultImageFrame(view);
            //    if (result != null && result != ImageFrame){
			//          ImageFrame = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ImageFrameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImageFrame();
				//if (result != null && ImageFrame != null){
				//	return !ImageFrame.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _yaw;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Độ quay")]
        [ToolTip("Độ quay")]
		//[Index(12)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Yaw
        { 
		    get => GetPropertyValue<decimal?>("Yaw");                         
			set => SetPropertyValue<decimal?>("Yaw", value); 
			
        }
		//Tooltip for Object
		public object YawToolTipControllerText(View view)
        {
        //    if (Yaw != null) 
		//			return Yaw;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultYaw(View view = null)
        { 
			return Yaw;
        }
		//Set Default Value
		public void SetDefaultYaw(View view = null)
        {
            //if (Yaw is null){
            //    var result = GetDefaultYaw(view);
            //    if (result != null && result != Yaw){
			//          Yaw = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool YawIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultYaw();
				//if (result != null && Yaw != null){
				//	return !Yaw.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _roll;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Độ nghiêng")]
        [ToolTip("Độ nghiêng")]
		//[Index(13)]		
		[ModelDefault("DisplayFormat", "{0:n2}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Roll
        { 
		    get => GetPropertyValue<decimal?>("Roll");                         
			set => SetPropertyValue<decimal?>("Roll", value); 
			
        }
		//Tooltip for Object
		public object RollToolTipControllerText(View view)
        {
        //    if (Roll != null) 
		//			return Roll;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultRoll(View view = null)
        { 
			return Roll;
        }
		//Set Default Value
		public void SetDefaultRoll(View view = null)
        {
            //if (Roll is null){
            //    var result = GetDefaultRoll(view);
            //    if (result != null && result != Roll){
			//          Roll = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RollIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRoll();
				//if (result != null && Roll != null){
				//	return !Roll.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultLink(View view = null);
        //SetDefaultVertical(View view = null);
        //SetDefaultHorizontal(View view = null);
        //SetDefaultReliability(View view = null);
        //SetDefaultSize(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultBegin(View view = null);
        //SetDefaultEnd(View view = null);
        //SetDefaultBeginFrame(View view = null);
        //SetDefaultEndFrame(View view = null);
        //SetDefaultRecognitionObject(View view = null);
        //SetDefaultImageFrame(View view = null);
        //SetDefaultYaw(View view = null);
        //SetDefaultRoll(View view = null);
			
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
		 		 
    }
}
