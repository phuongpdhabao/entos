using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum OrderType
    {
					[XafDisplayName("Báo giá")]
        Quotation,
					[XafDisplayName("Hợp đồng bán")]
        SaleContract,
					[XafDisplayName("Hợp đồng mua")]
        PurchaseContract,
					[XafDisplayName("Hóa đơn bán")]
        SalelInvoice,
					[XafDisplayName("Hóa đơn mua")]
        PurchaseInvoice,
					[XafDisplayName("Mua dịch vụ")]
        ServiceInvoice,
					[XafDisplayName("Chuyển hàng")]
        InternalInvoice,
	    }

}