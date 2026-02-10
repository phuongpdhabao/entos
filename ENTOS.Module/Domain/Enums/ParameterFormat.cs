using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ParameterFormat
    {
					[XafDisplayName("Văn bản")]
        Text,
					[XafDisplayName("Số nguyên")]
        Int,
					[XafDisplayName("Thập phân")]
        Double,
					[XafDisplayName("Ngày")]
        Date,
					[XafDisplayName("Giờ")]
        Time,
					[XafDisplayName("Lôgic")]
        Boolean,
	    }

}