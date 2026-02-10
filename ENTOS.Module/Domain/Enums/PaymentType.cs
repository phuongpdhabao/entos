using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum PaymentType
    {
					[XafDisplayName("Báo có")]
        BankIn,
					[XafDisplayName("Chi tiền")]
        FundOut,
					[XafDisplayName("Thu tiền")]
        FundIn,
					[XafDisplayName("Ủy nhiệm chi")]
        BankOut,
	    }

}