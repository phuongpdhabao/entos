using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum UserInterfaceType
    {
					[XafDisplayName("Common")]
        Common,
					[XafDisplayName("Desktop")]
        Desktop,
					[XafDisplayName("Web")]
        Web,
					[XafDisplayName("Mobile")]
        Mobile,
					[XafDisplayName("Game")]
        Game,
	    }

}