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
    [ModelDefault("Caption", "Mùa giải"), ImageName("TournamentSeason")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Name))]
 
	[MobileColumnAttribute(Context = "TournamentSeason_ListView", TargetItems = "Tournament.Subject"+ "," + nameof(Begin)+ "," + nameof(Code))]
	[MobileColumnAttribute(Context = "Tournament_TournamentSeasonList_ListView", TargetItems = nameof(Code)+ "," + nameof(Begin))]
	[MobileColumnAttribute(Context = "TournamentSeason_LookupListView", TargetItems = nameof(Begin)+ "," + "Tournament.Subject"+ "," + nameof(Code))]
	[DefaultProperty("Code")]
 
[OptimisticLocking(true)]
    public partial class TournamentSeason:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public TournamentSeason(Session session)
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
                        //if (Session.FindObject<Module.BusinessObjects.TournamentRound>(CriteriaOperator.Parse("[TournamentSeason.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool tournamentroundlist = Session.Query<Module.BusinessObjects.TournamentRound>().Where(x => x.TournamentSeason.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
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
                        //if (Session.FindObject<Module.BusinessObjects.Prize>(CriteriaOperator.Parse("[TournamentSeason.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool prizelist = Session.Query<Module.BusinessObjects.Prize>().Where(x => x.TournamentSeason.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(PrizeList), prizelist);
                        if (prizelist)
                            return true;

                    }                    
                }				
				if (MatchJoinList.IsLoaded)
                {
                    if (MatchJoinList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(MatchJoinList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(MatchJoinList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.MatchJoin>(CriteriaOperator.Parse("[TournamentSeason.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool matchjoinlist = Session.Query<Module.BusinessObjects.MatchJoin>().Where(x => x.TournamentSeason.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(MatchJoinList), matchjoinlist);
                        if (matchjoinlist)
                            return true;

                    }                    
                }				
				if (MatchList.IsLoaded)
                {
                    if (MatchList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(MatchList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(MatchList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Match>(CriteriaOperator.Parse("[TournamentSeason.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool matchlist = Session.Query<Module.BusinessObjects.Match>().Where(x => x.TournamentSeason.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(MatchList), matchlist);
                        if (matchlist)
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
                        //if (Session.FindObject<Module.BusinessObjects.Post>(CriteriaOperator.Parse("[TournamentSeason.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool postlist = Session.Query<Module.BusinessObjects.Post>().Where(x => x.TournamentSeason.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
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
               

		//private Module.BusinessObjects.Tournament _tournament;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giải đấu")]
        [ToolTip("Giải đấu")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TournamentCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Tournament-TournamentSeasonList")]
	 
		public Module.BusinessObjects.Tournament Tournament
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Tournament>("Tournament");                         
			set => SetPropertyValue<Module.BusinessObjects.Tournament>("Tournament", value); 
			
        }
		//Tooltip for Object
		public object TournamentToolTipControllerText(View view)
        {
        //    if (Tournament != null) 
		//			return Tournament;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Tournament GetDefaultTournament(View view = null)
        { 
			return Tournament;
        }
		//Set Default Value
		public void SetDefaultTournament(View view = null)
        {
            //if (Tournament is null){
            //    var result = GetDefaultTournament(view);
            //    if (result != null && result != Tournament){
			//          Tournament = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TournamentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTournament();
				//if (result != null && Tournament != null){
				//	return !Tournament.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TournamentCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Tournament));
            }
        }
	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(1)]		

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
		//Set Default Value

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
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(2)]		

 		[Size(100)]
		[RuleUniqueValue("UniqueTournamentSeasonCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredTournamentSeasonCode", DefaultContexts.Save)]
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

	
       
		//private DateTime? _begin;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Khai mạc")]
        [ToolTip("Khai mạc")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? Begin
        { 
		    get => GetPropertyValue<DateTime?>("Begin");                         
			set => SetPropertyValue<DateTime?>("Begin", value); 
			
        }
		//Tooltip for Object
		public object BeginToolTipControllerText(View view)
        {
        //    if (Begin != null) 
		//			return Begin;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultBegin(View view = null)
        { 
			return Begin;
        }
		//Set Default Value
		public void SetDefaultBegin(View view = null)
        {
            //if (Begin is null){
            //    var result = GetDefaultBegin(view);
            //    if (result != null && result != Begin){
			//          Begin = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BeginIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBegin();
				//if (result != null && Begin != null){
				//	return !Begin.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _end;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Bế mạc")]
        [ToolTip("Bế mạc")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? End
        { 
		    get => GetPropertyValue<DateTime?>("End");                         
			set => SetPropertyValue<DateTime?>("End", value); 
			
        }
		//Tooltip for Object
		public object EndToolTipControllerText(View view)
        {
        //    if (End != null) 
		//			return End;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultEnd(View view = null)
        { 
			return End;
        }
		//Set Default Value
		public void SetDefaultEnd(View view = null)
        {
            //if (End is null){
            //    var result = GetDefaultEnd(view);
            //    if (result != null && result != End){
			//          End = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EndIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEnd();
				//if (result != null && End != null){
				//	return !End.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _sponsor;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhà tài trợ")]
        [ToolTip("Nhà tài trợ")]
		//[Index(5)]		

 		[Size(100)]
		public string Sponsor
        { 
		    get => GetPropertyValue<string>("Sponsor");                         
			set => SetPropertyValue<string>("Sponsor", value); 
			
        }
		//Tooltip for Object
		public object SponsorToolTipControllerText(View view)
        {
        //    if (Sponsor != null) 
		//			return Sponsor;
            return null;
        }
		//Get Default Value
        public string GetDefaultSponsor(View view = null)
        { 
			return Sponsor;
        }
		//Set Default Value
		public void SetDefaultSponsor(View view = null)
        {
            //if (Sponsor is null){
            //    var result = GetDefaultSponsor(view);
            //    if (result != null && result != Sponsor){
			//          Sponsor = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SponsorIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSponsor();
				//if (result != null && Sponsor != null){
				//	return !Sponsor.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vòng đấu")]
		//[Index(6)]
		[DevExpress.Xpo.Association("TournamentSeason-TournamentRoundList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.TournamentRound> TournamentRoundList
        {      
		    get => GetCollection<Module.BusinessObjects.TournamentRound>("TournamentRoundList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giải thưởng")]
		//[Index(7)]
		[DevExpress.Xpo.Association("TournamentSeason-PrizeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Prize> PrizeList
        {      
		    get => GetCollection<Module.BusinessObjects.Prize>("PrizeList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tham dự")]
		//[Index(8)]
		[DevExpress.Xpo.Association("TournamentSeason-MatchJoinList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.MatchJoin> MatchJoinList
        {      
		    get => GetCollection<Module.BusinessObjects.MatchJoin>("MatchJoinList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trận đấu")]
		//[Index(9)]
		[DevExpress.Xpo.Association("TournamentSeason-MatchList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Match> MatchList
        {      
		    get => GetCollection<Module.BusinessObjects.Match>("MatchList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tin tức")]
		//[Index(10)]
		[DevExpress.Xpo.Association("TournamentSeason-PostList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Post> PostList
        {      
		    get => GetCollection<Module.BusinessObjects.Post>("PostList"); 
			
        }
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultTournament(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultBegin(View view = null);
        //SetDefaultEnd(View view = null);
        //SetDefaultSponsor(View view = null);
			
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

                switch (propertyName)
                {       
				
                    case nameof(Tournament):
                        OnChangedTournament(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedTournament(object oldValue, object newValue)
        {
            #region 2604ImportCode
            if (newValue is null) return;
SetDefaultName();            
            #endregion 2604ImportCode
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
			//	SetDefaultTournamentRoundList();
			//	SetDefaultPrizeList();
			//	SetDefaultMatchJoinList();
			//	SetDefaultMatchList();
			//	SetDefaultPostList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 2602ImportCode
		public string GetDefaultName(View view = null)
        {
            //Code: 2602            Oid: 3454a347-a1bf-49d0-ba06-7d7ccc73cc82
            return Tournament?.Name;
        }
#endregion 2602ImportCode
#region 2603ImportCode
		public void SetDefaultName(View view = null)
        {
            //Code: 2603            Oid: dc89dc08-f658-4418-9e1f-66131047d798
            if(String.IsNullOrEmpty(Name)) Name = GetDefaultName();
        }
#endregion 2603ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
