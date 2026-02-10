using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum AlignmentRelative
    {
					[XafDisplayName("Margin")]
        Margin,
					[XafDisplayName("Page")]
        Page,
					[XafDisplayName("Column")]
        Column,
					[XafDisplayName("Character")]
        Character,
					[XafDisplayName("Left Margin")]
        LeftMargin,
					[XafDisplayName("Right Margin")]
        RightMargin,
					[XafDisplayName("Inside Margin")]
        InsideMargin,
					[XafDisplayName("Outside Margin")]
        OutsideMargin,
	    }

}