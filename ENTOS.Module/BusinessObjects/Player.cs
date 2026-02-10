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
	[NavigationItem("Tournament")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Đấu thủ"), ImageName("Player")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "Player_ListView", TargetItems = nameof(Name)+ "," + nameof(PlayerType)+ "," + nameof(Team))]
	[MobileColumnAttribute(Context = "Team_PlayerList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Player_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Contact_PlayerList_ListView", TargetItems = nameof(Name)+ "," + nameof(PlayerType)+ "," + nameof(Team))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Player:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Player(Session session)
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
				if (MatchJoin.IsLoaded)
                {
                    if (MatchJoin.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(MatchJoin)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(MatchJoin)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.MatchJoin>(CriteriaOperator.Parse("[Player.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool matchjoin = Session.Query<Module.BusinessObjects.MatchJoin>().Where(x => x.Player.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(MatchJoin), matchjoin);
                        if (matchjoin)
                            return true;

                    }                    
                }				
				if (PostList.IsLoaded)
                {
                    if (PostList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(PostList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(PostList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Post>(CriteriaOperator.Parse("[Player.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool postlist = Session.Query<Module.BusinessObjects.Post>().Where(x => x.Player.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(PostList), postlist);
                        if (postlist)
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

 		[Size(200)]
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

	
       
		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(1)]		

 		[Size(20)]
		[RuleUniqueValue("UniquePlayerCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredPlayerCode", DefaultContexts.Save)]
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

	
       
		//private Module.BusinessObjects.Domain _domain;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bộ môn")]
        [ToolTip("Bộ môn")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(DomainCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Domain Domain
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Domain>("Domain");                         
			set => SetPropertyValue<Module.BusinessObjects.Domain>("Domain", value); 
			
        }
		//Tooltip for Object
		public object DomainToolTipControllerText(View view)
        {
        //    if (Domain != null) 
		//			return Domain;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Domain GetDefaultDomain(View view = null)
        { 
			return Domain;
        }
		//Set Default Value
		public void SetDefaultDomain(View view = null)
        {
            //if (Domain is null){
            //    var result = GetDefaultDomain(view);
            //    if (result != null && result != Domain){
			//          Domain = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DomainIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDomain();
				//if (result != null && Domain != null){
				//	return !Domain.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator DomainCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Domain));
            }
        }
	
       
		//private PlayerType _playertype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(3)]		
		public PlayerType PlayerType
        { 
		    get => GetPropertyValue<PlayerType>("PlayerType");                         
			set => SetPropertyValue<PlayerType>("PlayerType", value); 
			
        }
		//Tooltip for Object
		public object PlayerTypeToolTipControllerText(View view)
        {
        //    if (PlayerType != null) 
		//			return PlayerType;
            return null;
        }
		//Get Default Value
        public PlayerType GetDefaultPlayerType(View view = null)
        { 
			return PlayerType;
        }
		//Set Default Value
		public void SetDefaultPlayerType(View view = null)
        {
            //if (PlayerType is null){
            //    var result = GetDefaultPlayerType(view);
            //    if (result != null && result != PlayerType){
			//          PlayerType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PlayerTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPlayerType();
				//if (result != null && PlayerType != null){
				//	return !PlayerType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _businessrole;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vai trò")]
        [ToolTip("Vai trò")]
		//[Index(4)]		

 		[Size(150)]
		public string BusinessRole
        { 
		    get => GetPropertyValue<string>("BusinessRole");                         
			set => SetPropertyValue<string>("BusinessRole", value); 
			
        }
		//Tooltip for Object
		public object BusinessRoleToolTipControllerText(View view)
        {
        //    if (BusinessRole != null) 
		//			return BusinessRole;
            return null;
        }
		//Get Default Value
        public string GetDefaultBusinessRole(View view = null)
        { 
			return BusinessRole;
        }
		//Set Default Value
		public void SetDefaultBusinessRole(View view = null)
        {
            //if (BusinessRole is null){
            //    var result = GetDefaultBusinessRole(view);
            //    if (result != null && result != BusinessRole){
			//          BusinessRole = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BusinessRoleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBusinessRole();
				//if (result != null && BusinessRole != null){
				//	return !BusinessRole.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Contact _contact;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Liên hệ")]
        [ToolTip("Liên hệ")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ContactCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Contact-PlayerList")]
	 
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
	
       
		//private Module.BusinessObjects.Country _country;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quốc tịch")]
        [ToolTip("Quốc tịch")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CountryCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Country Country
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Country>("Country");                         
			set => SetPropertyValue<Module.BusinessObjects.Country>("Country", value); 
			
        }
		//Tooltip for Object
		public object CountryToolTipControllerText(View view)
        {
        //    if (Country != null) 
		//			return Country;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Country GetDefaultCountry(View view = null)
        { 
			return Country;
        }
		//Set Default Value
		public void SetDefaultCountry(View view = null)
        {
            //if (Country is null){
            //    var result = GetDefaultCountry(view);
            //    if (result != null && result != Country){
			//          Country = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CountryIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCountry();
				//if (result != null && Country != null){
				//	return !Country.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CountryCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Country));
            }
        }
	
       
		//private DateTime? _fromdate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Từ ngày")]
        [ToolTip("Từ ngày")]
		//[Index(7)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? FromDate
        { 
		    get => GetPropertyValue<DateTime?>("FromDate");                         
			set => SetPropertyValue<DateTime?>("FromDate", value); 
			
        }
		//Tooltip for Object
		public object FromDateToolTipControllerText(View view)
        {
        //    if (FromDate != null) 
		//			return FromDate;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultFromDate(View view = null)
        { 
			return FromDate;
        }
		//Set Default Value
		public void SetDefaultFromDate(View view = null)
        {
            //if (FromDate is null){
            //    var result = GetDefaultFromDate(view);
            //    if (result != null && result != FromDate){
			//          FromDate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FromDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFromDate();
				//if (result != null && FromDate != null){
				//	return !FromDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _todate;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đến ngày")]
        [ToolTip("Đến ngày")]
		//[Index(8)]		
		[ModelDefault("DisplayFormat", "d/M")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? ToDate
        { 
		    get => GetPropertyValue<DateTime?>("ToDate");                         
			set => SetPropertyValue<DateTime?>("ToDate", value); 
			
        }
		//Tooltip for Object
		public object ToDateToolTipControllerText(View view)
        {
        //    if (ToDate != null) 
		//			return ToDate;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultToDate(View view = null)
        { 
			return ToDate;
        }
		//Set Default Value
		public void SetDefaultToDate(View view = null)
        {
            //if (ToDate is null){
            //    var result = GetDefaultToDate(view);
            //    if (result != null && result != ToDate){
			//          ToDate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ToDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultToDate();
				//if (result != null && ToDate != null){
				//	return !ToDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trận đấu")]
		//[Index(9)]
		[DevExpress.Xpo.Association("Player-MatchJoin")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.MatchJoin> MatchJoin
        {      
		    get => GetCollection<Module.BusinessObjects.MatchJoin>("MatchJoin"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tin tức")]
		//[Index(10)]
		[DevExpress.Xpo.Association("Player-PostList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Post> PostList
        {      
		    get => GetCollection<Module.BusinessObjects.Post>("PostList"); 
			
        }
       
		//private Module.BusinessObjects.Team _team;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Đội")]
        [ToolTip("Đội")]
		//[Index(11)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TeamCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Team-PlayerList")]
	 
		public Module.BusinessObjects.Team Team
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Team>("Team");                         
			set => SetPropertyValue<Module.BusinessObjects.Team>("Team", value); 
			
        }
		//Tooltip for Object
		public object TeamToolTipControllerText(View view)
        {
        //    if (Team != null) 
		//			return Team;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Team GetDefaultTeam(View view = null)
        { 
			return Team;
        }
		//Set Default Value
		public void SetDefaultTeam(View view = null)
        {
            //if (Team is null){
            //    var result = GetDefaultTeam(view);
            //    if (result != null && result != Team){
			//          Team = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TeamIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTeam();
				//if (result != null && Team != null){
				//	return !Team.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TeamCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Team));
            }
        }
	
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultDomain(View view = null);
        //SetDefaultPlayerType(View view = null);
        //SetDefaultBusinessRole(View view = null);
        //SetDefaultContact(View view = null);
        //SetDefaultCountry(View view = null);
        //SetDefaultFromDate(View view = null);
        //SetDefaultToDate(View view = null);
        //SetDefaultTeam(View view = null);
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
			//	SetDefaultMatchJoin();
			//	SetDefaultPostList();
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
