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
	[NavigationItem("ProductBusiness")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Bảo hành"), ImageName("RMA")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "RMA_ListView", TargetItems = nameof(RmaNumber)+ "," + nameof(Problem)+ "," + nameof(RmaCode))]
	[MobileColumnAttribute(Context = "RMA_LookupListView", TargetItems = nameof(RmaCode))]
	[DefaultProperty("RmaCode")]
 
[OptimisticLocking(true)]
    public partial class RMA:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public RMA(Session session)
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
               

		//private string _rmacode;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(0)]		
		[ModelDefault("DisplayFormat", "n0")]
		[ModelDefault("EditMask", "n0")]

 		[Size(10)]
		public string RmaCode
        { 
		    get => GetPropertyValue<string>("RmaCode");                         
			set => SetPropertyValue<string>("RmaCode", value); 
			
        }
		//Tooltip for Object
		public object RmaCodeToolTipControllerText(View view)
        {
        //    if (RmaCode != null) 
		//			return RmaCode;
            return null;
        }
		//Get Default Value
        public string GetDefaultRmaCode(View view = null)
        { 
			return RmaCode;
        }
		//Set Default Value
		public void SetDefaultRmaCode(View view = null)
        {
            //if (RmaCode is null){
            //    var result = GetDefaultRmaCode(view);
            //    if (result != null && result != RmaCode){
			//          RmaCode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RmaCodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRmaCode();
				//if (result != null && RmaCode != null){
				//	return !RmaCode.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.ProductItem _itemwarranty;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Hàng hóa")]
        [ToolTip("Hàng hóa")]
		//[Index(1)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ItemWarrantyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.ProductItem ItemWarranty
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ProductItem>("ItemWarranty");                         
			set => SetPropertyValue<Module.BusinessObjects.ProductItem>("ItemWarranty", value); 
			
        }
		//Tooltip for Object
		public object ItemWarrantyToolTipControllerText(View view)
        {
        //    if (ItemWarranty != null) 
		//			return ItemWarranty;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ProductItem GetDefaultItemWarranty(View view = null)
        { 
			return ItemWarranty;
        }
		//Set Default Value
		public void SetDefaultItemWarranty(View view = null)
        {
            //if (ItemWarranty is null){
            //    var result = GetDefaultItemWarranty(view);
            //    if (result != null && result != ItemWarranty){
			//          ItemWarranty = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ItemWarrantyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultItemWarranty();
				//if (result != null && ItemWarranty != null){
				//	return !ItemWarranty.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ItemWarrantyCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ItemWarranty));
            }
        }
	
       
		//private string _customer;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Khách hàng")]
        [ToolTip("Khách hàng")]
		//[Index(2)]		

 		[Size(200)]
		public string Customer
        { 
		    get => GetPropertyValue<string>("Customer");                         
			set => SetPropertyValue<string>("Customer", value); 
			
        }
		//Tooltip for Object
		public object CustomerToolTipControllerText(View view)
        {
        //    if (Customer != null) 
		//			return Customer;
            return null;
        }
		//Get Default Value
        public string GetDefaultCustomer(View view = null)
        { 
			return Customer;
        }
		//Set Default Value
		public void SetDefaultCustomer(View view = null)
        {
            //if (Customer is null){
            //    var result = GetDefaultCustomer(view);
            //    if (result != null && result != Customer){
			//          Customer = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CustomerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCustomer();
				//if (result != null && Customer != null){
				//	return !Customer.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _productname;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Sản phẩm")]
        [ToolTip("Sản phẩm")]
		//[Index(3)]		

 		[Size(200)]
		public string ProductName
        { 
		    get => GetPropertyValue<string>("ProductName");                         
			set => SetPropertyValue<string>("ProductName", value); 
			
        }
		//Tooltip for Object
		public object ProductNameToolTipControllerText(View view)
        {
        //    if (ProductName != null) 
		//			return ProductName;
            return null;
        }
		//Get Default Value
        public string GetDefaultProductName(View view = null)
        { 
			return ProductName;
        }
		//Set Default Value
		public void SetDefaultProductName(View view = null)
        {
            //if (ProductName is null){
            //    var result = GetDefaultProductName(view);
            //    if (result != null && result != ProductName){
			//          ProductName = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ProductNameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultProductName();
				//if (result != null && ProductName != null){
				//	return !ProductName.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _serialnumber;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số serial")]
        [ToolTip("Số serial")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "n0")]
		[ModelDefault("EditMask", "n0")]

 		[Size(20)]
		public string SerialNumber
        { 
		    get => GetPropertyValue<string>("SerialNumber");                         
			set => SetPropertyValue<string>("SerialNumber", value); 
			
        }
		//Tooltip for Object
		public object SerialNumberToolTipControllerText(View view)
        {
        //    if (SerialNumber != null) 
		//			return SerialNumber;
            return null;
        }
		//Get Default Value
        public string GetDefaultSerialNumber(View view = null)
        { 
			return SerialNumber;
        }
		//Set Default Value
		public void SetDefaultSerialNumber(View view = null)
        {
            //if (SerialNumber is null){
            //    var result = GetDefaultSerialNumber(view);
            //    if (result != null && result != SerialNumber){
			//          SerialNumber = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SerialNumberIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSerialNumber();
				//if (result != null && SerialNumber != null){
				//	return !SerialNumber.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _problem;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Báo lỗi")]
        [ToolTip("Báo lỗi")]
		//[Index(5)]		

 		[Size(200)]
		public string Problem
        { 
		    get => GetPropertyValue<string>("Problem");                         
			set => SetPropertyValue<string>("Problem", value); 
			
        }
		//Tooltip for Object
		public object ProblemToolTipControllerText(View view)
        {
        //    if (Problem != null) 
		//			return Problem;
            return null;
        }
		//Get Default Value
        public string GetDefaultProblem(View view = null)
        { 
			return Problem;
        }
		//Set Default Value
		public void SetDefaultProblem(View view = null)
        {
            //if (Problem is null){
            //    var result = GetDefaultProblem(view);
            //    if (result != null && result != Problem){
			//          Problem = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ProblemIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultProblem();
				//if (result != null && Problem != null){
				//	return !Problem.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _action;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Xử lý")]
        [ToolTip("Xử lý")]
		//[Index(6)]		

 		[Size(200)]
		public string Action
        { 
		    get => GetPropertyValue<string>("Action");                         
			set => SetPropertyValue<string>("Action", value); 
			
        }
		//Tooltip for Object
		public object ActionToolTipControllerText(View view)
        {
        //    if (Action != null) 
		//			return Action;
            return null;
        }
		//Get Default Value
        public string GetDefaultAction(View view = null)
        { 
			return Action;
        }
		//Set Default Value
		public void SetDefaultAction(View view = null)
        {
            //if (Action is null){
            //    var result = GetDefaultAction(view);
            //    if (result != null && result != Action){
			//          Action = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ActionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAction();
				//if (result != null && Action != null){
				//	return !Action.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _datereceived;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày nhận")]
        [ToolTip("Ngày nhận")]
		//[Index(7)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? DateReceived
        { 
		    get => GetPropertyValue<DateTime?>("DateReceived");                         
			set => SetPropertyValue<DateTime?>("DateReceived", value); 
			
        }
		//Tooltip for Object
		public object DateReceivedToolTipControllerText(View view)
        {
        //    if (DateReceived != null) 
		//			return DateReceived;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultDateReceived(View view = null)
        { 
			return DateReceived;
        }
		//Set Default Value
		public void SetDefaultDateReceived(View view = null)
        {
            //if (DateReceived is null){
            //    var result = GetDefaultDateReceived(view);
            //    if (result != null && result != DateReceived){
			//          DateReceived = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DateReceivedIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDateReceived();
				//if (result != null && DateReceived != null){
				//	return !DateReceived.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _datesend;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày gửi")]
        [ToolTip("Ngày gửi")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? DateSend
        { 
		    get => GetPropertyValue<DateTime?>("DateSend");                         
			set => SetPropertyValue<DateTime?>("DateSend", value); 
			
        }
		//Tooltip for Object
		public object DateSendToolTipControllerText(View view)
        {
        //    if (DateSend != null) 
		//			return DateSend;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultDateSend(View view = null)
        { 
			return DateSend;
        }
		//Set Default Value
		public void SetDefaultDateSend(View view = null)
        {
            //if (DateSend is null){
            //    var result = GetDefaultDateSend(view);
            //    if (result != null && result != DateSend){
			//          DateSend = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DateSendIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDateSend();
				//if (result != null && DateSend != null){
				//	return !DateSend.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _datereturn;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày trả")]
        [ToolTip("Ngày trả")]
		//[Index(9)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? DateReturn
        { 
		    get => GetPropertyValue<DateTime?>("DateReturn");                         
			set => SetPropertyValue<DateTime?>("DateReturn", value); 
			
        }
		//Tooltip for Object
		public object DateReturnToolTipControllerText(View view)
        {
        //    if (DateReturn != null) 
		//			return DateReturn;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultDateReturn(View view = null)
        { 
			return DateReturn;
        }
		//Set Default Value
		public void SetDefaultDateReturn(View view = null)
        {
            //if (DateReturn is null){
            //    var result = GetDefaultDateReturn(view);
            //    if (result != null && result != DateReturn){
			//          DateReturn = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DateReturnIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDateReturn();
				//if (result != null && DateReturn != null){
				//	return !DateReturn.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhân viên")]
        [ToolTip("Nhân viên")]
		//[Index(10)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MemberCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Member Member
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Member");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Member", value); 
			
        }
		//Tooltip for Object
		public object MemberToolTipControllerText(View view)
        {
        //    if (Member != null) 
		//			return Member;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        { 
			return Member;
        }
		//Set Default Value
		public void SetDefaultMember(View view = null)
        {
            //if (Member is null){
            //    var result = GetDefaultMember(view);
            //    if (result != null && result != Member){
			//          Member = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MemberIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMember();
				//if (result != null && Member != null){
				//	return !Member.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator MemberCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Member));
            }
        }
	
       
		//private string _rmanumber;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số RMA")]
        [ToolTip("Số RMA")]
		//[Index(11)]		
		[ModelDefault("DisplayFormat", "n0")]
		[ModelDefault("EditMask", "n0")]

 		[Size(100)]
		public string RmaNumber
        { 
		    get => GetPropertyValue<string>("RmaNumber");                         
			set => SetPropertyValue<string>("RmaNumber", value); 
			
        }
		//Tooltip for Object
		public object RmaNumberToolTipControllerText(View view)
        {
        //    if (RmaNumber != null) 
		//			return RmaNumber;
            return null;
        }
		//Get Default Value
        public string GetDefaultRmaNumber(View view = null)
        { 
			return RmaNumber;
        }
		//Set Default Value
		public void SetDefaultRmaNumber(View view = null)
        {
            //if (RmaNumber is null){
            //    var result = GetDefaultRmaNumber(view);
            //    if (result != null && result != RmaNumber){
			//          RmaNumber = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RmaNumberIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRmaNumber();
				//if (result != null && RmaNumber != null){
				//	return !RmaNumber.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _itemreplace;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Hàng đổi")]
        [ToolTip("Hàng đổi")]
		//[Index(12)]		

 		[Size(20)]
		public string ItemReplace
        { 
		    get => GetPropertyValue<string>("ItemReplace");                         
			set => SetPropertyValue<string>("ItemReplace", value); 
			
        }
		//Tooltip for Object
		public object ItemReplaceToolTipControllerText(View view)
        {
        //    if (ItemReplace != null) 
		//			return ItemReplace;
            return null;
        }
		//Get Default Value
        public string GetDefaultItemReplace(View view = null)
        { 
			return ItemReplace;
        }
		//Set Default Value
		public void SetDefaultItemReplace(View view = null)
        {
            //if (ItemReplace is null){
            //    var result = GetDefaultItemReplace(view);
            //    if (result != null && result != ItemReplace){
			//          ItemReplace = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ItemReplaceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultItemReplace();
				//if (result != null && ItemReplace != null){
				//	return !ItemReplace.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _note;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(13)]		

 		[Size(200)]
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultRmaCode(View view = null);
        //SetDefaultItemWarranty(View view = null);
        //SetDefaultCustomer(View view = null);
        //SetDefaultProductName(View view = null);
        //SetDefaultSerialNumber(View view = null);
        //SetDefaultProblem(View view = null);
        //SetDefaultAction(View view = null);
        //SetDefaultDateReceived(View view = null);
        //SetDefaultDateSend(View view = null);
        //SetDefaultDateReturn(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultRmaNumber(View view = null);
        //SetDefaultItemReplace(View view = null);
        //SetDefaultNote(View view = null);
			
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
