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
	[NavigationItem("Accounting")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Bút toán mẫu"), ImageName("EntryTemplate")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "EntryTemplate_ListView", TargetItems = nameof(AccountingTemplate))]
	[DefaultProperty("Account")]
 
[OptimisticLocking(true)]
    public partial class EntryTemplate:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public EntryTemplate(Session session)
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
               

		//private Module.BusinessObjects.EntryFolder _entryfolder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài khoản kế toán")]
        [ToolTip("Tài khoản kế toán")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(EntryFolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		[RuleRequiredField("RequiredEntryTemplateEntryFolder", DefaultContexts.Save)]
		public Module.BusinessObjects.EntryFolder EntryFolder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.EntryFolder>("EntryFolder");                         
			set => SetPropertyValue<Module.BusinessObjects.EntryFolder>("EntryFolder", value); 
			
        }
		//Tooltip for Object
		public object EntryFolderToolTipControllerText(View view)
        {
        //    if (EntryFolder != null) 
		//			return EntryFolder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.EntryFolder GetDefaultEntryFolder(View view = null)
        { 
			return EntryFolder;
        }
		//Set Default Value
		public void SetDefaultEntryFolder(View view = null)
        {
            //if (EntryFolder is null){
            //    var result = GetDefaultEntryFolder(view);
            //    if (result != null && result != EntryFolder){
			//          EntryFolder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EntryFolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEntryFolder();
				//if (result != null && EntryFolder != null){
				//	return !EntryFolder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator EntryFolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(EntryFolder));
            }
        }
	
       
		//private bool _debit;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ghi nợ")]
        [ToolTip("Ghi nợ")]
		//[Index(2)]		
		public bool Debit
        { 
		    get => GetPropertyValue<bool>("Debit");                         
			set => SetPropertyValue<bool>("Debit", value); 
			
        }
		//Tooltip for Object
		public object DebitToolTipControllerText(View view)
        {
        //    if (Debit != null) 
		//			return Debit;
            return null;
        }
		//Get Default Value
        public bool GetDefaultDebit(View view = null)
        { 
			return Debit;
        }
		//Set Default Value
		public void SetDefaultDebit(View view = null)
        {
            //if (Debit is null){
            //    var result = GetDefaultDebit(view);
            //    if (result != null && result != Debit){
			//          Debit = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DebitIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDebit();
				//if (result != null && Debit != null){
				//	return !Debit.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.EntryFolder _partyfolder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài khoản đối ứng")]
        [ToolTip("Tài khoản đối ứng")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PartyFolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		[RuleRequiredField("RequiredEntryTemplatePartyFolder", DefaultContexts.Save)]
		public Module.BusinessObjects.EntryFolder PartyFolder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.EntryFolder>("PartyFolder");                         
			set => SetPropertyValue<Module.BusinessObjects.EntryFolder>("PartyFolder", value); 
			
        }
		//Tooltip for Object
		public object PartyFolderToolTipControllerText(View view)
        {
        //    if (PartyFolder != null) 
		//			return PartyFolder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.EntryFolder GetDefaultPartyFolder(View view = null)
        { 
			return PartyFolder;
        }
		//Set Default Value
		public void SetDefaultPartyFolder(View view = null)
        {
            //if (PartyFolder is null){
            //    var result = GetDefaultPartyFolder(view);
            //    if (result != null && result != PartyFolder){
			//          PartyFolder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PartyFolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPartyFolder();
				//if (result != null && PartyFolder != null){
				//	return !PartyFolder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PartyFolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PartyFolder));
            }
        }
	
       
		//private StringLookup _amount;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giá trị")]
        [ToolTip("Giá trị")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(AmountCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [DataSourceProperty("FieldSource")]
	    [ValueConverter(typeof(StringLookupToStringConverter))]
	    [ImmediatePostData()]
		public StringLookup Amount
        { 
		    get => GetPropertyValue<StringLookup>("Amount");                         
			set => SetPropertyValue<StringLookup>("Amount", value); 
			
        }
		//Tooltip for Object
		public object AmountToolTipControllerText(View view)
        {
        //    if (Amount != null) 
		//			return Amount;
            return null;
        }
		//Get Default Value
        public StringLookup GetDefaultAmount(View view = null)
        { 
			return Amount;
        }
		//Set Default Value
		public void SetDefaultAmount(View view = null)
        {
            //if (Amount is null){
            //    var result = GetDefaultAmount(view);
            //    if (result != null && result != Amount){
			//          Amount = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AmountIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAmount();
				//if (result != null && Amount != null){
				//	return !Amount.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator AmountCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Amount));
            }
        }
	
       
		//private Module.BusinessObjects.AccountingTemplate _accountingtemplate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hạch toán mẫu")]
        [ToolTip("Hạch toán mẫu")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(AccountingTemplateCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("AccountingTemplate-EntryTemplates")]
	 
		public Module.BusinessObjects.AccountingTemplate AccountingTemplate
        { 
		    get => GetPropertyValue<Module.BusinessObjects.AccountingTemplate>("AccountingTemplate");                         
			set => SetPropertyValue<Module.BusinessObjects.AccountingTemplate>("AccountingTemplate", value); 
			
        }
		//Tooltip for Object
		public object AccountingTemplateToolTipControllerText(View view)
        {
        //    if (AccountingTemplate != null) 
		//			return AccountingTemplate;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.AccountingTemplate GetDefaultAccountingTemplate(View view = null)
        { 
			return AccountingTemplate;
        }
		//Set Default Value
		public void SetDefaultAccountingTemplate(View view = null)
        {
            //if (AccountingTemplate is null){
            //    var result = GetDefaultAccountingTemplate(view);
            //    if (result != null && result != AccountingTemplate){
			//          AccountingTemplate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool AccountingTemplateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultAccountingTemplate();
				//if (result != null && AccountingTemplate != null){
				//	return !AccountingTemplate.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator AccountingTemplateCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(AccountingTemplate));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultEntryFolder(View view = null);
        //SetDefaultDebit(View view = null);
        //SetDefaultPartyFolder(View view = null);
        //SetDefaultAmount(View view = null);
        //SetDefaultAccountingTemplate(View view = null);
			
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
#region EntryTemplateImportCode
[Browsable(false)]
        public System.Collections.Generic.IList<DevExpress.ExpressApp.Utils.StringObject> AvailablePropertyNames
        {
            get
            {
                var stringObjects = new System.Collections.Generic.List<DevExpress.ExpressApp.Utils.StringObject>();
                if (this.AccountingTemplate != null && this.AccountingTemplate.SystemType != (Type) null)
                {
                    var members = XafTypesInfo.Instance.FindTypeInfo(this.AccountingTemplate.SystemType).Members;
                    foreach (var member in members)
                        if ((member.IsVisible ||
                             member.FindAttribute<DevExpress.ExpressApp.Security.SecurityBrowsableAttribute>() != null))
                        {
                            if (member.MemberType == typeof(Int16) || member.MemberType == typeof(Int32) ||
                                member.MemberType == typeof(UInt16) || member.MemberType == typeof(UInt32) ||
                                member.MemberType == typeof(Int64) || member.MemberType == typeof(Single) ||
                                member.MemberType == typeof(Double) || member.MemberType == typeof(Decimal))
                                stringObjects.Add(new DevExpress.ExpressApp.Utils.StringObject(member.Name));
                        }
                }
                return stringObjects;
            }
        }
 [Size(100)]
 public StringLookup Field
 { 
	    get => GetPropertyValue<StringLookup>("Amount");                         
		set => SetPropertyValue<StringLookup>("Amount", value); 
			
 }
 [Browsable(false)]
 public System.Collections.Generic.IList<StringLookup> FieldSource
 {
     get
     {
         var objectype = this.AccountingTemplate.SystemType;

         System.Collections.Generic.List<StringLookup> stringObjectList = new System.Collections.Generic.List<StringLookup>();
         if (objectype != (Type)null)
         {
             var members = XafTypesInfo.Instance.FindTypeInfo(objectype).Members;
             foreach (var member in members)
                 if (member.MemberTypeInfo.UnderlyingTypeInfo.Name == "Decimal" && !member.IsReadOnly)
                 {
                     stringObjectList.Add(new StringLookup(DevExpress.ExpressApp.Utils.CaptionHelper.GetMemberCaption(member), member.Name));
                 }

         }

         return (System.Collections.Generic.IList<StringLookup>)stringObjectList;
     }
 }
#endregion EntryTemplateImportCode
		 		 
    }
}
