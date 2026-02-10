using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.Xpo;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ENTOS.Module;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;
using ENTOS.Module.Helpers;
using ENTOS.Module.Extensions;
using ENTOS.Module.SystemServices;
using ENTOS.Module.Services;


 
namespace ENTOS.Module.Services 
{

    public partial class OcrValueService : BaseService
    {

        public OcrValueService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public OcrValueService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3904ImportCode
               public static bool ValidationCheckExpression(OcrValue ocrValue, List<OcrValue> allValues)
       {
           if (ocrValue?.ExtractionKey == null)
               return true;

           string validation = ocrValue.ExtractionKey.Validation;
           if (string.IsNullOrWhiteSpace(validation))
               return true; // không có rule thì coi như hợp lệ

           string layout = ocrValue.ExtractionKey.DataLayout.GetName() ?? "";

           // Nếu là table: chỉ lấy các giá trị có cùng Y
           var scopeValues = layout == "Table"
               ? allValues.Where(v => v.Y == ocrValue.Y && v.OcrPage == ocrValue.OcrPage).ToList()
               : allValues;

           try
           {
               // ------------------------
               // Trường hợp NumberToText
               // ------------------------
               if (validation.Contains("NumberToText", StringComparison.OrdinalIgnoreCase))
               {
                   // Ví dụ: "NumberToText(FieldA) == 'một trăm'"
                   var match = System.Text.RegularExpressions.Regex.Match(validation, @"NumberToText\((\w+)\)");
                   if (match.Success)
                   {
                       string fieldCode = match.Groups[1].Value;
                       string expectedText = ocrValue.Value;

                       var val = scopeValues.FirstOrDefault(v => v.ExtractionKey.Code == fieldCode);
                       var value = CastValue(val?.Value, val?.ExtractionKey.DataType?.Name) as decimal?;
                       if (val != null)
                       {
                           string word = Module.Extensions.NumberExtensions.ToWordsVN(value.ToInt()) + " đồng";
                           return string.Equals(word, expectedText, StringComparison.OrdinalIgnoreCase);
                       }
                   }
                   return false;
               }

               // ------------------------
               // Trường hợp SUM
               // ------------------------
               if (validation.Contains("SUM", StringComparison.OrdinalIgnoreCase))
               {
                   // Ví dụ: "SUM(FieldA) == FieldB"
                   var match = System.Text.RegularExpressions.Regex.Match(validation, @"SUM\((\w+)\)");
                   if (match.Success)
                   {
                       string fieldCode = match.Groups[1].Value;

                       var sumVals = scopeValues
                           .Where(v => v.ExtractionKey.Code == fieldCode)
                           .Select(v => Convert.ToDecimal(CastValue(v.Value, v.ExtractionKey.DataType?.Name)))
                           .ToList();

                       decimal sum = sumVals.Sum();

                       decimal target = Convert.ToDecimal(CastValue(ocrValue.Value, ocrValue.ExtractionKey.DataType?.Name));
                       return sum == target;
                   }
                   return false;
               }

               // ------------------------
               // Trường hợp biểu thức toán cơ bản
               // ------------------------
               // Ví dụ: "FieldA + FieldB == FieldC"
               string expr = validation;

               // Tìm các fieldCode (chỉ lấy chữ và số, bỏ toán tử)
               var codes = System.Text.RegularExpressions.Regex.Matches(expr, @"[A-Za-z_]\w+")
                   .Cast<System.Text.RegularExpressions.Match>()
                   .Select(m => m.Value)
                   .Distinct()
                   .ToList();

               foreach (var code in codes)
               {
                   var val = scopeValues.FirstOrDefault(v => v.ExtractionKey.Code == code);
                   if (val != null)
                   {
                       object casted = CastValue(val.Value, val.ExtractionKey.DataType?.Name);
                       expr = System.Text.RegularExpressions.Regex.Replace(expr, $@"\b{code}\b", casted.ToString());
                   }
               }

               // Thay == thành toán tử C# để evaluate
               if (expr.Contains("=="))
               {
                   var parts = expr.Split("==", StringSplitOptions.TrimEntries);
                   if (parts.Length == 2)
                   {
                       decimal left = Convert.ToDecimal(new System.Data.DataTable().Compute(parts[0], ""));
                       decimal right = Convert.ToDecimal(new System.Data.DataTable().Compute(parts[1], ""));
                       return left == right;
                   }
               }
               else
               {
                   // Chỉ là 1 phép tính: so sánh với chính ocrValue.Value
                   decimal computed = Convert.ToDecimal(new System.Data.DataTable().Compute(expr, ""));
                   decimal current = Convert.ToDecimal(CastValue(ocrValue.Value, ocrValue.ExtractionKey.DataType?.Name));
                   return computed == current;
               }
           }
           catch
           {
               return false;
           }

           return false;
       }


        #endregion SourceCode3904ImportCode

        #region SourceCode3882ImportCode
                                        
        private static readonly Dictionary<string, Type> TypeMap = new(StringComparer.OrdinalIgnoreCase)
        {
            // integer
            { "int", typeof(int) },
            { "int32", typeof(int) },
            { "int64", typeof(long) },
            { "long", typeof(long) },
            { "short", typeof(short) },
            { "int16", typeof(short) },

            // float / decimal
            { "float", typeof(float) },
            { "double", typeof(double) },
            { "decimal", typeof(decimal) },

            // boolean
            { "bool", typeof(bool) },
            { "boolean", typeof(bool) },

            // datetime
            { "datetime", typeof(DateTime) },

            // guid
            { "guid", typeof(Guid) },

            // string
            { "string", typeof(string) }
        };
        private static string NormalizeNumberString(string value)
        {
            value = value.Trim();
            // Xử lý phần trăm
            if (value.EndsWith("%"))
            {
                value = value.Substring(0, value.Length - 1).Trim();

                if (decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var number))
                {
                    number = number / 100m;
                    value = number.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }

            }

            if (value.Contains(",") && value.Contains("."))
            {
                if (value.IndexOf(',') < value.IndexOf('.'))
                {
                    // US format: 1,234,567.89
                    value = value.Replace(",", "");
                }
                else
                {
                    // EU format: 1.234.567,89
                    int lastComma = value.LastIndexOf(',');
                    string integerPart = value.Substring(0, lastComma).Replace(".", "");
                    string decimalPart = value.Substring(lastComma + 1);
                    value = integerPart + "." + decimalPart;
                }
            }
            else if (value.Contains(","))
            {
                int lastComma = value.LastIndexOf(',');
                string[] parts = value.Split(',');
                string lastPart = parts[^1];

                if (lastPart.Length == 3)
                {
                    // nghìn: xóa hết dấu ,
                    value = value.Replace(",", "");
                }
                else
                {
                    // thập phân: chỉ giữ dấu cuối làm thập phân
                    string integerPart = value.Substring(0, lastComma).Replace(",", "");
                    string decimalPart = value.Substring(lastComma + 1);
                    value = integerPart + "." + decimalPart;
                }
            }
            else if (value.Contains("."))
            {
                int lastDot = value.LastIndexOf('.');
                string[] parts = value.Split('.');
                string lastPart = parts[^1];

                if (lastPart.Length == 3)
                {
                    // nghìn: xóa hết dấu .
                    value = value.Replace(".", "");
                }
                else
                {
                    // thập phân: chỉ giữ dấu cuối làm thập phân
                    string integerPart = value.Substring(0, lastDot).Replace(".", "");
                    string decimalPart = value.Substring(lastDot + 1);
                    value = integerPart + "." + decimalPart;
                }
            }

            return value;
        }
        public static object CastValue(string value, string dataType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dataType))
                    return value;

                var isNullable = dataType.EndsWith("?");
                var typeName = isNullable ? dataType.TrimEnd('?') : dataType;

                if (!TypeMap.TryGetValue(typeName, out var type))
                    type = typeof(string);

                if (isNullable)
                    type = typeof(Nullable<>).MakeGenericType(type);

                if (string.IsNullOrWhiteSpace(value))
                    return null;

                // --- Normalize giá trị số ---
                if (type == typeof(int) || type == typeof(long) || type == typeof(short) ||
                    type == typeof(float) || type == typeof(double) || type == typeof(decimal) ||
                    (Nullable.GetUnderlyingType(type) != null &&
                     (Nullable.GetUnderlyingType(type) == typeof(int) ||
                      Nullable.GetUnderlyingType(type) == typeof(long) ||
                      Nullable.GetUnderlyingType(type) == typeof(short) ||
                      Nullable.GetUnderlyingType(type) == typeof(float) ||
                      Nullable.GetUnderlyingType(type) == typeof(double) ||
                      Nullable.GetUnderlyingType(type) == typeof(decimal))))
                {
                    value = NormalizeNumberString(value);
                }

                var converter = System.ComponentModel.TypeDescriptor.GetConverter(type);
                return converter.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, value);
            }
            catch
            {
                return null;
            }
        }
        public static bool ValidateOcrValue(OcrValue ocrValue)
        {
            // --- 1. Kiểm tra ép kiểu chính ---
            var extractionKey = ocrValue.ExtractionKey;
            var castedValue = CastValue(ocrValue.Value, extractionKey.DataType.Name);
            if (castedValue == null) return false;

            if (!ValidationCheckExpression(ocrValue, ocrValue.OcrDocument.OcrValueList.ToList()))
                return false;

            return true;
        }






        #endregion SourceCode3882ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.OcrValue ocrvalue)
        //{
        //    var result = "";
            //if (this.Name != null)
            //{
            //    if (!string.IsNullOrEmpty(result))
            //        result += "\r\n";
            //    result += "Tiêu đề:" + Name;
            //}            
        //    return result;
        //}
		
		//Tooltip for Object
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.OcrValue ocrvalue)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ValueToolTipControllerText(View view, Module.BusinessObjects.OcrValue ocrvalue)
        //{
        //    if (Value != null) 
		//			return Value;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ExtractionKeyToolTipControllerText(View view, Module.BusinessObjects.OcrValue ocrvalue)
        //{
        //    if (ExtractionKey != null) 
		//			return ExtractionKey;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object XToolTipControllerText(View view, Module.BusinessObjects.OcrValue ocrvalue)
        //{
        //    if (X != null) 
		//			return X;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object YToolTipControllerText(View view, Module.BusinessObjects.OcrValue ocrvalue)
        //{
        //    if (Y != null) 
		//			return Y;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WidthToolTipControllerText(View view, Module.BusinessObjects.OcrValue ocrvalue)
        //{
        //    if (Width != null) 
		//			return Width;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object HeightToolTipControllerText(View view, Module.BusinessObjects.OcrValue ocrvalue)
        //{
        //    if (Height != null) 
		//			return Height;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ConfidenceToolTipControllerText(View view, Module.BusinessObjects.OcrValue ocrvalue)
        //{
        //    if (Confidence != null) 
		//			return Confidence;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OcrPageToolTipControllerText(View view, Module.BusinessObjects.OcrValue ocrvalue)
        //{
        //    if (OcrPage != null) 
		//			return OcrPage;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OcrDocumentToolTipControllerText(View view, Module.BusinessObjects.OcrValue ocrvalue)
        //{
        //    if (OcrDocument != null) 
		//			return OcrDocument;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object InvalidToolTipControllerText(View view, Module.BusinessObjects.OcrValue ocrvalue)
        //{
        //    if (Invalid != null) 
		//			return Invalid;
        //    return null;
        //}
    

	    #endregion
  

    }
}
