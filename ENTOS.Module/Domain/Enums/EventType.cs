using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum EventType
    {
					[XafDisplayName("Giải đấu")]
        Tournament,
					[XafDisplayName("Cuộc thi")]
        Contest,
					[XafDisplayName("Giải thưởng")]
        Award,
					[XafDisplayName("Hội thảo")]
        Semina,
					[XafDisplayName("Team building")]
        TeamBuilding,
					[XafDisplayName("Đào tạo")]
        Training,
	    }

}