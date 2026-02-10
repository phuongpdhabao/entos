using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum DataTypeMemberType
    {
					[XafDisplayName("Hằng số")]
        Constant,
					[XafDisplayName("Phương thức")]
        Method,
					[XafDisplayName("Property")]
        Property,
					[XafDisplayName("Field")]
        Field,
					[XafDisplayName("Sự kiện")]
        SoftwareEvent,
					[XafDisplayName("Khởi tạo")]
        Constructor,
					[XafDisplayName("Chỉ mục")]
        Indexer,
					[XafDisplayName("Chung kết")]
        Finalizer,
					[XafDisplayName("Toán tử")]
        Operator,
					[XafDisplayName("Kiểu lồng")]
        NestedType,
	    }

}