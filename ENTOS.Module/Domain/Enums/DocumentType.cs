using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum DocumentType
    {
					[XafDisplayName("Hiệu chỉnh")]
        Editing,
					[XafDisplayName("Phân tích")]
        Analysis,
					[XafDisplayName("Dịch thuật")]
        Translation,
					[XafDisplayName("Lồng tiếng")]
        VoiceOver,
					[XafDisplayName("Làm video")]
        Video,
	    }

}