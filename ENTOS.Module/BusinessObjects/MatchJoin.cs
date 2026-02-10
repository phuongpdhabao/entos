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
    [ModelDefault("Caption", "Tham dự trận đấu"), ImageName("MatchJoin")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "Match_MatchJoinList_ListView", TargetItems = nameof(BusinessRole))]
	[MobileColumnAttribute(Context = "MatchJoin_LookupListView", TargetItems = nameof(BusinessRole))]
	[MobileColumnAttribute(Context = "TournamentSeason_MatchJoinList_ListView", TargetItems = nameof(BusinessRole))]
	[MobileColumnAttribute(Context = "Prize_MatchJoinList_ListView", TargetItems = nameof(BusinessRole))]
	[MobileColumnAttribute(Context = "Player_MatchJoin_ListView", TargetItems = nameof(BusinessRole))]
	[MobileColumnAttribute(Context = "Team_MatchJoinList_ListView", TargetItems = nameof(BusinessRole))]
	[MobileColumnAttribute(Context = "MatchJoin_ListView", TargetItems = nameof(BusinessRole))]
	[DefaultProperty("BusinessRole")]
 
[OptimisticLocking(true)]
    public partial class MatchJoin:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public MatchJoin(Session session)
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
				if (MatchDataList.IsLoaded)
                {
                    if (MatchDataList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(MatchDataList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(MatchDataList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.MatchData>(CriteriaOperator.Parse("[MatchJoin.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool matchdatalist = Session.Query<Module.BusinessObjects.MatchData>().Where(x => x.MatchJoin.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(MatchDataList), matchdatalist);
                        if (matchdatalist)
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
               

		//private string _businessrole;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Vai trò")]
        [ToolTip("Vai trò")]
		//[Index(0)]		

 		[Size(200)]
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

	
       
		//private int? _from;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Từ")]
        [ToolTip("Từ")]
		//[Index(1)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? From
        { 
		    get => GetPropertyValue<int?>("From");                         
			set => SetPropertyValue<int?>("From", value); 
			
        }
		//Tooltip for Object
		public object FromToolTipControllerText(View view)
        {
        //    if (From != null) 
		//			return From;
            return null;
        }
		//Get Default Value
        public int? GetDefaultFrom(View view = null)
        { 
			return From;
        }
		//Set Default Value
		public void SetDefaultFrom(View view = null)
        {
            //if (From is null){
            //    var result = GetDefaultFrom(view);
            //    if (result != null && result != From){
			//          From = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FromIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFrom();
				//if (result != null && From != null){
				//	return !From.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _to;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đến")]
        [ToolTip("Đến")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? To
        { 
		    get => GetPropertyValue<int?>("To");                         
			set => SetPropertyValue<int?>("To", value); 
			
        }
		//Tooltip for Object
		public object ToToolTipControllerText(View view)
        {
        //    if (To != null) 
		//			return To;
            return null;
        }
		//Get Default Value
        public int? GetDefaultTo(View view = null)
        { 
			return To;
        }
		//Set Default Value
		public void SetDefaultTo(View view = null)
        {
            //if (To is null){
            //    var result = GetDefaultTo(view);
            //    if (result != null && result != To){
			//          To = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ToIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTo();
				//if (result != null && To != null){
				//	return !To.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _score;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Điểm")]
        [ToolTip("Điểm")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Score
        { 
		    get => GetPropertyValue<int?>("Score");                         
			set => SetPropertyValue<int?>("Score", value); 
			
        }
		//Tooltip for Object
		public object ScoreToolTipControllerText(View view)
        {
        //    if (Score != null) 
		//			return Score;
            return null;
        }
		//Get Default Value
        public int? GetDefaultScore(View view = null)
        { 
			return Score;
        }
		//Set Default Value
		public void SetDefaultScore(View view = null)
        {
            //if (Score is null){
            //    var result = GetDefaultScore(view);
            //    if (result != null && result != Score){
			//          Score = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ScoreIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultScore();
				//if (result != null && Score != null){
				//	return !Score.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _order;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Xếp hạng")]
        [ToolTip("Xếp hạng")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Order
        { 
		    get => GetPropertyValue<int?>("Order");                         
			set => SetPropertyValue<int?>("Order", value); 
			
        }
		//Tooltip for Object
		public object OrderToolTipControllerText(View view)
        {
        //    if (Order != null) 
		//			return Order;
            return null;
        }
		//Get Default Value
        public int? GetDefaultOrder(View view = null)
        { 
			return Order;
        }
		//Set Default Value
		public void SetDefaultOrder(View view = null)
        {
            //if (Order is null){
            //    var result = GetDefaultOrder(view);
            //    if (result != null && result != Order){
			//          Order = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OrderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrder();
				//if (result != null && Order != null){
				//	return !Order.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dữ liệu")]
		//[Index(5)]
		[DevExpress.Xpo.Association("MatchJoin-MatchDataList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.MatchData> MatchDataList
        {      
		    get => GetCollection<Module.BusinessObjects.MatchData>("MatchDataList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giải thưởng")]
		//[Index(6)]
		[DataSourceCriteria("Not MatchJoinList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("MatchJoinList-PrizeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Prize> PrizeList
        {      
		    get => GetCollection<Module.BusinessObjects.Prize>("PrizeList"); 
			
        }
       
		//private Module.BusinessObjects.Team _team;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Team")]
        [ToolTip("Team")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TeamCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Team-MatchJoinList")]
	 
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
	
       
		//private Module.BusinessObjects.Match _match;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Match")]
        [ToolTip("Match")]
		//[Index(8)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MatchCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Match-MatchJoinList")]
	 
		public Module.BusinessObjects.Match Match
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Match>("Match");                         
			set => SetPropertyValue<Module.BusinessObjects.Match>("Match", value); 
			
        }
		//Tooltip for Object
		public object MatchToolTipControllerText(View view)
        {
        //    if (Match != null) 
		//			return Match;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Match GetDefaultMatch(View view = null)
        { 
			return Match;
        }
		//Set Default Value
		public void SetDefaultMatch(View view = null)
        {
            //if (Match is null){
            //    var result = GetDefaultMatch(view);
            //    if (result != null && result != Match){
			//          Match = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MatchIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMatch();
				//if (result != null && Match != null){
				//	return !Match.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator MatchCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Match));
            }
        }
	
       
		//private Module.BusinessObjects.Player _player;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Player")]
        [ToolTip("Player")]
		//[Index(9)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PlayerCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Player-MatchJoin")]
	 
		public Module.BusinessObjects.Player Player
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Player>("Player");                         
			set => SetPropertyValue<Module.BusinessObjects.Player>("Player", value); 
			
        }
		//Tooltip for Object
		public object PlayerToolTipControllerText(View view)
        {
        //    if (Player != null) 
		//			return Player;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Player GetDefaultPlayer(View view = null)
        { 
			return Player;
        }
		//Set Default Value
		public void SetDefaultPlayer(View view = null)
        {
            //if (Player is null){
            //    var result = GetDefaultPlayer(view);
            //    if (result != null && result != Player){
			//          Player = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PlayerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPlayer();
				//if (result != null && Player != null){
				//	return !Player.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PlayerCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Player));
            }
        }
	
       
		//private Module.BusinessObjects.TournamentSeason _tournamentseason;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mùa giải")]
        [ToolTip("Mùa giải")]
		//[Index(10)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TournamentSeasonCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("TournamentSeason-MatchJoinList")]
	 
		public Module.BusinessObjects.TournamentSeason TournamentSeason
        { 
		    get => GetPropertyValue<Module.BusinessObjects.TournamentSeason>("TournamentSeason");                         
			set => SetPropertyValue<Module.BusinessObjects.TournamentSeason>("TournamentSeason", value); 
			
        }
		//Tooltip for Object
		public object TournamentSeasonToolTipControllerText(View view)
        {
        //    if (TournamentSeason != null) 
		//			return TournamentSeason;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.TournamentSeason GetDefaultTournamentSeason(View view = null)
        { 
			return TournamentSeason;
        }
		//Set Default Value
		public void SetDefaultTournamentSeason(View view = null)
        {
            //if (TournamentSeason is null){
            //    var result = GetDefaultTournamentSeason(view);
            //    if (result != null && result != TournamentSeason){
			//          TournamentSeason = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TournamentSeasonIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTournamentSeason();
				//if (result != null && TournamentSeason != null){
				//	return !TournamentSeason.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TournamentSeasonCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(TournamentSeason));
            }
        }
	
       
		//private int? _duration;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thời lượng")]
        [ToolTip("Thời lượng")]
		//[Index(11)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Duration
        { 
		    #region 1632ImportCode 
get => To - From;
#endregion 1632ImportCode
			
        }
		//Tooltip for Object
		public object DurationToolTipControllerText(View view)
        {
        //    if (Duration != null) 
		//			return Duration;
            return null;
        }
		//Get Default Value
        public int? GetDefaultDuration(View view = null)
        { 
			return Duration;
        }
		//Set Default Value
		public void SetDefaultDuration(View view = null)
        {
            //if (Duration is null){
            //    var result = GetDefaultDuration(view);
            //    if (result != null && result != Duration){
			//          Duration = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DurationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDuration();
				//if (result != null && Duration != null){
				//	return !Duration.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultBusinessRole(View view = null);
        //SetDefaultFrom(View view = null);
        //SetDefaultTo(View view = null);
        //SetDefaultScore(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultTeam(View view = null);
        //SetDefaultMatch(View view = null);
        //SetDefaultPlayer(View view = null);
        //SetDefaultTournamentSeason(View view = null);
        //SetDefaultDuration(View view = null);
			
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
			//	SetDefaultMatchDataList();
			//	SetDefaultPrizeList();
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
