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
	[NavigationItem("ProductBusiness")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Chính sách giá"), ImageName("PricePolicy")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "PricePolicy_ListView", TargetItems = nameof(Field)+ "," + nameof(CurrencyType)+ "," + nameof(Name))]
	[MobileColumnAttribute(Context = "PricePolicy_LookupListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class PricePolicy:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public PricePolicy(Session session)
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
				if (PriceParameterList.IsLoaded)
                {
                    if (PriceParameterList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(PriceParameterList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(PriceParameterList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.PriceParameter>(CriteriaOperator.Parse("[PricePolicy.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool priceparameterlist = Session.Query<Module.BusinessObjects.PriceParameter>().Where(x => x.PricePolicy.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(PriceParameterList), priceparameterlist);
                        if (priceparameterlist)
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

	
       
		//private string _describe;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mô tả")]
        [ToolTip("Mô tả")]
		//[Index(1)]		

 		[Size(150)]
		public string Describe
        { 
		    get => GetPropertyValue<string>("Describe");                         
			set => SetPropertyValue<string>("Describe", value); 
			
        }
		//Tooltip for Object
		public object DescribeToolTipControllerText(View view)
        {
        //    if (Describe != null) 
		//			return Describe;
            return null;
        }
		//Get Default Value
        public string GetDefaultDescribe(View view = null)
        { 
			return Describe;
        }
		//Set Default Value
		public void SetDefaultDescribe(View view = null)
        {
            //if (Describe is null){
            //    var result = GetDefaultDescribe(view);
            //    if (result != null && result != Describe){
			//          Describe = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool DescribeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultDescribe();
				//if (result != null && Describe != null){
				//	return !Describe.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _field;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Trường")]
        [ToolTip("Trường")]
		//[Index(2)]		

 		[Size(100)]
		public string Field
        { 
		    get => GetPropertyValue<string>("Field");                         
			set => SetPropertyValue<string>("Field", value); 
			
        }
		//Tooltip for Object
		public object FieldToolTipControllerText(View view)
        {
        //    if (Field != null) 
		//			return Field;
            return null;
        }
		//Get Default Value
        public string GetDefaultField(View view = null)
        { 
			return Field;
        }
		//Set Default Value
		public void SetDefaultField(View view = null)
        {
            //if (Field is null){
            //    var result = GetDefaultField(view);
            //    if (result != null && result != Field){
			//          Field = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FieldIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultField();
				//if (result != null && Field != null){
				//	return !Field.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _emptyvalue;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Giá trị trống")]
        [ToolTip("Giá trị trống")]
		//[Index(3)]		

 		[Size(150)]
		public string EmptyValue
        { 
		    get => GetPropertyValue<string>("EmptyValue");                         
			set => SetPropertyValue<string>("EmptyValue", value); 
			
        }
		//Tooltip for Object
		public object EmptyValueToolTipControllerText(View view)
        {
        //    if (EmptyValue != null) 
		//			return EmptyValue;
            return null;
        }
		//Get Default Value
        public string GetDefaultEmptyValue(View view = null)
        { 
			return EmptyValue;
        }
		//Set Default Value
		public void SetDefaultEmptyValue(View view = null)
        {
            //if (EmptyValue is null){
            //    var result = GetDefaultEmptyValue(view);
            //    if (result != null && result != EmptyValue){
			//          EmptyValue = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool EmptyValueIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultEmptyValue();
				//if (result != null && EmptyValue != null){
				//	return !EmptyValue.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Round _round;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Làm tròn")]
        [ToolTip("Làm tròn")]
		//[Index(4)]		
		public Round Round
        { 
		    get => GetPropertyValue<Round>("Round");                         
			set => SetPropertyValue<Round>("Round", value); 
			
        }
		//Tooltip for Object
		public object RoundToolTipControllerText(View view)
        {
        //    if (Round != null) 
		//			return Round;
            return null;
        }
		//Get Default Value
        public Round GetDefaultRound(View view = null)
        { 
			return Round;
        }
		//Set Default Value
		public void SetDefaultRound(View view = null)
        {
            //if (Round is null){
            //    var result = GetDefaultRound(view);
            //    if (result != null && result != Round){
			//          Round = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool RoundIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultRound();
				//if (result != null && Round != null){
				//	return !Round.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Currency _currencytype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại tiền")]
        [ToolTip("Loại tiền")]
		//[Index(5)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CurrencyTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Currency CurrencyType
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Currency>("CurrencyType");                         
			set => SetPropertyValue<Module.BusinessObjects.Currency>("CurrencyType", value); 
			
        }
		//Tooltip for Object
		public object CurrencyTypeToolTipControllerText(View view)
        {
        //    if (CurrencyType != null) 
		//			return CurrencyType;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Currency GetDefaultCurrencyType(View view = null)
        { 
			return CurrencyType;
        }
		//Set Default Value
		public void SetDefaultCurrencyType(View view = null)
        {
            //if (CurrencyType is null){
            //    var result = GetDefaultCurrencyType(view);
            //    if (result != null && result != CurrencyType){
			//          CurrencyType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CurrencyTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCurrencyType();
				//if (result != null && CurrencyType != null){
				//	return !CurrencyType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CurrencyTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(CurrencyType));
            }
        }
	
       
		//private string _expression;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Biểu thức")]
        [ToolTip("Biểu thức")]
		//[Index(6)]		

 		[Size(150)]
		public string Expression
        { 
		    get => GetPropertyValue<string>("Expression");                         
			set => SetPropertyValue<string>("Expression", value); 
			
        }
		//Tooltip for Object
		public object ExpressionToolTipControllerText(View view)
        {
        //    if (Expression != null) 
		//			return Expression;
            return null;
        }
		//Get Default Value
        public string GetDefaultExpression(View view = null)
        { 
			return Expression;
        }
		//Set Default Value
		public void SetDefaultExpression(View view = null)
        {
            //if (Expression is null){
            //    var result = GetDefaultExpression(view);
            //    if (result != null && result != Expression){
			//          Expression = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ExpressionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultExpression();
				//if (result != null && Expression != null){
				//	return !Expression.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _expressions;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Biểu thức")]
        [ToolTip("Biểu thức")]
		//[Index(7)]		

 		[Size(200)]
		public string Expressions
        { 
		    get => GetPropertyValue<string>("Expressions");                         
			set => SetPropertyValue<string>("Expressions", value); 
			
        }
		//Tooltip for Object
		public object ExpressionsToolTipControllerText(View view)
        {
        //    if (Expressions != null) 
		//			return Expressions;
            return null;
        }
		//Get Default Value
        public string GetDefaultExpressions(View view = null)
        { 
			return Expressions;
        }
		//Set Default Value
		public void SetDefaultExpressions(View view = null)
        {
            //if (Expressions is null){
            //    var result = GetDefaultExpressions(view);
            //    if (result != null && result != Expressions){
			//          Expressions = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ExpressionsIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultExpressions();
				//if (result != null && Expressions != null){
				//	return !Expressions.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tham số")]
		//[Index(8)]
		[DevExpress.Xpo.Association("PricePolicy-PriceParameterList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.PriceParameter> PriceParameterList
        {      
		    get => GetCollection<Module.BusinessObjects.PriceParameter>("PriceParameterList"); 
			
        }
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultName(View view = null);
        //SetDefaultDescribe(View view = null);
        //SetDefaultField(View view = null);
        //SetDefaultEmptyValue(View view = null);
        //SetDefaultRound(View view = null);
        //SetDefaultCurrencyType(View view = null);
        //SetDefaultExpression(View view = null);
        //SetDefaultExpressions(View view = null);
			
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
			//	SetDefaultPriceParameterList();
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
