using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum EntryType
    {
					[XafDisplayName("Nợ")]
        Debit,
					[XafDisplayName("Có")]
        Credit,
	    }

}