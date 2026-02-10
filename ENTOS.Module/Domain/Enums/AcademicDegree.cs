using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum AcademicDegree
    {
					[XafDisplayName("Cao đẳng")]
        Associate,
					[XafDisplayName("Đại học")]
        Bachelor,
					[XafDisplayName("Thạc sĩ")]
        Master,
					[XafDisplayName("Tiến sĩ")]
        Doctor,
					[XafDisplayName("Tiến sĩ khoa học")]
        DoctorOfScience,
					[XafDisplayName("Trung cấp")]
        Profession,
					[XafDisplayName("Tú tài")]
        Diploma,
	    }

}