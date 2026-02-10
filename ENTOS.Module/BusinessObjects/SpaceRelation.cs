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
    [ModelDefault("Caption", "Quan hệ địa bàn"), ImageName("SpaceRelation")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(OrderLower)+ "," + nameof(OrderUpper))]
 
	[MobileColumnAttribute(Context = "Space_UpperLeftList_ListView", TargetItems = "RelationType.Upper"+ "," + nameof(RelationType)+ "," + nameof(OrderUpper))]
	[MobileColumnAttribute(Context = "SpaceRelation_LookupListView", TargetItems = nameof(Lower)+ "," + nameof(Upper)+ "," + nameof(RelationType))]
	[MobileColumnAttribute(Context = "SpaceRelation_ListView", TargetItems = nameof(Upper)+ "," + nameof(Lower)+ "," + nameof(RelationType))]
	[MobileColumnAttribute(Context = "Space_LowerRightList_ListView", TargetItems = nameof(OrderLower)+ "," + "RelationType.Lower"+ "," + nameof(RelationType))]
 
	[RuleCombinationOfPropertiesIsUnique("UniqueRule.SpaceRelation", DefaultContexts.Save, "OrderUpper, Lower", "OrderLower, Upper")]
[OptimisticLocking(true)]
    public partial class SpaceRelation:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public SpaceRelation(Session session)
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
               

		//private Module.BusinessObjects.RelationType _relationtype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại quan hệ")]
        [ToolTip("Loại quan hệ")]
		//[Index(0)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[ObjectTypeCode] = 'Space'")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.RelationType RelationType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.RelationType>("RelationType");                         
			set => SetPropertyValue<Module.BusinessObjects.RelationType>("RelationType", value); 
			
        }
		//Tooltip for Object
		public object RelationTypeToolTipControllerText(View view)
        {
        //    if (RelationType != null) 
		//			return RelationType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.RelationType GetDefaultRelationType(View view = null)
        { 
			return RelationType;
        }
		//Set Default Value
		public void SetDefaultRelationType(View view = null)
        {
            //if (RelationType is null){
            //    var result = GetDefaultRelationType(view);
            //    if (result != null && result != RelationType){
			//          RelationType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RelationTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRelationType();
				//if (result != null && RelationType != null){
				//	return !RelationType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator RelationTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(RelationType));
            }
        }
	
       
		//private Module.BusinessObjects.Space _upper;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(1)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[SpaceType.Country.Oid] = '@This.Lower.SpaceType.Country.Oid'")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Upper-LowerRightList")]
	 
		public Module.BusinessObjects.Space Upper
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Space>("Upper");                         
			set => SetPropertyValue<Module.BusinessObjects.Space>("Upper", value); 
			
        }
		//Tooltip for Object
		public object UpperToolTipControllerText(View view)
        {
        //    if (Upper != null) 
		//			return Upper;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Space GetDefaultUpper(View view = null)
        { 
			return Upper;
        }
		//Set Default Value
		public void SetDefaultUpper(View view = null)
        {
            //if (Upper is null){
            //    var result = GetDefaultUpper(view);
            //    if (result != null && result != Upper){
			//          Upper = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpper();
				//if (result != null && Upper != null){
				//	return !Upper.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpperCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Upper));
            }
        }
	
       
		//private Module.BusinessObjects.Space _lower;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cấp dưới")]
        [ToolTip("Cấp dưới")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[SpaceType.Country.Oid] = '@This.Lower.SpaceType.Country.Oid'")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Lower-UpperLeftList")]
	 
		public Module.BusinessObjects.Space Lower
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Space>("Lower");                         
			set => SetPropertyValue<Module.BusinessObjects.Space>("Lower", value); 
			
        }
		//Tooltip for Object
		public object LowerToolTipControllerText(View view)
        {
        //    if (Lower != null) 
		//			return Lower;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Space GetDefaultLower(View view = null)
        { 
			return Lower;
        }
		//Set Default Value
		public void SetDefaultLower(View view = null)
        {
            //if (Lower is null){
            //    var result = GetDefaultLower(view);
            //    if (result != null && result != Lower){
			//          Lower = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LowerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLower();
				//if (result != null && Lower != null){
				//	return !Lower.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator LowerCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Lower));
            }
        }
	
       
		//private decimal? _borderlength;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Biên giới")]
        [ToolTip("Biên giới")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? BorderLength
        { 
		    get => GetPropertyValue<decimal?>("BorderLength");                         
			set => SetPropertyValue<decimal?>("BorderLength", value); 
			
        }
		//Tooltip for Object
		public object BorderLengthToolTipControllerText(View view)
        {
        //    if (BorderLength != null) 
		//			return BorderLength;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultBorderLength(View view = null)
        { 
			return BorderLength;
        }
		//Set Default Value
		public void SetDefaultBorderLength(View view = null)
        {
            //if (BorderLength is null){
            //    var result = GetDefaultBorderLength(view);
            //    if (result != null && result != BorderLength){
			//          BorderLength = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool BorderLengthIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultBorderLength();
				//if (result != null && BorderLength != null){
				//	return !BorderLength.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _orderlower;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự dưới")]
        [ToolTip("Thứ tự dưới")]
		//[Index(4)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? OrderLower
        { 
		    get => GetPropertyValue<int?>("OrderLower");                         
			set => SetPropertyValue<int?>("OrderLower", value); 
			
        }
		//Tooltip for Object
		public object OrderLowerToolTipControllerText(View view)
        {
        //    if (OrderLower != null) 
		//			return OrderLower;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool OrderLowerIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrderLower();
				//if (result != null && OrderLower != null){
				//	return !OrderLower.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private int? _orderupper;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự trên")]
        [ToolTip("Thứ tự trên")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? OrderUpper
        { 
		    get => GetPropertyValue<int?>("OrderUpper");                         
			set => SetPropertyValue<int?>("OrderUpper", value); 
			
        }
		//Tooltip for Object
		public object OrderUpperToolTipControllerText(View view)
        {
        //    if (OrderUpper != null) 
		//			return OrderUpper;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool OrderUpperIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrderUpper();
				//if (result != null && OrderUpper != null){
				//	return !OrderUpper.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _relation;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quan hệ")]
        [ToolTip("Quan hệ")]
		//[Index(6)]		

 		[Size(100)]
	    [NotMapped()]
	    [NonPersistent()]
		public string Relation
        { 
		    get => GetPropertyValue<string>("Relation");                         
			set => SetPropertyValue<string>("Relation", value); 
			
        }
		//Tooltip for Object
		public object RelationToolTipControllerText(View view)
        {
        //    if (Relation != null) 
		//			return Relation;
            return null;
        }
		//Get Default Value
        public string GetDefaultRelation(View view = null)
        { 
			return Relation;
        }
		//Set Default Value
		public void SetDefaultRelation(View view = null)
        {
            //if (Relation is null){
            //    var result = GetDefaultRelation(view);
            //    if (result != null && result != Relation){
			//          Relation = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RelationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRelation();
				//if (result != null && Relation != null){
				//	return !Relation.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Space _space;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Vị trí")]
        [ToolTip("Vị trí")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[SpaceType.Country.Oid] = '@This.Lower.SpaceType.Country.Oid'")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [NotMapped()]
	    [NonPersistent()]
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
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultRelationType(View view = null);
        //SetDefaultUpper(View view = null);
        //SetDefaultLower(View view = null);
        //SetDefaultBorderLength(View view = null);
        //SetDefaultOrderLower(View view = null);
        //SetDefaultOrderUpper(View view = null);
        //SetDefaultRelation(View view = null);
        //SetDefaultSpace(View view = null);
			
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
				
                    case nameof(Lower):
                        OnChangedLower(oldValue, newValue);
                        break;
				
                    case nameof(Upper):
                        OnChangedUpper(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedLower(object oldValue, object newValue)
        {
            #region 1536ImportCode
            if (newValue is null) return;
SetDefaultOrderUpper();            
            #endregion 1536ImportCode
        }               
        private void OnChangedUpper(object oldValue, object newValue)
        {
            #region 1532ImportCode
            if (newValue is null) return;
SetDefaultOrderLower();            
            #endregion 1532ImportCode
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
#region 1535ImportCode
		public void SetDefaultOrderUpper(View view = null)
        {
            //Code: 1535            Oid: 0c652374-a155-4e4f-a512-626b7e7eb40a
            if(OrderUpper == null) OrderUpper = GetDefaultOrderUpper();

        }
#endregion 1535ImportCode
#region 1534ImportCode
		public int? GetDefaultOrderUpper(View view = null)
        {
            //Code: 1534            Oid: e146b328-da55-48c8-83c3-5b772441c442
            if (Lower != null && Lower.UpperLeftList != null)
{
    var lasted = Lower.UpperLeftList.Where(m => m.OrderUpper != null).OrderByDescending(m => m.OrderUpper).FirstOrDefault();
    if (lasted != null)
        return lasted.OrderUpper + 1;
    return 1;
}
return null;
        }
#endregion 1534ImportCode
#region 1533ImportCode
		public void SetDefaultOrderLower(View view = null)
        {
            //Code: 1533            Oid: 5c7c329d-7485-4c0c-98a4-d29d1b32634e
            if(OrderLower == null) OrderLower = GetDefaultOrderLower();

        }
#endregion 1533ImportCode
#region 1531ImportCode
		public int? GetDefaultOrderLower(View view = null)
        {
            //Code: 1531            Oid: a2a058c3-02b4-4ffc-a500-644142a1a98c
            if (Upper != null && Upper.LowerRightList != null)
{
    var lasted = Upper.LowerRightList.Where(m => m.OrderLower != null).OrderByDescending(m => m.OrderLower).FirstOrDefault();
    if (lasted != null)
        return lasted.OrderLower + 1;
    return 1;
}
return null;
        }
#endregion 1531ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
