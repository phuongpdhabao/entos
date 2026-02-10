using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum Alignment
    {
					[XafDisplayName(" ")]
        Empty,
					[XafDisplayName("Căn trái")]
        Left,
					[XafDisplayName("Căn giữa")]
        Centered,
					[XafDisplayName("Căn phải")]
        Right,
					[XafDisplayName("Căn đều")]
        Justified,
	    }

}