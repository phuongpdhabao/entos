using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum SourceCodeType
    {
					[XafDisplayName("Đầy đủ")]
        Fulll,
					[XafDisplayName("Đơn giản")]
        Simple,
					[XafDisplayName("Tối giản")]
        Minimum,
	    }

}