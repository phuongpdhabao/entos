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
    [ModelDefault("Caption", "Vòng đấu"), ImageName("TournamentRound")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "Tournament_TournamentRoundList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "TournamentRound_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "TournamentSeason_TournamentRoundList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "TournamentRound_ListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class TournamentRound:  DevExpress.Xpo.XPLiteObject , IReOrder , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public TournamentRound(Session session)
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
                        //if (Session.FindObject<Module.BusinessObjects.Match>(CriteriaOperator.Parse("[TournamentRound.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool matchlist = Session.Query<Module.BusinessObjects.Match>().Where(x => x.TournamentRound.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(MatchList), matchlist);
                        if (matchlist)
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

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trận đấu")]
		//[Index(1)]
		[DevExpress.Xpo.Association("TournamentRound-MatchList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Match> MatchList
        {      
		    get => GetCollection<Module.BusinessObjects.Match>("MatchList"); 
			
        }
       
		//private Module.BusinessObjects.TournamentSeason _tournamentseason;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mùa giải")]
        [ToolTip("Mùa giải")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TournamentSeasonCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("TournamentSeason-TournamentRoundList")]
	 
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
	
       
		//private Module.BusinessObjects.Tournament _tournament;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giải đấu")]
        [ToolTip("Giải đấu")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TournamentCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Tournament-TournamentRoundList")]
	 
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
	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
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

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultTournamentSeason(View view = null);
        //SetDefaultTournament(View view = null);
        //SetDefaultOrder(View view = null);
			
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
			//	SetDefaultMatchList();
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
