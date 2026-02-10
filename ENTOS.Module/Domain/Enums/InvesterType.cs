using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum InvesterType
    {
					[XafDisplayName("Mạo hiểm")]
        Venture,
					[XafDisplayName("Quỹ đầu tư")]
        Fund,
					[XafDisplayName("Công ty chứng khoán")]
        Security,
					[XafDisplayName("Ngân hàng")]
        Bank,
					[XafDisplayName("Doanh nghiệp")]
        Enterprise,
					[XafDisplayName("Cá nhân")]
        Private,
	    }

}