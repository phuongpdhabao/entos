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
    [ModelDefault("Caption", "Quan hệ tổ chức"), ImageName("OrgRelation")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
 
[OptimisticLocking(true)]
    public partial class OrgRelation:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public OrgRelation(Session session)
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
               

		//private Module.BusinessObjects.Org _upper;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpperCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Org Upper
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Org>("Upper");                         
			set => SetPropertyValue<Module.BusinessObjects.Org>("Upper", value); 
			
        }
		//Tooltip for Object
		public object UpperToolTipControllerText(View view)
        {
        //    if (Upper != null) 
		//			return Upper;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Org GetDefaultUpper(View view = null)
        { 
			return Upper;
        }
		//Set Default Value
		public void SetDefaultUpper(View view = null)
        {
            //if (Upper is null){
            //    var result = GetDefaultUpper(view);
            //    if (result != null && result != Upper){
			//          Upper = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpper();
				//if (result != null && Upper != null){
				//	return !Upper.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpperCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Upper));
            }
        }
	
       
		//private Module.BusinessObjects.Org _lower;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cấp dưới")]
        [ToolTip("Cấp dưới")]
		//[Index(1)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LowerCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Org Lower
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Org>("Lower");                         
			set => SetPropertyValue<Module.BusinessObjects.Org>("Lower", value); 
			
        }
		//Tooltip for Object
		public object LowerToolTipControllerText(View view)
        {
        //    if (Lower != null) 
		//			return Lower;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Org GetDefaultLower(View view = null)
        { 
			return Lower;
        }
		//Set Default Value
		public void SetDefaultLower(View view = null)
        {
            //if (Lower is null){
            //    var result = GetDefaultLower(view);
            //    if (result != null && result != Lower){
			//          Lower = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LowerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLower();
				//if (result != null && Lower != null){
				//	return !Lower.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LowerCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Lower));
            }
        }
	
       
		//private Module.BusinessObjects.RelationType _relationtype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại quan hệ")]
        [ToolTip("Loại quan hệ")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(RelationTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.RelationType RelationType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.RelationType>("RelationType");                         
			set => SetPropertyValue<Module.BusinessObjects.RelationType>("RelationType", value); 
			
        }
		//Tooltip for Object
		public object RelationTypeToolTipControllerText(View view)
        {
        //    if (RelationType != null) 
		//			return RelationType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.RelationType GetDefaultRelationType(View view = null)
        { 
			return RelationType;
        }
		//Set Default Value
		public void SetDefaultRelationType(View view = null)
        {
            //if (RelationType is null){
            //    var result = GetDefaultRelationType(view);
            //    if (result != null && result != RelationType){
			//          RelationType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RelationTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRelationType();
				//if (result != null && RelationType != null){
				//	return !RelationType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator RelationTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(RelationType));
            }
        }
	
       
		//private DateTime? _startdate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Từ")]
        [ToolTip("Từ")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? StartDate
        { 
		    get => GetPropertyValue<DateTime?>("StartDate");                         
			set => SetPropertyValue<DateTime?>("StartDate", value); 
			
        }
		//Tooltip for Object
		public object StartDateToolTipControllerText(View view)
        {
        //    if (StartDate != null) 
		//			return StartDate;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultStartDate(View view = null)
        { 
			return StartDate;
        }
		//Set Default Value
		public void SetDefaultStartDate(View view = null)
        {
            //if (StartDate is null){
            //    var result = GetDefaultStartDate(view);
            //    if (result != null && result != StartDate){
			//          StartDate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StartDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStartDate();
				//if (result != null && StartDate != null){
				//	return !StartDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _enddate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đến")]
        [ToolTip("Đến")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? EndDate
        { 
		    get => GetPropertyValue<DateTime?>("EndDate");                         
			set => SetPropertyValue<DateTime?>("EndDate", value); 
			
        }
		//Tooltip for Object
		public object EndDateToolTipControllerText(View view)
        {
        //    if (EndDate != null) 
		//			return EndDate;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultEndDate(View view = null)
        { 
			return EndDate;
        }
		//Set Default Value
		public void SetDefaultEndDate(View view = null)
        {
            //if (EndDate is null){
            //    var result = GetDefaultEndDate(view);
            //    if (result != null && result != EndDate){
			//          EndDate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EndDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEndDate();
				//if (result != null && EndDate != null){
				//	return !EndDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _rate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tỉ lệ")]
        [ToolTip("Tỉ lệ")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p0")]
		public decimal? Rate
        { 
		    get => GetPropertyValue<decimal?>("Rate");                         
			set => SetPropertyValue<decimal?>("Rate", value); 
			
        }
		//Tooltip for Object
		public object RateToolTipControllerText(View view)
        {
        //    if (Rate != null) 
		//			return Rate;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultRate(View view = null)
        { 
			return Rate;
        }
		//Set Default Value
		public void SetDefaultRate(View view = null)
        {
            //if (Rate is null){
            //    var result = GetDefaultRate(view);
            //    if (result != null && result != Rate){
			//          Rate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRate();
				//if (result != null && Rate != null){
				//	return !Rate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultUpper(View view = null);
        //SetDefaultLower(View view = null);
        //SetDefaultRelationType(View view = null);
        //SetDefaultStartDate(View view = null);
        //SetDefaultEndDate(View view = null);
        //SetDefaultRate(View view = null);
			
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
