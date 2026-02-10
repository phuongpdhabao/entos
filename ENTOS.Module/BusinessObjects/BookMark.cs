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
    [ModelDefault("Caption", "Liên kết"), ImageName("BookMark")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("BookMark ExtractorDataList Hide_None__" , TargetItems = "ExtractorDataList" , Criteria = "[AIExtractor] Is Null",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("BookMark Image, URL, Note Hide_None__" , TargetItems = "Image, URL, Note" , Criteria = "[AIExtractor] Is Not Null",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide , Context = "DetailView" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
    [ShowToolTipAttribute(TargetItems = nameof(URL))]
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Name)+ "," + nameof(CreatedDate)+ "," + nameof(Update)+ "," + nameof(Updater)+ "," + nameof(Order)+ "," + nameof(Member)+ "," + nameof(SystemType))]
 
	[MobileColumnAttribute(Context = "Company_BookMarkList_ListView", TargetItems = nameof(URL)+ "," + nameof(Image)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "BookMark_ListView", TargetItems = nameof(Image)+ "," + nameof(URL)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_BookMarkList_ListView", TargetItems = nameof(Image)+ "," + nameof(Name)+ "," + nameof(URL))]
	[MobileColumnAttribute(Context = "Product_BookMarkList_ListView", TargetItems = nameof(Image)+ "," + nameof(Name)+ "," + nameof(URL))]
	[MobileColumnAttribute(Context = "SourceCode_BookMarkList_ListView", TargetItems = nameof(URL)+ "," + nameof(Name)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Post_BookMarkList_ListView", TargetItems = nameof(Name)+ "," + nameof(URL)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Contact_BookMarkList_ListView", TargetItems = nameof(Name)+ "," + nameof(URL)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "WorkType_BookMarkList_ListView", TargetItems = nameof(URL)+ "," + nameof(Image)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "BookMark_LookupListView", TargetItems = nameof(Name)+ "," + nameof(Image)+ "," + nameof(URL))]
	[MobileColumnAttribute(Context = "Video_FileList_ListView", TargetItems = nameof(Name)+ "," + nameof(URL)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Invester_BookMarkList_ListView", TargetItems = nameof(Name)+ "," + nameof(URL)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Org_BookMarkList_ListView", TargetItems = nameof(URL)+ "," + nameof(Image)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Recognition_BookMarkList_ListView", TargetItems = nameof(URL)+ "," + nameof(Image)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "AIExtractor_BookMarkList_ListView", TargetItems = nameof(Update)+ "," + nameof(Name)+ "," + nameof(URL))]
	[MobileColumnAttribute(Context = "Equipment_BookMarkList_ListView", TargetItems = nameof(URL)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "ProductListing_BookMarkList_ListView", TargetItems = nameof(Name)+ "," + nameof(Image)+ "," + nameof(URL))]
	[MobileColumnAttribute(Context = "Asset_BookMarkList_ListView", TargetItems = nameof(URL)+ "," + nameof(Name)+ "," + nameof(Image))]
	[DefaultProperty("Order")]
 
	
	[CustomFilter("IFolder", "Folder.Oid = ?")]
	[DevExpress.ExpressApp.SystemModule.ListViewFilter("Có ảnh", "Not (Image is null)")]
	[DevExpress.ExpressApp.SystemModule.ListViewFilter("Tất cả", "", true)]
	[DevExpress.ExpressApp.SystemModule.ListViewFilter("Ảnh trống", "Image is null")]
	[UpDownTopBottomOrder(AscSort = true, ChangeBetweenRow = false, AutoSave = false)]
[OptimisticLocking(true)]
    public partial class BookMark:  DevExpress.Xpo.XPLiteObject , IUrlInfo, IReOrder, IObjectImage, INewObjectSession, IUpCaseModify ,IFolder, INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public BookMark(Session session)
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
				if (ExtractorDataList.IsLoaded)
                {
                    if (ExtractorDataList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ExtractorDataList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ExtractorDataList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ExtractorData>(CriteriaOperator.Parse("[BookMark.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool extractordatalist = Session.Query<Module.BusinessObjects.ExtractorData>().Where(x => x.BookMark.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ExtractorDataList), extractordatalist);
                        if (extractordatalist)
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

 		[Size(1000)]
	    [ModelDefault("RowCount","1")]
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
		//Set Default Value

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

	
       
		//private string _url;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Địa chỉ")]
        [ToolTip("Địa chỉ")]
		//[Index(1)]		

 		[Size(1000)]
	    [ModelDefault("RowCount","1")]
	    [ImmediatePostData()]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string URL
        { 
		    get => GetPropertyValue<string>("URL");                         
			set => SetPropertyValue<string>("URL", value); 
			
        }
		//Tooltip for Object
		public object URLToolTipControllerText(View view)
        {
            #region 1092ImportCode 
if (!string.IsNullOrEmpty(URL))
{
    var otherBookmarks = new XPCollection<BookMark>(Session, (DevExpress.Data.Filtering.CriteriaOperator.Parse("Oid <> ? and URL = ? ", Oid, URL)));
    if (otherBookmarks != null && otherBookmarks.Count > 0)
    {
        return string.Join(", ", otherBookmarks.Where(m => m.Folder != null && !string.IsNullOrEmpty(m.Folder.Name)).Select(m => m.Folder.Name));
    }
}
#endregion 1092ImportCode
            return null;
        }
		//Get Default Value
        public string GetDefaultURL(View view = null)
        { 
			return URL;
        }
		//Set Default Value
		public void SetDefaultURL(View view = null)
        {
            //if (URL is null){
            //    var result = GetDefaultURL(view);
            //    if (result != null && result != URL){
			//          URL = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool URLIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultURL();
				//if (result != null && URL != null){
				//	return !URL.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
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

	
       
		//private string _note;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi chú")]
        [ToolTip("Ghi chú")]
		//[Index(3)]		

 		[Size(1000)]
	    [EditorAlias("FileBrowserPropertyEditor")]
	    [ModelDefault("RowCount","1")]
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

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dữ liệu")]
		//[Index(4)]
		[DevExpress.Xpo.Association("BookMark-ExtractorDataList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ExtractorData> ExtractorDataList
        {      
		    get => GetCollection<Module.BusinessObjects.ExtractorData>("ExtractorDataList"); 
			
        }
       
		//private DateTime? _createddate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy H:mm")]
		public DateTime? CreatedDate
        { 
		    get => GetPropertyValue<DateTime?>("CreatedDate");                         
			set => SetPropertyValue<DateTime?>("CreatedDate", value); 
			
        }
		//Tooltip for Object
		public object CreatedDateToolTipControllerText(View view)
        {
        //    if (CreatedDate != null) 
		//			return CreatedDate;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool CreatedDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCreatedDate();
				//if (result != null && CreatedDate != null){
				//	return !CreatedDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(6)]		
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
		//[Index(7)]		
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
	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(8)]		
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
		//Set Default Value

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

	
       
		//private Module.BusinessObjects.Contact _contact;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên hệ")]
        [ToolTip("Liên hệ")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ContactCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Contact-BookMarkList")]
	 
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
	
       
		//private Module.BusinessObjects.Org _org;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tổ chức")]
        [ToolTip("Tổ chức")]
		//[Index(10)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OrgCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Org-BookMarkList")]
	 
		public Module.BusinessObjects.Org Org
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Org>("Org");                         
			set => SetPropertyValue<Module.BusinessObjects.Org>("Org", value); 
			
        }
		//Tooltip for Object
		public object OrgToolTipControllerText(View view)
        {
        //    if (Org != null) 
		//			return Org;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Org GetDefaultOrg(View view = null)
        { 
			return Org;
        }
		//Set Default Value
		public void SetDefaultOrg(View view = null)
        {
            //if (Org is null){
            //    var result = GetDefaultOrg(view);
            //    if (result != null && result != Org){
			//          Org = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OrgIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrg();
				//if (result != null && Org != null){
				//	return !Org.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator OrgCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Org));
            }
        }
	
       
		//private Module.BusinessObjects.Asset _asset;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài sản")]
        [ToolTip("Tài sản")]
		//[Index(11)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(AssetCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Asset-BookMarkList")]
	 
		public Module.BusinessObjects.Asset Asset
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Asset>("Asset");                         
			set => SetPropertyValue<Module.BusinessObjects.Asset>("Asset", value); 
			
        }
		//Tooltip for Object
		public object AssetToolTipControllerText(View view)
        {
        //    if (Asset != null) 
		//			return Asset;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Asset GetDefaultAsset(View view = null)
        { 
			return Asset;
        }
		//Set Default Value
		public void SetDefaultAsset(View view = null)
        {
            //if (Asset is null){
            //    var result = GetDefaultAsset(view);
            //    if (result != null && result != Asset){
			//          Asset = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AssetIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAsset();
				//if (result != null && Asset != null){
				//	return !Asset.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator AssetCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Asset));
            }
        }
	
       
		//private Module.BusinessObjects.ProductListing _productlisting;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Niêm yết sản phẩm")]
        [ToolTip("Niêm yết sản phẩm")]
		//[Index(12)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductListingCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("ProductListing-BookMarkList")]
	 
		public Module.BusinessObjects.ProductListing ProductListing
        { 
		    get => GetPropertyValue<Module.BusinessObjects.ProductListing>("ProductListing");                         
			set => SetPropertyValue<Module.BusinessObjects.ProductListing>("ProductListing", value); 
			
        }
		//Tooltip for Object
		public object ProductListingToolTipControllerText(View view)
        {
        //    if (ProductListing != null) 
		//			return ProductListing;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.ProductListing GetDefaultProductListing(View view = null)
        { 
			return ProductListing;
        }
		//Set Default Value
		public void SetDefaultProductListing(View view = null)
        {
            //if (ProductListing is null){
            //    var result = GetDefaultProductListing(view);
            //    if (result != null && result != ProductListing){
			//          ProductListing = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ProductListingIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultProductListing();
				//if (result != null && ProductListing != null){
				//	return !ProductListing.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ProductListingCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(ProductListing));
            }
        }
	
       
		//private Module.BusinessObjects.Video _video;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tư liệu")]
        [ToolTip("Tư liệu")]
		//[Index(13)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(VideoCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Video-FileList")]
	 
		public Module.BusinessObjects.Video Video
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Video>("Video");                         
			set => SetPropertyValue<Module.BusinessObjects.Video>("Video", value); 
			
        }
		//Tooltip for Object
		public object VideoToolTipControllerText(View view)
        {
        //    if (Video != null) 
		//			return Video;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Video GetDefaultVideo(View view = null)
        { 
			return Video;
        }
		//Set Default Value
		public void SetDefaultVideo(View view = null)
        {
            //if (Video is null){
            //    var result = GetDefaultVideo(view);
            //    if (result != null && result != Video){
			//          Video = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool VideoIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultVideo();
				//if (result != null && Video != null){
				//	return !Video.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator VideoCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Video));
            }
        }
	
       
		//private Module.BusinessObjects.Member _member;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(14)]		
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
	
       
		//private Module.BusinessObjects.Folder _folder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(15)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(FolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-BookMarkList")]
	 
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
	
       
		//private int? _quantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(16)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Quantity
        { 
		    get => GetPropertyValue<int?>("Quantity");                         
			set => SetPropertyValue<int?>("Quantity", value); 
			
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

	
       
		//private Module.BusinessObjects.Product _product;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sản phẩm")]
        [ToolTip("Sản phẩm")]
		//[Index(17)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ProductCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Product-BookMarkList")]
	 
		public Module.BusinessObjects.Product Product
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Product>("Product");                         
			set => SetPropertyValue<Module.BusinessObjects.Product>("Product", value); 
			
        }
		//Tooltip for Object
		public object ProductToolTipControllerText(View view)
        {
        //    if (Product != null) 
		//			return Product;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Product GetDefaultProduct(View view = null)
        { 
			return Product;
        }
		//Set Default Value
		public void SetDefaultProduct(View view = null)
        {
            //if (Product is null){
            //    var result = GetDefaultProduct(view);
            //    if (result != null && result != Product){
			//          Product = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ProductIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultProduct();
				//if (result != null && Product != null){
				//	return !Product.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ProductCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Product));
            }
        }
	
       
		//private Module.BusinessObjects.Post _post;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bài viết")]
        [ToolTip("Bài viết")]
		//[Index(18)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PostCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Post-BookMarkList")]
	 
		public Module.BusinessObjects.Post Post
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Post>("Post");                         
			set => SetPropertyValue<Module.BusinessObjects.Post>("Post", value); 
			
        }
		//Tooltip for Object
		public object PostToolTipControllerText(View view)
        {
        //    if (Post != null) 
		//			return Post;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Post GetDefaultPost(View view = null)
        { 
			return Post;
        }
		//Set Default Value
		public void SetDefaultPost(View view = null)
        {
            //if (Post is null){
            //    var result = GetDefaultPost(view);
            //    if (result != null && result != Post){
			//          Post = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PostIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPost();
				//if (result != null && Post != null){
				//	return !Post.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PostCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Post));
            }
        }
	
       
		//private Module.BusinessObjects.Website _website;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Website")]
        [ToolTip("Website")]
		//[Index(19)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(WebsiteCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Website Website
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Website>("Website");                         
			set => SetPropertyValue<Module.BusinessObjects.Website>("Website", value); 
			
        }
		//Tooltip for Object
		public object WebsiteToolTipControllerText(View view)
        {
        //    if (Website != null) 
		//			return Website;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Website GetDefaultWebsite(View view = null)
        { 
			return Website;
        }
		//Set Default Value
		public void SetDefaultWebsite(View view = null)
        {
            //if (Website is null){
            //    var result = GetDefaultWebsite(view);
            //    if (result != null && result != Website){
			//          Website = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WebsiteIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWebsite();
				//if (result != null && Website != null){
				//	return !Website.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator WebsiteCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Website));
            }
        }
	
       
		//private bool _flag;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ")]
        [ToolTip("Cờ")]
		//[Index(20)]		
	    [NotMapped()]
	    [NonPersistent()]
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

	
       
		//private LinkType _linktype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(21)]		
		public LinkType LinkType
        { 
		    get => GetPropertyValue<LinkType>("LinkType");                         
			set => SetPropertyValue<LinkType>("LinkType", value); 
			
        }
		//Tooltip for Object
		public object LinkTypeToolTipControllerText(View view)
        {
        //    if (LinkType != null) 
		//			return LinkType;
            return null;
        }
		//Get Default Value
        public LinkType GetDefaultLinkType(View view = null)
        { 
			return LinkType;
        }
		//Set Default Value
		public void SetDefaultLinkType(View view = null)
        {
            //if (LinkType is null){
            //    var result = GetDefaultLinkType(view);
            //    if (result != null && result != LinkType){
			//          LinkType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LinkTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLinkType();
				//if (result != null && LinkType != null){
				//	return !LinkType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.WorkType _worktype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại công việc")]
        [ToolTip("Loại công việc")]
		//[Index(22)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(WorkTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("WorkType-BookMarkList")]
	 
		public Module.BusinessObjects.WorkType WorkType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.WorkType>("WorkType");                         
			set => SetPropertyValue<Module.BusinessObjects.WorkType>("WorkType", value); 
			
        }
		//Tooltip for Object
		public object WorkTypeToolTipControllerText(View view)
        {
        //    if (WorkType != null) 
		//			return WorkType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.WorkType GetDefaultWorkType(View view = null)
        { 
			return WorkType;
        }
		//Set Default Value
		public void SetDefaultWorkType(View view = null)
        {
            //if (WorkType is null){
            //    var result = GetDefaultWorkType(view);
            //    if (result != null && result != WorkType){
			//          WorkType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WorkTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWorkType();
				//if (result != null && WorkType != null){
				//	return !WorkType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator WorkTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(WorkType));
            }
        }
	
       
		//private System.Type _systemtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu hệ thống")]
        [ToolTip("Kiểu hệ thống")]
		//[Index(23)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SystemTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [TypeConverter(typeof(DevExpress.Persistent.Base.Security.SecurityTargetTypeConverter))]
	    [ValueConverter(typeof(DevExpress.ExpressApp.Utils.TypeToStringConverter))]
		public System.Type SystemType
        { 
		    get => GetPropertyValue<System.Type>("SystemType");                         
			set => SetPropertyValue<System.Type>("SystemType", value); 
			
        }
		//Tooltip for Object
		public object SystemTypeToolTipControllerText(View view)
        {
        //    if (SystemType != null) 
		//			return SystemType;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool SystemTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSystemType();
				//if (result != null && SystemType != null){
				//	return !SystemType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SystemTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SystemType));
            }
        }
	
       
		//private System.Guid _objectid;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã đối tượng")]
        [ToolTip("Mã đối tượng")]
		//[Index(24)]		
	    [ModelDefault("AllowEdit", "False")]
		public System.Guid ObjectID
        { 
		    get => GetPropertyValue<System.Guid>("ObjectID");                         
			set => SetPropertyValue<System.Guid>("ObjectID", value); 
			
        }
		//Tooltip for Object
		public object ObjectIDToolTipControllerText(View view)
        {
        //    if (ObjectID != null) 
		//			return ObjectID;
            return null;
        }
		//Get Default Value
        public System.Guid GetDefaultObjectID(View view = null)
        { 
			return ObjectID;
        }
		//Set Default Value
		public void SetDefaultObjectID(View view = null)
        {
            //if (ObjectID is null){
            //    var result = GetDefaultObjectID(view);
            //    if (result != null && result != ObjectID){
			//          ObjectID = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ObjectIDIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultObjectID();
				//if (result != null && ObjectID != null){
				//	return !ObjectID.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private SoftwareObjectType _softwareobjecttype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu đối tượng")]
        [ToolTip("Kiểu đối tượng")]
		//[Index(25)]		
		public SoftwareObjectType SoftwareObjectType
        { 
		    get => GetPropertyValue<SoftwareObjectType>("SoftwareObjectType");                         
			set => SetPropertyValue<SoftwareObjectType>("SoftwareObjectType", value); 
			
        }
		//Tooltip for Object
		public object SoftwareObjectTypeToolTipControllerText(View view)
        {
        //    if (SoftwareObjectType != null) 
		//			return SoftwareObjectType;
            return null;
        }
		//Get Default Value
        public SoftwareObjectType GetDefaultSoftwareObjectType(View view = null)
        { 
			return SoftwareObjectType;
        }
		//Set Default Value
		public void SetDefaultSoftwareObjectType(View view = null)
        {
            //if (SoftwareObjectType is null){
            //    var result = GetDefaultSoftwareObjectType(view);
            //    if (result != null && result != SoftwareObjectType){
			//          SoftwareObjectType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SoftwareObjectTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSoftwareObjectType();
				//if (result != null && SoftwareObjectType != null){
				//	return !SoftwareObjectType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.SourceCode _sourcecode;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã nguồn")]
        [ToolTip("Mã nguồn")]
		//[Index(26)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SourceCodeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("SourceCode-BookMarkList")]
	 
		public Module.BusinessObjects.SourceCode SourceCode
        { 
		    get => GetPropertyValue<Module.BusinessObjects.SourceCode>("SourceCode");                         
			set => SetPropertyValue<Module.BusinessObjects.SourceCode>("SourceCode", value); 
			
        }
		//Tooltip for Object
		public object SourceCodeToolTipControllerText(View view)
        {
        //    if (SourceCode != null) 
		//			return SourceCode;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.SourceCode GetDefaultSourceCode(View view = null)
        { 
			return SourceCode;
        }
		//Set Default Value
		public void SetDefaultSourceCode(View view = null)
        {
            //if (SourceCode is null){
            //    var result = GetDefaultSourceCode(view);
            //    if (result != null && result != SourceCode){
			//          SourceCode = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SourceCodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSourceCode();
				//if (result != null && SourceCode != null){
				//	return !SourceCode.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SourceCodeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SourceCode));
            }
        }
	
       
		//private string _xpath;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Xpath")]
        [ToolTip("Xpath")]
		//[Index(27)]		

 		[Size(250)]
		public string Xpath
        { 
		    get => GetPropertyValue<string>("Xpath");                         
			set => SetPropertyValue<string>("Xpath", value); 
			
        }
		//Tooltip for Object
		public object XpathToolTipControllerText(View view)
        {
        //    if (Xpath != null) 
		//			return Xpath;
            return null;
        }
		//Get Default Value
        public string GetDefaultXpath(View view = null)
        { 
			return Xpath;
        }
		//Set Default Value
		public void SetDefaultXpath(View view = null)
        {
            //if (Xpath is null){
            //    var result = GetDefaultXpath(view);
            //    if (result != null && result != Xpath){
			//          Xpath = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool XpathIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultXpath();
				//if (result != null && Xpath != null){
				//	return !Xpath.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _flag2;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ 2")]
        [ToolTip("Cờ 2")]
		//[Index(28)]		
		public bool Flag2
        { 
		    get => GetPropertyValue<bool>("Flag2");                         
			set => SetPropertyValue<bool>("Flag2", value); 
			
        }
		//Tooltip for Object
		public object Flag2ToolTipControllerText(View view)
        {
        //    if (Flag2 != null) 
		//			return Flag2;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFlag2(View view = null)
        { 
			return Flag2;
        }
		//Set Default Value
		public void SetDefaultFlag2(View view = null)
        {
            //if (Flag2 is null){
            //    var result = GetDefaultFlag2(view);
            //    if (result != null && result != Flag2){
			//          Flag2 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Flag2IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFlag2();
				//if (result != null && Flag2 != null){
				//	return !Flag2.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Recognition _recognition;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhận dạng")]
        [ToolTip("Nhận dạng")]
		//[Index(29)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(RecognitionCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Recognition-BookMarkList")]
	 
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
	
       
		//private Module.BusinessObjects.Space _space;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vị trí")]
        [ToolTip("Vị trí")]
		//[Index(30)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SpaceCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Space Space
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Space>("Space");                         
			set => SetPropertyValue<Module.BusinessObjects.Space>("Space", value); 
			
        }
		//Tooltip for Object
		public object SpaceToolTipControllerText(View view)
        {
        //    if (Space != null) 
		//			return Space;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Space GetDefaultSpace(View view = null)
        { 
			return Space;
        }
		//Set Default Value
		public void SetDefaultSpace(View view = null)
        {
            //if (Space is null){
            //    var result = GetDefaultSpace(view);
            //    if (result != null && result != Space){
			//          Space = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SpaceIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSpace();
				//if (result != null && Space != null){
				//	return !Space.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SpaceCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Space));
            }
        }
	
       
		//private Module.BusinessObjects.Invester _invester;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhà đầu tư")]
        [ToolTip("Nhà đầu tư")]
		//[Index(31)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(InvesterCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Invester-BookMarkList")]
	 
		public Module.BusinessObjects.Invester Invester
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Invester>("Invester");                         
			set => SetPropertyValue<Module.BusinessObjects.Invester>("Invester", value); 
			
        }
		//Tooltip for Object
		public object InvesterToolTipControllerText(View view)
        {
        //    if (Invester != null) 
		//			return Invester;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Invester GetDefaultInvester(View view = null)
        { 
			return Invester;
        }
		//Set Default Value
		public void SetDefaultInvester(View view = null)
        {
            //if (Invester is null){
            //    var result = GetDefaultInvester(view);
            //    if (result != null && result != Invester){
			//          Invester = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool InvesterIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultInvester();
				//if (result != null && Invester != null){
				//	return !Invester.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator InvesterCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Invester));
            }
        }
	
       
		//private Module.BusinessObjects.Company _company;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Công ty")]
        [ToolTip("Công ty")]
		//[Index(32)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CompanyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Company-BookMarkList")]
	 
		public Module.BusinessObjects.Company Company
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Company>("Company");                         
			set => SetPropertyValue<Module.BusinessObjects.Company>("Company", value); 
			
        }
		//Tooltip for Object
		public object CompanyToolTipControllerText(View view)
        {
        //    if (Company != null) 
		//			return Company;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Company GetDefaultCompany(View view = null)
        { 
			return Company;
        }
		//Set Default Value
		public void SetDefaultCompany(View view = null)
        {
            //if (Company is null){
            //    var result = GetDefaultCompany(view);
            //    if (result != null && result != Company){
			//          Company = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CompanyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCompany();
				//if (result != null && Company != null){
				//	return !Company.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CompanyCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Company));
            }
        }
	
       
		//private Module.BusinessObjects.AIExtractor _aiextractor;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trích AI")]
        [ToolTip("Trích AI")]
		//[Index(33)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(AIExtractorCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("AIExtractor-BookMarkList")]
	 
		public Module.BusinessObjects.AIExtractor AIExtractor
        { 
		    get => GetPropertyValue<Module.BusinessObjects.AIExtractor>("AIExtractor");                         
			set => SetPropertyValue<Module.BusinessObjects.AIExtractor>("AIExtractor", value); 
			
        }
		//Tooltip for Object
		public object AIExtractorToolTipControllerText(View view)
        {
        //    if (AIExtractor != null) 
		//			return AIExtractor;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.AIExtractor GetDefaultAIExtractor(View view = null)
        { 
			return AIExtractor;
        }
		//Set Default Value
		public void SetDefaultAIExtractor(View view = null)
        {
            //if (AIExtractor is null){
            //    var result = GetDefaultAIExtractor(view);
            //    if (result != null && result != AIExtractor){
			//          AIExtractor = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AIExtractorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAIExtractor();
				//if (result != null && AIExtractor != null){
				//	return !AIExtractor.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator AIExtractorCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(AIExtractor));
            }
        }
	
       
		//private Module.BusinessObjects.Equipment _equipment;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thiết bị")]
        [ToolTip("Thiết bị")]
		//[Index(34)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(EquipmentCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Equipment-BookMarkList")]
	 
		public Module.BusinessObjects.Equipment Equipment
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Equipment>("Equipment");                         
			set => SetPropertyValue<Module.BusinessObjects.Equipment>("Equipment", value); 
			
        }
		//Tooltip for Object
		public object EquipmentToolTipControllerText(View view)
        {
        //    if (Equipment != null) 
		//			return Equipment;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Equipment GetDefaultEquipment(View view = null)
        { 
			return Equipment;
        }
		//Set Default Value
		public void SetDefaultEquipment(View view = null)
        {
            //if (Equipment is null){
            //    var result = GetDefaultEquipment(view);
            //    if (result != null && result != Equipment){
			//          Equipment = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EquipmentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEquipment();
				//if (result != null && Equipment != null){
				//	return !Equipment.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator EquipmentCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Equipment));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 1072ImportCode
            base.AfterConstruction();
SetDefaultCreatedDate();
SetDefaultMember();
            #endregion 1072ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultURL(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultNote(View view = null);
        //SetDefaultCreatedDate(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultContact(View view = null);
        //SetDefaultOrg(View view = null);
        //SetDefaultAsset(View view = null);
        //SetDefaultProductListing(View view = null);
        //SetDefaultVideo(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultProduct(View view = null);
        //SetDefaultPost(View view = null);
        //SetDefaultWebsite(View view = null);
        //SetDefaultFlag(View view = null);
        //SetDefaultLinkType(View view = null);
        //SetDefaultWorkType(View view = null);
        //SetDefaultSystemType(View view = null);
        //SetDefaultObjectID(View view = null);
        //SetDefaultSoftwareObjectType(View view = null);
        //SetDefaultSourceCode(View view = null);
        //SetDefaultXpath(View view = null);
        //SetDefaultFlag2(View view = null);
        //SetDefaultRecognition(View view = null);
        //SetDefaultSpace(View view = null);
        //SetDefaultInvester(View view = null);
        //SetDefaultCompany(View view = null);
        //SetDefaultAIExtractor(View view = null);
        //SetDefaultEquipment(View view = null);
			
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
            #region 3176ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 3176ImportCode
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

                switch (propertyName)
                {       
				
                    case nameof(ProductListing):
                        OnChangedProductListing(oldValue, newValue);
                        break;
				
                    case nameof(URL):
                        OnChangedURL(oldValue, newValue);
                        break;
				
                    case nameof(Org):
                        OnChangedOrg(oldValue, newValue);
                        break;
				
                    case nameof(Contact):
                        OnChangedContact(oldValue, newValue);
                        break;
				
                    case nameof(Name):
                        OnChangedName(oldValue, newValue);
                        break;
				
                    case nameof(Post):
                        OnChangedPost(oldValue, newValue);
                        break;
				
                    case nameof(Folder):
                        OnChangedFolder(oldValue, newValue);
                        break;
				
                    case nameof(Product):
                        OnChangedProduct(oldValue, newValue);
                        break;
				
                    case nameof(Video):
                        OnChangedVideo(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedProductListing(object oldValue, object newValue)
        {
            #region 1352ImportCode
            if (newValue is null) return;
SystemType = typeof(ProductListing);
ObjectID = ProductListing.Oid;            
            #endregion 1352ImportCode
        }               
        private void OnChangedURL(object oldValue, object newValue)
        {
            #region 1080ImportCode
                                if (string.IsNullOrEmpty(URL)) return;
                    var newUrl = System.Web.HttpUtility.HtmlDecode(URL);
                    if (newUrl != URL)
                        URL = newUrl;
                    SetDefaultName();            
            #endregion 1080ImportCode
        }               
        private void OnChangedOrg(object oldValue, object newValue)
        {
            #region 1351ImportCode
            if (newValue is null) return;
SystemType = typeof(Org);
ObjectID = Org.Oid;            
            #endregion 1351ImportCode
        }               
        private void OnChangedContact(object oldValue, object newValue)
        {
            #region 1350ImportCode
            if (newValue is null) return;
SystemType = typeof(Contact);
ObjectID = Contact.Oid;            
            #endregion 1350ImportCode
        }               
        private void OnChangedName(object oldValue, object newValue)
        {
            #region 1310ImportCode
                                if (string.IsNullOrEmpty(Name))
                        return;
                    var newName = System.Web.HttpUtility.HtmlDecode(Name);
                    //Xử lý ký tự đặc biệt mã ASCII 160 giống dấu cách
                    newName = newName.Replace(" ", " "); 
                    if (newName != Name)
                        Name = newName;            
            #endregion 1310ImportCode
        }               
        private void OnChangedPost(object oldValue, object newValue)
        {
            #region 1354ImportCode
            if (newValue is null) return;
SystemType = typeof(Post);
ObjectID = Post.Oid;            
            #endregion 1354ImportCode
        }               
        private void OnChangedFolder(object oldValue, object newValue)
        {
            #region 1090ImportCode
            if (newValue is null) return;
SetDefaultOrder();
SetDefaultSystemType();
ObjectID = Folder.Oid;            
            #endregion 1090ImportCode
        }               
        private void OnChangedProduct(object oldValue, object newValue)
        {
            #region 1353ImportCode
            if (newValue is null) return;
SystemType = typeof(Product);
ObjectID = Product.Oid;            
            #endregion 1353ImportCode
        }               
        private void OnChangedVideo(object oldValue, object newValue)
        {
            #region 1084ImportCode
            if (newValue is null) return;
SetDefaultOrder();
SystemType = typeof(Video);
ObjectID = Video.Oid;            
            #endregion 1084ImportCode
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
			//	SetDefaultExtractorDataList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1082ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 1082            Oid: 71602fc1-d95f-4d0a-820e-3d7bbd731e22
            Order= GetDefaultOrder();
        }
#endregion 1082ImportCode
#region 1081ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 1081            Oid: d479b0ba-9ff3-429b-95d6-177b1d63d574
                        var type = this.GetType();
            var folderMember = type.GetProperty("Folder");
            if (folderMember != null)
            {
                var parentObjectObject = folderMember.GetValue(this);
                if (parentObjectObject != null)
                {
                    //var fileListMember = videoObject.GetPropertyValue("FileList");
                    var list = parentObjectObject.GetPropertyValue("BookMarkList") as XPCollection<BookMark>;
                    var lasted = list.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
                    if (lasted != null)
                        return lasted.Order + 1;
                    return 1;
                }

            }
            var videoMember = type.GetProperty("Video");
            if (videoMember != null)
            {
                var parentObjectObject = videoMember.GetValue(this);
                if (parentObjectObject != null)
                {
                    //var fileListMember = videoObject.GetPropertyValue("FileList");
                    var list = parentObjectObject.GetPropertyValue("FileList") as XPCollection<BookMark>;
                    var lasted = list.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
                    if (lasted != null)
                        return lasted.Order + 1;
                    return 1;
                }

            }
            return null;
        }
#endregion 1081ImportCode
#region 3436ImportCode
		public void SetDefaultSystemType(View view = null)
        {
            //Code: 3436            Oid: 033aca3c-874e-4ecf-93a2-1c350d71526b
            SystemType= GetDefaultSystemType();
        }
#endregion 3436ImportCode
#region 3177ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 3177            Oid: fd2678dd-baa8-4720-84d3-1e3ffc94bd35
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 3177ImportCode
#region 1079ImportCode
		public string GetDefaultName(View view = null)
        {
            //Code: 1079            Oid: 1b19f5f5-bfea-41c3-8f32-c3feec593bf6
            if (!string.IsNullOrEmpty(URL))
{
    var fileInfo = new System.IO.FileInfo(URL);
    return fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
}
return null;
        }
#endregion 1079ImportCode
#region 2599ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 2599            Oid: 0da8e180-11a0-45b6-8657-e188abd75351
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 2599ImportCode
#region 3178ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 3178            Oid: ec57a5c0-4e85-4bf3-924f-775a09fa8fc0
            Updater = GetDefaultUpdater();
        }
#endregion 3178ImportCode
#region 1070ImportCode
		public DateTime? GetDefaultCreatedDate(View view = null)
        {
            //Code: 1070            Oid: 23c6d2cf-797e-4177-a322-f5cca19538f1
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1070ImportCode
#region 2600ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 2600            Oid: c91479f4-d949-484c-9115-1109eebe6a63
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2600ImportCode
#region 1071ImportCode
		public void SetDefaultCreatedDate(View view = null)
        {
            //Code: 1071            Oid: 9526aec8-05e5-4fee-8b73-32434006850f
            if(CreatedDate == null) CreatedDate = GetDefaultCreatedDate();
        }
#endregion 1071ImportCode
#region 3179ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 3179            Oid: b029443f-6b6e-4c7e-8c25-4b31ffc3b421
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3179ImportCode
#region 1338ImportCode
		public Type GetDefaultSystemType(View view = null)
        {
            //Code: 1338            Oid: f1907a69-9f77-4591-9d04-debea4e84fda
            if(Folder != null && Folder.SystemType != null)
return Folder.SystemType;
return null;
        }
#endregion 1338ImportCode
#region 1078ImportCode
		public void SetDefaultName(View view = null)
        {
            //Code: 1078            Oid: 3017ce85-be57-411c-b227-9a91b0725809
            if(String.IsNullOrEmpty(Name)) Name = GetDefaultName();

        }
#endregion 1078ImportCode
#region 3175ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 3175            Oid: 6b75c633-ba17-48b8-a70d-31fb687dc94e
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 3175ImportCode
        #endregion
//Mã nguồn bổ sung
#region BookMarkImportCode
#region IOpenLinkInBrowser
[Browsable(false)]
public bool ShowOpenHyperLink
{
    get
    {
        if (!string.IsNullOrEmpty(URL))
            return true;
        return false;
    }
}

public bool ReCreateChoice()
{
    return true;
}
public DevExpress.ExpressApp.Actions.ChoiceActionItem[] ChoiceActionLinks(View currentView)
{
        var result = new System.Collections.Generic.List<DevExpress.ExpressApp.Actions.ChoiceActionItem>();
		result.Add(new DevExpress.ExpressApp.Actions.ChoiceActionItem("Mở Web", "OpenURL"));
        return result.ToArray();
}

public string[] HyperLinks(View currentView, DevExpress.ExpressApp.Actions.ChoiceActionItem choiceItem)
{
    return new string[] { URL};
    return null;
}
#endregion

        public void SetBookMarkNote(string noteValue) 
        {
            if (!string.IsNullOrEmpty(Note))
                noteValue += System.Environment.NewLine;
            Note += noteValue;
        }

        public string[] GetBookMarkNote()
        {
            if (!string.IsNullOrEmpty(Note))
                return Note.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return null;
        }  
     
        public string GetOrderCode(int length  = 2)
        {
            string result = null;
            if(Order != null)
            {
                result = Order.Value.ToString("D");
                while(result.Length < length)
                    result = "0" + result;        
            }
            return result;
        }
#endregion BookMarkImportCode
		 		 
    }
}
