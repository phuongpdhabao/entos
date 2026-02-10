using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ApiMethodType
    {
					[XafDisplayName("Get")]
        Get,
					[XafDisplayName("Post")]
        Post,
					[XafDisplayName("Chạy file")]
        File,
	    }

}