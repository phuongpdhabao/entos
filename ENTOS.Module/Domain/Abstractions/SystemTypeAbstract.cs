using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using ENTOS.Module.BusinessObjects;
using System.ComponentModel;

namespace ENTOS.Domain.Abstractions
{
    [NonPersistent]

    public abstract class SystemTypeAbstract : UpdateAbstract, IUpperObject
    {
        public SystemTypeAbstract(Session session) : base(session) { }


        public override void AfterConstruction()
        {

            base.AfterConstruction();
        }
        //private System.Type _systemtype;
        //[Browsable(false)] 

        [VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Kiểu hệ thống")]
        [ToolTip("Kiểu hệ thống")]
        //[Index(13)]		
        [LookupEditorMode(LookupEditorMode.Auto)]
        //[ModelDefault("LookupProperty", "")]
        [DataSourceCriteriaProperty(nameof(SystemTypeCriteria))]
        //[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
        //[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
        //[DevExpress.Xpo.Association]
        //[NoForeignKey]

        [TypeConverter(typeof(DevExpress.Persistent.Base.Security.SecurityTargetTypeConverter))]
        [ValueConverter(typeof(DevExpress.ExpressApp.Utils.TypeToStringConverter))]
        public virtual System.Type SystemType
        {
            get => GetPropertyValue<System.Type>("SystemType");
            set => SetPropertyValue<System.Type>("SystemType", value);

        }
        ////Tooltip for Object
        //public object SystemTypeToolTipControllerText(View view)
        //{
        //    //    if (SystemType != null) 
        //    //			return SystemType;
        //    return null;
        //}
        ////Get Default Value
        //public System.Type GetDefaultSystemType(View view = null)
        //{
        //    return SystemType;
        //}
        ////Set Default Value
        //public void SetDefaultSystemType(View view = null)
        //{
        //    //if (SystemType is null){
        //    //    var result = GetDefaultSystemType(view);
        //    //    if (result != null && result != SystemType){
        //    //          SystemType = result;
        //    //	  }
        //    //}
        //}

        ////Check Not Validate
        //protected bool SystemTypeIsNotValidate
        //{
        //    get
        //    {

        //        //var result = GetDefaultSystemType();
        //        //if (result != null && SystemType != null){
        //        //	return !SystemType.Equals(result);
        //        //} 

        //        return false;
        //    }
        //}

        private CriteriaOperator SystemTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SystemType));
            }
        }

        //private System.Guid _objectid;
        //[Browsable(false)] 
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [DevExpress.Xpo.DisplayName("Mã đối tượng")]
        [ToolTip("Mã đối tượng")]
        //[Index(15)]		
        [ModelDefault("AllowEdit", "False")]
        public virtual System.Guid? ObjectID
        {
            get => GetPropertyValue<System.Guid?>("ObjectID");
            set => SetPropertyValue<System.Guid?>("ObjectID", value);

        }
        ////Tooltip for Object
        //public object ObjectIDToolTipControllerText(View view)
        //{
        //    //    if (ObjectID != null) 
        //    //			return ObjectID;
        //    return null;
        //}
        ////Get Default Value
        //public System.Guid GetDefaultObjectID(View view = null)
        //{
        //    return ObjectID;
        //}
        ////Set Default Value
        //public void SetDefaultObjectID(View view = null)
        //{
        //    //if (ObjectID is null){
        //    //    var result = GetDefaultObjectID(view);
        //    //    if (result != null && result != ObjectID){
        //    //          ObjectID = result;
        //    //	  }
        //    //}
        //}

        ////Check Not Validate
        //protected bool ObjectIDIsNotValidate
        //{
        //    get
        //    {

        //        //var result = GetDefaultObjectID();
        //        //if (result != null && ObjectID != null){
        //        //	return !ObjectID.Equals(result);
        //        //} 

        //        return false;
        //    }
        //}

    }
}
