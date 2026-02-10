using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum MethodType
    {
					[XafDisplayName("Đối tượng")]
        SoftwareObject,
					[XafDisplayName("Dịch vụ")]
        Service,
					[XafDisplayName("Handler sự kiện")]
        EventHandler,
	    }

}