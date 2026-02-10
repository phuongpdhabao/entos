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
    [ModelDefault("Caption", "Dữ liệu trận đấu"), ImageName("MatchData")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "MatchData_LookupListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "MatchData_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Match_MatchDataList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "MatchJoin_MatchDataList_ListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class MatchData:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public MatchData(Session session)
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
               

		//private Module.BusinessObjects.SubjectData _subjectdata;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tiêu chí")]
        [ToolTip("Tiêu chí")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SubjectDataCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.SubjectData SubjectData
        { 
		    get => GetPropertyValue<Module.BusinessObjects.SubjectData>("SubjectData");                         
			set => SetPropertyValue<Module.BusinessObjects.SubjectData>("SubjectData", value); 
			
        }
		//Tooltip for Object
		public object SubjectDataToolTipControllerText(View view)
        {
        //    if (SubjectData != null) 
		//			return SubjectData;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.SubjectData GetDefaultSubjectData(View view = null)
        { 
			return SubjectData;
        }
		//Set Default Value
		public void SetDefaultSubjectData(View view = null)
        {
            //if (SubjectData is null){
            //    var result = GetDefaultSubjectData(view);
            //    if (result != null && result != SubjectData){
			//          SubjectData = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SubjectDataIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSubjectData();
				//if (result != null && SubjectData != null){
				//	return !SubjectData.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SubjectDataCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SubjectData));
            }
        }
	
       
		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số liệu")]
        [ToolTip("Số liệu")]
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

	
       
		//private Module.BusinessObjects.MatchJoin _matchjoin;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tham gia trận đấu")]
        [ToolTip("Tham gia trận đấu")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MatchJoinCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("MatchJoin-MatchDataList")]
	 
		public Module.BusinessObjects.MatchJoin MatchJoin
        { 
		    get => GetPropertyValue<Module.BusinessObjects.MatchJoin>("MatchJoin");                         
			set => SetPropertyValue<Module.BusinessObjects.MatchJoin>("MatchJoin", value); 
			
        }
		//Tooltip for Object
		public object MatchJoinToolTipControllerText(View view)
        {
        //    if (MatchJoin != null) 
		//			return MatchJoin;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.MatchJoin GetDefaultMatchJoin(View view = null)
        { 
			return MatchJoin;
        }
		//Set Default Value
		public void SetDefaultMatchJoin(View view = null)
        {
            //if (MatchJoin is null){
            //    var result = GetDefaultMatchJoin(view);
            //    if (result != null && result != MatchJoin){
			//          MatchJoin = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool MatchJoinIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMatchJoin();
				//if (result != null && MatchJoin != null){
				//	return !MatchJoin.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator MatchJoinCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(MatchJoin));
            }
        }
	
       
		//private Module.BusinessObjects.Match _match;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Trận đấu")]
        [ToolTip("Trận đấu")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MatchCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Match-MatchDataList")]
	 
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
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultSubjectData(View view = null);
        //SetDefaultName(View view = null);
        //SetDefaultMatchJoin(View view = null);
        //SetDefaultMatch(View view = null);
			
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
