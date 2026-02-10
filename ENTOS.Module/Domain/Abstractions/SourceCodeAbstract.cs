using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using ENTOS.Module.BusinessObjects;

namespace ENTOS.Domain.Abstractions
{
    [NonPersistent]

    public abstract class SourceCodeAbstract : UpdateAbstract
    {
        public SourceCodeAbstract(Session session) : base(session) { }


        public override void AfterConstruction()
        {

            base.AfterConstruction();
            SetDefaultSourceCode();
        }

        //private Module.BusinessObjects.SourceCode _sourcecode;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]

        [VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Mã nguồn")]
        [ToolTip("Mã nguồn")]
        //[Index(3)]		
        [LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        [DataSourceCriteriaProperty(nameof(SourceCodeCriteria))]
        //[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        //[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
        //[DevExpress.Xpo.Association]
        //[NoForeignKey]

        [ModelDefault("AllowEdit", "False")]
        [NonCloneable()]
        public Module.BusinessObjects.SourceCode SourceCode
        {
            get => GetPropertyValue<Module.BusinessObjects.SourceCode>("SourceCode");
            set => SetPropertyValue<Module.BusinessObjects.SourceCode>("SourceCode", value);

        }
        //Tooltip for Object
        public object SourceCodeToolTipControllerText(View view)
        {
            //    if (SourceCode != null) 
            //			return SourceCode;
            return null;
        }
        //Get Default Value
        public Module.BusinessObjects.SourceCode GetDefaultSourceCode(View view = null)
        {
            return SourceCode;
        }
        //Set Default Value

        //Check Not Validate
        protected bool SourceCodeIsNotValidate
        {
            get
            {

                //var result = GetDefaultSourceCode();
                //if (result != null && SourceCode != null){
                //	return !SourceCode.Equals(result);
                //} 

                return false;
            }
        }

        private CriteriaOperator SourceCodeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SourceCode));
            }
        }

        public void SetDefaultSourceCode(View view = null)
        {
            //Code: 3487            Oid: e4956cae-3916-4bf0-8ee2-c16ebba2367a
            if (SourceCode == null) SourceCode = new SourceCode(Session);
            SourceCode.SystemType = this.GetType();
            SourceCode.ObjectID = Oid;
        }


        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (SourceCode != null && !SourceCode.IsDeleted)
            {
                Session.Delete(SourceCode);
            }
        }

    }
}
