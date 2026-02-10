using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum VideoCodec
    {
					[XafDisplayName("H264")]
        H264,
					[XafDisplayName("H265")]
        H265,
					[XafDisplayName("MPEG-4 Part 2")]
        MPEG4Part2,
					[XafDisplayName("WMV")]
        WMV,
	    }

}