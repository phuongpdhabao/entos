using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum Weekday
    {
					[XafDisplayName(" ")]
        None,
					[XafDisplayName("Thứ 2")]
        Mon,
					[XafDisplayName("Thứ 3")]
        Twe,
					[XafDisplayName("Thứ 4")]
        Wed,
					[XafDisplayName("Thứ 5")]
        Thu,
					[XafDisplayName("Thứ 6")]
        Fri,
					[XafDisplayName("Thứ 7")]
        Sat,
					[XafDisplayName("Chủ nhật")]
        Sun,
	    }

}