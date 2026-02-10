using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum PlayerType
    {
					[XafDisplayName("Đấu thủ")]
        Player,
					[XafDisplayName("Huấn luyện")]
        Coach,
					[XafDisplayName("Trọng tài")]
        Referee,
					[XafDisplayName("Hỗ trợ")]
        Supporter,
					[XafDisplayName("Giám khảo")]
        Examiner,
	    }

}