using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum BeginEnd
    {
					[XafDisplayName(" ")]
        None,
					[XafDisplayName("Đầu")]
        Begin,
					[XafDisplayName("Giữa")]
        Middle,
					[XafDisplayName("Cuối")]
        End,
	    }

}