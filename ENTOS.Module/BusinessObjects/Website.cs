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
    [ModelDefault("Caption", "Website"), ImageName("Website")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(Update))]
 
	[MobileColumnAttribute(Context = "Website_LookupListView", TargetItems = nameof(Name)+ "," + nameof(Update)+ "," + nameof(Icon))]
	[MobileColumnAttribute(Context = "Website_ListView", TargetItems = nameof(URL)+ "," + nameof(Icon)+ "," + nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Website:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Website(Session session)
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

	
       
		//private string _title;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhãn")]
        [ToolTip("Nhãn")]
		//[Index(1)]		

 		[Size(250)]
		public string Title
        { 
		    get => GetPropertyValue<string>("Title");                         
			set => SetPropertyValue<string>("Title", value); 
			
        }
		//Tooltip for Object
		public object TitleToolTipControllerText(View view)
        {
        //    if (Title != null) 
		//			return Title;
            return null;
        }
		//Get Default Value
        public string GetDefaultTitle(View view = null)
        { 
			return Title;
        }
		//Set Default Value
		public void SetDefaultTitle(View view = null)
        {
            //if (Title is null){
            //    var result = GetDefaultTitle(view);
            //    if (result != null && result != Title){
			//          Title = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TitleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTitle();
				//if (result != null && Title != null){
				//	return !Title.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _url;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Địa chỉ")]
        [ToolTip("Địa chỉ")]
		//[Index(2)]		

 		[Size(250)]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string URL
        { 
		    get => GetPropertyValue<string>("URL");                         
			set => SetPropertyValue<string>("URL", value); 
			
        }
		//Tooltip for Object
		public object URLToolTipControllerText(View view)
        {
        //    if (URL != null) 
		//			return URL;
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

	
       
		//private string _path;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đường dẫn")]
        [ToolTip("Đường dẫn")]
		//[Index(3)]		

 		[Size(250)]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string Path
        { 
		    get => GetPropertyValue<string>("Path");                         
			set => SetPropertyValue<string>("Path", value); 
			
        }
		//Tooltip for Object
		public object PathToolTipControllerText(View view)
        {
        //    if (Path != null) 
		//			return Path;
            return null;
        }
		//Get Default Value
        public string GetDefaultPath(View view = null)
        { 
			return Path;
        }
		//Set Default Value
		public void SetDefaultPath(View view = null)
        {
            //if (Path is null){
            //    var result = GetDefaultPath(view);
            //    if (result != null && result != Path){
			//          Path = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PathIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPath();
				//if (result != null && Path != null){
				//	return !Path.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(4)]		
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
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(FolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
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
	
       
		//private Module.BusinessObjects.Website _templatewebsite;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mẫu")]
        [ToolTip("Mẫu")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TemplateWebsiteCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Website TemplateWebsite
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Website>("TemplateWebsite");                         
			set => SetPropertyValue<Module.BusinessObjects.Website>("TemplateWebsite", value); 
			
        }
		//Tooltip for Object
		public object TemplateWebsiteToolTipControllerText(View view)
        {
        //    if (TemplateWebsite != null) 
		//			return TemplateWebsite;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Website GetDefaultTemplateWebsite(View view = null)
        { 
			return TemplateWebsite;
        }
		//Set Default Value
		public void SetDefaultTemplateWebsite(View view = null)
        {
            //if (TemplateWebsite is null){
            //    var result = GetDefaultTemplateWebsite(view);
            //    if (result != null && result != TemplateWebsite){
			//          TemplateWebsite = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TemplateWebsiteIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTemplateWebsite();
				//if (result != null && TemplateWebsite != null){
				//	return !TemplateWebsite.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TemplateWebsiteCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(TemplateWebsite));
            }
        }
	
       
		//private byte[] _icon;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Biểu tượng")]
        [ToolTip("Biểu tượng")]
		//[Index(7)]		
		[Appearance("Biểu tượngBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Icon
        { 
		    get => GetPropertyValue<byte[]>("Icon");                         
			set => SetPropertyValue<byte[]>("Icon", value); 
			
        }
		//Tooltip for Object
		public object IconToolTipControllerText(View view)
        {
        //    if (Icon != null) 
		//			return Icon;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultIcon(View view = null)
        { 
			return Icon;
        }
		//Set Default Value
		public void SetDefaultIcon(View view = null)
        {
            //if (Icon is null){
            //    var result = GetDefaultIcon(view);
            //    if (result != null && result != Icon){
			//          Icon = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool IconIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultIcon();
				//if (result != null && Icon != null){
				//	return !Icon.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _databasename;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên CSDL")]
        [ToolTip("Tên CSDL")]
		//[Index(8)]		

 		[Size(100)]
		public string DatabaseName
        { 
		    get => GetPropertyValue<string>("DatabaseName");                         
			set => SetPropertyValue<string>("DatabaseName", value); 
			
        }
		//Tooltip for Object
		public object DatabaseNameToolTipControllerText(View view)
        {
        //    if (DatabaseName != null) 
		//			return DatabaseName;
            return null;
        }
		//Get Default Value
        public string GetDefaultDatabaseName(View view = null)
        { 
			return DatabaseName;
        }
		//Set Default Value
		public void SetDefaultDatabaseName(View view = null)
        {
            //if (DatabaseName is null){
            //    var result = GetDefaultDatabaseName(view);
            //    if (result != null && result != DatabaseName){
			//          DatabaseName = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DatabaseNameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDatabaseName();
				//if (result != null && DatabaseName != null){
				//	return !DatabaseName.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.LoginAccount _loginaccountdb;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đăng nhập CSDL")]
        [ToolTip("Đăng nhập CSDL")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LoginAccountDBCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.LoginAccount LoginAccountDB
        { 
		    get => GetPropertyValue<Module.BusinessObjects.LoginAccount>("LoginAccountDB");                         
			set => SetPropertyValue<Module.BusinessObjects.LoginAccount>("LoginAccountDB", value); 
			
        }
		//Tooltip for Object
		public object LoginAccountDBToolTipControllerText(View view)
        {
        //    if (LoginAccountDB != null) 
		//			return LoginAccountDB;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.LoginAccount GetDefaultLoginAccountDB(View view = null)
        { 
			return LoginAccountDB;
        }
		//Set Default Value
		public void SetDefaultLoginAccountDB(View view = null)
        {
            //if (LoginAccountDB is null){
            //    var result = GetDefaultLoginAccountDB(view);
            //    if (result != null && result != LoginAccountDB){
			//          LoginAccountDB = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LoginAccountDBIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLoginAccountDB();
				//if (result != null && LoginAccountDB != null){
				//	return !LoginAccountDB.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LoginAccountDBCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(LoginAccountDB));
            }
        }
	
       
		//private Module.BusinessObjects.LoginAccount _loginaccountwebserver;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đăng nhập WS")]
        [ToolTip("Đăng nhập WS")]
		//[Index(10)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LoginAccountWebServerCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.LoginAccount LoginAccountWebServer
        { 
		    get => GetPropertyValue<Module.BusinessObjects.LoginAccount>("LoginAccountWebServer");                         
			set => SetPropertyValue<Module.BusinessObjects.LoginAccount>("LoginAccountWebServer", value); 
			
        }
		//Tooltip for Object
		public object LoginAccountWebServerToolTipControllerText(View view)
        {
        //    if (LoginAccountWebServer != null) 
		//			return LoginAccountWebServer;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.LoginAccount GetDefaultLoginAccountWebServer(View view = null)
        { 
			return LoginAccountWebServer;
        }
		//Set Default Value
		public void SetDefaultLoginAccountWebServer(View view = null)
        {
            //if (LoginAccountWebServer is null){
            //    var result = GetDefaultLoginAccountWebServer(view);
            //    if (result != null && result != LoginAccountWebServer){
			//          LoginAccountWebServer = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LoginAccountWebServerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLoginAccountWebServer();
				//if (result != null && LoginAccountWebServer != null){
				//	return !LoginAccountWebServer.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LoginAccountWebServerCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(LoginAccountWebServer));
            }
        }
	
       
		//private string _wordpressuser;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("WP User")]
        [ToolTip("WP User")]
		//[Index(11)]		

 		[Size(250)]
		public string WordpressUser
        { 
		    get => GetPropertyValue<string>("WordpressUser");                         
			set => SetPropertyValue<string>("WordpressUser", value); 
			
        }
		//Tooltip for Object
		public object WordpressUserToolTipControllerText(View view)
        {
        //    if (WordpressUser != null) 
		//			return WordpressUser;
            return null;
        }
		//Get Default Value
        public string GetDefaultWordpressUser(View view = null)
        { 
			return WordpressUser;
        }
		//Set Default Value
		public void SetDefaultWordpressUser(View view = null)
        {
            //if (WordpressUser is null){
            //    var result = GetDefaultWordpressUser(view);
            //    if (result != null && result != WordpressUser){
			//          WordpressUser = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WordpressUserIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWordpressUser();
				//if (result != null && WordpressUser != null){
				//	return !WordpressUser.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _wordpresskey;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("WP Key")]
        [ToolTip("WP Key")]
		//[Index(12)]		

 		[Size(250)]
		public string WordpressKey
        { 
		    get => GetPropertyValue<string>("WordpressKey");                         
			set => SetPropertyValue<string>("WordpressKey", value); 
			
        }
		//Tooltip for Object
		public object WordpressKeyToolTipControllerText(View view)
        {
        //    if (WordpressKey != null) 
		//			return WordpressKey;
            return null;
        }
		//Get Default Value
        public string GetDefaultWordpressKey(View view = null)
        { 
			return WordpressKey;
        }
		//Set Default Value
		public void SetDefaultWordpressKey(View view = null)
        {
            //if (WordpressKey is null){
            //    var result = GetDefaultWordpressKey(view);
            //    if (result != null && result != WordpressKey){
			//          WordpressKey = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WordpressKeyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWordpressKey();
				//if (result != null && WordpressKey != null){
				//	return !WordpressKey.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _woocommerceuser;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("WC User")]
        [ToolTip("WC User")]
		//[Index(13)]		

 		[Size(250)]
		public string WooCommerceUser
        { 
		    get => GetPropertyValue<string>("WooCommerceUser");                         
			set => SetPropertyValue<string>("WooCommerceUser", value); 
			
        }
		//Tooltip for Object
		public object WooCommerceUserToolTipControllerText(View view)
        {
        //    if (WooCommerceUser != null) 
		//			return WooCommerceUser;
            return null;
        }
		//Get Default Value
        public string GetDefaultWooCommerceUser(View view = null)
        { 
			return WooCommerceUser;
        }
		//Set Default Value
		public void SetDefaultWooCommerceUser(View view = null)
        {
            //if (WooCommerceUser is null){
            //    var result = GetDefaultWooCommerceUser(view);
            //    if (result != null && result != WooCommerceUser){
			//          WooCommerceUser = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WooCommerceUserIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWooCommerceUser();
				//if (result != null && WooCommerceUser != null){
				//	return !WooCommerceUser.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _woocommercekey;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("WC Key")]
        [ToolTip("WC Key")]
		//[Index(14)]		

 		[Size(250)]
		public string WooCommerceKey
        { 
		    get => GetPropertyValue<string>("WooCommerceKey");                         
			set => SetPropertyValue<string>("WooCommerceKey", value); 
			
        }
		//Tooltip for Object
		public object WooCommerceKeyToolTipControllerText(View view)
        {
        //    if (WooCommerceKey != null) 
		//			return WooCommerceKey;
            return null;
        }
		//Get Default Value
        public string GetDefaultWooCommerceKey(View view = null)
        { 
			return WooCommerceKey;
        }
		//Set Default Value
		public void SetDefaultWooCommerceKey(View view = null)
        {
            //if (WooCommerceKey is null){
            //    var result = GetDefaultWooCommerceKey(view);
            //    if (result != null && result != WooCommerceKey){
			//          WooCommerceKey = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WooCommerceKeyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWooCommerceKey();
				//if (result != null && WooCommerceKey != null){
				//	return !WooCommerceKey.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(16)]		
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

	
       
		//private Module.BusinessObjects.Website _websitetemplate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mẫu")]
        [ToolTip("Mẫu")]
		//[Index(17)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(WebsiteTemplateCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Website WebsiteTemplate
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Website>("WebsiteTemplate");                         
			set => SetPropertyValue<Module.BusinessObjects.Website>("WebsiteTemplate", value); 
			
        }
		//Tooltip for Object
		public object WebsiteTemplateToolTipControllerText(View view)
        {
        //    if (WebsiteTemplate != null) 
		//			return WebsiteTemplate;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Website GetDefaultWebsiteTemplate(View view = null)
        { 
			return WebsiteTemplate;
        }
		//Set Default Value
		public void SetDefaultWebsiteTemplate(View view = null)
        {
            //if (WebsiteTemplate is null){
            //    var result = GetDefaultWebsiteTemplate(view);
            //    if (result != null && result != WebsiteTemplate){
			//          WebsiteTemplate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool WebsiteTemplateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultWebsiteTemplate();
				//if (result != null && WebsiteTemplate != null){
				//	return !WebsiteTemplate.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator WebsiteTemplateCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(WebsiteTemplate));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 1382ImportCode
            base.AfterConstruction();
SetDefaultMember();
            #endregion 1382ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultTitle(View view = null);
        //SetDefaultURL(View view = null);
        //SetDefaultPath(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultTemplateWebsite(View view = null);
        //SetDefaultIcon(View view = null);
        //SetDefaultDatabaseName(View view = null);
        //SetDefaultLoginAccountDB(View view = null);
        //SetDefaultLoginAccountWebServer(View view = null);
        //SetDefaultWordpressUser(View view = null);
        //SetDefaultWordpressKey(View view = null);
        //SetDefaultWooCommerceUser(View view = null);
        //SetDefaultWooCommerceKey(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultWebsiteTemplate(View view = null);
			
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
            #region 1379ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 1379ImportCode
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
			//	SetDefaultWebsiteElementList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1383ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 1383            Oid: 823c13dd-98d0-4d11-aa65-19c0eae34833
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 1383ImportCode
#region 1378ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 1378            Oid: 35ddce89-6973-44a9-b555-673d295c6ad3
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 1378ImportCode
#region 1380ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 1380            Oid: 53a936c0-16d9-4922-b8b0-9098090bbac5
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1380ImportCode
#region 1381ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 1381            Oid: 1a4ec5ed-2a06-44bc-bce2-8db9b535e35a
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 1381ImportCode
        #endregion
//Mã nguồn bổ sung
#region WebsiteImportCode
        public const string CurrentWebsiteIsNullException = "Website bị trống";
        public const string CurrentWebsiteUrlIsEmptyException = "Địa chỉ Website bị trống";
#endregion WebsiteImportCode
		 		 
    }
}
