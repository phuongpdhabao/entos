using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum DocType
    {
					[XafDisplayName("Chỉ thị")]
        ChiThi,
					[XafDisplayName("Định mức chi phí")]
        DinhMucChiPhi,
					[XafDisplayName("Định mức xây lắp")]
        DinhMucXayLap,
					[XafDisplayName("Đơn giá")]
        DonGia,
					[XafDisplayName("Luật")]
        Luat,
					[XafDisplayName("Nghị định")]
        NghiDinh,
					[XafDisplayName("Quyết định")]
        QuyetDinh,
					[XafDisplayName("Thông tư")]
        ThongTu,
	    }

}