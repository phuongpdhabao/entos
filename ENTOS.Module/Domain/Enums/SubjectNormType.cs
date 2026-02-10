using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum SubjectNormType
    {
					[XafDisplayName("Trận đấu")]
        Match,
					[XafDisplayName("Đội")]
        Team,
					[XafDisplayName("Đấu thủ")]
        Player,
	    }

}