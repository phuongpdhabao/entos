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
    [ModelDefault("Caption", "Sửa thuật ngữ"), ImageName("TermCorrection")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
 
 
 
 
	[MobileColumnAttribute(Context = "TermCorrection_LookupListView", TargetItems = nameof(Term))]
	[MobileColumnAttribute(Context = "TermCorrection_ListView", TargetItems = nameof(Term))]
	[DefaultProperty("Term")]
 
	[NonPersistent()]
[OptimisticLocking(true)]
    public partial class TermCorrection:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public TermCorrection(Session session)
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
               

		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sửa thuật vị")]
		//[Index(0)]
		//[DevExpress.Xpo.Association]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.TermLocationCorrection> TermLocationCorrectionList
        {      

                #region 1447ImportCode 
get;set;
#endregion 1447ImportCode
			
        }
       
		//private Module.BusinessObjects.Term _term;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thuật ngữ")]
        [ToolTip("Thuật ngữ")]
		//[Index(1)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TermCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Term Term
        { 
		    #region 1446ImportCode 
get;set;
#endregion 1446ImportCode
			
        }
		//Tooltip for Object
		public object TermToolTipControllerText(View view)
        {
        //    if (Term != null) 
		//			return Term;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Term GetDefaultTerm(View view = null)
        { 
			return Term;
        }
		//Set Default Value
		public void SetDefaultTerm(View view = null)
        {
            //if (Term is null){
            //    var result = GetDefaultTerm(view);
            //    if (result != null && result != Term){
			//          Term = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TermIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTerm();
				//if (result != null && Term != null){
				//	return !Term.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TermCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Term));
            }
        }
	
       
		//private int? _optionnumber;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số tùy chọn")]
        [ToolTip("Số tùy chọn")]
		//[Index(2)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? OptionNumber
        { 
		    get => GetPropertyValue<int?>("OptionNumber");                         
			set => SetPropertyValue<int?>("OptionNumber", value); 
			
        }
		//Tooltip for Object
		public object OptionNumberToolTipControllerText(View view)
        {
        //    if (OptionNumber != null) 
		//			return OptionNumber;
            return null;
        }
		//Get Default Value
        public int? GetDefaultOptionNumber(View view = null)
        { 
			return OptionNumber;
        }
		//Set Default Value
		public void SetDefaultOptionNumber(View view = null)
        {
            //if (OptionNumber is null){
            //    var result = GetDefaultOptionNumber(view);
            //    if (result != null && result != OptionNumber){
			//          OptionNumber = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OptionNumberIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOptionNumber();
				//if (result != null && OptionNumber != null){
				//	return !OptionNumber.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultTerm(View view = null);
        //SetDefaultOptionNumber(View view = null);
			
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
			//	SetDefaultTermLocationCorrectionList();
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
#region TermCorrectionImportCode
        public void AddTerm(Module.BusinessObjects.Term term, System.Collections.Generic.List<string> words)
        {
            if (term != null)
            {
                if (Term is null)
                    Term = term;
                var termLocationCorrections = new System.Collections.Generic.List<TermLocationCorrection>();
                foreach (var termLocation in Term.TermLocationList)
                {
                    var termLocationCorrection = new TermLocationCorrection(Session);
                    termLocationCorrection.AddTermLocation(termLocation, words);
                    termLocationCorrection.TermCorrection = this;
                    termLocationCorrection.OptionNumber = words.Count;
                    termLocationCorrections.Add(termLocationCorrection);
                }
                OptionNumber = words.Count;
                TermLocationCorrectionList = new DevExpress.Xpo.XPCollection<TermLocationCorrection>(Session, termLocationCorrections);
            }
        }
#endregion TermCorrectionImportCode
		 		 
    }
}
