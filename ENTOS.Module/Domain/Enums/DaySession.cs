using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum DaySession
    {
					[XafDisplayName(" ")]
        None,
					[XafDisplayName("Sáng")]
        Morning,
					[XafDisplayName("Chiều")]
        Afternoon,
					[XafDisplayName("Tối")]
        Evening,
	    }

}