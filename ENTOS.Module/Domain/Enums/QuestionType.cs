using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum QuestionType
    {
					[XafDisplayName("Chọn một")]
        OneChoice,
					[XafDisplayName("Chọn nhiều")]
        MultiChoice,
					[XafDisplayName("Đáp số")]
        ResultValue,
					[XafDisplayName("Phức hợp")]
        Complex,
					[XafDisplayName("Tự luận")]
        Essay,
	    }

}