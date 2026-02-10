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
    [ModelDefault("Caption", "Can Chi"), ImageName("CanChi")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "CanChi_LookupListView", TargetItems = nameof(Name)+ "," + nameof(FaceIcon))]
	[MobileColumnAttribute(Context = "CanChi_ListView", TargetItems = nameof(Name)+ "," + nameof(FaceIcon)+ "," + nameof(BodyIcon))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class CanChi:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public CanChi(Session session)
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
               

		//private ThienCan _thiencan;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thiên Can")]
        [ToolTip("Thiên Can")]
		//[Index(0)]		
		public ThienCan ThienCan
        { 
		    get => GetPropertyValue<ThienCan>("ThienCan");                         
			set => SetPropertyValue<ThienCan>("ThienCan", value); 
			
        }
		//Tooltip for Object
		public object ThienCanToolTipControllerText(View view)
        {
        //    if (ThienCan != null) 
		//			return ThienCan;
            return null;
        }
		//Get Default Value
        public ThienCan GetDefaultThienCan(View view = null)
        { 
			return ThienCan;
        }
		//Set Default Value
		public void SetDefaultThienCan(View view = null)
        {
            //if (ThienCan is null){
            //    var result = GetDefaultThienCan(view);
            //    if (result != null && result != ThienCan){
			//          ThienCan = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ThienCanIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultThienCan();
				//if (result != null && ThienCan != null){
				//	return !ThienCan.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DiaChi _diachi;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Địa Chi")]
        [ToolTip("Địa Chi")]
		//[Index(1)]		
		public DiaChi DiaChi
        { 
		    get => GetPropertyValue<DiaChi>("DiaChi");                         
			set => SetPropertyValue<DiaChi>("DiaChi", value); 
			
        }
		//Tooltip for Object
		public object DiaChiToolTipControllerText(View view)
        {
        //    if (DiaChi != null) 
		//			return DiaChi;
            return null;
        }
		//Get Default Value
        public DiaChi GetDefaultDiaChi(View view = null)
        { 
			return DiaChi;
        }
		//Set Default Value
		public void SetDefaultDiaChi(View view = null)
        {
            //if (DiaChi is null){
            //    var result = GetDefaultDiaChi(view);
            //    if (result != null && result != DiaChi){
			//          DiaChi = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DiaChiIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDiaChi();
				//if (result != null && DiaChi != null){
				//	return !DiaChi.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(2)]		

 		[Size(20)]
		[RuleUniqueValue("UniqueCanChiName", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
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

	
       
		//private byte[] _bodyicon;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Con giáp")]
        [ToolTip("Con giáp")]
		//[Index(3)]		
		[Appearance("Con giápBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] BodyIcon
        { 
		    get => GetPropertyValue<byte[]>("BodyIcon");                         
			set => SetPropertyValue<byte[]>("BodyIcon", value); 
			
        }
		//Tooltip for Object
		public object BodyIconToolTipControllerText(View view)
        {
        //    if (BodyIcon != null) 
		//			return BodyIcon;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultBodyIcon(View view = null)
        { 
			return BodyIcon;
        }
		//Set Default Value
		public void SetDefaultBodyIcon(View view = null)
        {
            //if (BodyIcon is null){
            //    var result = GetDefaultBodyIcon(view);
            //    if (result != null && result != BodyIcon){
			//          BodyIcon = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BodyIconIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBodyIcon();
				//if (result != null && BodyIcon != null){
				//	return !BodyIcon.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private byte[] _faceicon;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mặt con giáp")]
        [ToolTip("Mặt con giáp")]
		//[Index(4)]		
		[Appearance("Mặt con giápBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] FaceIcon
        { 
		    get => GetPropertyValue<byte[]>("FaceIcon");                         
			set => SetPropertyValue<byte[]>("FaceIcon", value); 
			
        }
		//Tooltip for Object
		public object FaceIconToolTipControllerText(View view)
        {
        //    if (FaceIcon != null) 
		//			return FaceIcon;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultFaceIcon(View view = null)
        { 
			return FaceIcon;
        }
		//Set Default Value
		public void SetDefaultFaceIcon(View view = null)
        {
            //if (FaceIcon is null){
            //    var result = GetDefaultFaceIcon(view);
            //    if (result != null && result != FaceIcon){
			//          FaceIcon = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FaceIconIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFaceIcon();
				//if (result != null && FaceIcon != null){
				//	return !FaceIcon.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _oddnumber;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số dư")]
        [ToolTip("Số dư")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		[RuleUniqueValue("UniqueCanChiOddNumber", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		public int? OddNumber
        { 
		    get => GetPropertyValue<int?>("OddNumber");                         
			set => SetPropertyValue<int?>("OddNumber", value); 
			
        }
		//Tooltip for Object
		public object OddNumberToolTipControllerText(View view)
        {
        //    if (OddNumber != null) 
		//			return OddNumber;
            return null;
        }
		//Get Default Value
        public int? GetDefaultOddNumber(View view = null)
        { 
			return OddNumber;
        }
		//Set Default Value
		public void SetDefaultOddNumber(View view = null)
        {
            //if (OddNumber is null){
            //    var result = GetDefaultOddNumber(view);
            //    if (result != null && result != OddNumber){
			//          OddNumber = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OddNumberIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOddNumber();
				//if (result != null && OddNumber != null){
				//	return !OddNumber.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultThienCan(View view = null);
        //SetDefaultDiaChi(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultBodyIcon(View view = null);
        //SetDefaultFaceIcon(View view = null);
        //SetDefaultOddNumber(View view = null);
			
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
