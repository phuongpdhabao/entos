using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum Gender
    {
					[XafDisplayName("Nam")]
        Male,
					[XafDisplayName("Nữ")]
        Female,
					[XafDisplayName("Khác")]
        Other,
	    }

}