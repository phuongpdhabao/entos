using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum CurrencyType
    {
					[XafDisplayName("VNĐ")]
        VND,
					[XafDisplayName("USD")]
        USD,
					[XafDisplayName("EUR")]
        EUR,
					[XafDisplayName("AUD")]
        AUD,
					[XafDisplayName("SGD")]
        SGD,
					[XafDisplayName("CAD")]
        CAD,
					[XafDisplayName("GBP")]
        GBP,
	    }

}