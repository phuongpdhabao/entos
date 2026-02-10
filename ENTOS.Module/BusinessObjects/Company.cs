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
	[NavigationItem("Invest")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Công ty"), ImageName("Company")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(CompanyType)+ "," + nameof(Country)+ "," + nameof(Space)+ "," + nameof(Capital)+ "," + nameof(Currency)+ "," + nameof(Leader)+ "," + nameof(LeaderImage)+ "," + nameof(StockExchange)+ "," + nameof(Member)+ "," + nameof(Quantity)+ "," + nameof(Folder), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Member)+ "," + nameof(Update)+ "," + nameof(Updater))]
 
	[MobileColumnAttribute(Context = "Company_LookupListView", TargetItems = nameof(Currency)+ "," + nameof(CompanyType)+ "," + nameof(Link)+ "," + nameof(Update)+ "," + nameof(Image)+ "," + nameof(StockExchange)+ "," + nameof(Code)+ "," + nameof(Country)+ "," + nameof(Capital))]
	[MobileColumnAttribute(Context = "Folder_CompanyList_ListView", TargetItems = nameof(Image)+ "," + nameof(Code))]
	[MobileColumnAttribute(Context = "Domain_CompanyList_ListView", TargetItems = nameof(Code)+ "," + nameof(StockExchange)+ "," + nameof(Image))]
	[MobileColumnAttribute(Context = "Company_ListView", TargetItems = nameof(Image)+ "," + nameof(Code))]
	[DefaultProperty("Code")]
 
[OptimisticLocking(true)]
    public partial class Company:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public Company(Session session)
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
				if (ShareHolderList.IsLoaded)
                {
                    if (ShareHolderList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ShareHolderList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ShareHolderList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ShareHolder>(CriteriaOperator.Parse("[Company.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool shareholderlist = Session.Query<Module.BusinessObjects.ShareHolder>().Where(x => x.Company.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ShareHolderList), shareholderlist);
                        if (shareholderlist)
                            return true;

                    }                    
                }				
				if (BookMarkList.IsLoaded)
                {
                    if (BookMarkList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(BookMarkList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(BookMarkList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.BookMark>(CriteriaOperator.Parse("[Company.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool bookmarklist = Session.Query<Module.BusinessObjects.BookMark>().Where(x => x.Company.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(BookMarkList), bookmarklist);
                        if (bookmarklist)
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

 		[Size(20)]
		[RuleUniqueValue("UniqueCompanyCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredCompanyCode", DefaultContexts.Save)]
		public string Code
        { 
		    get => GetPropertyValue<string>("Code");                         
			set => SetPropertyValue<string>("Code", value); 
			
        }
		//Tooltip for Object
		public object CodeToolTipControllerText(View view)
        {
            if (Introduction != null) 
				return string.Format("{0}",Introduction);
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

	
       
		//private string _introduction;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Giới thiệu")]
        [ToolTip("Giới thiệu")]
		//[Index(1)]		

 		[Size(250)]
		public string Introduction
        { 
		    get => GetPropertyValue<string>("Introduction");                         
			set => SetPropertyValue<string>("Introduction", value); 
			
        }
		//Tooltip for Object
		public object IntroductionToolTipControllerText(View view)
        {
        //    if (Introduction != null) 
		//			return Introduction;
            return null;
        }
		//Get Default Value
        public string GetDefaultIntroduction(View view = null)
        { 
			return Introduction;
        }
		//Set Default Value
		public void SetDefaultIntroduction(View view = null)
        {
            //if (Introduction is null){
            //    var result = GetDefaultIntroduction(view);
            //    if (result != null && result != Introduction){
			//          Introduction = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool IntroductionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultIntroduction();
				//if (result != null && Introduction != null){
				//	return !Introduction.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private CompanyType _companytype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(2)]		
		public CompanyType CompanyType
        { 
		    get => GetPropertyValue<CompanyType>("CompanyType");                         
			set => SetPropertyValue<CompanyType>("CompanyType", value); 
			
        }
		//Tooltip for Object
		public object CompanyTypeToolTipControllerText(View view)
        {
        //    if (CompanyType != null) 
		//			return CompanyType;
            return null;
        }
		//Get Default Value
        public CompanyType GetDefaultCompanyType(View view = null)
        { 
			return CompanyType;
        }
		//Set Default Value
		public void SetDefaultCompanyType(View view = null)
        {
            //if (CompanyType is null){
            //    var result = GetDefaultCompanyType(view);
            //    if (result != null && result != CompanyType){
			//          CompanyType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CompanyTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCompanyType();
				//if (result != null && CompanyType != null){
				//	return !CompanyType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Country _country;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quốc gia")]
        [ToolTip("Quốc gia")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CountryCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Country Country
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Country>("Country");                         
			set => SetPropertyValue<Module.BusinessObjects.Country>("Country", value); 
			
        }
		//Tooltip for Object
		public object CountryToolTipControllerText(View view)
        {
        //    if (Country != null) 
		//			return Country;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Country GetDefaultCountry(View view = null)
        { 
			return Country;
        }
		//Set Default Value
		public void SetDefaultCountry(View view = null)
        {
            //if (Country is null){
            //    var result = GetDefaultCountry(view);
            //    if (result != null && result != Country){
			//          Country = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CountryIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCountry();
				//if (result != null && Country != null){
				//	return !Country.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CountryCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Country));
            }
        }
	
       
		//private Module.BusinessObjects.Space _space;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Địa bàn")]
        [ToolTip("Địa bàn")]
		//[Index(4)]		
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
	
       
		//private decimal? _capital;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Vốn")]
        [ToolTip("Vốn")]
		//[Index(5)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Capital
        { 
		    get => GetPropertyValue<decimal?>("Capital");                         
			set => SetPropertyValue<decimal?>("Capital", value); 
			
        }
		//Tooltip for Object
		public object CapitalToolTipControllerText(View view)
        {
        //    if (Capital != null) 
		//			return Capital;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultCapital(View view = null)
        { 
			return Capital;
        }
		//Set Default Value
		public void SetDefaultCapital(View view = null)
        {
            //if (Capital is null){
            //    var result = GetDefaultCapital(view);
            //    if (result != null && result != Capital){
			//          Capital = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CapitalIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCapital();
				//if (result != null && Capital != null){
				//	return !Capital.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Currency _currency;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tiền tệ")]
        [ToolTip("Tiền tệ")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(CurrencyCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Currency Currency
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Currency>("Currency");                         
			set => SetPropertyValue<Module.BusinessObjects.Currency>("Currency", value); 
			
        }
		//Tooltip for Object
		public object CurrencyToolTipControllerText(View view)
        {
        //    if (Currency != null) 
		//			return Currency;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Currency GetDefaultCurrency(View view = null)
        { 
			return Currency;
        }
		//Set Default Value
		public void SetDefaultCurrency(View view = null)
        {
            //if (Currency is null){
            //    var result = GetDefaultCurrency(view);
            //    if (result != null && result != Currency){
			//          Currency = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CurrencyIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCurrency();
				//if (result != null && Currency != null){
				//	return !Currency.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator CurrencyCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Currency));
            }
        }
	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Biểu tượng")]
        [ToolTip("Biểu tượng")]
		//[Index(7)]		
		[Appearance("Biểu tượngBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Image
        { 
		    get => GetPropertyValue<byte[]>("Image");                         
			set => SetPropertyValue<byte[]>("Image", value); 
			
        }
		//Tooltip for Object
		public object ImageToolTipControllerText(View view)
        {
        //    if (Image != null) 
		//			return Image;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultImage(View view = null)
        { 
			return Image;
        }
		//Set Default Value
		public void SetDefaultImage(View view = null)
        {
            //if (Image is null){
            //    var result = GetDefaultImage(view);
            //    if (result != null && result != Image){
			//          Image = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ImageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImage();
				//if (result != null && Image != null){
				//	return !Image.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _link;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Trang chủ")]
        [ToolTip("Trang chủ")]
		//[Index(8)]		

 		[Size(250)]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string Link
        { 
		    get => GetPropertyValue<string>("Link");                         
			set => SetPropertyValue<string>("Link", value); 
			
        }
		//Tooltip for Object
		public object LinkToolTipControllerText(View view)
        {
        //    if (Link != null) 
		//			return Link;
            return null;
        }
		//Get Default Value
        public string GetDefaultLink(View view = null)
        { 
			return Link;
        }
		//Set Default Value
		public void SetDefaultLink(View view = null)
        {
            //if (Link is null){
            //    var result = GetDefaultLink(view);
            //    if (result != null && result != Link){
			//          Link = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LinkIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLink();
				//if (result != null && Link != null){
				//	return !Link.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _leader;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lãnh đạo")]
        [ToolTip("Lãnh đạo")]
		//[Index(9)]		

 		[Size(250)]
		public string Leader
        { 
		    get => GetPropertyValue<string>("Leader");                         
			set => SetPropertyValue<string>("Leader", value); 
			
        }
		//Tooltip for Object
		public object LeaderToolTipControllerText(View view)
        {
        //    if (Leader != null) 
		//			return Leader;
            return null;
        }
		//Get Default Value
        public string GetDefaultLeader(View view = null)
        { 
			return Leader;
        }
		//Set Default Value
		public void SetDefaultLeader(View view = null)
        {
            //if (Leader is null){
            //    var result = GetDefaultLeader(view);
            //    if (result != null && result != Leader){
			//          Leader = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LeaderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLeader();
				//if (result != null && Leader != null){
				//	return !Leader.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private byte[] _leaderimage;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Chân dung")]
        [ToolTip("Chân dung")]
		//[Index(10)]		
		[Appearance("Chân dungBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] LeaderImage
        { 
		    get => GetPropertyValue<byte[]>("LeaderImage");                         
			set => SetPropertyValue<byte[]>("LeaderImage", value); 
			
        }
		//Tooltip for Object
		public object LeaderImageToolTipControllerText(View view)
        {
        //    if (LeaderImage != null) 
		//			return LeaderImage;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultLeaderImage(View view = null)
        { 
			return LeaderImage;
        }
		//Set Default Value
		public void SetDefaultLeaderImage(View view = null)
        {
            //if (LeaderImage is null){
            //    var result = GetDefaultLeaderImage(view);
            //    if (result != null && result != LeaderImage){
			//          LeaderImage = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool LeaderImageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultLeaderImage();
				//if (result != null && LeaderImage != null){
				//	return !LeaderImage.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private StockExchange _stockexchange;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Niêm yết")]
        [ToolTip("Niêm yết")]
		//[Index(11)]		
		public StockExchange StockExchange
        { 
		    get => GetPropertyValue<StockExchange>("StockExchange");                         
			set => SetPropertyValue<StockExchange>("StockExchange", value); 
			
        }
		//Tooltip for Object
		public object StockExchangeToolTipControllerText(View view)
        {
        //    if (StockExchange != null) 
		//			return StockExchange;
            return null;
        }
		//Get Default Value
        public StockExchange GetDefaultStockExchange(View view = null)
        { 
			return StockExchange;
        }
		//Set Default Value
		public void SetDefaultStockExchange(View view = null)
        {
            //if (StockExchange is null){
            //    var result = GetDefaultStockExchange(view);
            //    if (result != null && result != StockExchange){
			//          StockExchange = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool StockExchangeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultStockExchange();
				//if (result != null && StockExchange != null){
				//	return !StockExchange.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(12)]		
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
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cổ đông")]
		//[Index(13)]
		[DevExpress.Xpo.Association("Company-ShareHolderList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ShareHolder> ShareHolderList
        {      
		    get => GetCollection<Module.BusinessObjects.ShareHolder>("ShareHolderList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
		//[Index(14)]
		[DevExpress.Xpo.Association("Company-BookMarkList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BookMark> BookMarkList
        {      
		    get => GetCollection<Module.BusinessObjects.BookMark>("BookMarkList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lĩnh vực")]
		//[Index(15)]
		[DataSourceCriteria("Not CompanyList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("CompanyList-DomainList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Domain> DomainList
        {      
		    get => GetCollection<Module.BusinessObjects.Domain>("DomainList"); 
			
        }
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(16)]		
		public bool InActive
        { 
		    get => GetPropertyValue<bool>("InActive");                         
			set => SetPropertyValue<bool>("InActive", value); 
			
        }
		//Tooltip for Object
		public object InActiveToolTipControllerText(View view)
        {
        //    if (InActive != null) 
		//			return InActive;
            return null;
        }
		//Get Default Value
        public bool GetDefaultInActive(View view = null)
        { 
			return InActive;
        }
		//Set Default Value
		public void SetDefaultInActive(View view = null)
        {
            //if (InActive is null){
            //    var result = GetDefaultInActive(view);
            //    if (result != null && result != InActive){
			//          InActive = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool InActiveIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultInActive();
				//if (result != null && InActive != null){
				//	return !InActive.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(17)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
	    [ModelDefault("AllowEdit", "False")]
		public DateTime? Update
        { 
		    get => GetPropertyValue<DateTime?>("Update");                         
			set => SetPropertyValue<DateTime?>("Update", value); 
			
        }
		//Tooltip for Object
		public object UpdateToolTipControllerText(View view)
        {
        //    if (Update != null) 
		//			return Update;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool UpdateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpdate();
				//if (result != null && Update != null){
				//	return !Update.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Member _updater;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Người cập nhật")]
        [ToolTip("Người cập nhật")]
		//[Index(18)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpdaterCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [ModelDefault("AllowEdit", "False")]
		public Module.BusinessObjects.Member Updater
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Updater");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Updater", value); 
			
        }
		//Tooltip for Object
		public object UpdaterToolTipControllerText(View view)
        {
        //    if (Updater != null) 
		//			return Updater;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool UpdaterIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpdater();
				//if (result != null && Updater != null){
				//	return !Updater.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpdaterCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Updater));
            }
        }
	
       
		//private decimal? _quantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cổ phần")]
        [ToolTip("Cổ phần")]
		//[Index(19)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
		public decimal? Quantity
        { 
		    get => GetPropertyValue<decimal?>("Quantity");                         
			set => SetPropertyValue<decimal?>("Quantity", value); 
			
        }
		//Tooltip for Object
		public object QuantityToolTipControllerText(View view)
        {
        //    if (Quantity != null) 
		//			return Quantity;
            return null;
        }
		//Get Default Value
        public decimal? GetDefaultQuantity(View view = null)
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

	
       
		//private Module.BusinessObjects.Folder _folder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thư mục")]
        [ToolTip("Thư mục")]
		//[Index(20)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(FolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("Folder-CompanyList")]
	 
		public Module.BusinessObjects.Folder Folder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Folder>("Folder");                         
			set => SetPropertyValue<Module.BusinessObjects.Folder>("Folder", value); 
			
        }
		//Tooltip for Object
		public object FolderToolTipControllerText(View view)
        {
        //    if (Folder != null) 
		//			return Folder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Folder GetDefaultFolder(View view = null)
        { 
			return Folder;
        }
		//Set Default Value
		public void SetDefaultFolder(View view = null)
        {
            //if (Folder is null){
            //    var result = GetDefaultFolder(view);
            //    if (result != null && result != Folder){
			//          Folder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFolder();
				//if (result != null && Folder != null){
				//	return !Folder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator FolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Folder));
            }
        }
	
       
        private bool _display;
        [Browsable(false)]
        [NonPersistent]
        [ImmediatePostData]
        public bool Display
        {
            get { return _display; }
            set { SetPropertyValue("Display", ref _display, value); }
        }
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 2895ImportCode
            base.AfterConstruction();
SetDefaultMember();
            #endregion 2895ImportCode
            Display = true;
 
        //SetDefaultCode(View view = null);
        //SetDefaultIntroduction(View view = null);
        //SetDefaultCompanyType(View view = null);
        //SetDefaultCountry(View view = null);
        //SetDefaultSpace(View view = null);
        //SetDefaultCapital(View view = null);
        //SetDefaultCurrency(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultLink(View view = null);
        //SetDefaultLeader(View view = null);
        //SetDefaultLeaderImage(View view = null);
        //SetDefaultStockExchange(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultInActive(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultFolder(View view = null);
			
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
            #region 2890ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 2890ImportCode
//            Update = (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
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
			//	SetDefaultShareHolderList();
			//	SetDefaultBookMarkList();
			//	SetDefaultDomainList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 2891ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 2891            Oid: e50c84e6-ac00-4c7c-9c62-fcd1c8c8a1ca
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 2891ImportCode
#region 2896ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 2896            Oid: 46d31a01-6cdd-4ea1-87e8-3bae32a6a07c
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2896ImportCode
#region 2893ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 2893            Oid: e0b2a6cd-5042-4ee5-b8cf-feecb33cd3f7
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 2893ImportCode
#region 2889ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 2889            Oid: 6ac434b5-94b1-4d9f-9368-f5e488dd2097
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 2889ImportCode
#region 2892ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 2892            Oid: 1a83cc1a-db79-4f79-85dd-174f50a6fb50
            Updater = GetDefaultUpdater();
        }
#endregion 2892ImportCode
#region 2894ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 2894            Oid: 905d4481-0100-442e-ba28-b31277f4b26f
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 2894ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
