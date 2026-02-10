using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ResultFormat
    {
					[XafDisplayName("Ký tự")]
        String,
					[XafDisplayName("Phân số")]
        Fraction,
					[XafDisplayName("Số nguyên")]
        Int,
					[XafDisplayName("Số thập phân")]
        Decimal,
	    }

}