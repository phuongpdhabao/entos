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
    [ModelDefault("Caption", "Mã HS"), ImageName("HScode")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
 
 
 
	[MobileColumnAttribute(Context = "HScode_ListView", TargetItems = nameof(TaxRate)+ "," + nameof(Code)+ "," + nameof(Describe))]
	[MobileColumnAttribute(Context = "HScode_LookupListView", TargetItems = nameof(Code)+ "," + nameof(Code))]
	[DefaultProperty("Code")]
 
//[OptimisticLocking(false)]
    public partial class HScode: DevExpress.Persistent.BaseImpl.BaseObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public HScode(Session session)
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
				if (TaxRateList.IsLoaded)
                {
                    if (TaxRateList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(TaxRateList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(TaxRateList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.TaxRate>(CriteriaOperator.Parse("[HScode.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool taxratelist = Session.Query<Module.BusinessObjects.TaxRate>().Where(x => x.HScode.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(TaxRateList), taxratelist);
                        if (taxratelist)
                            return true;

                    }                    
                }				
				if (ProductTypeList.IsLoaded)
                {
                    if (ProductTypeList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ProductTypeList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ProductTypeList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ProductType>(CriteriaOperator.Parse("[HSCode.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool producttypelist = Session.Query<Module.BusinessObjects.ProductType>().Where(x => x.HSCode.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ProductTypeList), producttypelist);
                        if (producttypelist)
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

               

		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã HS")]
        [ToolTip("Mã HS")]
		//[Index(0)]		

 		[Size(100)]
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

	
       
		//private string _describe;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mô tả")]
        [ToolTip("Mô tả")]
		//[Index(1)]		

 		[Size(100)]
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

	
       
		//private string _url;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("URL")]
        [ToolTip("URL")]
		//[Index(2)]		

 		[Size(150)]
		public string URL
        { 
		    get => GetPropertyValue<string>("URL");                         
			set => SetPropertyValue<string>("URL", value); 
			
        }
		//Tooltip for Object
		public object URLToolTipControllerText(View view)
        {
        //    if (URL != null) 
		//			return URL;
            return null;
        }
		//Get Default Value
        public string GetDefaultURL(View view = null)
        { 
			return URL;
        }
		//Set Default Value
		public void SetDefaultURL(View view = null)
        {
            //if (URL is null){
            //    var result = GetDefaultURL(view);
            //    if (result != null && result != URL){
			//          URL = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool URLIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultURL();
				//if (result != null && URL != null){
				//	return !URL.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuế suất")]
		//[Index(3)]
		[DevExpress.Xpo.Association("HScode-TaxRateList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.TaxRate> TaxRateList
        {      
		    get => GetCollection<Module.BusinessObjects.TaxRate>("TaxRateList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại sản phẩm")]
		//[Index(4)]
		[DevExpress.Xpo.Association("HSCode-ProductTypeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductType> ProductTypeList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductType>("ProductTypeList"); 
			
        }
       
		//private decimal _taxrate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thuế suất")]
        [ToolTip("Thuế suất")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:p0}")]
		[ModelDefault("EditMask", "p0")]
		public decimal TaxRate
        { 
		    get => GetPropertyValue<decimal>("TaxRate");                         
			set => SetPropertyValue<decimal>("TaxRate", value); 
			
        }
		//Tooltip for Object
		public object TaxRateToolTipControllerText(View view)
        {
        //    if (TaxRate != null) 
		//			return TaxRate;
            return null;
        }
		//Get Default Value
        public decimal GetDefaultTaxRate(View view = null)
        { 
			return TaxRate;
        }
		//Set Default Value
		public void SetDefaultTaxRate(View view = null)
        {
            //if (TaxRate is null){
            //    var result = GetDefaultTaxRate(view);
            //    if (result != null && result != TaxRate){
			//          TaxRate = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TaxRateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTaxRate();
				//if (result != null && TaxRate != null){
				//	return !TaxRate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
 
            base.AfterConstruction();
 
        //SetDefaultCode(View view = null);
        //SetDefaultDescribe(View view = null);
        //SetDefaultURL(View view = null);
        //SetDefaultTaxRate(View view = null);
			
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
			//	SetDefaultTaxRateList();
			//	SetDefaultProductTypeList();
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
