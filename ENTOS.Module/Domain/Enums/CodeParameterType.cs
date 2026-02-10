using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum CodeParameterType
    {
					[XafDisplayName("Trường")]
        Field,
					[XafDisplayName("Phương thức")]
        Method,
					[XafDisplayName("Đối tượng")]
        Object,
	    }

}