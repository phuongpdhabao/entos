using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum KnowledgeCategory
    {
					[XafDisplayName("Kiến thức đại cương")]
        General,
					[XafDisplayName("Kiến thức chuyên ngành")]
        Specialization,
					[XafDisplayName("Kiến thức cơ sở ngành")]
        SpecializationBackground,
					[XafDisplayName("Kiễn thức kỹ năng")]
        Skill,
					[XafDisplayName("Giáo dục thể chất")]
        PhysicalEducation,
					[XafDisplayName("An ninh quốc phòng")]
        NationalDefense,
	    }

}