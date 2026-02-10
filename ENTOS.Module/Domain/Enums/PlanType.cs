using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum PlanType
    {
					[XafDisplayName(" ")]
        General,
					[XafDisplayName("Biên bản họp")]
        Meeting,
					[XafDisplayName("Dự án")]
        Project,
					[XafDisplayName("Kinh doanh")]
        Business,
					[XafDisplayName("Hội thảo")]
        Semina,
	    }

}