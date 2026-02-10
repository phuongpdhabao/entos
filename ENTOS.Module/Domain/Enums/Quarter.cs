using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum Quarter
    {
					[XafDisplayName(" ")]
        None,
					[XafDisplayName("Quý 1")]
        FirstQuarter,
					[XafDisplayName("Quý 2")]
        SecondQuarter,
					[XafDisplayName("Quý 3")]
        ThirdQuarter,
					[XafDisplayName("Quý 4")]
        ForthQuarter,
	    }

}