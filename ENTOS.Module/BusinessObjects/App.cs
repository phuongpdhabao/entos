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
	[NavigationItem("ApplicationBusiness")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Ứng dụng"), ImageName("App")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(Update))]
 
	[MobileColumnAttribute(Context = "App_LookupListView", TargetItems = nameof(Image)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_AppList_ListView", TargetItems = nameof(Name)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "App_ListView", TargetItems = nameof(Folder)+ "," + nameof(Image)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "AppGroup_AppList_ListView", TargetItems = nameof(Name)+ "," + nameof(Image))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class App:  DevExpress.Xpo.XPLiteObject , IObjectImage, INewObjectSession , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public App(Session session)
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
				if (AppPriceList.IsLoaded)
                {
                    if (AppPriceList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(AppPriceList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(AppPriceList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.AppPrice>(CriteriaOperator.Parse("[App.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool apppricelist = Session.Query<Module.BusinessObjects.AppPrice>().Where(x => x.App.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(AppPriceList), apppricelist);
                        if (apppricelist)
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
		[RuleRequiredField("RequiredAppName", DefaultContexts.Save)]
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

	
       
		//private string _homepage;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Địa chỉ")]
        [ToolTip("Địa chỉ")]
		//[Index(1)]		

 		[Size(230)]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string HomePage
        { 
		    get => GetPropertyValue<string>("HomePage");                         
			set => SetPropertyValue<string>("HomePage", value); 
			
        }
		//Tooltip for Object
		public object HomePageToolTipControllerText(View view)
        {
        //    if (HomePage != null) 
		//			return HomePage;
            return null;
        }
		//Get Default Value
        public string GetDefaultHomePage(View view = null)
        { 
			return HomePage;
        }
		//Set Default Value
		public void SetDefaultHomePage(View view = null)
        {
            //if (HomePage is null){
            //    var result = GetDefaultHomePage(view);
            //    if (result != null && result != HomePage){
			//          HomePage = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool HomePageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultHomePage();
				//if (result != null && HomePage != null){
				//	return !HomePage.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Folder _folder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#App# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-AppList")]
	 
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
	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Biểu tượng")]
        [ToolTip("Biểu tượng")]
		//[Index(3)]		
		[Appearance("Biểu tượngBackground", BackColor = "Transparent")]
	
		[ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 200)] 
	
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

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
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
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá")]
		//[Index(5)]
		[DevExpress.Xpo.Association("App-AppPriceList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.AppPrice> AppPriceList
        {      
		    get => GetCollection<Module.BusinessObjects.AppPrice>("AppPriceList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhóm")]
		//[Index(6)]
		[DataSourceCriteria("Not AppList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("AppGroupList-AppList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.AppGroup> AppGroupList
        {      
		    get => GetCollection<Module.BusinessObjects.AppGroup>("AppGroupList"); 
			
        }
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(7)]		
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

	
       
		//private string _xpathusername;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Username")]
        [ToolTip("Username")]
		//[Index(8)]		

 		[Size(200)]
		public string XpathUsername
        { 
		    get => GetPropertyValue<string>("XpathUsername");                         
			set => SetPropertyValue<string>("XpathUsername", value); 
			
        }
		//Tooltip for Object
		public object XpathUsernameToolTipControllerText(View view)
        {
        //    if (XpathUsername != null) 
		//			return XpathUsername;
            return null;
        }
		//Get Default Value
        public string GetDefaultXpathUsername(View view = null)
        { 
			return XpathUsername;
        }
		//Set Default Value
		public void SetDefaultXpathUsername(View view = null)
        {
            //if (XpathUsername is null){
            //    var result = GetDefaultXpathUsername(view);
            //    if (result != null && result != XpathUsername){
			//          XpathUsername = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool XpathUsernameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultXpathUsername();
				//if (result != null && XpathUsername != null){
				//	return !XpathUsername.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _xpathcontinue;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Continue")]
        [ToolTip("Continue")]
		//[Index(9)]		

 		[Size(200)]
		public string XpathContinue
        { 
		    get => GetPropertyValue<string>("XpathContinue");                         
			set => SetPropertyValue<string>("XpathContinue", value); 
			
        }
		//Tooltip for Object
		public object XpathContinueToolTipControllerText(View view)
        {
        //    if (XpathContinue != null) 
		//			return XpathContinue;
            return null;
        }
		//Get Default Value
        public string GetDefaultXpathContinue(View view = null)
        { 
			return XpathContinue;
        }
		//Set Default Value
		public void SetDefaultXpathContinue(View view = null)
        {
            //if (XpathContinue is null){
            //    var result = GetDefaultXpathContinue(view);
            //    if (result != null && result != XpathContinue){
			//          XpathContinue = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool XpathContinueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultXpathContinue();
				//if (result != null && XpathContinue != null){
				//	return !XpathContinue.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _xpathpassword;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Password")]
        [ToolTip("Password")]
		//[Index(10)]		

 		[Size(200)]
		public string XpathPassword
        { 
		    get => GetPropertyValue<string>("XpathPassword");                         
			set => SetPropertyValue<string>("XpathPassword", value); 
			
        }
		//Tooltip for Object
		public object XpathPasswordToolTipControllerText(View view)
        {
        //    if (XpathPassword != null) 
		//			return XpathPassword;
            return null;
        }
		//Get Default Value
        public string GetDefaultXpathPassword(View view = null)
        { 
			return XpathPassword;
        }
		//Set Default Value
		public void SetDefaultXpathPassword(View view = null)
        {
            //if (XpathPassword is null){
            //    var result = GetDefaultXpathPassword(view);
            //    if (result != null && result != XpathPassword){
			//          XpathPassword = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool XpathPasswordIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultXpathPassword();
				//if (result != null && XpathPassword != null){
				//	return !XpathPassword.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _capcha;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Capcha")]
        [ToolTip("Capcha")]
		//[Index(11)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Capcha
        { 
		    get => GetPropertyValue<int?>("Capcha");                         
			set => SetPropertyValue<int?>("Capcha", value); 
			
        }
		//Tooltip for Object
		public object CapchaToolTipControllerText(View view)
        {
        //    if (Capcha != null) 
		//			return Capcha;
            return null;
        }
		//Get Default Value
        public int? GetDefaultCapcha(View view = null)
        { 
			return Capcha;
        }
		//Set Default Value
		public void SetDefaultCapcha(View view = null)
        {
            //if (Capcha is null){
            //    var result = GetDefaultCapcha(view);
            //    if (result != null && result != Capcha){
			//          Capcha = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CapchaIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCapcha();
				//if (result != null && Capcha != null){
				//	return !Capcha.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _xpathlogin;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Login")]
        [ToolTip("Login")]
		//[Index(12)]		

 		[Size(200)]
		public string XpathLogin
        { 
		    get => GetPropertyValue<string>("XpathLogin");                         
			set => SetPropertyValue<string>("XpathLogin", value); 
			
        }
		//Tooltip for Object
		public object XpathLoginToolTipControllerText(View view)
        {
        //    if (XpathLogin != null) 
		//			return XpathLogin;
            return null;
        }
		//Get Default Value
        public string GetDefaultXpathLogin(View view = null)
        { 
			return XpathLogin;
        }
		//Set Default Value
		public void SetDefaultXpathLogin(View view = null)
        {
            //if (XpathLogin is null){
            //    var result = GetDefaultXpathLogin(view);
            //    if (result != null && result != XpathLogin){
			//          XpathLogin = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool XpathLoginIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultXpathLogin();
				//if (result != null && XpathLogin != null){
				//	return !XpathLogin.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _xpathframe;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Frame")]
        [ToolTip("Frame")]
		//[Index(13)]		

 		[Size(200)]
		public string XpathFrame
        { 
		    get => GetPropertyValue<string>("XpathFrame");                         
			set => SetPropertyValue<string>("XpathFrame", value); 
			
        }
		//Tooltip for Object
		public object XpathFrameToolTipControllerText(View view)
        {
        //    if (XpathFrame != null) 
		//			return XpathFrame;
            return null;
        }
		//Get Default Value
        public string GetDefaultXpathFrame(View view = null)
        { 
			return XpathFrame;
        }
		//Set Default Value
		public void SetDefaultXpathFrame(View view = null)
        {
            //if (XpathFrame is null){
            //    var result = GetDefaultXpathFrame(view);
            //    if (result != null && result != XpathFrame){
			//          XpathFrame = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool XpathFrameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultXpathFrame();
				//if (result != null && XpathFrame != null){
				//	return !XpathFrame.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(14)]		
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 0423ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
SetDefaultMember();
            #endregion 0423ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultHomePage(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultXpathUsername(View view = null);
        //SetDefaultXpathContinue(View view = null);
        //SetDefaultXpathPassword(View view = null);
        //SetDefaultCapcha(View view = null);
        //SetDefaultXpathLogin(View view = null);
        //SetDefaultXpathFrame(View view = null);
        //SetDefaultInActive(View view = null);
			
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
            #region 0519ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0519ImportCode
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
			//	SetDefaultAppPriceList();
			//	SetDefaultAppGroupList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1576ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 1576            Oid: 5e79f921-4288-4066-99ea-c8e7d357a824
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 1576ImportCode
#region 0073ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0073            Oid: 503fac16-3bf4-4d20-a3f3-5b6859708e82
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0073ImportCode
#region 1577ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 1577            Oid: f265df46-d78d-48e6-b4be-df4bf8c5b849
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 1577ImportCode
#region 0140ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0140            Oid: a4f858d6-7550-49e1-ab1f-969c9c1af149
            Update = GetDefaultUpdate();
        }
#endregion 0140ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
