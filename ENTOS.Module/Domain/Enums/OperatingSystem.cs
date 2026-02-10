using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum OperatingSystem
    {
					[XafDisplayName("Đa nền tảng")]
        CrossPlatform,
					[XafDisplayName("Windows")]
        Windows,
					[XafDisplayName("MacOS")]
        MacOS,
					[XafDisplayName("Linux")]
        Linux,
					[XafDisplayName("iOS")]
        iOS,
					[XafDisplayName("Android")]
        Android,
					[XafDisplayName("Web")]
        Web,
	    }

}