using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum CaseType
    {
					[XafDisplayName("Thường")]
        General,
					[XafDisplayName("Đầu hoa")]
        UpperCase,
					[XafDisplayName("Toàn hoa")]
        UpperCaseAll,
					[XafDisplayName("Ghép hoa")]
        UpperCaseMerge,
					[XafDisplayName("Nhiều hoa")]
        UpperCaseMany,
	    }

}