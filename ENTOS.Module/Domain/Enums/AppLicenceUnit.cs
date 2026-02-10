using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum AppLicenceUnit
    {
					[XafDisplayName("Người dùng")]
        User,
					[XafDisplayName("Thiết bị")]
        Device,
					[XafDisplayName("Bộ vi xử lý")]
        CPU,
					[XafDisplayName("Nhân vi xử lý")]
        Core,
	    }

}