using System;

namespace ENTOS.Module.Services
{
    public partial class OcrValueService
    {
        private static string ExtractValidationFieldCode(string validation, string functionName)
        {
            string pattern = $"{functionName}\\((\\w+)\\)";
            var match = System.Text.RegularExpressions.Regex.Match(validation, pattern);
            if (match.Success)
                return match.Groups[1].Value;

            return null;
        }

        private static bool EvaluateExpression(string expression, string currentValue, string currentDataType)
        {
            if (expression.Contains("=="))
            {
                var parts = expression.Split("==", StringSplitOptions.TrimEntries);
                if (parts.Length == 2)
                {
                    decimal left = Convert.ToDecimal(new System.Data.DataTable().Compute(parts[0], ""));
                    decimal right = Convert.ToDecimal(new System.Data.DataTable().Compute(parts[1], ""));
                    return left == right;
                }
                return false;
            }

            decimal computed = Convert.ToDecimal(new System.Data.DataTable().Compute(expression, ""));
            decimal current = Convert.ToDecimal(CastValue(currentValue, currentDataType));
            return computed == current;
        }
    }
}
