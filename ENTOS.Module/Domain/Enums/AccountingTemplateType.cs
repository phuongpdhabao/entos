using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum AccountingTemplateType
    {
					[XafDisplayName("Bán")]
        Sell,
					[XafDisplayName("Mua")]
        Buy,
					[XafDisplayName("Nội bộ")]
        Local,
	    }

}