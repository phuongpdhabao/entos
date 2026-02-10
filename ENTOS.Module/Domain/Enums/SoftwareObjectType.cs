using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum SoftwareObjectType
    {
					[XafDisplayName("Liên kết")]
        Link,
					[XafDisplayName("Loại sản phẩm")]
        ProductType,
					[XafDisplayName("Sản phẩm")]
        Product,
					[XafDisplayName("Liên hệ")]
        Contact,
					[XafDisplayName("Thư mục")]
        Folder,
					[XafDisplayName("Loại công việc")]
        WorkType,
					[XafDisplayName("Công việc")]
        Work,
					[XafDisplayName("Lịch")]
        Calendar,
					[XafDisplayName("Bài viết")]
        Post,
					[XafDisplayName("Niêm yết")]
        ProductListing,
					[XafDisplayName("Ứng dụng")]
        App,
					[XafDisplayName("Tiêu dùng")]
        Consume,
					[XafDisplayName("Thành viên")]
        Member,
					[XafDisplayName("Tổ chức")]
        Org,
					[XafDisplayName("Kế toán")]
        Accounting,
					[XafDisplayName("Tư liệu")]
        Video,
					[XafDisplayName("Tài sản")]
        Asset,
					[XafDisplayName("Đấu thủ")]
        Player,
					[XafDisplayName("Bộ phận")]
        Division,
					[XafDisplayName("Nhận dạng")]
        Recognition,
					[XafDisplayName("Công ty")]
        Company,
					[XafDisplayName("Biểu tượng")]
        SoftwareIcon,
					[XafDisplayName("Cửa hàng")]
        Shop,
					[XafDisplayName("Đối tượng")]
        SoftwareObject,
					[XafDisplayName("Phương thức")]
        Method,
					[XafDisplayName("Trường")]
        Field,
					[XafDisplayName("Kiểu dữ liệu")]
        DataType,
					[XafDisplayName("Lớp phần mềm")]
        SoftwareClass,
					[XafDisplayName("Mẫu tệp mã")]
        CodeFileTemplate,
					[XafDisplayName("Khởi tạo")]
        Constructor,
					[XafDisplayName("Sự kiện phần mềm")]
        SoftwareEvent,
					[XafDisplayName("Interface")]
        Interface,
					[XafDisplayName("Mã nguồn")]
        SourceCode,
					[XafDisplayName("Tham số phần mềm")]
        SoftwareParameter,
					[XafDisplayName("Thành viên kiểu")]
        DataTypeMember,
					[XafDisplayName("Thư viện")]
        SoftwareLibrary,
					[XafDisplayName("Loại dự án")]
        ProjectType,
					[XafDisplayName("Dự án phần mềm")]
        SoftwareProject,
	    }

}