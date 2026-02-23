using DevExpress.Data.Filtering;

namespace ENTOS.Module.Services
{
    public partial class LanguageService
    {
        private static CriteriaOperator GetTranslateDataServiceCriteria()
        {
            return CriteriaOperator.Parse("SoftwareServiceType.Code = 'Translate'");
        }
    }
}
