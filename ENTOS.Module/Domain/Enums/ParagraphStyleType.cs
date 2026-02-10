using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum ParagraphStyleType
    {
					[XafDisplayName(" ")]
        Empty,
					[XafDisplayName("Paragraph")]
        Paragraph,
					[XafDisplayName("Character")]
        Character,
					[XafDisplayName("Linked (paragraph and character)")]
        Linked,
					[XafDisplayName("Table")]
        Table,
					[XafDisplayName("List")]
        List,
	    }

}