namespace ENTOS.Module.Services
{
    public partial class OcrValueService
    {
        #region ValidationCheckExpression Helpers

        private static bool ValidationCheckExpression_HasValidation(string validation)
        {
            return !string.IsNullOrWhiteSpace(validation);
        }

        private static bool ValidationCheckExpression_IsTableLayout(string layout)
        {
            return layout == "Table";
        }

        private static bool ValidationCheckExpression_ContainsNumberToText(string validation)
        {
            return validation.Contains("NumberToText", System.StringComparison.OrdinalIgnoreCase);
        }

        private static bool ValidationCheckExpression_ContainsSum(string validation)
        {
            return validation.Contains("SUM", System.StringComparison.OrdinalIgnoreCase);
        }

        private static bool ValidationCheckExpression_ContainsEquality(string expr)
        {
            return expr.Contains("==");
        }

        private static string[] ValidationCheckExpression_SplitEquality(string expr)
        {
            return expr.Split("==", System.StringSplitOptions.TrimEntries);
        }

        #endregion

        #region CastValue Helpers

        private static bool CastValue_IsDecimalType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return false;
            }
            string lowerName = typeName.ToLower();
            return lowerName == "decimal" || lowerName == "number" || lowerName == "currency";
        }

        private static bool CastValue_IsIntegerType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return false;
            }
            string lowerName = typeName.ToLower();
            return lowerName == "int" || lowerName == "integer" || lowerName == "int32";
        }

        private static bool CastValue_IsDateType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return false;
            }
            string lowerName = typeName.ToLower();
            return lowerName == "date" || lowerName == "datetime";
        }

        #endregion

        #region Regex Helpers

        private static string ValidationCheckExpression_ExtractFieldFromMatch(System.Text.RegularExpressions.Match match)
        {
            if (match.Success && match.Groups.Count > 1)
            {
                return match.Groups[1].Value;
            }
            return null;
        }

        private static System.Collections.Generic.List<string> ValidationCheckExpression_ExtractFieldCodes(string expr)
        {
            var codes = new System.Collections.Generic.List<string>();
            var matches = System.Text.RegularExpressions.Regex.Matches(expr, @"[A-Za-z_]\w+");
            var seen = new System.Collections.Generic.HashSet<string>();
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                if (!seen.Contains(match.Value))
                {
                    codes.Add(match.Value);
                    seen.Add(match.Value);
                }
            }
            return codes;
        }

        #endregion

        #region Comparison Helpers

        private static bool CompareDecimalValues(decimal left, decimal right)
        {
            return left == right;
        }

        private static bool CompareStringValues(string value1, string value2)
        {
            return string.Equals(value1, value2, System.StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}
