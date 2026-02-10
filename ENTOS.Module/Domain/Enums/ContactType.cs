using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ContactType
    {
					[XafDisplayName("Khách hàng")]
        Customer,
					[XafDisplayName("Đối tác")]
        Partner,
					[XafDisplayName("Nhân viên")]
        Staff,
					[XafDisplayName("Người nổi tiếng")]
        Celebrity,
					[XafDisplayName("Học sinh")]
        Student,
					[XafDisplayName("Giáo viên")]
        Teacher,
	    }

}