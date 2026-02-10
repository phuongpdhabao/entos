using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum LinkType
    {
					[XafDisplayName("Trang chủ")]
        Home,
					[XafDisplayName(" Thư mục")]
        Folder,
					[XafDisplayName("Tài liệu")]
        Document,
					[XafDisplayName("Tin tức")]
        News,
					[XafDisplayName("Ảnh đại diện")]
        Image,
					[XafDisplayName("Ảnh")]
        Photo,
					[XafDisplayName("Di động")]
        Mobile,
					[XafDisplayName("Zalo")]
        Zalo,
					[XafDisplayName("Facebook")]
        Facebook,
					[XafDisplayName("Email")]
        Email,
					[XafDisplayName("Địa chỉ")]
        Address,
					[XafDisplayName("Bản đồ")]
        Map,
	    }

}