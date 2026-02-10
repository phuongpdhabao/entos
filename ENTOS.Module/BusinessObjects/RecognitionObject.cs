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
    [ModelDefault("Caption", "Đối tượng nhận dạng"), ImageName("RecognitionObject")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "RecognitionObject_LookupListView", TargetItems = nameof(Image)+ "," + nameof(Order))]
	[MobileColumnAttribute(Context = "Recognition_RecognitionObjectList_ListView", TargetItems = nameof(Order)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "RecognitionObject_ListView", TargetItems = nameof(Image)+ "," + nameof(Order))]
	[DefaultProperty("Order")]
 
[OptimisticLocking(true)]
    public partial class RecognitionObject:  DevExpress.Xpo.XPLiteObject , IObjectImage , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public RecognitionObject(Session session)
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

	
       
		//private RecognitionType _recognitiontype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(1)]		
		public RecognitionType RecognitionType
        { 
		    get => GetPropertyValue<RecognitionType>("RecognitionType");                         
			set => SetPropertyValue<RecognitionType>("RecognitionType", value); 
			
        }
		//Tooltip for Object
		public object RecognitionTypeToolTipControllerText(View view)
        {
        //    if (RecognitionType != null) 
		//			return RecognitionType;
            return null;
        }
		//Get Default Value
        public RecognitionType GetDefaultRecognitionType(View view = null)
        { 
			return RecognitionType;
        }
		//Set Default Value
		public void SetDefaultRecognitionType(View view = null)
        {
            //if (RecognitionType is null){
            //    var result = GetDefaultRecognitionType(view);
            //    if (result != null && result != RecognitionType){
			//          RecognitionType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RecognitionTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRecognitionType();
				//if (result != null && RecognitionType != null){
				//	return !RecognitionType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Ảnh")]
        [ToolTip("Ảnh")]
		//[Index(2)]		
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

	
       
		//private decimal? _reliability;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
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
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
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

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vị trí")]
		//[Index(5)]
		[DevExpress.Xpo.Association("RecognitionObject-RecognitionPositionList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.RecognitionPosition> RecognitionPositionList
        {      
		    get => GetCollection<Module.BusinessObjects.RecognitionPosition>("RecognitionPositionList"); 
			
        }
       
		//private Module.BusinessObjects.Recognition _recognition;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhận dạng")]
        [ToolTip("Nhận dạng")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(RecognitionCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Recognition-RecognitionObjectList")]
	 
		public Module.BusinessObjects.Recognition Recognition
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Recognition>("Recognition");                         
			set => SetPropertyValue<Module.BusinessObjects.Recognition>("Recognition", value); 
			
        }
		//Tooltip for Object
		public object RecognitionToolTipControllerText(View view)
        {
        //    if (Recognition != null) 
		//			return Recognition;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Recognition GetDefaultRecognition(View view = null)
        { 
			return Recognition;
        }
		//Set Default Value
		public void SetDefaultRecognition(View view = null)
        {
            //if (Recognition is null){
            //    var result = GetDefaultRecognition(view);
            //    if (result != null && result != Recognition){
			//          Recognition = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RecognitionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRecognition();
				//if (result != null && Recognition != null){
				//	return !Recognition.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator RecognitionCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Recognition));
            }
        }
	
       
		//private bool _flag;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ")]
        [ToolTip("Cờ")]
		//[Index(7)]		
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

	
       
		//private int? _quantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Quantity
        { 
		    #region 2507ImportCode 
get => RecognitionPositionList.Count();
#endregion 2507ImportCode
			
        }
		//Tooltip for Object
		public object QuantityToolTipControllerText(View view)
        {
        //    if (Quantity != null) 
		//			return Quantity;
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

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(9)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		[RuleUniqueValue("UniqueRecognitionObjectOrder", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredRecognitionObjectOrder", DefaultContexts.Save)]
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

	
       
		//private int? _frame;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Khung")]
        [ToolTip("Khung")]
		//[Index(10)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Frame
        { 
		    get => GetPropertyValue<int?>("Frame");                         
			set => SetPropertyValue<int?>("Frame", value); 
			
        }
		//Tooltip for Object
		public object FrameToolTipControllerText(View view)
        {
        //    if (Frame != null) 
		//			return Frame;
            return null;
        }
		//Get Default Value
        public int? GetDefaultFrame(View view = null)
        { 
			return Frame;
        }
		//Set Default Value
		public void SetDefaultFrame(View view = null)
        {
            //if (Frame is null){
            //    var result = GetDefaultFrame(view);
            //    if (result != null && result != Frame){
			//          Frame = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FrameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFrame();
				//if (result != null && Frame != null){
				//	return !Frame.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.RecognitionPosition _recognitionposition;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vị trí ảnh")]
        [ToolTip("Vị trí ảnh")]
		//[Index(11)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(RecognitionPositionCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.RecognitionPosition RecognitionPosition
        { 
		    get => GetPropertyValue<Module.BusinessObjects.RecognitionPosition>("RecognitionPosition");                         
			set => SetPropertyValue<Module.BusinessObjects.RecognitionPosition>("RecognitionPosition", value); 
			
        }
		//Tooltip for Object
		public object RecognitionPositionToolTipControllerText(View view)
        {
        //    if (RecognitionPosition != null) 
		//			return RecognitionPosition;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.RecognitionPosition GetDefaultRecognitionPosition(View view = null)
        { 
			return RecognitionPosition;
        }
		//Set Default Value
		public void SetDefaultRecognitionPosition(View view = null)
        {
            //if (RecognitionPosition is null){
            //    var result = GetDefaultRecognitionPosition(view);
            //    if (result != null && result != RecognitionPosition){
			//          RecognitionPosition = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RecognitionPositionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRecognitionPosition();
				//if (result != null && RecognitionPosition != null){
				//	return !RecognitionPosition.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator RecognitionPositionCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(RecognitionPosition));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultRecognitionType(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultReliability(View view = null);
        //SetDefaultSize(View view = null);
        //SetDefaultRecognition(View view = null);
        //SetDefaultFlag(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultFrame(View view = null);
        //SetDefaultRecognitionPosition(View view = null);
			
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
            Session.Delete(this.RecognitionPositionList);				
  
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
			//	SetDefaultRecognitionPositionList();
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
