using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum SoftwareObjectEvent
    {
					[XafDisplayName("Đã lưu")]
        Saved,
					[XafDisplayName("Đã nạp")]
        Loaded,
					[XafDisplayName("Đã xóa")]
        Deleted,
					[XafDisplayName("Đang lưu")]
        Saving,
					[XafDisplayName("Đang nạp")]
        Loading,
					[XafDisplayName("Đang xóa")]
        Deleting,
					[XafDisplayName("Mới")]
        New,
					[XafDisplayName("Thay đổi")]
        Changed,
	    }

}