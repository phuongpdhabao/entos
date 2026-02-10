using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum DataServiceType
    {
					[XafDisplayName("Dịch thuật")]
        Translate,
					[XafDisplayName("Văn bản Giọng nói")]
        TTS,
					[XafDisplayName("Giọng nói Văn bản")]
        STT,
					[XafDisplayName("Cắt ảnh")]
        ImageCrop,
					[XafDisplayName("Tẩy nền ảnh")]
        RemoveBackground,
					[XafDisplayName("Nhận diện")]
        FaceDetect,
					[XafDisplayName("Tìm kiếm ảnh")]
        ImageSearch,
	    }

}