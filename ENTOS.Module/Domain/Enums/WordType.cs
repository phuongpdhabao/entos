using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum WordType
    {
					[XafDisplayName(" ")]
        Blank,
					[XafDisplayName("Danh từ")]
        Noun,
					[XafDisplayName("Đại từ")]
        Pronoun,
					[XafDisplayName("Tính từ")]
        Adjective,
					[XafDisplayName("Động từ")]
        Verb,
					[XafDisplayName("Trạng từ")]
        Adverb,
					[XafDisplayName("Giới từ")]
        Preposition,
					[XafDisplayName("Từ hạn định")]
        Determiner,
					[XafDisplayName("Liên từ")]
        Conjunction,
					[XafDisplayName("Thán từ")]
        Interjection,
					[XafDisplayName("Số từ")]
        Numeral,
					[XafDisplayName("Đơn vị")]
        Unit,
	    }

}