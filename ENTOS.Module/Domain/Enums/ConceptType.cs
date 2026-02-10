using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ConceptType
    {
					[XafDisplayName("Sự vật")]
        Thing,
					[XafDisplayName("Nhân vật")]
        Person,
					[XafDisplayName("Tổ chức")]
        Organization,
					[XafDisplayName("Sự kiện")]
        Event,
					[XafDisplayName("Địa điểm")]
        Location,
					[XafDisplayName("Nguyên do")]
        Why,
					[XafDisplayName("Cách thức")]
        How,
	    }

}