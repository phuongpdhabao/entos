using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum DataServiceParameterType
    {
					[XafDisplayName("Giọng đọc")]
        Voice,
					[XafDisplayName("Mô hình AI")]
        Model,
					[XafDisplayName("Tham số ")]
        Parameter,
	    }

}