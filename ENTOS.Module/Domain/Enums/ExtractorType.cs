using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ExtractorType
    {
					[XafDisplayName("Lấy văn bản")]
        Text,
					[XafDisplayName("Lấy liên kết")]
        Link,
					[XafDisplayName("Lấy ảnh")]
        Image,
					[XafDisplayName("Lấy ảnh trong liên kết")]
        ImageInLink,
					[XafDisplayName("Lấy bảng")]
        Table,
					[XafDisplayName("Nhập text")]
        Input,
					[XafDisplayName("Nhập mật khẩu")]
        Password,
					[XafDisplayName("Bấm nút")]
        Button,
					[XafDisplayName("Đợi")]
        Wait,
					[XafDisplayName("Chạy Javascript")]
        RunJavascript,
					[XafDisplayName("Captcha")]
        Captcha,
					[XafDisplayName("Html")]
        Html,
					[XafDisplayName("Xóa thừa")]
        Delete,
					[XafDisplayName("Thay thế")]
        Replace,
	    }

}