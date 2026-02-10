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
    [ModelDefault("Caption", "Bản ghi thông điệp"), ImageName("MessageRecord")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "MessageChannel_MessageRecordList_ListView", TargetItems = nameof(DateTime)+ "," + nameof(CampaignContact))]
	[MobileColumnAttribute(Context = "MessageRecord_ListView", TargetItems = nameof(DateTime)+ "," + nameof(CampaignContact))]
	[MobileColumnAttribute(Context = "MessageRecord_LookupListView", TargetItems = nameof(CampaignContact))]
	[MobileColumnAttribute(Context = "MessageContact_CampaignContactChannel_ListView", TargetItems = nameof(DateTime))]
	[DefaultProperty("CampaignContact")]
 
[OptimisticLocking(true)]
    public partial class MessageRecord:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public MessageRecord(Session session)
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
               

		//private Module.BusinessObjects.MessageContact _campaigncontact;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Liên hệ")]
        [ToolTip("Liên hệ")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CampaignContactCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("CampaignContact-CampaignContactChannel")]
	 
		public Module.BusinessObjects.MessageContact CampaignContact
        { 
		    get => GetPropertyValue<Module.BusinessObjects.MessageContact>("CampaignContact");                         
			set => SetPropertyValue<Module.BusinessObjects.MessageContact>("CampaignContact", value); 
			
        }
		//Tooltip for Object
		public object CampaignContactToolTipControllerText(View view)
        {
        //    if (CampaignContact != null) 
		//			return CampaignContact;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.MessageContact GetDefaultCampaignContact(View view = null)
        { 
			return CampaignContact;
        }
		//Set Default Value
		public void SetDefaultCampaignContact(View view = null)
        {
            //if (CampaignContact is null){
            //    var result = GetDefaultCampaignContact(view);
            //    if (result != null && result != CampaignContact){
			//          CampaignContact = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CampaignContactIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCampaignContact();
				//if (result != null && CampaignContact != null){
				//	return !CampaignContact.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CampaignContactCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(CampaignContact));
            }
        }
	
       
		//private Module.BusinessObjects.MessageChannel _campaignchannel;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kênh")]
        [ToolTip("Kênh")]
		//[Index(1)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CampaignChannelCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("CampaignChannel-MessageRecordList")]
	 
		public Module.BusinessObjects.MessageChannel CampaignChannel
        { 
		    get => GetPropertyValue<Module.BusinessObjects.MessageChannel>("CampaignChannel");                         
			set => SetPropertyValue<Module.BusinessObjects.MessageChannel>("CampaignChannel", value); 
			
        }
		//Tooltip for Object
		public object CampaignChannelToolTipControllerText(View view)
        {
        //    if (CampaignChannel != null) 
		//			return CampaignChannel;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.MessageChannel GetDefaultCampaignChannel(View view = null)
        { 
			return CampaignChannel;
        }
		//Set Default Value
		public void SetDefaultCampaignChannel(View view = null)
        {
            //if (CampaignChannel is null){
            //    var result = GetDefaultCampaignChannel(view);
            //    if (result != null && result != CampaignChannel){
			//          CampaignChannel = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CampaignChannelIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCampaignChannel();
				//if (result != null && CampaignChannel != null){
				//	return !CampaignChannel.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CampaignChannelCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(CampaignChannel));
            }
        }
	
       
		//private DateTime? _datetime;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thời gian")]
        [ToolTip("Thời gian")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? DateTime
        { 
		    get => GetPropertyValue<DateTime?>("DateTime");                         
			set => SetPropertyValue<DateTime?>("DateTime", value); 
			
        }
		//Tooltip for Object
		public object DateTimeToolTipControllerText(View view)
        {
        //    if (DateTime != null) 
		//			return DateTime;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultDateTime(View view = null)
        { 
			return DateTime;
        }
		//Set Default Value
		public void SetDefaultDateTime(View view = null)
        {
            //if (DateTime is null){
            //    var result = GetDefaultDateTime(view);
            //    if (result != null && result != DateTime){
			//          DateTime = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DateTimeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDateTime();
				//if (result != null && DateTime != null){
				//	return !DateTime.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _done;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hoàn thành")]
        [ToolTip("Hoàn thành")]
		//[Index(3)]		
		public bool Done
        { 
		    get => GetPropertyValue<bool>("Done");                         
			set => SetPropertyValue<bool>("Done", value); 
			
        }
		//Tooltip for Object
		public object DoneToolTipControllerText(View view)
        {
        //    if (Done != null) 
		//			return Done;
            return null;
        }
		//Get Default Value
        public bool GetDefaultDone(View view = null)
        { 
			return Done;
        }
		//Set Default Value
		public void SetDefaultDone(View view = null)
        {
            //if (Done is null){
            //    var result = GetDefaultDone(view);
            //    if (result != null && result != Done){
			//          Done = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DoneIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDone();
				//if (result != null && Done != null){
				//	return !Done.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _address;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Địa chỉ")]
        [ToolTip("Địa chỉ")]
		//[Index(4)]		

 		[Size(200)]
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultCampaignContact(View view = null);
        //SetDefaultCampaignChannel(View view = null);
        //SetDefaultDateTime(View view = null);
        //SetDefaultDone(View view = null);
        //SetDefaultAddress(View view = null);
			
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
