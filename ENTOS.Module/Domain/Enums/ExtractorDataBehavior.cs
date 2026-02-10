using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ExtractorDataBehavior
    {
					[XafDisplayName(" ")]
        None,
					[XafDisplayName("Lấy số")]
        Number,
					[XafDisplayName("Lấy bên trái")]
        Left,
					[XafDisplayName("Lấy bên phải")]
        Right,
					[XafDisplayName("Thay thế")]
        Replace,
					[XafDisplayName("Lấy ngày")]
        Date,
	    }

}