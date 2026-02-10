using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum AssetContainerType
    {
					[XafDisplayName("Kho")]
        Stock,
					[XafDisplayName("Công cụ dụng cụ")]
        Tool,
					[XafDisplayName("Tài sản cố định")]
        FixedAsset,
	    }

}