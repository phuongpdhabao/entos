using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ContentCategory
    {
					[XafDisplayName("Nội dung")]
        Content,
					[XafDisplayName("Chương trình")]
        ShowTime,
					[XafDisplayName("Gói kênh")]
        ChannelGroup,
	    }

}