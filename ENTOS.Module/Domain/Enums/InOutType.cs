using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum InOutType
    {
					[XafDisplayName("Cần mua")]
        Order,
					[XafDisplayName("Đã mua")]
        Ordered,
					[XafDisplayName("Đã nhập")]
        In,
					[XafDisplayName("Đã xuất")]
        Out,
					[XafDisplayName("Giữ")]
        Booked,
	    }

}