using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum SoftwarePlatformType
    {
					[XafDisplayName("Giao diện")]
        UserInterface,
					[XafDisplayName("Nội dung")]
        CMS,
					[XafDisplayName("Nghiệp vụ")]
        Enterprise,
					[XafDisplayName("Máy học")]
        MachineLearning,
					[XafDisplayName("Trò chơi")]
        GameEngine,
					[XafDisplayName("IoT")]
        IoT,
					[XafDisplayName("Phát triển")]
        Development,
	    }

}