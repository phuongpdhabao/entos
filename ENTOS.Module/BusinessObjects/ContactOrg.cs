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
    [ModelDefault("Caption", "Liên hệ Tổ chức"), ImageName("ContactOrg")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("ContactOrg BusinessRole IsNotValidate" , TargetItems = "BusinessRole" , Criteria = "BusinessRoleIsNotValidate",AppearanceItemType = "ViewItem", FontColor = "#FF0000" )]
	[Appearance("ContactOrg FromtDate IsNotValidate" , TargetItems = "FromtDate" , Criteria = "FromtDateIsNotValidate",AppearanceItemType = "ViewItem", FontColor = "#FF0000" )]
	[Appearance("ContactOrg Concurence IsNotValidate" , TargetItems = "Concurence" , Criteria = "ConcurenceIsNotValidate",AppearanceItemType = "ViewItem", Context = "DetailView" , FontColor = "#FF0000" )]
	[Appearance("ContactOrg ToDate IsNotValidate" , TargetItems = "ToDate" , Criteria = "ToDateIsNotValidate",AppearanceItemType = "ViewItem", FontColor = "#FF0000" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "ContactOrg_ListView", TargetItems = nameof(ToDate)+ "," + nameof(FromtDate))]
	[MobileColumnAttribute(Context = "ContactOrg_LookupListView", TargetItems = nameof(Concurence))]
	[DefaultProperty("Contact")]
 
//[OptimisticLocking(false)]
    public partial class ContactOrg: DevExpress.Persistent.BaseImpl.BaseObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public ContactOrg(Session session)
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

               

		//private Module.BusinessObjects.OrgDivision _division;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bộ phận")]
        [ToolTip("Bộ phận")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DivisionCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.OrgDivision Division
        { 
		    get => GetPropertyValue<Module.BusinessObjects.OrgDivision>("Division");                         
			set => SetPropertyValue<Module.BusinessObjects.OrgDivision>("Division", value); 
			
        }
		//Tooltip for Object
		public object DivisionToolTipControllerText(View view)
        {
        //    if (Division != null) 
		//			return Division;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.OrgDivision GetDefaultDivision(View view = null)
        { 
			return Division;
        }
		//Set Default Value
		public void SetDefaultDivision(View view = null)
        {
            //if (Division is null){
            //    var result = GetDefaultDivision(view);
            //    if (result != null && result != Division){
			//          Division = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DivisionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDivision();
				//if (result != null && Division != null){
				//	return !Division.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator DivisionCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Division));
            }
        }
	
       
		//private string _businessrole;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vai trò")]
        [ToolTip("Vai trò")]
		//[Index(1)]		

 		[Size(20)]
		public string BusinessRole
        { 
		    get => GetPropertyValue<string>("BusinessRole");                         
			set => SetPropertyValue<string>("BusinessRole", value); 
			
        }
		//Tooltip for Object
		public object BusinessRoleToolTipControllerText(View view)
        {
        //    if (BusinessRole != null) 
		//			return BusinessRole;
            return null;
        }
		//Get Default Value
        public string GetDefaultBusinessRole(View view = null)
        { 
			return BusinessRole;
        }
		//Set Default Value
		public void SetDefaultBusinessRole(View view = null)
        {
            //if (BusinessRole is null){
            //    var result = GetDefaultBusinessRole(view);
            //    if (result != null && result != BusinessRole){
			//          BusinessRole = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BusinessRoleIsNotValidate
        {
            get
            {
			#region 0306ImportCode 
if(!string.IsNullOrEmpty(BusinessRole)) { var result = GetDefaultBusinessRole(); if(!string.IsNullOrEmpty(result)) return !result.Equals(BusinessRole); }return false;
#endregion 0306ImportCode                
   
                return false;
            }
        }

	
       
		//private DateTime? _fromtdate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Từ ngày")]
        [ToolTip("Từ ngày")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? FromtDate
        { 
		    get => GetPropertyValue<DateTime?>("FromtDate");                         
			set => SetPropertyValue<DateTime?>("FromtDate", value); 
			
        }
		//Tooltip for Object
		public object FromtDateToolTipControllerText(View view)
        {
        //    if (FromtDate != null) 
		//			return FromtDate;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultFromtDate(View view = null)
        { 
			return FromtDate;
        }
		//Set Default Value
		public void SetDefaultFromtDate(View view = null)
        {
            //if (FromtDate is null){
            //    var result = GetDefaultFromtDate(view);
            //    if (result != null && result != FromtDate){
			//          FromtDate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FromtDateIsNotValidate
        {
            get
            {
			#region 0296ImportCode 
var result = GetDefaultFromtDate(); if(result != null) return !result.Equals(FromtDate); return false;
#endregion 0296ImportCode                
   
                return false;
            }
        }

	
       
		//private DateTime? _todate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đến ngày")]
        [ToolTip("Đến ngày")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? ToDate
        { 
		    get => GetPropertyValue<DateTime?>("ToDate");                         
			set => SetPropertyValue<DateTime?>("ToDate", value); 
			
        }
		//Tooltip for Object
		public object ToDateToolTipControllerText(View view)
        {
        //    if (ToDate != null) 
		//			return ToDate;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultToDate(View view = null)
        { 
			return ToDate;
        }
		//Set Default Value
		public void SetDefaultToDate(View view = null)
        {
            //if (ToDate is null){
            //    var result = GetDefaultToDate(view);
            //    if (result != null && result != ToDate){
			//          ToDate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ToDateIsNotValidate
        {
            get
            {
			#region 0293ImportCode 
if(ToDate != null) { var result = GetDefaultToDate(); if(result != null) return !result.Equals(ToDate); }return false;
#endregion 0293ImportCode                
   
                return false;
            }
        }

	
       
		//private bool? _concurence;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Kiêm nhiệm")]
        [ToolTip("Kiêm nhiệm")]
		//[Index(4)]		
		public bool? Concurence
        { 
		    get => GetPropertyValue<bool?>("Concurence");                         
			set => SetPropertyValue<bool?>("Concurence", value); 
			
        }
		//Tooltip for Object
		public object ConcurenceToolTipControllerText(View view)
        {
        //    if (Concurence != null) 
		//			return Concurence;
            return null;
        }
		//Get Default Value
        public bool? GetDefaultConcurence(View view = null)
        { 
			return Concurence;
        }
		//Set Default Value
		public void SetDefaultConcurence(View view = null)
        {
            //if (Concurence is null){
            //    var result = GetDefaultConcurence(view);
            //    if (result != null && result != Concurence){
			//          Concurence = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ConcurenceIsNotValidate
        {
            get
            {
			#region 0267ImportCode 
if(Concurence != null) { var result = GetDefaultConcurence(); if(result != null) return !result.Equals(Concurence); }return false;
#endregion 0267ImportCode                
   
                return false;
            }
        }

	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(5)]		
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

	
       
 


		public override void AfterConstruction()
        {
 
            base.AfterConstruction();
 
        //SetDefaultDivision(View view = null);
        //SetDefaultBusinessRole(View view = null);
        //SetDefaultFromtDate(View view = null);
        //SetDefaultToDate(View view = null);
        //SetDefaultConcurence(View view = null);
        //SetDefaultOrder(View view = null);
			
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
