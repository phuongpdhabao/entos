using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ClassMemberModifier
    {
					[XafDisplayName("Static")]
        Static,
					[XafDisplayName("Abstract")]
        Abstract,
					[XafDisplayName("Async")]
        Async,
					[XafDisplayName("Const")]
        Const,
					[XafDisplayName("Extern")]
        Extern,
					[XafDisplayName("Init")]
        Init,
					[XafDisplayName("New")]
        New,
					[XafDisplayName("Override")]
        Override,
					[XafDisplayName("Partial")]
        Partial,
					[XafDisplayName("Readonly")]
        Readonly,
					[XafDisplayName("Sealed")]
        Sealed,
					[XafDisplayName("Event")]
        Event,
					[XafDisplayName("Unsafe")]
        Unsafe,
					[XafDisplayName("Virtual")]
        Virtual,
					[XafDisplayName("Volatile")]
        Volatile,
	    }

}