using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Domain.Abstractions
{
    [NonPersistent]
    [ShowToolTip(TargetItems = nameof(Update))]
    public abstract class UpdateAbstract : OidAbstract
    {
        public UpdateAbstract(Session session) : base(session) { }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [ModelDefault("Caption", "Người cập nhật")]
        [ToolTip("Người sửa")]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        [ModelDefault("DefaultListViewNewItemRowPosition", "None")]
        public virtual Module.BusinessObjects.Member Updater
        {
            get => GetPropertyValue<Module.BusinessObjects.Member>(nameof(Updater));
            set => SetPropertyValue(nameof(Updater), value);
        }

        private Module.BusinessObjects.Member _member;
        public virtual Module.BusinessObjects.Member GetDefaultModifiedBy()
        {
            if (_member is null)
                _member = Module.BusinessObjects.Member.GetCurrentMember(Session); //Cache User để không phải gọi lại database
            return _member;
        }

        public virtual void SetDefaultModifiedBy()
        {
            Updater = GetDefaultModifiedBy();
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [ModelDefault("Caption", "Cập nhật")]
        [ModelDefault("DisplayFormat", "d/M/yyyy")]
        [ModelDefault("EditMask", "d/M/yyyy")]
        [ModelDefault("AllowEdit", "False")]
        public DateTime? Update
        {
            get => GetPropertyValue<DateTime?>(nameof(Update));
            set => SetPropertyValue<DateTime?>(nameof(Update), value);
        }

        public virtual string ModifiedOnToolTipControllerText(View view)
        {
            if (Update != null)
                return Update.Value.ToString("HH:mm");
            return null;
        }

        public virtual DateTime GetDefaultModifiedOn()
        {
            return Module.Helpers.XafXpoHelper.GetDateTimeNowFromServer(Session);
        }

        public virtual void SetDefaultModifiedOn()
        {
            Update = GetDefaultModifiedOn();
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!(base.Session is NestedUnitOfWork))
            {
                SetDefaultModifiedBy();
                SetDefaultModifiedOn();
            }
        }

    }
}
