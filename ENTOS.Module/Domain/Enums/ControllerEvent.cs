using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ControllerEvent
    {
					[XafDisplayName("Tạo mới")]
        ViewControlsCreated,
					[XafDisplayName("Kích hoạt")]
        Activated,
					[XafDisplayName("Hủy kích hoạt")]
        Deactivated,
	    }

}