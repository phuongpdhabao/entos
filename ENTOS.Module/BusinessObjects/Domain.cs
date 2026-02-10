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
	[NavigationItem("Common")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Lĩnh vực"), ImageName("Domain")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(Update))]
 
	[MobileColumnAttribute(Context = "Company_DomainList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Domain_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "OrgDivision_DomainList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Domain_LowerDomainList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Tournament_Subject_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Domain_ListView", TargetItems = nameof(Name))]
	[DefaultProperty("Code")]
 
[OptimisticLocking(true)]
    public partial class Domain:  DevExpress.Xpo.XPLiteObject , IReOrder, DevExpress.Persistent.Base.General.ITreeNode , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Domain(Session session)
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
				if (LowerDomainList.IsLoaded)
                {
                    if (LowerDomainList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(LowerDomainList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(LowerDomainList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Domain>(CriteriaOperator.Parse("[UpperDomain.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool lowerdomainlist = Session.Query<Module.BusinessObjects.Domain>().Where(x => x.UpperDomain.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(LowerDomainList), lowerdomainlist);
                        if (lowerdomainlist)
                            return true;

                    }                    
                }				
				if (SubjectDataList.IsLoaded)
                {
                    if (SubjectDataList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(SubjectDataList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(SubjectDataList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.SubjectData>(CriteriaOperator.Parse("[Domain.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool subjectdatalist = Session.Query<Module.BusinessObjects.SubjectData>().Where(x => x.Domain.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(SubjectDataList), subjectdatalist);
                        if (subjectdatalist)
                            return true;

                    }                    
                }				
				if (BusinessRoleList.IsLoaded)
                {
                    if (BusinessRoleList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(BusinessRoleList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(BusinessRoleList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.BusinessRole>(CriteriaOperator.Parse("[Domain.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool businessrolelist = Session.Query<Module.BusinessObjects.BusinessRole>().Where(x => x.Domain.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(BusinessRoleList), businessrolelist);
                        if (businessrolelist)
                            return true;

                    }                    
                }				
				if (GradeSubjectList.IsLoaded)
                {
                    if (GradeSubjectList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(GradeSubjectList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(GradeSubjectList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.GradeSubject>(CriteriaOperator.Parse("[Domain.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool gradesubjectlist = Session.Query<Module.BusinessObjects.GradeSubject>().Where(x => x.Domain.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(GradeSubjectList), gradesubjectlist);
                        if (gradesubjectlist)
                            return true;

                    }                    
                }				
                                
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

 		[Size(100)]
		[RuleRequiredField("RequiredDomainName", DefaultContexts.Save)]
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

	
       
		//private string _english;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tiếng Anh")]
        [ToolTip("Tiếng Anh")]
		//[Index(1)]		

 		[Size(200)]
		public string English
        { 
		    get => GetPropertyValue<string>("English");                         
			set => SetPropertyValue<string>("English", value); 
			
        }
		//Tooltip for Object
		public object EnglishToolTipControllerText(View view)
        {
        //    if (English != null) 
		//			return English;
            return null;
        }
		//Get Default Value
        public string GetDefaultEnglish(View view = null)
        { 
			return English;
        }
		//Set Default Value
		public void SetDefaultEnglish(View view = null)
        {
            //if (English is null){
            //    var result = GetDefaultEnglish(view);
            //    if (result != null && result != English){
			//          English = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EnglishIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEnglish();
				//if (result != null && English != null){
				//	return !English.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(2)]		

 		[Size(20)]
		[RuleUniqueValue("UniqueDomainCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		public string Code
        { 
		    get => GetPropertyValue<string>("Code");                         
			set => SetPropertyValue<string>("Code", value); 
			
        }
		//Tooltip for Object
		public object CodeToolTipControllerText(View view)
        {
        //    if (Code != null) 
		//			return Code;
            return null;
        }
		//Get Default Value
        public string GetDefaultCode(View view = null)
        { 
			return Code;
        }
		//Set Default Value
		public void SetDefaultCode(View view = null)
        {
            //if (Code is null){
            //    var result = GetDefaultCode(view);
            //    if (result != null && result != Code){
			//          Code = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCode();
				//if (result != null && Code != null){
				//	return !Code.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DomainType _domaintype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(3)]		
		public DomainType DomainType
        { 
		    get => GetPropertyValue<DomainType>("DomainType");                         
			set => SetPropertyValue<DomainType>("DomainType", value); 
			
        }
		//Tooltip for Object
		public object DomainTypeToolTipControllerText(View view)
        {
        //    if (DomainType != null) 
		//			return DomainType;
            return null;
        }
		//Get Default Value
        public DomainType GetDefaultDomainType(View view = null)
        { 
			return DomainType;
        }
		//Set Default Value
		public void SetDefaultDomainType(View view = null)
        {
            //if (DomainType is null){
            //    var result = GetDefaultDomainType(view);
            //    if (result != null && result != DomainType){
			//          DomainType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DomainTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDomainType();
				//if (result != null && DomainType != null){
				//	return !DomainType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Domain _upperdomain;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpperDomainCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("UpperDomain-LowerDomainList")]
	 
		public Module.BusinessObjects.Domain UpperDomain
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Domain>("UpperDomain");                         
			set => SetPropertyValue<Module.BusinessObjects.Domain>("UpperDomain", value); 
			
        }
		//Tooltip for Object
		public object UpperDomainToolTipControllerText(View view)
        {
        //    if (UpperDomain != null) 
		//			return UpperDomain;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Domain GetDefaultUpperDomain(View view = null)
        { 
			return UpperDomain;
        }
		//Set Default Value
		public void SetDefaultUpperDomain(View view = null)
        {
            //if (UpperDomain is null){
            //    var result = GetDefaultUpperDomain(view);
            //    if (result != null && result != UpperDomain){
			//          UpperDomain = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperDomainIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpperDomain();
				//if (result != null && UpperDomain != null){
				//	return !UpperDomain.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpperDomainCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(UpperDomain));
            }
        }
	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(5)]		
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
		//Set Default Value

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
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp dưới")]
		//[Index(6)]
		[DevExpress.Xpo.Association("UpperDomain-LowerDomainList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Domain> LowerDomainList
        {      
		    get => GetCollection<Module.BusinessObjects.Domain>("LowerDomainList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dữ liệu")]
		//[Index(7)]
		[DevExpress.Xpo.Association("Domain-SubjectDataList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.SubjectData> SubjectDataList
        {      
		    get => GetCollection<Module.BusinessObjects.SubjectData>("SubjectDataList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vai trò")]
		//[Index(8)]
		[DevExpress.Xpo.Association("Domain-BusinessRoleList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BusinessRole> BusinessRoleList
        {      
		    get => GetCollection<Module.BusinessObjects.BusinessRole>("BusinessRoleList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Môn")]
		//[Index(9)]
        
        [RuleUniqueValue("RuleCollectionValidationDomain.GradeSubjectList.", DefaultContexts.Save, TargetPropertyName = nameof(Module.BusinessObjects.GradeSubject.Code))]
		[DevExpress.Xpo.Association("Domain-GradeSubjectList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.GradeSubject> GradeSubjectList
        {      
		    get => GetCollection<Module.BusinessObjects.GradeSubject>("GradeSubjectList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Company")]
		//[Index(10)]
		[DataSourceCriteria("Not DomainList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("CompanyList-DomainList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Company> CompanyList
        {      
		    get => GetCollection<Module.BusinessObjects.Company>("CompanyList"); 
			
        }
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(11)]		
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

	
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(12)]		
		public bool InActive
        { 
		    get => GetPropertyValue<bool>("InActive");                         
			set => SetPropertyValue<bool>("InActive", value); 
			
        }
		//Tooltip for Object
		public object InActiveToolTipControllerText(View view)
        {
        //    if (InActive != null) 
		//			return InActive;
            return null;
        }
		//Get Default Value
        public bool GetDefaultInActive(View view = null)
        { 
			return InActive;
        }
		//Set Default Value
		public void SetDefaultInActive(View view = null)
        {
            //if (InActive is null){
            //    var result = GetDefaultInActive(view);
            //    if (result != null && result != InActive){
			//          InActive = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool InActiveIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultInActive();
				//if (result != null && InActive != null){
				//	return !InActive.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(13)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
	    [ModelDefault("AllowEdit", "False")]
		public DateTime Update
        { 
		    get => GetPropertyValue<DateTime>("Update");                         
			set => SetPropertyValue<DateTime>("Update", value); 
			
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

	
       
		//private Module.BusinessObjects.OrgDivision _orgdivision;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Trực thuộc")]
        [ToolTip("Trực thuộc")]
		//[Index(14)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OrgDivisionCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("OrgDivision-DomainList")]
	 
		public Module.BusinessObjects.OrgDivision OrgDivision
        { 
		    get => GetPropertyValue<Module.BusinessObjects.OrgDivision>("OrgDivision");                         
			set => SetPropertyValue<Module.BusinessObjects.OrgDivision>("OrgDivision", value); 
			
        }
		//Tooltip for Object
		public object OrgDivisionToolTipControllerText(View view)
        {
        //    if (OrgDivision != null) 
		//			return OrgDivision;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.OrgDivision GetDefaultOrgDivision(View view = null)
        { 
			return OrgDivision;
        }
		//Set Default Value
		public void SetDefaultOrgDivision(View view = null)
        {
            //if (OrgDivision is null){
            //    var result = GetDefaultOrgDivision(view);
            //    if (result != null && result != OrgDivision){
			//          OrgDivision = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OrgDivisionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrgDivision();
				//if (result != null && OrgDivision != null){
				//	return !OrgDivision.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator OrgDivisionCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(OrgDivision));
            }
        }
	
       
		//private DevExpress.Persistent.Base.General.ITreeNode _parent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Parent")]
        [ToolTip("Parent")]
		//[Index(15)]		
		public DevExpress.Persistent.Base.General.ITreeNode Parent
        { 
		    #region 2614ImportCode 
get => UpperDomain;
#endregion 2614ImportCode
			
        }
		//Tooltip for Object
		public object ParentToolTipControllerText(View view)
        {
        //    if (Parent != null) 
		//			return Parent;
            return null;
        }
		//Get Default Value
        public DevExpress.Persistent.Base.General.ITreeNode GetDefaultParent(View view = null)
        { 
			return Parent;
        }
		//Set Default Value
		public void SetDefaultParent(View view = null)
        {
            //if (Parent is null){
            //    var result = GetDefaultParent(view);
            //    if (result != null && result != Parent){
			//          Parent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ParentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultParent();
				//if (result != null && Parent != null){
				//	return !Parent.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private System.ComponentModel.IBindingList _children;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Children")]
        [ToolTip("Children")]
		//[Index(16)]		
		public System.ComponentModel.IBindingList Children
        { 
		    #region 2615ImportCode 
get => LowerDomainList;
#endregion 2615ImportCode
			
        }
		//Tooltip for Object
		public object ChildrenToolTipControllerText(View view)
        {
        //    if (Children != null) 
		//			return Children;
            return null;
        }
		//Get Default Value
        public System.ComponentModel.IBindingList GetDefaultChildren(View view = null)
        { 
			return Children;
        }
		//Set Default Value
		public void SetDefaultChildren(View view = null)
        {
            //if (Children is null){
            //    var result = GetDefaultChildren(view);
            //    if (result != null && result != Children){
			//          Children = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ChildrenIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultChildren();
				//if (result != null && Children != null){
				//	return !Children.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 2487ImportCode
            base.AfterConstruction();
SetDefaultMember();
            #endregion 2487ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultEnglish(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultDomainType(View view = null);
        //SetDefaultUpperDomain(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultInActive(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultOrgDivision(View view = null);
        //SetDefaultParent(View view = null);
        //SetDefaultChildren(View view = null);
			
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
            #region 0375ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0375ImportCode
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
			//	SetDefaultLowerDomainList();
			//	SetDefaultSubjectDataList();
			//	SetDefaultBusinessRoleList();
			//	SetDefaultGradeSubjectList();
			//	SetDefaultCompanyList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 2488ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 2488            Oid: 8144ffe9-def8-4c1b-87c5-68207734d1cd
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2488ImportCode
#region 0093ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0093            Oid: 28768ff4-8e6a-4f23-a0ee-3e78e334b450
            Update = GetDefaultUpdate();
        }
#endregion 0093ImportCode
#region 0137ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0137            Oid: 87696fcc-0494-4b8e-b728-0b15dd0cf79d
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0137ImportCode
#region 2486ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 2486            Oid: 92226269-417a-4518-9e05-d28d5b9777ce
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 2486ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
