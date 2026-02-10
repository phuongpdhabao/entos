using System;
using System.Globalization;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;

namespace ENTOS.Module.Extensions
{
    /// <summary>
    /// Extension cho kiểu số: int, double, decimal, long, float, short, ushort, uint, ulong, byte, sbyte, BigInteger và nullable versions.
    /// Cung cấp các hàm kiểm tra, định dạng, toán học, thống kê và chuyển đổi.
    /// </summary>
    public static class NumberExtensions
    {
        #region Checks & Comparisons

        /// <summary>
        /// Kiểm tra số chẵn.
        /// </summary>
        public static bool IsEven(this int number) => number % 2 == 0;
        public static bool IsEven(this long number) => number % 2 == 0;
        public static bool IsEven(this short number) => number % 2 == 0;
        public static bool IsEven(this ushort number) => number % 2 == 0;
        public static bool IsEven(this uint number) => number % 2 == 0;
        public static bool IsEven(this ulong number) => number % 2 == 0;
        public static bool IsEven(this byte number) => number % 2 == 0;
        public static bool IsEven(this sbyte number) => number % 2 == 0;
        public static bool IsEven(this BigInteger number) => number % 2 == 0;
        public static bool? IsEven(this int? number) => number?.IsEven();
        public static bool? IsEven(this long? number) => number?.IsEven();
        public static bool? IsEven(this short? number) => number?.IsEven();
        public static bool? IsEven(this ushort? number) => number?.IsEven();
        public static bool? IsEven(this uint? number) => number?.IsEven();
        public static bool? IsEven(this ulong? number) => number?.IsEven();
        public static bool? IsEven(this byte? number) => number?.IsEven();
        public static bool? IsEven(this sbyte? number) => number?.IsEven();

        /// <summary>
        /// Kiểm tra số lẻ.
        /// </summary>
        public static bool IsOdd(this int number) => number % 2 != 0;
        public static bool IsOdd(this long number) => number % 2 != 0;
        public static bool IsOdd(this short number) => number % 2 != 0;
        public static bool IsOdd(this ushort number) => number % 2 != 0;
        public static bool IsOdd(this uint number) => number % 2 != 0;
        public static bool IsOdd(this ulong number) => number % 2 != 0;
        public static bool IsOdd(this byte number) => number % 2 != 0;
        public static bool IsOdd(this sbyte number) => number % 2 != 0;
        public static bool IsOdd(this BigInteger number) => number % 2 != 0;
        public static bool? IsOdd(this int? number) => number?.IsOdd();
        public static bool? IsOdd(this long? number) => number?.IsOdd();
        public static bool? IsOdd(this short? number) => number?.IsOdd();
        public static bool? IsOdd(this ushort? number) => number?.IsOdd();
        public static bool? IsOdd(this uint? number) => number?.IsOdd();
        public static bool? IsOdd(this ulong? number) => number?.IsOdd();
        public static bool? IsOdd(this byte? number) => number?.IsOdd();
        public static bool? IsOdd(this sbyte? number) => number?.IsOdd();

        /// <summary>
        /// Kiểm tra số có dương hay không ( > 0 ).
        /// </summary>
        public static bool IsPositive(this int number) => number > 0;
        public static bool IsPositive(this long number) => number > 0;
        public static bool IsPositive(this short number) => number > 0;
        public static bool IsPositive(this ushort number) => number > 0;
        public static bool IsPositive(this uint number) => number > 0;
        public static bool IsPositive(this ulong number) => number > 0;
        public static bool IsPositive(this byte number) => number > 0;
        public static bool IsPositive(this sbyte number) => number > 0;
        public static bool IsPositive(this float number) => number > 0;
        public static bool IsPositive(this double number) => number > 0;
        public static bool IsPositive(this decimal number) => number > 0;
        public static bool IsPositive(this BigInteger number) => number > 0;
        public static bool? IsPositive(this int? number) => number?.IsPositive();
        public static bool? IsPositive(this long? number) => number?.IsPositive();
        public static bool? IsPositive(this short? number) => number?.IsPositive();
        public static bool? IsPositive(this ushort? number) => number?.IsPositive();
        public static bool? IsPositive(this uint? number) => number?.IsPositive();
        public static bool? IsPositive(this ulong? number) => number?.IsPositive();
        public static bool? IsPositive(this byte? number) => number?.IsPositive();
        public static bool? IsPositive(this sbyte? number) => number?.IsPositive();
        public static bool? IsPositive(this float? number) => number?.IsPositive();
        public static bool? IsPositive(this double? number) => number?.IsPositive();
        public static bool? IsPositive(this decimal? number) => number?.IsPositive();

        /// <summary>
        /// Kiểm tra số có âm hay không ( < 0 ).
        /// </summary>
        public static bool IsNegative(this int number) => number < 0;
        public static bool IsNegative(this long number) => number < 0;
        public static bool IsNegative(this short number) => number < 0;
        public static bool IsNegative(this sbyte number) => number < 0;
        public static bool IsNegative(this float number) => number < 0;
        public static bool IsNegative(this double number) => number < 0;
        public static bool IsNegative(this decimal number) => number < 0;
        public static bool IsNegative(this BigInteger number) => number < 0;
        public static bool? IsNegative(this int? number) => number?.IsNegative();
        public static bool? IsNegative(this long? number) => number?.IsNegative();
        public static bool? IsNegative(this short? number) => number?.IsNegative();
        public static bool? IsNegative(this sbyte? number) => number?.IsNegative();
        public static bool? IsNegative(this float? number) => number?.IsNegative();
        public static bool? IsNegative(this double? number) => number?.IsNegative();
        public static bool? IsNegative(this decimal? number) => number?.IsNegative();

        /// <summary>
        /// Kiểm tra số có bằng 0 hay không.
        /// </summary>
        public static bool IsZero(this int number) => number == 0;
        public static bool IsZero(this long number) => number == 0;
        public static bool IsZero(this short number) => number == 0;
        public static bool IsZero(this ushort number) => number == 0;
        public static bool IsZero(this uint number) => number == 0;
        public static bool IsZero(this ulong number) => number == 0;
        public static bool IsZero(this byte number) => number == 0;
        public static bool IsZero(this sbyte number) => number == 0;
        public static bool IsZero(this float number) => number == 0;
        public static bool IsZero(this double number) => number == 0;
        public static bool IsZero(this decimal number) => number == 0;
        public static bool IsZero(this BigInteger number) => number == 0;
        public static bool? IsZero(this int? number) => number?.IsZero();
        public static bool? IsZero(this long? number) => number?.IsZero();
        public static bool? IsZero(this short? number) => number?.IsZero();
        public static bool? IsZero(this ushort? number) => number?.IsZero();
        public static bool? IsZero(this uint? number) => number?.IsZero();
        public static bool? IsZero(this ulong? number) => number?.IsZero();
        public static bool? IsZero(this byte? number) => number?.IsZero();
        public static bool? IsZero(this sbyte? number) => number?.IsZero();
        public static bool? IsZero(this float? number) => number?.IsZero();
        public static bool? IsZero(this double? number) => number?.IsZero();
        public static bool? IsZero(this decimal? number) => number?.IsZero();

        /// <summary>
        /// Kiểm tra số nằm trong khoảng [min, max].
        /// </summary>
        public static bool IsBetween(this int number, int min, int max) => number >= min && number <= max;
        public static bool IsBetween(this long number, long min, long max) => number >= min && number <= max;
        public static bool IsBetween(this short number, short min, short max) => number >= min && number <= max;
        public static bool IsBetween(this ushort number, ushort min, ushort max) => number >= min && number <= max;
        public static bool IsBetween(this uint number, uint min, uint max) => number >= min && number <= max;
        public static bool IsBetween(this ulong number, ulong min, ulong max) => number >= min && number <= max;
        public static bool IsBetween(this byte number, byte min, byte max) => number >= min && number <= max;
        public static bool IsBetween(this sbyte number, sbyte min, sbyte max) => number >= min && number <= max;
        public static bool IsBetween(this double number, double min, double max) => number >= min && number <= max;
        public static bool IsBetween(this decimal number, decimal min, decimal max) => number >= min && number <= max;
        public static bool IsBetween(this float number, float min, float max) => number >= min && number <= max;
        public static bool IsBetween(this BigInteger number, BigInteger min, BigInteger max) => number >= min && number <= max;
        public static bool? IsBetween(this int? number, int min, int max) => number?.IsBetween(min, max);
        public static bool? IsBetween(this long? number, long min, long max) => number?.IsBetween(min, max);
        public static bool? IsBetween(this short? number, short min, short max) => number?.IsBetween(min, max);
        public static bool? IsBetween(this ushort? number, ushort min, ushort max) => number?.IsBetween(min, max);
        public static bool? IsBetween(this uint? number, uint min, uint max) => number?.IsBetween(min, max);
        public static bool? IsBetween(this ulong? number, ulong min, ulong max) => number?.IsBetween(min, max);
        public static bool? IsBetween(this byte? number, byte min, byte max) => number?.IsBetween(min, max);
        public static bool? IsBetween(this sbyte? number, sbyte min, sbyte max) => number?.IsBetween(min, max);
        public static bool? IsBetween(this double? number, double min, double max) => number?.IsBetween(min, max);
        public static bool? IsBetween(this decimal? number, decimal min, decimal max) => number?.IsBetween(min, max);
        public static bool? IsBetween(this float? number, float min, float max) => number?.IsBetween(min, max);

        /// <summary>
        /// So sánh hai số thực có gần bằng nhau với một sai số cho trước.
        /// </summary>
        public static bool IsApproximately(this double number, double target, double tolerance = 0.000001)
        {
            return Math.Abs(number - target) < tolerance;
        }

        /// <summary>
        /// So sánh hai số thực có gần bằng nhau với một sai số cho trước.
        /// </summary>
        public static bool IsApproximately(this decimal number, decimal target, decimal tolerance = 0.000001m)
        {
            return Math.Abs(number - target) < tolerance;
        }

        public static bool? IsApproximately(this double? number, double target, double tolerance = 0.000001)
        {
            return number?.IsApproximately(target, tolerance);
        }

        public static bool? IsApproximately(this decimal? number, decimal target, decimal tolerance = 0.000001m)
        {
            return number?.IsApproximately(target, tolerance);
        }

        /// <summary>
        /// Kiểm tra số nguyên tố.
        /// </summary>
        public static bool IsPrime(this int number)
        {
            if (number < 2) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
                if (number % i == 0) return false;
            return true;
        }

        public static bool? IsPrime(this int? number) => number?.IsPrime();
        
        #endregion

        #region Advanced Mathematics

        /// <summary>
        /// Tính lũy thừa.
        /// </summary>
        public static double Power(this double number, double exponent) => Math.Pow(number, exponent);
        public static double Power(this int number, double exponent) => Math.Pow(number, exponent);
        public static double Power(this long number, double exponent) => Math.Pow(number, exponent);
        public static double Power(this float number, double exponent) => Math.Pow(number, exponent);
        public static decimal Power(this decimal number, int exponent)
        {
            if (exponent == 0) return 1;
            if (exponent < 0) return 1 / Power(number, -exponent);
            decimal result = 1;
            for (int i = 0; i < exponent; i++)
                result *= number;
            return result;
        }
        public static BigInteger Power(this BigInteger number, int exponent) => BigInteger.Pow(number, exponent);

        /// <summary>
        /// Tính căn bậc hai.
        /// </summary>
        public static double Sqrt(this double number) => Math.Sqrt(number);
        public static double Sqrt(this int number) => Math.Sqrt(number);
        public static double Sqrt(this long number) => Math.Sqrt(number);
        public static double Sqrt(this float number) => Math.Sqrt(number);
        public static double Sqrt(this decimal number) => Math.Sqrt((double)number);

        /// <summary>
        /// Tính căn bậc ba.
        /// </summary>
        public static double Cbrt(this double number) => Math.Cbrt(number);
        public static double Cbrt(this int number) => Math.Cbrt(number);
        public static double Cbrt(this long number) => Math.Cbrt(number);
        public static double Cbrt(this float number) => Math.Cbrt(number);
        public static double Cbrt(this decimal number) => Math.Cbrt((double)number);

        /// <summary>
        /// Tính logarit tự nhiên.
        /// </summary>
        public static double Ln(this double number) => Math.Log(number);
        public static double Ln(this int number) => Math.Log(number);
        public static double Ln(this long number) => Math.Log(number);
        public static double Ln(this float number) => Math.Log(number);
        public static double Ln(this decimal number) => Math.Log((double)number);

        /// <summary>
        /// Tính logarit cơ số 10.
        /// </summary>
        public static double Log10(this double number) => Math.Log10(number);
        public static double Log10(this int number) => Math.Log10(number);
        public static double Log10(this long number) => Math.Log10(number);
        public static double Log10(this float number) => Math.Log10(number);
        public static double Log10(this decimal number) => Math.Log10((double)number);

        /// <summary>
        /// Tính logarit với cơ số tùy chọn.
        /// </summary>
        public static double Log(this double number, double baseValue) => Math.Log(number, baseValue);
        public static double Log(this int number, double baseValue) => Math.Log(number, baseValue);
        public static double Log(this long number, double baseValue) => Math.Log(number, baseValue);
        public static double Log(this float number, double baseValue) => Math.Log(number, baseValue);
        public static double Log(this decimal number, double baseValue) => Math.Log((double)number, baseValue);

        /// <summary>
        /// Tính giá trị tuyệt đối.
        /// </summary>
        public static int Abs(this int number) => Math.Abs(number);
        public static long Abs(this long number) => Math.Abs(number);
        public static short Abs(this short number) => Math.Abs(number);
        public static sbyte Abs(this sbyte number) => Math.Abs(number);
        public static float Abs(this float number) => Math.Abs(number);
        public static double Abs(this double number) => Math.Abs(number);
        public static decimal Abs(this decimal number) => Math.Abs(number);
        public static BigInteger Abs(this BigInteger number) => BigInteger.Abs(number);

        /// <summary>
        /// Tính phần nguyên.
        /// </summary>
        public static double Floor(this double number) => Math.Floor(number);
        public static double Floor(this float number) => Math.Floor(number);
        public static decimal Floor(this decimal number) => Math.Floor(number);

        /// <summary>
        /// Tính phần trần.
        /// </summary>
        public static double Ceiling(this double number) => Math.Ceiling(number);
        public static double Ceiling(this float number) => Math.Ceiling(number);
        public static decimal Ceiling(this decimal number) => Math.Ceiling(number);

        /// <summary>
        /// Tính phần thập phân.
        /// </summary>
        public static double Fractional(this double number) => number - Math.Floor(number);
        public static float Fractional(this float number) => number - (float)Math.Floor(number);
        public static decimal Fractional(this decimal number) => number - Math.Floor(number);

        #endregion

        #region Formatting

        /// <summary>
        /// Định dạng số thành chuỗi tiền tệ.
        /// </summary>
        public static string ToCurrencyString(this decimal number, string culture = "vi-VN", int fractionDigits = 0)
        {
            var format = (NumberFormatInfo)new CultureInfo(culture).NumberFormat.Clone();
            format.CurrencyDecimalDigits = fractionDigits;
            return number.ToString("C", format);
        }

        /// <summary>
        /// Định dạng số thành chuỗi tiền tệ.
        /// </summary>
        public static string ToCurrencyString(this double number, string culture = "vi-VN", int fractionDigits = 0)
        {
            return ((decimal)number).ToCurrencyString(culture, fractionDigits);
        }

        public static string ToCurrencyString(this decimal? number, string culture = "vi-VN", int fractionDigits = 0)
        {
            return number?.ToCurrencyString(culture, fractionDigits) ?? string.Empty;
        }

        public static string ToCurrencyString(this double? number, string culture = "vi-VN", int fractionDigits = 0)
        {
            return number?.ToCurrencyString(culture, fractionDigits) ?? string.Empty;
        }

        /// <summary>
        /// Định dạng số thành phần trăm.
        /// </summary>
        public static string ToPercentage(this double number, int digits = 2)
        {
            return number.ToString($"P{digits}");
        }

        public static string ToPercentage(this double? number, int digits = 2)
        {
            return number?.ToPercentage(digits) ?? string.Empty;
        }

        /// <summary>
        /// Định dạng số lớn thành dạng rút gọn (1500 -> "1.5K", 1000000 -> "1M").
        /// </summary>
        public static string ToMetric(this double number, int precision = 1)
        {
            if (number < 1000) return number.ToString();
            if (number < 1_000_000) return (number / 1000).ToString($"F{precision}") + "K";
            if (number < 1_000_000_000) return (number / 1_000_000).ToString($"F{precision}") + "M";
            return (number / 1_000_000_000).ToString($"F{precision}") + "B";
        }

        public static string ToMetric(this double? number, int precision = 1)
        {
            return number?.ToMetric(precision) ?? string.Empty;
        }

        /// <summary>
        /// Chuyển đổi số byte thành chuỗi định dạng (KB, MB, GB, TB).
        /// </summary>
        public static string ToFileSizeString(this long bytes)
        {
            if (bytes < 0) throw new ArgumentOutOfRangeException(nameof(bytes));
            if (bytes == 0) return "0 Bytes";

            int mag = (int)Math.Log(bytes, 1024);
            decimal adjustedSize = (decimal)bytes / (1L << (mag * 10));

            return $"{adjustedSize:n2} {new[] { "Bytes", "KB", "MB", "GB", "TB" }[mag]}";
        }

        public static string ToFileSizeString(this long? bytes)
        {
            return bytes?.ToFileSizeString() ?? "0 Bytes";
        }

        /// <summary>
        /// Chuyển số thành số La Mã.
        /// </summary>
        public static string ToRomanNumeral(this int number)
        {
            if (number < 1 || number > 3999)
                throw new ArgumentOutOfRangeException(nameof(number), "Số La Mã chỉ hỗ trợ từ 1 đến 3999.");

            var romanNumerals = new[]
            {
                new { Value = 1000, Numeral = "M" },
                new { Value = 900, Numeral = "CM" },
                new { Value = 500, Numeral = "D" },
                new { Value = 400, Numeral = "CD" },
                new { Value = 100, Numeral = "C" },
                new { Value = 90, Numeral = "XC" },
                new { Value = 50, Numeral = "L" },
                new { Value = 40, Numeral = "XL" },
                new { Value = 10, Numeral = "X" },
                new { Value = 9, Numeral = "IX" },
                new { Value = 5, Numeral = "V" },
                new { Value = 4, Numeral = "IV" },
                new { Value = 1, Numeral = "I" }
            };

            var result = "";
            var remaining = number;

            foreach (var numeral in romanNumerals)
            {
                while (remaining >= numeral.Value)
                {
                    result += numeral.Numeral;
                    remaining -= numeral.Value;
                }
            }

            return result;
        }

        public static string ToRomanNumeral(this int? number)
        {
            return number?.ToRomanNumeral() ?? string.Empty;
        }

        /// <summary>
        /// Định dạng thời gian thành chuỗi dễ đọc.
        /// </summary>
        public static string ToDurationString(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalDays >= 1)
                return $"{(int)timeSpan.TotalDays} ngày {timeSpan.Hours} giờ {timeSpan.Minutes} phút";
            if (timeSpan.TotalHours >= 1)
                return $"{(int)timeSpan.TotalHours} giờ {timeSpan.Minutes} phút";
            if (timeSpan.TotalMinutes >= 1)
                return $"{(int)timeSpan.TotalMinutes} phút {timeSpan.Seconds} giây";
            return $"{timeSpan.TotalSeconds:F1} giây";
        }

        public static string ToDurationString(this TimeSpan? timeSpan)
        {
            return timeSpan?.ToDurationString() ?? string.Empty;
        }

        /// <summary>
        /// Chuyển số thành chữ tiếng Việt.
        /// </summary>
        public static string ToWordsVN(this int number)
        {
            if (number == 0) return "không";
            if (number < 0) return "âm " + ToWordsVN(Math.Abs(number));

            var words = new List<string>();
            var groups = new[] { "", "nghìn", "triệu", "tỷ", "nghìn tỷ" };
            var groupIndex = 0;

            while (number > 0)
            {
                var group = number % 1000;
                if (group != 0)
                {
                    var groupWords = ConvertGroupToWordsVN(group);
                    if (groupIndex > 0)
                    {
                        groupWords += " " + groups[groupIndex];
                    }
                    words.Insert(0, groupWords);
                }
                number /= 1000;
                groupIndex++;
            }

            return string.Join(" ", words).Trim();
        }

        /// <summary>
        /// Chuyển số thành chữ tiếng Việt (long).
        /// </summary>
        public static string ToWordsVN(this long number)
        {
            if (number == 0) return "không";
            if (number < 0) return "âm " + ToWordsVN(Math.Abs(number));

            var words = new List<string>();
            var groups = new[] { "", "nghìn", "triệu", "tỷ", "nghìn tỷ", "triệu tỷ" };
            var groupIndex = 0;

            while (number > 0)
            {
                var group = (int)(number % 1000);
                if (group != 0)
                {
                    var groupWords = ConvertGroupToWordsVN(group);
                    if (groupIndex > 0)
                    {
                        groupWords += " " + groups[groupIndex];
                    }
                    words.Insert(0, groupWords);
                }
                number /= 1000;
                groupIndex++;
            }

            return string.Join(" ", words).Trim();
        }

        /// <summary>
        /// Chuyển số thành chữ tiếng Anh.
        /// </summary>
        public static string ToWordsEN(this int number)
        {
            if (number == 0) return "zero";
            if (number < 0) return "negative " + ToWordsEN(Math.Abs(number));

            var words = new List<string>();
            var groups = new[] { "", "thousand", "million", "billion", "trillion" };
            var groupIndex = 0;

            while (number > 0)
            {
                var group = number % 1000;
                if (group != 0)
                {
                    var groupWords = ConvertGroupToWordsEN(group);
                    if (groupIndex > 0)
                    {
                        groupWords += " " + groups[groupIndex];
                    }
                    words.Insert(0, groupWords);
                }
                number /= 1000;
                groupIndex++;
            }

            return string.Join(" ", words).Trim();
        }

        /// <summary>
        /// Chuyển số thành chữ tiếng Anh (long).
        /// </summary>
        public static string ToWordsEN(this long number)
        {
            if (number == 0) return "zero";
            if (number < 0) return "negative " + ToWordsEN(Math.Abs(number));

            var words = new List<string>();
            var groups = new[] { "", "thousand", "million", "billion", "trillion", "quadrillion" };
            var groupIndex = 0;

            while (number > 0)
            {
                var group = (int)(number % 1000);
                if (group != 0)
                {
                    var groupWords = ConvertGroupToWordsEN(group);
                    if (groupIndex > 0)
                    {
                        groupWords += " " + groups[groupIndex];
                    }
                    words.Insert(0, groupWords);
                }
                number /= 1000;
                groupIndex++;
            }

            return string.Join(" ", words).Trim();
        }

        /// <summary>
        /// Chuyển số thành chữ tiếng Anh (decimal).
        /// </summary>
        public static string ToWordsEN(this decimal number, int decimalPlaces = 2)
        {
            var integerPart = (long)Math.Floor(number);
            var decimalPart = number - integerPart;

            var result = integerPart.ToWordsEN();

            if (decimalPart > 0)
            {
                // Làm tròn phần thập phân để tránh lỗi floating point
                var roundedDecimalPart = Math.Round(decimalPart, decimalPlaces);
                
                if (roundedDecimalPart > 0)
                {
                    // Chuyển thành chuỗi và loại bỏ số 0 thừa ở cuối
                    var decimalString = roundedDecimalPart.ToString($"F{decimalPlaces}");
                    var parts = decimalString.Split('.');
                    
                    if (parts.Length > 1)
                    {
                        var decimalDigits = parts[1].TrimEnd('0'); // Loại bỏ số 0 thừa ở cuối
                        
                        if (!string.IsNullOrEmpty(decimalDigits))
                        {
                            result += " point " + ConvertDecimalToWordsEN(decimalDigits);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Chuyển số thành chữ tiếng Việt (decimal).
        /// </summary>
        public static string ToWordsVN(this decimal number, int decimalPlaces = 2)
        {
            var integerPart = (long)Math.Floor(number);
            var decimalPart = number - integerPart;

            var result = integerPart.ToWordsVN();

            if (decimalPart > 0)
            {
                // Làm tròn phần thập phân để tránh lỗi floating point
                var roundedDecimalPart = Math.Round(decimalPart, decimalPlaces);
                
                if (roundedDecimalPart > 0)
                {
                    // Chuyển thành chuỗi và loại bỏ số 0 thừa ở cuối
                    var decimalString = roundedDecimalPart.ToString($"F{decimalPlaces}");
                    var parts = decimalString.Split('.');
                    
                    if (parts.Length > 1)
                    {
                        var decimalDigits = parts[1].TrimEnd('0'); // Loại bỏ số 0 thừa ở cuối
                        
                        if (!string.IsNullOrEmpty(decimalDigits))
                        {
                            var decimalNumber = long.Parse(decimalDigits);
                            if (decimalNumber > 0)
                            {
                                result += " phẩy " + decimalNumber.ToWordsVN();
                            }
                        }
                    }
                }
            }

            return result;
        }

        // Helper methods for Vietnamese
        private static string ConvertGroupToWordsVN(int group)
        {
            if (group == 0) return "";

            var words = new List<string>();
            var hundreds = group / 100;
            var tens = (group % 100) / 10;
            var ones = group % 10;

            // Hundreds
            if (hundreds > 0)
            {
                if (hundreds == 1)
                    words.Add("một trăm");
                else
                    words.Add(OnesToWordsVN(hundreds) + " trăm");
            }

            // Tens and ones
            if (tens > 0 || ones > 0)
            {
                if (hundreds > 0 && (tens == 0 || tens == 1))
                    words.Add("linh");

                if (tens == 1)
                {
                    if (ones == 0)
                        words.Add("mười");
                    else if (ones == 1)
                        words.Add("mười một");
                    else if (ones == 5)
                        words.Add("mười lăm");
                    else
                        words.Add("mười " + OnesToWordsVN(ones));
                }
                else if (tens > 1)
                {
                    words.Add(TensToWordsVN(tens));
                    if (ones == 1)
                        words.Add("mốt");
                    else if (ones == 5)
                        words.Add("lăm");
                    else if (ones > 0)
                        words.Add(OnesToWordsVN(ones));
                }
                else if (ones > 0)
                {
                    words.Add(OnesToWordsVN(ones));
                }
            }

            return string.Join(" ", words);
        }

        private static string OnesToWordsVN(int digit)
        {
            return digit switch
            {
                0 => "không",
                1 => "một",
                2 => "hai",
                3 => "ba",
                4 => "bốn",
                5 => "năm",
                6 => "sáu",
                7 => "bảy",
                8 => "tám",
                9 => "chín",
                _ => ""
            };
        }

        private static string TensToWordsVN(int tens)
        {
            return tens switch
            {
                2 => "hai mươi",
                3 => "ba mươi",
                4 => "bốn mươi",
                5 => "năm mươi",
                6 => "sáu mươi",
                7 => "bảy mươi",
                8 => "tám mươi",
                9 => "chín mươi",
                _ => ""
            };
        }

        // Helper methods for English
        private static string ConvertGroupToWordsEN(int group)
        {
            if (group == 0) return "";

            var words = new List<string>();
            var hundreds = group / 100;
            var tens = (group % 100) / 10;
            var ones = group % 10;

            // Hundreds
            if (hundreds > 0)
            {
                words.Add(OnesToWordsEN(hundreds) + " hundred");
            }

            // Tens and ones
            if (tens > 0 || ones > 0)
            {
                if (tens == 1)
                {
                    words.Add(TeensToWordsEN(group % 100));
                }
                else
                {
                    if (tens > 1)
                    {
                        words.Add(TensToWordsEN(tens));
                    }
                    if (ones > 0)
                    {
                        words.Add(OnesToWordsEN(ones));
                    }
                }
            }

            return string.Join(" ", words);
        }

        private static string OnesToWordsEN(int digit)
        {
            return digit switch
            {
                0 => "zero",
                1 => "one",
                2 => "two",
                3 => "three",
                4 => "four",
                5 => "five",
                6 => "six",
                7 => "seven",
                8 => "eight",
                9 => "nine",
                _ => ""
            };
        }

        private static string TeensToWordsEN(int number)
        {
            return number switch
            {
                10 => "ten",
                11 => "eleven",
                12 => "twelve",
                13 => "thirteen",
                14 => "fourteen",
                15 => "fifteen",
                16 => "sixteen",
                17 => "seventeen",
                18 => "eighteen",
                19 => "nineteen",
                _ => ""
            };
        }

        private static string TensToWordsEN(int tens)
        {
            return tens switch
            {
                2 => "twenty",
                3 => "thirty",
                4 => "forty",
                5 => "fifty",
                6 => "sixty",
                7 => "seventy",
                8 => "eighty",
                9 => "ninety",
                _ => ""
            };
        }

        private static string ConvertDecimalToWordsEN(string decimalDigits)
        {
            var words = new List<string>();
            foreach (var digit in decimalDigits)
            {
                if (digit == '0')
                    words.Add("zero");
                else
                    words.Add(OnesToWordsEN(int.Parse(digit.ToString())));
            }
            return string.Join(" ", words);
        }

        // Nullable versions
        public static string ToWordsVN(this int? number)
        {
            return number?.ToWordsVN() ?? string.Empty;
        }

        public static string ToWordsVN(this decimal? number, int decimalPlaces = 2)
        {
            return number?.ToWordsVN(decimalPlaces) ?? string.Empty;
        }

        public static string ToWordsEN(this int? number)
        {
            return number?.ToWordsEN() ?? string.Empty;
        }

        public static string ToWordsEN(this decimal? number, int decimalPlaces = 2)
        {
            return number?.ToWordsEN(decimalPlaces) ?? string.Empty;
        }
        
        #endregion

        #region Mathematical Operations

        /// <summary>
        /// Làm tròn số đến n chữ số thập phân.
        /// </summary>
        public static double RoundTo(this double number, int digits) => Math.Round(number, digits);
        public static decimal RoundTo(this decimal number, int digits) => Math.Round(number, digits);
        public static double? RoundTo(this double? number, int digits) => number?.RoundTo(digits);
        public static decimal? RoundTo(this decimal? number, int digits) => number?.RoundTo(digits);

        /// <summary>
        /// Làm tròn số đến giá trị gần nhất (ví dụ: 10, 100, 1000).
        /// </summary>
        public static decimal RoundToNearest(this decimal number, int nearest)
        {
            if (nearest <= 0) return number;
            return Math.Round(number / nearest, MidpointRounding.AwayFromZero) * nearest;
        }

        public static decimal? RoundToNearest(this decimal? number, int nearest)
        {
            return number?.RoundToNearest(nearest);
        }

        /// <summary>
        /// Giới hạn một giá trị trong một khoảng [min, max].
        /// </summary>
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;
            return value;
        }

        public static T? Clamp<T>(this T? value, T min, T max) where T : struct, IComparable<T>
        {
            return value?.Clamp(min, max);
        }

        /// <summary>
        /// Tính giai thừa của một số.
        /// </summary>
        public static long Factorial(this int number)
        {
            if (number < 0) throw new ArgumentOutOfRangeException(nameof(number), "Giai thừa không xác định cho số âm.");
            if (number > 20) throw new ArgumentOutOfRangeException(nameof(number), "Giai thừa quá lớn để tính bằng kiểu long.");
            return number == 0 ? 1 : number * (number - 1).Factorial();
        }

        public static long? Factorial(this int? number)
        {
            return number?.Factorial();
        }

        /// <summary>
        /// Lấy phần trăm của một số.
        /// </summary>
        public static decimal PercentOf(this int number, int percent) => (decimal)number * percent / 100;
        public static decimal PercentOf(this long number, int percent) => (decimal)number * percent / 100;
        public static decimal PercentOf(this decimal number, int percent) => number * percent / 100;
        public static double PercentOf(this double number, int percent) => number * percent / 100;
        public static decimal? PercentOf(this int? number, int percent) => number?.PercentOf(percent);
        public static decimal? PercentOf(this long? number, int percent) => number?.PercentOf(percent);
        public static decimal? PercentOf(this decimal? number, int percent) => number?.PercentOf(percent);
        public static double? PercentOf(this double? number, int percent) => number?.PercentOf(percent);

        /// <summary>
        /// Tính tỷ lệ phần trăm giữa hai số.
        /// </summary>
        public static double PercentageOf(this double part, double total) => total == 0 ? 0 : (part / total) * 100;
        public static decimal PercentageOf(this decimal part, decimal total) => total == 0 ? 0 : (part / total) * 100;
        public static double? PercentageOf(this double? part, double total) => part?.PercentageOf(total);
        public static decimal? PercentageOf(this decimal? part, decimal total) => part?.PercentageOf(total);

        /// <summary>
        /// Tính tỷ lệ tăng trưởng.
        /// </summary>
        public static double GrowthRate(this double current, double previous) => previous == 0 ? 0 : ((current - previous) / previous) * 100;
        public static decimal GrowthRate(this decimal current, decimal previous) => previous == 0 ? 0 : ((current - previous) / previous) * 100;
        public static double? GrowthRate(this double? current, double previous) => current?.GrowthRate(previous);
        public static decimal? GrowthRate(this decimal? current, decimal previous) => current?.GrowthRate(previous);

        #endregion

        #region Financial Mathematics

        /// <summary>
        /// Tính lãi đơn.
        /// </summary>
        public static decimal SimpleInterest(this decimal principal, decimal rate, decimal time)
        {
            return principal * rate * time / 100;
        }

        public static double SimpleInterest(this double principal, double rate, double time)
        {
            return principal * rate * time / 100;
        }

        /// <summary>
        /// Tính lãi kép.
        /// </summary>
        public static decimal CompoundInterest(this decimal principal, decimal rate, decimal time, int compoundsPerYear = 1)
        {
            return principal * (decimal)Math.Pow((double)(1 + rate / (100 * compoundsPerYear)), (double)(compoundsPerYear * time)) - principal;
        }

        public static double CompoundInterest(this double principal, double rate, double time, int compoundsPerYear = 1)
        {
            return principal * Math.Pow(1 + rate / (100 * compoundsPerYear), compoundsPerYear * time) - principal;
        }

        /// <summary>
        /// Tính khấu hao đường thẳng.
        /// </summary>
        public static decimal StraightLineDepreciation(this decimal cost, decimal salvageValue, decimal usefulLife)
        {
            return (cost - salvageValue) / usefulLife;
        }

        public static double StraightLineDepreciation(this double cost, double salvageValue, double usefulLife)
        {
            return (cost - salvageValue) / usefulLife;
        }

        /// <summary>
        /// Tính tỷ lệ lợi nhuận gộp.
        /// </summary>
        public static double GrossProfitMargin(this decimal revenue, decimal costOfGoods)
        {
            return revenue == 0 ? 0 : (double)((revenue - costOfGoods) / revenue * 100);
        }

        public static double GrossProfitMargin(this double revenue, double costOfGoods)
        {
            return revenue == 0 ? 0 : ((revenue - costOfGoods) / revenue * 100);
        }

        #endregion

        #region Unit Conversions

        /// <summary>
        /// Chuyển đổi đơn vị trọng lượng.
        /// </summary>
        public static double ToKilograms(this double pounds) => pounds * 0.453592;
        public static double ToPounds(this double kilograms) => kilograms * 2.20462;
        public static double ToGrams(this double ounces) => ounces * 28.3495;
        public static double ToOunces(this double grams) => grams * 0.035274;

        /// <summary>
        /// Chuyển đổi đơn vị độ dài.
        /// </summary>
        public static double ToMeters(this double feet) => feet * 0.3048;
        public static double ToFeet(this double meters) => meters * 3.28084;
        public static double ToCentimeters(this double inches) => inches * 2.54;
        public static double ToInches(this double centimeters) => centimeters * 0.393701;

        /// <summary>
        /// Chuyển đổi đơn vị nhiệt độ.
        /// </summary>
        public static double ToCelsius(this double fahrenheit) => (fahrenheit - 32) * 5 / 9;
        public static double ToFahrenheit(this double celsius) => celsius * 9 / 5 + 32;
        public static double ToKelvin(this double celsius) => celsius + 273.15;
        public static double ToCelsiusFromKelvin(this double kelvin) => kelvin - 273.15;

        /// <summary>
        /// Chuyển đổi tiền tệ (tỷ giá cố định cho ví dụ).
        /// </summary>
        public static decimal ToUSD(this decimal vnd, decimal exchangeRate = 0.000041m) => vnd * exchangeRate;
        public static decimal ToVND(this decimal usd, decimal exchangeRate = 24350m) => usd * exchangeRate;
        public static decimal ToEUR(this decimal vnd, decimal exchangeRate = 0.000038m) => vnd * exchangeRate;
        public static decimal ToVNDFromEUR(this decimal eur, decimal exchangeRate = 26300m) => eur * exchangeRate;

        #endregion

        #region Validation & Business Logic

        /// <summary>
        /// Kiểm tra tuổi hợp lệ.
        /// </summary>
        public static bool IsValidAge(this int age) => age >= 0 && age <= 150;
        public static bool IsValidAge(this int? age) => age?.IsValidAge() ?? false;

        /// <summary>
        /// Kiểm tra phần trăm hợp lệ.
        /// </summary>
        public static bool IsValidPercentage(this double percentage) => percentage >= 0 && percentage <= 100;
        public static bool IsValidPercentage(this decimal percentage) => percentage >= 0 && percentage <= 100;
        public static bool IsValidPercentage(this double? percentage) => percentage?.IsValidPercentage() ?? false;
        public static bool IsValidPercentage(this decimal? percentage) => percentage?.IsValidPercentage() ?? false;

        /// <summary>
        /// Kiểm tra số lượng hợp lệ.
        /// </summary>
        public static bool IsValidQuantity(this int quantity) => quantity >= 0;
        public static bool IsValidQuantity(this decimal quantity) => quantity >= 0;
        public static bool IsValidQuantity(this int? quantity) => quantity?.IsValidQuantity() ?? false;
        public static bool IsValidQuantity(this decimal? quantity) => quantity?.IsValidQuantity() ?? false;

        /// <summary>
        /// Kiểm tra giá hợp lệ.
        /// </summary>
        public static bool IsValidPrice(this decimal price) => price >= 0;
        public static bool IsValidPrice(this double price) => price >= 0;
        public static bool IsValidPrice(this decimal? price) => price?.IsValidPrice() ?? false;
        public static bool IsValidPrice(this double? price) => price?.IsValidPrice() ?? false;

        /// <summary>
        /// Kiểm tra chiết khấu hợp lệ.
        /// </summary>
        public static bool IsValidDiscount(this decimal discount) => discount >= 0 && discount <= 100;
        public static bool IsValidDiscount(this double discount) => discount >= 0 && discount <= 100;
        public static bool IsValidDiscount(this decimal? discount) => discount?.IsValidDiscount() ?? false;
        public static bool IsValidDiscount(this double? discount) => discount?.IsValidDiscount() ?? false;

        /// <summary>
        /// Kiểm tra số điện thoại hợp lệ.
        /// </summary>
        public static bool IsValidPhoneNumber(this string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^(0|\+84)[0-9]{9,10}$");
        }

        /// <summary>
        /// Kiểm tra mã số thuế hợp lệ.
        /// </summary>
        public static bool IsValidTaxCode(this string taxCode)
        {
            if (string.IsNullOrWhiteSpace(taxCode)) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(taxCode, @"^[0-9]{10,13}$");
        }

        #endregion
        
        #region Type Conversions & Timespan

        /// <summary>
        /// Chuyển số thành TimeSpan (giây).
        /// </summary>
        public static TimeSpan Seconds(this int number) => TimeSpan.FromSeconds(number);
        public static TimeSpan Seconds(this double number) => TimeSpan.FromSeconds(number);
        public static TimeSpan? Seconds(this int? number) => number?.Seconds();
        public static TimeSpan? Seconds(this double? number) => number?.Seconds();

        /// <summary>
        /// Chuyển số thành TimeSpan (phút).
        /// </summary>
        public static TimeSpan Minutes(this int number) => TimeSpan.FromMinutes(number);
        public static TimeSpan Minutes(this double number) => TimeSpan.FromMinutes(number);
        public static TimeSpan? Minutes(this int? number) => number?.Minutes();
        public static TimeSpan? Minutes(this double? number) => number?.Minutes();

        /// <summary>
        /// Chuyển số thành TimeSpan (giờ).
        /// </summary>
        public static TimeSpan Hours(this int number) => TimeSpan.FromHours(number);
        public static TimeSpan Hours(this double number) => TimeSpan.FromHours(number);
        public static TimeSpan? Hours(this int? number) => number?.Hours();
        public static TimeSpan? Hours(this double? number) => number?.Hours();

        /// <summary>
        /// Chuyển số thành TimeSpan (ngày).
        /// </summary>
        public static TimeSpan Days(this int number) => TimeSpan.FromDays(number);
        public static TimeSpan Days(this double number) => TimeSpan.FromDays(number);
        public static TimeSpan? Days(this int? number) => number?.Days();
        public static TimeSpan? Days(this double? number) => number?.Days();

        /// <summary>
        /// Chuyển số thành TimeSpan (tuần).
        /// </summary>
        public static TimeSpan Weeks(this int number) => TimeSpan.FromDays(number * 7);
        public static TimeSpan Weeks(this double number) => TimeSpan.FromDays(number * 7);
        public static TimeSpan? Weeks(this int? number) => number?.Weeks();
        public static TimeSpan? Weeks(this double? number) => number?.Weeks();
        
        #endregion

        #region Nullable Safe Operations

        /// <summary>
        /// Lấy giá trị hoặc giá trị mặc định nếu null.
        /// </summary>
        public static int GetValueOrDefault(this int? number, int defaultValue = 0) => number ?? defaultValue;
        public static long GetValueOrDefault(this long? number, long defaultValue = 0) => number ?? defaultValue;
        public static short GetValueOrDefault(this short? number, short defaultValue = 0) => number ?? defaultValue;
        public static ushort GetValueOrDefault(this ushort? number, ushort defaultValue = 0) => number ?? defaultValue;
        public static uint GetValueOrDefault(this uint? number, uint defaultValue = 0) => number ?? defaultValue;
        public static ulong GetValueOrDefault(this ulong? number, ulong defaultValue = 0) => number ?? defaultValue;
        public static byte GetValueOrDefault(this byte? number, byte defaultValue = 0) => number ?? defaultValue;
        public static sbyte GetValueOrDefault(this sbyte? number, sbyte defaultValue = 0) => number ?? defaultValue;
        public static double GetValueOrDefault(this double? number, double defaultValue = 0) => number ?? defaultValue;
        public static decimal GetValueOrDefault(this decimal? number, decimal defaultValue = 0) => number ?? defaultValue;
        public static float GetValueOrDefault(this float? number, float defaultValue = 0) => number ?? defaultValue;

        /// <summary>
        /// Kiểm tra có giá trị hay không (không null và khác 0).
        /// </summary>
        public static bool HasValue(this int? number) => number.HasValue && number.Value != 0;
        public static bool HasValue(this long? number) => number.HasValue && number.Value != 0;
        public static bool HasValue(this short? number) => number.HasValue && number.Value != 0;
        public static bool HasValue(this ushort? number) => number.HasValue && number.Value != 0;
        public static bool HasValue(this uint? number) => number.HasValue && number.Value != 0;
        public static bool HasValue(this ulong? number) => number.HasValue && number.Value != 0;
        public static bool HasValue(this byte? number) => number.HasValue && number.Value != 0;
        public static bool HasValue(this sbyte? number) => number.HasValue && number.Value != 0;
        public static bool HasValue(this double? number) => number.HasValue && number.Value != 0;
        public static bool HasValue(this decimal? number) => number.HasValue && number.Value != 0;
        public static bool HasValue(this float? number) => number.HasValue && number.Value != 0;

        /// <summary>
        /// Chuyển đổi an toàn giữa các kiểu nullable.
        /// </summary>
        public static int? ToInt(this double? number) => number?.ToInt();
        public static int? ToInt(this decimal? number) => number?.ToInt();
        public static int? ToInt(this long? number) => number?.ToInt();
        public static int? ToInt(this short? number) => number?.ToInt();
        public static int? ToInt(this ushort? number) => number?.ToInt();
        public static int? ToInt(this uint? number) => number?.ToInt();
        public static int? ToInt(this ulong? number) => number?.ToInt();
        public static int? ToInt(this byte? number) => number?.ToInt();
        public static int? ToInt(this sbyte? number) => number?.ToInt();
        public static double? ToDouble(this int? number) => number?.ToDouble();
        public static double? ToDouble(this decimal? number) => number?.ToDouble();
        public static double? ToDouble(this long? number) => number?.ToDouble();
        public static double? ToDouble(this short? number) => number?.ToDouble();
        public static double? ToDouble(this ushort? number) => number?.ToDouble();
        public static double? ToDouble(this uint? number) => number?.ToDouble();
        public static double? ToDouble(this ulong? number) => number?.ToDouble();
        public static double? ToDouble(this byte? number) => number?.ToDouble();
        public static double? ToDouble(this sbyte? number) => number?.ToDouble();
        public static decimal? ToDecimal(this int? number) => number?.ToDecimal();
        public static decimal? ToDecimal(this double? number) => number?.ToDecimal();
        public static decimal? ToDecimal(this long? number) => number?.ToDecimal();
        public static decimal? ToDecimal(this short? number) => number?.ToDecimal();
        public static decimal? ToDecimal(this ushort? number) => number?.ToDecimal();
        public static decimal? ToDecimal(this uint? number) => number?.ToDecimal();
        public static decimal? ToDecimal(this ulong? number) => number?.ToDecimal();
        public static decimal? ToDecimal(this byte? number) => number?.ToDecimal();
        public static decimal? ToDecimal(this sbyte? number) => number?.ToDecimal();

        /// <summary>
        /// Chuyển đổi an toàn sang int.
        /// </summary>
        public static int ToInt(this double number) => Convert.ToInt32(number);
        public static int ToInt(this decimal number) => Convert.ToInt32(number);
        public static int ToInt(this long number) => Convert.ToInt32(number);
        public static int ToInt(this short number) => Convert.ToInt32(number);
        public static int ToInt(this ushort number) => Convert.ToInt32(number);
        public static int ToInt(this uint number) => Convert.ToInt32(number);
        public static int ToInt(this ulong number) => Convert.ToInt32(number);
        public static int ToInt(this byte number) => Convert.ToInt32(number);
        public static int ToInt(this sbyte number) => Convert.ToInt32(number);
        public static int ToInt(this float number) => Convert.ToInt32(number);
        public static double ToDouble(this int number) => Convert.ToDouble(number);
        public static double ToDouble(this decimal number) => Convert.ToDouble(number);
        public static double ToDouble(this long number) => Convert.ToDouble(number);
        public static double ToDouble(this short number) => Convert.ToDouble(number);
        public static double ToDouble(this ushort number) => Convert.ToDouble(number);
        public static double ToDouble(this uint number) => Convert.ToDouble(number);
        public static double ToDouble(this ulong number) => Convert.ToDouble(number);
        public static double ToDouble(this byte number) => Convert.ToDouble(number);
        public static double ToDouble(this sbyte number) => Convert.ToDouble(number);
        public static double ToDouble(this float number) => Convert.ToDouble(number);
        public static decimal ToDecimal(this int number) => Convert.ToDecimal(number);
        public static decimal ToDecimal(this double number) => Convert.ToDecimal(number);
        public static decimal ToDecimal(this long number) => Convert.ToDecimal(number);
        public static decimal ToDecimal(this short number) => Convert.ToDecimal(number);
        public static decimal ToDecimal(this ushort number) => Convert.ToDecimal(number);
        public static decimal ToDecimal(this uint number) => Convert.ToDecimal(number);
        public static decimal ToDecimal(this ulong number) => Convert.ToDecimal(number);
        public static decimal ToDecimal(this byte number) => Convert.ToDecimal(number);
        public static decimal ToDecimal(this sbyte number) => Convert.ToDecimal(number);
        public static decimal ToDecimal(this float number) => Convert.ToDecimal(number);

        #endregion

        #region Statistics & Analysis

        /// <summary>
        /// Tính trung bình của một mảng số.
        /// </summary>
        public static double Average(this int[] numbers) => numbers.Length > 0 ? numbers.Sum() / (double)numbers.Length : 0;
        public static double Average(this double[] numbers) => numbers.Length > 0 ? numbers.Sum() / numbers.Length : 0;
        public static decimal Average(this decimal[] numbers) => numbers.Length > 0 ? numbers.Sum() / numbers.Length : 0;

        /// <summary>
        /// Tính trung vị của một mảng số.
        /// </summary>
        public static double Median(this int[] numbers)
        {
            if (numbers.Length == 0) return 0;
            var sorted = numbers.OrderBy(x => x).ToArray();
            int mid = sorted.Length / 2;
            return sorted.Length % 2 == 0 ? (sorted[mid - 1] + sorted[mid]) / 2.0 : sorted[mid];
        }

        public static double Median(this double[] numbers)
        {
            if (numbers.Length == 0) return 0;
            var sorted = numbers.OrderBy(x => x).ToArray();
            int mid = sorted.Length / 2;
            return sorted.Length % 2 == 0 ? (sorted[mid - 1] + sorted[mid]) / 2.0 : sorted[mid];
        }

        public static decimal Median(this decimal[] numbers)
        {
            if (numbers.Length == 0) return 0;
            var sorted = numbers.OrderBy(x => x).ToArray();
            int mid = sorted.Length / 2;
            return sorted.Length % 2 == 0 ? (sorted[mid - 1] + sorted[mid]) / 2 : sorted[mid];
        }

        /// <summary>
        /// Tính độ lệch chuẩn của một mảng số.
        /// </summary>
        public static double StandardDeviation(this double[] numbers)
        {
            if (numbers.Length == 0) return 0;
            double mean = numbers.Average();
            double sumOfSquares = numbers.Sum(x => Math.Pow(x - mean, 2));
            return Math.Sqrt(sumOfSquares / numbers.Length);
        }

        public static double StandardDeviation(this int[] numbers)
        {
            return numbers.Select(x => (double)x).ToArray().StandardDeviation();
        }

        /// <summary>
        /// Tính phương sai của một mảng số.
        /// </summary>
        public static double Variance(this double[] numbers)
        {
            if (numbers.Length == 0) return 0;
            double mean = numbers.Average();
            return numbers.Sum(x => Math.Pow(x - mean, 2)) / numbers.Length;
        }

        public static double Variance(this int[] numbers)
        {
            return numbers.Select(x => (double)x).ToArray().Variance();
        }

        #endregion

        #region Obsolete/Placeholder - These can be removed or implemented
        
        /// <summary>
        /// Chuyển số sang chữ (placeholder).
        /// Cần logic chuyển số thành chữ tiếng Việt/Anh.
        /// Logic này đã được chuyển sang StringExtensions.ToWordsVN().
        /// </summary>
        [Obsolete("Sử dụng StringExtensions.ToWordsVN() để chuyển số sang chữ tiếng Việt.", true)]
        public static string ToWords(this int number)
        {
            return number.ToString();
        }

        /// <summary>
        /// Sinh số nguyên random trong khoảng [min, max).
        /// </summary>
        [Obsolete("Nên tạo một instance của Random và gọi trực tiếp. Extension trên Random không phải là cách tiếp cận phổ biến.", true)]
        public static int NextInt(this Random rand, int min, int max)
        {
            return rand.Next(min, max);
        }

        /// <summary>
        /// Kiểm tra số hoàn hảo.
        /// </summary>
        public static bool IsPerfectNumber(this int number)
        {
            if (number < 2) return false;
            int sum = 1;
            for (int i = 2; i <= number / 2; i++)
                if (number % i == 0) sum += i;
            return sum == number;
        }

        public static bool? IsPerfectNumber(this int? number) => number?.IsPerfectNumber();

        /// <summary>
        /// Kiểm tra số Fibonacci.
        /// </summary>
        public static bool IsFibonacci(this int number)
        {
            if (number < 0) return false;
            int a = 0, b = 1;
            while (b < number)
            {
                int temp = a + b;
                a = b;
                b = temp;
            }
            return number == 0 || b == number;
        }

        public static bool? IsFibonacci(this int? number) => number?.IsFibonacci();

        #endregion
    }
} 