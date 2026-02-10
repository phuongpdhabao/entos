using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ExportFieldType
    {
					[XafDisplayName("Kiểu cơ bản")]
        BasicType,
					[XafDisplayName("Kiểu eNum")]
        eNum,
					[XafDisplayName("Kiểu tham chiếu")]
        ReferenceType,
					[XafDisplayName("Tab 1-n")]
        Tab1N,
					[XafDisplayName("Tab n-n")]
        TabNN,
					[XafDisplayName("Trỏ Oid")]
        Oid,
	    }

}