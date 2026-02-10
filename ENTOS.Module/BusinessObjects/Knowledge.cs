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
	[NavigationItem("TaskManagement")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Kiến thức"), ImageName("Knowledge")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("Knowledge Code None_None__" , TargetItems = "Code" , Criteria = "[MemberList][].Count() > 0",AppearanceItemType = "ViewItem", FontStyle = DevExpress.Drawing.DXFontStyle.Bold )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(Code)+ "," + nameof(Member)+ "," + nameof(PermissionPolicyRole)+ "," + nameof(MemberList), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
    [ShowToolTipAttribute(TargetItems = nameof(Code))]
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Code)+ "," + nameof(Member)+ "," + nameof(Update)+ "," + nameof(Updater))]
 
	[DefaultProperty("Name")]
 
[OptimisticLocking(true)]
    public partial class Knowledge:  DevExpress.Xpo.XPLiteObject  , INoIndexColumn, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public Knowledge(Session session)
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
				if (ArticleList.IsLoaded)
                {
                    if (ArticleList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ArticleList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ArticleList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Article>(CriteriaOperator.Parse("[Knowledge.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool articlelist = Session.Query<Module.BusinessObjects.Article>().Where(x => x.Knowledge.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ArticleList), articlelist);
                        if (articlelist)
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

 		[Size(250)]
		[RuleUniqueValue("UniqueKnowledgeName", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredKnowledgeName", DefaultContexts.Save)]
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

	
       
		//private string _code;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(1)]		

 		[Size(20)]
		[RuleUniqueValue("UniqueKnowledgeCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[RuleRequiredField("RequiredKnowledgeCode", DefaultContexts.Save)]
		public string Code
        { 
		    get => GetPropertyValue<string>("Code");                         
			set => SetPropertyValue<string>("Code", value); 
			
        }
		//Tooltip for Object
		public object CodeToolTipControllerText(View view)
        {
            #region 3278ImportCode 
    if (MemberList == null || MemberList.Count == 0)
        return "Không có thành viên nào.";

    var sb = new System.Text.StringBuilder();
    foreach (var member in MemberList) {
        sb.AppendLine($"{member.Name}");  // Giả sử Member có property Name
    }
    return sb.ToString();
#endregion 3278ImportCode
            return null;
        }
		//Get Default Value
		//Set Default Value

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

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(2)]		
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
	
       
		//private DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole _permissionpolicyrole;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhóm xem")]
        [ToolTip("Nhóm xem")]
		//[Index(3)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PermissionPolicyRoleCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole PermissionPolicyRole
        { 
		    get => GetPropertyValue<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole>("PermissionPolicyRole");                         
			set => SetPropertyValue<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole>("PermissionPolicyRole", value); 
			
        }
		//Tooltip for Object
		public object PermissionPolicyRoleToolTipControllerText(View view)
        {
        //    if (PermissionPolicyRole != null) 
		//			return PermissionPolicyRole;
            return null;
        }
		//Get Default Value
        public DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole GetDefaultPermissionPolicyRole(View view = null)
        { 
			return PermissionPolicyRole;
        }
		//Set Default Value
		public void SetDefaultPermissionPolicyRole(View view = null)
        {
            //if (PermissionPolicyRole is null){
            //    var result = GetDefaultPermissionPolicyRole(view);
            //    if (result != null && result != PermissionPolicyRole){
			//          PermissionPolicyRole = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PermissionPolicyRoleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPermissionPolicyRole();
				//if (result != null && PermissionPolicyRole != null){
				//	return !PermissionPolicyRole.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PermissionPolicyRoleCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PermissionPolicyRole));
            }
        }
	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đoạn viết")]
		//[Index(4)]
		[DevExpress.Xpo.Association("Knowledge-ArticleList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Article> ArticleList
        {      
		    get => GetCollection<Module.BusinessObjects.Article>("ArticleList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thành viên")]
		//[Index(5)]
		[DataSourceCriteria("Not KnowledgeList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("KnowledgeList-MemberList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Member> MemberList
        {      
		    get => GetCollection<Module.BusinessObjects.Member>("MemberList"); 
			
        }
       
		//private Module.BusinessObjects.Contact _contact;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên hệ")]
        [ToolTip("Liên hệ")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(ContactCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Contact Contact
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Contact>("Contact");                         
			set => SetPropertyValue<Module.BusinessObjects.Contact>("Contact", value); 
			
        }
		//Tooltip for Object
		public object ContactToolTipControllerText(View view)
        {
        //    if (Contact != null) 
		//			return Contact;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Contact GetDefaultContact(View view = null)
        { 
			return Contact;
        }
		//Set Default Value
		public void SetDefaultContact(View view = null)
        {
            //if (Contact is null){
            //    var result = GetDefaultContact(view);
            //    if (result != null && result != Contact){
			//          Contact = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ContactIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultContact();
				//if (result != null && Contact != null){
				//	return !Contact.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator ContactCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Contact));
            }
        }
	
       
		//private Module.BusinessObjects.GradeSubject _gradesubject;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Môn")]
        [ToolTip("Môn")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(GradeSubjectCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("GradeSubject-KnowledgeList")]
	 
		public Module.BusinessObjects.GradeSubject GradeSubject
        { 
		    get => GetPropertyValue<Module.BusinessObjects.GradeSubject>("GradeSubject");                         
			set => SetPropertyValue<Module.BusinessObjects.GradeSubject>("GradeSubject", value); 
			
        }
		//Tooltip for Object
		public object GradeSubjectToolTipControllerText(View view)
        {
        //    if (GradeSubject != null) 
		//			return GradeSubject;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.GradeSubject GetDefaultGradeSubject(View view = null)
        { 
			return GradeSubject;
        }
		//Set Default Value
		public void SetDefaultGradeSubject(View view = null)
        {
            //if (GradeSubject is null){
            //    var result = GetDefaultGradeSubject(view);
            //    if (result != null && result != GradeSubject){
			//          GradeSubject = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool GradeSubjectIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultGradeSubject();
				//if (result != null && GradeSubject != null){
				//	return !GradeSubject.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator GradeSubjectCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(GradeSubject));
            }
        }
	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(8)]		
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
		//[Index(9)]		
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
 
            #region 3248ImportCode
            base.AfterConstruction();
SetDefaultMember();
SetDefaultCode();
            #endregion 3248ImportCode
            Display = true;
 
        //SetDefaultName(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultPermissionPolicyRole(View view = null);
        //SetDefaultContact(View view = null);
        //SetDefaultGradeSubject(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultUpdater(View view = null);
			
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
            #region 3243ImportCode
            base.OnSaving();
SetDefaultUpdate();
SetDefaultUpdater();
            #endregion 3243ImportCode
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
			//	SetDefaultArticleList();
			//	SetDefaultMemberList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 3250ImportCode
		public string GetDefaultCode(View view = null)
        {
            //Code: 3250            Oid: 45f0d75b-eefa-4fe8-ac48-248137966794
            var keyCodeObject =
    Module.Helpers.ParameterHelper.GetSettingParameter(Session, "CodeObject");

//Trường hợp chỉ lấy mã trên đối tượng này
Type currentType = this.GetType();
//Trường hợp lấy mã từ đối tượng cha
//Type currentType = typeof(ObjectType);

//Kích thước mặc định là 3 số
int size = 3;
return Tools.GetCode(currentType , this.Session, this.Oid, keyCodeObject != null ? keyCodeObject.Value : "", size,
    " ");
return null;
        }
#endregion 3250ImportCode
#region 3244ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 3244            Oid: 02b994c6-3e72-45ce-8466-23fe41fc5afe
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 3244ImportCode
#region 3247ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 3247            Oid: 9d80e210-55b7-488b-ac97-32afeb7e9b2d
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 3247ImportCode
#region 3249ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 3249            Oid: 676269be-086f-4a6f-b11a-6d04ad8f85be
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3249ImportCode
#region 3242ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 3242            Oid: e3b02f20-a736-4088-bcfc-e5e2a7a7392e
            if (!IsDeleted)
Update = GetDefaultUpdate();
        }
#endregion 3242ImportCode
#region 3251ImportCode
		public void SetDefaultCode(View view = null)
        {
            //Code: 3251            Oid: d4746499-6f5f-4b14-9f94-71f32b5a426a
            if(String.IsNullOrEmpty(Code)) Code = GetDefaultCode();
        }
#endregion 3251ImportCode
#region 3245ImportCode
		public void SetDefaultUpdater(View view = null)
        {
            //Code: 3245            Oid: aa583ef8-bedf-49f5-923b-c4e72df3eb0d
            Updater = GetDefaultUpdater();
        }
#endregion 3245ImportCode
#region 3246ImportCode
		public Module.BusinessObjects.Member GetDefaultUpdater(View view = null)
        {
            //Code: 3246            Oid: eb4ad4c0-da64-4cba-8f66-497efd81ba3e
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 3246ImportCode
        #endregion
//Mã nguồn bổ sung
		 		 
    }
}
