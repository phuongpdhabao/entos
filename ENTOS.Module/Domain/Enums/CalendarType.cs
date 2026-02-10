using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum CalendarType
    {
					[XafDisplayName("Kế hoạch")]
        Plan,
					[XafDisplayName("Thực thi")]
        Fact,
					[XafDisplayName("Công việc")]
        Work,
					[XafDisplayName("Cá nhân")]
        Personal,
					[XafDisplayName("Gia đình")]
        Family,
	    }

}