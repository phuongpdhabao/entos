using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum Evaluation
    {
					[XafDisplayName("1 Star")]
        OneStar,
					[XafDisplayName("2 Star")]
        TwoStar,
					[XafDisplayName("3 Star")]
        ThreeStar,
					[XafDisplayName("4 Star")]
        FourStar,
					[XafDisplayName("5 Star")]
        FiveStar,
	    }

}