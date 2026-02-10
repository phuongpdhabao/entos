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
    [ModelDefault("Caption", "Giải thưởng"), ImageName("Prize")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Order))]
 
	[MobileColumnAttribute(Context = "MatchJoin_PrizeList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Tournament_PrizeList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "TournamentSeason_PrizeList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Prize_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Prize_LookupListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Prize:  DevExpress.Xpo.XPLiteObject , IReOrder , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public Prize(Session session)
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

	
       
		//private TournamentType _tournamenttype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại giải")]
        [ToolTip("Loại giải")]
		//[Index(1)]		
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

	
       
		//private PrizeType _prizetype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại thưởng")]
        [ToolTip("Loại thưởng")]
		//[Index(2)]		
		public PrizeType PrizeType
        { 
		    get => GetPropertyValue<PrizeType>("PrizeType");                         
			set => SetPropertyValue<PrizeType>("PrizeType", value); 
			
        }
		//Tooltip for Object
		public object PrizeTypeToolTipControllerText(View view)
        {
        //    if (PrizeType != null) 
		//			return PrizeType;
            return null;
        }
		//Get Default Value
        public PrizeType GetDefaultPrizeType(View view = null)
        { 
			return PrizeType;
        }
		//Set Default Value
		public void SetDefaultPrizeType(View view = null)
        {
            //if (PrizeType is null){
            //    var result = GetDefaultPrizeType(view);
            //    if (result != null && result != PrizeType){
			//          PrizeType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PrizeTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPrizeType();
				//if (result != null && PrizeType != null){
				//	return !PrizeType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _value;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giá trị")]
        [ToolTip("Giá trị")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Value
        { 
		    get => GetPropertyValue<int?>("Value");                         
			set => SetPropertyValue<int?>("Value", value); 
			
        }
		//Tooltip for Object
		public object ValueToolTipControllerText(View view)
        {
        //    if (Value != null) 
		//			return Value;
            return null;
        }
		//Get Default Value
        public int? GetDefaultValue(View view = null)
        { 
			return Value;
        }
		//Set Default Value
		public void SetDefaultValue(View view = null)
        {
            //if (Value is null){
            //    var result = GetDefaultValue(view);
            //    if (result != null && result != Value){
			//          Value = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultValue();
				//if (result != null && Value != null){
				//	return !Value.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _quantity;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Quantity
        { 
		    get => GetPropertyValue<int?>("Quantity");                         
			set => SetPropertyValue<int?>("Quantity", value); 
			
        }
		//Tooltip for Object
		public object QuantityToolTipControllerText(View view)
        {
        //    if (Quantity != null) 
		//			return Quantity;
            return null;
        }
		//Get Default Value
        public int? GetDefaultQuantity(View view = null)
        { 
			return Quantity;
        }
		//Set Default Value
		public void SetDefaultQuantity(View view = null)
        {
            //if (Quantity is null){
            //    var result = GetDefaultQuantity(view);
            //    if (result != null && result != Quantity){
			//          Quantity = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool QuantityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultQuantity();
				//if (result != null && Quantity != null){
				//	return !Quantity.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhận giải")]
		//[Index(5)]
		[DataSourceCriteria("Not PrizeList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("MatchJoinList-PrizeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.MatchJoin> MatchJoinList
        {      
		    get => GetCollection<Module.BusinessObjects.MatchJoin>("MatchJoinList"); 
			
        }
       
		//private Module.BusinessObjects.Tournament _tournament;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giải đấu")]
        [ToolTip("Giải đấu")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TournamentCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Tournament-PrizeList")]
	 
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
	
       
		//private Module.BusinessObjects.TournamentSeason _tournamentseason;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mùa giải")]
        [ToolTip("Mùa giải")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TournamentSeasonCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("TournamentSeason-PrizeList")]
	 
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
	
       
		//private int? _order;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(8)]		
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
		//Set Default Value

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
 
            #region 2502ImportCode
            base.AfterConstruction();
Quantity = 1;
            #endregion 2502ImportCode
 
        //SetDefaultName(View view = null);
        //SetDefaultTournamentType(View view = null);
        //SetDefaultPrizeType(View view = null);
        //SetDefaultValue(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultTournament(View view = null);
        //SetDefaultTournamentSeason(View view = null);
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

                switch (propertyName)
                {       
				
                    case nameof(Tournament):
                        OnChangedTournament(oldValue, newValue);
                        break;
				
                    case nameof(TournamentSeason):
                        OnChangedTournamentSeason(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedTournament(object oldValue, object newValue)
        {
            #region 2501ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 2501ImportCode
        }               
        private void OnChangedTournamentSeason(object oldValue, object newValue)
        {
            #region 2504ImportCode
            if (newValue is null) return;
SetDefaultOrder();            
            #endregion 2504ImportCode
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
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 2500ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 2500            Oid: 8bb1c657-d5b6-4e82-a6c3-0037d044812c
            Order= GetDefaultOrder();
        }
#endregion 2500ImportCode
#region 2499ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 2499            Oid: 30fd09ae-9dc6-4db1-9b39-7e95c38bf18a
            if (TournamentSeason != null && TournamentSeason.PrizeList != null)
{
    var lasted = TournamentSeason.PrizeList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
if (Tournament != null && Tournament.PrizeList != null)
{
    var lasted = Tournament.PrizeList.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
return null;
        }
#endregion 2499ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
