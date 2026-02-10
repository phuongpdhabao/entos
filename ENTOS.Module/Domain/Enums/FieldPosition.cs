using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum FieldPosition
    {
					[XafDisplayName("Cột trái")]
        Left,
					[XafDisplayName("Cột phải")]
        Right,
					[XafDisplayName("Tab")]
        Tab,
					[XafDisplayName("Không hiển thị")]
        Hidden,
	    }

}