using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum SoftwareClassType
    {
					[XafDisplayName("Controller")]
        Controller,
					[XafDisplayName("Dịch vụ")]
        SoftwareService,
					[XafDisplayName("Đối tượng")]
        SoftwareObject,
					[XafDisplayName("Editor")]
        SoftwareEditor,
					[XafDisplayName("Mở rộng")]
        SoftwareExtension,
					[XafDisplayName("Attribute")]
        SoftwareAttribute,
					[XafDisplayName("Tiện ích")]
        SoftwareUtility,
					[XafDisplayName("Trợ giúp")]
        SoftwareHelper,
					[XafDisplayName("Thư viện")]
        Library,
	    }

}