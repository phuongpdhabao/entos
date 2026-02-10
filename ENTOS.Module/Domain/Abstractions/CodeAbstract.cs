using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Domain.Abstractions
{
    [NonPersistent]

    public abstract class CodeAbstract : OidAbstract
    {
        public CodeAbstract(Session session) : base(session) { }


        public override void AfterConstruction()
        {

            base.AfterConstruction();
            SetDefaultCode();
        }
        //private string _code;
        [DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]

        [VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        [DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
        //[Index(0)]		
        [Size(20)]
        [RuleUniqueValue]
        [RuleRequiredField]
        [ModelDefault("AllowEdit", "False")]
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

        public void SetDefaultCode(View view = null)
        {
            //Code: 0619            Oid: 0480f368-4c74-4d88-a2c8-110057b1af31
            if (string.IsNullOrEmpty(Code)) Code = GetDefaultCode();

        }

        public virtual string GetDefaultCode(View view = null)
        {
            var keyCodeObject =
                    Module.Helpers.ParameterHelper.GetSettingParameter(Session, "CodeObject");
            //Kích thước mặc định là 3 số
            int size = 5;
            return Tools.GetCode(this.GetType(), this.Session, this.Oid, keyCodeObject != null ? keyCodeObject.Value : "", size,
                " ");
            return null;
        }

    }
}
