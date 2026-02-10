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
    [ModelDefault("Caption", "Sửa thuật vị"), ImageName("TermLocationCorrection")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
 
 
    [ShowToolTipAttribute(TargetItems = nameof(Caption)+ "," + nameof(TermCorrection))]
 
 
	[MobileColumnAttribute(Context = "TermLocationCorrection_LookupListView", TargetItems = nameof(Caption))]
	[MobileColumnAttribute(Context = "TermCorrection_TermLocationCorrectionList_ListView", TargetItems = nameof(Caption))]
	[MobileColumnAttribute(Context = "TermLocationCorrection_ListView", TargetItems = nameof(Caption))]
	[DefaultProperty("Caption")]
 
	[NonPersistent()]
[OptimisticLocking(true)]
    public partial class TermLocationCorrection:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted      //, HbBaseObject
    {
        public TermLocationCorrection(Session session)
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
               

		//private string _caption;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(0)]		

 		[Size(200)]
		public string Caption
        { 
		    #region 1449ImportCode 
            get
            {
                if(TermLocation != null)
                {
                    if(TermLocation.Term != null)
                        return TermLocation.Term.Name;
                    return TermLocation.MachineTranslate;
                }
                return null;
            }
#endregion 1449ImportCode
			
        }
		//Tooltip for Object
		public object CaptionToolTipControllerText(View view)
        {
            #region 1450ImportCode 
            if (TermLocation != null && TermLocation.Audio != null && TermLocation.Audio.Video != null)
            {
                string hoverText = "";
                //bool isSubtitle = TermLocation.Term != null;
                var audioList = TermLocation.Audio.Video.GetAudioListWithSort();
                var beforeAudio = audioList.LastOrDefault(x => x.Start < TermLocation.Audio.Start);
                if (beforeAudio != null)
                    hoverText += "<color=gray>" + beforeAudio.Content + "</color>";
                if (!string.IsNullOrEmpty(hoverText))
                    hoverText += "\r\n";
                string termName = TermLocation.Term != null ? TermLocation.Term.Name : TermLocation.MachineTranslate;
                var index = Services.TermLocationService.GetIndexContent(TermLocation, TermLocation.Audio.Content, termName);
                if (index >= 0)
                {
                    hoverText += TermLocation.Audio.Content.Substring(0, index);
                    hoverText += "<size=18><b>" + TermLocation.Audio.Content.Substring(index, termName.Length) + "</b></size>";
                    hoverText += TermLocation.Audio.Content.Substring(index + termName.Length);
                }
                else
                {
                    hoverText += TermLocation.Audio.Content;
                }
                var afterAudio = audioList.FirstOrDefault(x => x.Start > TermLocation.Audio.Start);
                if (afterAudio != null)
                {
                    if (!string.IsNullOrEmpty(hoverText))
                        hoverText += "\r\n";
                    hoverText += "<color=gray>" + afterAudio.Content + "</color>";
                }
                return hoverText;
                //if (TermLocation.Term != null)
                //    return TermLocation.Audio.SubtitleToolTipControllerText(view);
                //else 
                //    return TermLocation.Audio.ContentToolTipControllerText(view);
            }
#endregion 1450ImportCode
            return null;
        }
		//Get Default Value
        public string GetDefaultCaption(View view = null)
        { 
			return Caption;
        }
		//Set Default Value
		public void SetDefaultCaption(View view = null)
        {
            //if (Caption is null){
            //    var result = GetDefaultCaption(view);
            //    if (result != null && result != Caption){
			//          Caption = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CaptionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCaption();
				//if (result != null && Caption != null){
				//	return !Caption.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tùy chọn")]
		//[Index(1)]
		//[DevExpress.Xpo.Association]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.CorrectionOption> CorrectionOptionList
        {      

                #region 1054ImportCode 
get;set;
#endregion 1054ImportCode
			
        }
       
		//private Module.BusinessObjects.TermCorrection _termcorrection;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sửa thuật ngữ")]
        [ToolTip("Sửa thuật ngữ")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TermCorrectionCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.TermCorrection TermCorrection
        { 
		    #region 1053ImportCode 
get;set;
#endregion 1053ImportCode
			
        }
		//Tooltip for Object
		public object TermCorrectionToolTipControllerText(View view)
        {
            #region 1062ImportCode 
if (TermCorrection != null && TermCorrection != null && !string.IsNullOrEmpty(TermCorrection.Term.Name))
{
    return "<size=30>" + TermCorrection.Term.Name + "</size>";
}
#endregion 1062ImportCode
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.TermCorrection GetDefaultTermCorrection(View view = null)
        { 
			return TermCorrection;
        }
		//Set Default Value
		public void SetDefaultTermCorrection(View view = null)
        {
            //if (TermCorrection is null){
            //    var result = GetDefaultTermCorrection(view);
            //    if (result != null && result != TermCorrection){
			//          TermCorrection = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TermCorrectionIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTermCorrection();
				//if (result != null && TermCorrection != null){
				//	return !TermCorrection.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TermCorrectionCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(TermCorrection));
            }
        }
	
       
		//private int? _optionnumber;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Số tùy chọn")]
        [ToolTip("Số tùy chọn")]
		//[Index(3)]		
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

	
       
		//private Module.BusinessObjects.TermLocation _termlocation;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Thuật vị")]
        [ToolTip("Thuật vị")]
		//[Index(4)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(TermLocationCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.TermLocation TermLocation
        { 
		    #region 1429ImportCode 
get;set;
#endregion 1429ImportCode
			
        }
		//Tooltip for Object
		public object TermLocationToolTipControllerText(View view)
        {
        //    if (TermLocation != null) 
		//			return TermLocation;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.TermLocation GetDefaultTermLocation(View view = null)
        { 
			return TermLocation;
        }
		//Set Default Value
		public void SetDefaultTermLocation(View view = null)
        {
            //if (TermLocation is null){
            //    var result = GetDefaultTermLocation(view);
            //    if (result != null && result != TermLocation){
			//          TermLocation = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool TermLocationIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultTermLocation();
				//if (result != null && TermLocation != null){
				//	return !TermLocation.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator TermLocationCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(TermLocation));
            }
        }
	
       
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            base.AfterConstruction();
 
        //SetDefaultCaption(View view = null);
        //SetDefaultTermCorrection(View view = null);
        //SetDefaultOptionNumber(View view = null);
        //SetDefaultTermLocation(View view = null);
			
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
			//	SetDefaultCorrectionOptionList();
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
#region TermLocationCorrectionImportCode
public void AddTermLocation(Module.BusinessObjects.TermLocation termLocation, System.Collections.Generic.List<string> words)
{
    if (termLocation != null)
    {
        if (TermLocation is null)
            TermLocation = termLocation;
        var correctionOptions = new System.Collections.Generic.List<CorrectionOption>();
        foreach (var word in words)
        {
            var correctionOption = new CorrectionOption(Session);
            correctionOption.Name = word;
            correctionOption.TermLocationCorrection = this;
            correctionOptions.Add(correctionOption);

        }
        CorrectionOptionList = new DevExpress.Xpo.XPCollection<CorrectionOption>(Session, correctionOptions);
    }
}	
#endregion TermLocationCorrectionImportCode
		 		 
    }
}
