using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum DataLayout
    {
					[XafDisplayName("Header")]
        Header,
					[XafDisplayName("Table")]
        Table,
					[XafDisplayName("Table2")]
        Table2,
					[XafDisplayName("Footer")]
        Footer,
	    }

}