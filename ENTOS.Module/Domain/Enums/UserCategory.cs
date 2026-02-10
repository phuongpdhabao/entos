using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum UserCategory
    {
					[XafDisplayName("Cá nhân")]
        Personal,
					[XafDisplayName("Chính phủ")]
        Government,
					[XafDisplayName("Doanh nghiệp")]
        Business,
					[XafDisplayName("Giáo dục")]
        Education,
					[XafDisplayName("Giáo viên")]
        Teacher,
					[XafDisplayName("Học sinh")]
        Student,
	    }

}