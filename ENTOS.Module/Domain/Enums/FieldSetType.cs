using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum FieldSetType
    {
					[XafDisplayName("List")]
        ListView,
					[XafDisplayName("Lookup")]
        LookupListView,
					[XafDisplayName("Nested 1-n")]
        NestedListView1N,
					[XafDisplayName("Nested n-n")]
        NestedListViewNN,
					[XafDisplayName("Custom")]
        CustomListView,
	    }

}