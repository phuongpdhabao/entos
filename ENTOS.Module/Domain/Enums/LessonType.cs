using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum LessonType
    {
					[XafDisplayName("Thông thường")]
        Normal,
					[XafDisplayName("Kiểm tra")]
        Test,
					[XafDisplayName("Thi học kỳ")]
        SemesterTest,
	    }

}