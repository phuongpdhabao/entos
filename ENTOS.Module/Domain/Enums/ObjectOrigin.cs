using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ObjectOrigin
    {
					[XafDisplayName("Chức năng phần mềm")]
        SoftwareAction,
					[XafDisplayName("Báo giá")]
        Quotation,
					[XafDisplayName("Hợp đồng bán")]
        SalesContract,
					[XafDisplayName("Hợp đồng mua")]
        PurchaseContract,
					[XafDisplayName("Hóa đơn bán")]
        SalesInvoice,
					[XafDisplayName("Hóa đơn mua")]
        PurchaseInvoice,
					[XafDisplayName("Ủy nhiệm chi")]
        BankOut,
					[XafDisplayName("Báo có")]
        BankIn,
					[XafDisplayName("Phiếu chi")]
        CashOut,
					[XafDisplayName("Phiếu thu")]
        CashIn,
					[XafDisplayName("Chuyển tiền")]
        CashMove,
	    }

}