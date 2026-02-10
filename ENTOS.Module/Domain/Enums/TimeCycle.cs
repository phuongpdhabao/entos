using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum TimeCycle
    {
					[XafDisplayName(" ")]
        None,
					[XafDisplayName("Ngày")]
        Daily,
					[XafDisplayName("Tuần")]
        Weekly,
					[XafDisplayName("Tháng")]
        Monthly,
					[XafDisplayName("Quý")]
        Quarterly,
					[XafDisplayName("Năm")]
        Annual,
	    }

}