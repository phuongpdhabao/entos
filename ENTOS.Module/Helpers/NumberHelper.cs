using ENTOS.Module.SystemObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTOS.Module.Helpers
{
    public static class NumberHelper
    {


        /// <summary>
        /// Trích xuất số từ chuỗi văn bản.
        /// Loại bỏ tất cả ký tự không phải số và trả về số đầu tiên tìm được.
        /// </summary>
        /// <param name="text">Chuỗi văn bản cần trích xuất số</param>
        /// <returns>Số được trích xuất hoặc null nếu không tìm thấy</returns>
        /// <example>
        /// var number1 = Tools.GetNumberInText("abc123def"); // 123
        /// var number2 = Tools.GetNumberInText("Price: $1,234.56"); // 1234
        /// var number3 = Tools.GetNumberInText("No number here"); // null
        /// </example>
        public static int? GetNumberInText(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (Char.IsDigit(c))
                    sb.Append(c);
            }
            var resultString = sb.ToString();
            if (!string.IsNullOrEmpty(resultString))
                return Int32.Parse(resultString);
            return null;
        }

        public static bool IsNumber(Type type)
        {
            if (type == null) return false;
            if (type.IsEnum) return false;
            // from http://stackoverflow.com/a/5182747/172132
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumber(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }



        private static string ThreeNumber2Letter(string _number)
        {
            int _number1 = 0;
            int _number2 = 0;
            int _number3 = 0;
            if (_number.Length == 3)
            {
                _number1 = int.Parse(_number.Substring(0, 1));
                _number2 = int.Parse(_number.Substring(1, 1));
                _number3 = int.Parse(_number.Substring(2, 1));
            }
            else if (_number.Length == 2)
            {
                _number2 = int.Parse(_number.Substring(0, 1));
                _number3 = int.Parse(_number.Substring(1, 1));
            }
            else if (_number.Length == 1)
                _number3 = int.Parse(_number.Substring(0, 1));

            if (_number1 == 0 && _number2 == 0 && _number3 == 0)
                return "";
            switch (_number2)
            {
                case 0:
                    if (_number3 == 0)
                        return string.Format("{0} trăm", (object)OneNumber2Letter(_number1));
                    return string.Format("{0} trăm lẻ {1}", (object)OneNumber2Letter(_number1),
                        (object)OneNumber2Letter(_number3));
                case 1:
                    if (_number3 == 0)
                        return string.Format("{0} trăm mười", (object)OneNumber2Letter(_number1));
                    return string.Format("{0} trăm mười {1}", (object)OneNumber2Letter(_number1),
                        (object)OneNumber2Letter(_number3));
                default:
                    switch (_number3)
                    {
                        case 0:
                            return string.Format("{0} trăm {1} mươi", (object)OneNumber2Letter(_number1),
                                (object)OneNumber2Letter(_number2));
                        case 1:
                            return string.Format("{0} trăm {1} mươi mốt", (object)OneNumber2Letter(_number1),
                                (object)OneNumber2Letter(_number2));
                        case 4:
                            return string.Format("{0} trăm {1} mươi tư", (object)OneNumber2Letter(_number1),
                                (object)OneNumber2Letter(_number2));
                        default:
                            return string.Format("{0} trăm {1} mươi {2}", (object)OneNumber2Letter(_number1),
                                (object)OneNumber2Letter(_number2), (object)OneNumber2Letter(_number3));
                    }
            }
        }

        /// <summary>
        /// Lấy đơn vị tiếng Việt cho số
        /// </summary>
        /// <param name="_unit">Đơn vị cần chuyển đổi</param>
        /// <returns>Chuỗi đơn vị tiếng Việt</returns>
        private static string NumUnit(int _unit)
        {
            switch (_unit)
            {
                case 0:
                case 1:
                    return "";
                case 2:
                    return "nghìn";
                case 3:
                    return "triệu";
                case 4:
                    return "tỷ";
                default:
                    return string.Format("{0} {1}", (object)NumUnit(_unit - 3), (object)NumUnit(4));
            }
        }

        /// <summary>
        /// Chuyển đổi một chữ số thành chữ tiếng Việt
        /// </summary>
        /// <param name="_number">Chữ số cần chuyển đổi</param>
        /// <returns>Chuỗi chữ tiếng Việt</returns>
        private static string OneNumber2Letter(int _number)
        {
            switch (_number)
            {
                case 0:
                    return "không";
                case 1:
                    return "một";
                case 2:
                    return "hai";
                case 3:
                    return "ba";
                case 4:
                    return "bốn";
                case 5:
                    return "năm";
                case 6:
                    return "sáu";
                case 7:
                    return "bảy";
                case 8:
                    return "tám";
                case 9:
                    return "chín";
                default:
                    return "";
            }
        }

        public static bool IsNumber(string text, char[] unitCharacter)
        {
            if (string.IsNullOrEmpty(text)) return false;
            foreach (var c in text)
            {
                if (!char.IsDigit(c) && !c.Equals('.') && !c.Equals(',') && !unitCharacter.Contains(c))
                    return false;
            }
            return true;
        }

    }
}
