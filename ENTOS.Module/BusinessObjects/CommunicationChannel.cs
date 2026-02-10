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
    [ModelDefault("Caption", "Kênh truyền thông"), ImageName("CommunicationChannel")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "CommunicationChannel_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "CommunicationChannel_LookupListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
//[OptimisticLocking(false)]
    public partial class CommunicationChannel: DevExpress.Persistent.BaseImpl.BaseObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public CommunicationChannel(Session session)
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

               

		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(0)]		

 		[Size(100)]
		[RuleUniqueValue("UniqueCommunicationChannelName", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredCommunicationChannelName", DefaultContexts.Save)]
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

	
       
		//private string _server;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Máy chủ")]
        [ToolTip("Máy chủ")]
		//[Index(1)]		

 		[Size(100)]
		public string Server
        { 
		    get => GetPropertyValue<string>("Server");                         
			set => SetPropertyValue<string>("Server", value); 
			
        }
		//Tooltip for Object
		public object ServerToolTipControllerText(View view)
        {
        //    if (Server != null) 
		//			return Server;
            return null;
        }
		//Get Default Value
        public string GetDefaultServer(View view = null)
        { 
			return Server;
        }
		//Set Default Value
		public void SetDefaultServer(View view = null)
        {
            //if (Server is null){
            //    var result = GetDefaultServer(view);
            //    if (result != null && result != Server){
			//          Server = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ServerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultServer();
				//if (result != null && Server != null){
				//	return !Server.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _port;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cổng")]
        [ToolTip("Cổng")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Port
        { 
		    get => GetPropertyValue<int?>("Port");                         
			set => SetPropertyValue<int?>("Port", value); 
			
        }
		//Tooltip for Object
		public object PortToolTipControllerText(View view)
        {
        //    if (Port != null) 
		//			return Port;
            return null;
        }
		//Get Default Value
        public int? GetDefaultPort(View view = null)
        { 
			return Port;
        }
		//Set Default Value
		public void SetDefaultPort(View view = null)
        {
            //if (Port is null){
            //    var result = GetDefaultPort(view);
            //    if (result != null && result != Port){
			//          Port = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PortIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPort();
				//if (result != null && Port != null){
				//	return !Port.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.LoginAccount _loginaccount;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đăng nhập")]
        [ToolTip("Đăng nhập")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(LoginAccountCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.LoginAccount LoginAccount
        { 
		    get => GetPropertyValue<Module.BusinessObjects.LoginAccount>("LoginAccount");                         
			set => SetPropertyValue<Module.BusinessObjects.LoginAccount>("LoginAccount", value); 
			
        }
		//Tooltip for Object
		public object LoginAccountToolTipControllerText(View view)
        {
        //    if (LoginAccount != null) 
		//			return LoginAccount;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.LoginAccount GetDefaultLoginAccount(View view = null)
        { 
			return LoginAccount;
        }
		//Set Default Value
		public void SetDefaultLoginAccount(View view = null)
        {
            //if (LoginAccount is null){
            //    var result = GetDefaultLoginAccount(view);
            //    if (result != null && result != LoginAccount){
			//          LoginAccount = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LoginAccountIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLoginAccount();
				//if (result != null && LoginAccount != null){
				//	return !LoginAccount.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LoginAccountCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(LoginAccount));
            }
        }
	
       
		//private byte[] _icon;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Biểu tượng")]
        [ToolTip("Biểu tượng")]
		//[Index(4)]		
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

	
       
		//private string _url;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("URL")]
        [ToolTip("URL")]
		//[Index(5)]		

 		[Size(100)]
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

	
       
		//private int _length;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giới hạn")]
        [ToolTip("Giới hạn")]
		//[Index(6)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int Length
        { 
		    get => GetPropertyValue<int>("Length");                         
			set => SetPropertyValue<int>("Length", value); 
			
        }
		//Tooltip for Object
		public object LengthToolTipControllerText(View view)
        {
        //    if (Length != null) 
		//			return Length;
            return null;
        }
		//Get Default Value
        public int GetDefaultLength(View view = null)
        { 
			return Length;
        }
		//Set Default Value
		public void SetDefaultLength(View view = null)
        {
            //if (Length is null){
            //    var result = GetDefaultLength(view);
            //    if (result != null && result != Length){
			//          Length = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LengthIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLength();
				//if (result != null && Length != null){
				//	return !Length.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _attachment;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đính kèm")]
        [ToolTip("Đính kèm")]
		//[Index(7)]		
		public bool Attachment
        { 
		    get => GetPropertyValue<bool>("Attachment");                         
			set => SetPropertyValue<bool>("Attachment", value); 
			
        }
		//Tooltip for Object
		public object AttachmentToolTipControllerText(View view)
        {
        //    if (Attachment != null) 
		//			return Attachment;
            return null;
        }
		//Get Default Value
        public bool GetDefaultAttachment(View view = null)
        { 
			return Attachment;
        }
		//Set Default Value
		public void SetDefaultAttachment(View view = null)
        {
            //if (Attachment is null){
            //    var result = GetDefaultAttachment(view);
            //    if (result != null && result != Attachment){
			//          Attachment = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AttachmentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAttachment();
				//if (result != null && Attachment != null){
				//	return !Attachment.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _html;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Html")]
        [ToolTip("Html")]
		//[Index(8)]		
		[RuleRequiredField("RequiredCommunicationChannelHtml", DefaultContexts.Save)]
		public bool Html
        { 
		    get => GetPropertyValue<bool>("Html");                         
			set => SetPropertyValue<bool>("Html", value); 
			
        }
		//Tooltip for Object
		public object HtmlToolTipControllerText(View view)
        {
        //    if (Html != null) 
		//			return Html;
            return null;
        }
		//Get Default Value
        public bool GetDefaultHtml(View view = null)
        { 
			return Html;
        }
		//Set Default Value
		public void SetDefaultHtml(View view = null)
        {
            //if (Html is null){
            //    var result = GetDefaultHtml(view);
            //    if (result != null && result != Html){
			//          Html = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool HtmlIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultHtml();
				//if (result != null && Html != null){
				//	return !Html.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultServer(View view = null);
        //SetDefaultPort(View view = null);
        //SetDefaultLoginAccount(View view = null);
        //SetDefaultIcon(View view = null);
        //SetDefaultURL(View view = null);
        //SetDefaultLength(View view = null);
        //SetDefaultAttachment(View view = null);
        //SetDefaultHtml(View view = null);
			
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
