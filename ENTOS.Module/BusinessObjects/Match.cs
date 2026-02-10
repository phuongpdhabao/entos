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
    [ModelDefault("Caption", "Trận đấu"), ImageName("Match")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "Match_ListView", TargetItems = nameof(Name)+ "," + nameof(Date)+ "," + nameof(TournamentSeason))]
	[MobileColumnAttribute(Context = "TournamentSeason_MatchList_ListView", TargetItems = nameof(Name)+ "," + nameof(Date)+ "," + nameof(Time))]
	[MobileColumnAttribute(Context = "TournamentRound_MatchList_ListView", TargetItems = nameof(Date)+ "," + nameof(Name)+ "," + nameof(Time))]
	[MobileColumnAttribute(Context = "Match_LookupListView", TargetItems = nameof(Venue)+ "," + nameof(TournamentRound)+ "," + nameof(TournamentSeason))]
	[MobileColumnAttribute(Context = "Venue_MatchList_ListView", TargetItems = nameof(Time)+ "," + nameof(Date)+ "," + nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Match:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Match(Session session)
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
                        //if (Session.FindObject<Module.BusinessObjects.MatchJoin>(CriteriaOperator.Parse("[Match.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool matchjoinlist = Session.Query<Module.BusinessObjects.MatchJoin>().Where(x => x.Match.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(MatchJoinList), matchjoinlist);
                        if (matchjoinlist)
                            return true;

                    }                    
                }				
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
                        //if (Session.FindObject<Module.BusinessObjects.MatchData>(CriteriaOperator.Parse("[Match.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool matchdatalist = Session.Query<Module.BusinessObjects.MatchData>().Where(x => x.Match.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(MatchDataList), matchdatalist);
                        if (matchdatalist)
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
                        //if (Session.FindObject<Module.BusinessObjects.Post>(CriteriaOperator.Parse("[Match.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool postlist = Session.Query<Module.BusinessObjects.Post>().Where(x => x.Match.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
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
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(0)]		

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
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bộ môn")]
        [ToolTip("Bộ môn")]
		//[Index(1)]		
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
	
       
		//private DateTime? _date;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
		public DateTime? Date
        { 
		    get => GetPropertyValue<DateTime?>("Date");                         
			set => SetPropertyValue<DateTime?>("Date", value); 
			
        }
		//Tooltip for Object
		public object DateToolTipControllerText(View view)
        {
        //    if (Date != null) 
		//			return Date;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultDate(View view = null)
        { 
			return Date;
        }
		//Set Default Value
		public void SetDefaultDate(View view = null)
        {
            //if (Date is null){
            //    var result = GetDefaultDate(view);
            //    if (result != null && result != Date){
			//          Date = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDate();
				//if (result != null && Date != null){
				//	return !Date.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _time;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giờ")]
        [ToolTip("Giờ")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "H:mm")]
		[ModelDefault("EditMask", "H:mm")]
		public DateTime? Time
        { 
		    get => GetPropertyValue<DateTime?>("Time");                         
			set => SetPropertyValue<DateTime?>("Time", value); 
			
        }
		//Tooltip for Object
		public object TimeToolTipControllerText(View view)
        {
        //    if (Time != null) 
		//			return Time;
            return null;
        }
		//Get Default Value
        public DateTime? GetDefaultTime(View view = null)
        { 
			return Time;
        }
		//Set Default Value
		public void SetDefaultTime(View view = null)
        {
            //if (Time is null){
            //    var result = GetDefaultTime(view);
            //    if (result != null && result != Time){
			//          Time = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TimeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTime();
				//if (result != null && Time != null){
				//	return !Time.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Venue _venue;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Địa điểm")]
        [ToolTip("Địa điểm")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(VenueCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Venue-MatchList")]
	 
		public Module.BusinessObjects.Venue Venue
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Venue>("Venue");                         
			set => SetPropertyValue<Module.BusinessObjects.Venue>("Venue", value); 
			
        }
		//Tooltip for Object
		public object VenueToolTipControllerText(View view)
        {
        //    if (Venue != null) 
		//			return Venue;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Venue GetDefaultVenue(View view = null)
        { 
			return Venue;
        }
		//Set Default Value
		public void SetDefaultVenue(View view = null)
        {
            //if (Venue is null){
            //    var result = GetDefaultVenue(view);
            //    if (result != null && result != Venue){
			//          Venue = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool VenueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultVenue();
				//if (result != null && Venue != null){
				//	return !Venue.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator VenueCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Venue));
            }
        }
	
       
		//private Module.BusinessObjects.TournamentSeason _tournamentseason;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mùa giải")]
        [ToolTip("Mùa giải")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TournamentSeasonCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("TournamentSeason-MatchList")]
	 
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
	
       
		//private Module.BusinessObjects.TournamentRound _tournamentround;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Vòng đấu")]
        [ToolTip("Vòng đấu")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TournamentRoundCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("TournamentRound-MatchList")]
	 
		public Module.BusinessObjects.TournamentRound TournamentRound
        { 
		    get => GetPropertyValue<Module.BusinessObjects.TournamentRound>("TournamentRound");                         
			set => SetPropertyValue<Module.BusinessObjects.TournamentRound>("TournamentRound", value); 
			
        }
		//Tooltip for Object
		public object TournamentRoundToolTipControllerText(View view)
        {
        //    if (TournamentRound != null) 
		//			return TournamentRound;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.TournamentRound GetDefaultTournamentRound(View view = null)
        { 
			return TournamentRound;
        }
		//Set Default Value
		public void SetDefaultTournamentRound(View view = null)
        {
            //if (TournamentRound is null){
            //    var result = GetDefaultTournamentRound(view);
            //    if (result != null && result != TournamentRound){
			//          TournamentRound = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TournamentRoundIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTournamentRound();
				//if (result != null && TournamentRound != null){
				//	return !TournamentRound.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TournamentRoundCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(TournamentRound));
            }
        }
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tham gia")]
		//[Index(7)]
		[DevExpress.Xpo.Association("Match-MatchJoinList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.MatchJoin> MatchJoinList
        {      
		    get => GetCollection<Module.BusinessObjects.MatchJoin>("MatchJoinList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Dữ liệu")]
		//[Index(8)]
		[DevExpress.Xpo.Association("Match-MatchDataList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.MatchData> MatchDataList
        {      
		    get => GetCollection<Module.BusinessObjects.MatchData>("MatchDataList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tin tức")]
		//[Index(9)]
		[DevExpress.Xpo.Association("Match-PostList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Post> PostList
        {      
		    get => GetCollection<Module.BusinessObjects.Post>("PostList"); 
			
        }
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultDomain(View view = null);
        //SetDefaultDate(View view = null);
        //SetDefaultTime(View view = null);
        //SetDefaultVenue(View view = null);
        //SetDefaultTournamentSeason(View view = null);
        //SetDefaultTournamentRound(View view = null);
			
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
			//	SetDefaultMatchJoinList();
			//	SetDefaultMatchDataList();
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
