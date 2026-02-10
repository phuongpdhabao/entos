using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum SubscriptionCycle
    {
					[XafDisplayName("Tháng")]
        Month,
					[XafDisplayName("Năm")]
        Year,
					[XafDisplayName("Vĩnh viễn")]
        Perpetual,
	    }

}