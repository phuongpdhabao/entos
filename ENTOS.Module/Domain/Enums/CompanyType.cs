using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum CompanyType
    {
					[XafDisplayName("Khởi nghiệp")]
        Startup,
					[XafDisplayName("Cổ phần")]
        JoinStock,
					[XafDisplayName("Đại chúng")]
        IPO,
					[XafDisplayName("Niêm yết")]
        Listed,
	    }

}