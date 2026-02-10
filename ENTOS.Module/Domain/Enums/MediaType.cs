using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum MediaType
    {
					[XafDisplayName("Video")]
        Video,
					[XafDisplayName("Ảnh")]
        Image,
					[XafDisplayName("Văn bản")]
        TextBox,
					[XafDisplayName("Nhóm đối tượng")]
        Group,
					[XafDisplayName("Tách nhóm")]
        UnGroup,
					[XafDisplayName("Hủy TextBox")]
        UnTextBox,
					[XafDisplayName("Tạo TextBox")]
        CreateTextBox,
	    }

}