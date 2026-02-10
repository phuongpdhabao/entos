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
    [ModelDefault("Caption", "Liên hệ thông điệp"), ImageName("MessageContact")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "MessageContact_LookupListView", TargetItems = nameof(Contact))]
	[MobileColumnAttribute(Context = "Message_CampaignContacts_ListView", TargetItems = nameof(Contact))]
	[MobileColumnAttribute(Context = "MessageContact_ListView", TargetItems = nameof(Contact))]
	[DefaultProperty("Contact")]
 
[OptimisticLocking(true)]
    public partial class MessageContact:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public MessageContact(Session session)
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
				if (CampaignContactChannel.IsLoaded)
                {
                    if (CampaignContactChannel.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(CampaignContactChannel)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(CampaignContactChannel)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.MessageRecord>(CriteriaOperator.Parse("[CampaignContact.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool campaigncontactchannel = Session.Query<Module.BusinessObjects.MessageRecord>().Where(x => x.CampaignContact.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(CampaignContactChannel), campaigncontactchannel);
                        if (campaigncontactchannel)
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
               

		//private Module.BusinessObjects.Contact _contact;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Liên hệ")]
        [ToolTip("Liên hệ")]
		//[Index(0)]		
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
	
       
		//private Module.BusinessObjects.Message _campaign;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chiến dịch")]
        [ToolTip("Chiến dịch")]
		//[Index(1)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CampaignCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Campaign-CampaignContacts")]
	 
		public Module.BusinessObjects.Message Campaign
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Message>("Campaign");                         
			set => SetPropertyValue<Module.BusinessObjects.Message>("Campaign", value); 
			
        }
		//Tooltip for Object
		public object CampaignToolTipControllerText(View view)
        {
        //    if (Campaign != null) 
		//			return Campaign;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Message GetDefaultCampaign(View view = null)
        { 
			return Campaign;
        }
		//Set Default Value
		public void SetDefaultCampaign(View view = null)
        {
            //if (Campaign is null){
            //    var result = GetDefaultCampaign(view);
            //    if (result != null && result != Campaign){
			//          Campaign = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CampaignIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCampaign();
				//if (result != null && Campaign != null){
				//	return !Campaign.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CampaignCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Campaign));
            }
        }
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kênh truyền thông")]
		//[Index(2)]
		[DevExpress.Xpo.Association("CampaignContact-CampaignContactChannel")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.MessageRecord> CampaignContactChannel
        {      
		    get => GetCollection<Module.BusinessObjects.MessageRecord>("CampaignContactChannel"); 
			
        }
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultContact(View view = null);
        //SetDefaultCampaign(View view = null);
			
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
			//	SetDefaultCampaignContactChannel();
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
