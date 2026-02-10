using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace ENTOS.Domain.Abstractions
{
    [NonPersistent]

    public abstract class OidAbstract : XPLiteObject
    {
        public OidAbstract(Session session) : base(session) { }

        [Persistent("Oid")]
        [Key(true)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        private Guid oid = Guid.Empty;



        [PersistentAlias("oid")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public Guid Oid => oid;



        public override void AfterConstruction()
        {
            base.AfterConstruction();
            oid = XpoDefault.NewGuid();
        }
    }
}
