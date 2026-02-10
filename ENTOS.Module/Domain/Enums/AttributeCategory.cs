using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum AttributeCategory
    {
					[XafDisplayName("Hình thái")]
        Appearance,
					[XafDisplayName("Hành xử")]
        Behavior,
					[XafDisplayName("Dữ liệu")]
        Data,
					[XafDisplayName("Khác")]
        Mix,
	    }

}