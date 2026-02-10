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
    [ModelDefault("Caption", "Quá trình học tập"), ImageName("LearningHistory")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "LearningHistory_LookupListView", TargetItems = nameof(FromYear)+ "," + nameof(ToYear))]
	[MobileColumnAttribute(Context = "LearningHistory_ListView", TargetItems = nameof(ToYear)+ "," + nameof(FromYear))]
	[DefaultProperty("Contact")]
 
[OptimisticLocking(true)]
    public partial class LearningHistory:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public LearningHistory(Session session)
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
               

		//private Module.BusinessObjects.Contact _learnercontact;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Học viên")]
        [ToolTip("Học viên")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LearnerContactCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Contact LearnerContact
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Contact>("LearnerContact");                         
			set => SetPropertyValue<Module.BusinessObjects.Contact>("LearnerContact", value); 
			
        }
		//Tooltip for Object
		public object LearnerContactToolTipControllerText(View view)
        {
        //    if (LearnerContact != null) 
		//			return LearnerContact;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Contact GetDefaultLearnerContact(View view = null)
        { 
			return LearnerContact;
        }
		//Set Default Value
		public void SetDefaultLearnerContact(View view = null)
        {
            //if (LearnerContact is null){
            //    var result = GetDefaultLearnerContact(view);
            //    if (result != null && result != LearnerContact){
			//          LearnerContact = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LearnerContactIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLearnerContact();
				//if (result != null && LearnerContact != null){
				//	return !LearnerContact.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LearnerContactCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(LearnerContact));
            }
        }
	
       
		//private Module.BusinessObjects.Org _schoolorg;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Trường")]
        [ToolTip("Trường")]
		//[Index(1)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SchoolOrgCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Org SchoolOrg
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Org>("SchoolOrg");                         
			set => SetPropertyValue<Module.BusinessObjects.Org>("SchoolOrg", value); 
			
        }
		//Tooltip for Object
		public object SchoolOrgToolTipControllerText(View view)
        {
        //    if (SchoolOrg != null) 
		//			return SchoolOrg;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Org GetDefaultSchoolOrg(View view = null)
        { 
			return SchoolOrg;
        }
		//Set Default Value
		public void SetDefaultSchoolOrg(View view = null)
        {
            //if (SchoolOrg is null){
            //    var result = GetDefaultSchoolOrg(view);
            //    if (result != null && result != SchoolOrg){
			//          SchoolOrg = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SchoolOrgIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSchoolOrg();
				//if (result != null && SchoolOrg != null){
				//	return !SchoolOrg.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SchoolOrgCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SchoolOrg));
            }
        }
	
       
		//private Module.BusinessObjects.OrgDivision _shoolorgdivision;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Khoa")]
        [ToolTip("Khoa")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ShoolOrgDivisionCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.OrgDivision ShoolOrgDivision
        { 
		    get => GetPropertyValue<Module.BusinessObjects.OrgDivision>("ShoolOrgDivision");                         
			set => SetPropertyValue<Module.BusinessObjects.OrgDivision>("ShoolOrgDivision", value); 
			
        }
		//Tooltip for Object
		public object ShoolOrgDivisionToolTipControllerText(View view)
        {
        //    if (ShoolOrgDivision != null) 
		//			return ShoolOrgDivision;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.OrgDivision GetDefaultShoolOrgDivision(View view = null)
        { 
			return ShoolOrgDivision;
        }
		//Set Default Value
		public void SetDefaultShoolOrgDivision(View view = null)
        {
            //if (ShoolOrgDivision is null){
            //    var result = GetDefaultShoolOrgDivision(view);
            //    if (result != null && result != ShoolOrgDivision){
			//          ShoolOrgDivision = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ShoolOrgDivisionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultShoolOrgDivision();
				//if (result != null && ShoolOrgDivision != null){
				//	return !ShoolOrgDivision.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ShoolOrgDivisionCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ShoolOrgDivision));
            }
        }
	
       
		//private Module.BusinessObjects.Org _classorg;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Lớp")]
        [ToolTip("Lớp")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ClassOrgCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Org ClassOrg
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Org>("ClassOrg");                         
			set => SetPropertyValue<Module.BusinessObjects.Org>("ClassOrg", value); 
			
        }
		//Tooltip for Object
		public object ClassOrgToolTipControllerText(View view)
        {
        //    if (ClassOrg != null) 
		//			return ClassOrg;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Org GetDefaultClassOrg(View view = null)
        { 
			return ClassOrg;
        }
		//Set Default Value
		public void SetDefaultClassOrg(View view = null)
        {
            //if (ClassOrg is null){
            //    var result = GetDefaultClassOrg(view);
            //    if (result != null && result != ClassOrg){
			//          ClassOrg = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ClassOrgIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultClassOrg();
				//if (result != null && ClassOrg != null){
				//	return !ClassOrg.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ClassOrgCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ClassOrg));
            }
        }
	
       
		//private DateTime _fromyear;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Từ năm")]
        [ToolTip("Từ năm")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "yyyy")]
		[ModelDefault("EditMask", "yyyy")]
		public DateTime FromYear
        { 
		    get => GetPropertyValue<DateTime>("FromYear");                         
			set => SetPropertyValue<DateTime>("FromYear", value); 
			
        }
		//Tooltip for Object
		public object FromYearToolTipControllerText(View view)
        {
        //    if (FromYear != null) 
		//			return FromYear;
            return null;
        }
		//Get Default Value
        public DateTime GetDefaultFromYear(View view = null)
        { 
			return FromYear;
        }
		//Set Default Value
		public void SetDefaultFromYear(View view = null)
        {
            //if (FromYear is null){
            //    var result = GetDefaultFromYear(view);
            //    if (result != null && result != FromYear){
			//          FromYear = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FromYearIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFromYear();
				//if (result != null && FromYear != null){
				//	return !FromYear.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime _toyear;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Đến năm")]
        [ToolTip("Đến năm")]
		//[Index(7)]		
		[ModelDefault("DisplayFormat", "yyyy")]
		[ModelDefault("EditMask", "yyyy")]
		public DateTime ToYear
        { 
		    get => GetPropertyValue<DateTime>("ToYear");                         
			set => SetPropertyValue<DateTime>("ToYear", value); 
			
        }
		//Tooltip for Object
		public object ToYearToolTipControllerText(View view)
        {
        //    if (ToYear != null) 
		//			return ToYear;
            return null;
        }
		//Get Default Value
        public DateTime GetDefaultToYear(View view = null)
        { 
			return ToYear;
        }
		//Set Default Value
		public void SetDefaultToYear(View view = null)
        {
            //if (ToYear is null){
            //    var result = GetDefaultToYear(view);
            //    if (result != null && result != ToYear){
			//          ToYear = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ToYearIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultToYear();
				//if (result != null && ToYear != null){
				//	return !ToYear.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _graduation;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tốt nghiệp")]
        [ToolTip("Tốt nghiệp")]
		//[Index(8)]		
		public bool Graduation
        { 
		    get => GetPropertyValue<bool>("Graduation");                         
			set => SetPropertyValue<bool>("Graduation", value); 
			
        }
		//Tooltip for Object
		public object GraduationToolTipControllerText(View view)
        {
        //    if (Graduation != null) 
		//			return Graduation;
            return null;
        }
		//Get Default Value
        public bool GetDefaultGraduation(View view = null)
        { 
			return Graduation;
        }
		//Set Default Value
		public void SetDefaultGraduation(View view = null)
        {
            //if (Graduation is null){
            //    var result = GetDefaultGraduation(view);
            //    if (result != null && result != Graduation){
			//          Graduation = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool GraduationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultGraduation();
				//if (result != null && Graduation != null){
				//	return !Graduation.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultLearnerContact(View view = null);
        //SetDefaultSchoolOrg(View view = null);
        //SetDefaultShoolOrgDivision(View view = null);
        //SetDefaultClassOrg(View view = null);
        //SetDefaultFromYear(View view = null);
        //SetDefaultToYear(View view = null);
        //SetDefaultGraduation(View view = null);
			
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
