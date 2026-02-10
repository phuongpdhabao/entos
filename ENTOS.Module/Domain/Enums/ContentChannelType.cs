using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ContentChannelType
    {
					[XafDisplayName("Kênh truyền hình")]
        TelevisionChannel,
					[XafDisplayName("Kênh phát thanh")]
        RadioChannel,
					[XafDisplayName("Rạp chiếu phim")]
        Cinema,
					[XafDisplayName("Nhà hát")]
        Theatre,
					[XafDisplayName("Sân vận động")]
        Stadium,
	    }

}