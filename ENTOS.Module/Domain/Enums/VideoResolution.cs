using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum VideoResolution
    {
					[XafDisplayName("SD 480p 640 x 480")]
        SD,
					[XafDisplayName("HD 720p 1280 x 720")]
        HD,
					[XafDisplayName("Full HD 1080p 1920 x 1080")]
        FHD,
					[XafDisplayName("Quad HD 1440p 2560 x 1440")]
        QHD,
					[XafDisplayName("2K 1080p 2048 x 1080")]
        V2K,
					[XafDisplayName("4K 2160p 3840 x 2160")]
        V4K,
					[XafDisplayName("8K 4320p 7680 x 4320")]
        V8K,
	    }

}