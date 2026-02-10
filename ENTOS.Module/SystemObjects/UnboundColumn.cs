using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.Security;
using DevExpress.Xpo;


namespace ENTOS.Module.SystemObjects
{
    [NavigationItem("Default")]
    [ModelDefault("Caption", "Cột động"), ImageName("InsertTableColumnsToTheRight2")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
   
    //[OptimisticLocking(false)]
    public partial class UnboundColumn : GlobalFunctionInListView, INoIndexColumn     //, HbBaseObject
    {

        public UnboundColumn(Session session)
            : base(session) {              
        }

		

        //private string _condition;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Biểu thức nguồn")]
        [Index(5)]
        [ToolTip("Lấy dữ liệu từ nguồn")]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        //[CriteriaOptions("ObjectType")]
        //[ModelDefault("PropertyEditorType", "ExtendedPopupExpressionPropertyEditor")]
        [DevExpress.ExpressApp.Core.ElementTypeProperty("ObjectType")]
        //[DataSourceCriteriaProperty("ObjectType")]
        [ModelDefault("RowCount", "1")]
        [Required]
        public string SourceCondition
        {            
            get => GetPropertyValue<string>("SourceCondition");
            set => SetPropertyValue<string>("SourceCondition", value);
        }

        //private string _name;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Tên cột"), ToolTip("Tên cột")]
        //[Index(1)]		
        [Size(150)]
        public string Name
        {
            get => GetPropertyValue<string>("Name");
            set => SetPropertyValue<string>("Name", value);

        }

        //private string _name;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
        [VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Kiểu"), ToolTip("Kiểu")]
        //[Index(1)]		
        [Size(150)]
        public DevExpress.Data.UnboundColumnType ColumnType
        {
            get => GetPropertyValue<DevExpress.Data.UnboundColumnType>("ColumnType");
            set => SetPropertyValue<DevExpress.Data.UnboundColumnType>("ColumnType", value);

        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
        SetDefaultObjectType();

        //SetDefaultUser();
			//Condition = Tools.GetValue(Session, Tools.GetModuleName(GetType()), "Condition");
        }
        
		 
    }

   
}