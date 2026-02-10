using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum TransactionType
    {
					[XafDisplayName("Bán")]
        Sell,
					[XafDisplayName("Mua")]
        Buy,
	    }

}