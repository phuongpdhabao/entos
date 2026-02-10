using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum DataTypeModifier
    {
					[XafDisplayName("Abstract")]
        Abstract,
					[XafDisplayName("New")]
        New,
					[XafDisplayName("Partial")]
        Partial,
					[XafDisplayName("ReadOnly")]
        ReadOnly,
					[XafDisplayName("Ref")]
        Ref,
					[XafDisplayName("Sealed")]
        Sealed,
					[XafDisplayName("Static")]
        Static,
					[XafDisplayName("Unsafe")]
        Unsafe,
	    }

}