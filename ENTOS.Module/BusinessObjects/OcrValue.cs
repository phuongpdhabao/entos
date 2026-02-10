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
    [ModelDefault("Caption", "Giá trị nhận dạng"), ImageName("OcrValue")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("OcrValue Value None_None__Color [A=255, R=255, G=0, B=0]" , TargetItems = "Value" , Criteria = "[Invalid] = True",AppearanceItemType = "ViewItem", FontColor = "#FF0000" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class OcrValue:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public OcrValue(Session session)
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

 		[Size(250)]
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

	
       
		//private string _value;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giá trị")]
        [ToolTip("Giá trị")]
		//[Index(1)]		

 		[Size(250)]
		public string Value
        { 
		    get => GetPropertyValue<string>("Value");                         
			set => SetPropertyValue<string>("Value", value); 
			
        }
		//Tooltip for Object
		public object ValueToolTipControllerText(View view)
        {
        //    if (Value != null) 
		//			return Value;
            return null;
        }
		//Get Default Value
        public string GetDefaultValue(View view = null)
        { 
			return Value;
        }
		//Set Default Value
		public void SetDefaultValue(View view = null)
        {
            //if (Value is null){
            //    var result = GetDefaultValue(view);
            //    if (result != null && result != Value){
			//          Value = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultValue();
				//if (result != null && Value != null){
				//	return !Value.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.ExtractionKey _extractionkey;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Khóa nhận dạng")]
        [ToolTip("Khóa nhận dạng")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ExtractionKeyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.ExtractionKey ExtractionKey
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ExtractionKey>("ExtractionKey");                         
			set => SetPropertyValue<Module.BusinessObjects.ExtractionKey>("ExtractionKey", value); 
			
        }
		//Tooltip for Object
		public object ExtractionKeyToolTipControllerText(View view)
        {
        //    if (ExtractionKey != null) 
		//			return ExtractionKey;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ExtractionKey GetDefaultExtractionKey(View view = null)
        { 
			return ExtractionKey;
        }
		//Set Default Value
		public void SetDefaultExtractionKey(View view = null)
        {
            //if (ExtractionKey is null){
            //    var result = GetDefaultExtractionKey(view);
            //    if (result != null && result != ExtractionKey){
			//          ExtractionKey = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ExtractionKeyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultExtractionKey();
				//if (result != null && ExtractionKey != null){
				//	return !ExtractionKey.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ExtractionKeyCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ExtractionKey));
            }
        }
	
       
		//private decimal? _x;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("X")]
        [ToolTip("X")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? X
        { 
		    get => GetPropertyValue<decimal?>("X");                         
			set => SetPropertyValue<decimal?>("X", value); 
			
        }
		//Tooltip for Object
		public object XToolTipControllerText(View view)
        {
        //    if (X != null) 
		//			return X;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultX(View view = null)
        { 
			return X;
        }
		//Set Default Value
		public void SetDefaultX(View view = null)
        {
            //if (X is null){
            //    var result = GetDefaultX(view);
            //    if (result != null && result != X){
			//          X = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool XIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultX();
				//if (result != null && X != null){
				//	return !X.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _y;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Y")]
        [ToolTip("Y")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Y
        { 
		    get => GetPropertyValue<decimal?>("Y");                         
			set => SetPropertyValue<decimal?>("Y", value); 
			
        }
		//Tooltip for Object
		public object YToolTipControllerText(View view)
        {
        //    if (Y != null) 
		//			return Y;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultY(View view = null)
        { 
			return Y;
        }
		//Set Default Value
		public void SetDefaultY(View view = null)
        {
            //if (Y is null){
            //    var result = GetDefaultY(view);
            //    if (result != null && result != Y){
			//          Y = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool YIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultY();
				//if (result != null && Y != null){
				//	return !Y.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _width;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Rộng")]
        [ToolTip("Rộng")]
		//[Index(5)]		
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

	
       
		//private decimal? _height;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cao")]
        [ToolTip("Cao")]
		//[Index(6)]		
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

	
       
		//private decimal? _confidence;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Độ tin cậy")]
        [ToolTip("Độ tin cậy")]
		//[Index(7)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p2")]
		public decimal? Confidence
        { 
		    get => GetPropertyValue<decimal?>("Confidence");                         
			set => SetPropertyValue<decimal?>("Confidence", value); 
			
        }
		//Tooltip for Object
		public object ConfidenceToolTipControllerText(View view)
        {
        //    if (Confidence != null) 
		//			return Confidence;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultConfidence(View view = null)
        { 
			return Confidence;
        }
		//Set Default Value
		public void SetDefaultConfidence(View view = null)
        {
            //if (Confidence is null){
            //    var result = GetDefaultConfidence(view);
            //    if (result != null && result != Confidence){
			//          Confidence = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ConfidenceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultConfidence();
				//if (result != null && Confidence != null){
				//	return !Confidence.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.OcrPage _ocrpage;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Trang")]
        [ToolTip("Trang")]
		//[Index(8)]		
		[ModelDefault("EditMask", "n0")]
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OcrPageCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("OcrPage-OcrValue")]
	 
		public Module.BusinessObjects.OcrPage OcrPage
        { 
		    get => GetPropertyValue<Module.BusinessObjects.OcrPage>("OcrPage");                         
			set => SetPropertyValue<Module.BusinessObjects.OcrPage>("OcrPage", value); 
			
        }
		//Tooltip for Object
		public object OcrPageToolTipControllerText(View view)
        {
        //    if (OcrPage != null) 
		//			return OcrPage;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.OcrPage GetDefaultOcrPage(View view = null)
        { 
			return OcrPage;
        }
		//Set Default Value
		public void SetDefaultOcrPage(View view = null)
        {
            //if (OcrPage is null){
            //    var result = GetDefaultOcrPage(view);
            //    if (result != null && result != OcrPage){
			//          OcrPage = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OcrPageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOcrPage();
				//if (result != null && OcrPage != null){
				//	return !OcrPage.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator OcrPageCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(OcrPage));
            }
        }
	
       
		//private Module.BusinessObjects.OcrDocument _ocrdocument;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài liệu nhận dạng")]
        [ToolTip("Tài liệu nhận dạng")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OcrDocumentCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("OcrDocument-OcrValueList")]
	 
		public Module.BusinessObjects.OcrDocument OcrDocument
        { 
		    get => GetPropertyValue<Module.BusinessObjects.OcrDocument>("OcrDocument");                         
			set => SetPropertyValue<Module.BusinessObjects.OcrDocument>("OcrDocument", value); 
			
        }
		//Tooltip for Object
		public object OcrDocumentToolTipControllerText(View view)
        {
        //    if (OcrDocument != null) 
		//			return OcrDocument;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.OcrDocument GetDefaultOcrDocument(View view = null)
        { 
			return OcrDocument;
        }
		//Set Default Value
		public void SetDefaultOcrDocument(View view = null)
        {
            //if (OcrDocument is null){
            //    var result = GetDefaultOcrDocument(view);
            //    if (result != null && result != OcrDocument){
			//          OcrDocument = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OcrDocumentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOcrDocument();
				//if (result != null && OcrDocument != null){
				//	return !OcrDocument.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator OcrDocumentCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(OcrDocument));
            }
        }
	
       
		//private bool _invalid;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Không hợp lệ")]
        [ToolTip("Không hợp lệ")]
		//[Index(10)]		
	    [NonPersistent()]
	    [NotMapped()]
		public bool Invalid
        { 
		    get => GetPropertyValue<bool>("Invalid");                         
			set => SetPropertyValue<bool>("Invalid", value); 
			
        }
		//Tooltip for Object
		public object InvalidToolTipControllerText(View view)
        {
        //    if (Invalid != null) 
		//			return Invalid;
            return null;
        }
		//Get Default Value
        public bool GetDefaultInvalid(View view = null)
        { 
			return Invalid;
        }
		//Set Default Value
		public void SetDefaultInvalid(View view = null)
        {
            //if (Invalid is null){
            //    var result = GetDefaultInvalid(view);
            //    if (result != null && result != Invalid){
			//          Invalid = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool InvalidIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultInvalid();
				//if (result != null && Invalid != null){
				//	return !Invalid.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultValue(View view = null);
        //SetDefaultExtractionKey(View view = null);
        //SetDefaultX(View view = null);
        //SetDefaultY(View view = null);
        //SetDefaultWidth(View view = null);
        //SetDefaultHeight(View view = null);
        //SetDefaultConfidence(View view = null);
        //SetDefaultOcrPage(View view = null);
        //SetDefaultOcrDocument(View view = null);
        //SetDefaultInvalid(View view = null);
			
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
