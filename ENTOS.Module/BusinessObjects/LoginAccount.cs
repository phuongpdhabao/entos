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
    [ModelDefault("Caption", "Đăng nhập"), ImageName("LoginAccount")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("LoginAccount AppName IsNotValidate" , TargetItems = "AppName" , Criteria = "AppNameIsNotValidate",AppearanceItemType = "ViewItem", FontColor = "#FFA500" )]
	[Appearance("LoginAccount Label None_None__Color [A=255, R=0, G=0, B=255]" , TargetItems = "Label" , Criteria = "[MemberList][]",AppearanceItemType = "ViewItem", FontColor = "#0000FF" )]
	[Appearance("LoginAccount Name None_None__Color [A=255, R=192, G=192, B=192]" , TargetItems = "Name" , Criteria = "[HighLight] = -2",AppearanceItemType = "ViewItem", FontColor = "#C0C0C0" )]
	[Appearance("LoginAccount Password None_Disable__" , TargetItems = "Password" , Criteria = "[Member.Oid] <> CURRENTUSERID()",AppearanceItemType = "ViewItem", Enabled = false )]
	[Appearance("LoginAccount MemberList None_Disable__" , TargetItems = "MemberList" , Criteria = "[Member.Oid] <> CURRENTUSERID() And Not [MemberList][[Oid] = CURRENTUSERID()]",AppearanceItemType = "ViewItem", Context = "DetailView" , Enabled = false )]
	[Appearance("LoginAccount Member None_Disable__" , TargetItems = "Member" , Criteria = "!IsNewObject(this)",AppearanceItemType = "ViewItem", Context = "DetailView" , Enabled = false )]
	[Appearance("LoginAccount Name None_None_Color [A=0, R=0, G=0, B=0]_Color [A=255, R=0, G=255, B=0]" , TargetItems = "Name" , Criteria = "[HighLight] = 1",AppearanceItemType = "ViewItem", Context = "ListView" , BackColor = "#000000" , FontColor = "#00FF00" )]
	[Appearance("LoginAccount Name None_None__Color [A=255, R=255, G=0, B=0]" , TargetItems = "Name" , Criteria = "[HighLight] = -1",AppearanceItemType = "ViewItem", FontColor = "#FF0000" )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(Folder), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(Label)+ "," + nameof(Folder)+ "," + nameof(Update))]
 
	[MobileColumnAttribute(Context = "Member_LoginAccountList_ListView", TargetItems = nameof(Update))]
	[MobileColumnAttribute(Context = "Folder_LoginAccountList_ListView", TargetItems = nameof(Label)+ "," + nameof(Update))]
	[MobileColumnAttribute(Context = "LoginAccount_LookupListView", TargetItems = nameof(Label)+ "," + nameof(Update))]
	[MobileColumnAttribute(Context = "LoginAccount_ListView", TargetItems = nameof(Label)+ "," + nameof(Member))]
	[DefaultProperty("Label")]
 
	[CustomFilter("IFilterOwner", "Member.Oid = CurrentUserId() Or MemberList[Oid = CurrentUserId()]")]
[OptimisticLocking(true)]
    public partial class LoginAccount:  DevExpress.Xpo.XPLiteObject , IFilterOwner, INewObjectSession , INoIndexColumn, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public LoginAccount(Session session)
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
               

		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thành viên")]
        [ToolTip("Thành viên")]
		//[Index(0)]		
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
	
       
		//private string _appname;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ứng dụng")]
        [ToolTip("Ứng dụng")]
		//[Index(1)]		

 		[Size(100)]
	    [ModelDefault("PropertyEditorType", "StringComboEditor")]
	    [DataSourceProperty("AppDataSource")]
		public string AppName
        { 
		    get => GetPropertyValue<string>("AppName");                         
			set => SetPropertyValue<string>("AppName", value); 
			
        }
		//Tooltip for Object
		public object AppNameToolTipControllerText(View view)
        {
        //    if (AppName != null) 
		//			return AppName;
            return null;
        }
		//Get Default Value
        public string GetDefaultAppName(View view = null)
        { 
			return AppName;
        }
		//Set Default Value
		public void SetDefaultAppName(View view = null)
        {
            //if (AppName is null){
            //    var result = GetDefaultAppName(view);
            //    if (result != null && result != AppName){
			//          AppName = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AppNameIsNotValidate
        {
            get
            {
			#region 1045ImportCode 
 if (!string.IsNullOrEmpty(AppName))
                {
                    return Session.FindObject<Module.BusinessObjects.App>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ?", AppName)) is null;
                }
#endregion 1045ImportCode                
   
                return false;
            }
        }

	
       
		//private string _label;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(2)]		

 		[Size(150)]
		[RuleUniqueValue("UniqueLoginAccountLabel", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredLoginAccountLabel", DefaultContexts.Save)]
		public string Label
        { 
		    get => GetPropertyValue<string>("Label");                         
			set => SetPropertyValue<string>("Label", value); 
			
        }
		//Tooltip for Object
		public object LabelToolTipControllerText(View view)
        {
        //    if (Label != null) 
		//			return Label;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool LabelIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLabel();
				//if (result != null && Label != null){
				//	return !Label.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.LoginAccount _upperloginaccount;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sử dụng")]
        [ToolTip("Sử dụng")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[Member.Oid] = '@This.Member.Oid'")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.LoginAccount UpperLoginAccount
        { 
		    get => GetPropertyValue<Module.BusinessObjects.LoginAccount>("UpperLoginAccount");                         
			set => SetPropertyValue<Module.BusinessObjects.LoginAccount>("UpperLoginAccount", value); 
			
        }
		//Tooltip for Object
		public object UpperLoginAccountToolTipControllerText(View view)
        {
        //    if (UpperLoginAccount != null) 
		//			return UpperLoginAccount;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.LoginAccount GetDefaultUpperLoginAccount(View view = null)
        { 
			return UpperLoginAccount;
        }
		//Set Default Value
		public void SetDefaultUpperLoginAccount(View view = null)
        {
            //if (UpperLoginAccount is null){
            //    var result = GetDefaultUpperLoginAccount(view);
            //    if (result != null && result != UpperLoginAccount){
			//          UpperLoginAccount = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperLoginAccountIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpperLoginAccount();
				//if (result != null && UpperLoginAccount != null){
				//	return !UpperLoginAccount.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpperLoginAccountCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(UpperLoginAccount));
            }
        }
	
       
		//private Module.BusinessObjects.Folder _folder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#App# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-LoginAccountList")]
	 
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
		//Set Default Value

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
	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên đăng nhập")]
        [ToolTip("Tên đăng nhập")]
		//[Index(5)]		

 		[Size(100)]
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

	
       
		//private string _password;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mật khẩu")]
        [ToolTip("Mật khẩu")]
		//[Index(6)]		

 		[Size(200)]
	    [NotMapped()]
	    [ImmediatePostData()]
	    [NonPersistent()]
	    [ModelDefault("PropertyEditorType", "PasswordEditor")]
		public string Password
        { 
		    #region 1413ImportCode 
            get => _password;
            set => SetPropertyValue<string>("Password", ref _password, value); 
#endregion 1413ImportCode
			
        }
		//Tooltip for Object
		public object PasswordToolTipControllerText(View view)
        {
        //    if (Password != null) 
		//			return Password;
            return null;
        }
		//Get Default Value
        public string GetDefaultPassword(View view = null)
        { 
			return Password;
        }
		//Set Default Value
		public void SetDefaultPassword(View view = null)
        {
            //if (Password is null){
            //    var result = GetDefaultPassword(view);
            //    if (result != null && result != Password){
			//          Password = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PasswordIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPassword();
				//if (result != null && Password != null){
				//	return !Password.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _email;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Email")]
        [ToolTip("Email")]
		//[Index(7)]		

 		[Size(100)]
		public string Email
        { 
		    get => GetPropertyValue<string>("Email");                         
			set => SetPropertyValue<string>("Email", value); 
			
        }
		//Tooltip for Object
		public object EmailToolTipControllerText(View view)
        {
        //    if (Email != null) 
		//			return Email;
            return null;
        }
		//Get Default Value
        public string GetDefaultEmail(View view = null)
        { 
			return Email;
        }
		//Set Default Value
		public void SetDefaultEmail(View view = null)
        {
            //if (Email is null){
            //    var result = GetDefaultEmail(view);
            //    if (result != null && result != Email){
			//          Email = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EmailIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEmail();
				//if (result != null && Email != null){
				//	return !Email.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _phone;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Điện thoại")]
        [ToolTip("Điện thoại")]
		//[Index(8)]		

 		[Size(100)]
		public string Phone
        { 
		    get => GetPropertyValue<string>("Phone");                         
			set => SetPropertyValue<string>("Phone", value); 
			
        }
		//Tooltip for Object
		public object PhoneToolTipControllerText(View view)
        {
        //    if (Phone != null) 
		//			return Phone;
            return null;
        }
		//Get Default Value
        public string GetDefaultPhone(View view = null)
        { 
			return Phone;
        }
		//Set Default Value
		public void SetDefaultPhone(View view = null)
        {
            //if (Phone is null){
            //    var result = GetDefaultPhone(view);
            //    if (result != null && result != Phone){
			//          Phone = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PhoneIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPhone();
				//if (result != null && Phone != null){
				//	return !Phone.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _secondfactor;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hai yếu tố")]
        [ToolTip("Hai yếu tố")]
		//[Index(9)]		
		public bool SecondFactor
        { 
		    get => GetPropertyValue<bool>("SecondFactor");                         
			set => SetPropertyValue<bool>("SecondFactor", value); 
			
        }
		//Tooltip for Object
		public object SecondFactorToolTipControllerText(View view)
        {
        //    if (SecondFactor != null) 
		//			return SecondFactor;
            return null;
        }
		//Get Default Value
        public bool GetDefaultSecondFactor(View view = null)
        { 
			return SecondFactor;
        }
		//Set Default Value
		public void SetDefaultSecondFactor(View view = null)
        {
            //if (SecondFactor is null){
            //    var result = GetDefaultSecondFactor(view);
            //    if (result != null && result != SecondFactor){
			//          SecondFactor = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SecondFactorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSecondFactor();
				//if (result != null && SecondFactor != null){
				//	return !SecondFactor.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chia sẻ")]
		//[Index(10)]
		[DataSourceCriteria("Not LoginAccountList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("LoginAccountList-MemberList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Member> MemberList
        {      
		    get => GetCollection<Module.BusinessObjects.Member>("MemberList"); 
			
        }
       
		//private DateTime _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(11)]		
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

	
       
		//private DateTime? _passworddate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày mật khẩu")]
        [ToolTip("Ngày mật khẩu")]
		//[Index(12)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? PasswordDate
        { 
		    get => GetPropertyValue<DateTime?>("PasswordDate");                         
			set => SetPropertyValue<DateTime?>("PasswordDate", value); 
			
        }
		//Tooltip for Object
		public object PasswordDateToolTipControllerText(View view)
        {
        //    if (PasswordDate != null) 
		//			return PasswordDate;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultPasswordDate(View view = null)
        { 
			return PasswordDate;
        }
		//Set Default Value
		public void SetDefaultPasswordDate(View view = null)
        {
            //if (PasswordDate is null){
            //    var result = GetDefaultPasswordDate(view);
            //    if (result != null && result != PasswordDate){
			//          PasswordDate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PasswordDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPasswordDate();
				//if (result != null && PasswordDate != null){
				//	return !PasswordDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _storepassword;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("StorePassword")]
        [ToolTip("StorePassword")]
		//[Index(13)]		

 		[Size(200)]
	    [Browsable(false)]
		public string StorePassword
        { 
		    get => GetPropertyValue<string>("StorePassword");                         
			set => SetPropertyValue<string>("StorePassword", value); 
			
        }
		//Tooltip for Object
		public object StorePasswordToolTipControllerText(View view)
        {
        //    if (StorePassword != null) 
		//			return StorePassword;
            return null;
        }
		//Get Default Value
        public string GetDefaultStorePassword(View view = null)
        { 
			return StorePassword;
        }
		//Set Default Value
		public void SetDefaultStorePassword(View view = null)
        {
            //if (StorePassword is null){
            //    var result = GetDefaultStorePassword(view);
            //    if (result != null && result != StorePassword){
			//          StorePassword = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StorePasswordIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStorePassword();
				//if (result != null && StorePassword != null){
				//	return !StorePassword.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int _highlight;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("HighLight")]
        [ToolTip("HighLight")]
		//[Index(14)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
	    [NonPersistent()]
	    [NotMapped()]
		public int HighLight
        { 
		    get => GetPropertyValue<int>("HighLight");                         
			set => SetPropertyValue<int>("HighLight", value); 
			
        }
		//Tooltip for Object
		public object HighLightToolTipControllerText(View view)
        {
        //    if (HighLight != null) 
		//			return HighLight;
            return null;
        }
		//Get Default Value
        public int GetDefaultHighLight(View view = null)
        { 
			return HighLight;
        }
		//Set Default Value
		public void SetDefaultHighLight(View view = null)
        {
            //if (HighLight is null){
            //    var result = GetDefaultHighLight(view);
            //    if (result != null && result != HighLight){
			//          HighLight = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool HighLightIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultHighLight();
				//if (result != null && HighLight != null){
				//	return !HighLight.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(15)]		
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
 
            #region 0419ImportCode
            base.AfterConstruction();
SetDefaultUpdate();
SetDefaultMember();
            #endregion 0419ImportCode
            Display = true;
 
        //SetDefaultMember(View view = null);
        //SetDefaultAppName(View view = null);
        //SetDefaultLabel(View view = null);
        //SetDefaultUpperLoginAccount(View view = null);
        //SetDefaultFolder(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultPassword(View view = null);
        //SetDefaultEmail(View view = null);
        //SetDefaultPhone(View view = null);
        //SetDefaultSecondFactor(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultPasswordDate(View view = null);
        //SetDefaultStorePassword(View view = null);
        //SetDefaultHighLight(View view = null);
        //SetDefaultInActive(View view = null);
			
        }
        
        protected override void OnLoading()
        {
            base.OnLoading();
        }
        
        protected override void OnLoaded()
        {
            #region 0441ImportCode
                        base.OnLoaded();
            if (!string.IsNullOrEmpty(StorePassword))
            {
                if (SecuritySystem.CurrentUserId != null && Member != null)
                {
                    try
                    {
                        string decrypt = SecuritySystem.CurrentUserId.Equals(Member.Oid) ? 
                                SecuritySystem.CurrentUserId.ToString() : "";
                        if (string.IsNullOrEmpty(decrypt))
                        {
                            foreach(var member in MemberList)
                            {
                                if (member.Oid.Equals(SecuritySystem.CurrentUserId))
                                {
                                    decrypt = Member.Oid.ToString();
                                }
                                    
                            }
                        }
                        if (!string.IsNullOrEmpty(decrypt))
                            _password = DecryptCipherTextToPlainText(StorePassword, decrypt);
                    }
                    catch (Exception ex)
                    {
                        _password = DefaultPassword;
                        //Console.WriteLine(ex);
                    }
                }
                else
                    _password = DefaultPassword;
            }
        
            #endregion 0441ImportCode
        }

        private bool alreadySaving = false;        
        protected override void OnSaving()
        {
            #region 0469ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 0469ImportCode
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
				
                    case nameof(AppName):
                        OnChangedAppName(oldValue, newValue);
                        break;
				
                    case nameof(Password):
                        OnChangedPassword(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedAppName(object oldValue, object newValue)
        {
            #region 1580ImportCode
            if (newValue is null) return;
SetDefaultFolder();            
            #endregion 1580ImportCode
        }               
        private void OnChangedPassword(object oldValue, object newValue)
        {
            #region 0364ImportCode
            if (DefaultPassword.Equals(Password))
      return;
if (newValue != null)
{
    if (Member != null)
    {
        StorePassword = EncryptPlainTextToCipherText((string)newValue, Member.Oid.ToString());
    }
}
else
{
    StorePassword = Password;
}
PasswordDate = DateTime.Now;            
            #endregion 0364ImportCode
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
			//	SetDefaultMemberList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1416ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 1416            Oid: 05d716f7-7a67-4341-8912-711f403c4e89
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 1416ImportCode
#region 1578ImportCode
		public Module.BusinessObjects.Folder GetDefaultFolder(View view = null)
        {
            //Code: 1578            Oid: e9960ec3-8f8b-4f43-b55d-f582b46ad8aa
            var app = Session.FindObject<App>(CriteriaOperator.Parse("Name = ?", AppName)); 
return app?.Folder;
        }
#endregion 1578ImportCode
#region 0075ImportCode
		public DateTime GetDefaultUpdate(View view = null)
        {
            //Code: 0075            Oid: c333a351-3d61-4d9c-88f0-2b29e3cad6a3
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 0075ImportCode
#region 0113ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 0113            Oid: 20847ebc-8bc2-4558-ac37-882ec91e2c8c
            Update = GetDefaultUpdate();
        }
#endregion 0113ImportCode
#region 1579ImportCode
		public void SetDefaultFolder(View view = null)
        {
            //Code: 1579            Oid: bd74ef7d-9189-4643-983c-6df85a49fee8
            Folder ??= GetDefaultFolder();
        }
#endregion 1579ImportCode
#region 1571ImportCode
		public string GetDefaultLabel(View view = null)
        {
            //Code: 1571            Oid: 6e171d60-5b94-4829-bd7c-412cf6d9477b
                       if (Name is null)
               return AppName + " " + Member.Name;
           else
               return AppName + " " + Name;
        }
#endregion 1571ImportCode
#region 1415ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 1415            Oid: 7b401ec1-8ff5-44d4-9a3c-b1c89b70b51a
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 1415ImportCode
#region 1572ImportCode
		public void SetDefaultLabel(View view = null)
        {
            //Code: 1572            Oid: f7891dc2-b1d0-47f3-9a5a-e43bfc3ac12e
            if(String.IsNullOrEmpty(Label)) Label = GetDefaultLabel();

        }
#endregion 1572ImportCode
        #endregion
//Mã nguồn bổ sung
#region LoginAccountImportCode
        private string _password;
        [Browsable(false)]
        public System.Collections.Generic.IList<string> AppDataSource
        {
            get
            {
                return new XPCollection<Module.BusinessObjects.App>(Session).OrderBy(m => m.Name).Select(m => m.Name).ToList();
                //return null;
            }
        }

protected const string DefaultPassword = "************";
private static string privateKey = "wxM:%=4y/-W;8-P";
        private static string EncryptPlainTextToCipherText(string plainText, string securityKey)
{
            if (DefaultPassword.Equals(plainText))
                return DefaultPassword;
    // Getting the bytes of Input String.
    byte[] toEncryptedArray = System.Text.UTF8Encoding.UTF8.GetBytes(plainText);

    System.Security.Cryptography.MD5CryptoServiceProvider objMD5CryptoService = new System.Security.Cryptography.MD5CryptoServiceProvider();
    //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
    byte[] securityKeyArray = objMD5CryptoService.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(securityKey + privateKey));
    //De-allocatinng the memory after doing the Job.
    objMD5CryptoService.Clear();

    var objTripleDESCryptoService = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
    //Assigning the Security key to the TripleDES Service Provider.
    objTripleDESCryptoService.Key = securityKeyArray;
    //Mode of the Crypto service is Electronic Code Book.
    objTripleDESCryptoService.Mode = System.Security.Cryptography.CipherMode.ECB;
    //Padding Mode is PKCS7 if there is any extra byte is added.
    objTripleDESCryptoService.Padding = System.Security.Cryptography.PaddingMode.PKCS7;


    var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
    //Transform the bytes array to resultArray
    byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
    objTripleDESCryptoService.Clear();
    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
}

//This method is used to convert the Encrypted/Un-Readable Text back to readable  format.
private static string DecryptCipherTextToPlainText(string cipherText, string securityKey)
{
            if (DefaultPassword.Equals(cipherText))
                return DefaultPassword;
    byte[] toEncryptArray = Convert.FromBase64String(cipherText);
    System.Security.Cryptography.MD5CryptoServiceProvider objMD5CryptoService = new System.Security.Cryptography.MD5CryptoServiceProvider();

    //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
    byte[] securityKeyArray = objMD5CryptoService.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(securityKey + privateKey));
    objMD5CryptoService.Clear();

    var objTripleDESCryptoService = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
    //Assigning the Security key to the TripleDES Service Provider.
    objTripleDESCryptoService.Key = securityKeyArray;
    //Mode of the Crypto service is Electronic Code Book.
    objTripleDESCryptoService.Mode = System.Security.Cryptography.CipherMode.ECB;
    //Padding Mode is PKCS7 if there is any extra byte is added.
    objTripleDESCryptoService.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

    var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();
    //Transform the bytes array to resultArray
    byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
    objTripleDESCryptoService.Clear();

    //Convert and return the decrypted data/byte into string format.
    return System.Text.UTF8Encoding.UTF8.GetString(resultArray);
}
#endregion LoginAccountImportCode
		 		 
    }
}
