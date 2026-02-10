using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum SoftwareExtensionType
    {
					[XafDisplayName("Mô đun")]
        Module,
					[XafDisplayName("Plugin")]
        Plugin,
					[XafDisplayName("Theme")]
        Theme,
	    }

}