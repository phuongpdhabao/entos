using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum AccessModifier
    {
					[XafDisplayName("Public")]
        Public,
					[XafDisplayName("Protected Internal")]
        ProtectedInternal,
					[XafDisplayName("Protected")]
        Protected,
					[XafDisplayName("Internal")]
        Internal,
					[XafDisplayName("Private Protected")]
        PrivateProtected,
					[XafDisplayName("Private")]
        Private,
	    }

}