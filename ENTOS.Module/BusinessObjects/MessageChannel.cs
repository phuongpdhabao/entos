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
    [ModelDefault("Caption", "Kênh thông điệp"), ImageName("MessageChannel")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "MessageChannel_ListView", TargetItems = nameof(Channel))]
	[MobileColumnAttribute(Context = "Message_CampaigneChannels_ListView", TargetItems = nameof(Channel))]
	[DefaultProperty("Channel")]
 
[OptimisticLocking(true)]
    public partial class MessageChannel:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public MessageChannel(Session session)
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
				if (MessageRecordList.IsLoaded)
                {
                    if (MessageRecordList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(MessageRecordList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(MessageRecordList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.MessageRecord>(CriteriaOperator.Parse("[CampaignChannel.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool messagerecordlist = Session.Query<Module.BusinessObjects.MessageRecord>().Where(x => x.CampaignChannel.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(MessageRecordList), messagerecordlist);
                        if (messagerecordlist)
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
               

		//private Module.BusinessObjects.Message _marketingcampaign;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Marketing")]
        [ToolTip("Marketing")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MarketingCampaignCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("MarketingCampaign-CampaigneChannels")]
	 
		public Module.BusinessObjects.Message MarketingCampaign
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Message>("MarketingCampaign");                         
			set => SetPropertyValue<Module.BusinessObjects.Message>("MarketingCampaign", value); 
			
        }
		//Tooltip for Object
		public object MarketingCampaignToolTipControllerText(View view)
        {
        //    if (MarketingCampaign != null) 
		//			return MarketingCampaign;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Message GetDefaultMarketingCampaign(View view = null)
        { 
			return MarketingCampaign;
        }
		//Set Default Value
		public void SetDefaultMarketingCampaign(View view = null)
        {
            //if (MarketingCampaign is null){
            //    var result = GetDefaultMarketingCampaign(view);
            //    if (result != null && result != MarketingCampaign){
			//          MarketingCampaign = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MarketingCampaignIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMarketingCampaign();
				//if (result != null && MarketingCampaign != null){
				//	return !MarketingCampaign.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator MarketingCampaignCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(MarketingCampaign));
            }
        }
	
       
		//private Module.BusinessObjects.CommunicationChannel _channel;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kênh truyền thông")]
        [ToolTip("Kênh truyền thông")]
		//[Index(1)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ChannelCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.CommunicationChannel Channel
        { 
		    get => GetPropertyValue<Module.BusinessObjects.CommunicationChannel>("Channel");                         
			set => SetPropertyValue<Module.BusinessObjects.CommunicationChannel>("Channel", value); 
			
        }
		//Tooltip for Object
		public object ChannelToolTipControllerText(View view)
        {
        //    if (Channel != null) 
		//			return Channel;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.CommunicationChannel GetDefaultChannel(View view = null)
        { 
			return Channel;
        }
		//Set Default Value
		public void SetDefaultChannel(View view = null)
        {
            //if (Channel is null){
            //    var result = GetDefaultChannel(view);
            //    if (result != null && result != Channel){
			//          Channel = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ChannelIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultChannel();
				//if (result != null && Channel != null){
				//	return !Channel.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ChannelCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Channel));
            }
        }
	
       
		//private string _contentbody;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(2)]		

 		[Size(1000)]
		public string ContentBody
        { 
		    get => GetPropertyValue<string>("ContentBody");                         
			set => SetPropertyValue<string>("ContentBody", value); 
			
        }
		//Tooltip for Object
		public object ContentBodyToolTipControllerText(View view)
        {
        //    if (ContentBody != null) 
		//			return ContentBody;
            return null;
        }
		//Get Default Value
        public string GetDefaultContentBody(View view = null)
        { 
			return ContentBody;
        }
		//Set Default Value
		public void SetDefaultContentBody(View view = null)
        {
            //if (ContentBody is null){
            //    var result = GetDefaultContentBody(view);
            //    if (result != null && result != ContentBody){
			//          ContentBody = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ContentBodyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultContentBody();
				//if (result != null && ContentBody != null){
				//	return !ContentBody.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên hệ")]
		//[Index(4)]
		[DevExpress.Xpo.Association("CampaignChannel-MessageRecordList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.MessageRecord> MessageRecordList
        {      
		    get => GetCollection<Module.BusinessObjects.MessageRecord>("MessageRecordList"); 
			
        }
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultMarketingCampaign(View view = null);
        //SetDefaultChannel(View view = null);
			
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
			//	SetDefaultContentBody();
			//	SetDefaultAttachments();
			//	SetDefaultMessageRecordList();
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
