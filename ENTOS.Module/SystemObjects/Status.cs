﻿using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace ENTOS.Module.SystemObjects
{
    [NavigationItem("Default")]
    [ModelDefault("Caption", "Trạng thái")]
    [ImageName("Action_StateMachine")]
    [ModelDefault("DefaultLookupEditorMode", "AllItems")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [DefaultProperty("Name")]   
    public class Status : XPLiteObject
    {
        public Status(Session session)
            : base(session)
        {
            
        }

        [Key(AutoGenerate = true)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Guid Oid { get; set; }

        private string _name;
        [DetailViewLayout(LayoutColumnPosition.Left, " ", 0)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [ModelDefault("Caption", "Tên")]
        [ToolTip("Tên")]
        [Size(200)]    
        public string Name
        {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        private string _code;
        [DetailViewLayout(LayoutColumnPosition.Left, " ", 0)]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(true)]
        [ModelDefault("Caption", "Mã")]
        [ToolTip("Mã")]
        [Size(200)]        
        [RuleUniqueValue("UniqueStatusCode", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public string Code
        {
            get { return _code; }
            set { SetPropertyValue("Code", ref _code, value); }
        }

        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 0)]
        [VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Màu"), ToolTip("Màu")]
        //[Index(2)]	
        [DevExpress.Xpo.Persistent]
        [ValueConverter(typeof(DevExpress.ExpressApp.StateMachine.Xpo.NullableColorConverter))]
        public Color? Color
        {
            get => GetPropertyValue<Color?>("Color");
            set => SetPropertyValue<Color?>("Color", value);

        }

        //private string _icon;
        //[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 0)]
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[ModelDefault("Caption", "Biểu tượng")]
        //[Index(3)]
        //[ToolTip("Biểu tượng")]
        //[Size(100)]
        //[ModelDefault("Width", "100")]
        ////[RuleRequiredField("EmptySofStatusIcon", DefaultContexts.Save)]
        ////[RuleUniqueValue("UniqueSofStatusIcon", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        //public string Icon
        //{ 
        //    get { return _icon; }
        //    set { SetPropertyValue("Icon", ref _icon, value); }
        //}

		//private int? _order;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 0)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự"),ToolTip("Thứ tự")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Order
        { 
		    get => GetPropertyValue<int?>("Order");                         
			set => SetPropertyValue<int?>("Order", value); 
			
        }
		//Tooltip for Object
		public string OrderToolTipControllerText(View view)
        {
        //    if (Order != null) 
		//			return Order;
            return null;
        }
		//Get Default Value
        public int? GetDefaultOrder()
        { 
			return Order;
        }
		//Set Default Value
		public void SetDefaultOrder()
        {
            //if (Order is null){
            //    var result = GetDefaultOrder();
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
        #region Check Valid Value

        [Browsable(false)]
        public bool NameIsValided
        {
            get { return true; }
        }

        #endregion

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            //Condition = Tools.GetValue(Session, Tools.GetModuleName(GetType()), "Condition");
        }


        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsSaving)
                if (propertyName == "propertyName")
                {
                }
        }

        #region Set Default Value

        public void SetDefaultName()
        {
        }

        #endregion
    }
}