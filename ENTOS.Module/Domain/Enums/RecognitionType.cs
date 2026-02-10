using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum RecognitionType
    {
					[XafDisplayName("Khuôn mặt")]
        Face,
					[XafDisplayName("Biển số")]
        NumberPlate,
					[XafDisplayName("Ô tô")]
        Car,
					[XafDisplayName("Xe máy")]
        Motorcycle,
	    }

}