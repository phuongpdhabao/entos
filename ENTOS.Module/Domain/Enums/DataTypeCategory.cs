using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum DataTypeCategory
    {
					[XafDisplayName("eNum")]
        eNum,
					[XafDisplayName("Struct")]
        Struct,
					[XafDisplayName("Interface")]
        Interface,
					[XafDisplayName("Lớp")]
        SoftwareClass,
					[XafDisplayName("Delegate")]
        Delegate,
	    }

}