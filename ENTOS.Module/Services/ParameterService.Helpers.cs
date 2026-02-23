using System.Globalization;

namespace ENTOS.Module.Services
{
    public partial class ParameterService
    {
        private static double ParseDoubleValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return System.Convert.ToDouble(value, new CultureInfo("en-US"));
            return 0;
        }
    }
}
