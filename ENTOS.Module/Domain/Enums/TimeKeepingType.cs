using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum TimeKeepingType
    {
					[XafDisplayName("Nghỉ có báo")]
        InformOffWork,
					[XafDisplayName("Nghỉ có báo 2")]
        InformOffWork2,
					[XafDisplayName("Nghỉ phép")]
        OnLeave,
					[XafDisplayName("Nghỉ không báo")]
        NoInformOffWork,
					[XafDisplayName("Nghỉ không báo 2")]
        NoInformOffWork2,
					[XafDisplayName("Làm thêm tự nguyện")]
        OverTimeWork,
					[XafDisplayName("Làm thêm theo yêu cầu")]
        OverTimeWork2,
					[XafDisplayName("Làm thêm theo yêu cầu 2")]
        OverTimeWork3,
					[XafDisplayName("Đăng nhập")]
        Login,
	    }

}