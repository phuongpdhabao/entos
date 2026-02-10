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
    [ModelDefault("Caption", "Giải đấu"), ImageName("Tournament")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member))]
 
	[MobileColumnAttribute(Context = "Tournament_LookupListView", TargetItems = nameof(Logo)+ "," + nameof(Code))]
	[MobileColumnAttribute(Context = "Tournament_ListView", TargetItems = nameof(Code)+ "," + nameof(Logo)+ "," + nameof(Member))]
	[DefaultProperty("Code")]
 
[OptimisticLocking(true)]
    public partial class Tournament:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Tournament(Session session)
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
				if (TournamentSeasonList.IsLoaded)
                {
                    if (TournamentSeasonList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(TournamentSeasonList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(TournamentSeasonList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.TournamentSeason>(CriteriaOperator.Parse("[Tournament.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool tournamentseasonlist = Session.Query<Module.BusinessObjects.TournamentSeason>().Where(x => x.Tournament.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(TournamentSeasonList), tournamentseasonlist);
                        if (tournamentseasonlist)
                            return true;

                    }                    
                }				
				if (TournamentRoundList.IsLoaded)
                {
                    if (TournamentRoundList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(TournamentRoundList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(TournamentRoundList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.TournamentRound>(CriteriaOperator.Parse("[Tournament.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool tournamentroundlist = Session.Query<Module.BusinessObjects.TournamentRound>().Where(x => x.Tournament.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(TournamentRoundList), tournamentroundlist);
                        if (tournamentroundlist)
                            return true;

                    }                    
                }				
				if (PrizeList.IsLoaded)
                {
                    if (PrizeList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(PrizeList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(PrizeList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Prize>(CriteriaOperator.Parse("[Tournament.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool prizelist = Session.Query<Module.BusinessObjects.Prize>().Where(x => x.Tournament.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(PrizeList), prizelist);
                        if (prizelist)
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
               

		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(0)]		

 		[Size(150)]
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

	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(1)]		

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

	
       
		//private Module.BusinessObjects.Domain _domain;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
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
	
       
		//private TournamentType _tournamenttype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(3)]		
		public TournamentType TournamentType
        { 
		    get => GetPropertyValue<TournamentType>("TournamentType");                         
			set => SetPropertyValue<TournamentType>("TournamentType", value); 
			
        }
		//Tooltip for Object
		public object TournamentTypeToolTipControllerText(View view)
        {
        //    if (TournamentType != null) 
		//			return TournamentType;
            return null;
        }
		//Get Default Value
        public TournamentType GetDefaultTournamentType(View view = null)
        { 
			return TournamentType;
        }
		//Set Default Value
		public void SetDefaultTournamentType(View view = null)
        {
            //if (TournamentType is null){
            //    var result = GetDefaultTournamentType(view);
            //    if (result != null && result != TournamentType){
			//          TournamentType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TournamentTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTournamentType();
				//if (result != null && TournamentType != null){
				//	return !TournamentType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private byte[] _logo;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Biểu tượng")]
        [ToolTip("Biểu tượng")]
		//[Index(4)]		
		[Appearance("Biểu tượngBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Logo
        { 
		    get => GetPropertyValue<byte[]>("Logo");                         
			set => SetPropertyValue<byte[]>("Logo", value); 
			
        }
		//Tooltip for Object
		public object LogoToolTipControllerText(View view)
        {
        //    if (Logo != null) 
		//			return Logo;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultLogo(View view = null)
        { 
			return Logo;
        }
		//Set Default Value
		public void SetDefaultLogo(View view = null)
        {
            //if (Logo is null){
            //    var result = GetDefaultLogo(view);
            //    if (result != null && result != Logo){
			//          Logo = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LogoIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLogo();
				//if (result != null && Logo != null){
				//	return !Logo.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Space _space;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Địa bàn")]
        [ToolTip("Địa bàn")]
		//[Index(5)]		
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
	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(6)]		
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
	
       
		//private Module.BusinessObjects.Org _org;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tổ chức")]
        [ToolTip("Tổ chức")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(OrgCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
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
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mùa giải")]
		//[Index(8)]
		[DevExpress.Xpo.Association("Tournament-TournamentSeasonList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.TournamentSeason> TournamentSeasonList
        {      
		    get => GetCollection<Module.BusinessObjects.TournamentSeason>("TournamentSeasonList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vòng đấu")]
		//[Index(9)]
		[DevExpress.Xpo.Association("Tournament-TournamentRoundList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.TournamentRound> TournamentRoundList
        {      
		    get => GetCollection<Module.BusinessObjects.TournamentRound>("TournamentRoundList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giải thưởng")]
		//[Index(10)]
		[DevExpress.Xpo.Association("Tournament-PrizeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Prize> PrizeList
        {      
		    get => GetCollection<Module.BusinessObjects.Prize>("PrizeList"); 
			
        }
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 1628ImportCode
            base.AfterConstruction();
SetDefaultMember();
            #endregion 1628ImportCode
 
        //SetDefaultCode(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultDomain(View view = null);
        //SetDefaultTournamentType(View view = null);
        //SetDefaultLogo(View view = null);
        //SetDefaultSpace(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultOrg(View view = null);
			
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
			//	SetDefaultTournamentSeasonList();
			//	SetDefaultTournamentRoundList();
			//	SetDefaultPrizeList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1629ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 1629            Oid: ba49f6c2-d0b5-42b9-94cd-7a6e988ee6e6
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 1629ImportCode
#region 1627ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 1627            Oid: 7e73bcdf-28d8-47be-81a5-3e6f1d9ff175
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 1627ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
