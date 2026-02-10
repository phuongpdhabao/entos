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
	[NavigationItem("Communication")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Tổ chức"), ImageName("Org")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("Org ProductTypeList, ProductPriceList Hide_None__" , TargetItems = "ProductTypeList, ProductPriceList" ,AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(TaxCode)+ "," + nameof(Representative)+ "," + nameof(UpperOrgName)+ "," + nameof(OrgType)+ "," + nameof(SinceDate)+ "," + nameof(Folder)+ "," + nameof(Member)+ "," + nameof(Image)+ "," + nameof(InActive)+ "," + nameof(BusinessRoleList)+ "," + nameof(PaymentAccountList)+ "," + nameof(HistoryList), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(Update)+ "," + nameof(Updater))]
 
	[MobileColumnAttribute(Context = "Course_ClassList_ListView", TargetItems = nameof(Folder)+ "," + nameof(Image)+ "," + nameof(Code))]
	[MobileColumnAttribute(Context = "ProductType_OrgList_ListView", TargetItems = nameof(Image)+ "," + nameof(Folder)+ "," + nameof(Code))]
	[MobileColumnAttribute(Context = "Folder_OrgList_ListView", TargetItems = nameof(Code)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Org_ListView", TargetItems = nameof(Folder)+ "," + nameof(Image)+ "," + nameof(Code))]
	[MobileColumnAttribute(Context = "Org_LookupListView", TargetItems = nameof(Image)+ "," + nameof(Code)+ "," + nameof(Folder))]
	[MobileColumnAttribute(Context = "EducationDegree_OrgList_ListView", TargetItems = nameof(Image)+ "," + nameof(Folder)+ "," + nameof(Code))]
	[DefaultProperty("Code")]
 
[OptimisticLocking(true)]
    public partial class Org:  DevExpress.Xpo.XPLiteObject , IUpCaseModify, IObjectImage, INewObjectSession, ICustomerGroup, IWebData, IFilterMine , INoIndexColumn, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public Org(Session session)
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
				if (ContactList.IsLoaded)
                {
                    if (ContactList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ContactList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ContactList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Contact>(CriteriaOperator.Parse("[Org.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool contactlist = Session.Query<Module.BusinessObjects.Contact>().Where(x => x.Org.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ContactList), contactlist);
                        if (contactlist)
                            return true;

                    }                    
                }				
				if (DivisionList.IsLoaded)
                {
                    if (DivisionList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(DivisionList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(DivisionList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.OrgDivision>(CriteriaOperator.Parse("[Org.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool divisionlist = Session.Query<Module.BusinessObjects.OrgDivision>().Where(x => x.Org.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(DivisionList), divisionlist);
                        if (divisionlist)
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
                        //if (Session.FindObject<Module.BusinessObjects.BusinessRole>(CriteriaOperator.Parse("[Org.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool businessrolelist = Session.Query<Module.BusinessObjects.BusinessRole>().Where(x => x.Org.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(BusinessRoleList), businessrolelist);
                        if (businessrolelist)
                            return true;

                    }                    
                }				
				if (PaymentAccountList.IsLoaded)
                {
                    if (PaymentAccountList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(PaymentAccountList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(PaymentAccountList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.PaymentAccount>(CriteriaOperator.Parse("[Org.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool paymentaccountlist = Session.Query<Module.BusinessObjects.PaymentAccount>().Where(x => x.Org.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(PaymentAccountList), paymentaccountlist);
                        if (paymentaccountlist)
                            return true;

                    }                    
                }				
				if (HistoryList.IsLoaded)
                {
                    if (HistoryList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(HistoryList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(HistoryList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.History>(CriteriaOperator.Parse("[Org.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool historylist = Session.Query<Module.BusinessObjects.History>().Where(x => x.Org.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(HistoryList), historylist);
                        if (historylist)
                            return true;

                    }                    
                }				
				if (ProductPriceList.IsLoaded)
                {
                    if (ProductPriceList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ProductPriceList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ProductPriceList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ProductPrice>(CriteriaOperator.Parse("[Supplier.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool productpricelist = Session.Query<Module.BusinessObjects.ProductPrice>().Where(x => x.Supplier.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ProductPriceList), productpricelist);
                        if (productpricelist)
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
               

		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(0)]		

 		[Size(150)]
		[RuleUniqueValue("UniqueOrgCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredOrgCode", DefaultContexts.Save)]
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

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(1)]		

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

	
       
		//private string _taxcode;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã số thuế")]
        [ToolTip("Mã số thuế")]
		//[Index(2)]		

 		[Size(20)]
		public string TaxCode
        { 
		    get => GetPropertyValue<string>("TaxCode");                         
			set => SetPropertyValue<string>("TaxCode", value); 
			
        }
		//Tooltip for Object
		public object TaxCodeToolTipControllerText(View view)
        {
        //    if (TaxCode != null) 
		//			return TaxCode;
            return null;
        }
		//Get Default Value
        public string GetDefaultTaxCode(View view = null)
        { 
			return TaxCode;
        }
		//Set Default Value
		public void SetDefaultTaxCode(View view = null)
        {
            //if (TaxCode is null){
            //    var result = GetDefaultTaxCode(view);
            //    if (result != null && result != TaxCode){
			//          TaxCode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TaxCodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTaxCode();
				//if (result != null && TaxCode != null){
				//	return !TaxCode.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _representative;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Đại diện")]
        [ToolTip("Đại diện")]
		//[Index(3)]		

 		[Size(100)]
		public string Representative
        { 
		    get => GetPropertyValue<string>("Representative");                         
			set => SetPropertyValue<string>("Representative", value); 
			
        }
		//Tooltip for Object
		public object RepresentativeToolTipControllerText(View view)
        {
        //    if (Representative != null) 
		//			return Representative;
            return null;
        }
		//Get Default Value
        public string GetDefaultRepresentative(View view = null)
        { 
			return Representative;
        }
		//Set Default Value
		public void SetDefaultRepresentative(View view = null)
        {
            //if (Representative is null){
            //    var result = GetDefaultRepresentative(view);
            //    if (result != null && result != Representative){
			//          Representative = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RepresentativeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRepresentative();
				//if (result != null && Representative != null){
				//	return !Representative.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _upperorgname;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chủ quản")]
        [ToolTip("Chủ quản")]
		//[Index(4)]		

 		[Size(100)]
		public string UpperOrgName
        { 
		    get => GetPropertyValue<string>("UpperOrgName");                         
			set => SetPropertyValue<string>("UpperOrgName", value); 
			
        }
		//Tooltip for Object
		public object UpperOrgNameToolTipControllerText(View view)
        {
        //    if (UpperOrgName != null) 
		//			return UpperOrgName;
            return null;
        }
		//Get Default Value
        public string GetDefaultUpperOrgName(View view = null)
        { 
			return UpperOrgName;
        }
		//Set Default Value
		public void SetDefaultUpperOrgName(View view = null)
        {
            //if (UpperOrgName is null){
            //    var result = GetDefaultUpperOrgName(view);
            //    if (result != null && result != UpperOrgName){
			//          UpperOrgName = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperOrgNameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpperOrgName();
				//if (result != null && UpperOrgName != null){
				//	return !UpperOrgName.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private OrgType _orgtype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(5)]		
		public OrgType OrgType
        { 
		    get => GetPropertyValue<OrgType>("OrgType");                         
			set => SetPropertyValue<OrgType>("OrgType", value); 
			
        }
		//Tooltip for Object
		public object OrgTypeToolTipControllerText(View view)
        {
        //    if (OrgType != null) 
		//			return OrgType;
            return null;
        }
		//Get Default Value
        public OrgType GetDefaultOrgType(View view = null)
        { 
			return OrgType;
        }
		//Set Default Value
		public void SetDefaultOrgType(View view = null)
        {
            //if (OrgType is null){
            //    var result = GetDefaultOrgType(view);
            //    if (result != null && result != OrgType){
			//          OrgType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OrgTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrgType();
				//if (result != null && OrgType != null){
				//	return !OrgType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _link;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
        [ToolTip("Liên kết")]
		//[Index(6)]		

 		[Size(250)]
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

	
       
		//private string _address;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Địa chỉ")]
        [ToolTip("Địa chỉ")]
		//[Index(7)]		

 		[Size(250)]
		public string Address
        { 
		    get => GetPropertyValue<string>("Address");                         
			set => SetPropertyValue<string>("Address", value); 
			
        }
		//Tooltip for Object
		public object AddressToolTipControllerText(View view)
        {
        //    if (Address != null) 
		//			return Address;
            return null;
        }
		//Get Default Value
        public string GetDefaultAddress(View view = null)
        { 
			return Address;
        }
		//Set Default Value
		public void SetDefaultAddress(View view = null)
        {
            //if (Address is null){
            //    var result = GetDefaultAddress(view);
            //    if (result != null && result != Address){
			//          Address = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AddressIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAddress();
				//if (result != null && Address != null){
				//	return !Address.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _sincedate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thành lập")]
        [ToolTip("Thành lập")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? SinceDate
        { 
		    get => GetPropertyValue<DateTime?>("SinceDate");                         
			set => SetPropertyValue<DateTime?>("SinceDate", value); 
			
        }
		//Tooltip for Object
		public object SinceDateToolTipControllerText(View view)
        {
        //    if (SinceDate != null) 
		//			return SinceDate;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultSinceDate(View view = null)
        { 
			return SinceDate;
        }
		//Set Default Value
		public void SetDefaultSinceDate(View view = null)
        {
            //if (SinceDate is null){
            //    var result = GetDefaultSinceDate(view);
            //    if (result != null && result != SinceDate){
			//          SinceDate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SinceDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSinceDate();
				//if (result != null && SinceDate != null){
				//	return !SinceDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Folder _folder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#Org# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-OrgList")]
	 
		public Module.BusinessObjects.Folder Folder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Folder>("Folder");                         
			set => SetPropertyValue<Module.BusinessObjects.Folder>("Folder", value); 
			
        }
		//Tooltip for Object
		public object FolderToolTipControllerText(View view)
        {
        //    if (Folder != null) 
		//			return Folder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Folder GetDefaultFolder(View view = null)
        { 
			return Folder;
        }
		//Set Default Value
		public void SetDefaultFolder(View view = null)
        {
            //if (Folder is null){
            //    var result = GetDefaultFolder(view);
            //    if (result != null && result != Folder){
			//          Folder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFolder();
				//if (result != null && Folder != null){
				//	return !Folder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator FolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Folder));
            }
        }
	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
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
	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Biểu tượng")]
        [ToolTip("Biểu tượng")]
		//[Index(11)]		
		[Appearance("Biểu tượngBackground", BackColor = "Transparent")]
	
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

	
       
		//private bool _inactive;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
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

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
		//[Index(13)]
		[DevExpress.Xpo.Association("Org-BookMarkList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BookMark> BookMarkList
        {      
		    get => GetCollection<Module.BusinessObjects.BookMark>("BookMarkList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên hệ")]
		//[Index(14)]
		[DevExpress.Xpo.Association("Org-ContactList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Contact> ContactList
        {      
		    get => GetCollection<Module.BusinessObjects.Contact>("ContactList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bộ phận")]
		//[Index(15)]
        
        [RuleUniqueValue("RuleCollectionValidationOrg.DivisionList.", DefaultContexts.Save, TargetPropertyName = nameof(Module.BusinessObjects.OrgDivision.Order))]
		[DevExpress.Xpo.Association("Org-DivisionList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.OrgDivision> DivisionList
        {      
		    get => GetCollection<Module.BusinessObjects.OrgDivision>("DivisionList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chức danh")]
		//[Index(16)]
		[DevExpress.Xpo.Association("Org-BusinessRoleList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BusinessRole> BusinessRoleList
        {      
		    get => GetCollection<Module.BusinessObjects.BusinessRole>("BusinessRoleList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài khoản")]
		//[Index(17)]
		[DevExpress.Xpo.Association("Org-PaymentAccountList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.PaymentAccount> PaymentAccountList
        {      
		    get => GetCollection<Module.BusinessObjects.PaymentAccount>("PaymentAccountList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lịch sử")]
		//[Index(18)]
		[DevExpress.Xpo.Association("Org-HistoryList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.History> HistoryList
        {      
		    get => GetCollection<Module.BusinessObjects.History>("HistoryList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại sản phẩm")]
		//[Index(19)]
		[DataSourceCriteria("Not OrgList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("OrgList-ProductTypeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductType> ProductTypeList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductType>("ProductTypeList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sản phẩm")]
		//[Index(20)]
		[DevExpress.Xpo.Association("Supplier-ProductPriceList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductPrice> ProductPriceList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductPrice>("ProductPriceList"); 
			
        }
       
		//private Module.BusinessObjects.Contact _contact;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên hệ")]
        [ToolTip("Liên hệ")]
		//[Index(23)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ContactCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Contact Contact
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Contact>("Contact");                         
			set => SetPropertyValue<Module.BusinessObjects.Contact>("Contact", value); 
			
        }
		//Tooltip for Object
		public object ContactToolTipControllerText(View view)
        {
        //    if (Contact != null) 
		//			return Contact;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Contact GetDefaultContact(View view = null)
        { 
			return Contact;
        }
		//Set Default Value
		public void SetDefaultContact(View view = null)
        {
            //if (Contact is null){
            //    var result = GetDefaultContact(view);
            //    if (result != null && result != Contact){
			//          Contact = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ContactIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultContact();
				//if (result != null && Contact != null){
				//	return !Contact.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ContactCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Contact));
            }
        }
	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(24)]		
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

	
       
		//private Module.BusinessObjects.Member _updater;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Người cập nhật")]
        [ToolTip("Người cập nhật")]
		//[Index(25)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpdaterCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [ModelDefault("AllowEdit", "False")]
		public Module.BusinessObjects.Member Updater
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Updater");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Updater", value); 
			
        }
		//Tooltip for Object
		public object UpdaterToolTipControllerText(View view)
        {
        //    if (Updater != null) 
		//			return Updater;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool UpdaterIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpdater();
				//if (result != null && Updater != null){
				//	return !Updater.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpdaterCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Updater));
            }
        }
	
       
		//private decimal? _positivefeedback;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đánh giá tốt")]
        [ToolTip("Đánh giá tốt")]
		//[Index(28)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p2")]
		public decimal? PositiveFeedback
        { 
		    get => GetPropertyValue<decimal?>("PositiveFeedback");                         
			set => SetPropertyValue<decimal?>("PositiveFeedback", value); 
			
        }
		//Tooltip for Object
		public object PositiveFeedbackToolTipControllerText(View view)
        {
        //    if (PositiveFeedback != null) 
		//			return PositiveFeedback;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultPositiveFeedback(View view = null)
        { 
			return PositiveFeedback;
        }
		//Set Default Value
		public void SetDefaultPositiveFeedback(View view = null)
        {
            //if (PositiveFeedback is null){
            //    var result = GetDefaultPositiveFeedback(view);
            //    if (result != null && result != PositiveFeedback){
			//          PositiveFeedback = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PositiveFeedbackIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPositiveFeedback();
				//if (result != null && PositiveFeedback != null){
				//	return !PositiveFeedback.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Folder _folderhome;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục gốc")]
        [ToolTip("Thư mục gốc")]
		//[Index(29)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(FolderHomeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Folder FolderHome
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Folder>("FolderHome");                         
			set => SetPropertyValue<Module.BusinessObjects.Folder>("FolderHome", value); 
			
        }
		//Tooltip for Object
		public object FolderHomeToolTipControllerText(View view)
        {
        //    if (FolderHome != null) 
		//			return FolderHome;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Folder GetDefaultFolderHome(View view = null)
        { 
			return FolderHome;
        }
		//Set Default Value
		public void SetDefaultFolderHome(View view = null)
        {
            //if (FolderHome is null){
            //    var result = GetDefaultFolderHome(view);
            //    if (result != null && result != FolderHome){
			//          FolderHome = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FolderHomeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFolderHome();
				//if (result != null && FolderHome != null){
				//	return !FolderHome.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator FolderHomeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(FolderHome));
            }
        }
	
       
        private bool _display;
        [Browsable(false)]
        [NonPersistent]
        [ImmediatePostData]
        public bool Display
        {
            get { return _display; }
            set { SetPropertyValue("Display", ref _display, value); }
        }
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 2512ImportCode
            base.AfterConstruction();
SetDefaultMember();
            #endregion 2512ImportCode
            Display = true;
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultTaxCode(View view = null);
        //SetDefaultRepresentative(View view = null);
        //SetDefaultUpperOrgName(View view = null);
        //SetDefaultOrgType(View view = null);
        //SetDefaultLink(View view = null);
        //SetDefaultAddress(View view = null);
        //SetDefaultSinceDate(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultInActive(View view = null);
        //SetDefaultContact(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultPositiveFeedback(View view = null);
        //SetDefaultFolderHome(View view = null);
			
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
            #region 1270ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 1270ImportCode
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
            Session.Delete(this.BookMarkList);				
  
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
			//	SetDefaultBookMarkList();
			//	SetDefaultContactList();
			//	SetDefaultDivisionList();
			//	SetDefaultBusinessRoleList();
			//	SetDefaultPaymentAccountList();
			//	SetDefaultHistoryList();
			//	SetDefaultProductTypeList();
			//	SetDefaultProductPriceList();
			//	SetDefaultEducationDegreeList();
			//	SetDefaultMusicPerformanceList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1344ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 1344            Oid: 9237ec22-4619-4039-a362-10d21bad52e1
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 1344ImportCode
#region 1343ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 1343            Oid: 8fcc8e67-7e6c-4d8a-92e6-8fc9b640272c
            Updater = GetDefaultUpdater();
        }
#endregion 1343ImportCode
#region 2513ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 2513            Oid: 7317715f-e39b-48a6-8dad-e5f2e28056f6
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2513ImportCode
#region 1271ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 1271            Oid: 83e919d1-fdbd-4076-af1e-6c76f3b5ecfc
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1271ImportCode
#region 1269ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 1269            Oid: 2600f7e8-2a33-475c-b555-019e52a71170
            Update = GetDefaultUpdate();
        }
#endregion 1269ImportCode
#region 2511ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 2511            Oid: bffd2fea-c290-4f8f-bfec-3a76317050ed
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 2511ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
