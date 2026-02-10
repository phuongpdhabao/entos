using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum PriceType
    {
					[XafDisplayName("Chiết khấu")]
        Discount,
					[XafDisplayName("Lợi nhuận")]
        Margin,
	    }

}