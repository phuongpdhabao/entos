using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum TermType
    {
					[XafDisplayName("Thuật ngữ")]
        Term,
					[XafDisplayName("Từ điển")]
        Dictionary,
					[XafDisplayName("Từ ghép")]
        MergeTerm,
					[XafDisplayName("Đầu hoa")]
        UpperCase,
					[XafDisplayName("Toàn hoa")]
        UpperCaseAll,
					[XafDisplayName("Viết tắt")]
        Short,
					[XafDisplayName("Số và ký tự")]
        Number,
					[XafDisplayName("Phi thuật")]
        NoneTerm,
	    }

}