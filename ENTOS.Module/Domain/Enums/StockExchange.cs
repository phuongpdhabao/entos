using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum StockExchange
    {
					[XafDisplayName(" ")]
        NONE,
					[XafDisplayName("HOSE")]
        HOSE,
					[XafDisplayName("HNX")]
        HNX,
					[XafDisplayName("OTC")]
        OTC,
					[XafDisplayName("NYSE")]
        NYSE,
	    }

}