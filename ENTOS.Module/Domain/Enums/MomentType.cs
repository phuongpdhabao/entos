using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum MomentType
    {
					[XafDisplayName("Phút")]
        Minute,
					[XafDisplayName("Giờ")]
        Hour,
					[XafDisplayName("Ngày")]
        Date,
					[XafDisplayName("Tuần")]
        Week,
					[XafDisplayName("Tháng")]
        Month,
					[XafDisplayName("Quý")]
        Quarter,
					[XafDisplayName("Năm")]
        Year,
					[XafDisplayName("Thập niên")]
        Decade,
					[XafDisplayName("Thế kỷ")]
        Century,
	    }

}