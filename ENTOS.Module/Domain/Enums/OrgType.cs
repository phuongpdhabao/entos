using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum OrgType
    {
					[XafDisplayName("Khách hàng")]
        Customer,
					[XafDisplayName("Nhà cung cấp")]
        Vendor,
					[XafDisplayName("Nhà sản xuất")]
        Brand,
					[XafDisplayName("Đối tác")]
        Partner,
					[XafDisplayName("Trường học")]
        School,
					[XafDisplayName("Bệnh viện")]
        Hospital,
					[XafDisplayName("Chính quyền")]
        Government,
					[XafDisplayName("Công an Quân đội")]
        PoliceArmy,
	    }

}