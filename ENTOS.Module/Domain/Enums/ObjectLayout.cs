using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ObjectLayout
    {
					[XafDisplayName("Behind text")]
        BehindText,
					[XafDisplayName("In front of text")]
        InfrontOfText,
					[XafDisplayName("Inline with text")]
        InlineWithText,
					[XafDisplayName("Square")]
        Square,
					[XafDisplayName("Through")]
        Through,
					[XafDisplayName("Tight")]
        Tight,
					[XafDisplayName("Top and bottom")]
        TopAndBottom,
	    }

}